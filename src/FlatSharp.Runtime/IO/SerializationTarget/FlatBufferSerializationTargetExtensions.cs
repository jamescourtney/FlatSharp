using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace FlatSharp;

/// <summary>
/// Extensions for IFlatBufferSerializationTarget
/// </summary>
public static class FlatBufferSerializationTargetExtensions
{
    public static void WriteBool<T>(this T target, long offset, bool value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target.WriteUInt8(offset, value ? SerializationHelpers.True : SerializationHelpers.False);
    }
    
    public static void WriteUInt8<T>(this T target, long offset, byte value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target[offset] = value;
    }
    
    public static void WriteInt8<T>(this T target, long offset, sbyte value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target[offset] = unchecked((byte)value);
    }
    
    public static void WriteUInt16<T>(this T target, long offset, ushort value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(ushort));
        BinaryPrimitives.WriteUInt16LittleEndian(
            target.AsSpan(offset, sizeof(ushort)),
            value);
    }
    
    public static void WriteInt16<T>(this T target, long offset, short value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(short));
        BinaryPrimitives.WriteInt16LittleEndian(
            target.AsSpan(offset, sizeof(short)),
            value);
    }
    
    public static void WriteUInt32<T>(this T target, long offset, uint value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(uint));
        BinaryPrimitives.WriteUInt32LittleEndian(
            target.AsSpan(offset, sizeof(uint)),
            value);
    }
    
    public static void WriteInt32<T>(this T target, long offset, int value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(
            target.AsSpan(offset, sizeof(int)),
            value);
    }
    
    public static void WriteUInt64<T>(this T target, long offset, ulong value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(ulong));
        BinaryPrimitives.WriteUInt64LittleEndian(
            target.AsSpan(offset, sizeof(ulong)),
            value);
    }
    
    public static void WriteInt64<T>(this T target, long offset, long value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(long));
        BinaryPrimitives.WriteInt64LittleEndian(
            target.AsSpan(offset, sizeof(long)),
            value);
    }
    
    
    public static void WriteFloat32<T>(this T target, long offset, float value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(float));
        
#if NETSTANDARD
        ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
        {
            value = value
        };
        
        target.WriteUInt32(offset, floatLayout.bytes);
#else
        BinaryPrimitives.WriteSingleLittleEndian(
            target.AsSpan(offset, sizeof(float)),
            value);
#endif
    }
    
    public static void WriteFloat64<T>(this T target, long offset, double value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        CheckAlignment(offset, sizeof(double));
        
#if NETSTANDARD
        target.WriteInt64(offset, BitConverter.DoubleToInt64Bits(value));
#else
        BinaryPrimitives.WriteDoubleLittleEndian(
            target.AsSpan(offset, sizeof(double)),
            value);  
#endif   
    }
    
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
        where TElement : unmanaged
        where TSerializationTarget : IFlatBufferSerializationTarget<TSerializationTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct 
    #endif
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