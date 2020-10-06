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
    using System.Linq;

    /// <summary>
    /// A class that FlatSharp implements to approximate a dictionary as a programming model. The class relies on
    /// the assumption that dictionary keys correspond to "key" members in the table.
    /// </summary>
    public abstract class FlatBufferDictionaryVector<TKey, TValue> 
        : IDictionary<TKey, TValue> 
        where TValue : class
    {
        private readonly IReadOnlyList<TValue> vector;

        public FlatBufferDictionaryVector(IReadOnlyList<TValue> innerVector)
        {
            this.vector = innerVector;
        }

        public TValue this[TKey key] 
        { 
            get
            {
                if (this.TryGetValue(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }

            set
            {
                throw new NotMutableException();
            }
        }

        public ICollection<TKey> Keys 
            => this.vector.Select(this.GetKey).ToList();

        public ICollection<TValue> Values 
            => this.vector.ToList();

        public int Count => this.vector.Count;

        public bool IsReadOnly => true;

        public bool ContainsKey(TKey key)
        {
            return this.TryGetValue(key, out _);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var pair in this)
            {
                array[arrayIndex++] = pair;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var value in this.vector)
            {
                yield return new KeyValuePair<TKey, TValue>(this.GetKey(value), value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = this.vector.BinarySearchByFlatBufferKey(key);
            return value != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IDictionary<TKey, TValue> ToDictionary()
        {
            var result = new Dictionary<TKey, TValue>(this.Count);
            foreach (var pair in this)
            {
                result[pair.Key] = pair.Value;
            }

            return result;
        }

        public bool Remove(TKey key)
        {
            throw new NotMutableException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotMutableException();
        }

        public void Clear()
        {
            throw new NotMutableException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotMutableException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotMutableException();
        }

        protected abstract TKey GetKey(TValue value);
    }
}
