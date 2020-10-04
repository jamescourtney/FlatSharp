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

namespace FlatSharp.Unsafe
{
    using System;
    using System.Buffers;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed unsafe class UnsafeArrayInputBuffer : IInputBuffer
    {
        private readonly byte[] array;
        private readonly int offset;
        private readonly int length;

        public UnsafeArrayInputBuffer(ArraySegment<byte> memory)
        {
            this.length = memory.Count;
            this.offset = memory.Offset;
            this.array = memory.Array;

            if (!BitConverter.IsLittleEndian)
            {
                throw new InvalidOperationException("UnsafeArrayInputBuffer only works on little-endian architectures presently. On big-endian systems, ArrayInputBuffer and MemoryInputBuffer will both work.");
            }
        }

        public UnsafeArrayInputBuffer(byte[] memory) : this(new ArraySegment<byte>(memory))
        {
        }

        public int Length => this.length;

        public ISharedStringReader SharedStringReader { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(byte));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            checked
            {
                this.EnsureInBounds(offset, sizeof(double));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(double*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            checked
            {
                this.EnsureInBounds(offset, sizeof(float));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(float*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            checked
            {
                this.EnsureInBounds(offset, sizeof(int));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(int*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            checked
            {
                this.EnsureInBounds(offset, sizeof(long));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(long*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(sbyte));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(sbyte*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            checked
            {
                this.EnsureInBounds(offset, sizeof(short));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(short*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            checked
            {
                this.EnsureInBounds(offset, sizeof(uint));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(uint*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ulong));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(ulong*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ushort));
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return *(ushort*)pByte;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetByteMemory(int start, int length)
        {
            checked
            {
                EnsureInBounds(start, length);
                return new ArraySegment<byte>(this.array, this.offset + start, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.GetByteMemory(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            checked
            {
                this.EnsureInBounds(offset, byteLength);
                fixed (byte* pByte = &this.array[this.offset + offset])
                {
                    return encoding.GetString(pByte, byteLength);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureInBounds(int offset, int size)
        {
            checked
            {
                if (offset + size > this.length || offset < 0 || size < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset)
        {
            return serializer.Parse(new Wrapper(this), offset);
        }
        
        private readonly struct Wrapper : IInputBuffer
        {
            private readonly UnsafeArrayInputBuffer buffer;

            public Wrapper(UnsafeArrayInputBuffer buffer) => this.buffer = buffer;

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
