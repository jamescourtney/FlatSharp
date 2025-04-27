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
    private readonly long length;
    private readonly ref byte value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan(Span<byte> span)
    {
        this.length = span.Length;
        this.value = ref span[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan(ref byte value, long length)
    {
        this.value = ref value;
        this.length = length;
    }

    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.length;
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

            return ref Unsafe.Add(ref this.value, (IntPtr)index);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(long start, long length)
    {
        bool isOutOfRange = (length | start) < 0;
        isOutOfRange |= (ulong)(start + length) > (ulong)this.Length;

        if (isOutOfRange)
        {
            ThrowOutOfRange();
        }

        return new BigSpan(ref Unsafe.Add(ref this.value, (IntPtr)start), length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigSpan Slice(long start)
    {
        if ((ulong)start > (ulong)this.Length)
        {
            ThrowOutOfRange();
        }

        return new BigSpan(ref Unsafe.Add(ref this.value, (IntPtr)start), this.Length - start);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> ToSpan(long start, int length)
    {
        this.CheckRange(start, length);

        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref this.value, (IntPtr)start),
            length);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long WriteAndProvisionString(
        string value,
        SerializationContext context)
    {
        var encoding = SerializationHelpers.Encoding;

        // Allocate more than we need and then give back what we don't use.
        int maxItems = encoding.GetMaxByteCount(value.Length) + 1;
        long stringStartOffset = context.AllocateVector(sizeof(byte), maxItems, sizeof(byte));

        BigSpan scopedThis = this.Slice(stringStartOffset, maxItems + sizeof(uint));
        Span<byte> destination = scopedThis.ToSpan(sizeof(uint), maxItems);

        int bytesWritten = encoding.GetBytes(value, destination);

        // null teriminator
        scopedThis.UnsafeWriteByte(sizeof(uint) + bytesWritten, 0);

        // write length
        scopedThis.UnsafeWriteInt(0, bytesWritten);

        // give back unused space. Account for null terminator.
        context.Offset -= maxItems - (bytesWritten + 1);

        return stringStartOffset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUOffset(
        long offset,
        long secondOffset)
    {
        long difference = secondOffset - offset;
        uint uoffset = checked((uint)difference);
        this.WriteUInt(offset, uoffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T ReadUnaligned<T>(long offset)
        where T : unmanaged
    {
        this.CheckRange(offset, Unsafe.SizeOf<T>());
        return this.ReadUnalignedUnsafe<T>(offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteUnaligned<T>(long offset, T value)
        where T : unmanaged
    {
        this.CheckRange(offset, Unsafe.SizeOf<T>());
        this.WriteUnalignedUnsafe(offset, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal T ReadUnalignedUnsafe<T>(long offset)
        where T : unmanaged
    {
#if DEBUG
        CheckAlignment(offset, Unsafe.SizeOf<T>());
        CheckRange(offset, Unsafe.SizeOf<T>());
#endif

        return Unsafe.ReadUnaligned<T>(ref Unsafe.Add(ref this.value, (IntPtr)offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void WriteUnalignedUnsafe<T>(long offset, T value)
        where T : unmanaged
    {
#if DEBUG
        CheckAlignment(offset, Unsafe.SizeOf<T>());
        CheckRange(offset, Unsafe.SizeOf<T>());
#endif

        Unsafe.WriteUnaligned(ref Unsafe.Add(ref this.value, (IntPtr)offset), value);
    }

    internal static double ReverseEndianness(double value)
    {
        long longValue = Unsafe.As<double, long>(ref value);
        longValue = BinaryPrimitives.ReverseEndianness(longValue);
        return BitConverter.Int64BitsToDouble(longValue);
    }

    internal static float ReverseEndianness(float value)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckRange(long start, int length)
    {
        long sum = start + (long)length;
        if ((ulong)sum > (ulong)this.Length)
        {
            ThrowOutOfRange();
        }
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