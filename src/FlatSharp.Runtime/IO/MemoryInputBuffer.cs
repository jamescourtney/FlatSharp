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
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// An implementation of InputBuffer for writable memory segments.
    /// </summary>
    public struct MemoryInputBuffer : IInputBuffer
    {
        private readonly MemoryPointer pointer;

        public MemoryInputBuffer(Memory<byte> memory)
        {
            this.pointer = new MemoryPointer { memory = memory };
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.pointer.memory.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(int offset)
        {
            return ScalarSpanReader.ReadByte(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(int offset)
        {
            return ScalarSpanReader.ReadSByte(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            return ScalarSpanReader.ReadUShort(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            return ScalarSpanReader.ReadShort(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            return ScalarSpanReader.ReadUInt(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            return ScalarSpanReader.ReadInt(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            return ScalarSpanReader.ReadULong(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            return ScalarSpanReader.ReadLong(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            return ScalarSpanReader.ReadFloat(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            return ScalarSpanReader.ReadDouble(this.pointer.memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            return ScalarSpanReader.ReadString(this.pointer.memory.Span.Slice(offset, byteLength), encoding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetByteMemory(int start, int length)
        {
            return this.pointer.memory.Slice(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.GetByteMemory(start, length);
        }

        public T InvokeParse<T>(IGeneratedSerializer<T> serializer, int offset)
        {
            return serializer.Parse(this, offset);
        }

        // Memory<byte> is a relatively heavy struct. It's cheaper to wrap it in a
        // a reference that will be collected ephemerally in Gen0 than is is to
        // copy it around.
        private class MemoryPointer
        {
            public Memory<byte> memory;
        }
    }
}
