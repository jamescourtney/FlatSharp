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

namespace FlatSharp.Unsafe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed unsafe class UnsafeSpanWriter : ISpanWriter
    {
        public UnsafeSpanWriter()
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new InvalidOperationException("UnsafeSpanWriter only works on little-endian architectures. On big-endian systems, SpanWriter will work.");
            }
        }

        public void WriteByte(Span<byte> span, byte value, int offset, SerializationContext context)
        {
            span[offset] = value;
        }

        public void WriteSByte(Span<byte> span, sbyte value, int offset, SerializationContext context)
        {
            span[offset] = (byte)value;
        }

        public void WriteDouble(Span<byte> span, double value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(double));
            checked
            {
                EnsureInBounds(span, offset, sizeof(double));
                fixed (byte* pByte = &span[offset])
                {
                    *(double*)pByte = value;
                }
            }
        }

        public void WriteFloat(Span<byte> span, float value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(float));
            checked
            {
                EnsureInBounds(span, offset, sizeof(float));
                fixed (byte* pByte = &span[offset])
                {
                    *(float*)pByte = value;
                }
            }
        }

        public void WriteInt(Span<byte> span, int value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(int));
            checked
            {
                EnsureInBounds(span, offset, sizeof(int));
                fixed (byte* pByte = &span[offset])
                {
                    *(int*)pByte = value;
                }
            }
        }

        public void WriteLong(Span<byte> span, long value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(long));
            checked
            {
                EnsureInBounds(span, offset, sizeof(long));
                fixed (byte* pByte = &span[offset])
                {
                    *(long*)pByte = value;
                }
            }
        }

        public void WriteShort(Span<byte> span, short value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(short));
            checked
            {
                EnsureInBounds(span, offset, sizeof(short));
                fixed (byte* pByte = &span[offset])
                {
                    *(short*)pByte = value;
                }
            }
        }

        public void WriteUInt(Span<byte> span, uint value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(uint));
            checked
            {
                EnsureInBounds(span, offset, sizeof(uint));
                fixed (byte* pByte = &span[offset])
                {
                    *(uint*)pByte = value;
                }
            }
        }

        public void WriteULong(Span<byte> span, ulong value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            checked
            {
                EnsureInBounds(span, offset, sizeof(ulong));
                fixed (byte* pByte = &span[offset])
                {
                    *(ulong*)pByte = value;
                }
            }
        }

        public void WriteUShort(Span<byte> span, ushort value, int offset, SerializationContext context)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            checked
            {
                EnsureInBounds(span, offset, sizeof(ushort));
                fixed (byte* pByte = &span[offset])
                {
                    *(ushort*)pByte = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureInBounds(Span<byte> span, int offset, int size)
        {
            checked
            {
                if (offset + size > span.Length || offset < 0 || size < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetStringBytes(Span<byte> destination, string value, Encoding encoding)
        {
            checked
            {
                fixed (byte* pByte = destination)
                fixed (char* pChar = value)
                {
                    return encoding.GetBytes(pChar, value.Length, pByte, destination.Length);
                }
            }
        }
    }
}
