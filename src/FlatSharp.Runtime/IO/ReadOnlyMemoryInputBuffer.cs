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
    using System.Runtime.CompilerServices;
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

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.memory.Length;
        }

        public ISharedStringReader SharedStringReader
        {
            get;
            set; 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(int offset)
        {
            return ScalarSpanReader.ReadByte(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(int offset)
        {
            return ScalarSpanReader.ReadSByte(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            return ScalarSpanReader.ReadUShort(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            return ScalarSpanReader.ReadShort(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            return ScalarSpanReader.ReadUInt(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            return ScalarSpanReader.ReadInt(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            return ScalarSpanReader.ReadULong(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            return ScalarSpanReader.ReadLong(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            return ScalarSpanReader.ReadFloat(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            return ScalarSpanReader.ReadDouble(this.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            return ScalarSpanReader.ReadString(this.memory.Span.Slice(offset, byteLength), encoding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Memory<byte> GetByteMemory(int start, int length)
        {
            throw new InvalidOperationException("ReadOnlyMemory inputs may not deserialize writable memory.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.memory.Slice(start, length);
        }

        public TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset)
        {
            return serializer.Parse(new Wrapper(this), offset);
        }


        private readonly struct Wrapper : IInputBuffer
        {
            private readonly ReadOnlyMemoryInputBuffer buffer;

            public Wrapper(ReadOnlyMemoryInputBuffer buffer) => this.buffer = buffer;

            public ISharedStringReader SharedStringReader { get => this.buffer.SharedStringReader; set => this.buffer.SharedStringReader = value; }
            public int Length
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.buffer.Length;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory(int start, int length) => this.buffer.GetByteMemory(start, length);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length) => this.buffer.GetReadOnlyByteMemory(start, length);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset) => throw new NotImplementedException();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public byte ReadByte(int offset) => this.buffer.ReadByte(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ReadDouble(int offset) => this.buffer.ReadDouble(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public float ReadFloat(int offset) => this.buffer.ReadFloat(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int ReadInt(int offset) => this.buffer.ReadInt(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long ReadLong(int offset) => this.buffer.ReadLong(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sbyte ReadSByte(int offset) => this.buffer.ReadSByte(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public short ReadShort(int offset) => this.buffer.ReadShort(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public string ReadString(int offset, int byteLength, Encoding encoding) => this.buffer.ReadString(offset, byteLength, encoding);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint ReadUInt(int offset) => this.buffer.ReadUInt(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ulong ReadULong(int offset) => this.buffer.ReadULong(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ushort ReadUShort(int offset) => this.buffer.ReadUShort(offset);
        }
    }
}
