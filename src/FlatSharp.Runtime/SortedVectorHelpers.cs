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
    using System;
    using System.Collections.Generic;

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
        /// Perfors a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable BinarySearchByFlatBufferKey<TTable, TKey>(this IList<TTable> sortedVector, TKey key)
            where TTable : class, IKeyedTable<TKey>
        {
            return BinarySearch(sortedVector.Count, key, i => sortedVector[i]);
        }

        /// <summary>
        /// Perfors a binary search on the given sorted vector with the given key. The vector is presumed to be sorted.
        /// </summary>
        /// <returns>A value if found, null otherwise.</returns>
        public static TTable BinarySearchByFlatBufferKey<TTable, TKey>(this IReadOnlyList<TTable> sortedVector, TKey key)
            where TTable : class, IKeyedTable<TKey>
        {
            return BinarySearch(sortedVector.Count, key, i => sortedVector[i]);
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
            byte[] rightData = InputBuffer.Encoding.GetBytes(right);
            return left =>
            {
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

                return FlatBufferStringComparer.CompareSpans(leftSpan, rightData);
            };
        }

        private static TTable BinarySearch<TTable, TKey>(int count, TKey key, Func<int, TTable> elementAt)
            where TTable : class, IKeyedTable<TKey>
        {
            // For strings, we avoid having to re-convert our search key to UTF8 tons of times.
            // For others, this is a passthrough.
            Func<TKey, int> comparer = GetComparerFunc(key);

            int min = 0;
            int max = count - 1;

            while (min <= max)
            {
                // (min + max) / 2, written to avoid overflows.
                int mid = min + ((max - min) >> 1);

                var currentIndex = elementAt(mid);
                int comparison = comparer(currentIndex.Key);

                if (comparison == 0)
                {
                    return currentIndex;
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
