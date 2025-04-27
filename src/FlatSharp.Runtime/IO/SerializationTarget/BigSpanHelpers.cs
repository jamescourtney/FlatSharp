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

namespace FlatSharp
{

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
}

namespace FlatSharp.Internal
{
    public static partial class BigSpanExtensions
    {    
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte UnsafeReadByte(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<byte>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteByte(this BigSpan span, long offset, byte value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<byte>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte UnsafeReadByte(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadByte(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static sbyte UnsafeReadSByte(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<sbyte>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteSByte(this BigSpan span, long offset, sbyte value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<sbyte>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static sbyte UnsafeReadSByte(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadSByte(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ushort UnsafeReadUShort(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<ushort>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteUShort(this BigSpan span, long offset, ushort value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<ushort>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ushort UnsafeReadUShort(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadUShort(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static short UnsafeReadShort(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<short>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteShort(this BigSpan span, long offset, short value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<short>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static short UnsafeReadShort(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadShort(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int UnsafeReadInt(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<int>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteInt(this BigSpan span, long offset, int value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<int>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int UnsafeReadInt(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadInt(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint UnsafeReadUInt(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<uint>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteUInt(this BigSpan span, long offset, uint value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<uint>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint UnsafeReadUInt(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadUInt(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static long UnsafeReadLong(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<long>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteLong(this BigSpan span, long offset, long value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<long>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static long UnsafeReadLong(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadLong(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ulong UnsafeReadULong(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<ulong>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteULong(this BigSpan span, long offset, ulong value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BinaryPrimitives.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<ulong>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ulong UnsafeReadULong(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadULong(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float UnsafeReadFloat(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<float>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BigSpan.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteFloat(this BigSpan span, long offset, float value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BigSpan.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<float>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float UnsafeReadFloat(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadFloat(offset);
            }

    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double UnsafeReadDouble(this BigSpan span, long offset)
            {
                var value = span.ReadUnalignedUnsafe<double>(offset);
                if (!BitConverter.IsLittleEndian)
                {
                    value = BigSpan.ReverseEndianness(value);
                }

                return value;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnsafeWriteDouble(this BigSpan span, long offset, double value)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    value = BigSpan.ReverseEndianness(value);
                }

                span.WriteUnalignedUnsafe<double>(offset, value);
            }
            

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double UnsafeReadDouble(this BigReadOnlySpan span, long offset)
            {
                return span.Span.UnsafeReadDouble(offset);
            }

        }
}