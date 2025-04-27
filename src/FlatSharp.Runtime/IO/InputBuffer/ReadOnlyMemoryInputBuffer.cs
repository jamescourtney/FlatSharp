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

using System.Runtime.InteropServices;
using System.Text;

namespace FlatSharp;

/// <summary>
/// An implemenation of InputBuffer that accepts ReadOnlyMemory. ReadOnlyMemoryInputBuffer
/// behaves identically to MemoryInputBuffer with one exception, which is that it will refuse
/// to deserialize any mutable memory (Memory{T}) instances. These will result in an exception
/// being thrown. ReadOnlyMemoryInputBuffer guarantees that the objects returned will
/// not modify in the input buffer (unless unsafe operations / MemoryMarshal) are used.
/// </summary>
public struct ReadOnlyMemoryInputBuffer : IInputBuffer
{
    private const string ErrorMessage = "ReadOnlyMemory inputs may not deserialize writable memory.";

    private readonly MemoryPointer pointer;

    public ReadOnlyMemoryInputBuffer(ReadOnlyMemory<byte> memory, bool isPinned = false)
    {
        this.pointer = new MemoryPointer { memory = memory, isPinned = isPinned };
    }

    public bool IsPinned => this.pointer.isPinned;

    public bool IsReadOnly => true;

    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.pointer.memory.Length;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan GetReadOnlySpan()
    {
        var rwMemory = MemoryMarshal.AsMemory(this.pointer.memory);
        return new(new BigSpan(rwMemory.Span));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length)
    {
        checked
        {
            return this.pointer.memory.Slice((int)offset, length);
        }
    }

    public BigSpan GetSpan()
    {
        FSThrow.InvalidOperation(ErrorMessage);
        return default;
    }

    public Memory<byte> GetMemory(long offset, int length)
    {
        return FSThrow.InvalidOperation<Memory<byte>>(ErrorMessage);
    }

    // Memory<byte> is a relatively heavy struct. It's cheaper to wrap it in a
    // a reference that will be collected ephemerally in Gen0 than is is to
    // copy it around.
    private class MemoryPointer
    {
        public ReadOnlyMemory<byte> memory;
        public bool isPinned;
    }
}
