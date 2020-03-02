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
    /// A utility class for accessing memory.
    /// </summary>
    public class ReadOnlyMemoryInputBuffer : InputBuffer
    {
        private readonly ReadOnlyMemory<byte> memory;

        public override int Length => this.memory.Length;

        public ReadOnlyMemoryInputBuffer(ReadOnlyMemory<byte> memory)
        {
            this.memory = memory;
        }

        public override byte ReadByte(int offset)
        {
            return this.memory.Span[offset];
        }

        public override sbyte ReadSByte(int offset)
        {
            return (sbyte)this.memory.Span[offset];
        }

        public override ushort ReadUShort(int offset)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(this.memory.Span.Slice(offset));
        }

        public override short ReadShort(int offset)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(this.memory.Span.Slice(offset));
        }

        public override uint ReadUInt(int offset)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(this.memory.Span.Slice(offset));
        }

        public override int ReadInt(int offset)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(this.memory.Span.Slice(offset));
        }

        public override ulong ReadULong(int offset)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(this.memory.Span.Slice(offset));
        }

        public override long ReadLong(int offset)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(this.memory.Span.Slice(offset));
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
#if NETCOREAPP
            return encoding.GetString(this.memory.Span.Slice(offset, byteLength));
#else
            return encoding.GetString(this.memory.Span.Slice(offset, byteLength).ToArray());
#endif
        }

        protected override Memory<byte> ReadByteMemoryBlockProtected(int start, int length)
        {
            throw new InvalidOperationException("ReadOnlyMemory inputs may not access writable memory.");
        }

        protected override ReadOnlyMemory<byte> ReadByteReadOnlyMemoryBlockProtected(int start, int length)
        {
            return this.memory.Slice(start, length);
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
