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
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A vector implementation that is filled on demand. Optimized
    /// for data locality, random access, and reasonably low memory overhead.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FlatBufferProgressiveVector<T, TInputBuffer> : IList<T>, IReadOnlyList<T>
        where T : notnull
        where TInputBuffer : IInputBuffer
    {
        // The chunk size here matches the number of bits in the presenceMask array below.
        private const uint ChunkSize = 32;

        // A semi-sparse array. Each row contains ChunkSize items.
        // This approach allows fast access while not broadly over allocating.
        // Using "mini arrays" also ensures good sequential access performance.
        private readonly T?[]?[] items;
        private readonly FlatBufferVectorBase<T, TInputBuffer> innerVector;

        public FlatBufferProgressiveVector(
            FlatBufferVectorBase<T, TInputBuffer> innerVector)
        {
            this.Count = innerVector.Count;
            this.innerVector = innerVector;
            this.items = new T[(innerVector.Count / ChunkSize) + 1][];
        }

        /// <summary>
        /// Gets or sets the item at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                uint uindex = (uint)index;
                if (uindex >= this.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                GetAddress(uindex, out uint rowIndex, out uint colIndex);

                T?[]?[] items = this.items;
                T?[]? row = this.GetOrCreateRow(items, rowIndex);
                T? item = row[colIndex];

                if (!typeof(T).IsValueType)
                {
                    if (item is null)
                    {
                        item = this.innerVector[index];
                        row[colIndex] = item;
                    }
                }

                return item!;
            }

            set
            {
                // Let inner vector do the validation of the index.
                this.innerVector[index] = value;

                GetAddress((uint)index, out uint rowIndex, out uint colIndex);

                T?[]?[] items = this.items;
                T?[] row = this.GetOrCreateRow(items, rowIndex);
                row[colIndex] = value;
            }
        }

        public int Count { get; }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetAddress(uint index, out uint rowIndex, out uint colIndex)
        {
            rowIndex = index / ChunkSize;
            colIndex = index % ChunkSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T?[] GetOrCreateRow(T?[]?[] items, uint rowIndex)
        {
            T?[]? row = items[rowIndex];

            if (row is null)
            {
                row = new T[ChunkSize];
                items[rowIndex] = row;

                // For value types -- we can't rely on null to tell
                // us if the value is allocated or not, so just greedily
                // allocate the whole chunk. Chunks are relatively
                // small, so the overhead here is not enormous, and there
                // is no extra allocation since this is a value type.
                if (typeof(T).IsValueType)
                {
                    int rowStartIndex = checked((int)(ChunkSize * rowIndex));
                    this.innerVector.CopyRangeTo(rowStartIndex, row, ChunkSize);
                }
            }

            return row;
        }
    }
}
