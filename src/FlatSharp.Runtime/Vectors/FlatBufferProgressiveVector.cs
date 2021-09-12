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
    using System.ComponentModel;

    /// <summary>
    /// A vector implementation that is gradually filled up.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FlatBufferProgressiveVector<T> : IList<T>, IReadOnlyList<T>
        where T : notnull
    {
        // Must be a power of 2.
        private const int ChunkSize = 64;

        // A semi-sparse array. Each row contains ChunkSize items.
        // This approach allows fast access while not over allocating.
        private readonly T?[]?[] items;
        private readonly IReadOnlyList<T> innerVector;

        public FlatBufferProgressiveVector(
            IReadOnlyList<T> innerVector)
        {
            this.innerVector = innerVector;
            this.items = new T[innerVector.Count / ChunkSize][];
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                ref T?[]? row = ref this.items[index / ChunkSize];
                if (row is null)
                {
                    row = new T[ChunkSize];
                }

                ref T? item = ref row[index & (ChunkSize - 1)];
                if (item is null)
                {
                    item = this.innerVector[index];
                }

                return item;
            }
            set
            {
                throw new NotMutableException("FlatBufferVector does not allow mutating items.");
            }
        }

        public int Count => this.innerVector.Count;

        public bool IsReadOnly => true;

        public void Add(T item)
        {
            throw new NotMutableException("FlatBufferVector does not allow adding items.");
        }

        public void Clear()
        {
            throw new NotMutableException("FlatBufferVector does not support clearing.");
        }

        public bool Contains(T? item)
        {
            return this.IndexOf(item) >= 0;
        }

        public void CopyTo(T[]? array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException();
            }

            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                yield return this[i];
            }
        }

        public int IndexOf(T? item)
        {
            // FlatBuffer vectors are not allowed to have null by definition.
            if (item is null)
            {
                return -1;
            }

            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                if (item.Equals(this[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotMutableException("FlatBufferVector does not support inserting.");
        }

        public bool Remove(T item)
        {
            throw new NotMutableException("FlatBufferVector does not support removing.");
        }

        public void RemoveAt(int index)
        {
            throw new NotMutableException("FlatBufferVector does not support removing.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
