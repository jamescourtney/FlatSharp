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
        public void UnionTest()
        {
            UnionTable<SimpleStruct, SimpleTable> table = new UnionTable<SimpleStruct, SimpleTable>
            {
                Item = new FlatBufferUnion<SimpleStruct, SimpleTable>(new SimpleStruct
                {
                    Long = 123,
                })
            };

            byte[] buffer = new byte[100];
            FlatBufferSerializer.Default.Serialize(table, buffer.AsSpan());

            var result = FlatBufferSerializer.Default.Parse<UnionTable<SimpleStruct, SimpleTable>>(buffer);

            Assert.AreEqual(result.Item.Discriminator, 1);
            Assert.AreEqual(result.Item.Item1.Long, 123);

            table.Item = new FlatBufferUnion<SimpleStruct, SimpleTable>(new SimpleTable
            {
                String = "foobar"
            });

            buffer = new byte[100];
            FlatBufferSerializer.Default.Serialize(table, buffer.AsSpan());

            result = FlatBufferSerializer.Default.Parse<UnionTable<SimpleStruct, SimpleTable>>(buffer);

            Assert.AreEqual(result.Item.Discriminator, 2);
            Assert.AreEqual(result.Item.Item2.String, "foobar");
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
