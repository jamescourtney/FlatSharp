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

using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace FlatSharp;

/// <summary>
/// Utility class for writing items to spans.
/// </summary>
public struct SpanWriter : ISpanWriter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteByte(Span<byte> span, byte value, int offset)
    {
        span[offset] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSByte(Span<byte> span, sbyte value, int offset)
    {
        span[offset] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUShort(Span<byte> span, ushort value, int offset)
    {
        this.CheckAlignment(offset, sizeof(ushort));
        BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteShort(Span<byte> span, short value, int offset)
    {
        this.CheckAlignment(offset, sizeof(short));
        BinaryPrimitives.WriteInt16LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt(Span<byte> span, uint value, int offset)
    {
        this.CheckAlignment(offset, sizeof(uint));
        BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt(Span<byte> span, int value, int offset)
    {
        this.CheckAlignment(offset, sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteULong(Span<byte> span, ulong value, int offset)
    {
        this.CheckAlignment(offset, sizeof(ulong));
        BinaryPrimitives.WriteUInt64LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteLong(Span<byte> span, long value, int offset)
    {
        this.CheckAlignment(offset, sizeof(long));
        BinaryPrimitives.WriteInt64LittleEndian(span.Slice(offset), value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteFloat(Span<byte> span, float value, int offset)
    {
        ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
        {
            value = value
        };

        this.WriteUInt(span, floatLayout.bytes, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteDouble(Span<byte> span, double value, int offset)
    {
        this.WriteLong(span, BitConverter.DoubleToInt64Bits(value), offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetStringBytes(Span<byte> destination, string value, Encoding encoding)
    {
#if NETSTANDARD2_0
        int length = value.Length;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(encoding.GetMaxByteCount(length));
        int bytesWritten = encoding.GetBytes(value, 0, length, buffer, 0);
        buffer.AsSpan().Slice(0, bytesWritten).CopyTo(destination);
        ArrayPool<byte>.Shared.Return(buffer);
#else
        int bytesWritten = encoding.GetBytes(value, destination);
#endif

        return bytesWritten;
    }

    public void FlushSharedStrings(ISharedStringWriter writer, Span<byte> destination, SerializationContext context)
    {
        writer.FlushWrites(this, destination, context);
    }
}
