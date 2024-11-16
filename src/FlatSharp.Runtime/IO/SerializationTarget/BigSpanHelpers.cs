/*
 * Copyright 2024 James Courtney
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



using System.Buffers.Binary;

namespace FlatSharp;

public readonly ref partial struct BigSpan
{


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(long offset)
        {
            var value = this.ReadUnaligned<byte>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteByte(long offset, byte value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(long offset)
        {
            var value = this.ReadUnaligned<sbyte>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteSByte(long offset, sbyte value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(long offset)
        {
            var value = this.ReadUnaligned<ushort>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUShort(long offset, ushort value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(long offset)
        {
            var value = this.ReadUnaligned<short>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteShort(long offset, short value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(long offset)
        {
            var value = this.ReadUnaligned<int>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt(long offset, int value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(long offset)
        {
            var value = this.ReadUnaligned<uint>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUInt(long offset, uint value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(long offset)
        {
            var value = this.ReadUnaligned<long>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLong(long offset, long value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(long offset)
        {
            var value = this.ReadUnaligned<ulong>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteULong(long offset, ulong value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(long offset)
        {
            var value = this.ReadUnaligned<float>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFloat(long offset, float value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(long offset)
        {
            var value = this.ReadUnaligned<double>(offset);
            if (!BitConverter.IsLittleEndian)
            {
                value = ReverseEndianness(value);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDouble(long offset, double value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = ReverseEndianness(value);
            }

            this.WriteUnaligned(offset, value);
        }
}

public readonly ref partial struct BigReadOnlySpan
{


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(long offset) => this.span.ReadByte(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(long offset) => this.span.ReadSByte(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(long offset) => this.span.ReadUShort(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(long offset) => this.span.ReadShort(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(long offset) => this.span.ReadInt(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(long offset) => this.span.ReadUInt(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(long offset) => this.span.ReadLong(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(long offset) => this.span.ReadULong(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(long offset) => this.span.ReadFloat(offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(long offset) => this.span.ReadDouble(offset);
}
