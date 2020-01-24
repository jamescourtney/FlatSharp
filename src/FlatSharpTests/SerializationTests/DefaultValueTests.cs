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
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    [TestClass]
    public class DefaultValueTests
    {
        private static byte[] EmptyTable =
        {
            4, 0, 0, 0,           // uoffset to the start of the table.
            252, 255, 255, 255,   // soffset_t to the vtable
            4, 0,                 // vtable size
            4, 0,                 // table length
        };

        [TestMethod]
        public void Read_AllDefaultValues()
        {
            var parsed = FlatBufferSerializer.Default.Parse<AllDefaultValueTypes>(EmptyTable);

            Assert.IsTrue(parsed.Bool);
            Assert.AreEqual((byte)1, parsed.Byte);
            Assert.AreEqual((sbyte)-1, parsed.SByte);
            Assert.AreEqual(ushort.MaxValue, parsed.UShort);
            Assert.AreEqual(short.MinValue, parsed.Short);
            Assert.AreEqual(uint.MaxValue, parsed.UInt);
            Assert.AreEqual(int.MinValue, parsed.Int);
            Assert.AreEqual(3.14f, parsed.Float);
            Assert.AreEqual(ulong.MaxValue, parsed.ULong);
            Assert.AreEqual(long.MinValue, parsed.Long);
            Assert.AreEqual(3.14159d, parsed.Double);
            Assert.AreEqual(SimpleEnum.B, parsed.Enum);
        }

        [TestMethod]
        public void Write_AllDefaultValues()
        {
            var all = new AllDefaultValueTypes()
            {
                Bool = true,
                Byte = 1,
                SByte = -1,
                UShort = ushort.MaxValue,
                Short = short.MinValue,
                UInt = uint.MaxValue,
                Int = int.MinValue,
                Float = 3.14f,
                ULong = ulong.MaxValue,
                Long = long.MinValue,
                Double = 3.14159d,
                Enum = SimpleEnum.B,
            };

            Span<byte> buffer = new byte[100];
            int count = FlatBufferSerializer.Default.Serialize(all, buffer);

            buffer = buffer.Slice(0, count);

            Assert.IsTrue(buffer.SequenceEqual(EmptyTable));
        }

        [FlatBufferEnum(typeof(sbyte))]
        public enum SimpleEnum : sbyte
        {
            A = 1,
            B = 2,
            C = 3,
        }

        [FlatBufferTable]
        public class AllDefaultValueTypes
        {
            [FlatBufferItem(0, DefaultValue = true)]
            public virtual bool Bool { get; set; }

            [FlatBufferItem(1, DefaultValue = (byte)1)]
            public virtual byte Byte { get; set; }

            [FlatBufferItem(2, DefaultValue = (sbyte)-1)]
            public virtual sbyte SByte { get; set; }

            [FlatBufferItem(3, DefaultValue = ushort.MaxValue)]
            public virtual ushort UShort { get; set; }

            [FlatBufferItem(4, DefaultValue = short.MinValue)]
            public virtual short Short { get; set; }

            [FlatBufferItem(5, DefaultValue = uint.MaxValue)]
            public virtual uint UInt { get; set; }

            [FlatBufferItem(6, DefaultValue = int.MinValue)]
            public virtual int Int { get; set; }

            [FlatBufferItem(7, DefaultValue = 3.14f)]
            public virtual float Float { get; set; }

            [FlatBufferItem(8, DefaultValue = ulong.MaxValue)]
            public virtual ulong ULong { get; set; }

            [FlatBufferItem(9, DefaultValue = long.MinValue)]
            public virtual long Long { get; set; }

            [FlatBufferItem(10, DefaultValue = 3.14159d)]
            public virtual double Double { get; set; }

            [FlatBufferItem(11, DefaultValue = SimpleEnum.B)]
            public virtual SimpleEnum Enum { get; set; }
        }
    }
}
