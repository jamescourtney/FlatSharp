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
    using System.Buffers.Binary;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// An implementation of <see cref="InputBuffer"/> for managed arrays.
    /// </summary>
    public sealed class ArrayInputBuffer : InputBuffer
    {
        private readonly ArraySegment<byte> memory;

        public override int Length => this.memory.Count;

        public ArrayInputBuffer(ArraySegment<byte> memory)
        {
            this.memory = memory;
        }

        public ArrayInputBuffer(byte[] buffer) : this(new ArraySegment<byte>(buffer))
        {
        }

        public override byte ReadByte(int offset)
        {
            return this.memory.AsSpan()[offset];
        }

        public override sbyte ReadSByte(int offset)
        {
            return (sbyte)this.memory.AsSpan()[offset];
        }

        public override ushort ReadUShort(int offset)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override short ReadShort(int offset)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override uint ReadUInt(int offset)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override int ReadInt(int offset)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override ulong ReadULong(int offset)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override long ReadLong(int offset)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(this.memory.AsSpan().Slice(offset));
        }

        public override float ReadFloat(int offset)
        {
            Span<FloatLayout> layouts = stackalloc FloatLayout[1];
            layouts[0] = new FloatLayout { bytes = this.ReadUInt(offset) };
            return layouts[0].value;
        }

        public override double ReadDouble(int offset)
        {
            return BitConverter.Int64BitsToDouble(this.ReadLong(offset));
        }

        protected override string ReadStringProtected(int offset, int byteLength, Encoding encoding)
        {
#if NETCOREAPP2_1
            return encoding.GetString(this.memory.AsSpan().Slice(offset, byteLength));
#else
            return encoding.GetString(this.memory.AsSpan().Slice(offset, byteLength).ToArray());
#endif
        }

        protected override Memory<byte> ReadByteMemoryBlockProtected(int start, int length)
        {
            return new Memory<byte>(this.memory.Array, this.memory.Offset + start, length);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatLayout
        {
            [FieldOffset(0)]
            public uint bytes;

            [FieldOffset(0)]
            public float value;
        }
    }
}
