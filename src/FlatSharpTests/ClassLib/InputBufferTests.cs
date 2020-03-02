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
    using FlatSharp.Unsafe;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    [TestClass]
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

        [TestMethod]
        public void SafeMemoryInputBuffer()
        {
            this.InputBufferTest(new MemoryInputBuffer(Input));
            this.StringInputBufferTest(new MemoryInputBuffer(StringInput));
            this.TestDeserializeBoth(b => new MemoryInputBuffer(b));
        }

        [TestMethod]
        public void SafeReadOnlyMemoryInputBuffer()
        {
            this.InputBufferTest(new ReadOnlyMemoryInputBuffer(Input));
            this.StringInputBufferTest(new ReadOnlyMemoryInputBuffer(StringInput));

            this.TestDeserialize<ReadOnlyMemoryInputBuffer, ReadOnlyMemoryTable>(b => new ReadOnlyMemoryInputBuffer(b));
            Assert.ThrowsException<InvalidOperationException>(
                () => this.TestDeserialize<ReadOnlyMemoryInputBuffer, MemoryTable>(b => new ReadOnlyMemoryInputBuffer(b)));
        }

        [TestMethod]
        public void ArrayInputBuffer()
        {
            this.InputBufferTest(new ArrayInputBuffer(Input));
            this.StringInputBufferTest(new ArrayInputBuffer(StringInput));
            this.TestDeserializeBoth(b => new ArrayInputBuffer(b));
        }

        [TestMethod]
        public void UnsafeArrayInputBuffer()
        {
            this.InputBufferTest(new UnsafeArrayInputBuffer(Input));
            this.StringInputBufferTest(new UnsafeArrayInputBuffer(StringInput));
            this.TestDeserializeBoth(b => new UnsafeArrayInputBuffer(b));
        }

        [TestMethod]
        public void UnsafeMemoryInputBuffer()
        {
            using (var buffer = new UnsafeMemoryInputBuffer(new Memory<byte>(Input)))
            {
                this.InputBufferTest(buffer);
            }

            using (var buffer = new UnsafeMemoryInputBuffer(new Memory<byte>(StringInput)))
            {
                this.StringInputBufferTest(buffer);
            }

            this.TestDeserializeBoth(b => new UnsafeMemoryInputBuffer(b));
        }

        private void InputBufferTest(InputBuffer ib)
        {
            var mem = new Memory<byte>(Input);

            Assert.IsFalse(ib.ReadBool(0));
            Assert.IsTrue(ib.ReadBool(1));

            this.CompareEqual<byte>(sizeof(byte), i => mem.Span[i], i => ib.ReadByte(i));
            this.CompareEqual<sbyte>(sizeof(sbyte), i => (sbyte)mem.Span[i], i => ib.ReadSByte(i));

            this.CompareEqual<ushort>(sizeof(ushort), i => BinaryPrimitives.ReadUInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadUShort(i));
            this.CompareEqual<short>(sizeof(short), i => BinaryPrimitives.ReadInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadShort(i));

            this.CompareEqual<uint>(sizeof(uint), i => BinaryPrimitives.ReadUInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadUInt(i));
            this.CompareEqual<int>(sizeof(int), i => BinaryPrimitives.ReadInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadInt(i));

#if NETCOREAPP
            this.CompareEqual<float>(sizeof(float), i => BitConverter.ToSingle(mem.Span.Slice(i)), i => ib.ReadFloat(i));
#endif

            this.CompareEqual<ulong>(sizeof(ulong), i => BinaryPrimitives.ReadUInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadULong(i));
            this.CompareEqual<long>(sizeof(long), i => BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadLong(i));
            this.CompareEqual<double>(sizeof(double), i => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i))), i => ib.ReadDouble(i));
        }

        private void StringInputBufferTest(InputBuffer ib)
        {
            Assert.AreEqual(StringData, ib.ReadString(0));
        }

        private void CompareEqual<T>(
            int size, 
            Func<int, T> readMemoryAtIndex,
            Func<int, T> readAtIndex)
        {
            for (int i = 0; i < Input.Length - size; ++i)
            {
                var memory = readMemoryAtIndex(i);
                var bufferValue = readAtIndex(i);
                Assert.AreEqual(memory, bufferValue);
            }
        }

        private void TestDeserializeBoth<TBuffer>(Func<byte[], TBuffer> bufferBuilder) where TBuffer : InputBuffer
        {
            this.TestDeserialize<TBuffer, ReadOnlyMemoryTable>(bufferBuilder);
            this.TestDeserialize<TBuffer, MemoryTable>(bufferBuilder);
        }

        private void TestDeserialize<TBuffer, TType>(Func<byte[], TBuffer> bufferBuilder) 
            where TBuffer : InputBuffer
            where TType : class, IMemoryTable, new()
        {
            TType table = new TType
            {
                IntMemory = new int[] { 1, 2, 3, 4, 5, },
                Memory = new byte[] { 6, 7, 8, 9, 10 }
            };

            byte[] dest = new byte[1024];
            FlatBufferSerializer.Default.Serialize(table, dest);

            TBuffer buffer = bufferBuilder(dest);
            var parsed = FlatBufferSerializer.Default.Parse<TType>(buffer);
            for (int i = 1; i <= 5; ++i)
            {
                Assert.AreEqual(i, parsed.IntMemory.Span[i - 1]);
                Assert.AreEqual(i + 5, parsed.Memory.Span[i - 1]);
            }
        }

        private interface IMemoryTable
        {
            ReadOnlyMemory<int> IntMemory { get; set; }

            ReadOnlyMemory<byte> Memory { get; set; }
        }

        [FlatBufferTable]
        public class ReadOnlyMemoryTable : IMemoryTable
        {
            [FlatBufferItem(0)]
            public ReadOnlyMemory<int> IntMemory { get; set; }

            [FlatBufferItem(1)]
            public ReadOnlyMemory<byte> Memory { get; set; }
        }

        [FlatBufferTable]
        public class MemoryTable : IMemoryTable
        {
            [FlatBufferItem(0)]
            public Memory<int> IntMemory { get; set; }

            [FlatBufferItem(1)]
            public Memory<byte> Memory { get; set; }

            ReadOnlyMemory<int> IMemoryTable.IntMemory { get => this.IntMemory; set => this.IntMemory = MemoryMarshal.AsMemory(value); }

            ReadOnlyMemory<byte> IMemoryTable.Memory { get => this.Memory; set => this.Memory = MemoryMarshal.AsMemory(value); }
        }
    }
}
