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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Factory delegate.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate FlatBufferWriteThroughVector<T, TInputBuffer> FlatBufferWriteThroughVectorCtor<T, TInputBuffer>(
        TInputBuffer memory,
        int index0Offset,
        int itemSize,
        List<T> list) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// A base class for implementing write-through vectors.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FlatBufferWriteThroughVector<T, TInputBuffer> : IList<T>, IReadOnlyList<T>
        where TInputBuffer : IInputBuffer
    {
        private readonly TInputBuffer memory;
        private readonly int index0Offset;
        private readonly int itemSize;
        private readonly List<T> list;

        protected FlatBufferWriteThroughVector(
            TInputBuffer memory,
            int index0Offset,
            int itemSize,
            List<T> list)
        {
            this.memory = memory;
            this.index0Offset = index0Offset;
            this.itemSize = itemSize;
            this.list = list;
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.list[index];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("FlatBufferVectors cannot contain null items.");
                }

                checked
                {
                    this.list[index] = value;
                    Memory<byte> memory = this.memory.GetByteMemory(this.index0Offset + (index * this.itemSize), this.itemSize);
                    this.WriteItem(value, memory.Span);
                }
            }
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.list.Count;
        }

        public bool IsReadOnly => false;

        public void Add(T item) => throw new NotMutableException("FlatBufferVector does not allow adding items.");

        public void Clear() => throw new NotMutableException("FlatBufferVector does not support clearing.");

        public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

        public T[] ToArray() => this.list.ToArray();

        public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

        public void Insert(int index, T? item) => throw new NotMutableException("FlatBufferVector does not support inserting.");

        public bool Remove(T? item) => throw new NotMutableException("FlatBufferVector does not support removing.");

        public void RemoveAt(int index) => throw new NotMutableException("FlatBufferVector does not support removing.");

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Contains(T item)
        {
            if (item is null)
            {
                return false;
            }

            return this.list.Contains(item);
        }

        public int IndexOf(T item)
        {
            if (item is null)
            {
                return -1;
            }

            return this.list.IndexOf(item);
        }

        protected abstract void WriteItem(T item, Span<byte> span);
    }
}
