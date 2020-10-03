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
    using System.Text;

    /// <summary>
    /// An implemenation of InputBuffer that accepts ReadOnlyMemory. ReadOnlyMemoryInputBuffer
    /// behaves identically to MemoryInputBuffer with one exception, which is that it will refuse
    /// to deserialize any mutable memory (Memory{T}) instances. These will result in an exception
    /// being thrown. ReadOnlyMemoryInputBuffer guarantees that the objects returned will
    /// not modify in the input buffer (unless unsafe operations / MemoryMarshal) are used.
    /// </summary>
    public class ReadOnlyMemoryInputBuffer : IInputBuffer
    {
        private readonly ReadOnlyMemory<byte> memory;

        public ReadOnlyMemoryInputBuffer(ReadOnlyMemory<byte> memory)
        {
            this.memory = memory;
        }

        public int Length => this.memory.Length;

        public ISharedStringReader SharedStringReader { get; set; }

        public byte ReadByte(int offset)
        {
            return ScalarSpanReader.ReadByte(this.memory.Span.Slice(offset));
        }

        public sbyte ReadSByte(int offset)
        {
            return ScalarSpanReader.ReadSByte(this.memory.Span.Slice(offset));
        }

        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            return ScalarSpanReader.ReadUShort(this.memory.Span.Slice(offset));
        }

        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            return ScalarSpanReader.ReadShort(this.memory.Span.Slice(offset));
        }

        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            return ScalarSpanReader.ReadUInt(this.memory.Span.Slice(offset));
        }

        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            return ScalarSpanReader.ReadInt(this.memory.Span.Slice(offset));
        }

        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            return ScalarSpanReader.ReadULong(this.memory.Span.Slice(offset));
        }

        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            return ScalarSpanReader.ReadLong(this.memory.Span.Slice(offset));
        }

        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            return ScalarSpanReader.ReadFloat(this.memory.Span.Slice(offset));
        }

        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            return ScalarSpanReader.ReadDouble(this.memory.Span.Slice(offset));
        }

        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            return ScalarSpanReader.ReadString(this.memory.Span.Slice(offset, byteLength), encoding);
        }

        public virtual Memory<byte> GetByteMemory(int start, int length)
        {
            throw new InvalidOperationException("ReadOnlyMemory inputs may not deserialize writable memory.");
        }

        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.memory.Slice(start, length);
        }
    }
}
