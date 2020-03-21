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

                return FlatBufferStringComparer.CompareSpans(leftSpan, rightData);
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
