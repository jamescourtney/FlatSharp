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
    using System.Runtime.InteropServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    [TestClass]
    public class StructSerializationTests
    {
        [TestMethod]
        public void ClassSpan_Serialize_TableMember()
        {
            var table = new SimpleTable<SimpleStruct>
            {
                Struct = new SimpleStruct { Long = 1, Byte = 2, Uint = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                236, 255, 255, 255,
                1, 0, 0, 0, 0, 0, 0, 0,
                2,
                0, 0, 0, // padding
                3, 0, 0, 0,
                6, 0,
                20, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void ValueSpan_Serialize_TableMember()
        {
            var table = new SimpleTable<SimpleValueStruct>
            {
                Struct = new SimpleValueStruct { Long = 1, Byte = 2, Uint = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                236, 255, 255, 255,
                1, 0, 0, 0, 0, 0, 0, 0,
                2,
                0, 0, 0, // padding
                3, 0, 0, 0,
                6, 0,
                20, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [FlatBufferTable]
        public class SimpleTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T Struct { get; set; }
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

        [FlatBufferStruct]
        [StructLayout(LayoutKind.Explicit)]
        public struct SimpleValueStruct
        {
            [FlatBufferItem(0)]
            [FieldOffset(0)]
            public long Long { get; set; }

            [FlatBufferItem(1)]
            [FieldOffset(8)]
            public byte Byte;

            [FlatBufferItem(2)]
            [FieldOffset(12)]
            public uint Uint;
        }
    }
}
