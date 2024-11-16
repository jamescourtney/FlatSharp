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

using System.Buffers.Binary;
using System.Runtime.InteropServices;


#if TRUE

public readonly ref struct BigSpan
{
    private readonly nuint length;
    private readonly ref byte value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan(Span<byte> span)
    {
        this.length = (nuint)span.Length;
        this.value = ref span[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BigSpan(ref byte value, nuint length)
    {
        this.value = ref value;
        this.length = length;
    }

    public ref byte this[nuint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index >= this.length)
            {
                ThrowOutOfRange();
            }

            return ref Unsafe.Add(ref this.value, index);
        }
    }

    public nuint Length => this.length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(nuint start, nuint length)
    {
        nuint sum = start + length;
        if (sum < start || sum > this.length)
        {
            ThrowOutOfRange();
        }

        return new BigSpan(ref Unsafe.Add(ref this.value, start), length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(nuint start)
    {
        if (start > this.length)
        {
            ThrowOutOfRange();
        }

        return new BigSpan(ref Unsafe.Add(ref this.value, start), this.length - start);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> ToSpan(nuint start, int length)
    {
        if (length < 0 || (start + (uint)length) > this.length)
        {
            ThrowOutOfRange();
        }

        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref this.value, start),
            length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ReadLittleEndian<T>(nuint offset)
        where T : unmanaged
    {
        var slice = this.ToSpan(offset, Unsafe.SizeOf<T>());
        return Unsafe.ReadUnaligned<T>(ref slice[0]);
    }

    private static void ThrowOutOfRange()
    {
        throw new IndexOutOfRangeException();
    }
}

#endif