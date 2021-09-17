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

namespace FlatSharpTests
{
    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    
    public class InputBufferTests
    {
        private const string StringData = "how now brown cow";

        private static readonly Random Random = new Random();
        private static readonly byte[] Input = new byte[100 * 8];
        private static readonly byte[] StringInput;

        static InputBufferTests()
        {
            Random.NextBytes(Input);

            Input[0] = 0;
            Input[1] = 1;

            byte[] stringData = new byte[1024];
            int byteCount = Encoding.UTF8.GetBytes(StringData, 0, StringData.Length, stringData, 8);

            stringData[0] = 4;
            stringData[4] = (byte)byteCount;

            StringInput = stringData;
        }

        [Fact]
        public void MemoryInputBuffer()
        {
            this.InputBufferTest(new MemoryInputBuffer(Input));
            this.StringInputBufferTest(new MemoryInputBuffer(StringInput));
            this.TestDeserializeBoth(b => new MemoryInputBuffer(b));
            this.TestReadByteArray(b => new MemoryInputBuffer(b));
            this.TableSerializationTest(
                default(SpanWriter),
                (s, b) => s.Parse<PrimitiveTypesTable>(b.AsMemory()));
        }

        [Fact]
        public void ReadOnlyMemoryInputBuffer()
        {
            this.InputBufferTest(new ReadOnlyMemoryInputBuffer(Input));
            this.StringInputBufferTest(new ReadOnlyMemoryInputBuffer(StringInput));

            this.TestDeserialize<ReadOnlyMemoryInputBuffer, ReadOnlyMemoryTable>(b => new ReadOnlyMemoryInputBuffer(b));
            this.TestReadByteArray(b => new ReadOnlyMemoryInputBuffer(b));
            var ex = Assert.Throws<InvalidOperationException>(
                () => this.TestDeserialize<ReadOnlyMemoryInputBuffer, MemoryTable>(b => new ReadOnlyMemoryInputBuffer(b)));
            Assert.Equal("ReadOnlyMemory inputs may not deserialize writable memory.", ex.Message);

            this.TableSerializationTest(
                default(SpanWriter),
                (s, b) => s.Parse<PrimitiveTypesTable>((ReadOnlyMemory<byte>)b.AsMemory()));
        }

        [Fact]
        public void ArrayInputBuffer()
        {
            this.InputBufferTest(new ArrayInputBuffer(Input));
            this.StringInputBufferTest(new ArrayInputBuffer(StringInput));
            this.TestDeserializeBoth(b => new ArrayInputBuffer(b));
            this.TestReadByteArray(b => new ArrayInputBuffer(b));
            this.TableSerializationTest(
                default(SpanWriter),
                (s, b) => s.Parse<PrimitiveTypesTable>(b));
        }

        [Fact]
        public void ArraySegmentInputBuffer()
        {
            this.InputBufferTest(new ArraySegmentInputBuffer(new ArraySegment<byte>(Input)));
            this.StringInputBufferTest(new ArraySegmentInputBuffer(new ArraySegment<byte>(StringInput)));
            this.TestDeserializeBoth(b => new ArraySegmentInputBuffer(new ArraySegment<byte>(b)));
            this.TestReadByteArray(b => new ArraySegmentInputBuffer(new ArraySegment<byte>(b)));
            this.TableSerializationTest(
                default(SpanWriter),
                (s, b) => s.Parse<PrimitiveTypesTable>(new ArraySegment<byte>(b)));
        }

        [Fact]
        public void InitializeVTable_EmptyTable()
        {
            byte[] buffer =
            {
                4, 0, // vtable length
                4, 0, // table length
                4, 0, 0, 0 // soffset to vtable.
            };

            new ArrayInputBuffer(buffer).InitializeVTable(4, out int vtableOffset, out int maxVtableIndex);
            Assert.Equal(0, vtableOffset);
            Assert.Equal(-1, maxVtableIndex);
        }

