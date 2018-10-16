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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Binary format testing for vector serialization.
    /// </summary>
    [TestClass]
    public class VectorSerializationTests
    {
        [TestMethod]
        public void NullListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = null
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = new short[0],
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = new short[] { 1, 2, 3, }
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length

                // vector data
                1, 0,
                2, 0,
                3, 0
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyMemoryVector()
        {
            var root = new RootTable<Memory<bool>>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleMemoryVector()
        {
            var root = new RootTable<Memory<bool>>
            {
                Vector = new bool[] { true, false, true },
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length
                1, 0, 1,             // True false true
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullString()
        {
            var root = new RootTable<string>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyString()
        {
            var root = new RootTable<string>
            {
                Vector = string.Empty,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to string
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
                0,                   // null terminator (special case for strings).
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleString()
        {
            var root = new RootTable<string>
            {
                Vector = new string(new char[] { (char)1, (char)2, (char)3 }),
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length
                1, 2, 3, 0,          // data + null terminator (special case for string vectors).
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleArray()
        {
            var root = new RootTable<long[]>
            {
                Vector = new[] { 1L, 2, 3 }
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length

                // vector data
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 0, 0, 0, 0,
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullArray()
        {
            var root = new RootTable<int[]>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyArray()
        {
            var root = new RootTable<int[]>
            {
                Vector = new int[0],
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullStringInVector()
        {
            var root = new RootTable<IList<string>>
            {
                Vector = new string[] { "foobar", "banana", null, "two" },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<string>>>();

            byte[] target = new byte[10240];
            Assert.ThrowsException<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [TestMethod]
        public void NullStructInVector()
        {
            var root = new RootTable<IList<Struct>>
            {
                Vector = new[] { new Struct {  Integer = 1, }, null, new Struct { Integer = 3 } },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<Struct>>>();

            byte[] target = new byte[10240];
            Assert.ThrowsException<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }
        }

        [FlatBufferStruct]
        public class Struct
        {
            [FlatBufferItem(0)]
            public virtual int Integer { get; set; }
        }
    }
}
