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
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests serialization of nested structs.
    /// </summary>
    [TestClass]
    public class NestedStructTests
    {
        [TestMethod]
        public void NestedStructs_Greedy() => this.RunTest(FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void NestedStructs_GreedyMutable() => this.RunTest(FlatBufferDeserializationOption.GreedyMutable);

        [TestMethod]
        public void NestedStructs_VectorCacheMutable() => this.RunTest(FlatBufferDeserializationOption.VectorCacheMutable);

        [TestMethod]
        public void NestedStructs_VectorCache() => this.RunTest(FlatBufferDeserializationOption.VectorCache);

        [TestMethod]
        public void NestedStructs_PropertyCache() => this.RunTest(FlatBufferDeserializationOption.PropertyCache);

        [TestMethod]
        public void NestedStructs_Lazy() => this.RunTest(FlatBufferDeserializationOption.Lazy);

        private void RunTest(FlatBufferDeserializationOption option)
        {
            Table table = new Table
            {
                Outer = new OuterStruct
                {
                    InnerVirtual = new InnerStruct { A = 3 },
                    NonVirtualInner = new InnerStruct { A = 30 }
                }
            };

            OuterStruct s = new OuterStruct();

            FlatBufferSerializer serializer = new FlatBufferSerializer(option);
            byte[] data = new byte[1024];
            serializer.Serialize(table, data);

            var parsed = serializer.Parse<Table>(data);
            Assert.AreEqual(3, parsed.Outer.InnerVirtual.A);
            Assert.AreEqual(30, parsed.Outer.NonVirtualInner.A);
        }

        [FlatBufferTable]
        public class Table
        {
            [FlatBufferItem(0)]
            public virtual OuterStruct? Outer { get; set; }
        }

        [FlatBufferStruct]
        public class OuterStruct
        {
            [FlatBufferItem(0)]
            public virtual InnerStruct InnerVirtual { get; set; }

            [FlatBufferItem(1)]
            public InnerStruct NonVirtualInner { get; set; }
        }
    }
}
