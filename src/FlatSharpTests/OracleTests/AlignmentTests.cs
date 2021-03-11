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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AlignmentTests
    {
        [TestMethod]
        public void NestedStructWithOddAlignment_Parse()
        {
            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var offset = Oracle.AlignmentTestOuter.CreateAlignmentTestOuter(builder, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Oracle.AlignmentTestOuterHoder.StartAlignmentTestOuterHoder(builder);
            Oracle.AlignmentTestOuterHoder.AddValue(builder, offset);
            var testData = Oracle.AlignmentTestOuterHoder.EndAlignmentTestOuterHoder(builder);
            builder.Finish(testData.Value);

            byte[] realBuffer = builder.SizedByteArray();
            var parsed = FlatBufferSerializer.Default.Parse<AlignmentTestDataHolder>(realBuffer);

            Assert.IsNotNull(parsed);

            var outer = parsed.Value;
            Assert.IsNotNull(outer);
            Assert.AreEqual(1, outer.A);
            Assert.AreEqual(2, outer.B);
            Assert.AreEqual(3, outer.C);
            Assert.AreEqual(4u, outer.D);
            Assert.AreEqual(5, outer.E);
            Assert.AreEqual(6ul, outer.F);

            var inner = outer.Inner;
            Assert.IsNotNull(inner);
            Assert.AreEqual(7, inner.A);
            Assert.AreEqual(8, inner.B);
            Assert.AreEqual(9, inner.C);
        }

        [TestMethod]
        public void NestedStructWithOddAlignment_Serialize()
        {
            AlignmentTestDataHolder holder = new AlignmentTestDataHolder
            {
                Value = new AlignmentTestOuter
                {
                    A = 1,
                    B = 2,
                    C = 3,
                    D = 4,
                    E = 5,
                    F = 6,
                    Inner = new AlignmentTestInner
                    {
                        A = 7,
                        B = 8,
                        C = 9,
                    }
                }
            };

            Span<byte> memory = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(holder, memory);

            var parsed = Oracle.AlignmentTestOuterHoder.GetRootAsAlignmentTestOuterHoder(new FlatBuffers.ByteBuffer(memory.Slice(0, offset).ToArray()));

            Assert.IsNotNull(parsed);

            var outer = parsed.Value.Value;
            Assert.IsNotNull(outer);
            Assert.AreEqual(1, outer.A);
            Assert.AreEqual(2, outer.B);
            Assert.AreEqual(3, outer.C);
            Assert.AreEqual(4u, outer.D);
            Assert.AreEqual(5, outer.E);
            Assert.AreEqual(6ul, outer.F);

            var inner = outer.G;
            Assert.IsNotNull(inner);
            Assert.AreEqual(7, inner.A);
            Assert.AreEqual(8, inner.B);
            Assert.AreEqual(9, inner.C);
        }

        [TestMethod]
        public void StructVectorDeserialize()
        {
            Oracle.StructVectorsTableT table = new Oracle.StructVectorsTableT
            {
                Vec = new Oracle.StructVectorsT
                {
                    HashVec = new ulong[] { 1, 2, 3, 4 },
                    AlignmentVec = new Oracle.FiveByteStructT[]
                    {
                        new() { Int = 5, Byte = 6, },
                        new() { Int = 7, Byte = 8, },
                        new() { Int = 9, Byte = 10, },
                    }
                }
            };

            var builder = new FlatBuffers.FlatBufferBuilder(1024);
            var offset = Oracle.StructVectorsTable.Pack(builder, table);
            builder.Finish(offset.Value);

            byte[] serialized = builder.SizedByteArray();

            var parsed = FlatBufferSerializer.Default.Parse<StructVectorsTable>(serialized);

            Assert.AreEqual<ulong>(1, parsed.Vectors.Hash_0);
            Assert.AreEqual<ulong>(2, parsed.Vectors.Hash_1);
            Assert.AreEqual<ulong>(3, parsed.Vectors.Hash_2);
            Assert.AreEqual<ulong>(4, parsed.Vectors.Hash_3);

            Assert.AreEqual(5, parsed.Vectors.AlignmentVec_0.Int);
            Assert.AreEqual(6, parsed.Vectors.AlignmentVec_0.Byte);

            Assert.AreEqual(7, parsed.Vectors.AlignmentVec_1.Int);
            Assert.AreEqual(8, parsed.Vectors.AlignmentVec_1.Byte);

            Assert.AreEqual(9, parsed.Vectors.AlignmentVec_2.Int);
            Assert.AreEqual(10, parsed.Vectors.AlignmentVec_2.Byte);
        }

        [TestMethod]
        public void StructVectorSerialize()
        {
            StructVectorsTable table = new StructVectorsTable
            {
                Vectors = new StructVectors
                {
                    Hash_0 = 1,
                    Hash_1 = 2,
                    Hash_2 = 3,
                    Hash_3 = 4,
                    AlignmentVec_0 = new() { Int = 5, Byte = 6 },
                    AlignmentVec_1 = new() { Int = 7, Byte = 8 },
                    AlignmentVec_2 = new() { Int = 9, Byte = 10 },
                }
            };

            byte[] data = new byte[1024];
            FlatBufferSerializer.Default.Serialize(table, data);

            var parsed = Oracle.StructVectorsTable.GetRootAsStructVectorsTable(new FlatBuffers.ByteBuffer(data)).UnPack();

            Assert.AreEqual<ulong>(1, parsed.Vec.HashVec[0]);
            Assert.AreEqual<ulong>(2, parsed.Vec.HashVec[1]);
            Assert.AreEqual<ulong>(3, parsed.Vec.HashVec[2]);
            Assert.AreEqual<ulong>(4, parsed.Vec.HashVec[3]);

            Assert.AreEqual(5, parsed.Vec.AlignmentVec[0].Int);
            Assert.AreEqual(6, parsed.Vec.AlignmentVec[0].Byte);

            Assert.AreEqual(7, parsed.Vec.AlignmentVec[1].Int);
            Assert.AreEqual(8, parsed.Vec.AlignmentVec[1].Byte);

            Assert.AreEqual(9, parsed.Vec.AlignmentVec[2].Int);
            Assert.AreEqual(10, parsed.Vec.AlignmentVec[2].Byte);
        }

        [FlatBufferTable]
        public class StructVectorsTable
        {
            [FlatBufferItem(0)]
            public StructVectors? Vectors { get; set; }
        }

        /*
         * struct StructVectors {
         *    AlignmentVec:[FiveByteStruct:3];
         *    HashVec:[ulong:4];
         *  } 
         */
        [FlatBufferStruct]
        public class StructVectors
        {
            [FlatBufferItem(0)]
            public FiveByteStruct AlignmentVec_0 { get; set; }

            [FlatBufferItem(1)]
            public FiveByteStruct AlignmentVec_1 { get; set; }

            [FlatBufferItem(2)]
            public FiveByteStruct AlignmentVec_2 { get; set; }

            [FlatBufferItem(3)]
            public ulong Hash_0 { get; set; }

            [FlatBufferItem(4)]
            public ulong Hash_1 { get; set; }

            [FlatBufferItem(5)]
            public ulong Hash_2 { get; set; }

            [FlatBufferItem(6)]
            public ulong Hash_3 { get; set; }
        }

        [FlatBufferTable]
        public class AlignmentTestDataHolder
        {
            [FlatBufferItem(0)]
            public virtual AlignmentTestOuter? Value { get; set; }
        }

        /// <summary>
        /// Declares a very inefficient struct with the following schema:
        /// {
        ///    byte    (1)
        ///    padding (1)
        ///    ushort  (2)
        ///    ----
        ///    byte    (1)
        ///    padding (3)
        ///    ----
        ///    uint    (4)
        ///    ----
        ///    byte    (1)
        ///    padding (3)
        ///    ----
        ///    ulong    (8)
        ///    ----
        ///    (substructure)
        /// </summary>
        [FlatBufferStruct]
        public class AlignmentTestOuter
        {
            [FlatBufferItem(0)]
            public virtual byte A { get; set; }

            [FlatBufferItem(1)]
            public virtual ushort B { get; set; }

            [FlatBufferItem(2)]
            public virtual byte C { get; set; }

            [FlatBufferItem(3)]
            public virtual uint D { get; set; }

            [FlatBufferItem(4)]
            public virtual byte E { get; set; }

            [FlatBufferItem(5)]
            public virtual ulong F { get; set; }

            [FlatBufferItem(6)]
            public virtual AlignmentTestInner Inner { get; set; }
        }

        /// <summary>
        /// Declared out of order.
        /// </summary>
        [FlatBufferStruct]
        public class AlignmentTestInner
        {
            [FlatBufferItem(2)]
            public virtual sbyte C { get; set; }

            [FlatBufferItem(0)]
            public virtual byte A { get; set; }

            [FlatBufferItem(1)]
            public virtual int B { get; set; }
        }
    }
}
