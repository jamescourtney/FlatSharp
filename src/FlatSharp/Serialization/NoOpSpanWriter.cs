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
    using System.Runtime.CompilerServices;
    using System.Text;

    internal sealed class NoOpSpanWriter : SpanWriter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteInt(Span<byte> span, int value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteLong(Span<byte> span, long value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteShort(Span<byte> span, short value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SpanCopy(ReadOnlySpan<byte> source, Span<byte> destination)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Span<byte> SliceSpan(Span<byte> span, int start)
        {
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Span<byte> SliceSpan(Span<byte> span, int start, int length)
        {
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int WriteStringProtected(Span<byte> span, string value, Encoding encoding)
        {
            return encoding.GetMaxByteCount(value.Length);
        }
    }
}