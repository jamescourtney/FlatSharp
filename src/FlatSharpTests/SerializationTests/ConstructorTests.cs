/*
 * Copyright 2021 James Courtney
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
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        public void ConstructorSerializationTests()
        {
            OuterTable outer = new OuterTable
            {
                Struct = new OuterStruct(),
            };

            byte[] data = new byte[1024];
            FlatBufferSerializer.Default.Serialize(outer, data);
            var parsed = FlatBufferSerializer.Default.Parse<OuterTable>(data);

            Assert.IsNotNull(parsed.Context);
            Assert.IsNull(outer.Context);

            Assert.IsNull(parsed.Struct.InnerA.Context);
            Assert.IsNull(outer.Struct.InnerA.Context);

            Assert.IsNotNull(parsed.Struct.InnerB.Context);
            Assert.IsNull(outer.Struct.InnerB.Context);
        }

        [TestMethod]
        public void Struct_Serialize_InnerNull()
        {
            OuterTable outer = new OuterTable
            {
                Struct = new OuterStruct(),
            };

            outer.Struct.InnerB = null;

            byte[] data = new byte[1024];
            data.AsSpan().Fill(123);

            int length = FlatBufferSerializer.Default.Serialize(outer, data);
            var parsed = FlatBufferSerializer.Default.Parse<OuterTable>(data);

            // Null overwrites buffer to 0 on serialize.
            Assert.AreEqual(0, parsed.Struct.InnerB.Item);

            Span<byte> expected = new byte[]
            {
                4, 0, 0, 0,
                220, 255, 255, 255,
                1, 0, 0, 0,
                123, 123, 123, 123,   // padding bytes are not overwritten.
                1, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0,           // null InnerB struct is overwritten with 0.
                0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0,
                6, 0, 36, 0, 4, 0,
            };

            Assert.IsTrue(expected.SequenceEqual(data.AsSpan().Slice(0, length)));
        }

        [TestMethod]
        public void Struct_Serialize_InnerNull_2()
        {
            OuterTable outer = new OuterTable
            {
                Struct = new OuterStruct(),
            };

            outer.Struct.InnerA = null;

            byte[] data = new byte[1024];
            data.AsSpan().Fill(123);

            FlatBufferSerializer.Default.Serialize(outer, data);
            var parsed = FlatBufferSerializer.Default.Parse<OuterTable>(data);

            // Null overwrites buffer to 0 on serialize.
            Assert.AreEqual(0, parsed.Struct.InnerA.Item);
        }

        [FlatBufferTable]
        public class OuterTable
        {
            public OuterTable()
            {
            }

            protected OuterTable(FlatSharpDeserializationContext context)
            {
                this.Context = context;
            }

            public FlatSharpDeserializationContext Context { get; }

            [FlatBufferItem(0)]
            public virtual OuterStruct? Struct { get; set; }
        }

        [FlatBufferStruct]
        public class OuterStruct
        {
            public OuterStruct()
            {
                this.InnerA = new InnerStructA(1);
                this.InnerB = new InnerStructB(2);
            }

            [FlatBufferItem(0)]
            public virtual InnerStructA InnerA { get; set; }

            [FlatBufferItem(1)]
            public virtual InnerStructB InnerB { get; set; }
        }

        [FlatBufferStruct]
        public class InnerStructA
        {
            public InnerStructA(int value)
            {
                this.Item = value;
                this.Item2 = value;
            }

            protected InnerStructA()
            {
            }

            public FlatSharpDeserializationContext Context { get; }

            [FlatBufferItem(0)]
            public virtual int Item { get; set; }

            [FlatBufferItem(1)]
            public virtual long Item2 { get; set; }
        }

        [FlatBufferStruct]
        public class InnerStructB
        {
            public InnerStructB(int value)
            {
                this.Item = value;
                this.Item2 = value;
            }

            protected InnerStructB(FlatSharpDeserializationContext context)
            {
                this.Context = context;
            }

            public FlatSharpDeserializationContext Context { get; }

            [FlatBufferItem(0)]
            public virtual int Item { get; set; }

            [FlatBufferItem(1)]
            public virtual long Item2 { get; set; }
        }
    }
}
