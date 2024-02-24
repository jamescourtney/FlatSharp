/*
 * Copyright 2020 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/* 
This file includes sections modeled after the dotnet runtime project on github. The dotnet license file is included here:

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
*/

using System.Buffers;
using System.Linq;
using FlatSharp.Attributes;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

namespace FlatSharp;

/// <summary>
/// Helper methods for dealing with sorted vectors. This class provides functionality for both sorting vectors and
/// binary searching through them.
/// </summary>
public static class SortedVectorHelpers
{
    /// <summary>
    /// Performs a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
    /// </summary>
    /// <returns>A value if found, null otherwise.</returns>
    public static TTable? BinarySearchByFlatBufferKey<TTable, TKey>(this IList<TTable> sortedVector, TKey key)
        where TTable : class, ISortableTable<TKey>
        where TKey : notnull
    {
        CheckKeyNotNull(key);

        if (key is string str)
        {
            using SimpleStringComparer cmp = new SimpleStringComparer(str);

            return BinarySearchByFlatBufferKey<ListIndexable<TTable, string>, TTable, string, SimpleStringComparer>(
                new ListIndexable<TTable, string>(sortedVector),
                sortedVector,
                cmp);
        }
        else
        {
            return BinarySearchByFlatBufferKey<ListIndexable<TTable, TKey>, TTable, TKey, NaiveComparer<TKey>>(
                new ListIndexable<TTable, TKey>(sortedVector),
                sortedVector,
                new NaiveComparer<TKey>(key));
        }
    }

    /// <summary>
    /// Performs a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
    /// </summary>
    /// <returns>A value if found, null otherwise.</returns>
    public static TTable? BinarySearchByFlatBufferKey<TTable, TKey>(this IReadOnlyList<TTable> sortedVector, TKey key)
       where TTable : class, ISortableTable<TKey>
       where TKey : notnull
    {
        CheckKeyNotNull(key);

        if (key is string str)
        {
            using SimpleStringComparer cmp = new SimpleStringComparer(str);

            return BinarySearchByFlatBufferKey<ReadOnlyListIndexable<TTable, string>, TTable, string, SimpleStringComparer>(
                new ReadOnlyListIndexable<TTable, string>(sortedVector),
                sortedVector,
                cmp);
        }
        else
        {
            return BinarySearchByFlatBufferKey<ReadOnlyListIndexable<TTable, TKey>, TTable, TKey, NaiveComparer<TKey>>(
                new ReadOnlyListIndexable<TTable, TKey>(sortedVector),
                sortedVector,
                new NaiveComparer<TKey>(key));
        }
    }

