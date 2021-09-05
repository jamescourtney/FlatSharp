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
    using Xunit;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Does oracle-based testing using the Google.Flatbuffers code. These test all boil down to:
    ///     Can the Google Flatbuffer code parse data we generated?
    /// </summary>
    
    public partial class OracleSerializeTests
    {
        [Fact]
        public void SimpleTypes_SafeSpanWriter() =>
            this.SimpleTypesTest<SpanWriter, BasicTypes>(new SpanWriter());

        [Fact]
        public void SimpleTypes_ForceWrite()
        {
            this.SimpleTypesTest<SpanWriter, BasicTypes>(new SpanWriter());
        }

        [Fact]
        public void SimpleTypesForceWrite_SizeCompare()
        {
            int withForceWriteSize = this.SimpleTypesDefaultValues_Test<BasicTypesForceWrite>();
            int defaultSize = this.SimpleTypesDefaultValues_Test<BasicTypes>();

            Assert.True(withForceWriteSize > defaultSize);
        }

        private int SimpleTypesDefaultValues_Test<TBasicTypes>()
            where TBasicTypes : class, IBasicTypes, new()
        {
            var simple = new TBasicTypes();

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(simple, memory);

            var oracle = Oracle.BasicTypes.GetRootAsBasicTypes(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.Equal(oracle.Byte, simple.Byte);
            Assert.Equal(oracle.SByte, simple.SByte);

            Assert.Equal(oracle.UShort, simple.UShort);
            Assert.Equal(oracle.Short, simple.Short);

            Assert.Equal(oracle.UInt, simple.UInt);
            Assert.Equal(oracle.Int, simple.Int);

            Assert.Equal(oracle.ULong, simple.ULong);
            Assert.Equal(oracle.Long, simple.Long);

            Assert.Equal(oracle.Float, simple.Float);
            Assert.Equal(oracle.Double, simple.Double);
            Assert.Equal(oracle.String, simple.String);

            return size;
        }

        private void SimpleTypesTest<TSpanWriter, TBasicTypes>(TSpanWriter writer) 
            where TSpanWriter : ISpanWriter
            where TBasicTypes : class, IBasicTypes, new()
        {
            var simple = new TBasicTypes
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

            Assert.True(oracle.Bool);
            Assert.Equal(oracle.Byte, simple.Byte);
            Assert.Equal(oracle.SByte, simple.SByte);

            Assert.Equal(oracle.UShort, simple.UShort);
            Assert.Equal(oracle.Short, simple.Short);

            Assert.Equal(oracle.UInt, simple.UInt);
            Assert.Equal(oracle.Int, simple.Int);

            Assert.Equal(oracle.ULong, simple.ULong);
            Assert.Equal(oracle.Long, simple.Long);

            Assert.Equal(oracle.Float, simple.Float);
            Assert.Equal(oracle.Double, simple.Double);
            Assert.Equal(oracle.String, simple.String);
        }

        [Fact]
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

            Assert.Equal(oracle.Value, simple.Value);
            Assert.Equal(oracle.Next?.Value, simple.Next.Value);
            Assert.Null(oracle.Next?.Next);
        }

        [Fact]
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

            Assert.Equal(holder.Fake, oracle.Fake);

            Assert.Equal(holder.Location.X, oracle.SingleLocation.Value.X);
            Assert.Equal(holder.Location.Y, oracle.SingleLocation.Value.Y);
            Assert.Equal(holder.Location.Z, oracle.SingleLocation.Value.Z);

            Assert.Equal(holder.LocationVector.Count, oracle.LocationVectorLength);

            for (int i = 0; i < holder.LocationVector.Count; ++i)
            {
                var holderLoc = holder.LocationVector[i];
                var oracleLoc = oracle.LocationVector(i);

                Assert.Equal(holderLoc.X, oracleLoc.Value.X);
                Assert.Equal(holderLoc.Y, oracleLoc.Value.Y);
                Assert.Equal(holderLoc.Z, oracleLoc.Value.Z);
            }
        }

        [Fact]
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

            Assert.Equal(table.IntVector.Count, oracle.IntVectorLength);
            Assert.Equal(table.LongVector.Count, oracle.LongVectorLength);
            Assert.Equal(table.ByteVector1.Count, oracle.ByteVector1Length);
            Assert.Equal(table.ByteVector2.Value.Length, oracle.ByteVector2Length);

            for (int i = 0; i < table.IntVector.Count; ++i)
            {
                Assert.Equal(table.IntVector[i], oracle.IntVector(i));
            }

            for (int i = 0; i < table.LongVector.Count; ++i)
            {
                Assert.Equal(table.LongVector[i], oracle.LongVector(i));
            }

            for (int i = 0; i < table.ByteVector1.Count; ++i)
            {
                Assert.Equal(table.ByteVector1[i], oracle.ByteVector1(i));
            }

            for (int i = 0; i < table.ByteVector2.Value.Length; ++i)
            {
                Assert.Equal(table.ByteVector2.Value.Span[i], oracle.ByteVector2(i));
            }
        }

        [Fact]
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

            Assert.Equal(3, oracle.VectorLength);

            Assert.Equal(1, oracle.Vector(0).Value.Int);
            Assert.Equal((byte)1, oracle.Vector(0).Value.Byte);

            Assert.Equal(2, oracle.Vector(1).Value.Int);
            Assert.Equal((byte)2, oracle.Vector(1).Value.Byte);

            Assert.Equal(3, oracle.Vector(2).Value.Int);
            Assert.Equal((byte)3, oracle.Vector(2).Value.Byte);
        }

        [Fact]
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

            Assert.Equal(1, (int)oracle.ValueType);
            var bt = oracle.Value<Oracle.BasicTypes>().Value;

            Assert.True(bt.Bool);
            Assert.Equal(bt.Byte, simple.Byte);
            Assert.Equal(bt.SByte, simple.SByte);

            Assert.Equal(bt.UShort, simple.UShort);
            Assert.Equal(bt.Short, simple.Short);

            Assert.Equal(bt.UInt, simple.UInt);
            Assert.Equal(bt.Int, simple.Int);

            Assert.Equal(bt.ULong, simple.ULong);
            Assert.Equal(bt.Long, simple.Long);

            Assert.Equal(bt.Float, simple.Float);
            Assert.Equal(bt.Double, simple.Double);
            Assert.Equal(bt.String, simple.String);
        }

        [Fact]
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
            Assert.Equal(3, (int)oracle.ValueType);
        }

        [Fact]
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

            Assert.Equal(0, (int)oracle.ValueType);
        }

        [Fact]
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

            Assert.Equal(2, (int)oracle.ValueType);
            var bt = oracle.Value<Oracle.Location>().Value;

            Assert.Equal(bt.X, location.X);
            Assert.Equal(bt.Y, location.Y);
            Assert.Equal(bt.Z, location.Z);
        }

        [Fact]
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

            Assert.NotNull(oracle.Outer);

            var outer = oracle.Outer.Value;
            Assert.Equal(100, outer.A);
            Assert.Equal(401, outer.Inner.A);
        }

        [Fact]
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

            Assert.NotNull(oracle.Outer);

            var outer = oracle.Outer.Value;
            Assert.Equal(100, outer.A);
            Assert.Equal(0, outer.Inner.A);
        }

        [Fact]
        public void VectorOfUnion()
        {
            ArrayVectorOfUnionTable table = new ArrayVectorOfUnionTable
            {
                Union = new[]
                {
                    new FlatBufferUnion<BasicTypes, Location, string>(new BasicTypes { Int = 7 }),
                    new FlatBufferUnion<BasicTypes, Location, string>(new Location { X = 1, Y = 2, Z = 3 }),
                    new FlatBufferUnion<BasicTypes, Location, string>("foobar"),
                }
            };

            Span<byte> memory = new byte[10240];
            int size = FlatBufferSerializer.Default.Serialize(table, memory);

            var oracle = Oracle.UnionVectorTable.GetRootAsUnionVectorTable(
                new FlatBuffers.ByteBuffer(memory.Slice(0, size).ToArray()));

            Assert.Equal(3, oracle.ValueLength);

            Assert.Equal(Oracle.Union.BasicTypes, oracle.ValueType(0));
            Assert.Equal(Oracle.Union.Location, oracle.ValueType(1));
            Assert.Equal(Oracle.Union.stringValue, oracle.ValueType(2));

            var basicTypes = oracle.Value<Oracle.BasicTypes>(0).Value;
            Assert.Equal(7, basicTypes.Int);

            var location = oracle.Value<Oracle.Location>(1).Value;
            Assert.Equal(1, location.X);
            Assert.Equal(2, location.Y);
            Assert.Equal(3, location.Z);

            string stringValue = oracle.ValueAsString(2);
            Assert.Equal("foobar", stringValue);
        }

        [Fact]
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

        [Fact]
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
            Assert.Equal(5, myItem.Value);
            //Assert.Equal(18, bytesWritten); // table offset (4), vtable offset (4), vtable headers (6), data (4)

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

                Assert.True(comparer.Compare(previous, current) <= 0, $"Expect: {previous} <= {current}");
                Assert.True(comparer.Compare(previous, oracle) <= 0, $"Expect: {previous} <= {oracle}");
                Assert.True(comparer.Compare(current, oracle) == 0, $"Expect: {current} == {oracle}");

                // FlatBuffers c# has a bug where binary search is broken when using default values.
                // Assert.True(oracleContains(current));
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
