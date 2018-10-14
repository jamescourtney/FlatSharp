﻿/*
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

namespace FlatSharp.Unsafe
{
    using System;
    using System.Buffers;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed unsafe class UnsafeSpanWriter : SpanWriter
    {
        public UnsafeSpanWriter()
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new InvalidOperationException("UnsafeSpanWriter only works on little-endian architectures presently. On big-endian systems, ArrayInputBuffer and MemoryInputBuffer will both work.");
            }
        }

        public override void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context)
        {
            span[offset] = value;
        }

        public override void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context)
        {
            span[offset] = (byte)value;
        }

        public override void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(double*)pByte = value;
                }
            }
        }

        public override void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(float*)pByte = value;
                }
            }
        }

        public override void WriteInt(Span<byte> span, int value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(int*)pByte = value;
                }
            }
        }

        public override void WriteLong(Span<byte> span, long value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(long*)pByte = value;
                }
            }
        }

        public override void WriteShort(Span<byte> span, short value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(short*)pByte = value;
                }
            }
        }

        public override void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(uint*)pByte = value;
                }
            }
        }

        public override void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(ulong*)pByte = value;
                }
            }
        }

        public override void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context)
        {
            checked
            {
                fixed (byte* pByte = &span[offset])
                {
                    *(ushort*)pByte = value;
                }
            }
        }

        protected override int WriteStringProtected(Span<byte> span, string value, Encoding encoding)
        {
            checked
            {
                fixed (byte* pByte = span)
                fixed (char* pChar = value)
                {
                    return encoding.GetBytes(pChar, value.Length, pByte, span.Length);
                }
            }
        }
    }
}
