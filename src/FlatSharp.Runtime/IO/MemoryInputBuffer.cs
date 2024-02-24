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

using System.Text;

namespace FlatSharp;

/// <summary>
/// An implementation of InputBuffer for writable memory segments.
/// </summary>
public struct MemoryInputBuffer : IInputBuffer
{
    private readonly MemoryPointer pointer;

    public MemoryInputBuffer(Memory<byte> memory, bool isPinned = false)
    {
        this.pointer = new MemoryPointer { memory = memory, isPinned = isPinned };
    }

    public bool IsPinned => this.pointer.isPinned;

    public bool IsReadOnly => false;

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.pointer.memory.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte(int offset)
    {
        return ScalarSpanReader.ReadByte(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSByte(int offset)
    {
        return ScalarSpanReader.ReadSByte(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort(int offset)
    {
        this.CheckAlignment(offset, sizeof(ushort));
        return ScalarSpanReader.ReadUShort(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShort(int offset)
    {
        this.CheckAlignment(offset, sizeof(short));
        return ScalarSpanReader.ReadShort(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt(int offset)
    {
        this.CheckAlignment(offset, sizeof(uint));
        return ScalarSpanReader.ReadUInt(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt(int offset)
    {
        this.CheckAlignment(offset, sizeof(int));
        return ScalarSpanReader.ReadInt(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULong(int offset)
    {
        this.CheckAlignment(offset, sizeof(ulong));
        return ScalarSpanReader.ReadULong(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLong(int offset)
    {
        this.CheckAlignment(offset, sizeof(long));
        return ScalarSpanReader.ReadLong(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloat(int offset)
    {
        this.CheckAlignment(offset, sizeof(float));
        return ScalarSpanReader.ReadFloat(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble(int offset)
    {
        this.CheckAlignment(offset, sizeof(double));
        return ScalarSpanReader.ReadDouble(this.pointer.memory.Span.Slice(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadString(int offset, int byteLength, Encoding encoding)
    {
        return ScalarSpanReader.ReadString(this.pointer.memory.Span.Slice(offset, byteLength), encoding);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> GetReadOnlySpan()
    {
        return this.pointer.memory.Span;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> GetSpan()
    {
        return this.pointer.memory.Span;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory()
    {
        return this.pointer.memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<byte> GetMemory()
    {
        return this.pointer.memory;
    }

    // Memory<byte> is a relatively heavy struct. It's cheaper to wrap it in a
    // a reference that will be collected ephemerally in Gen0 than it is to
    // copy it around.
    private class MemoryPointer
    {
        public Memory<byte> memory;
        public bool isPinned;
    }
}
