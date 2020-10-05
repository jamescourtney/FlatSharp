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
    public class TableSerializationTests
    {
        [TestMethod]
        public void AllMembersNull()
        {
            SimpleTable table = new SimpleTable();

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                252, 255, 255, 255,
                4, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void RootTableNull()
        {
            Assert.ThrowsException<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize<SimpleTable>(null, new byte[1024]));
        }

        [TestMethod]
        public void TableWithStruct()
        {
            SimpleTable table = new SimpleTable
            {
                Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,             // uoffset to table start
                236, 255, 255, 255,     // soffet to vtable (4 - x = 24 => x = -20)
                2, 0, 0, 0, 0, 0, 0, 0, // struct.long
                1,                      // struct.byte
                0, 0, 0,                // padding
                3, 0, 0, 0,             // struct.uint
                8, 0,                   // vtable length
                20, 0,                  // table length
                0, 0,                   // index 0 offset
                4, 0,                   // Index 1 offset
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void TableWithStructAndString()
        {
            SimpleTable table = new SimpleTable
            {
                String = "hi",
                Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,             // uoffset to table start
                228, 255, 255, 255,     // soffet to vtable (4 - x = 24 => x = -20)
                32, 0, 0, 0,            // uoffset to string
                0, 0, 0, 0,             // padding
                2, 0, 0, 0, 0, 0, 0, 0, // struct.long
                1,                      // struct.byte
                0, 0, 0,                // padding
                3, 0, 0, 0,             // struct.uint
                8, 0,                   // vtable length
                28, 0,                  // table length
                4, 0,                   // index 0 offset
                12, 0,                  // Index 1 offset
                2, 0, 0, 0,             // string length
                104, 105, 0,            // hi + null terminator
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void TableParse_NotMutable()
        {
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy);
            var table = this.SerializeAndParse(options, out _);

            Assert.ThrowsException<NotMutableException>(() => table.String = null);
            Assert.ThrowsException<NotMutableException>(() => table.Struct = null);
            Assert.ThrowsException<NotMutableException>(() => table.StructVector = new List<SimpleStruct>());
            Assert.ThrowsException<NotMutableException>(() => table.StructVector.Add(null));
        }

        [TestMethod]
        public void TableParse_LazyMutable()
        {
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.VectorCacheMutable);
            var table = this.SerializeAndParse(options, out _);
            
            var newString = Guid.NewGuid().ToString();
            table.String = newString;
            Assert.AreEqual(newString, table.String);

            var newLong = DateTimeOffset.UtcNow.Ticks;
            table.Struct.Long = newLong;
            Assert.AreEqual(newLong, table.Struct.Long);

            var newStruct = new SimpleStruct();
            table.Struct = newStruct;
            Assert.AreEqual(newStruct, table.Struct);

            Assert.AreEqual(typeof(List<SimpleStruct>), table.StructVector.GetType());
            int count = table.StructVector.Count;
            table.StructVector.Add(new SimpleStruct());
            Assert.AreEqual(count + 1, table.StructVector.Count);
        }

        [TestMethod]
        public void TableParse_GreedyImmutable()
        {
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy);
            var table = this.SerializeAndParse(options, out var buffer);

            bool reaped = false;
            for (int i = 0; i < 5; ++i)
            {
                GC.Collect(2, GCCollectionMode.Forced);
                GCSettings.LatencyMode = GCLatencyMode.Batch;
                if (!buffer.TryGetTarget(out _))
                {
                    reaped = true;
                    break;
                }
            }

            Assert.IsTrue(reaped, "GC did not reclaim underlying byte buffer.");

            // The buffer has been collected. Now verify that we can read all the data as
            // we expect. This demonstrates that we've copied, as well as that we've 
            // released references.
            Assert.IsNotNull(table.String);
            Assert.IsNotNull(table.Struct);
            Assert.IsNotNull(table.StructVector);

            Assert.AreEqual("hi", table.String);
            Assert.AreEqual(1, table.Struct.Byte);
            Assert.AreEqual(2, table.Struct.Long);
            Assert.AreEqual(3u, table.Struct.Uint);

            Assert.AreEqual(4, table.StructVector[0].Byte);
            Assert.AreEqual(5, table.StructVector[0].Long);
            Assert.AreEqual(6u, table.StructVector[0].Uint);
        }

        private SimpleTable SerializeAndParse(FlatBufferSerializerOptions options, out WeakReference<byte[]> buffer)
        {
            SimpleTable table = new SimpleTable
            {
                String = "hi",
                Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 },
                StructVector = new List<SimpleStruct> { new SimpleStruct { Byte = 4, Long = 5, Uint = 6 } },
            };

            var serializer = new FlatBufferSerializer(options);

            var rawBuffer = new byte[1024];
            serializer.Serialize(table, rawBuffer);
            buffer = new WeakReference<byte[]>(rawBuffer);

            string csharp = serializer.Compile<SimpleTable>().CSharp;
            return serializer.Parse<SimpleTable>(rawBuffer);
        }

        [FlatBufferTable]
        public class SimpleTable
        {
            [FlatBufferItem(0)]
            public virtual string String { get; set; }

            [FlatBufferItem(1)]
            public virtual SimpleStruct Struct { get; set; }

            [FlatBufferItem(2)]
            public virtual IList<SimpleStruct> StructVector { get; set; }

            [FlatBufferItem(4)]
            public virtual SimpleTable InnerTable { get; set; }

            [FlatBufferItem(5, DefaultValue = 0L)]
            public virtual long NoSetter { get; }
        }

        [FlatBufferStruct]
        public class SimpleStruct
        {
            [FlatBufferItem(0)]
            public virtual long Long { get; set; }

            [FlatBufferItem(1)]
            public virtual byte Byte { get; set; }

            [FlatBufferItem(2)]
            public virtual uint Uint { get; set; }
        }
    }
}
