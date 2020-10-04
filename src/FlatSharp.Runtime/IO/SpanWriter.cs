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
    using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        public void InvokeWrite<TItemType>(IGeneratedSerializer<TItemType> serializer, Span<byte> destination, TItemType item, int offset, SerializationContext context)
        {
            serializer.Write(
                this,
                destination,
                item,
                offset,
                context);
        }

        public void FlushSharedStrings(ISharedStringWriter writer, Span<byte> destination, SerializationContext context)
        {
            writer.FlushWrites(this, destination, context);
        }
    }
}