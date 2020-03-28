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

namespace FlatSharp
{
    using FlatSharp.Attributes;
    using System;
    using System.Buffers.Binary;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Serializer extension methods.
    /// </summary>
    public static class SortedVectorHelpers
    {
        /// <summary>
        /// Gets the flatbuffer comparer for the given type.
        /// </summary>
        public static IComparer<T> GetComparer<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (IComparer<T>)FlatBufferStringComparer.Instance;
            }

            return Comparer<T>.Default;
        }

        /// <summary>
        /// Sorts the given vector with keys of type TKey.
        /// </summary>
        public static void SortVector<TKey>(
            Span<byte> buffer, 
            int vectorUOffset, 
            int vtableIndex,
            TKey defaultValue,
            int keyInlineSize,
            StructSpanComparer<TKey>.StructSpanComparerReader reader)
            where TKey : struct, IComparable<TKey>
        {
            ISpanComparer comparer = new StructSpanComparer<TKey>(reader, defaultValue);
            SortVector(buffer, comparer, vectorUOffset, keyInlineSize, vtableIndex);
        }

        /// <summary>
        /// Sorts the given vector with keys of type String.
        /// </summary>
        public static void SortStringVector(Span<byte> buffer, int vectorUOffset, int vtableIndex)
        {
            ISpanComparer comparer = FlatBufferStringComparer.Instance;
            SortVector(buffer, comparer, vectorUOffset, null, vtableIndex);
        }

        private static void SortVector(
            Span<byte> buffer, 
            ISpanComparer keyComparer, 
            int vectorUOffset, 
            int? keyInlineSize,
            int vtableIndex)
        {
            checked
            {
                int vectorStartOffset = vectorUOffset + (int)ScalarSpanReader.ReadUInt(buffer, vectorUOffset);
                int vectorLength = (int)ScalarSpanReader.ReadUInt(buffer, vectorStartOffset);

                int index0Position = vectorStartOffset + sizeof(int);

                SortVectorRecursive(buffer, keyComparer, index0Position, vectorLength, keyInlineSize, vtableIndex);
            }
        }

        /// <summary>
        /// Quicksort on hard mode.
        /// </summary>
        private static void SortVectorRecursive(
            Span<byte> buffer,
            ISpanComparer keyComparer,
            int index0Offset,
            int vectorLength,
            int? inlineKeySize,
            int vtableIndex)
        {
            checked
            {
                if (vectorLength <= 1)
                {
                    return;
                }

                int pivotIndex = vectorLength - 1;
                var pivot = GetKeyOffset(buffer, index0Offset, pivotIndex, vtableIndex, inlineKeySize);

                int i = 0;
                for (int j = 0; j < vectorLength - 1; ++j)
                {
                    var currentItem = GetKeyOffset(buffer, index0Offset, j, vtableIndex, inlineKeySize);

                    if (keyComparer.Compare(buffer, currentItem.offset, currentItem.length, buffer, pivot.offset, pivot.length) < 0)
                    {
                        SwapVectorPositions(buffer, index0Offset, i, j);
                        i++;
                    }
                }

                SwapVectorPositions(buffer, index0Offset, i, pivotIndex);
                pivotIndex = i;

                SortVectorRecursive(buffer, keyComparer, index0Offset, pivotIndex, inlineKeySize, vtableIndex);
                SortVectorRecursive(buffer, keyComparer, index0Offset + (sizeof(int) * (pivotIndex + 1)), vectorLength - (pivotIndex + 1), inlineKeySize, vtableIndex);
            }
        }

        /// <summary>
        /// Swaps the two given indices in the vector, adjusting the UOffsets to compensate for the swap.
        /// </summary>
        private static void SwapVectorPositions(Span<byte> vector, int index0Offset, int leftIndex, int rightIndex)
        {
            checked
            {
                if (leftIndex == rightIndex)
                {
                    return;
                }

                if (leftIndex > rightIndex)
                {
                    // Left is always less than right. 
                    SwapVectorPositions(vector, index0Offset, rightIndex, leftIndex);
                    return;
                }

                int difference = (rightIndex - leftIndex) * sizeof(int);

                Span<byte> leftSpan = vector.Slice(index0Offset + (sizeof(int) * leftIndex));
                Span<byte> rightSpan = vector.Slice(index0Offset + (sizeof(int) * rightIndex));

                int leftValue = (int)ScalarSpanReader.ReadUInt(leftSpan, 0);
                int rightValue = (int)ScalarSpanReader.ReadUInt(rightSpan, 0);

                leftValue -= difference;
                rightValue += difference;

                BinaryPrimitives.WriteUInt32LittleEndian(leftSpan, (uint)rightValue);
                BinaryPrimitives.WriteUInt32LittleEndian(rightSpan, (uint)leftValue);
            }
        }

        /// <summary>
        /// For the given index in a vector, follow the indirection to return a span representing
        /// the key for that table.
        /// </summary>
        private static (int offset, int length) GetKeyOffset(
            Span<byte> buffer, 
            int index0Position, 
            int vectorIndex, 
            int vtableIndex,
            int? inlineItemSize)
        {
            checked 
            {
                // Find offset to the table at the index.
                int tableOffset = index0Position + (sizeof(uint) * vectorIndex);
                tableOffset += (int)ScalarSpanReader.ReadUInt(buffer, tableOffset);

                // Consult the vtable.
                int vtableOffset = tableOffset - ScalarSpanReader.ReadInt(buffer, tableOffset);

                // Vtables have two extra entries: vtable length and table length. The number of entries is vtableLengthBytes / 2 - 2
                int vtableLengthEntries = (ScalarSpanReader.ReadUShort(buffer, vtableOffset) / 2) - 2;

                if (vtableIndex >= vtableLengthEntries)
                {
                    // Undefined.
                    return (-1, -1);
                }

                // Absolute offset of the field within the table.
                int fieldOffset = tableOffset + ScalarSpanReader.ReadUShort(buffer, vtableOffset + 2 * (2 + vtableIndex));
                if (inlineItemSize != null)
                {
                    return (fieldOffset, inlineItemSize.Value);
                }

                // Strings are stored as a uoffset reference. Follow the indirection one more time.
                int uoffsetToString = fieldOffset + (int)ScalarSpanReader.ReadUInt(buffer, fieldOffset);
                int stringLength = (int)ScalarSpanReader.ReadUInt(buffer, uoffsetToString);
                return (uoffsetToString + sizeof(uint), stringLength);
            }
        }

        /// <summary>
        /// Perfors a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable BinarySearchByFlatBufferKey<TTable, TKey>(this IList<TTable> sortedVector, TKey key)
            where TTable : class
        {
            return GenericBinarySearch(sortedVector.Count, i => sortedVector[i], key);
        }

        /// <summary>
        /// Perfors a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable BinarySearchByFlatBufferKey<TTable, TKey>(this IReadOnlyList<TTable> sortedVector, TKey key)
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
            else
            {
                IComparer<T> comparer = Comparer<T>.Default;
                return left => comparer.Compare(left, comparison);
            }
        }

        private static Func<string, int> GetStringComparerFunc(string right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("key");
            }

            byte[] rightData = InputBuffer.Encoding.GetBytes(right);
            return left =>
            {
                if (left == null)
                {
                    throw new InvalidOperationException("Sorted FlatBuffer vectors may not have null-valued keys.");
                }

                var enc = InputBuffer.Encoding;
                int maxLength = enc.GetMaxByteCount(left.Length);

#if NETSTANDARD
                byte[] leftBytes = enc.GetBytes(left);
                int leftLength = leftBytes.Length;
                Span<byte> leftSpan = leftBytes;
#else
                Span<byte> leftSpan = stackalloc byte[maxLength];
                int leftLength = enc.GetBytes(left, leftSpan);
                leftSpan = leftSpan.Slice(0, leftLength);
#endif

                return FlatBufferStringComparer.Instance.Compare(leftSpan, 0, leftSpan.Length, rightData, 0, rightData.Length);
            };
        }

        /// <summary>
        /// Cache TTable -> (TKey, Func{TTable, TKey})
        /// </summary>
        private static ConcurrentDictionary<Type, (Type, object)> KeyGetterCallbacks = new ConcurrentDictionary<Type, (Type, object)>();

        /// <summary>
        /// Reflects on TTable to return a delegate that accesses the flat buffer key property. The type of the property must precisely match TKey.
        /// </summary>
        private static Func<TTable, TKey> GetOrCreateGetKeyCallback<TTable, TKey>(TKey key)
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
                var keyGetterDelegate = (Func<TTable, TKey>)Delegate.CreateDelegate(typeof(Func<TTable, TKey>), keyProperty.GetMethod ?? keyProperty.GetGetMethod());
                value = (keyProperty.PropertyType, keyGetterDelegate);
                KeyGetterCallbacks[typeof(TTable)] = value;
            }

            if (key.GetType() != value.Item1)
            {
                throw new InvalidOperationException($"Key '{key}' had type '{key?.GetType()}', but the key for table '{typeof(TTable).Name}' is of type '{value.Item1.Name}'.");
            }

            return (Func<TTable, TKey>)value.Item2;
        }

        private static TTable GenericBinarySearch<TTable, TKey>(int count, Func<int, TTable> itemAtIndex, TKey key)
            where TTable : class
        {
            Func<TKey, int> compare = GetComparerFunc<TKey>(key);
            Func<TTable, TKey> keyGetter = GetOrCreateGetKeyCallback<TTable, TKey>(key);

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
    }
}
