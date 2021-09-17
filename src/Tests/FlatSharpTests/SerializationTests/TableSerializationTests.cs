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
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    
    public class TableSerializationTests
    {
        [Fact]
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
            Assert.True(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void RootTableNull()
        {
            Assert.Throws<ArgumentNullException>(() => FlatBufferSerializer.Default.Serialize<SimpleTable>(null, new byte[1024]));
        }

        [Fact]
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
            Assert.True(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
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
                232, 255, 255, 255,     // soffet to vtable (4 - x = 24 => x = -20)
                2, 0, 0, 0, 0, 0, 0, 0, // struct.long
                1, 0, 0, 0,             // struct.byte + padding
                3, 0, 0, 0,             // struct.uint
                12, 0, 0, 0,            // uoffset to string
                8, 0,                   // vtable length
                24, 0,                  // table length
                20, 0,                  // index 0 offset
                4, 0,                   // Index 1 offset
                2, 0, 0, 0,             // string length
                104, 105, 0,            // hi + null terminator
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.True(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void EmptyTableSerialize()
        {
            EmptyTable table = new EmptyTable();

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                252, 255, 255, 255,
                4, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.True(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));

            int maxSize = FlatBufferSerializer.Default.GetMaxSize(table);
            Assert.Equal(23, maxSize);
        }

        [Fact]
        public void TableWithStructAndStringNonVirtual()
        {
            SimpleTableNonVirtual table = new SimpleTableNonVirtual
            {
                String = "hi",
                Struct = new SimpleStructNonVirtual { Byte = 1, Long = 2, Uint = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,             // uoffset to table start
                232, 255, 255, 255,     // soffet to vtable (4 - x = 24 => x = -20)
                2, 0, 0, 0, 0, 0, 0, 0, // struct.long
                1, 0, 0, 0,             // struct.byte + padding
                3, 0, 0, 0,             // struct.uint
                12, 0, 0, 0,            // uoffset to string
                8, 0,                   // vtable length
                24, 0,                  // table length
                20, 0,                  // index 0 offset
                4, 0,                   // Index 1 offset
                2, 0, 0, 0,             // string length
                104, 105, 0,            // hi + null terminator
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.True(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
            var parsed = FlatBufferSerializer.Default.Parse<SimpleTableNonVirtual>(buffer);
        }

        [Fact]
        public void TableParse_NotMutable()
        {
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy);
            var table = this.SerializeAndParse(options, out _);

            Assert.Throws<NotMutableException>(() => table.String = null);
            Assert.Throws<NotMutableException>(() => table.Struct = null);
            Assert.Throws<NotMutableException>(() => table.StructVector = new List<SimpleStruct>());
            Assert.Throws<NotMutableException>(() => table.StructVector.Add(null));

            Assert.IsAssignableFrom<FlatBufferVector<SimpleStruct, ArrayInputBuffer>>(table.StructVector);
        }

        [Fact]
        public void TableParse_Progressive()
        {
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Progressive);
            var table = this.SerializeAndParse(options, out _);

            Assert.Throws<NotMutableException>(() => table.String = string.Empty);
            Assert.Throws<NotMutableException>(() => table.Struct.Long = 0);
            Assert.Throws<NotMutableException>(() => table.Struct = new());

            Assert.Equal(typeof(FlatBufferProgressiveVector<SimpleStruct, ArrayInputBuffer>), table.StructVector.GetType());
        }

        [Fact]
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

            Assert.True(reaped, "GC did not reclaim underlying byte buffer.");

            // The buffer has been collected. Now verify that we can read all the data as
            // we expect. This demonstrates that we've copied, as well as that we've 
            // released references.
            Assert.NotNull(table.String);
            Assert.NotNull(table.Struct);
            Assert.NotNull(table.StructVector);

            Assert.Equal("hi", table.String);
            Assert.Equal(1, table.Struct.Byte);
            Assert.Equal(2, table.Struct.Long);
            Assert.Equal(3u, table.Struct.Uint);

            Assert.Equal(4, table.StructVector[0].Byte);
            Assert.Equal(5, table.StructVector[0].Long);
            Assert.Equal(6u, table.StructVector[0].Uint);
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
            public virtual string? String { get; set; }

            [FlatBufferItem(1)]
            public virtual SimpleStruct? Struct { get; set; }

            [FlatBufferItem(2)]
            public virtual IList<SimpleStruct>? StructVector { get; set; }

            [FlatBufferItem(4)]
            public virtual SimpleTable? InnerTable { get; set; }

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

        [FlatBufferTable]
        public class SimpleTableNonVirtual
        {
            [FlatBufferItem(0)]
            public string? String { get; set; }

            [FlatBufferItem(1)]
            public SimpleStructNonVirtual? Struct { get; set; }

            [FlatBufferItem(2)]
            public IList<SimpleStructNonVirtual>? StructVector { get; set; }

            [FlatBufferItem(4)]
            public SimpleTableNonVirtual? InnerTable { get; set; }
        }

        [FlatBufferStruct]
        public class SimpleStructNonVirtual
        {
            [FlatBufferItem(0)]
            public long Long { get; set; }

            [FlatBufferItem(1)]
            public byte Byte { get; set; }

            [FlatBufferItem(2)]
            public uint Uint { get; set; }
        }

        [FlatBufferTable]
        public class EmptyTable
        {
        }
    }
}
