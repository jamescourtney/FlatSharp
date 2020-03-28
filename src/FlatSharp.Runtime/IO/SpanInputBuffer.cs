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
    using System.Buffers.Binary;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// An implementation of <see cref="InputBuffer"/> for managed arrays.
    /// </summary>
    public abstract class SpanInputBuffer : InputBuffer
    {
        public sealed override byte ReadByte(int offset) => ScalarSpanReader.ReadByte(this.GetSpan(), offset);

        public sealed override sbyte ReadSByte(int offset) => ScalarSpanReader.ReadSByte(this.GetSpan(), offset);

        public sealed override ushort ReadUShort(int offset) => ScalarSpanReader.ReadUShort(this.GetSpan(), offset);

        public sealed override short ReadShort(int offset) => ScalarSpanReader.ReadShort(this.GetSpan(), offset);

        public sealed override uint ReadUInt(int offset) => ScalarSpanReader.ReadUInt(this.GetSpan(), offset);

        public sealed override int ReadInt(int offset) => ScalarSpanReader.ReadInt(this.GetSpan(), offset);

        public sealed override ulong ReadULong(int offset) => ScalarSpanReader.ReadULong(this.GetSpan(), offset);

        public sealed override long ReadLong(int offset) => ScalarSpanReader.ReadLong(this.GetSpan(), offset);

        public sealed override float ReadFloat(int offset) => ScalarSpanReader.ReadFloat(this.GetSpan(), offset);

        public sealed override double ReadDouble(int offset)
        {
            return BitConverter.Int64BitsToDouble(this.ReadLong(offset));
        }

        protected sealed override string ReadStringProtected(int offset, int byteLength, Encoding encoding)
        {
#if NETCOREAPP
            return encoding.GetString(this.GetSpan().Slice(offset, byteLength));
#else
            return encoding.GetString(this.GetSpan().Slice(offset, byteLength).ToArray());
#endif
        }

        protected abstract ReadOnlySpan<byte> GetSpan();
    }
}
