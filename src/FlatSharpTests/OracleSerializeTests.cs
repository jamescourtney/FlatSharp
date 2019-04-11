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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Does oracle-based testing using the Google.Flatbuffers code. These test all boil down to:
    ///     Can the Google Flatbuffer code parse data we generated?
    /// </summary>
    [TestClass]
    public partial class OracleSerializeTests
    {
        [TestMethod]
        public void SimpleTypes()
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
            int size = FlatBufferSerializer.Default.Serialize(simple, memory);

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
