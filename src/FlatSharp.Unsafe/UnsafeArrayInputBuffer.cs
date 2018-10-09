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

    public sealed unsafe class UnsafeArrayInputBuffer : InputBuffer
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

        public override int Length => this.length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte ReadByte(int offset)
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
        public override double ReadDouble(int offset)
        {
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
        public override float ReadFloat(int offset)
        {
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
        public override int ReadInt(int offset)
        {
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
        public override long ReadLong(int offset)
        {
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
        public override sbyte ReadSByte(int offset)
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
        public override short ReadShort(int offset)
        {
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
        public override uint ReadUInt(int offset)
        {
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
        public override ulong ReadULong(int offset)
        {
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
        public override ushort ReadUShort(int offset)
        {
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
        protected override Memory<byte> ReadByteMemoryBlockProtected(int start, int length)
        {
            checked
            {
                this.EnsureInBounds(start, length);
                return new ArraySegment<byte>(this.array, this.offset + start, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ReadStringProtected(int offset, int byteLength, Encoding encoding)
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
                if (offset + size >= this.length || offset < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
