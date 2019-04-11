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
        public void Union_2Items_TableAndStruct()
        {
            var expectedStruct = new SimpleStruct { Long = 123 };
            var expectedTable = new SimpleTable { String = "foobar" };

            var testStruct = CreateAndDeserialize1(expectedStruct, expectedTable);
            Assert.AreEqual(testStruct.Long, 123);

            var testTable = CreateAndDeserialize2(expectedStruct, expectedTable);
            Assert.AreEqual(testTable.String, "foobar");

        }

        [TestMethod]
        public void Union_2Items_StringAndTable()
        {
            var expectedString = "foobar";
            var expectedTable = new SimpleTable { String = expectedString };

            Assert.AreEqual(expectedString, CreateAndDeserialize1(expectedString, expectedTable));
            Assert.AreEqual(expectedString, CreateAndDeserialize2(expectedString, expectedTable).String);
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
            public virtual string String { get; set; }
        }

        [FlatBufferStruct]
        public class SimpleStruct
        {
            [FlatBufferItem(0)]
            public virtual long Long { get; set; }
        }
    }
}
