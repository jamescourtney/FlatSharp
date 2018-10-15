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
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Binary format testing for vector deserialization.
    /// </summary>
    [TestClass]
    public class VectorDeserializationTests
    {
        [TestMethod]
        public void NullListVector()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);
            Assert.AreEqual(null, item.Vector);
        }

        [TestMethod]
        public void EmptyListVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);
            Assert.AreEqual(0, item.Vector.Count);
        }

        [TestMethod]
        public void SimpleListVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length,

                // vector data
                1, 0,
                2, 0,
                3, 0
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);

            Assert.AreEqual(3, item.Vector.Count);
            Assert.AreEqual(1, item.Vector[0]);
            Assert.AreEqual(2, item.Vector[1]);
            Assert.AreEqual(3, item.Vector[2]);
        }

        [TestMethod]
        public void EmptyMemoryVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };
            
            var item = FlatBufferSerializer.Default.Parse<RootTable<Memory<bool>>>(data);
            Assert.IsTrue(item.Vector.IsEmpty);
        }

        [TestMethod]
        public void SimpleMemoryVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length
                1, 0, 1,     // True false true
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<Memory<bool>>>(data);
            Assert.AreEqual(3, item.Vector.Length);
            Assert.IsTrue(item.Vector.Span[0]);
            Assert.IsFalse(item.Vector.Span[1]);
            Assert.IsTrue(item.Vector.Span[2]);
        }

        [TestMethod]
        public void NullString()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.AreEqual(null, item.Vector);
        }

        [TestMethod]
        public void EmptyString()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
                0,           // null terminator (special case for string vectors).
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.AreEqual(string.Empty, item.Vector);
        }

        [TestMethod]
        public void SimpleString()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length
                1, 2, 3, 0,  // data + null terminator (special case for string vectors).
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.AreEqual(3, item.Vector.Length);
            Assert.AreEqual(new string(new char[] { (char)1, (char)2, (char)3 }), item.Vector);
        }

        [TestMethod]
        public void NullArray()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<int[]>>(data);
            Assert.IsNull(item.Vector);
        }

        [TestMethod]
        public void EmptyArray()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };

            // we parse non-scalar array and scalar arrays differently, so test for both.
            var item = FlatBufferSerializer.Default.Parse<RootTable<int[]>>(data);
            Assert.AreEqual(0, item.Vector.Length);

            var stringItem = FlatBufferSerializer.Default.Parse<RootTable<string[]>>(data);
            Assert.AreEqual(0, stringItem.Vector.Length);
        }

        [TestMethod]
        public void SimpleArray()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length

                // vector data
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 0, 0, 0, 0,
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<long[]>>(data);
            Assert.AreEqual(3, item.Vector.Length);
            Assert.AreEqual(1L, item.Vector[0]);
            Assert.AreEqual(2L, item.Vector[1]);
            Assert.AreEqual(3L, item.Vector[2]);
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }
        }
    }
}
