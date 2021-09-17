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
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// A class that FlatSharp uses to deserialize vectors of unions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FlatBufferVectorOfUnion<T, TInputBuffer> : FlatBufferVectorBase<T, TInputBuffer>
        where TInputBuffer : IInputBuffer
        where T : IFlatBufferUnion
    {
        private readonly int discriminatorVectorOffset;
        private readonly int offsetVectorOffset;

        protected FlatBufferVectorOfUnion(
            TInputBuffer memory,
            int discriminatorOffset,
            int offsetVectorOffset,
            in TableFieldContext fieldContext) : base(memory, fieldContext)
        {
            uint discriminatorCount = memory.ReadUInt(discriminatorOffset);
            uint offsetCount = memory.ReadUInt(offsetVectorOffset);

            if (discriminatorCount != offsetCount)
            {
                throw new InvalidDataException($"Union vector had mismatched number of discriminators and offsets.");
            }

            checked
            {
                this.Count = (int)offsetCount;
                this.discriminatorVectorOffset = discriminatorOffset + sizeof(int);
                this.offsetVectorOffset = offsetVectorOffset + sizeof(int);
            }
        }

        protected override void ParseItem(int index, out T item)
        {
            checked
            {
                this.ParseItem(
                    this.memory,
                    this.discriminatorVectorOffset + index,
                    this.offsetVectorOffset + (index * sizeof(int)),
                    this.fieldContext,
                    out item);
            }
        }

        protected abstract void ParseItem(
            TInputBuffer buffer,
            int discriminatorOffset,
            int offsetOffset,
            TableFieldContext fieldContext,
            out T item);
    }
}