        [Fact]
        public void InitializeVTable_InvalidLength()
        {
            byte[] buffer =
            {
                3, 0, // vtable length
                4, 0, // table length
                4, 0, 0, 0 // soffset to vtable.
            };

            var ex = Assert.Throws<InvalidDataException>(() => 
                new ArrayInputBuffer(buffer).InitializeVTable(4, out _, out _));

            Assert.Equal(
                "FlatBuffer was in an invalid format: VTable was not long enough to be valid.",
                ex.Message);
        }

        [Fact]
        public void InitializeVTable_NormalTable()
        {
            byte[] buffer =
            {
                8, 0, // vtable length
                12, 0, // table length
                4, 0, // index 0 offset
                8, 0, // index 1 offset
                8, 0, 0, 0, // soffset to vtable.
                1, 0, 0, 0,
                2, 0, 0, 0,
            };

            new ArrayInputBuffer(buffer).InitializeVTable(8, out int vtableOffset, out int maxVtableIndex);
            Assert.Equal(0, vtableOffset);
            Assert.Equal(1, maxVtableIndex);
        }

        [Fact]
        public void ReadUOffset()
        {
            byte[] buffer = { 4, 0, 0, 0 };
            Assert.Equal(4, new ArrayInputBuffer(buffer).ReadUOffset(0));

            buffer = new byte[] { 3, 0, 0, 0 };
            var ex = Assert.Throws<InvalidDataException>(() => new ArrayInputBuffer(buffer).ReadUOffset(0));
            Assert.Equal(
                "FlatBuffer was in an invalid format: Decoded uoffset_t had value less than 4. Value = 3",
                ex.Message);
        }

        private void TableSerializationTest(
            ISpanWriter writer,
            Func<FlatBufferSerializer, byte[], PrimitiveTypesTable> parse)
        {
            PrimitiveTypesTable original = new()
            {
                A = true,
                B = 1,
                C = 2,
                D = 3,
                E = 4,
                F = 5,
                G = 6,
                H = 7,
                I = 8,
                J = 9.1f,
                K = 10.2,
                L = "foobar",
                M = "foobar2",
            };

            byte[] data = new byte[1024];
            FlatBufferSerializer.Default.Serialize(original, data, writer);
            PrimitiveTypesTable parsed = parse(FlatBufferSerializer.Default, data);

            Assert.Equal(original.A, parsed.A);
            Assert.Equal(original.B, parsed.B);
            Assert.Equal(original.C, parsed.C);
            Assert.Equal(original.D, parsed.D);
            Assert.Equal(original.E, parsed.E);
            Assert.Equal(original.F, parsed.F);
            Assert.Equal(original.G, parsed.G);
            Assert.Equal(original.H, parsed.H);
            Assert.Equal(original.I, parsed.I);
            Assert.Equal(original.J, parsed.J);
            Assert.Equal(original.K, parsed.K);
            Assert.Equal(original.L, parsed.L);
            Assert.Equal(original.M, parsed.M);
        }

