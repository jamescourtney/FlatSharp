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

    public long Length => this.pointer.segment.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> GetReadOnlySpan(long offset, int length)
    {
        return this.GetSpan(offset, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> GetSpan(long offset, int length)
    {
        checked
        {
            return this.pointer.segment.AsSpan((int)offset, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<byte> GetMemory(long offset, int length)
    {
        checked
        {
            return this.pointer.segment.AsMemory((int)offset, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length)
    {
        return this.GetMemory(offset, length);
    }

    // Array Segment is a relatively heavy struct. It contains an array pointer, an int offset, and and int length.
    // Copying this by value for each method call is actually slower than having a little private pointer to a single item.
    private class ArraySegmentPointer
    {
        public ArraySegment<byte> segment;
    }
}
