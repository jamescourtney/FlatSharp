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
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests object recycling.
    /// </summary>
    [TestClass]
    public class RecycleTests
    {
        private const int TestStructPoolSize = 3;
        private const int TestTablePoolSize = -1;

#if NET5_0_OR_GREATER
        [TestMethod]
        public void Recycle_Accessor_Combinations()
        {
            var serializer = FlatBufferSerializer.Default.Compile<AccessorCombinationTable_Valid>();
        }

        [TestMethod]
        public void Recycle_Accessor_NonVirtual()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => FlatBufferSerializer.Default.Compile<AccessorCombinationTable_NonVirtualInit>());

            Assert.AreEqual(
                ex.Message,
                "FlatBuffer property 'FlatSharpTests.RecycleTests.AccessorCombinationTable_NonVirtualInit.Init' is non-virtual in a table with object recycling enabled. This combination is not supported. Consider marking the property as virtual or disabling recycling.");
        }
#endif

        [TestMethod]
        public void Recycle_Allocation_Count_Verification()
        {
            static void SerializeAndParse(FlatBufferDeserializationOption option)
            {
                ResetCounts();

                TestTable table = new TestTable
                {
                    String = "foo",
                    Union = new FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>(new NonRecyclableStruct()),
                    VectorOfRecyclableStruct = new List<TestStruct>
                    {
                        new(), new(), new(), new(), new(),
                    }
                };

                var fbSerializer = new FlatBufferSerializer(option);
                ISerializer<TestTable> serializer = fbSerializer.Compile<TestTable>();

                Assert.AreEqual(5, TestStruct.CtorCount);
                Assert.AreEqual(1, NonRecyclableStruct.CtorCount);
                Assert.AreEqual(1, TestTable.CtorCount);

                ResetCounts();

                byte[] data = new byte[1024];
                serializer.Write(data, table);

                for (int i = 0; i < 1000; ++i)
                {
                    var parsed = serializer.Parse(data);
                    NonRecyclableStruct @struct = parsed.Union.Item3;
                    int sum = @struct.Int;
                    foreach (var s in parsed.VectorOfRecyclableStruct)
                    {
                        sum += s.Int;
                    }

                    serializer.Recycle(ref parsed);
                    Assert.IsNull(parsed);
                }
            }

            SerializeAndParse(FlatBufferDeserializationOption.Greedy);

            // pool size is capped at 3 but we need 5, so we cons 2 items per iteration.
            // First iteration makes 5 items
            // 999 next iterations make 2 each, for total of 2003.
            Assert.AreEqual(2003, TestStruct.CtorCount);
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount); // 1 per iteration. No recycling.
            Assert.AreEqual(1, TestTable.CtorCount);              // Pooled -- root just has one.

            SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable);
            Assert.AreEqual(2003, TestStruct.CtorCount);
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount);
            Assert.AreEqual(1, TestTable.CtorCount);

            SerializeAndParse(FlatBufferDeserializationOption.VectorCache);
            Assert.AreEqual(2003, TestStruct.CtorCount);
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount);
            Assert.AreEqual(1, TestTable.CtorCount);

            SerializeAndParse(FlatBufferDeserializationOption.VectorCacheMutable);
            Assert.AreEqual(2003, TestStruct.CtorCount);
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount);
            Assert.AreEqual(1, TestTable.CtorCount);

            SerializeAndParse(FlatBufferDeserializationOption.PropertyCache);
            Assert.AreEqual(5000, TestStruct.CtorCount);             // Vector objects leak with property cache.
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount);
            Assert.AreEqual(1, TestTable.CtorCount);

            SerializeAndParse(FlatBufferDeserializationOption.Lazy);
            Assert.AreEqual(5000, TestStruct.CtorCount);             // Everything leaks with lazy
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount);
            Assert.AreEqual(1, TestTable.CtorCount);                 // Root is successfully recycled.
        }

        [TestMethod]
        public void Recycle_Circular_Chain_With_Error_Conditions()
        {
            var a = new Chain_A
            {
                Next = new Chain_B
                {
                    Next = new Chain_C
                    {
                        Next = new Chain_A
                        {
                            Next = new Chain_B
                            {
                                Next = new Chain_C()
                            }
                        }
                    }
                }
            };

            FlatBufferSerializer pcSerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.PropertyCache);
            var chainASerializer = pcSerializer.Compile<Chain_A>();

            byte[] buffer = new byte[1024];
            chainASerializer.Write(buffer, a);

            Assert.AreEqual(2, Chain_A.CtorCount);
            Assert.AreEqual(2, Chain_B.CtorCount);
            Assert.AreEqual(2, Chain_C.CtorCount);

            ResetCounts();

            InvalidOperationException ex;
            for (int i = 0; i < 100; ++i)
            {
                Chain_A a1 = chainASerializer.Parse<Chain_A>(buffer);
                Chain_A a1Copy = a1;

                Chain_B b1 = a1.Next;
                Chain_C c1 = b1.Next;
                Chain_A a2 = c1.Next;
                Chain_B b2 = a2.Next;
                Chain_C c2 = b2.Next;

                Assert.IsNotNull(c2);
                Assert.IsNull(c2.Next);

                chainASerializer.Recycle(ref a1);
                Assert.IsNull(a1);

                const string Substring = "FlatSharp object used after recycle";
                ex = Assert.ThrowsException<InvalidOperationException>(() => a1Copy.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));

                ex = Assert.ThrowsException<InvalidOperationException>(() => b1.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));

                ex = Assert.ThrowsException<InvalidOperationException>(() => c1.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));

                ex = Assert.ThrowsException<InvalidOperationException>(() => a2.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));

                ex = Assert.ThrowsException<InvalidOperationException>(() => b2.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));

                ex = Assert.ThrowsException<InvalidOperationException>(() => c2.Next);
                Assert.IsTrue(ex.Message.Contains(Substring));
            }

            Chain_A doubleRecycle1 = chainASerializer.Parse<Chain_A>(buffer);
            Chain_A doubleRecycle2 = doubleRecycle1;

            chainASerializer.Recycle(ref doubleRecycle1);
            ex = Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    var temp = doubleRecycle2;
                    chainASerializer.Recycle(ref temp);
                });

            Assert.IsTrue(ex.Message.Contains("FlatSharp object recycled twice."));

            Assert.AreEqual(2, Chain_A.CtorCount);
            Assert.AreEqual(2, Chain_B.CtorCount);
            Assert.AreEqual(2, Chain_C.CtorCount);
        }

        [TestMethod]
        public void PropertyCache_ListVector_IsNotRecycled()
        {
            var table = new GenericTable<IList<TestStruct>>
            {
                Item = new List<TestStruct> { new(), new(), new() }
            };

            RunVectorTest100Parses<IList<TestStruct>, TestStruct>(FlatBufferDeserializationOption.PropertyCache, table);
            Assert.AreEqual(100 * 3, TestStruct.CtorCount); // no pooling.
        }

        [TestMethod]
        public void VectorCache_UnionListVector_IsRecycled()
        {
            var table = new GenericTable<IList<FlatBufferUnion<TestStruct, string>>>
            {
                Item = new List<FlatBufferUnion<TestStruct, string>>
                {
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(string.Empty),
                }
            };

            RunVectorTest100Parses<IList<FlatBufferUnion<TestStruct, string>>, FlatBufferUnion<TestStruct, string>>(FlatBufferDeserializationOption.VectorCache, table);
            Assert.AreEqual(3, TestStruct.CtorCount);
        }

        [TestMethod]
        public void PropertyCache_UnionListVector_IsNotRecycled()
        {
            var table = new GenericTable<IList<FlatBufferUnion<TestStruct, string>>>
            {
                Item = new List<FlatBufferUnion<TestStruct, string>>
                {
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(string.Empty),
                }
            };

            RunVectorTest100Parses<IList<FlatBufferUnion<TestStruct, string>>, FlatBufferUnion<TestStruct, string>>(FlatBufferDeserializationOption.PropertyCache, table);
            Assert.AreEqual(100 * 3, TestStruct.CtorCount); // no pooling.
        }

        [TestMethod]
        public void PropertyCache_IndexedVector_IsNotRecycled()
        {
            var table = new GenericTable<IIndexedVector<string, TableWithKey>>
            {
                Item = new IndexedVector<string, TableWithKey>
                {
                    new TableWithKey { Key = "a" },
                    new TableWithKey { Key = "b" },
                    new TableWithKey { Key = "c" },
                }
            };

            RunVectorTest100Parses<IIndexedVector<string, TableWithKey>, KeyValuePair<string, TableWithKey>>(
                FlatBufferDeserializationOption.PropertyCache,
                table);

            Assert.AreEqual(100 * 3, TableWithKey.CtorCount); // no pooling.
        }

        [TestMethod]
        public void Lazy_IndexedVector_IsNotRecycled()
        {
            var table = new GenericTable<IIndexedVector<string, TableWithKey>>
            {
                Item = new IndexedVector<string, TableWithKey>
                {
                    new TableWithKey { Key = "a" },
                    new TableWithKey { Key = "b" },
                    new TableWithKey { Key = "c" },
                }
            };

            RunVectorTest100Parses<IIndexedVector<string, TableWithKey>, KeyValuePair<string, TableWithKey>>(
                FlatBufferDeserializationOption.Lazy,
                table);

            Assert.AreEqual(100 * 3, TableWithKey.CtorCount); // no pooling.
        }

        [TestMethod]
        public void VectorCache_IndexedVector_IsRecycled()
        {
            var table = new GenericTable<IIndexedVector<string, TableWithKey>>
            {
                Item = new IndexedVector<string, TableWithKey>
                {
                    new TableWithKey { Key = "a" },
                    new TableWithKey { Key = "b" },
                    new TableWithKey { Key = "c" },
                }
            };

            RunVectorTest100Parses<IIndexedVector<string, TableWithKey>, KeyValuePair<string, TableWithKey>>(
                FlatBufferDeserializationOption.VectorCache,
                table);

            Assert.AreEqual(3, TableWithKey.CtorCount);
        }

        [TestMethod]
        public void Greedy_IndexedVector_IsRecycled()
        {
            var table = new GenericTable<IIndexedVector<string, TableWithKey>>
            {
                Item = new IndexedVector<string, TableWithKey>
                {
                    new TableWithKey { Key = "a" },
                    new TableWithKey { Key = "b" },
                    new TableWithKey { Key = "c" },
                }
            };

            RunVectorTest100Parses<IIndexedVector<string, TableWithKey>, KeyValuePair<string, TableWithKey>>(
                FlatBufferDeserializationOption.Greedy,
                table);

            Assert.AreEqual(3, TableWithKey.CtorCount);
        }

        [TestMethod]
        public void PropertyCache_UnionArrayVector_IsRecycled()
        {
            var table = new GenericTable<FlatBufferUnion<TestStruct, string>[]>
            {
                Item = new FlatBufferUnion<TestStruct, string>[]
                {
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(new TestStruct()),
                    new FlatBufferUnion<TestStruct, string>(string.Empty),
                }
            };

            RunVectorTest100Parses<FlatBufferUnion<TestStruct, string>[], FlatBufferUnion<TestStruct, string>>(FlatBufferDeserializationOption.PropertyCache, table);
            Assert.AreEqual(3, TestStruct.CtorCount);
        }

        [TestMethod]
        public void PropertyCache_ArrayVector_IsRecycled()
        {
            var table = new GenericTable<TestStruct[]>
            {
                Item = new TestStruct[] { new(), new(), new() }
            };

            RunVectorTest100Parses<TestStruct[], TestStruct>(FlatBufferDeserializationOption.PropertyCache, table);
            Assert.AreEqual(3, TestStruct.CtorCount); // pooling working.
        }

        [TestMethod]
        public void Lazy_ArrayVector_IsNotRecycled()
        {
            var table = new GenericTable<TestStruct[]>
            {
                Item = new TestStruct[] { new(), new(), new() }
            };

            RunVectorTest100Parses<TestStruct[], TestStruct>(FlatBufferDeserializationOption.Lazy, table);
            Assert.AreEqual(100 * 3, TestStruct.CtorCount);
        }

        private void RunVectorTest100Parses<TVector, TItem>(
            FlatBufferDeserializationOption option,
            GenericTable<TVector> table)
            where TVector : IEnumerable<TItem>
        {
            FlatBufferSerializer serializer = new FlatBufferSerializer(option);
            var tableSerializer = serializer.Compile<GenericTable<TVector>>();

            byte[] data = new byte[tableSerializer.GetMaxSize(table)];
            tableSerializer.Write(data, table);

            ResetCounts();
            for (int i = 0; i < 100; ++i)
            {
                var parsed = tableSerializer.Parse(data);
                foreach (var item in parsed.Item)
                {
                }

                tableSerializer.Recycle(ref parsed);
                Assert.IsNull(parsed);
            }
        }

        [TestMethod]
        public void Lazy_ListVector_DangerousRecycle_OK()
        {
            var table = new GenericTable<IList<TestStruct>>
            {
                Item = new List<TestStruct> { new(), new(), new() }
            };

            ResetCounts();

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy).Compile<GenericTable<IList<TestStruct>>>();
            byte[] data = new byte[1024];
            serializer.Write(data, table);

            for (int i = 0; i < 100; ++i)
            {
                var parsed = serializer.Parse(data);
                var vector = parsed.Item;
                int count = vector.Count;
                for (int j = 0; j < count; ++j)
                {
                    var @struct = vector[j];
                    ((IFlatBufferDeserializedObject)@struct).DangerousRecycle();
                }
            }

            Assert.AreEqual(1, TestStruct.CtorCount);
        }

        [TestMethod]
        public void VectorCache_ListVector_DangerousRecycle_ImproperUsage()
        {
            try
            {
                FlatSharpRuntimeSettings.EnableRecyclingDiagnostics = true;
                var table = new GenericTable<IList<TestStruct>>
                {
                    Item = new List<TestStruct> { new(), new(), new() }
                };

                ResetCounts();

                var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.VectorCache).Compile<GenericTable<IList<TestStruct>>>();
                byte[] data = new byte[1024];
                serializer.Write(data, table);

                var parsed = serializer.Parse(data);
                var vector = parsed.Item;
                int count = vector.Count;
                for (int j = 0; j < count; ++j)
                {
                    var @struct = vector[j];
                    ((IFlatBufferDeserializedObject)@struct).DangerousRecycle();
                }

                var ex = Assert.ThrowsException<InvalidOperationException>(
                    () => serializer.Recycle(ref parsed));

                Assert.IsTrue(ex.Message.Contains("FlatSharp object recycled twice."));
                Assert.IsTrue(ex.Message.Contains(".GetOrCreate")); // allocation stack.
                Assert.IsTrue(ex.Message.Contains(".DangerousRecycle")); // release stack
            }
            finally
            {
                FlatSharpRuntimeSettings.EnableRecyclingDiagnostics = false;
            }
        }

        private static void ResetCounts()
        {
            TestTable.CtorCount = 0;
            TestStruct.CtorCount = 0;
            NonRecyclableTable.CtorCount = 0;
            NonRecyclableStruct.CtorCount = 0;
            TableWithKey.CtorCount = 0;

            Chain_A.CtorCount = 0;
            Chain_B.CtorCount = 0;
            Chain_C.CtorCount = 0;
        }

