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
    /// Does oracle-based testing using Google's flatbuffers code. These tests all boil down to:
    ///     Can we parse data we created using the official Google library?
    /// </summary>
    [TestClass]
    public partial class OracleDeserializeTests
    {
        [TestMethod]
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
            var simpleUnsafe = FlatBufferSerializer.Default.Parse<BasicTypes>(new UnsafeMemoryInputBuffer(realBuffer));
            var simpleUnsafeArray = FlatBufferSerializer.Default.Parse<BasicTypes>(new UnsafeArrayInputBuffer(realBuffer));

            foreach (var parsed in new[] { simple, simpleUnsafe, simpleUnsafeArray })
            {
                Assert.IsTrue(parsed.Bool);
                Assert.AreEqual(oracle.Byte, parsed.Byte);
                Assert.AreEqual(oracle.SByte, parsed.SByte);

                Assert.AreEqual(oracle.UShort, parsed.UShort);
                Assert.AreEqual(oracle.Short, parsed.Short);

                Assert.AreEqual(oracle.UInt, parsed.UInt);
                Assert.AreEqual(oracle.Int, parsed.Int);

                Assert.AreEqual(oracle.ULong, parsed.ULong);
                Assert.AreEqual(oracle.Long, parsed.Long);

                Assert.AreEqual(oracle.Float, parsed.Float);
                Assert.AreEqual(oracle.Double, parsed.Double);
                Assert.AreEqual("foobar", parsed.String);
            }

            // Ensures the caching works correctly.
            //Assert.ReferenceEquals(cached.String, cached.String);
            Assert.IsTrue(object.ReferenceEquals(simple.String, simple.String));
        }

        [TestMethod]
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

            Assert.IsNotNull(linkedList);
            Assert.IsNotNull(linkedList.Next);
            Assert.IsNull(linkedList.Next.Next);

            Assert.AreEqual("node 1", linkedList.Value);
            Assert.AreEqual("node 2", linkedList.Next.Value);
        }

        [TestMethod]
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
            Assert.AreEqual(6, intItems.Count);
            for (int i = 0; i < 6; ++i)
            {
                Assert.AreEqual(1 + i, intItems[i]);
            }

            IList<long> longItems = parsed.LongVector;
            Assert.AreEqual(6, longItems.Count);
            for (int i = 0; i < 6; ++i)
            {
                Assert.AreEqual(7 + i, longItems[i]);
            }

            Memory<byte> mem = parsed.ByteVector2;
            Assert.AreEqual(5, mem.Length);
            for (int i = 1; i <= 5; ++i)
            {
                Assert.AreEqual(i, mem.Span[i - 1]);
            }

            Assert.IsTrue(parsed.ByteVector3.IsEmpty);
        }

        [TestMethod]
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

            Assert.AreEqual(parsed.Fake, "foobar");
            Assert.AreEqual(parsed.Location.X, 0.1f);
            Assert.AreEqual(parsed.Location.Y, 0.2f);
            Assert.AreEqual(parsed.Location.Z, 0.3f);

            Assert.AreEqual(parsed.LocationVector.Count, 3);
            for (int i = 0; i < 3; ++i)
            {
                Assert.AreEqual((float)(3 * i + 1), parsed.LocationVector[i].X);
                Assert.AreEqual((float)(3 * i + 2), parsed.LocationVector[i].Y);
                Assert.AreEqual((float)(3 * i + 3), parsed.LocationVector[i].Z);
            }
        }

        [TestMethod]
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

            Assert.AreEqual(3, parsed.Vector.Length);

            Assert.AreEqual(1, parsed.Vector[0].Int);
            Assert.AreEqual(2, parsed.Vector[1].Int);
            Assert.AreEqual(3, parsed.Vector[2].Int);

            Assert.AreEqual((byte)1, parsed.Vector[0].Byte);
            Assert.AreEqual((byte)2, parsed.Vector[1].Byte);
            Assert.AreEqual((byte)3, parsed.Vector[2].Byte);
        }

        [TestMethod]
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

            Assert.AreEqual(1, unionTable.Union.Discriminator);
            BasicTypes parsed = unionTable.Union.Item1;
            Assert.IsNotNull(parsed);

            Assert.IsTrue(parsed.Bool);
            Assert.AreEqual(oracle.Byte, parsed.Byte);
            Assert.AreEqual(oracle.SByte, parsed.SByte);

            Assert.AreEqual(oracle.UShort, parsed.UShort);
            Assert.AreEqual(oracle.Short, parsed.Short);

            Assert.AreEqual(oracle.UInt, parsed.UInt);
            Assert.AreEqual(oracle.Int, parsed.Int);

            Assert.AreEqual(oracle.ULong, parsed.ULong);
            Assert.AreEqual(oracle.Long, parsed.Long);

            Assert.AreEqual(oracle.Float, parsed.Float);
            Assert.AreEqual(oracle.Double, parsed.Double);
            Assert.AreEqual("foobar", parsed.String);
        }

        [TestMethod]
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

            Assert.AreEqual(2, unionTable.Union.Discriminator);
            Location parsed = unionTable.Union.Item2;
            Assert.IsNotNull(parsed);

            Assert.AreEqual(1.0f, parsed.X);
            Assert.AreEqual(2.0f, parsed.Y);
            Assert.AreEqual(3.0f, parsed.Z);
        }

        [TestMethod]
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

            Assert.AreEqual(3, unionTable.Union.Discriminator);
            string parsed = unionTable.Union.Item3;
            Assert.AreEqual("foobar", parsed);
        }

        [TestMethod]
        public void Union_NotSet()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);

            var offset = Oracle.UnionTable.CreateUnionTable(builder);

            builder.Finish(offset.Value);
            byte[] realBuffer = builder.DataBuffer.ToSizedArray();

            var unionTable = FlatBufferSerializer.Default.Parse<UnionTable>(realBuffer);
            Assert.IsNull(unionTable.Union);
        }

        [TestMethod]
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

            Assert.IsNotNull(parsed?.OuterStruct?.InnerStruct);

            Assert.AreEqual(401, parsed.OuterStruct.InnerStruct.A);
            Assert.AreEqual(100, parsed.OuterStruct.A);
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
