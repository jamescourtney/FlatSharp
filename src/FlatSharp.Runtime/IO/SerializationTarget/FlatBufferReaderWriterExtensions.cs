using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace FlatSharp;

/// <summary>
/// Extensions for IFlatBufferReaderWriter
/// </summary>
public static class FlatBufferReaderWriterExtensions
{
    public static void WriteBool<T>(this T target, long offset, bool value)
        where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target.WriteUInt8(offset, value ? SerializationHelpers.True : SerializationHelpers.False);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFloat32<T>(this T target, long offset, float value)
        where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
        {
            value = value
        };
        
        target.WriteUInt32(offset, floatLayout.bytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat32<T>(this T target, long offset)
        where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
        {
            bytes = target.ReadUInt32(offset),
        };
        
        return floatLayout.value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFloat64<T>(this T target, long offset, double value)
        where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target.WriteInt64(offset, BitConverter.DoubleToInt64Bits(value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadFloat64<T>(this T target, long offset)
        where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        return target.ReadInt64(offset);
    }
    
    public static void WriteReadOnlyByteMemoryBlock<TBuffer>(
        this TBuffer target,
        ReadOnlyMemory<byte> memory,
        long offset,
        SerializationContext ctx) 
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        int numberOfItems = memory.Length;
        long vectorStartOffset = ctx.AllocateVector(itemAlignment: sizeof(byte), numberOfItems, sizePerItem: sizeof(byte));

        target.WriteUOffset(offset, vectorStartOffset);
        target.WriteInt32(vectorStartOffset, numberOfItems);
        target.CopyFrom(vectorStartOffset + sizeof(uint), memory.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UnsafeWriteSpan<TSerializationTarget, TElement>(
        this TSerializationTarget target,
        Span<TElement> buffer,
        long offset,
        int alignment,
        SerializationContext ctx) 
        where TElement : unmanaged
        where TSerializationTarget : IFlatBufferReaderWriter<TSerializationTarget>
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

        target.CopyFrom(
            vectorStartOffset + sizeof(uint),
            MemoryMarshal.Cast<TElement, byte>(buffer));
    }

    /// <summary>
    /// Writes the given string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteString<TBuffer>(
        this TBuffer target,
        string value,
        long offset,
        SerializationContext context) 
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
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
    public static long WriteAndProvisionString<TBuffer>(
        this TBuffer target,
        string value,
        SerializationContext context)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        var encoding = SerializationHelpers.Encoding;

        // Allocate more than we need and then give back what we don't use.
        int maxItems = encoding.GetMaxByteCount(value.Length) + 1;
        long stringStartOffset = context.AllocateVector(sizeof(byte), maxItems, sizeof(byte));
        int bytesWritten = target.WriteStringBytes(
            stringStartOffset + sizeof(int),
            value,
            SerializationHelpers.Encoding);

        // null teriminator
        target.WriteUInt8(stringStartOffset + bytesWritten + sizeof(uint), 0);

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
        where TSerializationTarget : IFlatBufferReaderWriter<TSerializationTarget>
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