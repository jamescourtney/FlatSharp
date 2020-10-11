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

namespace FlatSharp
{
    using System;
    using System.Buffers.Binary;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using FlatSharp.Attributes;

#nullable enable

    /// <summary>
    /// Helper methods for dealing with sorted vectors. This class provides functionality for both sorting vectors and
    /// binary searching through them.
    /// </summary>
    public static class SortedVectorHelpers
    {
        /// <summary>
        /// Sorts the given flatbuffer vector. This method, used incorrectly, is a fantastic way to corrupt your buffer.
        /// </summary>
        /// <remarks>
        /// This method assumes that all vector members are tables, and each table has a defined key. Our vector looks like this:
        /// [length] [ uoffset to first table ] [ uoffset 2 ] ... [ uoffset N ]
        /// 
        /// Prior to sorting, we iterate through the vector and populate an array with tuples of (absolute key offset, key length, absolute table offset).
        /// Then, we sort that array. This saves us from having to follow unnecessary indirections from vector -> table -> vtable -> key.
        /// 
        /// After we sort our array of tuples, we go back and overwrite the vectors with the updated uoffsets, which need to be adjusted relative to the
        /// new position within the vector.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SortVector<TSpanComparer>(
            Span<byte> buffer,
            int vectorUOffset,
            int vtableIndex,
            int? keyInlineSize,
            TSpanComparer comparer) where TSpanComparer : ISpanComparer
        {
            checked
            {
                int vectorStartOffset = vectorUOffset + (int)ScalarSpanReader.ReadUInt(buffer.Slice(vectorUOffset));
                int vectorLength = (int)ScalarSpanReader.ReadUInt(buffer.Slice(vectorStartOffset));
                int index0Position = vectorStartOffset + sizeof(int);

                // Traverse the vector and figure out the offsets of all the keys.
                // Store that in some local data, hopefully on the stack. 512 is somewhat arbitrary, but we want to avoid stack overflows.
                Span<(int offset, int length, int tableOffset)> keyOffsets =
                    vectorLength < 512
                    ? stackalloc (int, int, int)[vectorLength]
                    : new (int, int, int)[vectorLength];

                for (int i = 0; i < keyOffsets.Length; ++i)
                {
                    keyOffsets[i] = GetKeyOffset(buffer, index0Position, i, vtableIndex, keyInlineSize);
                }

                // Sort the offsets.
                IntroSort(buffer, comparer, 0, vectorLength - 1, keyOffsets);

                // Overwrite the vector with the sorted offsets. Bound the vector so we're confident we aren't 
                // partying inappropriately in the rest of the buffer.
                Span<byte> boundedVector = buffer.Slice(index0Position, sizeof(uint) * vectorLength);
                int nextPosition = index0Position;
                for (int i = 0; i < keyOffsets.Length; ++i)
                {
                    (_, _, int tableOffset) = keyOffsets[i];
                    BinaryPrimitives.WriteUInt32LittleEndian(boundedVector.Slice(sizeof(uint) * i), (uint)(tableOffset - nextPosition));
                    nextPosition += sizeof(uint);
                }
            }
        }

        /// <summary>
        /// Performs a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable? BinarySearchByFlatBufferKey<TTable, TKey>(this IList<TTable> sortedVector, TKey key)
            where TTable : class
        {
            return GenericBinarySearch(sortedVector.Count, i => sortedVector[i], key);
        }

        /// <summary>
        /// Performs a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable? BinarySearchByFlatBufferKey<TTable, TKey>(this TTable[] sortedVector, TKey key)
            where TTable : class
        {
            return GenericBinarySearch(sortedVector.Length, i => sortedVector[i], key);
        }

        /// <summary>
        /// Performs a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable? BinarySearchByFlatBufferKey<TTable, TKey>(this IReadOnlyList<TTable> sortedVector, TKey key)
           where TTable : class
        {
            return GenericBinarySearch(sortedVector.Count, i => sortedVector[i], key);
        }

