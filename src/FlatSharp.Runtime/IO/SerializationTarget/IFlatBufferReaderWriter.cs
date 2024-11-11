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

using System.Text;

namespace FlatSharp;

/// <summary>
/// Represents a target for a FlatBuffer serialization operation.
/// </summary>
public interface IFlatBufferReaderWriter<out T>
    where T : IFlatBufferReaderWriter<T>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{
    /// <summary>
    /// Gets the length.
    /// </summary>
    long Length { get; }

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start, long length);

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start);
    
    #region Write Methods for Primitives

    void WriteUInt8(long offset, byte value);
    
    byte ReadUInt8(long offset);
    
    void WriteInt8(long offset, sbyte value);
    
    sbyte ReadInt8(long offset);
    
    void WriteInt16(long offset, short value);
    
    short ReadInt16(long offset);
    
    void WriteUInt16(long offset, ushort value);
    
    ushort ReadUInt16(long offset);
    
    void WriteInt32(long offset, int value);
    
    int ReadInt32(long offset);
    
    void WriteUInt32(long offset, uint value);
    
    uint ReadUInt32(long offset);
    
    void WriteInt64(long offset, long value);
    
    long ReadInt64(long offset);
    
    void WriteUInt64(long offset, ulong value);
    
    ulong ReadUInt64(long offset);

    int WriteStringBytes(long offset, string value, Encoding encoding);

    /// <summary>
    /// Copies the given data into this object starting at the given offset.
    /// </summary>
    void CopyFrom(long offset, ReadOnlySpan<byte> data);

    void CopyTo(long offset, Span<byte> destination);

    #endregion
}