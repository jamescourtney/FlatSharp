/*
 * Copyright 2021 James Courtney
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
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// An <see cref="IIndexedVector{TKey, TValue}"/> implementation that loads data progressively.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FlatBufferProgressiveIndexedVector<TKey, TValue, TInputBuffer> : IIndexedVector<TKey, TValue>
        where TValue : class
        where TKey : notnull
        where TInputBuffer : IInputBuffer
    {
        private readonly Dictionary<TKey, TValue?> backingDictionary;
        private readonly FlatBufferProgressiveVector<TValue, TInputBuffer> backingVector;

        public FlatBufferProgressiveIndexedVector(FlatBufferVector<TValue, TInputBuffer> items)
        {
            this.backingDictionary = new Dictionary<TKey, TValue?>();
            this.backingVector = new FlatBufferProgressiveVector<TValue, TInputBuffer>(items);
        }

        /// <summary>
        /// An indexer for getting values by their keys.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (this.TryGetValue(key, out TValue? value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Indicates if this IndexedVector is read only.
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        public int Count => this.backingVector.Count;

        /// <summary>
        /// Freezes an Indexed vector, preventing further modifications.
        /// </summary>
        public void Freeze()
        {
        }

        /// <summary>
        /// Returns true if the vector contains the given key.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return this.TryGetValue(key, out _);
        }

        /// <summary>
        /// Tries to get the given value from the backing dictionary.
        /// </summary>
        public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            if (this.backingDictionary.TryGetValue(key, out value))
            {
                return value is not null;
            }

            value = ((IList<TValue>)this.backingVector).BinarySearchByFlatBufferKey(key);
            this.backingDictionary[key] = value;
            return value is not null;
        }

        /// <summary>
        /// Gets the dictionary's enumerator.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            int count = this.backingVector.Count;
            for (int i = 0; i < count; ++i)
            {
                TValue item = this.backingVector[i];
                yield return new KeyValuePair<TKey, TValue>(
                    IndexedVector<TKey, TValue>.GetKey(item),
                    item);
            }
        }

        /// <summary>
        /// Gets a non-generic enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Adds or replaces the item with the given key to the indexed vector.
        /// </summary>
        public void AddOrReplace(TValue value)
        {
            throw new NotMutableException();
        }

        /// <summary>
        /// Attempts to add the value to the indexed vector, if a key does not already exist.
        /// </summary>
        public bool Add(TValue value)
        {
            throw new NotMutableException();
        }

        public void Clear()
        {
            throw new NotMutableException();
        }

        public bool Remove(TKey key)
        {
            throw new NotMutableException();
        }
    }
}
