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
    using System.Collections.ObjectModel;
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

        [TestMethod]
        public void StringVector_GreedyDeserialize_Mutable()
        {
            RootTable<IList<string>> root = new RootTable<IList<string>>
            {
                Vector = new List<string> { "one", "two", "three" }
            };

            var options = new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GenerateMutableObjects | FlatBufferSerializerFlags.GreedyDeserialize);
            FlatBufferSerializer serializer = new FlatBufferSerializer(options);

            byte[] buffer = new byte[100];
            serializer.Serialize(root, buffer);

            var parsed = serializer.Parse<RootTable<IList<string>>>(buffer);

            Assert.AreEqual(typeof(List<string>), parsed.Vector.GetType());
            Assert.IsFalse(parsed.Vector.IsReadOnly);

            // Shouldn't throw.
            parsed.Vector.Add("four");
        }

        [TestMethod]
        public void StringVector_GreedyDeserialize_NotMutable()
        {
            RootTable<IList<string>> root = new RootTable<IList<string>>
            {
                Vector = new List<string> { "one", "two", "three" }
            };

            var options = new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GreedyDeserialize);
            FlatBufferSerializer serializer = new FlatBufferSerializer(options);

            byte[] buffer = new byte[100];
            serializer.Serialize(root, buffer);

            var parsed = serializer.Parse<RootTable<IList<string>>>(buffer);

            Assert.AreEqual(typeof(ReadOnlyCollection<string>), parsed.Vector.GetType());
            Assert.IsTrue(parsed.Vector.IsReadOnly);

            Assert.ThrowsException<NotSupportedException>(() => parsed.Vector.Add("four"));
        }

        [TestMethod]
        public void MemoryVector_GreedyDeserialize()
        {
            RootTable<Memory<byte>> root = new RootTable<Memory<byte>>()
            {
                Vector = new Memory<byte>(new byte[100])
            };

            root.Vector.Span.Fill(1);

            byte[] buffer = new byte[1024];
            var options = new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GreedyDeserialize);
            var serializer = new FlatBufferSerializer(options);

            serializer.Serialize(root, buffer.AsSpan());

            var parsed1 = serializer.Parse<RootTable<Memory<byte>>>(buffer);
            var parsed2 = serializer.Parse<RootTable<Memory<byte>>>(buffer);

            Assert.AreEqual((byte)1, parsed1.Vector.Span[0]);
            Assert.AreEqual((byte)1, parsed2.Vector.Span[0]);

            // Asser that this change affects only the 'parsed1' object.
            parsed1.Vector.Span[0] = 2;

            Assert.AreEqual((byte)2, parsed1.Vector.Span[0]);
            Assert.AreEqual((byte)1, parsed2.Vector.Span[0]);
        }

        /// <summary>
        /// Asserts that when greedy parsing is off, a change to the memory of the parsed object represents a change in the buffer.
        /// </summary>
        [TestMethod]
        public void MemoryVector_LazyDeserialize()
        {
            RootTable<Memory<byte>> root = new RootTable<Memory<byte>>()
            {
                Vector = new Memory<byte>(new byte[100])
            };

            root.Vector.Span.Fill(1);

            byte[] buffer = new byte[1024];
            var options = new FlatBufferSerializerOptions(FlatBufferSerializerFlags.None);
            var serializer = new FlatBufferSerializer(options);

            serializer.Serialize(root, buffer.AsSpan());

            var parsed1 = serializer.Parse<RootTable<Memory<byte>>>(buffer);
            var parsed2 = serializer.Parse<RootTable<Memory<byte>>>(buffer);

            Assert.AreEqual((byte)1, parsed1.Vector.Span[0]);
            Assert.AreEqual((byte)1, parsed2.Vector.Span[0]);

            // Asser that this change affects both objects.
            parsed1.Vector.Span[0] = 2;

            Assert.AreEqual((byte)2, parsed1.Vector.Span[0]);
            Assert.AreEqual((byte)2, parsed2.Vector.Span[0]);
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }
        }
    }
}