        private static Func<T, int> GetComparerFunc<T>(T comparison)
        {
            if (typeof(T) == typeof(string))
            {
                return (Func<T, int>)(object)GetStringComparerFunc(comparison as string);
            }
            else if (typeof(T) == typeof(SharedString))
            {
                return (Func<T, int>)(object)GetSharedStringComparerFunc(comparison as SharedString);
            }
            else
            {
                IComparer<T> comparer = Comparer<T>.Default;
                return left => comparer.Compare(left, comparison);
            }
        }

        private static Func<SharedString, int> GetSharedStringComparerFunc(SharedString? right)
        {
            Func<string?, int> nonSharedCallback = GetStringComparerFunc(right?.String);
            return ss =>
            {
                return nonSharedCallback(ss?.String);
            };
        }

        private static Func<string?, int> GetStringComparerFunc(string? right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("key");
            }

            byte[] rightData = SerializationHelpers.Encoding.GetBytes(right);
            return left =>
            {
                if (left == null)
                {
                    throw new InvalidOperationException("Sorted FlatBuffer vectors may not have null-valued keys.");
                }

                var enc = SerializationHelpers.Encoding;
                int maxLength = enc.GetMaxByteCount(left.Length);

#if NETSTANDARD
               byte[] leftBytes = enc.GetBytes(left);
               int leftLength = leftBytes.Length;
               Span<byte> leftSpan = leftBytes;
#else
               Span<byte> leftSpan = maxLength < 1024 ? stackalloc byte[maxLength] : new byte[maxLength];
               int leftLength = enc.GetBytes(left, leftSpan);
               leftSpan = leftSpan.Slice(0, leftLength);
#endif

               return StringSpanComparer.Instance.Compare(true, leftSpan, true, rightData);
            };
        }

        /// <summary>
        /// Cache TTable -> (TKey, Func{TTable, TKey})
        /// </summary>
        private static ConcurrentDictionary<Type, (Type, object)> KeyGetterCallbacks = new ConcurrentDictionary<Type, (Type, object)>();

        /// <summary>
        /// Reflects on TTable to return a delegate that accesses the flat buffer key property. The type of the property must precisely match TKey.
        /// </summary>
        internal static Func<TTable, TKey> GetOrCreateGetKeyCallback<TTable, TKey>()
        {
            if (!KeyGetterCallbacks.TryGetValue(typeof(TTable), out (Type, object) value))
            {
                var keys = typeof(TTable)
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<FlatBufferItemAttribute>()?.Key == true)
                    .ToArray();

                if (keys.Length == 0)
                {
                    throw new InvalidOperationException($"Table '{typeof(TTable).Name}' does not declare a property with the Key attribute set.");
                }
                else if (keys.Length > 1)
                {
                    throw new InvalidOperationException($"Table '{typeof(TTable).Name}' declares more than one property with the Key attribute set.");
                }

                PropertyInfo keyProperty = keys[0];
                var keyGetterDelegate = (Func<TTable, TKey>)Delegate.CreateDelegate(typeof(Func<TTable, TKey>), keyProperty.GetMethod);
                value = (keyProperty.PropertyType, keyGetterDelegate);
                KeyGetterCallbacks[typeof(TTable)] = value;
            }

            if (value.Item2 is Func<TTable, TKey> callback)
            {
                return callback;
            }

            throw new InvalidOperationException($"Key type was: '{typeof(TKey)}', but the key for table '{typeof(TTable).Name}' is of type '{value.Item1.Name}'.");
        }

        private static TTable? GenericBinarySearch<TTable, TKey>(int count, Func<int, TTable> itemAtIndex, TKey key)
            where TTable : class
        {
            Func<TKey, int> compare = GetComparerFunc<TKey>(key);
            Func<TTable, TKey> keyGetter = GetOrCreateGetKeyCallback<TTable, TKey>();

            int min = 0;
            int max = count - 1;

            while (min <= max)
            {
                // (min + max) / 2, written to avoid overflows.
                int mid = min + ((max - min) >> 1);

                var midElement = itemAtIndex(mid);
                int comparison = compare(keyGetter(midElement));

                if (comparison == 0)
                {
                    return midElement;
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
        /// A partial introsort implementation, inspired by the Array.Sort implemenation from the CoreCLR. 
        /// Due to the amount of indirection in FlatBuffers, it's not possible to use the built-in sorting algorithms,
        /// so we do the next best thing. Note that this is not a true IntroSort, since we omit the HeapSort component.
        /// </summary>
        private static void IntroSort<TSpanComparer>(
            ReadOnlySpan<byte> buffer,
            TSpanComparer keyComparer,
            int lo,
            int hi,
            Span<(int offset, int length, int tableOffset)> keyLocations) where TSpanComparer : ISpanComparer
        {
            checked
            {
                while (true)
                {
                    if (hi <= lo)
                    {
                        break;
                    }

                    int numElements = hi - lo + 1;
                    if (numElements <= 16)
                    {
                        switch (numElements)
                        {
                            case 1:
                                return;
                            case 2:
                                SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                                return;
                            case 3:
                                SwapIfGreater(buffer, keyComparer, lo, hi - 1, keyLocations);
                                SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                                SwapIfGreater(buffer, keyComparer, hi - 1, hi, keyLocations);
                                return;
                            default:
                                InsertionSort(buffer, keyComparer, lo, hi, keyLocations);
                                return;
                        }
                    }

                    // Use median-of-three partitioning.
                    int middle = lo + ((hi - lo) >> 1);
                    {
                        SwapIfGreater(buffer, keyComparer, lo, middle, keyLocations);
                        SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                        SwapIfGreater(buffer, keyComparer, middle, hi, keyLocations);
                    }

                    // Move the pivot to hi - 1 (since we know hi is already larger than the pivot).
                    SwapVectorPositions(middle, hi - 1, keyLocations);
                    var (pivotOffset, pivotLength, _) = keyLocations[hi - 1];
                    bool pivotExists = pivotOffset != 0;
                    var pivotSpan = buffer.Slice(pivotOffset, pivotLength);

                    // Partition
                    int num2 = lo;
                    int num3 = hi - 1;
                    while (num2 < num3)
                    {
                        while (true)
                        {
                            var (keyOffset, keyLength, _) = keyLocations[++num2];
                            var keySpan = buffer.Slice(keyOffset, keyLength);
                            if (keyComparer.Compare(keyOffset != 0, keySpan, pivotExists, pivotSpan) >= 0)
                            {
                                break;
                            }
                        }

                        while (true)
                        {
                            var (keyOffset, keyLength, _) = keyLocations[--num3];
                            var keySpan = buffer.Slice(keyOffset, keyLength);
                            if (keyComparer.Compare(pivotExists, pivotSpan, keyOffset != 0, keySpan) >= 0)
                            {
                                break;
                            }
                        }

                        if (num2 < num3)
                        {
                            SwapVectorPositions(num2, num3, keyLocations);
                        }
                    }

                    SwapVectorPositions(num2, hi - 1, keyLocations);

                    IntroSort(
                        buffer,
                        keyComparer,
                        num2 + 1,
                        hi,
                        keyLocations);

                    hi = num2 - 1;
                }
            }
        }

        private static void InsertionSort<TSpanComparer>(
            ReadOnlySpan<byte> buffer,
            TSpanComparer comparer,
            int lo,
            int hi,
            Span<(int offset, int length, int tableOffset)> keyLocations) where TSpanComparer : ISpanComparer
        {
            for (int i = lo; i < hi; i++)
            {
                int num = i;

                var valTuple = keyLocations[i + 1];
                ReadOnlySpan<byte> valSpan = buffer.Slice(valTuple.offset, valTuple.length);

                while (num >= lo)
                {
                    (int keyOffset, int keyLength, _) = keyLocations[num];
                    ReadOnlySpan<byte> keySpan = buffer.Slice(keyOffset, keyLength);

                    if (comparer.Compare(valTuple.offset != 0, valSpan, keyOffset != 0, keySpan) < 0)
                    {
                        keyLocations[num + 1] = keyLocations[num];
                        num--;
                    }
                    else
                    {
                        break;
                    }
                }

                keyLocations[num + 1] = valTuple;
            }
        }

        private static void SwapIfGreater<TSpanComparer>(
            ReadOnlySpan<byte> vector,
            TSpanComparer comparer,
            int leftIndex,
            int rightIndex,
            Span<(int, int, int)> keyOffsets) where TSpanComparer : ISpanComparer
        {
            if (leftIndex != rightIndex)
            {
                (int leftOffset, int leftLength, _) = keyOffsets[leftIndex];
                (int rightOffset, int rightLength, _) = keyOffsets[rightIndex];

                bool leftExists = leftOffset != 0;
                bool rightExists = rightOffset != 0;

                var leftSpan = vector.Slice(leftOffset, leftLength);
                var rightSpan = vector.Slice(rightOffset, rightLength);

                if (comparer.Compare(leftExists, leftSpan, rightExists, rightSpan) > 0)
                {
                    SwapVectorPositions(leftIndex, rightIndex, keyOffsets);
                }
            }
        }

        private static void SwapVectorPositions(int leftIndex, int rightIndex, Span<(int, int, int)> keyOffsets)
        {
            checked
            {
                if (leftIndex == rightIndex)
                {
                    return;
                }

                var temp = keyOffsets[leftIndex];
                keyOffsets[leftIndex] = keyOffsets[rightIndex];
                keyOffsets[rightIndex] = temp;
            }
        }

        /// <summary>
        /// For the given index in a vector, follow the indirection to return a tuple representing
        /// the key start offset, the key length, and the table offset. It's advantageous to return the
        /// tuple here since we can store that in a span.
        /// </summary>
        private static (int offset, int length, int tableOffset) GetKeyOffset(
            ReadOnlySpan<byte> buffer,
            int index0Position,
            int vectorIndex,
            int vtableIndex,
            int? inlineItemSize)
        {
            checked
            {
                // Find offset to the table at the index.
                int tableOffset = index0Position + (sizeof(uint) * vectorIndex);
                tableOffset += (int)ScalarSpanReader.ReadUInt(buffer.Slice(tableOffset));

                // Consult the vtable.
                int vtableOffset = tableOffset - ScalarSpanReader.ReadInt(buffer.Slice(tableOffset));

                // Vtables have two extra entries: vtable length and table length. The number of entries is vtableLengthBytes / 2 - 2
                int vtableLengthEntries = (ScalarSpanReader.ReadUShort(buffer.Slice(vtableOffset)) / 2) - 2;

                if (vtableIndex >= vtableLengthEntries)
                {
                    return (0, 0, tableOffset);
                }

                // Absolute offset of the field within the table.
                int fieldOffset = tableOffset + ScalarSpanReader.ReadUShort(buffer.Slice(vtableOffset + 2 * (2 + vtableIndex)));
                if (inlineItemSize != null)
                {
                    return (fieldOffset, inlineItemSize.Value, tableOffset);
                }

                if (fieldOffset == 0)
                {
                    return (0, 0, tableOffset);
                }

                // Strings are stored as a uoffset reference. Follow the indirection one more time.
                int uoffsetToString = fieldOffset + (int)ScalarSpanReader.ReadUInt(buffer.Slice(fieldOffset));
                int stringLength = (int)ScalarSpanReader.ReadUInt(buffer.Slice(uoffsetToString));
                return (uoffsetToString + sizeof(uint), stringLength, tableOffset);
            }
        }

    }
}
