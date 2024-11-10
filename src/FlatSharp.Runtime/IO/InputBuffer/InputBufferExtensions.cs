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

using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

/// <summary>
/// Extensions for input buffers.
/// </summary>
public static class InputBufferExtensions
{
    /// <summary>
    /// Reads a bool.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBool<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadBool(offset);
        }

        return buffer.ReadByte(offset) != SerializationHelpers.False;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadByte(offset);
        }

        return buffer.GetReadOnlySpan(offset, sizeof(byte))[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ReadSByte<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadSByte(offset);
        }

        return (sbyte)buffer.ReadByte(offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(ushort));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadUShort(offset);
        }

        return BinaryPrimitives.ReadUInt16LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(ushort)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadShort<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(short));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadShort(offset);
        }

        return BinaryPrimitives.ReadInt16LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(short)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(uint));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadUInt(offset);
        }

        return BinaryPrimitives.ReadUInt32LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(uint)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(int));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadInt(offset);
        }

        return BinaryPrimitives.ReadInt32LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(int)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadULong<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(ulong));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadULong(offset);
        }

        return BinaryPrimitives.ReadUInt64LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(ulong)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadLong<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(long));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadLong(offset);
        }

        return BinaryPrimitives.ReadInt64LittleEndian(buffer.GetReadOnlySpan(offset, sizeof(long)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(float));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadFloat(offset);
        }

#if NETSTANDARD
        ScalarSpanReader.FloatLayout layout = new()
        {
            bytes = buffer.ReadUInt(offset)
        };

        return layout.value;
#else
        return BinaryPrimitives.ReadSingleLittleEndian(buffer.GetReadOnlySpan(offset, sizeof(float)));
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        buffer.CheckAlignment(offset, sizeof(double));
        if (buffer is IInputBufferFull)
        {
            return ((IInputBufferFull)buffer).ReadDouble(offset);
        }

#if NETSTANDARD
        return BitConverter.Int64BitsToDouble(buffer.ReadLong(offset));
#else
        return BinaryPrimitives.ReadDoubleLittleEndian(buffer.GetReadOnlySpan(offset, sizeof(double)));
#endif
    }

    /// <summary>
    /// Reads a string at the given offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadString<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        checked
        {
            // Strings are stored by reference.
            offset += buffer.ReadUOffset(offset);
            return buffer.ReadStringFromUOffset(offset);
        }
    }

    /// <summary>
    /// Reads a string from the given uoffset.
    /// </summary>
    public static string ReadStringFromUOffset<TBuffer>(this TBuffer buffer, long stringStart)
        where TBuffer : IInputBuffer
    {
        int numberOfBytes = (int)buffer.ReadUInt(stringStart);
        ReadOnlySpan<byte> stringValue = buffer.GetReadOnlySpan(stringStart + sizeof(int), numberOfBytes);

#if NETSTANDARD2_0
        byte[] temp = ArrayPool<byte>.Shared.Rent(numberOfBytes);
        stringValue.CopyTo(temp);
        string result = SerializationHelpers.Encoding.GetString(temp, 0, numberOfBytes);
        ArrayPool<byte>.Shared.Return(temp);
        return result;
#else
        return SerializationHelpers.Encoding.GetString(stringValue);
#endif
    }

    /// <summary>
    /// Reads the given uoffset.
    /// </summary>
    public static int ReadUOffset<TBuffer>(this TBuffer buffer, long offset) where TBuffer : IInputBuffer
    {
        int uoffset = buffer.ReadInt(offset);
        if (uoffset < sizeof(uint))
        {
            FSThrow.InvalidData_UOffsetTooSmall((uint)uoffset);
        }

        return uoffset;
    }

    /// <summary>
    /// Validates a vtable and reads the initial bytes of a vtable.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitializeVTable<TBuffer>(
        this TBuffer buffer,
        long tableOffset,
        out long vtableOffset,
        out ulong vtableFieldCount,
        out ReadOnlySpan<byte> fieldData) where TBuffer : IInputBuffer
    {
        vtableOffset = tableOffset - buffer.ReadInt(tableOffset);
        ushort vtableLength = buffer.ReadUShort(vtableOffset);

        if (vtableLength < 4)
        {
            FSThrow.InvalidData_VTableTooShort();
        }

        fieldData = buffer.GetReadOnlySpan(vtableOffset, vtableLength).Slice(4);
        vtableFieldCount = (ulong)(fieldData.Length / 2);
    }

    // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<byte> ReadByteMemoryBlock<TBuffer>(this TBuffer buffer, long uoffset)
        where TBuffer : IInputBuffer
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset += buffer.ReadUOffset(uoffset);
        return buffer.GetMemory(uoffset + sizeof(int), buffer.ReadInt(uoffset));
    }

    // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<byte> ReadByteReadOnlyMemoryBlock<TBuffer>(this TBuffer buffer, long uoffset)
        where TBuffer : IInputBuffer
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset += buffer.ReadUOffset(uoffset);
        return buffer.GetReadOnlyMemory(uoffset + sizeof(uint), buffer.ReadInt(uoffset));
    }

    /// <summary>
    /// Reads a sequence of TElement items from the buffer at the given offset using the equivalent of reinterpret_cast.
    /// </summary>
    public static Span<TElement> UnsafeReadSpan<TBuffer, TElement>(this TBuffer buffer, long uoffset)
        where TBuffer : IInputBuffer 
        where TElement : unmanaged
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset = uoffset + buffer.ReadUOffset(uoffset);

        // We need to construct a Span<TElement> from byte buffer that:
        // 1. starts at correct offset for vector data
        // 2. has a length based on *TElement* count not *byte* count
        var byteSpanAtDataOffset = buffer.GetSpan(
            uoffset + sizeof(uint), 
            checked(Unsafe.SizeOf<TElement>() * (int)buffer.ReadUInt(uoffset)));
        
        var sourceSpan = MemoryMarshal.Cast<byte, TElement>(byteSpanAtDataOffset);

        return sourceSpan;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static long CopyTo<TBuffer, TTarget>(this TBuffer buffer, TTarget target)
        where TBuffer : IInputBuffer
        where TTarget : IFlatBufferSerializationTarget<TTarget>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        if (target.Length < buffer.Length)
        {
            FSThrow.BufferTooSmall(buffer.Length);
            return 0;
        }

        long offset = 0;
        while (offset < buffer.Length)
        {
            long remaining = buffer.Length - offset;
            var chunk = buffer.GetReadOnlySpan(offset, (int)Math.Min(int.MaxValue, remaining));
            chunk.CopyTo(target.AsSpan(offset, chunk.Length));

            offset += chunk.Length;
        }

        return offset;
    }

    [ExcludeFromCodeCoverage] // Not currently used.
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckAlignment<TBuffer>(this TBuffer buffer, long offset, int size) where TBuffer : IInputBuffer
    {
#if DEBUG
        if (offset % size != 0)
        {
            FSThrow.InvalidOperation(
                $"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
        }
#endif
    }
}