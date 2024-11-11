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
using System.Text;

namespace FlatSharp;

/// <summary>
/// A virtual serialization target that pretends to write data. Useful for simulating how many bytes
/// a write operation will consume.
/// </summary>
internal readonly struct VirtualSerializationTarget : IFlatBufferReaderWriter<VirtualSerializationTarget>
{
    public long Length => long.MaxValue;

    public VirtualSerializationTarget Slice(long start, long length)
    {
        return this;
    }

    public VirtualSerializationTarget Slice(long start)
    {
        return this;
    }

    public void WriteUInt8(long offset, byte value)
    {
    }

    public byte ReadUInt8(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteInt8(long offset, sbyte value)
    {
    }

    public sbyte ReadInt8(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteInt16(long offset, short value)
    {
    }

    public short ReadInt16(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteUInt16(long offset, ushort value)
    {
    }

    public ushort ReadUInt16(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteInt32(long offset, int value)
    {
    }

    public int ReadInt32(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteUInt32(long offset, uint value)
    {
    }

    public uint ReadUInt32(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteInt64(long offset, long value)
    {
    }

    public long ReadInt64(long offset)
    {
        throw new NotImplementedException();
    }

    public void WriteUInt64(long offset, ulong value)
    {
    }

    public ulong ReadUInt64(long offset)
    {
        throw new NotImplementedException();
    }

    public int WriteStringBytes(long offset, string value, Encoding encoding)
    {
        return encoding.GetByteCount(value);
    }

    public void CopyFrom(long offset, ReadOnlySpan<byte> data)
    {
    }

    public void CopyTo(long offset, Span<byte> destination)
    {
        throw new NotImplementedException();
    }
}