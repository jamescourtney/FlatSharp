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
    using FlatSharp;
    using FlatSharp.Unsafe;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Does oracle-based testing using the Google.Flatbuffers code. These test all boil down to:
    ///     Can the Google Flatbuffer code parse data we generated?
    /// </summary>
    [TestClass]
    public partial class OracleSerializeTests
    {
        [TestMethod]
        public void SimpleTypes_SafeSpanWriter()
        {
            this.SimpleTypesTest(new SpanWriter());
        }

        [TestMethod]
        public void SimpleTypes_UnsafeSpanWriter()
        {
            this.SimpleTypesTest(new UnsafeSpanWriter());
        }

        private void SimpleTypesTest<TSpanWriter>(TSpanWriter writer) where TSpanWriter : ISpanWriter
        {
            var simple = new BasicTypes
            {
                Bool = true,
                Byte = GetRandom<byte>(),
                SByte = GetRandom<sbyte>(),
                Double = GetRandom<double>(),
                Float = GetRandom<float>(),
                Int = GetRandom<int>(),
                Long = GetRandom<long>(),
                Short = GetRandom<short>(),
                String = "foobar",
                UInt = GetRandom<uint>(),
                ULong = GetRandom<ulong>(),
                UShort = GetRandom<ushort>(),
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(simple, memory, writer);

            var oracle = Oracle.BasicTypes.GetRootAsBasicTypes(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.IsTrue(oracle.Bool);
            Assert.AreEqual(oracle.Byte, simple.Byte);
            Assert.AreEqual(oracle.SByte, simple.SByte);

            Assert.AreEqual(oracle.UShort, simple.UShort);
            Assert.AreEqual(oracle.Short, simple.Short);

            Assert.AreEqual(oracle.UInt, simple.UInt);
            Assert.AreEqual(oracle.Int, simple.Int);

            Assert.AreEqual(oracle.ULong, simple.ULong);
            Assert.AreEqual(oracle.Long, simple.Long);

            Assert.AreEqual(oracle.Float, simple.Float);
            Assert.AreEqual(oracle.Double, simple.Double);
            Assert.AreEqual(oracle.String, simple.String);
        }

        [TestMethod]
        public void LinkedList()
        {
            var simple = new TestLinkedListNode
            {
                Value = "node 1",
                Next = new TestLinkedListNode
                {
                    Value = "node 2",
                    Next = null,
                }
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(simple, memory);

            var oracle = Oracle.LinkedListNode.GetRootAsLinkedListNode(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(oracle.Value, simple.Value);
            Assert.AreEqual(oracle.Next?.Value, simple.Next.Value);
            Assert.IsNull(oracle.Next?.Next);
        }

        [TestMethod]
        public void StructWithinTable()
        {
            LocationHolder holder = new LocationHolder
            {
                Fake = "foobar",
                Location = new Location { X = 1.1f, Y = 1.2f, Z = 1.3f },
                LocationVector = new[] 
                {
                    new Location { X = 2, Y = 3, Z = 4, },
                    new Location { X = 5, Y = 6, Z = 7, },
                    new Location { X = 8, Y = 9, Z = 10, }
                },
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(holder, memory);

            var oracle = Oracle.LocationHolder.GetRootAsLocationHolder(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(holder.Fake, oracle.Fake);

            Assert.AreEqual(holder.Location.X, oracle.SingleLocation.Value.X);
            Assert.AreEqual(holder.Location.Y, oracle.SingleLocation.Value.Y);
            Assert.AreEqual(holder.Location.Z, oracle.SingleLocation.Value.Z);

            Assert.AreEqual(holder.LocationVector.Count, oracle.LocationVectorLength);

            for (int i = 0; i < holder.LocationVector.Count; ++i)
            {
                var holderLoc = holder.LocationVector[i];
                var oracleLoc = oracle.LocationVector(i);

                Assert.AreEqual(holderLoc.X, oracleLoc.Value.X);
                Assert.AreEqual(holderLoc.Y, oracleLoc.Value.Y);
                Assert.AreEqual(holderLoc.Z, oracleLoc.Value.Z);
            }
        }

        [TestMethod]
        public void ScalarVectors()
        {
            ScalarVectorsTable table = new ScalarVectorsTable
            {
                IntVector = new [] { 1, 2, 3, 4, 5 },
                LongVector = new [] { 1L, 2L, 3L, 4L, 5L, },
                ByteVector1 = new byte[] { 1, 2, 3, 4, 5, },
                ByteVector2 = new byte[] { 6, 7, 8, 9, 10 },
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.Vectors.GetRootAsVectors(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(table.IntVector.Count, oracle.IntVectorLength);
            Assert.AreEqual(table.LongVector.Count, oracle.LongVectorLength);
            Assert.AreEqual(table.ByteVector1.Count, oracle.ByteVector1Length);
            Assert.AreEqual(table.ByteVector2.Length, oracle.ByteVector2Length);

            for (int i = 0; i < table.IntVector.Count; ++i)
            {
                Assert.AreEqual(table.IntVector[i], oracle.IntVector(i));
            }

            for (int i = 0; i < table.LongVector.Count; ++i)
            {
                Assert.AreEqual(table.LongVector[i], oracle.LongVector(i));
            }

            for (int i = 0; i < table.ByteVector1.Count; ++i)
            {
                Assert.AreEqual(table.ByteVector1[i], oracle.ByteVector1(i));
            }

            for (int i = 0; i < table.ByteVector2.Length; ++i)
            {
                Assert.AreEqual(table.ByteVector2.Span[i], oracle.ByteVector2(i));
            }
        }

        [TestMethod]
        public void FiveByteStructVector()
        {
            FiveByteStructTable table = new FiveByteStructTable
            {
                Vector = new[]
                {
                    new FiveByteStruct { Byte = 1, Int = 1 },
                    new FiveByteStruct { Byte = 2, Int = 2 },
                    new FiveByteStruct { Byte = 3, Int = 3 },
                }
            };

            Span<byte> buffer = new byte[1024];
            var bytecount = FlatBufferSerializer.Default.Serialize(table, buffer);
            buffer = buffer.Slice(0, bytecount);

            var oracle = Oracle.FiveByteStructTable.GetRootAsFiveByteStructTable(
                new FlatBuffers.ByteBuffer(buffer.ToArray()));

            Assert.AreEqual(3, oracle.VectorLength);

            Assert.AreEqual(1, oracle.Vector(0).Value.Int);
            Assert.AreEqual((byte)1, oracle.Vector(0).Value.Byte);

            Assert.AreEqual(2, oracle.Vector(1).Value.Int);
            Assert.AreEqual((byte)2, oracle.Vector(1).Value.Byte);

            Assert.AreEqual(3, oracle.Vector(2).Value.Int);
            Assert.AreEqual((byte)3, oracle.Vector(2).Value.Byte);
        }

        [TestMethod]
        public void Union_Table_BasicTypes()
        {
            var simple = new BasicTypes
            {
                Bool = true,
                Byte = GetRandom<byte>(),
                SByte = GetRandom<sbyte>(),
                Double = GetRandom<double>(),
                Float = GetRandom<float>(),
                Int = GetRandom<int>(),
                Long = GetRandom<long>(),
                Short = GetRandom<short>(),
                String = "foobar",
                UInt = GetRandom<uint>(),
                ULong = GetRandom<ulong>(),
                UShort = GetRandom<ushort>(),
            };

            var table = new UnionTable
            {
                Union = new FlatBufferUnion<BasicTypes, Location, string>(simple)
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.UnionTable.GetRootAsUnionTable(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(1, (int)oracle.ValueType);
            var bt = oracle.Value<Oracle.BasicTypes>().Value;

            Assert.IsTrue(bt.Bool);
            Assert.AreEqual(bt.Byte, simple.Byte);
            Assert.AreEqual(bt.SByte, simple.SByte);

            Assert.AreEqual(bt.UShort, simple.UShort);
            Assert.AreEqual(bt.Short, simple.Short);

            Assert.AreEqual(bt.UInt, simple.UInt);
            Assert.AreEqual(bt.Int, simple.Int);

            Assert.AreEqual(bt.ULong, simple.ULong);
            Assert.AreEqual(bt.Long, simple.Long);

            Assert.AreEqual(bt.Float, simple.Float);
            Assert.AreEqual(bt.Double, simple.Double);
            Assert.AreEqual(bt.String, simple.String);
        }

        [TestMethod]
        public void Union_String()
        {
            var table = new UnionTable
            {
                Union = new FlatBufferUnion<BasicTypes, Location, string>("foobar")
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.UnionTable.GetRootAsUnionTable(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            // There is nothing to assert here, since the google C# SDK does not support reading strings
            // out of a union. All indications are that we are doing it right, but there's no way to test
            // this particular scenario.
            Assert.AreEqual(3, (int)oracle.ValueType);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Union_NotSet()
        {
            var table = new UnionTable
            {
                Union = null,
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.UnionTable.GetRootAsUnionTable(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(0, (int)oracle.ValueType);
        }

        [TestMethod]
        public void Union_Struct_Location()
        {
            var location = new Location
            {
                X = 1.0f, Y = 2.0f, Z = 3.0f
            };

            var table = new UnionTable
            {
                Union = new FlatBufferUnion<BasicTypes, Location, string>(location)
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.UnionTable.GetRootAsUnionTable(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.AreEqual(2, (int)oracle.ValueType);
            var bt = oracle.Value<Oracle.Location>().Value;

            Assert.AreEqual(bt.X, location.X);
            Assert.AreEqual(bt.Y, location.Y);
            Assert.AreEqual(bt.Z, location.Z);
        }

        [TestMethod]
        public void NestedStruct_InnerStructSet()
        {
            var nested = new NestedStructs
            {
                OuterStruct = new OuterStruct
                {
                    A = 100,
                    InnerStruct = new InnerStruct { A = 401 }
                }
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(nested, memory);

            var oracle = Oracle.NestedStructs.GetRootAsNestedStructs(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.IsNotNull(oracle.Outer);

            var outer = oracle.Outer.Value;
            Assert.AreEqual(100, outer.A);
            Assert.AreEqual(401, outer.Inner.A);
        }

        [TestMethod]
        public void NestedStruct_InnerStructNotSet()
        {
            var nested = new NestedStructs
            {
                OuterStruct = new OuterStruct
                {
                    A = 100,
                    InnerStruct = null,
                }
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(nested, memory);

            var oracle = Oracle.NestedStructs.GetRootAsNestedStructs(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.IsNotNull(oracle.Outer);

            var outer = oracle.Outer.Value;
            Assert.AreEqual(100, outer.A);
            Assert.AreEqual(0, outer.Inner.A);
        }

        [TestMethod]
        public void SortedVectors()
        {
            var test = new SortedVectorTest<SortedVectorItem<int>>
            {
                Double = new List<SortedVectorItem<double>>(),
                IntVector = new List<SortedVectorItem<int>>(),
                StringVector = new List<SortedVectorItem<string>>(),
            };

            const int Iterations = 1000;
            Random random = new Random();

            for (int i = 0; i < Iterations; ++i)
            {
                string value = Guid.NewGuid().ToString();
                test.StringVector.Add(new SortedVectorItem<string> { Value = value });
                test.IntVector.Add(new SortedVectorItem<int> { Value = random.Next() });
                test.Double.Add(new SortedVectorItem<double> { Value = random.NextDouble() * random.Next() });
            }

            byte[] data = new byte[1024 * 1024];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(test, data);

            var parsed = FlatBufferSerializer.Default.Parse<SortedVectorTest<SortedVectorItem<int>>>(data);
            var table = Oracle.SortedVectorTest.GetRootAsSortedVectorTest(new FlatBuffers.ByteBuffer(data));

            VerifySorted<IList<SortedVectorItem<string>>, SortedVectorItem<string>, string>(parsed.StringVector, i => i.Value, i => table.String(i)?.Value, t => table.StringByKey(t) != null, new Utf8StringComparer());
            VerifySorted<IList<SortedVectorItem<int>>, SortedVectorItem<int>, int>(parsed.IntVector, i => i.Value, i => table.Int32(i).Value.Value, t => table.Int32ByKey(t) != null, Comparer<int>.Default);
            VerifySorted<IList<SortedVectorItem<double>>, SortedVectorItem<double>, double>(parsed.Double, i => i.Value, i => table.Double(i).Value.Value, t => table.DoubleByKey(t) != null, Comparer<double>.Default);
        }

        [TestMethod]
        public void SortedVectors_DefaultValue()
        {
            var test = new SortedVectorTest<SortedVectorDefaultValueItem>
            {
                IntVector = new List<SortedVectorDefaultValueItem>(),
            };

            // Verify that we actually write 5 in the output even though it has a default value set.
            SortedVectorDefaultValueItem myItem = new SortedVectorDefaultValueItem();
            byte[] data = new byte[1024 * 1024];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(myItem, data);
            Assert.AreEqual(5, myItem.Value);
            //Assert.AreEqual(18, bytesWritten); // table offset (4), vtable offset (4), vtable headers (6), data (4)

            const int Iterations = 1000;
            Random random = new Random();

            for (int i = 0; i < Iterations; ++i)
            {
                test.IntVector.Add(new SortedVectorDefaultValueItem { Value = random.Next() });
            }

            test.IntVector.Add(myItem);

            bytesWritten = FlatBufferSerializer.Default.Serialize(test, data);

            var parsed = FlatBufferSerializer.Default.Parse<SortedVectorTest<SortedVectorDefaultValueItem>>(data);
            var table = Oracle.SortedVectorTest.GetRootAsSortedVectorTest(new FlatBuffers.ByteBuffer(data));

            VerifySorted<IList<SortedVectorDefaultValueItem>, SortedVectorDefaultValueItem, int>(parsed.IntVector, i => i.Value, i => table.Int32(i).Value.Value, t => table.Int32ByKey(t) != null, Comparer<int>.Default);
        }

        private static void VerifySorted<TList, TItem, TKey>(
            TList items, 
            Func<TItem, TKey> keyGet,
            Func<int, TKey> oracleGet,
            Func<TKey, bool> oracleContains,
            IComparer<TKey> comparer) where TList : IList<TItem>
        {
            TKey previous = keyGet(items[0]);

            for (int i = 0; i < items.Count; ++i)
            {
                TKey current = keyGet(items[i]);
                TKey oracle = oracleGet(i);

                Assert.IsTrue(comparer.Compare(previous, current) <= 0);
                Assert.IsTrue(comparer.Compare(previous, oracle) <= 0);
                Assert.IsTrue(comparer.Compare(current, oracle) == 0);

                // FlatBuffers c# has a bug where binary search is broken when using default values.
                // Assert.IsTrue(oracleContains(current));
            }
        }

        private static readonly Random Random = new Random();

        private static T GetRandom<T>() where T : struct
        {
            int bytes = Marshal.SizeOf<T>();
            byte[] random = new byte[bytes];
            Random.NextBytes(random);

            return MemoryMarshal.Cast<byte, T>(random)[0];
        }
    }
}
