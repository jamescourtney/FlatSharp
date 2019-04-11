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
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    [TestClass]
    public class UnionTests
    {
        [TestMethod]
        public void Union_2Items_TableAndStruct_RoundTrip()
        {
            var expectedStruct = new SimpleStruct { Long = 123 };
            var expectedTable = new SimpleTable { Int = 456 };

            var testStruct = CreateAndDeserialize1(expectedStruct, expectedTable);
            Assert.AreEqual(testStruct.Long, 123);

            var testTable = CreateAndDeserialize2(expectedStruct, expectedTable);
            Assert.AreEqual(testTable.Int, 456);
        }

        [TestMethod]
        public void Union_2Items_Struct_SerializationTest()
        {
            byte[] expectedData =
            {
                4, 0, 0, 0,         // offset to table
                244, 255, 255, 255, // soffset to vtable
                2,                  // discriminator (1 byte)
                0, 0, 0,            // padding
                12, 0, 0, 0,        // uoffset_t to struct data
                8, 0,               // vtable length
                12, 0,              // table length
                4, 0,               // discriminator offset
                8, 0,               // value offset

                // struct data
                123, 0, 0, 0, 0, 0, 0, 0  
            };

            var union = new UnionTable<string, SimpleStruct>
            {
                Item = new FlatBufferUnion<string, SimpleStruct>(new SimpleStruct { Long = 123 })
            };

            Span<byte> data = new byte[100];
            int count = FlatBufferSerializer.Default.Serialize(union, data);

            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(data.Slice(0, count)));
        }

        [TestMethod]
        public void Union_2Items_Table_SerializationTest()
        {
            byte[] expectedData =
            {
                4, 0, 0, 0,         // offset to table
                244, 255, 255, 255, // soffset to vtable
                1,                  // discriminator (1 byte)
                0, 0, 0,            // padding
                12, 0, 0, 0,        // uoffset_t to table data
                8, 0,               // vtable length
                12, 0,              // table length
                4, 0,               // discriminator offset
                8, 0,               // value offset

                // table data
                248, 255, 255, 255, // soffset to vtable
                123, 0, 0, 0,       // value of index 0
                6, 0,               // vtable length
                8, 0,               // table length
                4, 0                // offset of index 0
            };

            var union = new UnionTable<SimpleTable, SimpleStruct>
            {
                Item = new FlatBufferUnion<SimpleTable, SimpleStruct>(new SimpleTable { Int = 123 })
            };

            Span<byte> data = new byte[100];
            int count = FlatBufferSerializer.Default.Serialize(union, data);

            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(data.Slice(0, count)));
        }

        [TestMethod]
        public void Union_2Items_StringAndTable_RoundTrip()
        {
            var expectedString = "foobar";
            var expectedInt = 123;
            var expectedTable = new SimpleTable { Int = expectedInt };

            Assert.AreEqual(expectedString, CreateAndDeserialize1(expectedString, expectedTable));
            Assert.AreEqual(expectedInt, CreateAndDeserialize2(expectedString, expectedTable).Int);
        }

        private static T1 CreateAndDeserialize1<T1, T2>(T1 one, T2 two)
        {
            var table = new UnionTable<T1, T2>
            {
                Item = new FlatBufferUnion<T1, T2>(one)
            };

            byte[] buffer = new byte[1024];

            FlatBufferSerializer.Default.Serialize(table, buffer);

            var parseResult = FlatBufferSerializer.Default.Parse<UnionTable<T1, T2>>(buffer);
            Assert.AreEqual(1, parseResult.Item.Discriminator);

            return parseResult.Item.Item1;
        }

        private static T2 CreateAndDeserialize2<T1, T2>(T1 one, T2 two)
        {
            var table = new UnionTable<T1, T2>
            {
                Item = new FlatBufferUnion<T1, T2>(two)
            };

            byte[] buffer = new byte[1024];

            FlatBufferSerializer.Default.Serialize(table, buffer);

            var parseResult = FlatBufferSerializer.Default.Parse<UnionTable<T1, T2>>(buffer);
            Assert.AreEqual(2, parseResult.Item.Discriminator);

            return parseResult.Item.Item2;
        }

        [FlatBufferTable]
        public class UnionTable<T1, T2>
        {
            [FlatBufferItem(0)]
            public virtual FlatBufferUnion<T1, T2> Item { get; set; }
        }

        [FlatBufferTable]
        public class SimpleTable
        {
            [FlatBufferItem(0)]
            public virtual int Int { get; set; }
        }

        [FlatBufferStruct]
        public class SimpleStruct
        {
            [FlatBufferItem(0)]
            public virtual long Long { get; set; }
        }
    }
}
