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
    public static bool ReadBool<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
    {
        return buffer.ReadByte(offset) != SerializationHelpers.False;
    }

    /// <summary>
    /// Reads a string at the given offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadString<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
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
    public static string ReadStringFromUOffset<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
    {
        int numberOfBytes = (int)buffer.ReadUInt(uoffset);
        return buffer.ReadString(uoffset + sizeof(int), numberOfBytes, SerializationHelpers.Encoding);
    }

    /// <summary>
    /// Reads the given uoffset.
    /// </summary>
    public static int ReadUOffset<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
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
        int tableOffset,
        out int vtableOffset,
        out nuint vtableFieldCount,
        out ReadOnlySpan<byte> fieldData) where TBuffer : IInputBuffer
    {
        vtableOffset = tableOffset - buffer.ReadInt(tableOffset);
        ushort vtableLength = buffer.ReadUShort(vtableOffset);

        if (vtableLength < 4)
        {
            FSThrow.InvalidData_VTableTooShort();
        }

        fieldData = buffer.GetReadOnlySpan().Slice(vtableOffset, vtableLength).Slice(4);
        vtableFieldCount = (nuint)fieldData.Length / 2;
    }

    // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<byte> ReadByteMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset = uoffset + buffer.ReadUOffset(uoffset);
        return buffer.GetMemory().Slice(uoffset + sizeof(uint), (int)buffer.ReadUInt(uoffset));
    }

    // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<byte> ReadByteReadOnlyMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset = uoffset + buffer.ReadUOffset(uoffset);
        return buffer.GetReadOnlyMemory().Slice(uoffset + sizeof(uint), (int)buffer.ReadUInt(uoffset));
    }
    
    /// <summary>
    /// Reads a sequence of TElement items from the buffer at the given offset using the equivalent of reinterpret_cast.
    /// </summary>
    public static Span<TElement> UnsafeReadSpan<TBuffer, TElement>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer where TElement : struct
    {
        // The local value stores a uoffset_t, so follow that now.
        uoffset = uoffset + buffer.ReadUOffset(uoffset);

        // We need to construct a Span<TElement> from byte buffer that:
        // 1. starts at correct offset for vector data
        // 2. has a length based on *TElement* count not *byte* count
        var byteSpanAtDataOffset = buffer.GetSpan().Slice(uoffset + sizeof(uint));
        var sourceSpan = MemoryMarshal.Cast<byte, TElement>(byteSpanAtDataOffset).Slice(0, (int)buffer.ReadUInt(uoffset));

        return sourceSpan;
    }

    [ExcludeFromCodeCoverage] // Not currently used.
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckAlignment<TBuffer>(this TBuffer buffer, int offset, int size) where TBuffer : IInputBuffer
    {
#if DEBUG
        if (offset % size != 0)
        {
            FSThrow.InvalidOperation($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
        }
#endif
    }
}
