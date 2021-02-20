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

namespace FlatSharp
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

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
        /// Reads a shared string at the given offset.
        /// </summary>
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

        /// <summary>
        /// Reads a string from the given uoffset.
        /// </summary>
        public static string ReadStringFromUOffset<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            checked
            {
                int numberOfBytes = (int)buffer.ReadUInt(uoffset);
                return buffer.ReadString(uoffset + sizeof(int), numberOfBytes, SerializationHelpers.Encoding);
            }
        }

        /// <summary>
        /// Reads the given uoffset.
        /// </summary>
        public static int ReadUOffset<TBuffer>(this TBuffer buffer, int offset) where TBuffer : IInputBuffer
        {
            uint uoffset = buffer.ReadUInt(offset);
            if (uoffset < sizeof(uint))
            {
                ThrowUOffsetLessThanMinimumException(uoffset);
            }

            return checked((int)uoffset);
        }

        /// <summary>
        /// Left as no inlining. Literal strings seem to prevent JIT inlining.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowUOffsetLessThanMinimumException(uint uoffset)
        {
            throw new IndexOutOfRangeException($"Decoded uoffset_t had value less than {sizeof(uint)}. Value = {uoffset}");
        }

        /// <summary>
        /// Traverses a vtable to find the absolute offset of a table field.
        /// </summary>
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
                    ThrowInvalidVtableException();
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidVtableException()
        {
            throw new IndexOutOfRangeException("VTable was not long enough to be valid.");
        }

        // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<byte> ReadByteMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            checked
            {
                // The local value stores a uoffset_t, so follow that now.
                uoffset = uoffset + buffer.ReadUOffset(uoffset);
                return buffer.GetByteMemory(uoffset + sizeof(uint), (int)buffer.ReadUInt(uoffset));
            }
        }

        // Seems to break JIT in .NET Core 2.1. Framework 4.7 and Core 3.1 work as expected.
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<byte> ReadByteReadOnlyMemoryBlock<TBuffer>(this TBuffer buffer, int uoffset) where TBuffer : IInputBuffer
        {
            checked
            {
                // The local value stores a uoffset_t, so follow that now.
                uoffset = uoffset + buffer.ReadUOffset(uoffset);
                return buffer.GetReadOnlyByteMemory(uoffset + sizeof(uint), (int)buffer.ReadUInt(uoffset));
            }
        }

        [ExcludeFromCodeCoverage] // Not currently used.
        [Conditional("DEBUG")]
        public static void CheckAlignment<TBuffer>(this TBuffer buffer, int offset, int size) where TBuffer : IInputBuffer
        {
#if DEBUG
            if (offset % size != 0)
            {
                throw new InvalidOperationException($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
            }
#endif
        }
    }
}
