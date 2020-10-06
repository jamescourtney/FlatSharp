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

#nullable enable
namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An indexed vector -- suitable for accessing values by their keys.
    /// </summary>
    public interface IIndexedVector<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TValue : class
    {
        /// <summary>
        /// Looks up the value by key.
        /// </summary>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Indicates if this indexed vector is read only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the number of items in this vector.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Tries to get the value of the given key.
        /// </summary>
        bool TryGetValue(TKey key, out TValue? value);

        /// <summary>
        /// Returns true if the vector contains the given key.
        /// </summary>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Adds or replaces the given value in the indexed vector.
        /// </summary>
        void AddOrReplace(TValue value);

        /// <summary>
        /// Tries to add the given value. Fails if a key already exists.
        /// </summary>
        bool Add(TValue value);

        /// <summary>
        /// Prevents further mutations to this vector.
        /// </summary>
        void Freeze();

        /// <summary>
        /// Clears the vector.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the item identified by the key. This method returns false if the key is not present.
        /// </summary>
        bool Remove(TKey key);
    }

    /// <summary>
    /// Extensions for the IIndexedVector interface.
    /// </summary>
    public static class IIndexedVectorExtensions
    {
        /// <summary>
        /// Clones the given indexed vector.
        /// </summary>
        public static IIndexedVector<TKey, TValue> Clone<TKey, TValue>(
            this IIndexedVector<TKey, TValue> source,
            Func<TValue, TValue> valueClone) where TValue : class
        {
            return new IndexedVector<TKey, TValue>(source.Select(x => valueClone(x.Value)), source.Count, mutable: false);
        }
    }
}
