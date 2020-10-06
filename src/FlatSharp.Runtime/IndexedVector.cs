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
    using System.Collections;
    using System.Collections.Generic;

#nullable enable

    /// <summary>
    /// An indexed vector.
    /// </summary>
    public sealed class IndexedVector<TKey, TValue> : IIndexedVector<TKey, TValue>
        where TValue : class
    {
        private static readonly Func<TValue, TKey> KeyGetter;

        private readonly Dictionary<TKey, TValue> backingDictionary;
        private bool mutable;

        static IndexedVector()
        {
            KeyGetter = SortedVectorHelpers.GetOrCreateGetKeyCallback<TValue, TKey>();
        }

        public IndexedVector()
        {
            this.backingDictionary = new Dictionary<TKey, TValue>();
            this.mutable = true;
        }

        public IndexedVector(IEnumerable<TValue> items, int capacity, bool mutable)
        {
            var dictionary = new Dictionary<TKey, TValue>(capacity);
            foreach (var item in items)
            {
                dictionary[KeyGetter(item)] = item;
            }

            this.backingDictionary = dictionary;
            this.mutable = mutable;
        }

        public IndexedVector(IReadOnlyList<TValue> items, bool mutable) : this(items, items.Count, mutable)
        {
        }

        /// <summary>
        /// Gets the key from the given value.
        /// </summary>
        public static TKey GetKey(TValue value) => KeyGetter(value);

        /// <summary>
        /// An indexer for getting values by their keys.
        /// </summary>
        public TValue this[TKey key]
        {
            get => this.backingDictionary[key];
        }

        /// <summary>
        /// Indicates if this IndexedVector is read only.
        /// </summary>
        public bool IsReadOnly => !this.mutable;

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        public int Count => this.backingDictionary.Count;

        /// <summary>
        /// Freezes an Indexed vector, preventing further modifications.
        /// </summary>
        public void Freeze()
        {
            this.mutable = false;
        }

        /// <summary>
        /// Returns true if the vector contains the given key.
        /// </summary>
        public bool ContainsKey(TKey key) => this.backingDictionary.ContainsKey(key);

        /// <summary>
        /// Tries to get the given value from the backing dictionary.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue? value)
        {
            return this.backingDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the dictionary's enumerator.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.backingDictionary.GetEnumerator();

        /// <summary>
        /// Gets a non-generic enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Adds or replaces the item with the given key to the indexed vector.
        /// </summary>
        public void AddOrReplace(TValue value)
        {
            if (!this.mutable)
            {
                throw new NotMutableException();
            }

            this.backingDictionary[KeyGetter(value)] = value;
        }

        /// <summary>
        /// Attempts to add the value to the indexed vector, if a key does not already exist.
        /// </summary>
        public bool Add(TValue value)
        {
            if (!this.mutable)
            {
                return false;
            }

            TKey key = KeyGetter(value);

#if NETCOREAPP
            return this.backingDictionary.TryAdd(key, value);
#else
            if (this.backingDictionary.ContainsKey(key))
            {
                return false;
            }

            this.backingDictionary[key] = value;
            return true;
#endif
        }

        public void Clear()
        {
            if (!this.mutable)
            {
                throw new NotMutableException();
            }

            this.backingDictionary.Clear();
        }

        public bool Remove(TKey key)
        {
            if (!this.mutable)
            {
                throw new NotMutableException();
            }

            return this.backingDictionary.Remove(key);
        }
    }
}
