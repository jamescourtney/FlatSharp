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
    public class FlatBufferProgressiveVector<T> : IList<T>, IReadOnlyList<T>
        where T : notnull
    {
        private static readonly ulong[] Empty = new ulong[0];

        // The chunk size here matches the number of bits in the presenceMask array below.
        private const int ChunkSize = 64;

        // A semi-sparse array. Each row contains ChunkSize items.
        // This approach allows fast access while not broadly over allocating.
        // Using "mini arrays" also ensures good sequential access performance.
        private readonly T?[]?[] items;

        // array of bitmasks indicating presence. This is necessary for value types where we can't test for null.
        private readonly ulong[] presenceMask; 
        private readonly IList<T> innerVector;

        public FlatBufferProgressiveVector(
            IList<T> innerVector)
        {
            this.Count = innerVector.Count;
            this.innerVector = innerVector;
            this.items = new T[(innerVector.Count / ChunkSize) + 1][];

            if (typeof(T).IsValueType)
            {
                presenceMask = new ulong[this.items.Length];
            }
            else
            {
                presenceMask = Empty;
            }
        }

        /// <summary>
        /// Gets the item at the given index.
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

                var (row, col) = GetAddress(uindex);

                ref T? item = ref this.GetCellRef(row, col);
                if (typeof(T).IsValueType)
                {
                    ref ulong maskRef = ref this.GetMaskRef(row);
                    ulong mask = GetMask(col);

                    if ((maskRef & mask) == 0)
                    {
                        item = this.innerVector[index];
                        maskRef |= mask;
                    }
                }
                else
                {
                    if (item is null)
                    {
                        item = this.innerVector[index];
                    }
                }

                return item!;
            }

            set
            {
                // Let inner vector do the validation.
                this.innerVector[index] = value;

                var (row, col) = GetAddress((uint)index);
                this.GetCellRef(row, col) = value; // save.

                if (typeof(T).IsValueType)
                {
                    this.GetMaskRef(row) |= GetMask(col);
                }
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
        private static (uint row, uint col) GetAddress(uint index)
        {
            return (index / ChunkSize, index % ChunkSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref T? GetCellRef(uint rowAddr, uint colAddr)
        {
            ref T?[]? row = ref this.items[rowAddr];
            if (row is null)
            {
                row = new T[ChunkSize];
            }

            return ref row[colAddr];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref ulong GetMaskRef(uint rowAddr)
        {
            return ref this.presenceMask[rowAddr];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong GetMask(uint colAddr)
        {
            return 1ul << (int)colAddr;
        }

    }
}
