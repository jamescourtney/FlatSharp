/*
 * Copyright 2018 James Courtney
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
    using System.Text;

    /// <summary>
    /// An implementation of <see cref="IInputBuffer"/> for managed arrays.
    /// </summary>
    public sealed class ArrayInputBuffer : IInputBuffer
    {
        private readonly ArraySegment<byte> memory;

        public ArrayInputBuffer(ArraySegment<byte> memory)
        {
            this.memory = memory;
        }

        public ArrayInputBuffer(byte[] buffer) : this(new ArraySegment<byte>(buffer))
        {
        }

        public int Length => this.memory.Count;

        public ISharedStringReader SharedStringReader { get; set; }

        public byte ReadByte(int offset)
        {
            return ScalarSpanReader.ReadByte(this.memory.AsSpan().Slice(offset));
        }

        public sbyte ReadSByte(int offset)
        {
            return ScalarSpanReader.ReadSByte(this.memory.AsSpan().Slice(offset));
        }

        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            return ScalarSpanReader.ReadUShort(this.memory.AsSpan().Slice(offset));
        }

        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            return ScalarSpanReader.ReadShort(this.memory.AsSpan().Slice(offset));
        }

        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            return ScalarSpanReader.ReadUInt(this.memory.AsSpan().Slice(offset));
        }

        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            return ScalarSpanReader.ReadInt(this.memory.AsSpan().Slice(offset));
        }

        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            return ScalarSpanReader.ReadULong(this.memory.AsSpan().Slice(offset));
        }

        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            return ScalarSpanReader.ReadLong(this.memory.AsSpan().Slice(offset));
        }

        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            return ScalarSpanReader.ReadFloat(this.memory.AsSpan().Slice(offset));
        }

        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            return ScalarSpanReader.ReadDouble(this.memory.AsSpan().Slice(offset));
        }

        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            return ScalarSpanReader.ReadString(this.memory.AsSpan().Slice(offset, byteLength), encoding);
        }

        public Memory<byte> GetByteMemory(int start, int length)
        {
            return new Memory<byte>(this.memory.Array, this.memory.Offset + start, length);
        }

        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.GetByteMemory(start, length);
        }
    }
}
