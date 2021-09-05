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
    using Xunit;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    public class UnionTests
    {
        [Fact]
        public void Union_Default_Fails()
        {
            UnionTable<SimpleTable, SimpleStruct> table = new()
            {
                Item = default(FlatBufferUnion<SimpleTable, SimpleStruct>)
            };

            Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.GetMaxSize(table));
            Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(table, new byte[1024]));
        }

        [Fact]
        public void Union_2Items_TableAndStruct_RoundTrip()
        {
            var expectedStruct = new SimpleStruct { Long = 123 };
            var expectedTable = new SimpleTable { Int = 456 };

            var testStruct = CreateAndDeserialize1(expectedStruct, expectedTable);
            Assert.Equal(123, testStruct.Long);

            var testTable = CreateAndDeserialize2(expectedStruct, expectedTable);
            Assert.Equal(456, testTable.Int);
        }

        [Fact]
        public void Union_2Items_Struct_SerializationTest()
        {
            byte[] expectedData =
            {
                4, 0, 0, 0,         // offset to table
                246, 255, 255, 255, // soffset to vtable
                16, 0, 0, 0,        // uoffset_t to struct data
                2, 0,               // discriminator (1 byte), padding
                8, 0,               // vtable length
                9, 0,               // table length
                8, 0,               // discriminator offset
                4, 0,               // value offset

                0, 0,               // padding

                // struct data
                123, 0, 0, 0, 0, 0, 0, 0  
            };

            var union = new UnionTable<string, SimpleStruct>
            {
                Item = new FlatBufferUnion<string, SimpleStruct>(new SimpleStruct { Long = 123 })
            };

            Span<byte> data = new byte[100];
            int count = FlatBufferSerializer.Default.Serialize(union, data);

            Assert.True(expectedData.AsSpan().SequenceEqual(data.Slice(0, count)));
        }

        [Fact]
        public void Union_2Items_Table_SerializationTest()
        {
            byte[] expectedData =
            {
                4, 0, 0, 0,         // offset to table
                246, 255, 255, 255, // soffset to vtable
                16, 0, 0, 0,        // uoffset_t to table data
                1, 0,               // discriminator (1 byte), padding
                8, 0,               // vtable length
                9, 0,               // table length
                8, 0,               // discriminator offset
                4, 0,               // value offset
                0, 0,               // padding

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

            Assert.True(expectedData.AsSpan().SequenceEqual(data.Slice(0, count)));
        }

        [Fact]
        public void Union_2Items_StringAndTable_RoundTrip()
        {
            var expectedString = "foobar";
            var expectedInt = 123;
            var expectedTable = new SimpleTable { Int = expectedInt };

            Assert.Equal(expectedString, CreateAndDeserialize1(expectedString, expectedTable));
            Assert.Equal(expectedInt, CreateAndDeserialize2(expectedString, expectedTable).Int);
        }

        [Fact]
        public void Union_2Items_MultipleStructs_RoundTrip()
        {
            var simpleStruct1 = new SimpleStruct { Long = 3 };
            var simpleStruct2 = new SimpleStruct2 { Long = 4 };

            Assert.Equal(simpleStruct1.Long, CreateAndDeserialize1(simpleStruct1, simpleStruct2).Long);
            Assert.Equal(simpleStruct2.Long, CreateAndDeserialize2(simpleStruct1, simpleStruct2).Long);
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
            Assert.Equal(1, parseResult.Item.Value.Discriminator);

            return parseResult.Item.Value.Item1;
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
            Assert.Equal(2, parseResult.Item.Value.Discriminator);

            return parseResult.Item.Value.Item2;
        }

        [FlatBufferTable]
        public class UnionTable<T1, T2>
        {
            [FlatBufferItem(0)]
            public virtual FlatBufferUnion<T1, T2>? Item { get; set; }
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

        [FlatBufferStruct]
        public class SimpleStruct2
        {
            [FlatBufferItem(0)]
            public virtual long Long { get; set; }
        }
    }
}
