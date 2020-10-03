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

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlatSharp
{
    public interface ISpanWriter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteInt(Span<byte> span, int value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteLong(Span<byte> span, long value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteShort(Span<byte> span, short value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetStringBytes(Span<byte> destination, string value, Encoding encoding);
    }
}