/*
 * Copyright 2020 James Courtney
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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    
    public class SharedStringTests
    {
        [Fact]
        public void Test_NonSharedStringVector()
        {
            var t = new RegularStringsVector
            {
                StringVector = new List<string> { "string", "string", "string" },
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<RegularStringsVector>();
            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                248, 255, 255, 255, // soffset to vtable.
                12, 0, 0, 0,        // uoffset to vector
                6, 0, 8, 0,         // vtable length, table length
                4, 0, 0, 0,         // offset within table to item 0 + alignment
                3, 0, 0, 0,         // vector length
                12, 0, 0, 0,        // uffset to first item
                20, 0, 0, 0,         // uoffset to second item
                28, 0, 0, 0,         // uoffset to third item
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', 
                (byte)'n', (byte)'g', 0, 0, // null terminator + padding
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0, 0, // null terminator + padding
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0,  // null terminator
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void Test_TableNonSharedStrings()
        {
            var t = new RegularStringsTable
            {
                String1 = "string",
                String2 = "string",
                String3 = "string",
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<RegularStringsTable>();
            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                32, 0, 0 ,0,        // uoffset to string 2
                40, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                12, 0, 8, 0,        // vtable(2), vtable(1)
                4, 0, 0, 0,         // vtable(1), padding
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', 
                (byte)'n', (byte)'g', 0, 0, // null terminator.
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0, 0, // null terminator.
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0, // null terminator.
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void Test_VectorSharedStrings()
        {
            var t = new SharedStringsVector
            {
                StringVector = new List<string> { "string", "string", "string" },
            };

            byte[] destination = new byte[1024];

            int maxBytes = FlatBufferSerializer.Default.GetMaxSize(t);
            var serializer = FlatBufferSerializer.Default.Compile<SharedStringsVector>();

            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            Assert.True(bytesWritten <= maxBytes);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                248, 255, 255, 255, // soffset to vtable.
                12, 0, 0, 0,        // uoffset to vector
                6, 0, 8, 0,         // vtable length, table length
                4, 0, 0, 0,         // offset within table to item 0 + alignment
                3, 0, 0, 0,         // vector length
                12, 0, 0, 0,        // uffset to first item
                8, 0, 0, 0,         // uoffset to second item
                4, 0, 0, 0,         // uoffset to third item
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', 
                (byte)'n', (byte)'g', 0 // null terminator.
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void Test_TableSharedStrings()
        {
            var t = new SharedStringsTable
            {
                String1 = "string",
                String2 = "string",
                String3 = "string",
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<SharedStringsTable>();
            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                20, 0, 0 ,0,        // uoffset to string 2
                16, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                12, 0, 8, 0,        // vtable(0), vtable(1)
                4, 0, 0, 0,         // vtable(2), padding
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', (byte)'n', (byte)'g',
                0 // null terminator.
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [Fact]
        public void Test_TableSharedStringsWithNull()
        {
            var t = new SharedStringsTable
            {
                String1 = null,
                String2 = "string",
                String3 = (string)null,
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<SharedStringsTable>();
            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                248, 255, 255, 255, // soffset to vtable.
                12, 0, 0 ,0,        // uoffset to string
                8, 0, 8, 0,         // vtable length, table length
                0, 0, 4, 0,         // vtable(0), vtable(1)
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0 // null terminator.
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        /// <summary>
        /// Tests with an LRU string writer that can only hold onto one item at a time. Each new string it sees evicts the old one.
        /// </summary>
        [Fact]
        public void Test_TableSharedStringsWithEviction()
        {
            var t = new SharedStringsTable
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<SharedStringsTable>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(1)
                });

            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                32, 0, 0 ,0,        // uoffset to string 2
                36, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                12, 0, 8, 0,        // vtable(0), vtable(1)
                4, 0, 0, 0,         // vtable(2), padding
                6, 0, 0, 0,         // string0 length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', 
                (byte)'n', (byte)'g', 0, 0, // null terminator + 1 byte padding
                3, 0, 0, 0,         // string1 length
                (byte)'f', (byte)'o', (byte)'o', 0, // string1 + null terminator
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0 // null terminator
            };

            Assert.True(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        /// <summary>
        /// Identical to the above test but with a large enough LRU cache to handle both strings.
        /// </summary>
        [Fact]
        public void Test_TableSharedStringsWithoutEviction()
        {
            var t = new SharedStringsTable
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<SharedStringsTable>();

            int bytesWritten = serializer.Write(default(SpanWriter), destination, t);

            byte[] stringBytes = Encoding.UTF8.GetBytes("string");

            // We can't predict ordering since there is hashing under the hood.
            int firstIndex = destination.AsSpan().IndexOf(stringBytes);
            int secondIndex = destination.AsSpan().Slice(0, firstIndex + 1).IndexOf(stringBytes);
            Assert.Equal(-1, secondIndex);
        }

        [FlatBufferTable]
        public class SharedStringsVector : object
        {
            [FlatBufferItem(0, SharedString = true)]
            public virtual IList<string>? StringVector { get; set; }
        }

        [FlatBufferTable]
        public class RegularStringsVector : object
        {
            [FlatBufferItem(0)]
            public virtual IList<string>? StringVector { get; set; }
        }

        [FlatBufferTable]
        public class SharedStringsTable : object
        {
            [FlatBufferItem(0, SharedString = true)]
            public virtual string? String1 { get; set; }

            [FlatBufferItem(1, SharedString = true)]
            public virtual string? String2 { get; set; }

            [FlatBufferItem(2, SharedString = true)]
            public virtual string? String3 { get; set; }
        }

        [FlatBufferTable]
        public class RegularStringsTable : object
        {
            [FlatBufferItem(0)]
            public virtual string? String1 { get; set; }

            [FlatBufferItem(1)]
            public virtual string? String2 { get; set; }

            [FlatBufferItem(2)]
            public virtual string? String3 { get; set; }
        }
    }
}