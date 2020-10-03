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

namespace FlatSharp
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public interface IInputBuffer
    {
        ISharedStringReader SharedStringReader { get; set; }

        int Length { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        byte ReadByte(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sbyte ReadSByte(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ushort ReadUShort(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        short ReadShort(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        uint ReadUInt(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int ReadInt(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ulong ReadULong(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long ReadLong(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float ReadFloat(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        double ReadDouble(int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string ReadString(int offset, int byteLength, Encoding encoding);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Memory<byte> GetByteMemory(int start, int length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length);
    }

    public static class InputBufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
        {
            return buffer.ReadByte(offset) != InputBuffer.False;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SharedString ReadSharedString<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
        {
            checked
            {
                var reader = buffer.SharedStringReader;
                if (reader != null)
                {
                    int uoffset = offset + buffer.ReadUOffset(offset);
                    return reader.ReadSharedString(buffer, uoffset);
                }
                else
                {
                    return buffer.ReadString(offset);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadStringFromUOffset<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            checked
            {
                int numberOfBytes = (int)buffer.ReadUInt(uoffset);
                return buffer.ReadString(uoffset + sizeof(int), numberOfBytes, InputBuffer.Encoding);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUOffset<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
        {
            uint uoffset = buffer.ReadUInt(offset);
            if (uoffset < sizeof(uint))
            {
                ThrowUOffsetLessThanMinimumException(uoffset);
            }

            return checked((int)uoffset);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowUOffsetLessThanMinimumException(uint uoffset)
        {
            throw new IndexOutOfRangeException($"Decoded uoffset_t had value less than {sizeof(uint)}. Value = {uoffset}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAbsoluteTableFieldLocation<TBuffer>(this TBuffer buffer, int tableOffset, int index) where TBuffer : IInputBuffer
        {
            checked
            {
                int vtableOffset = tableOffset - buffer.ReadInt(tableOffset);
                int vtableLength = buffer.ReadUShort(vtableOffset);

                // VTable structure:
                // ushort: vtable length
                // ushort: table length
                // ushort: index 0 offset
                // ushort: index 1 offset
                // etc
                if (vtableLength < 4)
                {
                    throw new IndexOutOfRangeException("VTable was not long enough to be valid.");
                }

                // the max index is ((vtableLength - 4) / 2) - 1
                if (index >= (vtableLength - 4) / 2)
                {
                    // Not present, return 0. 0 is an indication that that field is not present.
                    return 0;
                }

                ushort relativeOffset = buffer.ReadUShort(vtableOffset + 2 * (2 + index));
                if (relativeOffset == 0)
                {
                    return 0;
                }

                return tableOffset + relativeOffset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<byte> ReadByteMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            return buffer.ReadByteMemoryBlockImpl(
                uoffset,
                buffer.GetByteMemory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<byte> ReadByteReadOnlyMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            return buffer.ReadByteMemoryBlockImpl(
                uoffset,
                buffer.GetReadOnlyByteMemory);
        }

        private static T ReadByteMemoryBlockImpl<T, TBuffer>(this TBuffer buffer, int uoffset, Func<int, int, T> callback) where TBuffer : IInputBuffer
        {
            checked
            {
                // The local value stores a uoffset_t, so follow that now.
                uoffset = uoffset + buffer.ReadUOffset(uoffset);

                // Skip the first 4 bytes of the vector, which contains the length.
                return callback(
                    uoffset + sizeof(uint),
                    (int)buffer.ReadUInt(uoffset));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void CheckAlignment<TBuffer>(this TBuffer buffer, int offset, int size) where TBuffer : IInputBuffer
        {
            if (offset % size != 0)
            {
                throw new InvalidOperationException($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
            }
        }
    }

    /// <summary>
    /// A buffer for reading from memory.
    /// </summary>
    public static class InputBuffer
    {
        internal static readonly Encoding Encoding = new UTF8Encoding(false);
        internal const byte True = 1;
        internal const byte False = 0;
    }
}
