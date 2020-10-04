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
    using System.Text;

    /// <summary>
    /// Defines a buffer that FlatSharp can parse from. Implementations will be fastest when using a struct.
    /// </summary>
    public interface IInputBuffer
    {
        /// <summary>
        /// Gets or sets the SharedStringReader for this buffer.
        /// </summary>
        ISharedStringReader SharedStringReader { get; set; }

        /// <summary>
        /// Gets the length of this input buffer.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Reads the byte at the given offset.
        /// </summary>
        byte ReadByte(int offset);

        /// <summary>
        /// Reads the sbyte at the given offset.
        /// </summary>
        sbyte ReadSByte(int offset);

        /// <summary>
        /// Reads the ushort at the given offset.
        /// </summary>
        ushort ReadUShort(int offset);

        /// <summary>
        /// Reads the short at the given offset.
        /// </summary>
        short ReadShort(int offset);

        /// <summary>
        /// Reads the uint at the given offset.
        /// </summary>
        uint ReadUInt(int offset);

        /// <summary>
        /// Reads the int at the given offset.
        /// </summary>
        int ReadInt(int offset);

        /// <summary>
        /// Reads the ulong at the given offset.
        /// </summary>
        ulong ReadULong(int offset);

        /// <summary>
        /// Reads the long at the given offset.
        /// </summary>
        long ReadLong(int offset);

        /// <summary>
        /// Reads the float at the given offset.
        /// </summary>
        float ReadFloat(int offset);

        /// <summary>
        /// Reads the double at the given offset.
        /// </summary>
        double ReadDouble(int offset);

        /// <summary>
        /// Reads the string of the given length at the given offset with the given encoding.
        /// </summary>
        string ReadString(int offset, int byteLength, Encoding encoding);

        /// <summary>
        /// Reads the byte memory at the given offset with the given length.
        /// </summary>
        Memory<byte> GetByteMemory(int start, int length);

        /// <summary>
        /// Reads the read only byte memory at the given offset with the given length.
        /// </summary>
        ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length);

        /// <summary>
        /// Invokes the parse method on the <see cref="IGeneratedSerializer{T}"/> parameter. Allows passing
        /// generic parameters.
        /// </summary>
        TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset);
    }
}
