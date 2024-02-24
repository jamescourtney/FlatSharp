/*
 * Copyright 2021 James Courtney
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

using System.Text;

namespace FlatSharp;

/// <summary>
/// An implementation of <see cref="IInputBuffer"/> for array segments.
/// </summary>
public struct ArraySegmentInputBuffer : IInputBuffer
{
    private readonly ArraySegmentPointer pointer;

    public ArraySegmentInputBuffer(ArraySegment<byte> memory)
    {
        this.pointer = new ArraySegmentPointer { segment = memory };
    }

    public bool IsPinned => false;

    public bool IsReadOnly => false;

    public int Length => this.pointer.segment.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte(int offset)
    {
        return ScalarSpanReader.ReadByte(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSByte(int offset)
    {
        return ScalarSpanReader.ReadSByte(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort(int offset)
    {
        this.CheckAlignment(offset, sizeof(ushort));
        return ScalarSpanReader.ReadUShort(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShort(int offset)
    {
        this.CheckAlignment(offset, sizeof(short));
        return ScalarSpanReader.ReadShort(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt(int offset)
    {
        this.CheckAlignment(offset, sizeof(uint));
        return ScalarSpanReader.ReadUInt(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt(int offset)
    {
        this.CheckAlignment(offset, sizeof(int));
        return ScalarSpanReader.ReadInt(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULong(int offset)
    {
        this.CheckAlignment(offset, sizeof(ulong));
        return ScalarSpanReader.ReadULong(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLong(int offset)
    {
        this.CheckAlignment(offset, sizeof(long));
        return ScalarSpanReader.ReadLong(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloat(int offset)
    {
        this.CheckAlignment(offset, sizeof(float));
        return ScalarSpanReader.ReadFloat(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble(int offset)
    {
        this.CheckAlignment(offset, sizeof(double));
        return ScalarSpanReader.ReadDouble(this.pointer.segment.AsSpan().Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadString(int offset, int byteLength, Encoding encoding)
    {
        return ScalarSpanReader.ReadString(this.pointer.segment.AsSpan().Slice(offset, byteLength), encoding);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> GetReadOnlySpan()
    {
        return this.pointer.segment;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> GetSpan()
    {
        return this.pointer.segment;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<byte> GetMemory()
    {
        return this.pointer.segment;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory()
    {
        return this.pointer.segment;
    }

    // Array Segment is a relatively heavy struct. It contains an array pointer, an int offset, and and int length.
    // Copying this by value for each method call is actually slower than having a little private pointer to a single item.
    private class ArraySegmentPointer
    {
        public ArraySegment<byte> segment;
    }
}
