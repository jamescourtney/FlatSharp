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

using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;

namespace FlatSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A base class that FlatBuffersNet implements to deserialize vectors.
    /// </summary>
    public abstract class FlatBufferVector<T> : IVector<T>
    {
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public abstract void Add(T item);

        public abstract void Clear();

        public abstract bool Contains(T item);

        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract bool Remove(T item);

        public abstract int Count
        {
            get;
        }

        public abstract bool IsReadOnly
        {
            get;
        }

        public abstract int IndexOf(T item);

        public abstract void Insert(int index, T item);

        public abstract void RemoveAt(int index);

        public abstract T this[int index]
        {
            get;
            set;
        }

        public abstract Memory<byte> GetByteMemory();

        public abstract Memory<byte> GetByteMemory(int index);

        public abstract Memory<byte> GetByteMemory(int index, int length);

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return Count;
            }
        }
    }
    
    /// <summary>
    /// A base class that FlatBuffersNet implements to deserialize vectors. FlatBufferVetor{T} is a lazy implementation
    /// which will create a new instance for each item it returns. Calling .ToList() is an effective way to do caching.
    /// </summary>
    public abstract class FlatBufferVector<T, TInputBuffer> : FlatBufferVector<T>
        where TInputBuffer : IInputBuffer
    {
        private readonly TInputBuffer memory;
        private readonly int offset;
        private readonly int itemSize;
        private readonly int count;

        protected FlatBufferVector(
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
        public override T this[int index]
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


        public override Memory<byte> GetByteMemory()
        {
            return this.memory.GetByteMemory(GetOffset(this.itemSize, this.offset, 0), this.itemSize * this.count);
        }

        public override Memory<byte> GetByteMemory(int index)
        {
            if ((uint)index >= (uint)this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return this.memory.GetByteMemory(GetOffset(this.itemSize, this.offset, index), this.itemSize);
        }


        public override Memory<byte> GetByteMemory(int index, int length)
        {
            if ((uint)index >= (uint)this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            if ((uint)index + (uint)length >= (uint)this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            
            return this.memory.GetByteMemory(GetOffset(this.itemSize, this.offset, index), this.itemSize * length);
        }

        public override int Count => this.count;

        public override bool IsReadOnly => true;

        public override void Add(T item)
        {
            throw new NotMutableException("FlatBufferVector does not allow adding items.");
        }

        public override void Clear()
        {
            throw new NotMutableException("FlatBufferVector does not support clearing.");
        }

        public override bool Contains(T? item)
        {
            return this.IndexOf(item) >= 0;
        }

        public override void CopyTo(T[]? array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var count = this.count;
            var memory = this.memory;
            var offset = this.offset;
            var itemSize = this.itemSize;

            for (int i = 0; i < count; ++i)
            {
                array[arrayIndex + i] = this.ParseItem(memory, offset);
                offset = checked(offset + itemSize);
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.count];

            var offset = this.offset;
            var itemSize = this.itemSize;
            var memory = this.memory;

            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = this.ParseItem(memory, offset);
                offset = checked(offset + itemSize);
            }

            return array;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            int count = this.count;
            for (int i = 0; i < count; ++i)
            {
                yield return this.ParseItem(this.memory, GetOffset(this.itemSize, this.offset, i));
            }
        }

        public override int IndexOf(T? item)
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
            var list = new List<T>(this.count);
            list.AddRange(this);
            return list;
        }

        public override void Insert(int index, T item)
        {
            throw new NotMutableException("FlatBufferVector does not support inserting.");
        }

        public override bool Remove(T item)
        {
            throw new NotMutableException("FlatBufferVector does not support removing.");
        }

        public override void RemoveAt(int index)
        {
            throw new NotMutableException("FlatBufferVector does not support removing.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetOffset(int itemSize, int baseOffset, int index)
        {
            return checked(baseOffset + (itemSize * index));
        }

        protected abstract T ParseItem(TInputBuffer buffer, int offset);
    }
}
