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

using System.Text;

namespace FlatSharp;

/// <summary>
/// Defines a span writer.
/// </summary>
public interface ISpanWriter
{
    /// <summary>
    /// Writes the given byte to the span at the given offset.
    /// </summary>
    void WriteByte(Span<byte> span, byte value, int offset);

    /// <summary>
    /// Writes the given double to the span at the given offset.
    /// </summary>
    void WriteDouble(Span<byte> span, double value, int offset);

    /// <summary>
    /// Writes the given float to the span at the given offset.
    /// </summary>
    void WriteFloat(Span<byte> span, float value, int offset);

    /// <summary>
    /// Writes the given int to the span at the given offset.
    /// </summary>
    void WriteInt(Span<byte> span, int value, int offset);

    /// <summary>
    /// Writes the given long to the span at the given offset.
    /// </summary>
    void WriteLong(Span<byte> span, long value, int offset);

    /// <summary>
    /// Writes the given sbyte to the span at the given offset.
    /// </summary>
    void WriteSByte(Span<byte> span, sbyte value, int offset);

    /// <summary>
    /// Writes the given short to the span at the given offset.
    /// </summary>
    void WriteShort(Span<byte> span, short value, int offset);

    /// <summary>
    /// Writes the given uint to the span at the given offset.
    /// </summary>
    void WriteUInt(Span<byte> span, uint value, int offset);

    /// <summary>
    /// Writes the given ulong to the span at the given offset.
    /// </summary>
    void WriteULong(Span<byte> span, ulong value, int offset);

    /// <summary>
    /// Writes the given ushort to the span at the given offset.
    /// </summary>
    void WriteUShort(Span<byte> span, ushort value, int offset);

    /// <summary>
    /// Writes the bytes of the given string to the destination span according to the given encoding.
    /// </summary>
    int GetStringBytes(Span<byte> destination, string value, Encoding encoding);

    /// <summary>
    /// Invokes the <see cref="ISharedStringWriter.FlushWrites{TSpanWriter}(TSpanWriter, Span{byte}, SerializationContext)"/> method.
    /// </summary>
    void FlushSharedStrings(
        ISharedStringWriter writer,
        Span<byte> destination,
        SerializationContext context);
}
