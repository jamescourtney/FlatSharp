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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    [TestClass]
    public class SharedStringTests
    {
        [TestMethod]
        public void Test_NonSharedStringVector()
        {
            var t = new StringsVector<string>
            {
                StringVector = new List<string> { "string", "string", "string" },
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<StringsVector<string>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(3)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void Test_TableNonSharedStrings()
        {
            var t = new StringsTable<string>
            {
                String1 = "string",
                String2 = "string",
                String3 = "string",
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<string>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(5)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                32, 0, 0 ,0,        // uoffset to string 2
                40, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                4, 0, 8, 0,         // vtable(0), vtable(1)
                12, 0, 0, 0,        // vtable(2), padding
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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void Test_VectorSharedStrings()
        {
            var t = new StringsVector<SharedString>
            {
                StringVector = new List<SharedString> { "string", "string", "string" },
            };

            byte[] destination = new byte[1024];

            int maxBytes = FlatBufferSerializer.Default.GetMaxSize(t);

            var serializer = FlatBufferSerializer.Default.Compile<StringsVector<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(5)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            Assert.IsTrue(bytesWritten <= maxBytes);

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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void Test_TableSharedStrings()
        {
            var t = new StringsTable<SharedString>
            {
                String1 = "string",
                String2 = "string",
                String3 = "string",
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(5)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                20, 0, 0 ,0,        // uoffset to string 2
                16, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                4, 0, 8, 0,         // vtable(0), vtable(1)
                12, 0, 0, 0,        // vtable(2), padding
                6, 0, 0, 0,         // string length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', (byte)'n', (byte)'g',
                0 // null terminator.
            };

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void Test_TableSharedStringsWithNull()
        {
            var t = new StringsTable<SharedString>
            {
                String1 = null,
                String2 = "string",
                String3 = (string)null,
            };

            byte[] destination = new byte[1024];

            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(5)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        /// <summary>
        /// Tests with an LRU string writer that can only hold onto one item at a time. Each new string it sees evicts the old one.
        /// </summary>
        [TestMethod]
        public void Test_TableSharedStringsWithEviction()
        {
            var t = new StringsTable<SharedString>
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(1)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                32, 0, 0 ,0,        // uoffset to string 2
                36, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                4, 0, 8, 0,         // vtable(0), vtable(1)
                12, 0, 0, 0,        // vtable(2), padding
                6, 0, 0, 0,         // string0 length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', 
                (byte)'n', (byte)'g', 0, 0, // null terminator + 1 byte padding
                3, 0, 0, 0,         // string1 length
                (byte)'f', (byte)'o', (byte)'o', 0, // string1 + null terminator
                6, 0, 0, 0,
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0 // null terminator
            };

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, bytesWritten)));
        }

        /// <summary>
        /// Identical to the above test but with a large enough LRU cache to handle both strings.
        /// </summary>
        [TestMethod]
        public void Test_TableSharedStringsWithoutEviction()
        {
            var t = new StringsTable<SharedString>
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(100)
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            byte[] stringBytes = Encoding.UTF8.GetBytes("string");

            // We can't predict ordering since there is hashing under the hood.
            int firstIndex = destination.AsSpan().IndexOf(stringBytes);
            int secondIndex = destination.AsSpan().Slice(0, firstIndex + 1).IndexOf(stringBytes);
            Assert.AreEqual(-1, secondIndex);
        }

        /// <summary>
        /// Tests reading shared strings.
        /// </summary>
        [TestMethod]
        public void Test_ReadSharedStrings()
        {
            var t = new StringsTable<SharedString>
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<StringsTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringWriterFactory = () => new SharedStringWriter(100),
                    SharedStringReaderFactory = () => SharedStringReader.Create(100),
                });

            int bytesWritten = serializer.Write(SpanWriter.Instance, destination, t);

            var table = serializer.Parse(destination);
            Assert.AreEqual("string", (string)table.String1);
            Assert.AreEqual("foo", (string)table.String2);
            Assert.AreEqual("string", (string)table.String3);

            Assert.IsTrue(object.ReferenceEquals(table.String1.String, table.String3.String));
        }

        [FlatBufferTable]
        public class StringsVector<T> : object
        {
            [FlatBufferItem(0)]
            public virtual IList<T> StringVector { get; set; }
        }

        [FlatBufferTable]
        public class StringsTable<T> : object
        {
            [FlatBufferItem(0)]
            public virtual T String1 { get; set; }

            [FlatBufferItem(1)]
            public virtual T String2 { get; set; }

            [FlatBufferItem(2)]
            public virtual T String3 { get; set; }
        }
    }
}