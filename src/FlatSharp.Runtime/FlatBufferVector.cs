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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A base class that FlatBuffersNet implements to deserialize vectors. FlatBufferVetor{T} is a lazy implementation
    /// which will create a new instance for each item it returns. Calling .ToList() is an effective way to do caching.
    /// </summary>
    public abstract class FlatBufferVector<T, TInputBuffer> : IList<T>, IReadOnlyList<T>
        where TInputBuffer : IInputBuffer
    {
        private readonly TInputBuffer memory;
        private readonly int offset;
        private readonly int itemSize;
        private readonly int count;

        public FlatBufferVector(
            TInputBuffer memory,
            int offset,
            int itemSize)
        {
            this.memory = memory;
            this.offset = offset;
            this.itemSize = itemSize;
            this.count = checked((int)this.memory.ReadUInt(this.offset));

            // Advance to the start of the element at index 0. Easiest to do this once
            // in the .ctor than repeatedly for each index.
            this.offset = checked(this.offset + sizeof(uint));
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)this.count)
                {
                    throw new IndexOutOfRangeException();
                }

                return this.ParseItem(this.memory, GetOffset(this.itemSize, this.offset, index));
            }
            set
            {
                throw new NotMutableException("FlatBufferVector does not allow mutating items.");
            }
        }

        public int Count => this.count;

        public bool IsReadOnly => true;

        public void Add(T item)
        {
            throw new NotMutableException("FlatBufferVector does not allow adding items.");
        }

        public void Clear()
        {
            throw new NotMutableException("FlatBufferVector does not support clearing.");
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var count = this.count;
            var memory = this.memory;
            var offset = this.offset;
            var itemSize = this.itemSize;

            if (array.Length == count)
            {
                // special case: write code where the compiler can elide the 
                // bounds check on the array.
                for (int i = 0; i < array.Length; ++i)
                {
                    array[i] = this.ParseItem(memory, offset);
                    offset = checked(offset + itemSize);
                }
            }
            else
            {
                for (int i = 0; i < count; ++i)
                {
                    array[arrayIndex + i] = this.ParseItem(memory, offset);
                    offset = checked(offset + itemSize);
                }
            }
        }

        public T[] ToArray()
        {
            int count = this.count;
            T[] array = new T[count];
            this.CopyTo(array, 0);
            return array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int count = this.count;
            for (int i = 0; i < count; ++i)
            {
                yield return this.ParseItem(this.memory, GetOffset(this.itemSize, this.offset, i));
            }
        }

        public int IndexOf(T item)
        {
            // FlatBuffer vectors are not allowed to have null by definition.
            if (item is null)
            {
                return -1;
            }

            int count = this.count;
            var memory = this.memory;
            var offset = this.offset;
            var itemSize = this.itemSize;

            for (int i = 0; i < count; ++i)
            {
                if (item.Equals(this.ParseItem(memory, offset)))
                {
                    return i;
                }

                offset = checked(offset + itemSize);
            }

            return -1;
        }

        public List<T> FlatBufferVectorToList()
        {
            return new List<T>(this);
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
        private static int GetOffset(int itemSize, int baseOffset, int index)
        {
            return checked(baseOffset + (itemSize * index));
        }

        protected abstract T ParseItem(TInputBuffer buffer, int offset);
    }
}