        private void InputBufferTest(IInputBuffer ib)
        {
            var mem = new Memory<byte>(Input);

            Assert.False(ib.ReadBool(0));
            Assert.True(ib.ReadBool(1));

            this.CompareEqual<byte>(sizeof(byte), i => mem.Span[i], i => ib.ReadByte(i));
            this.CompareEqual<sbyte>(sizeof(sbyte), i => (sbyte)mem.Span[i], i => ib.ReadSByte(i));

            this.CompareEqual<ushort>(sizeof(ushort), i => BinaryPrimitives.ReadUInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadUShort(i));
            this.CompareEqual<short>(sizeof(short), i => BinaryPrimitives.ReadInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadShort(i));

            this.CompareEqual<uint>(sizeof(uint), i => BinaryPrimitives.ReadUInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadUInt(i));
            this.CompareEqual<int>(sizeof(int), i => BinaryPrimitives.ReadInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadInt(i));

#if NETCOREAPP2_1_OR_GREATER
            this.CompareEqual<float>(sizeof(float), i => BitConverter.ToSingle(mem.Span.Slice(i)), i => ib.ReadFloat(i));
#endif

            this.CompareEqual<ulong>(sizeof(ulong), i => BinaryPrimitives.ReadUInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadULong(i));
            this.CompareEqual<long>(sizeof(long), i => BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadLong(i));
            this.CompareEqual<double>(sizeof(double), i => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i))), i => ib.ReadDouble(i));
        }

        private void StringInputBufferTest(IInputBuffer ib)
        {
            Assert.Equal(StringData, ib.ReadString(0));
        }

        private void CompareEqual<T>(
            int size, 
            Func<int, T> readMemoryAtIndex,
            Func<int, T> readAtIndex)
        {
            for (int i = 0; i < Input.Length - size; i += size)
            {
                var memory = readMemoryAtIndex(i);
                var bufferValue = readAtIndex(i);
                Assert.Equal(memory, bufferValue);
            }
        }

        private void TestDeserializeBoth<TBuffer>(Func<byte[], TBuffer> bufferBuilder) where TBuffer : IInputBuffer
        {
            this.TestDeserialize<TBuffer, ReadOnlyMemoryTable>(bufferBuilder);
            this.TestDeserialize<TBuffer, MemoryTable>(bufferBuilder);
        }

        private void TestReadByteArray<TBuffer>(Func<byte[], TBuffer> bufferBuilder) where TBuffer : IInputBuffer
        {
            byte[] buffer = new byte[] { 4, 0, 0, 0, 7, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 };
            TBuffer inputBuffer = bufferBuilder(buffer);

            byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, };

            Assert.True(inputBuffer.ReadByteReadOnlyMemoryBlock(0).Span.SequenceEqual(expected));
        }

        private void TestDeserialize<TBuffer, TType>(Func<byte[], TBuffer> bufferBuilder) 
            where TBuffer : IInputBuffer
            where TType : class, IMemoryTable, new()
        {
            TType table = new TType
            {
                Memory = new byte[] { 6, 7, 8, 9, 10 }
            };

            byte[] dest = new byte[1024];
            FlatBufferSerializer.Default.Serialize(table, dest);

            TBuffer buffer = bufferBuilder(dest);
            var parsed = FlatBufferSerializer.Default.Parse<TType, TBuffer>(buffer);
            for (int i = 1; i <= 5; ++i)
            {
                Assert.Equal(i + 5, parsed.Memory.Span[i - 1]);
            }
        }

        private interface IMemoryTable
        {
            ReadOnlyMemory<byte> Memory { get; set; }
        }

        [FlatBufferTable]
        public class PrimitiveTypesTable
        {
            [FlatBufferItem(0)]
            public virtual bool A { get; set; }

            [FlatBufferItem(1)]
            public virtual byte B { get; set; }

            [FlatBufferItem(2)]
            public virtual sbyte C { get; set; }

            [FlatBufferItem(3)]
            public virtual ushort D { get; set; }

            [FlatBufferItem(4)]
            public virtual short E { get; set; }

            [FlatBufferItem(5)]
            public virtual uint F { get; set; }

            [FlatBufferItem(6)]
            public virtual int G { get; set; }

            [FlatBufferItem(7)]
            public virtual ulong H { get; set; }

            [FlatBufferItem(8)]
            public virtual long I { get; set; }

            [FlatBufferItem(9)]
            public virtual float J { get; set; }

            [FlatBufferItem(10)]
            public virtual double K { get; set; }

            [FlatBufferItem(11)]
            public virtual string? L { get; set; }

            [FlatBufferItem(12, SharedString = true)]
            public virtual string? M { get; set; }
        }

        [FlatBufferTable]
        public class ReadOnlyMemoryTable : IMemoryTable
        {
            [FlatBufferItem(0)]
            public virtual ReadOnlyMemory<byte> Memory { get; set; }
        }

        [FlatBufferTable]
        public class MemoryTable : IMemoryTable
        {
            [FlatBufferItem(0)]
            public virtual Memory<byte> Memory { get; set; }

            ReadOnlyMemory<byte> IMemoryTable.Memory { get => this.Memory; set => this.Memory = MemoryMarshal.AsMemory(value); }
        }
    }
}
