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

namespace FlatSharp;

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;


public readonly ref partial struct BigReadOnlySpan
{
    private readonly BigSpan span;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan(BigSpan span)
    {
        this.span = span;
    }

    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.span.Length;
    }

    public readonly ref byte this[long index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref this.span[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan Slice(long start, long length)
    {
        return new(this.span.Slice(start, length));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigReadOnlySpan Slice(long start)
    {
        return new(this.span.Slice(start));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> ToSpan(long start, int length)
    {
        return this.span.ToSpan(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ReadBool(long offset) => this.span.ReadBool(offset);

    private static void ThrowOutOfRange()
    {
        throw new IndexOutOfRangeException();
    }

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    private static void CheckAlignment(long offset, int size)
    {
#if DEBUG
        if (offset % size != 0)
        {
            FSThrow.InvalidOperation($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
        }
#endif
    }
}