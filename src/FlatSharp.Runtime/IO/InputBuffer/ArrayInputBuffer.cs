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
/// An implementation of <see cref="IInputBuffer"/> for managed arrays.
/// </summary>
public struct ArrayInputBuffer : IInputBuffer
{
    private readonly byte[] memory;

    public ArrayInputBuffer(byte[] buffer)
    {
        this.memory = buffer;
    }

    public bool IsPinned => false;

    public bool IsReadOnly => false;

    public long Length => this.memory.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan GetReadOnlySpan()
    {
        return new(this.GetSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan GetSpan()
    {
        return new(this.memory.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<byte> GetMemory(long offset, int length)
    {
        checked
        {
            return this.memory.AsMemory().Slice((int)offset, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length)
    {
        return this.GetMemory(offset, length);
    }
}
