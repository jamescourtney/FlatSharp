﻿/*
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
    public class SpanWriter
    {
        /// <summary>
        /// A default instance. Spanwriter is stateless and threadsafe.
        /// </summary>
        public static SpanWriter Instance { get; } = new SpanWriter();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUOffset(Span<byte> span, int offset, int secondOffset, SerializationContext context)
        {
            checked
            {
                uint uoffset = (uint)(secondOffset - offset);
                this.WriteUInt(span, uoffset, offset, context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBool(Span<byte> span, bool b, int offset, SerializationContext context)
        {
            this.WriteByte(span, b ? InputBuffer.True : InputBuffer.False, offset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context)
        {
            span[offset] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context)
        {
            span[offset] = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(ushort));
            BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteShort(Span<byte> span, short value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(short));
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(uint));
            BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteInt(Span<byte> span, int value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(int));
            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(ulong));
            BinaryPrimitives.WriteUInt64LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteLong(Span<byte> span, long value, int offset, SerializationContext context)
        {
            CheckAlignment(offset, sizeof(long));
            BinaryPrimitives.WriteInt64LittleEndian(span.Slice(offset), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context)
        {
            ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
            {
                value = value
            };

            this.WriteUInt(span, floatLayout.bytes, offset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context)
        {
            this.WriteLong(span, BitConverter.DoubleToInt64Bits(value), offset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteString(Span<byte> span, string value, int offset, SerializationContext context)
        {
            int stringOffset = this.WriteAndProvisionString(span, value, context);
            this.WriteUOffset(span, offset, stringOffset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteSharedString(Span<byte> span, SharedString value, int offset, SerializationContext context)
        {
            var manager = context.SharedStringWriter;
            if (manager != null)
            {
                manager.WriteSharedString(this, span, offset, value, context);
            }
            else
            {
                this.WriteString(span, value, offset, context);
            }
        }

        /// <summary>
        /// Writes the string to the buffer, returning the absolute offset of the string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteAndProvisionString(Span<byte> span, string value, SerializationContext context)
        {
            checked
            {
                var encoding = InputBuffer.Encoding;

                // Allocate more than we need and then give back what we don't use.
                int maxItems = encoding.GetMaxByteCount(value.Length);
                int stringStartOffset = context.AllocateVector(sizeof(byte), maxItems + 1, sizeof(byte));

                int bytesWritten = this.WriteStringProtected(span.Slice(stringStartOffset + sizeof(uint), maxItems), value, encoding);

                // null teriminator
                span[stringStartOffset + bytesWritten + sizeof(uint)] = 0;

                // write length
                this.WriteInt(span, bytesWritten, stringStartOffset, context);

                // give back unused space.
                context.Offset -= maxItems - bytesWritten;

                return stringStartOffset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual int WriteStringProtected(Span<byte> span, string value, Encoding encoding)
        {
#if NETCOREAPP
            return encoding.GetBytes(value, span);
#else
            var bytes = encoding.GetBytes(value);
            bytes.CopyTo(span);
            return bytes.Length;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteReadOnlyByteMemoryBlock(
            Span<byte> span,
            ReadOnlyMemory<byte> memory,
            int offset, 
            int alignment, 
            int inlineSize, 
            SerializationContext ctx)
        {
            Debug.Assert(alignment == 1);
            Debug.Assert(inlineSize == 1);

            int numberOfItems = memory.Length;
            int vectorStartOffset = ctx.AllocateVector(alignment, numberOfItems, inlineSize);

            this.WriteUOffset(span, offset, vectorStartOffset, ctx);
            this.WriteInt(span, numberOfItems, vectorStartOffset, ctx);

            memory.Span.CopyTo(span.Slice(vectorStartOffset + sizeof(uint)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteReadOnlyMemoryBlock<T>(Span<byte> span, ReadOnlyMemory<T> memory, int offset, int alignment, int inlineSize, SerializationContext ctx) where T : struct
        {
            Debug.Assert(alignment == inlineSize);

            int numberOfItems = memory.Length;
            int vectorStartOffset = ctx.AllocateVector(alignment, numberOfItems, inlineSize);

            this.WriteUOffset(span, offset, vectorStartOffset, ctx);
            this.WriteInt(span, numberOfItems, vectorStartOffset, ctx);
            MemoryMarshal.Cast<T, byte>(memory.Span).CopyTo(span.Slice(vectorStartOffset + sizeof(uint)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        protected static void CheckAlignment(int offset, int size)
        {
            if (offset % size != 0)
            {
                throw new InvalidOperationException($"BugCheck: attempted to read unaligned data at index: {offset}, expected alignment: {size}");
            }
        }
    }
}