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
    /// A base flat buffer vector, common to standard vectors and unions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FlatBufferVectorBase<T, TInputBuffer> : IList<T>, IReadOnlyList<T>
        where TInputBuffer : IInputBuffer
    {
        protected readonly TInputBuffer memory;
        protected readonly TableFieldContext fieldContext;

        protected FlatBufferVectorBase(
            TInputBuffer memory,
            TableFieldContext fieldContext)
        {
            this.memory = memory;
            this.fieldContext = fieldContext;
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        public virtual T this[int index]
        {
            get
            {
                if ((uint)index >= this.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                this.ParseItem(index, out T item);
                return item;
            }
            set
            {
                throw new NotMutableException("FlatBufferVector does not allow mutating items.");
            }
        }

        public int Count { get; protected init; }

        public bool IsReadOnly => true;

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
            for (int i = 0; i < count; ++i)
            {
                this.ParseItem(i, out array[arrayIndex + i]);
            }
        }

        internal void CopyRangeTo(int startIndex, T?[] array, uint count)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            // might have more elements than we do.
            count = Math.Min(
                checked((uint)(this.Count - startIndex)),
                count);

            for (int i = 0; i < count; ++i)
            {
                this.ParseItem(i + startIndex, out array[i]);
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];

            for (int i = 0; i < array.Length; ++i)
            {
                this.ParseItem(i, out array[i]);
            }

            return array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                this.ParseItem(i, out T parsed);
                yield return parsed;
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
                this.ParseItem(i, out T parsed);
                if (item.Equals(parsed))
                {
                    return i;
                }
            }

            return -1;
        }

        public List<T> FlatBufferVectorToList()
        {
            int count = this.Count;
            var list = new List<T>(count);
            
            for (int i = 0; i < count; ++i)
            {
                this.ParseItem(i, out T item);
                list.Add(item);
            }

            return list;
        }

        protected abstract void ParseItem(int index, out T item);

        public void Add(T item)
        {
            throw new NotMutableException("FlatBufferVector does not allow adding items.");
        }

        public void Clear()
        {
            throw new NotMutableException("FlatBufferVector does not support clearing.");
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

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
