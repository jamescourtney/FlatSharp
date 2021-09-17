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

    /// <summary>
    /// A base flat buffer vector for non-unions.
    /// </summary>
    public abstract class FlatBufferVector<T, TInputBuffer> : FlatBufferVectorBase<T, TInputBuffer>
        where TInputBuffer : IInputBuffer
    {
        private readonly int offset;
        private readonly int itemSize;

        protected FlatBufferVector(
            TInputBuffer memory,
            int offset,
            int itemSize,
            in TableFieldContext fieldContext) : base(memory, fieldContext)
        {
            this.offset = offset;
            this.itemSize = itemSize;
            this.Count = checked((int)this.memory.ReadUInt(this.offset));

            // Advance to the start of the element at index 0. Easiest to do this once
            // in the .ctor than repeatedly for each index.
            this.offset = checked(this.offset + sizeof(uint));
        }

        public override T this[int index]
        { 
            get => base[index];
            set
            {
                if (this.fieldContext.WriteThrough)
                {
                    int offset = checked(this.offset + (itemSize * index));
                    Span<byte> span = this.memory.GetByteMemory(offset, this.itemSize).Span;
                    this.WriteThrough(value, span);
                }
                else
                {
                    base[index] = value;
                }
            }
        }

        protected override void ParseItem(int index, out T item)
        {
            int offset = checked(this.offset + (itemSize * index));
            this.ParseItem(this.memory, offset, this.fieldContext, out item);
        }

        protected abstract void ParseItem(TInputBuffer buffer, int offset, TableFieldContext context, out T item);

        protected abstract void WriteThrough(T item, Span<byte> data);
    }
}
