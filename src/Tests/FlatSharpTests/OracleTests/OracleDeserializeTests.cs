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
    /// Does oracle-based testing using Google's flatbuffers code. These tests all boil down to:
    ///     Can we parse data we created using the official Google library?
    /// </summary>
    
    public partial class OracleDeserializeTests
    {
        [Fact]
        public void SimpleTypes()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var fbOffset = Oracle.BasicTypes.CreateBasicTypes(
                builder,
                Bool: true,
                Byte: GetRandom<byte>(),
                SByte: GetRandom<sbyte>(),
                UShort: GetRandom<ushort>(),
                Short: GetRandom<short>(),
                UInt: GetRandom<uint>(),
                Int: GetRandom<int>(),
                ULong: GetRandom<ulong>(),
                Long: GetRandom<long>(),
                Float: GetRandom<float>(),
                Double: GetRandom<double>(),
                StringOffset: builder.CreateString("foobar"));

            builder.Finish(fbOffset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();

            var oracle = Oracle.BasicTypes.GetRootAsBasicTypes(new FlatBuffers.ByteBuffer(realBuffer));
            
            var simple = FlatBufferSerializer.Default.Parse<BasicTypes>(realBuffer);

            foreach (var parsed in new[] { simple })
            {
                Assert.True(parsed.Bool);
                Assert.Equal(oracle.Byte, parsed.Byte);
                Assert.Equal(oracle.SByte, parsed.SByte);

                Assert.Equal(oracle.UShort, parsed.UShort);
                Assert.Equal(oracle.Short, parsed.Short);

                Assert.Equal(oracle.UInt, parsed.UInt);
                Assert.Equal(oracle.Int, parsed.Int);

                Assert.Equal(oracle.ULong, parsed.ULong);
                Assert.Equal(oracle.Long, parsed.Long);

                Assert.Equal(oracle.Float, parsed.Float);
                Assert.Equal(oracle.Double, parsed.Double);
                Assert.Equal("foobar", parsed.String);
            }

            // Ensures the caching works correctly.
            //Assert.ReferenceEquals(cached.String, cached.String);
            Assert.True(object.ReferenceEquals(simple.String, simple.String));
        }

        [Fact]
        public void LinkedList()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var testData = Oracle.LinkedListNode.CreateLinkedListNode(
                builder,
                builder.CreateString("node 1"),
                Oracle.LinkedListNode.CreateLinkedListNode(
                    builder,
                    builder.CreateString("node 2")));

            builder.Finish(testData.Value);

            byte[] realBuffer = builder.SizedByteArray();

            var linkedList = FlatBufferSerializer.Default.Parse<TestLinkedListNode>(realBuffer);

            Assert.NotNull(linkedList);
            Assert.NotNull(linkedList.Next);
            Assert.Null(linkedList.Next.Next);

            Assert.Equal("node 1", linkedList.Value);
            Assert.Equal("node 2", linkedList.Next.Value);
        }

        [Fact]
        public void ScalarVectors()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var testData = Oracle.Vectors.CreateVectors(
                builder,
                Oracle.Vectors.CreateIntVectorVector(builder, new[] { 1, 2, 3, 4, 5, 6, }),
                Oracle.Vectors.CreateLongVectorVector(builder, new[] { 7L, 8, 9, 10, 11, 12, }),
                Oracle.Vectors.CreateByteVector1Vector(builder, new byte[] { 1, 2, 3, 4, 5 }),
                Oracle.Vectors.CreateByteVector2Vector(builder, new byte[] { 1, 2, 3, 4, 5 }));

            builder.Finish(testData.Value);

            byte[] realBuffer = builder.SizedByteArray();

            var parsed = FlatBufferSerializer.Default.Parse<ScalarVectorsTable>(realBuffer);

            IList<int> intItems = parsed.IntVector;
            Assert.Equal(6, intItems.Count);
            for (int i = 0; i < 6; ++i)
            {
                Assert.Equal(1 + i, intItems[i]);
            }

            IList<long> longItems = parsed.LongVector;
            Assert.Equal(6, longItems.Count);
            for (int i = 0; i < 6; ++i)
            {
                Assert.Equal(7 + i, longItems[i]);
            }

            Memory<byte> mem = parsed.ByteVector2.Value;
            Assert.Equal(5, mem.Length);
            for (int i = 1; i <= 5; ++i)
            {
                Assert.Equal(i, mem.Span[i - 1]);
            }

            Assert.True(parsed.ByteVector3.IsEmpty);
        }

        [Fact]
        public void LocationStruct()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var fakeString = builder.CreateString("foobar");
            Oracle.LocationHolder.StartLocationVectorVector(builder, 3);
            Oracle.Location.CreateLocation(builder, 7f, 8f, 9f);
            Oracle.Location.CreateLocation(builder, 4f, 5f, 6f);
            Oracle.Location.CreateLocation(builder, 1f, 2f, 3f);
            var vectorOffset = builder.EndVector();

            Oracle.LocationHolder.StartLocationHolder(builder);
            Oracle.LocationHolder.AddFake(builder, fakeString);
            Oracle.LocationHolder.AddSingleLocation(builder, Oracle.Location.CreateLocation(builder, 0.1f, 0.2f, 0.3f));
            Oracle.LocationHolder.AddLocationVector(builder, vectorOffset);
            var testData = Oracle.LocationHolder.EndLocationHolder(builder);

            builder.Finish(testData.Value);

            byte[] realBuffer = builder.SizedByteArray();
            var parsed = FlatBufferSerializer.Default.Parse<LocationHolder>(realBuffer);

            Assert.Equal("foobar", parsed.Fake);
            Assert.Equal(0.1f, parsed.Location.X);
            Assert.Equal(0.2f, parsed.Location.Y);
            Assert.Equal(0.3f, parsed.Location.Z);
            Assert.Equal(3, parsed.LocationVector.Count);

            for (int i = 0; i < 3; ++i)
            {
                Assert.Equal((float)(3 * i + 1), parsed.LocationVector[i].X);
                Assert.Equal((float)(3 * i + 2), parsed.LocationVector[i].Y);
                Assert.Equal((float)(3 * i + 3), parsed.LocationVector[i].Z);
            }
        }

        [Fact]
        public void FiveByteStructVector()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            Oracle.FiveByteStructTable.StartVectorVector(builder, 3);
            Oracle.FiveByteStruct.CreateFiveByteStruct(builder, 3, 3);
            Oracle.FiveByteStruct.CreateFiveByteStruct(builder, 2, 2);
            Oracle.FiveByteStruct.CreateFiveByteStruct(builder, 1, 1);
            var vectorOffset = builder.EndVector();

            Oracle.FiveByteStructTable.StartFiveByteStructTable(builder);
            Oracle.FiveByteStructTable.AddVector(builder, vectorOffset);
            var testData = Oracle.FiveByteStructTable.EndFiveByteStructTable(builder);

            builder.Finish(testData.Value);

            byte[] realBuffer = builder.SizedByteArray();
            var parsed = FlatBufferSerializer.Default.Parse<FiveByteStructTable>(realBuffer);

            Assert.Equal(3, parsed.Vector.Length);

            Assert.Equal(1, parsed.Vector[0].Int);
            Assert.Equal(2, parsed.Vector[1].Int);
            Assert.Equal(3, parsed.Vector[2].Int);

            Assert.Equal((byte)1, parsed.Vector[0].Byte);
            Assert.Equal((byte)2, parsed.Vector[1].Byte);
            Assert.Equal((byte)3, parsed.Vector[2].Byte);
        }

        [Fact]
        public void Union_Table_BasicTypes()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var basicTypesOffset = Oracle.BasicTypes.CreateBasicTypes(
                builder,
                Bool: true,
                Byte: GetRandom<byte>(),
                SByte: GetRandom<sbyte>(),
                UShort: GetRandom<ushort>(),
                Short: GetRandom<short>(),
                UInt: GetRandom<uint>(),
                Int: GetRandom<int>(),
                ULong: GetRandom<ulong>(),
                Long: GetRandom<long>(),
                Float: GetRandom<float>(),
                Double: GetRandom<double>(),
                StringOffset: builder.CreateString("foobar"));

            var offset = Oracle.UnionTable.CreateUnionTable(
                builder,
                Oracle.Union.BasicTypes,
                basicTypesOffset.Value);

            builder.Finish(offset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();

            var oracle = Oracle.UnionTable.GetRootAsUnionTable(new FlatBuffers.ByteBuffer(realBuffer)).Value<Oracle.BasicTypes>().Value;
            var unionTable = FlatBufferSerializer.Default.Parse<UnionTable>(realBuffer);

            Assert.Equal(1, unionTable.Union.Value.Discriminator);
            BasicTypes parsed = unionTable.Union.Value.Item1;
            Assert.NotNull(parsed);

            Assert.True(parsed.Bool);
            Assert.Equal(oracle.Byte, parsed.Byte);
            Assert.Equal(oracle.SByte, parsed.SByte);

            Assert.Equal(oracle.UShort, parsed.UShort);
            Assert.Equal(oracle.Short, parsed.Short);

            Assert.Equal(oracle.UInt, parsed.UInt);
            Assert.Equal(oracle.Int, parsed.Int);

            Assert.Equal(oracle.ULong, parsed.ULong);
            Assert.Equal(oracle.Long, parsed.Long);

            Assert.Equal(oracle.Float, parsed.Float);
            Assert.Equal(oracle.Double, parsed.Double);
            Assert.Equal("foobar", parsed.String);
        }

        [Fact]
        public void Union_Struct_Location()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var locationOffset = Oracle.Location.CreateLocation(
                builder,
                1.0f,
                2.0f,
                3.0f);

            var offset = Oracle.UnionTable.CreateUnionTable(
                builder,
                Oracle.Union.Location,
                locationOffset.Value);

            builder.Finish(offset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();
            var unionTable = FlatBufferSerializer.Default.Parse<UnionTable>(realBuffer);

            Assert.Equal(2, unionTable.Union.Value.Discriminator);
            Location parsed = unionTable.Union.Value.Item2;
            Assert.NotNull(parsed);

            Assert.Equal(1.0f, parsed.X);
            Assert.Equal(2.0f, parsed.Y);
            Assert.Equal(3.0f, parsed.Z);
        }

        [Fact]
        public void Union_String()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var stringOffset = builder.CreateString("foobar");

            var offset = Oracle.UnionTable.CreateUnionTable(
                builder,
                Oracle.Union.stringValue,
                stringOffset.Value);

            builder.Finish(offset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();
            var unionTable = FlatBufferSerializer.Default.Parse<UnionTable>(realBuffer);

            Assert.Equal(3, unionTable.Union.Value.Discriminator);
            string parsed = unionTable.Union.Value.Item3;
            Assert.Equal("foobar", parsed);
        }

        [Fact]
        public void Union_NotSet()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);

            var offset = Oracle.UnionTable.CreateUnionTable(builder);

            builder.Finish(offset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();

            var unionTable = FlatBufferSerializer.Default.Parse<UnionTable>(realBuffer);
            Assert.Null(unionTable.Union);
        }

        [Fact]
        public void NestedStruct()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var outerOffset = Oracle.OuterStruct.CreateOuterStruct(builder, 401, 100);
            Oracle.NestedStructs.StartNestedStructs(builder);
            Oracle.NestedStructs.AddOuter(builder, outerOffset);
            var offset = Oracle.NestedStructs.EndNestedStructs(builder);
            builder.Finish(offset.Value);

            byte[] realBuffer = builder.DataBuffer.ToSizedArray();
            
            var parsed = FlatBufferSerializer.Default.Parse<NestedStructs>(realBuffer);

            Assert.NotNull(parsed?.OuterStruct?.InnerStruct);

            Assert.Equal(401, parsed.OuterStruct.InnerStruct.A);
            Assert.Equal(100, parsed.OuterStruct.A);
        }


        [Fact]
        public void VectorOfUnion()
        {
            Oracle.VectorOfUnionTableT table = new Oracle.VectorOfUnionTableT
            {
                Value = new List<Oracle.UnionUnion>
                {
                    new Oracle.UnionUnion { Value = new Oracle.BasicTypesT { Int = 7 }, Type = Oracle.Union.BasicTypes },
                    new Oracle.UnionUnion { Value = new Oracle.LocationT { X = 1, Y = 2, Z = 3 }, Type = Oracle.Union.Location },
                    new Oracle.UnionUnion { Value = "foobar", Type = Oracle.Union.stringValue },
                }
            };

            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var offset = Oracle.VectorOfUnionTable.Pack(builder, table);
            builder.Finish(offset.Value);
            byte[] data = builder.SizedByteArray();

            var parsed = FlatBufferSerializer.Default.Parse<ArrayVectorOfUnionTable>(data);

            Assert.Equal(3, parsed.Union.Length);

            Assert.Equal(1, parsed.Union[0].Discriminator);
            Assert.Equal(2, parsed.Union[1].Discriminator);
            Assert.Equal(3, parsed.Union[2].Discriminator);

            Assert.True(parsed.Union[0].TryGet(out BasicTypes basicTypes));
            Assert.Equal(7, basicTypes.Int);

            Assert.True(parsed.Union[1].TryGet(out Location location));
            Assert.Equal(1, location.X);
            Assert.Equal(2, location.Y);
            Assert.Equal(3, location.Z);

            Assert.True(parsed.Union[2].TryGet(out string str));
            Assert.Equal("foobar", str);
        }

        [Fact]
        public void SortedVectors()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024 * 1024);

            var strings = new List<string>();
            var stringOffsets = new List<FlatBuffers.Offset<Oracle.SortedVectorStringTable>>();

            List<int> ints = new List<int>();
            var intOffsets = new List<FlatBuffers.Offset<Oracle.SortedVectorInt32Table>>();

            List<double> doubles = new List<double>();
            var doubleOffsets = new List<FlatBuffers.Offset<Oracle.SortedVectorDoubleTable>>();

            const int Iterations = 1000;
            Random random = new Random();

            for (int i = 0; i < Iterations; ++i)
            {
                string value = Guid.NewGuid().ToString();
                strings.Add(value);
                stringOffsets.Add(Oracle.SortedVectorStringTable.CreateSortedVectorStringTable(builder, builder.CreateString(value)));
            }

            for (int i = 0; i < Iterations; ++i)
            {
                int value = random.Next();
                ints.Add(value);
                intOffsets.Add(Oracle.SortedVectorInt32Table.CreateSortedVectorInt32Table(builder, value));
            }

            for (int i = 0; i < Iterations; ++i)
            {
                double value = random.NextDouble() * random.Next();
                doubles.Add(value);
                doubleOffsets.Add(Oracle.SortedVectorDoubleTable.CreateSortedVectorDoubleTable(builder, value));
            }

            var table = Oracle.SortedVectorTest.CreateSortedVectorTest(
                builder,
                Oracle.SortedVectorInt32Table.CreateSortedVectorOfSortedVectorInt32Table(builder, intOffsets.ToArray()),
                Oracle.SortedVectorStringTable.CreateSortedVectorOfSortedVectorStringTable(builder, stringOffsets.ToArray()),
                Oracle.SortedVectorDoubleTable.CreateSortedVectorOfSortedVectorDoubleTable(builder, doubleOffsets.ToArray()));

            builder.Finish(table.Value);
            byte[] serialized = builder.SizedByteArray();

            var parsed = FlatBufferSerializer.Default.Parse<SortedVectorTest<SortedVectorItem<int>>>(serialized);

            VerifySorted(parsed.StringVector, new Utf8StringComparer(), strings, new List<string> { Guid.NewGuid().ToString(), "banana" });
            VerifySorted(parsed.IntVector, Comparer<int>.Default, ints, new List<int> { -1, -3, 0 });
            VerifySorted(parsed.Double, Comparer<double>.Default, doubles, new List<double> { Math.PI, Math.E, Math.Sqrt(2) });
        }

        [Fact]
        public void SortedVectors_NullKey_NotAllowed()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024 * 1024);

            var strings = new List<string>();
            var stringOffsets = new List<FlatBuffers.Offset<Oracle.SortedVectorStringTable>>();

            foreach (string s in new[] { Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString() })
            {
                strings.Add(s);
                FlatBuffers.StringOffset strOffset = default;
                if (s != null)
                {
                    strOffset = builder.CreateString(s);
                }

                stringOffsets.Add(Oracle.SortedVectorStringTable.CreateSortedVectorStringTable(builder, strOffset));
            }

            Assert.Throws<InvalidOperationException>(
                () => Oracle.SortedVectorStringTable.CreateSortedVectorOfSortedVectorStringTable(builder, stringOffsets.ToArray()));
        }

        private static void VerifySorted<T>(IList<SortedVectorItem<T>> items, IComparer<T> comparer, List<T> expectedItems, List<T> unexpectedItems)
        {
            T previous = items[0].Value;
            for (int i = 1; i < items.Count; ++i)
            {
                T current = items[i].Value;
                Assert.True(comparer.Compare(previous, current) <= 0);
            }

            foreach (var expectedItem in expectedItems)
            {
                SortedVectorItem<T> item = items.BinarySearchByFlatBufferKey(expectedItem);
                Assert.NotNull(item);
            }

            foreach (var unexpectedItem in unexpectedItems)
            {
                Assert.Null(items.BinarySearchByFlatBufferKey(unexpectedItem));
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
