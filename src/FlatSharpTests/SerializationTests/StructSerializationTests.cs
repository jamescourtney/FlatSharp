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
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
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

        [TestMethod]
        public void ValueSpan_Serialize_TableMember_SmallValueStruct()
        {
            var table = new SimpleTable<SmallStruct>
            {
                Struct = new SmallStruct { A = 1, B = 2, C = 3 }
            };

            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                248, 255, 255, 255,
                1,
                2,
                3, 0,
                6, 0,
                8, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void ValueSpan_Serialize_TableMember_SmallValueStruct_Default()
        {
            var table = new SimpleTable<SmallStruct>();

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
        public void ValueSpan_Serialize_TableMember_NullableSmallValueStruct()
        {
            var table = new SimpleTable<SmallStruct?>
            {
                Struct = new SmallStruct { A = 1, B = 2, C = 3 }
            };


            byte[] buffer = new byte[1024];

            byte[] expectedData =
            {
                4, 0, 0, 0,
                248, 255, 255, 255,
                1,
                2,
                3, 0,
                6, 0,
                8, 0,
                4, 0,
            };

            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, buffer);
            Assert.IsTrue(expectedData.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
        }

        [TestMethod]
        public void ValueSpan_Serialize_TableMember_NullableSmallValueStruct_Null()
        {
            var table = new SimpleTable<SmallStruct?>
            {
                Struct = null
            };

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
        public void NestedStructs()
        {
            var table = new SimpleTable<OuterStruct>
            {
                /*
                Struct = new OuterStruct
                {
                    Parent = new InnerStruct
                    {
                        Count = 1,
                        Id = 2,
                        Length = 3,
                        Prefix = 4,
                    },
                    Ratio = 3.14f,
                    Size = 2,
                    Time = 3,
                }
                */
            };

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

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct SimpleValueStruct
        {
            [FieldOffset(0)]
            public long Long;

            [FieldOffset(8)]
            public byte Byte;

            [FieldOffset(12)]
            public uint Uint;
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 4)]
        public struct SmallStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] public byte B;
            [FieldOffset(2)] public ushort C;
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct InnerStruct
        {
            [FieldOffset(0)] public ulong Id;
            [FieldOffset(8)] public short Count;
            [FieldOffset(10)] public sbyte Prefix;
            [FieldOffset(12)] public uint Length;
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 26)]
        public struct OuterStruct
        {
            [FieldOffset(0)] public InnerStruct Parent;
            [FieldOffset(16)] public int Time;
            [FieldOffset(20)] public float Ratio;
            [FieldOffset(24)] public ushort Size;
        }
    }
}
