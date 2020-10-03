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
    using System.Buffers.Binary;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Utility class for writing items to spans.
    /// </summary>
    public struct SpanWriter : ISpanWriter
    {
        /// <summary>
        /// A default instance. Spanwriter is stateless and threadsafe.
        /// </summary>
        public static SpanWriter Instance => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context)
        {
            span[offset] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context)
        {
            span[offset] = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteShort(Span<byte> span, short value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(short));
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(uint));
            BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt(Span<byte> span, int value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(int));
            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            BinaryPrimitives.WriteUInt64LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLong(Span<byte> span, long value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(long));
            BinaryPrimitives.WriteInt64LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context)
        {
            ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
            {
                value = value
            };

            this.WriteUInt(span, floatLayout.bytes, offset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context)
        {
            this.WriteLong(span, BitConverter.DoubleToInt64Bits(value), offset, context);
        }

        public int GetStringBytes(Span<byte> destination, string value, Encoding encoding)
        {
#if NETCOREAPP
            int bytesWritten = encoding.GetBytes(value, destination);
#else
            var bytes = encoding.GetBytes(value);
            bytes.CopyTo(destination);
            int bytesWritten = bytes.Length;
#endif

            return bytesWritten;
        }
    }

    public static class SpanWriterExtensions
    {
        public static void WriteReadOnlyByteMemoryBlock<TSpanWriter>(
            this TSpanWriter spanWriter,
            Span<byte> span,
            ReadOnlyMemory<byte> memory,
            int offset,
            SerializationContext ctx) where TSpanWriter : ISpanWriter
        {
            int numberOfItems = memory.Length;
            int vectorStartOffset = ctx.AllocateVector(itemAlignment: sizeof(byte), numberOfItems, sizePerItem: sizeof(byte));

            spanWriter.WriteUOffset(span, offset, vectorStartOffset, ctx);
            spanWriter.WriteInt(span, numberOfItems, vectorStartOffset, ctx);

            memory.Span.CopyTo(span.Slice(vectorStartOffset + sizeof(uint)));
        }

        /// <summary>
        /// Writes the given string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString<TSpanWriter>(
            this TSpanWriter spanWriter,
            Span<byte> span, 
            string value, 
            int offset, 
            SerializationContext context) where TSpanWriter : ISpanWriter
        {
            int stringOffset = spanWriter.WriteAndProvisionString(span, value, context);
            spanWriter.WriteUOffset(span, offset, stringOffset, context);
        }

        /// <summary>
        /// Writes the given shared string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSharedString<TSpanWriter>(
            this TSpanWriter writer, 
            Span<byte> span, 
            SharedString value, 
            int offset, 
            SerializationContext context) where TSpanWriter : ISpanWriter
        {
            var manager = context.SharedStringWriter;
            if (manager != null)
            {
                manager.WriteSharedString(writer, span, offset, value, context);
            }
            else
            {
                writer.WriteString(span, value, offset, context);
            }
        }

        /// <summary>
        /// Writes the string to the buffer, returning the absolute offset of the string.
        /// </summary>
        public static int WriteAndProvisionString<TSpanWriter>(this TSpanWriter spanWriter, Span<byte> span, string value, SerializationContext context)
            where TSpanWriter : ISpanWriter
        {
            checked
            {
                var encoding = InputBuffer.Encoding;

                // Allocate more than we need and then give back what we don't use.
                int maxItems = encoding.GetMaxByteCount(value.Length) + 1;
                int stringStartOffset = context.AllocateVector(sizeof(byte), maxItems, sizeof(byte));

                int bytesWritten = spanWriter.GetStringBytes(span.Slice(stringStartOffset + sizeof(uint), maxItems), value, encoding);

                // null teriminator
                span[stringStartOffset + bytesWritten + sizeof(uint)] = 0;

                // write length
                spanWriter.WriteInt(span, bytesWritten, stringStartOffset, context);

                // give back unused space. Account for null terminator.
                context.Offset -= maxItems - (bytesWritten + 1);

                return stringStartOffset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUOffset<TSpanWriter>(this TSpanWriter spanWriter, Span<byte> span, int offset, int secondOffset, SerializationContext context)
            where TSpanWriter : ISpanWriter
        {
            checked
            {
                uint uoffset = (uint)(secondOffset - offset);
                spanWriter.WriteUInt(span, uoffset, offset, context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBool<TSpanWriter>(this TSpanWriter spanWriter, Span<byte> span, bool b, int offset, SerializationContext context) 
            where TSpanWriter : ISpanWriter
        {
            spanWriter.WriteByte(span, b ? InputBuffer.True : InputBuffer.False, offset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void CheckAlignment<TSpanWriter>(this TSpanWriter spanWriter, int offset, int size) where TSpanWriter : ISpanWriter
        {
            if (offset % size != 0)
            {
                throw new InvalidOperationException($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
            }
        }
    }
}