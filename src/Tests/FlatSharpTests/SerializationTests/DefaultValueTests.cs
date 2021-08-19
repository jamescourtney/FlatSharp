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
    using Xunit;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    
    public class DefaultValueTests
    {
        private static byte[] EmptyTable =
        {
            4, 0, 0, 0,           // uoffset to the start of the table.
            252, 255, 255, 255,   // soffset_t to the vtable
            4, 0,                 // vtable size
            4, 0,                 // table length
        };

        [Fact]
        public void Read_AllDefaultValues()
        {
            var parsed = FlatBufferSerializer.Default.Parse<AllDefaultValueTypes>(EmptyTable);

            Assert.True(parsed.Bool);
            Assert.Equal((byte)1, parsed.Byte);
            Assert.Equal((sbyte)-1, parsed.SByte);
            Assert.Equal(ushort.MaxValue, parsed.UShort);
            Assert.Equal(short.MinValue, parsed.Short);
            Assert.Equal(uint.MaxValue, parsed.UInt);
            Assert.Equal(int.MinValue, parsed.Int);
            Assert.Equal(3.14f, parsed.Float);
            Assert.Equal(ulong.MaxValue, parsed.ULong);
            Assert.Equal(long.MinValue, parsed.Long);
            Assert.Equal(3.14159d, parsed.Double);
            Assert.Equal(SimpleEnum.B, parsed.Enum);

            Assert.Null(parsed.OptBool);
            Assert.Null(parsed.OptShort);
            Assert.Null(parsed.OptUShort);
            Assert.Null(parsed.OptInt);
            Assert.Null(parsed.OptUInt);
            Assert.Null(parsed.OptLong);
            Assert.Null(parsed.OptULong);
            Assert.Null(parsed.OptDouble);
            Assert.Null(parsed.OptFloat);
            Assert.Null(parsed.OptEnum);
        }

        [Fact]
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

            Span<byte> buffer = new byte[1024];
            int count = FlatBufferSerializer.Default.Serialize(all, buffer);

            buffer = buffer.Slice(0, count);

            Assert.True(buffer.SequenceEqual(EmptyTable));
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

            [FlatBufferItem(12)]
            public virtual bool? OptBool { get; set; }

            [FlatBufferItem(13)]
            public virtual byte? OptByte { get; set; }

            [FlatBufferItem(14)]
            public virtual sbyte? OptSByte { get; set; }

            [FlatBufferItem(15)]
            public virtual ushort? OptUShort { get; set; }

            [FlatBufferItem(16)]
            public virtual short? OptShort { get; set; }

            [FlatBufferItem(17)]
            public virtual uint? OptUInt { get; set; }

            [FlatBufferItem(18)]
            public virtual int? OptInt { get; set; }

            [FlatBufferItem(19)]
            public virtual float? OptFloat { get; set; }

            [FlatBufferItem(20)]
            public virtual ulong? OptULong { get; set; }

            [FlatBufferItem(21)]
            public virtual long? OptLong { get; set; }

            [FlatBufferItem(22)]
            public virtual double? OptDouble { get; set; }

            [FlatBufferItem(23)]
            public virtual SimpleEnum? OptEnum { get; set; }
        }
    }
}
