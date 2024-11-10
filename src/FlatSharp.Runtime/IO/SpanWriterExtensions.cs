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

using System.Buffers;
using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

/// <summary>
/// Extension methods that apply to all <see cref="ISpanWriter"/> implementations.
/// </summary>
public static class SpanWriterExtensions
{
    public static void WriteReadOnlyByteMemoryBlock<TTarget>(
        this TTarget target,
        ReadOnlyMemory<byte> memory,
        long offset,
        SerializationContext ctx) 
        where TTarget : IFlatBufferSerializationTarget<TTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        int numberOfItems = memory.Length;
        long vectorStartOffset = ctx.AllocateVector(itemAlignment: sizeof(byte), numberOfItems, sizePerItem: sizeof(byte));

        target.WriteUOffset(offset, vectorStartOffset);
        target.WriteInt32(vectorStartOffset, numberOfItems);

        memory.Span.CopyTo(target.AsSpan(vectorStartOffset + sizeof(uint), numberOfItems));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UnsafeWriteSpan<TSerializationTarget, TElement>(
        this TSerializationTarget target,
        Span<TElement> buffer,
        long offset,
        int alignment,
        SerializationContext ctx) 
        where TSerializationTarget : IFlatBufferSerializationTarget<TSerializationTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct 
    #endif
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

        target.WriteUOffset(offset, vectorStartOffset);
        target.WriteInt32(vectorStartOffset, numberOfItems);

        Span<byte> destination = target.AsSpan(
            vectorStartOffset + sizeof(uint),
            checked(numberOfItems * Unsafe.SizeOf<TElement>()));

        MemoryMarshal.Cast<TElement, byte>(buffer).CopyTo(destination);
    }

    /// <summary>
    /// Writes the given string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteString<TTarget>(
        this TTarget target,
        string value,
        long offset,
        SerializationContext context) 
        where TTarget : IFlatBufferSerializationTarget<TTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        long stringOffset = target.WriteAndProvisionString(value, context);
        target.WriteUOffset(offset, stringOffset);
    }

    /// <summary>
    /// Writes the string to the buffer, returning the absolute offset of the string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long WriteAndProvisionString<TTarget>(
        this TTarget target,
        string value,
        SerializationContext context)
        where TTarget : IFlatBufferSerializationTarget<TTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        var encoding = SerializationHelpers.Encoding;

        // Allocate more than we need and then give back what we don't use.
        int maxItems = encoding.GetMaxByteCount(value.Length) + 1;
        long stringStartOffset = context.AllocateVector(sizeof(byte), maxItems, sizeof(byte));

        Span<byte> destination = target.AsSpan(stringStartOffset + sizeof(uint), maxItems);

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
        target[stringStartOffset + bytesWritten + sizeof(uint)] = 0;

        // write length
        target.WriteInt32(stringStartOffset, bytesWritten);

        // give back unused space. Account for null terminator.
        context.Offset -= maxItems - (bytesWritten + 1);

        return stringStartOffset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUOffset<TSerializationTarget>(
        this TSerializationTarget target,
        long offset,
        long secondOffset)
        where TSerializationTarget : IFlatBufferSerializationTarget<TSerializationTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        checked
        {
            uint uoffset = (uint)(secondOffset - offset);
            target.WriteUInt32(offset, uoffset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteBool<TSpanWriter>(this TSpanWriter spanWriter, Span<byte> span, bool b, int offset)
        where TSpanWriter : ISpanWriter
    {
        spanWriter.WriteByte(span, b ? SerializationHelpers.True : SerializationHelpers.False, offset);
    }

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    public static void CheckAlignment<TSpanWriter>(this TSpanWriter spanWriter, long offset, int size) where TSpanWriter : ISpanWriter
    {
#if DEBUG
        if (offset % size != 0)
        {
            FSThrow.InvalidOperation($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
        }
#endif
    }
}
