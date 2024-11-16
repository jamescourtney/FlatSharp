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


public readonly ref partial struct BigSpan
{
#if NET7_0_OR_GREATER
    private readonly long length;
    private readonly ref byte value;
#else
    private readonly Span<byte> span;
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan(Span<byte> span)
    {
#if NET7_0_OR_GREATER
        this.length = span.Length;
        this.value = ref span[0];
#else
        this.span = span;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BigSpan(ref byte value, long length)
    {
#if NET7_0_OR_GREATER
        this.value = ref value;
        this.length = length;
#else
        unsafe
        {
            fixed (byte* pByte = &value)
            {
                this.span = new(pByte, checked((int)length));
            }
        }
#endif
    }

    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET7_0_OR_GREATER
        get => this.length;
#else
        get => this.span.Length;
#endif
    }

    public ref byte this[long index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((ulong)index >= (ulong)this.Length)
            {
                ThrowOutOfRange();
            }

#if NET7_0_OR_GREATER
            return ref Unsafe.Add(ref this.value, (IntPtr)index);
#else
            return ref this.span[(int)index];
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(long start, long length)
    {
        if ((length | start) < 0 || start + length > this.Length)
        {
            ThrowOutOfRange();
        }

#if NET7_0_OR_GREATER
        return new BigSpan(ref Unsafe.Add(ref this.value, (IntPtr)start), length);
#else
        return new(this.span.Slice((int)start, (int)length));
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(long start)
    {
        if (start > this.Length)
        {
            ThrowOutOfRange();
        }

#if NET7_0_OR_GREATER
        return new BigSpan(ref Unsafe.Add(ref this.value, (IntPtr)start), this.Length - start);
#else
        return new(this.span.Slice((int)start));
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> ToSpan(long start, int length)
    {
        if (length < 0 || start < 0 || start + length > this.Length)
        {
            ThrowOutOfRange();
        }

#if NET7_0_OR_GREATER
        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref this.value, (IntPtr)start),
            length);
#else
        return this.span.Slice((int)start, length);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBool(long offset, bool value)
    {
        this[offset] = value ? SerializationHelpers.True : SerializationHelpers.False;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ReadBool(long offset) => this[offset] != SerializationHelpers.False;

    public void WriteReadOnlyByteMemoryBlock(
        ReadOnlyMemory<byte> memory,
        long offset,
        SerializationContext ctx)
    {
        int numberOfItems = memory.Length;
        long vectorStartOffset = ctx.AllocateVector(itemAlignment: sizeof(byte), numberOfItems, sizePerItem: sizeof(byte));

        this.WriteUOffset(offset, vectorStartOffset);
        this.WriteInt(vectorStartOffset, numberOfItems);

        memory.Span.CopyTo(this.ToSpan(vectorStartOffset + sizeof(uint), numberOfItems));
    }

    public void UnsafeWriteSpan<TElement>(
        Span<TElement> buffer,
        long offset,
        int alignment,
        SerializationContext ctx)
        where TElement : unmanaged
    {
        // Since we are copying bytes here, only LE is supported.
        FlatSharpInternal.AssertLittleEndian();
        FlatSharpInternal.AssertWellAligned<TElement>(alignment);

        int numberOfItems = buffer.Length;
        long vectorStartOffset = ctx.AllocateVector(
            itemAlignment: alignment,
            numberOfItems,
            sizePerItem: Unsafe.SizeOf<TElement>());

        this.WriteUOffset(offset, vectorStartOffset);
        this.WriteInt(vectorStartOffset, numberOfItems);

        Span<byte> destination = this.ToSpan(
            vectorStartOffset + sizeof(uint),
            checked(numberOfItems * Unsafe.SizeOf<TElement>()));

        MemoryMarshal.Cast<TElement, byte>(buffer).CopyTo(destination);
    }

    /// <summary>
    /// Writes the given string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteString(
        string value,
        long offset,
        SerializationContext context)
    {
        long stringOffset = this.WriteAndProvisionString(value, context);
        this.WriteUOffset(offset, stringOffset);
    }

    /// <summary>
    /// Writes the string to the buffer, returning the absolute offset of the string.
    /// </summary>
    public long WriteAndProvisionString(
        string value,
        SerializationContext context)
    {
        var encoding = SerializationHelpers.Encoding;

        // Allocate more than we need and then give back what we don't use.
        int maxItems = encoding.GetMaxByteCount(value.Length) + 1;
        long stringStartOffset = context.AllocateVector(sizeof(byte), maxItems, sizeof(byte));

        Span<byte> destination = this.ToSpan(stringStartOffset + sizeof(uint), maxItems);

#if NETSTANDARD2_0
        int length = value.Length;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(encoding.GetMaxByteCount(length));
        int bytesWritten = encoding.GetBytes(value, 0, length, buffer, 0);
        buffer.AsSpan().Slice(0, bytesWritten).CopyTo(destination);
        ArrayPool<byte>.Shared.Return(buffer);
#else
        int bytesWritten = encoding.GetBytes(value, destination);
#endif

        // null teriminator
        this[stringStartOffset + bytesWritten + sizeof(uint)] = 0;

        // write length
        this.WriteInt(stringStartOffset, bytesWritten);

        // give back unused space. Account for null terminator.
        context.Offset -= maxItems - (bytesWritten + 1);

        return stringStartOffset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUOffset(
        long offset,
        long secondOffset)
    {
        checked
        {
            uint uoffset = (uint)(secondOffset - offset);
            this.WriteUInt(offset, uoffset);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T ReadUnaligned<T>(long offset)
        where T : unmanaged
    {
        CheckAlignment(offset, Unsafe.SizeOf<T>());
        var slice = this.ToSpan(offset, Unsafe.SizeOf<T>());
        return Unsafe.ReadUnaligned<T>(ref slice[0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteUnaligned<T>(long offset, T value)
        where T : unmanaged
    {
        CheckAlignment(offset, Unsafe.SizeOf<T>());
        var slice = this.ToSpan(offset, Unsafe.SizeOf<T>());
        Unsafe.WriteUnaligned(ref slice[0], value);
    }

    private static double ReverseEndianness(double value)
    {
        long longValue = Unsafe.As<double, long>(ref value);
        longValue = BinaryPrimitives.ReverseEndianness(longValue);
        return BitConverter.Int64BitsToDouble(longValue);
    }

    private static float ReverseEndianness(float value)
    {
        uint intValue = Unsafe.As<float, uint>(ref value);
        intValue = BinaryPrimitives.ReverseEndianness(intValue);

        ScalarSpanReader.FloatLayout f = new()
        {
            bytes = intValue
        };

        return f.value;
    }

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