    public static void RegisterKeyLookup<TTable, TKey>(Func<TTable, TKey> keyGetter, ushort keyIndex)
    {
        KeyLookup<TTable, TKey>.KeyGetter = keyGetter;
        KeyLookup<TTable, TKey>.KeyIndex = keyIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CheckKeyNotNull<TKey>(TKey key)
    {
        if (key is null)
        {
            FSThrow.ArgumentNull(nameof(key));
        }
    }

    private static TTable? BinarySearchByFlatBufferKey<TIndexable, TTable, TKey, TComparer>(
        TIndexable indexable,
        object realVector,
        TComparer comparer)
        where TIndexable : struct, IIndexable<TTable, TKey>
        where TTable : class
        where TComparer : struct, ISimpleComparer<TKey>
    {
        // String searches take two forms:
        // For greedy deserialized buffers, we don't have the raw bytes, so we search inefficiently. This involves
        // a string -> byte[] copy for each binary search jump.
        // For lazy buffers (this first case), we can interrogate the underlying buffer directly.
        if (typeof(TKey) == typeof(string) &&
            realVector is IFlatBufferDeserializedVector vector &&
            vector.ItemSize == sizeof(int) &&
            comparer is SimpleStringComparer ssc)
        {
            ushort keyIndex = KeyLookup<TTable, string>.KeyIndex;

            return GenericBinarySearch<RawIndexableVector<TTable>, TTable, ReadOnlyMemory<byte>, SimpleStringComparer>(
                new RawIndexableVector<TTable>(vector, keyIndex),
                ssc);
        }
        else
        {
            return GenericBinarySearch<TIndexable, TTable, TKey, TComparer>(indexable, comparer);
        }
    }

    private static TTable? GenericBinarySearch<TVector, TTable, TKey, TComparer>(
        TVector vector,
        TComparer comparer)
        where TVector : struct, IIndexable<TTable, TKey>
        where TComparer : struct, ISimpleComparer<TKey>
        where TTable : class
    {
        int min = 0;
        int max = vector.Count - 1;

        while (min <= max)
        {
            // (min + max) / 2, written to avoid overflows.
            int mid = min + ((max - min) >> 1);

            TKey midKey = vector.KeyAt(mid);
            int comparison = comparer.CompareTo(midKey);

            if (comparison == 0)
            {
                return vector[mid];
            }

            if (comparison < 0)
            {
                min = mid + 1;
            }
            else
            {
                max = mid - 1;
            }
        }

        return null;
    }

    /// <summary>
    /// Holds lookup information for keys. Faster than dictionary.
    /// </summary>
    internal static class KeyLookup<TTable, TKey>
    {
        static KeyLookup()
        {
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
            // Convention is for static constructors in the table to register key lookups. Force them to run here before fields
            // are accessed.
            if (RuntimeFeature.IsDynamicCodeSupported) // this should be true for all cases except native AOT. This does not need to run for NativeAOT since static constructors are pre-executed.
            {
#pragma warning disable IL2059
                RuntimeHelpers.RunClassConstructor(typeof(TTable).TypeHandle);
#pragma warning restore IL2059
            }
#else
            RuntimeHelpers.RunClassConstructor(typeof(TTable).TypeHandle);
#endif
        }

        private static string NotInitializedErrorMessage = $"Type '{typeof(TTable).Name}' has not registered a sorted vector key of type '{typeof(TKey).Name}'.";

        private static Func<TTable, TKey>? getter;
        private static ushort index;

        public static Func<TTable, TKey> KeyGetter
        {
            get
            {
                EnsureInitialized();
                return getter;
            }

            set
            {
                getter = value;
            }
        }

        public static ushort KeyIndex
        {
            get
            {
                EnsureInitialized();
                return index;
            }

            set
            {
                index = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET6_0_OR_GREATER
        [MemberNotNull("getter")]
#endif
        [ExcludeFromCodeCoverage]
        private static void EnsureInitialized()
        {
            if (getter is null)
            {
                FSThrow.InvalidOperation(NotInitializedErrorMessage);
            }
        }
    }

    /// <summary>
    /// Helper interface to express something with an indexer.
    /// </summary>
    private interface IIndexable<T, TKey>
    {
        int Count { get; }

        T this[int index] { get; }

        TKey KeyAt(int index);
    }

    private struct ListIndexable<T, TKey> : IIndexable<T, TKey>
    {
        private readonly IList<T> items;

        public ListIndexable(IList<T> items)
        {
            this.items = items;
        }

        public T this[int index] => this.items[index];

        public TKey KeyAt(int index) => KeyLookup<T, TKey>.KeyGetter(this[index]);

        public int Count => this.items.Count;
    }

    private struct ReadOnlyListIndexable<T, TKey> : IIndexable<T, TKey>
    {
        private readonly IReadOnlyList<T> items;

        public ReadOnlyListIndexable(IReadOnlyList<T> items)
        {
            this.items = items;
        }

        public T this[int index] => this.items[index];

        public TKey KeyAt(int index) => KeyLookup<T, TKey>.KeyGetter(this[index]);

        public int Count => this.items.Count;
    }

    private struct RawIndexableVector<TTable> : IIndexable<TTable, ReadOnlyMemory<byte>>
    {
        private readonly IFlatBufferDeserializedVector vector;

        // Save the input buffer here. Since input buffers are usually structs, we can only box it once if we save it here. Otherwise,
        // we'll have to box each time we access it, which causes lots of allocations.
        private readonly IInputBuffer inputBuffer;
        private readonly ushort keyIndex;

        public RawIndexableVector(IFlatBufferDeserializedVector vector, ushort keyIndex)
        {
            this.vector = vector;
            this.inputBuffer = vector.InputBuffer;
            this.keyIndex = keyIndex;
        }

        public TTable this[int index] => (TTable)this.vector.ItemAt(index);

        public ReadOnlyMemory<byte> KeyAt(int index)
        {
            IFlatBufferDeserializedVector vector = this.vector;
            IInputBuffer buffer = this.inputBuffer;

            // Read uoffset.
            int offset = vector.OffsetOf(index);

            // Increment uoffset to move to start of table.
            offset += buffer.ReadUOffset(offset);

            // Follow soffset to start of vtable.
            int vtableStart = offset - buffer.ReadInt(offset);

            ushort vtableLength = buffer.ReadUShort(vtableStart);
            int tableOffset = 0;
            int keyFieldOffset = 4 + checked(2 * this.keyIndex);

            if (keyFieldOffset + sizeof(ushort) <= vtableLength)
            {
                tableOffset = buffer.ReadUShort(vtableStart + keyFieldOffset);
            }

            if (tableOffset == 0)
            {
                return FSThrow.InvalidOperation<ReadOnlyMemory<byte>>("Sorted FlatBuffer vectors may not have null-valued keys.");
            }

            offset += tableOffset;

            // Start of the string.
            offset += buffer.ReadUOffset(offset);

            // Length of the string.
            int stringLength = (int)buffer.ReadUInt(offset);

            return buffer.GetReadOnlyMemory().Slice(offset + sizeof(int), stringLength);
        }

        public int Count => this.vector.Count;
    }

    private interface ISimpleComparer<T> : IDisposable
    {
        int CompareTo(T right);
    }

    private struct NaiveComparer<T> : ISimpleComparer<T>
    {
        private readonly T right;

        public NaiveComparer(T right)
        {
            // Enforce generic constraints we can't express otherwise.
            FlatSharpInternal.Assert(typeof(T) != typeof(string), "Naive comparer doesn't work for strings");
            FlatSharpInternal.Assert(typeof(T).IsValueType, "Naive comparer only works for value types");

            this.right = right;
        }

        public int CompareTo(T left)
        {
            return Comparer<T>.Default.Compare(left, this.right);
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            // No-op.
        }
    }

    private struct SimpleStringComparer : ISimpleComparer<string>, ISimpleComparer<ReadOnlyMemory<byte>>
    {
        private readonly byte[] pooledArray;
        private readonly int length;

        public SimpleStringComparer(string right)
        {
            var enc = SerializationHelpers.Encoding;
            int maxLength = enc.GetMaxByteCount(right.Length);

            this.pooledArray = ArrayPool<byte>.Shared.Rent(maxLength);
            this.length = enc.GetBytes(right, 0, right.Length, this.pooledArray, 0);
        }

        public int CompareTo(string left)
        {
            FlatSharpInternal.Assert(left is not null, "Sorted FlatBuffer vectors may not have null-valued keys.");

            var enc = SerializationHelpers.Encoding;
            int comp;
            byte[]? temp = null;
            int maxLength = enc.GetMaxByteCount(left.Length);

#if NETSTANDARD2_0
            temp = ArrayPool<byte>.Shared.Rent(maxLength);
            int tempLength = enc.GetBytes(left, 0, left.Length, temp, 0);
            Span<byte> leftSpan = temp.AsSpan().Slice(0, tempLength);
#else
            Span<byte> leftSpan = maxLength < 1024 ? stackalloc byte[maxLength] : (temp = ArrayPool<byte>.Shared.Rent(maxLength));
            int leftLength = enc.GetBytes(left, leftSpan);
            leftSpan = leftSpan.Slice(0, leftLength);
#endif

            comp = StringSpanComparer.Instance.Compare(true, leftSpan, true, this.pooledArray.AsSpan().Slice(0, this.length));

            if (temp is not null)
            {
                ArrayPool<byte>.Shared.Return(temp);
            }

            return comp;
        }

        public int CompareTo(ReadOnlyMemory<byte> left)
        {
            return StringSpanComparer.Instance.Compare(true, left.Span, true, this.pooledArray.AsSpan().Slice(0, this.length));
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(this.pooledArray);
        }
    }
}
