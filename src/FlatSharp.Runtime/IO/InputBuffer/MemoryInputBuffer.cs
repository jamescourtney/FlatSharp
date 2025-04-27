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

namespace FlatSharp;

/// <summary>
/// An implementation of InputBuffer for writable memory segments.
/// </summary>
public readonly struct MemoryInputBuffer : IInputBuffer
{
    private readonly MemoryPointer pointer;

    public MemoryInputBuffer(Memory<byte> memory, bool isPinned = false)
    {
        this.pointer = new MemoryPointer { memory = memory, isPinned = isPinned };
    }

    public bool IsPinned => this.pointer.isPinned;

    public bool IsReadOnly => false;

    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.pointer.memory.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan GetReadOnlySpan()
    {
        return new(this.GetSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan GetSpan()
    {
        return new(this.pointer.memory.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length)
    {
        return this.GetMemory(offset, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<byte> GetMemory(long offset, int length)
    {
        checked
        {
            return this.pointer.memory.Slice((int)offset, length);
        }
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