#if NET5_0_OR_GREATER
        [FlatBufferTable(PoolSize = -1)]
        public class AccessorCombinationTable_Valid
        {
            [FlatBufferItem(0)]
            public virtual int Regular { get; set; }

            [FlatBufferItem(1)]
            public virtual int RegularVirtual { get; set; }

            [FlatBufferItem(2)]
            public virtual int InitVirtual { get; init; }

            [FlatBufferItem(3)]
            public virtual int ReadOnlyVirtual { get; }
        }

        [FlatBufferTable(PoolSize = -1)]
        public class AccessorCombinationTable_NonVirtualInit
        {
            [FlatBufferItem(0)]
            public int Init { get; init; }

            [FlatBufferItem(1)]
            public int Set { get; set; }
        }
#endif

        [FlatBufferTable(PoolSize = TestTablePoolSize)]
        public class TestTable
        {
            public static int CtorCount = 0;

            public TestTable() => CtorCount++;

            [FlatBufferItem(0)]
            public virtual string? String { get; set; }

            [FlatBufferItem(1)]
            public virtual IList<TestStruct>? VectorOfRecyclableStruct { get; set; }

            [FlatBufferItem(2)]
            public virtual IList<TestTable>? VectorOfRecyclableTable { get; set; }

            [FlatBufferItem(3)]
            public virtual IList<NonRecyclableStruct>? VectorOfNonRecyclableStruct { get; set; }

            [FlatBufferItem(4)]
            public virtual IList<NonRecyclableTable>? VectorOfNonRecyclableTable { get; set; }

            [FlatBufferItem(5)]
            public virtual FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>? Union { get; set; }

            [FlatBufferItem(7)]
            public virtual FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>? Union2 { get; set; }

            [FlatBufferItem(9)]
            public virtual IList<FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>>? VectorOfUnion { get; set; }
        }

        [FlatBufferTable]
        public class GenericTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }
        }

        [FlatBufferTable(PoolSize = -1)]
        public class TableWithKey
        {
            public static int CtorCount = 0;

            public TableWithKey() => CtorCount++;

            [FlatBufferItem(0, Key = true, Required = true)]
            public virtual string Key { get; set; }
        }

        [FlatBufferTable]
        public class NonRecyclableTable
        {
            public static int CtorCount = 0;

            public NonRecyclableTable() => CtorCount++;

            [FlatBufferItem(0)] public virtual int Int { get; set; }
        }

        [FlatBufferStruct(PoolSize = TestStructPoolSize)]
        public class TestStruct
        {
            public static int CtorCount = 0;

            public TestStruct() => CtorCount++;

            [FlatBufferItem(0)] public virtual int Int { get; set; }
        }

        [FlatBufferStruct]
        public class NonRecyclableStruct
        {
            public static int CtorCount = 0;

            public NonRecyclableStruct() => CtorCount++;

            [FlatBufferItem(0)] public virtual int Int { get; set; }
        }

        [FlatBufferTable(PoolSize = -1)]
        public class Chain_A
        {
            public static int CtorCount = 0;

            public Chain_A() => CtorCount++;

            [FlatBufferItem(0)]
            public virtual Chain_B? Next { get; set; }
        }

        [FlatBufferTable(PoolSize = -1)]
        public class Chain_B
        {
            public static int CtorCount = 0;

            public Chain_B() => CtorCount++;

            [FlatBufferItem(0)]
            public virtual Chain_C? Next { get; set; }
        }

        [FlatBufferTable(PoolSize = -1)]
        public class Chain_C
        {
            public static int CtorCount = 0;

            public Chain_C() => CtorCount++;

            [FlatBufferItem(0)]
            public virtual Chain_A? Next { get; set; }
        }
    }
}
