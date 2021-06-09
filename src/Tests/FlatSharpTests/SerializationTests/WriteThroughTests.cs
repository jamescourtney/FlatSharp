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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for primitive types.
    /// </summary>
    [TestClass]
    public class WriteThroughTests
    {
        [TestMethod]
        public void WriteThrough_InvalidDeserializationOption()
        {
            foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                if (option == FlatBufferDeserializationOption.VectorCacheMutable || option == FlatBufferDeserializationOption.Lazy)
                {
                    continue;
                }

                var serializer = new FlatBufferSerializer(option);
                var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => serializer.Compile<Table<WriteThroughStruct<bool>>>());

                Assert.AreEqual(
                    ex.Message,
                    "Property 'Value' of Struct 'FlatSharpTests.WriteThroughTests.WriteThroughStruct<System.Boolean>' specifies the WriteThrough option. However, WriteThrough is only supported when using deserialization option 'VectorCacheMutable' or 'Lazy'.");
            }
        }

        [TestMethod]
        public void WriteThrough_SimpleInt()
        {
            static void Test(FlatBufferDeserializationOption option)
            {
                var table = new Table<WriteThroughStruct<int>>
                {
                    Struct = new WriteThroughStruct<int>
                    {
                        Value = 5
                    }
                };

                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                byte[] buffer = new byte[1024];
                serializer.Serialize(table, buffer);

                // parse
                var parsed1 = serializer.Parse<Table<WriteThroughStruct<int>>>(buffer);

                // mutate
                parsed1.Struct.Value = 300;
                Assert.AreEqual(300, parsed1.Struct.Value);

                // verify
                var parsed2 = serializer.Parse<Table<WriteThroughStruct<int>>>(buffer);
                Assert.AreEqual(300, parsed2.Struct.Value);
            }

            Test(FlatBufferDeserializationOption.VectorCacheMutable);
            Test(FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void WriteThrough_NestedStruct()
        {
            static void Test(FlatBufferDeserializationOption option)
            {
                var table = new Table<WriteThroughStruct>
                {
                    Struct = new WriteThroughStruct
                    {
                        Value = new OtherStruct { Prop1 = 10, Prop2 = 10 }
                    }
                };

                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                byte[] buffer = new byte[1024];
                serializer.Serialize(table, buffer);

                // parse
                var parsed1 = serializer.Parse<Table<WriteThroughStruct>>(buffer);

                // mutate
                Assert.AreEqual(10, parsed1.Struct.Value.Prop1);
                Assert.AreEqual(10, parsed1.Struct.Value.Prop2);
                parsed1.Struct.Value = new OtherStruct { Prop1 = 300, Prop2 = 300 };
                Assert.AreEqual(300, parsed1.Struct.Value.Prop1);
                Assert.AreEqual(300, parsed1.Struct.Value.Prop2);

                // verify, set to null
                var parsed2 = serializer.Parse<Table<WriteThroughStruct>>(buffer);
                Assert.AreEqual(300, parsed2.Struct.Value.Prop1);
                Assert.AreEqual(300, parsed2.Struct.Value.Prop2);
                parsed2.Struct.Value = null!;

                if (option == FlatBufferDeserializationOption.VectorCacheMutable)
                {
                    // we are null temporarily until we re-parse.
                    Assert.AreSame(null, parsed2.Struct.Value);
                }
                else if (option == FlatBufferDeserializationOption.Lazy)
                {
                    // lazy write through clears it out.
                    Assert.AreEqual(0, parsed2.Struct.Value.Prop1);
                    Assert.AreEqual(0, parsed2.Struct.Value.Prop2);
                }
                else
                {
                    Assert.Fail();
                }

                // verify, set to null
                var parsed3 = serializer.Parse<Table<WriteThroughStruct>>(buffer);
                Assert.AreEqual(0, parsed3.Struct.Value.Prop1);
                Assert.AreEqual(0, parsed3.Struct.Value.Prop2);
            }

            Test(FlatBufferDeserializationOption.VectorCacheMutable);
            Test(FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void WriteThrough_Vector_List()
        {
            static void Test(FlatBufferDeserializationOption option)
            {
                var table = new Table<IList<WriteThroughStruct<long>>>
                {
                    Struct = new List<WriteThroughStruct<long>>
                    {
                        new WriteThroughStruct<long> { Value = 5 }
                    }
                };

                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                byte[] buffer = new byte[1024];
                serializer.Serialize(table, buffer);

                // parse
                var parsed1 = serializer.Parse<Table<IList<WriteThroughStruct<long>>>>(buffer);

                // mutate
                parsed1.Struct[0].Value = 300;
                Assert.AreEqual(300, parsed1.Struct[0].Value);

                // verify
                var parsed2 = serializer.Parse<Table<IList<WriteThroughStruct<long>>>>(buffer);
                Assert.AreEqual(300, parsed2.Struct[0].Value);
            }

            Test(FlatBufferDeserializationOption.VectorCacheMutable);
            Test(FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void WriteThrough_Vector_Array()
        {
            static void Test(FlatBufferDeserializationOption option)
            {
                var table = new Table<WriteThroughStruct<short>[]>
                {
                    Struct = new WriteThroughStruct<short>[]
                    {
                        new WriteThroughStruct<short> { Value = 5 }
                    }
                };

                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                byte[] buffer = new byte[1024];
                serializer.Serialize(table, buffer);

                // parse
                var parsed1 = serializer.Parse<Table<WriteThroughStruct<short>[]>>(buffer);

                // mutate
                parsed1.Struct[0].Value = 300;
                Assert.AreEqual(300, parsed1.Struct[0].Value);

                // verify
                var parsed2 = serializer.Parse<Table<WriteThroughStruct<short>[]>>(buffer);
                Assert.AreEqual(300, parsed2.Struct[0].Value);
            }

            Test(FlatBufferDeserializationOption.VectorCacheMutable);
            Test(FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void WriteThrough_Lazy_ThrowsForOtherProperties()
        {
            var table = new Table<OtherStruct>
            {
                Struct = new OtherStruct { Prop1 = 1, Prop2 = 2 }
            };

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            byte[] buffer = new byte[1024];
            serializer.Serialize(table, buffer);
            string csharp = serializer.Compile<Table<OtherStruct>>().CSharp;

            var parsed = serializer.Parse<Table<OtherStruct>>(buffer);

            Assert.ThrowsException<NotMutableException>(() => parsed.Struct.Prop1 = 2);
            Assert.ThrowsException<NotMutableException>(() => parsed.Struct.Prop2 = 4);
        }

        [FlatBufferTable]
        public class Table<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Struct { get; set; }
        }

        [FlatBufferStruct]
        public class WriteThroughStruct<T>
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        public class WriteThroughStruct
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual OtherStruct Value { get; set; }
        }

        [FlatBufferStruct]
        public class OtherStruct
        {
            [FlatBufferItem(0)]
            public virtual int Prop1 { get; set; }

            [FlatBufferItem(1)]
            public virtual int Prop2 { get; set; }
        }
    }
}
