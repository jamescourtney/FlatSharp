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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    [TestClass]
    public class SharedStringTests
    {
        public void Test_SharedStrings()
        {

        }

        [TestMethod]
        public void Test_VectorSharedStrings()
        {
            StringsVector t = new StringsVector
            {
                StringVector = new List<string> { "string", "string", "string" },
            };

            byte[] destination = new byte[1024];
            int sharedBytesWritten = FlatBufferSerializer.Default.Serialize(t, destination, new SpanWriter(new LruSharedStringWriter(5)));

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
                (byte)'s', (byte)'t', (byte)'r', (byte)'i', (byte)'n', (byte)'g',
                0 // null terminator.
            };

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, sharedBytesWritten)));
        }

        [TestMethod]
        public void Test_TableSharedStrings()
        {
            StringsTable t = new StringsTable
            {
                String1 = "string",
                String2 = "string",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            int sharedBytesWritten = FlatBufferSerializer.Default.Serialize(t, destination, new SpanWriter(new LruSharedStringWriter(5)));

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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, sharedBytesWritten)));
        }
        
        /// <summary>
        /// Tests with an LRU string writer that can only hold onto one item at a time. Each new string it sees evicts the old one.
        /// </summary>
        [TestMethod]
        public void Test_TableSharedStringsWithEviction()
        {
            StringsTable t = new StringsTable
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            int sharedBytesWritten = FlatBufferSerializer.Default.Serialize(t, destination, new SpanWriter(new LruSharedStringWriter(1)));

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

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, sharedBytesWritten)));
        }

        /// <summary>
        /// Identical to the above test but with a large enough LRU cache to handle both strings.
        /// </summary>
        [TestMethod]
        public void Test_TableSharedStringsWithoutEviction()
        {
            StringsTable t = new StringsTable
            {
                String1 = "string",
                String2 = "foo",
                String3 = "string",
            };

            byte[] destination = new byte[1024];
            int sharedBytesWritten = FlatBufferSerializer.Default.Serialize(t, destination, new SpanWriter(new LruSharedStringWriter(2)));

            byte[] expectedBytes = new byte[]
            {
                4, 0, 0, 0,         // offset to table start
                240, 255, 255, 255, // soffset to vtable.
                24, 0, 0, 0,        // uoffset to string 1
                32, 0, 0 ,0,        // uoffset to string 2
                16, 0, 0, 0,        // uoffset to string 3 
                10, 0, 16, 0,       // vtable length, table length
                4, 0, 8, 0,         // vtable(0), vtable(1)
                12, 0, 0, 0,        // vtable(2), padding
                6, 0, 0, 0,         // string0 length
                (byte)'s', (byte)'t', (byte)'r', (byte)'i',
                (byte)'n', (byte)'g', 0, 0, // null terminator + 1 byte padding
                3, 0, 0, 0,         // string1 length
                (byte)'f', (byte)'o', (byte)'o', 0, // string1 + null terminator
            };

            Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(destination.AsSpan().Slice(0, sharedBytesWritten)));
        }

        [FlatBufferTable]
        public class StringsVector : object
        {
            [FlatBufferItem(0)]
            public virtual IList<string> StringVector { get; set; }
        }

        [FlatBufferTable]
        public class StringsTable : object
        {
            [FlatBufferItem(0)]
            public virtual string String1 { get; set; }

            [FlatBufferItem(1)]
            public virtual string String2 { get; set; }

            [FlatBufferItem(2)]
            public virtual string String3 { get; set; }
        }
    }
}