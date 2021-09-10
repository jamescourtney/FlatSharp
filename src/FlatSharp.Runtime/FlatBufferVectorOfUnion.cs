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
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A base class that FlatBuffersNet implements to deserialize vectors. FlatBufferVetor{T} is a lazy implementation
    /// which will create a new instance for each item it returns. Calling .ToList() is an effective way to do caching.
    /// </summary>
    public abstract class FlatBufferVectorOfUnion<T, TInputBuffer> : IList<T>, IReadOnlyList<T>
        where TInputBuffer : IInputBuffer
        where T : IFlatBufferUnion
    {
        private readonly TInputBuffer memory;
        private readonly int count;
        private readonly int discriminatorVectorOffset;
        private readonly int offsetVectorOffset;
        private readonly TableFieldContext fieldContext;

        protected FlatBufferVectorOfUnion(
            TInputBuffer memory,
            int discriminatorOffset,
            int offsetVectorOffset,
            TableFieldContext fieldContext)
        {
            this.memory = memory;

            uint discriminatorCount = memory.ReadUInt(discriminatorOffset);
            uint offsetCount = memory.ReadUInt(offsetVectorOffset);
            this.fieldContext = fieldContext;

            if (discriminatorCount != offsetCount)
            {
                throw new InvalidDataException($"Union vector had mismatched number of discriminators and offsets.");
            }

            checked
            {
                this.count = (int)offsetCount;
                this.discriminatorVectorOffset = discriminatorOffset + sizeof(int);
                this.offsetVectorOffset = offsetVectorOffset + sizeof(int);
            }
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                return this.ParseItemHelper(this.memory, index);
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

        public bool Contains(T? item)
        {
            return this.IndexOf(item) >= 0;
        }

        public void CopyTo(T[]? array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var count = this.Count;
            var memory = this.memory;

            for (int i = 0; i < count; ++i)
            {
                array[arrayIndex + i] = this.ParseItemHelper(memory, i);
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            var memory = this.memory;

            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = this.ParseItemHelper(memory, i);
            }

            return array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                yield return this.ParseItemHelper(this.memory, i);
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
            var memory = this.memory;

            for (int i = 0; i < count; ++i)
            {
                if (item.Equals(this.ParseItemHelper(memory, i)))
                {
                    return i;
                }
            }

            return -1;
        }

        public List<T> FlatBufferVectorToList()
        {
            var list = new List<T>(this.Count);
            list.AddRange(this);
            return list;
        }

        public void Insert(int index, T? item)
        {
            throw new NotMutableException("FlatBufferVector does not support inserting.");
        }

        public bool Remove(T? item)
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

        private T ParseItemHelper(TInputBuffer buffer, int index)
        {
            if ((uint)index >= (uint)this.count)
            {
                throw new IndexOutOfRangeException("Union vector index out of range.");
            }

            checked
            {
                return this.ParseItem(
                    buffer, 
                    this.discriminatorVectorOffset + index, 
                    this.offsetVectorOffset + (index * sizeof(int)),
                    this.fieldContext);
            }
        }

        protected abstract T ParseItem(
            TInputBuffer buffer,
            int discriminatorOffset,
            int offsetOffset,
            in TableFieldContext fieldContext);
    }
}
