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
using System.Buffers;
using System.Text;

namespace FlatSharp;

public partial struct ArraySerializationTarget
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt8(long offset, byte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUInt8(long offset)
    {
        return this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt8(long offset, sbyte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadInt8(long offset)
    {
        return (sbyte)this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt16(long offset, short value)
    {
        BinaryPrimitives.WriteInt16LittleEndian(
            this.AsSpan(offset, sizeof(short)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16(long offset)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(short)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt16(long offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(
            this.AsSpan(offset, sizeof(ushort)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16(long offset)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ushort)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt32(long offset, int value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            this.AsSpan(offset, sizeof(int)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32(long offset)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(int)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt32(long offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(
            this.AsSpan(offset, sizeof(uint)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32(long offset)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(uint)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt64(long offset, long value)
    {
        BinaryPrimitives.WriteInt64LittleEndian(
            this.AsSpan(offset, sizeof(long)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64(long offset)
    {
        return BinaryPrimitives.ReadInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(long)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt64(long offset, ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(
            this.AsSpan(offset, sizeof(ulong)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64(long offset)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ulong)));
    }

    public int WriteStringBytes(long offset, string value, Encoding encoding)
    {
        checked
        {
            int maxBytes = encoding.GetMaxByteCount(value.Length);
            
#if NETSTANDARD2_0
            byte[] temp = ArrayPool<byte>.Shared.Rent(maxBytes);
            int length = encoding.GetBytes(value, 0, value.Length, temp, 0);
            temp.AsSpan().CopyTo(this.AsSpan((int)offset, length));
            ArrayPool<byte>.Shared.Return(temp);

            return length;
#else
            int size = maxBytes;
            long remainingSpace = this.Length - offset;
            if (remainingSpace < maxBytes)
            {
                size = (int)Math.Min(int.MaxValue, remainingSpace);
            }

            Span<byte> span = this.AsSpan(offset, size);
            return encoding.GetBytes(value, span);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(long offset, Span<byte> target)
    {
        this.AsReadOnlySpan(offset, target.Length).CopyTo(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyFrom(long offset, ReadOnlySpan<byte> data)
    {
        data.CopyTo(this.AsSpan(offset, data.Length));
    }

    private partial Span<byte> AsSpan(long offset, int length);

    private partial ReadOnlySpan<byte> AsReadOnlySpan(long offset, int length);
}


public partial struct InputBufferSerializationTarget<TInputBuffer>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt8(long offset, byte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUInt8(long offset)
    {
        return this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt8(long offset, sbyte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadInt8(long offset)
    {
        return (sbyte)this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt16(long offset, short value)
    {
        BinaryPrimitives.WriteInt16LittleEndian(
            this.AsSpan(offset, sizeof(short)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16(long offset)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(short)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt16(long offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(
            this.AsSpan(offset, sizeof(ushort)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16(long offset)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ushort)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt32(long offset, int value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            this.AsSpan(offset, sizeof(int)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32(long offset)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(int)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt32(long offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(
            this.AsSpan(offset, sizeof(uint)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32(long offset)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(uint)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt64(long offset, long value)
    {
        BinaryPrimitives.WriteInt64LittleEndian(
            this.AsSpan(offset, sizeof(long)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64(long offset)
    {
        return BinaryPrimitives.ReadInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(long)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt64(long offset, ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(
            this.AsSpan(offset, sizeof(ulong)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64(long offset)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ulong)));
    }

    public int WriteStringBytes(long offset, string value, Encoding encoding)
    {
        checked
        {
            int maxBytes = encoding.GetMaxByteCount(value.Length);
            
#if NETSTANDARD2_0
            byte[] temp = ArrayPool<byte>.Shared.Rent(maxBytes);
            int length = encoding.GetBytes(value, 0, value.Length, temp, 0);
            temp.AsSpan().CopyTo(this.AsSpan((int)offset, length));
            ArrayPool<byte>.Shared.Return(temp);

            return length;
#else
            int size = maxBytes;
            long remainingSpace = this.Length - offset;
            if (remainingSpace < maxBytes)
            {
                size = (int)Math.Min(int.MaxValue, remainingSpace);
            }

            Span<byte> span = this.AsSpan(offset, size);
            return encoding.GetBytes(value, span);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(long offset, Span<byte> target)
    {
        this.AsReadOnlySpan(offset, target.Length).CopyTo(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyFrom(long offset, ReadOnlySpan<byte> data)
    {
        data.CopyTo(this.AsSpan(offset, data.Length));
    }

    private partial Span<byte> AsSpan(long offset, int length);

    private partial ReadOnlySpan<byte> AsReadOnlySpan(long offset, int length);
}


public partial struct MemorySerializationTarget
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt8(long offset, byte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUInt8(long offset)
    {
        return this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt8(long offset, sbyte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadInt8(long offset)
    {
        return (sbyte)this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt16(long offset, short value)
    {
        BinaryPrimitives.WriteInt16LittleEndian(
            this.AsSpan(offset, sizeof(short)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16(long offset)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(short)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt16(long offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(
            this.AsSpan(offset, sizeof(ushort)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16(long offset)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ushort)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt32(long offset, int value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            this.AsSpan(offset, sizeof(int)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32(long offset)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(int)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt32(long offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(
            this.AsSpan(offset, sizeof(uint)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32(long offset)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(uint)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt64(long offset, long value)
    {
        BinaryPrimitives.WriteInt64LittleEndian(
            this.AsSpan(offset, sizeof(long)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64(long offset)
    {
        return BinaryPrimitives.ReadInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(long)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt64(long offset, ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(
            this.AsSpan(offset, sizeof(ulong)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64(long offset)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ulong)));
    }

    public int WriteStringBytes(long offset, string value, Encoding encoding)
    {
        checked
        {
            int maxBytes = encoding.GetMaxByteCount(value.Length);
            
#if NETSTANDARD2_0
            byte[] temp = ArrayPool<byte>.Shared.Rent(maxBytes);
            int length = encoding.GetBytes(value, 0, value.Length, temp, 0);
            temp.AsSpan().CopyTo(this.AsSpan((int)offset, length));
            ArrayPool<byte>.Shared.Return(temp);

            return length;
#else
            int size = maxBytes;
            long remainingSpace = this.Length - offset;
            if (remainingSpace < maxBytes)
            {
                size = (int)Math.Min(int.MaxValue, remainingSpace);
            }

            Span<byte> span = this.AsSpan(offset, size);
            return encoding.GetBytes(value, span);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(long offset, Span<byte> target)
    {
        this.AsReadOnlySpan(offset, target.Length).CopyTo(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyFrom(long offset, ReadOnlySpan<byte> data)
    {
        data.CopyTo(this.AsSpan(offset, data.Length));
    }

    private partial Span<byte> AsSpan(long offset, int length);

    private partial ReadOnlySpan<byte> AsReadOnlySpan(long offset, int length);
}


public ref partial struct SpanSerializationTarget
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt8(long offset, byte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUInt8(long offset)
    {
        return this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt8(long offset, sbyte value)
    {
        this.AsSpan(offset, sizeof(byte))[0] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadInt8(long offset)
    {
        return (sbyte)this.AsReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt16(long offset, short value)
    {
        BinaryPrimitives.WriteInt16LittleEndian(
            this.AsSpan(offset, sizeof(short)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16(long offset)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(short)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt16(long offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(
            this.AsSpan(offset, sizeof(ushort)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16(long offset)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ushort)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt32(long offset, int value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            this.AsSpan(offset, sizeof(int)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32(long offset)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(int)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt32(long offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(
            this.AsSpan(offset, sizeof(uint)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32(long offset)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(uint)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteInt64(long offset, long value)
    {
        BinaryPrimitives.WriteInt64LittleEndian(
            this.AsSpan(offset, sizeof(long)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64(long offset)
    {
        return BinaryPrimitives.ReadInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(long)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUInt64(long offset, ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(
            this.AsSpan(offset, sizeof(ulong)),
            value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64(long offset)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(
            this.AsReadOnlySpan(offset, sizeof(ulong)));
    }

    public int WriteStringBytes(long offset, string value, Encoding encoding)
    {
        checked
        {
            int maxBytes = encoding.GetMaxByteCount(value.Length);
            
#if NETSTANDARD2_0
            byte[] temp = ArrayPool<byte>.Shared.Rent(maxBytes);
            int length = encoding.GetBytes(value, 0, value.Length, temp, 0);
            temp.AsSpan().CopyTo(this.AsSpan((int)offset, length));
            ArrayPool<byte>.Shared.Return(temp);

            return length;
#else
            int size = maxBytes;
            long remainingSpace = this.Length - offset;
            if (remainingSpace < maxBytes)
            {
                size = (int)Math.Min(int.MaxValue, remainingSpace);
            }

            Span<byte> span = this.AsSpan(offset, size);
            return encoding.GetBytes(value, span);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(long offset, Span<byte> target)
    {
        this.AsReadOnlySpan(offset, target.Length).CopyTo(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyFrom(long offset, ReadOnlySpan<byte> data)
    {
        data.CopyTo(this.AsSpan(offset, data.Length));
    }

    private partial Span<byte> AsSpan(long offset, int length);

    private partial ReadOnlySpan<byte> AsReadOnlySpan(long offset, int length);
}

