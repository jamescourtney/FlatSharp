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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
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
        [TestMethod]
        public void TestPooling()
        {
            TestTable table = new TestTable
            {
                String = "foo",
                Union = new FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>(new NonRecyclableStruct()),
                VectorOfRecyclableStruct = new List<TestStruct>
                {
                    new(), new(), new(), new(), new(),
                }
            };

            var fbSerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.VectorCache);
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
            }

            // pool size is capped at 3 but we need 5, so we cons 2 items per iteration.
            // First iteration makes 5 items
            // 999 next iterations make 2 each, for total of 2003.
            Assert.AreEqual(2003, TestStruct.CtorCount);

            // no pooling; one per iteration.
            Assert.AreEqual(1000, NonRecyclableStruct.CtorCount); 

            // no limit.
            Assert.AreEqual(1, TestTable.CtorCount);
        }

        private static void ResetCounts()
        {
            TestTable.CtorCount = 0;
            TestStruct.CtorCount = 0;
            NonRecyclableTable.CtorCount = 0;
            NonRecyclableStruct.CtorCount = 0;
        }

        [FlatBufferTable(PoolSize = -1)]
        public class TestTable
        {
            public static int CtorCount = 0;

            public TestTable() => CtorCount++;

            [FlatBufferItem(0)]
            public virtual string String { get; set; }

            [FlatBufferItem(1)]
            public virtual IList<TestStruct> VectorOfRecyclableStruct { get; set; }

            [FlatBufferItem(2)]
            public virtual IList<TestTable> VectorOfRecyclableTable { get; set; }

            [FlatBufferItem(3)]
            public virtual IList<NonRecyclableStruct> VectorOfNonRecyclableStruct { get; set; }

            [FlatBufferItem(4)]
            public virtual IList<NonRecyclableTable> VectorOfNonRecyclableTable { get; set; }

            [FlatBufferItem(5)]
            public virtual FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable> Union { get; set; }

            [FlatBufferItem(7)]
            public virtual IList<FlatBufferUnion<TestStruct, TestTable, NonRecyclableStruct, NonRecyclableTable>> VectorOfUnion { get; set; }
        }

        [FlatBufferTable]
        public class NonRecyclableTable
        {
            public static int CtorCount = 0;

            public NonRecyclableTable() => CtorCount++;

            [FlatBufferItem(0)] public virtual int Int { get; set; }
        }

        [FlatBufferStruct(PoolSize = 3)]
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
    }
}
