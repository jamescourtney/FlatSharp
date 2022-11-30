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

using System;

namespace FlatSharpEndToEndTests.RawData;

public class RawDataTableTests
{
    [Fact]
    public void AllMembersNull()
    {
        SimpleTable table = new SimpleTable();
        byte[] buffer = table.AllocateAndSerialize();

        byte[] expectedData =
        {
            4, 0, 0, 0,
            252, 255, 255, 255,
            4, 0,
            4, 0,
        };

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));
    }

    [Fact]
    public void TableWithStruct()
    {
        SimpleTable table = new SimpleTable
        {
            Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 }
        };

        byte[] buffer = table.AllocateAndSerialize();

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

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));
    }

    [Fact]
    public void TableWithStructAndString()
    {
        SimpleTable table = new SimpleTable
        {
            String = "hi",
            Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 }
        };

        byte[] buffer = table.AllocateAndSerialize();

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

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void TableWithNullReferenceStruct(FlatBufferDeserializationOption option)
    {
        SimpleTable table = new SimpleTable
        {
            OuterStruct = new()
            {
                Inner = null!,
            }
        };

        byte[] buffer = table.AllocateAndSerialize();

        byte[] expectedData =
        {
            4, 0, 0, 0,             // uoffset to table start
            236, 255, 255, 255,     // soffet to vtable (4 - x = 24 => x = -20)
            0, 0, 0, 0, 0, 0, 0, 0, // struct.long
            0, 0, 0, 0,             // struct.byte + padding
            0, 0, 0, 0,             // struct.uint
            14, 0,                  // vtable length
            20, 0,                  // table length
            0, 0,                   // index 0 offset
            0, 0,                   // Index 1 offset
            0, 0,                   // Index 2 offset
            0, 0,                   // Index 3 offset
            4, 0,                   // Index 4 offset
        };

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));

        SimpleTable parsed = SimpleTable.Serializer.Parse(buffer, option);

        Assert.Equal(0u, parsed.OuterStruct.Inner.Uint);
        Assert.Equal(0, parsed.OuterStruct.Inner.Byte);
        Assert.Equal(0L, parsed.OuterStruct.Inner.Long);
    }

    [Fact]
    public void EmptyTable_Serialize_MaxSize()
    {
        EmptyTable table = new EmptyTable();

        byte[] buffer = table.AllocateAndSerialize();

        byte[] expectedData =
        {
            4, 0, 0, 0,
            252, 255, 255, 255,
            4, 0,
            4, 0,
        };

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));

        int maxSize = EmptyTable.Serializer.GetMaxSize(table);
        Assert.Equal(23, maxSize);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Table_With_Default_Values(FlatBufferDeserializationOption option)
    {
        DefaultValueTypes defaults = new();

        Assert.True(defaults.Bool);
        Assert.Equal<byte>(1, defaults.Byte);
        Assert.Equal<sbyte>(-1, defaults.Sbyte);
        Assert.Equal<ushort>(ushort.MaxValue, defaults.Ushort);
        Assert.Equal<short>(short.MinValue, defaults.Short);
        Assert.Equal<uint>(uint.MaxValue, defaults.Uint);
        Assert.Equal<int>(int.MinValue, defaults.Int);
        Assert.Equal<float>(3.14f, defaults.Float);
        Assert.Equal<ulong>(long.MaxValue, defaults.Ulong); // FlatC issue where it can't represent defaults over long.MaxValue
        Assert.Equal<long>(long.MinValue, defaults.Long);
        Assert.Equal<double>(3.14159d, defaults.Double);
        Assert.Equal<SimpleEnum>(SimpleEnum.B, defaults.SimpleEnum);

        Assert.Null(defaults.OptBool);
        Assert.Null(defaults.OptByte);
        Assert.Null(defaults.OptSbyte);
        Assert.Null(defaults.OptUshort);
        Assert.Null(defaults.OptShort);
        Assert.Null(defaults.OptInt);
        Assert.Null(defaults.OptUint);
        Assert.Null(defaults.OptFloat);
        Assert.Null(defaults.OptUlong);
        Assert.Null(defaults.OptLong);
        Assert.Null(defaults.OptDouble);
        Assert.Null(defaults.OptSimpleEnum);

        byte[] buffer = defaults.AllocateAndSerialize();

        byte[] expectedData =
        {
            4, 0, 0, 0,
            252, 255, 255, 255,
            4, 0,
            4, 0,
        };

        Assert.True(expectedData.AsSpan().SequenceEqual(buffer));

        defaults = DefaultValueTypes.Serializer.Parse(buffer, option);

        Assert.True(defaults.Bool);
        Assert.Equal<byte>(1, defaults.Byte);
        Assert.Equal<sbyte>(-1, defaults.Sbyte);
        Assert.Equal<ushort>(ushort.MaxValue, defaults.Ushort);
        Assert.Equal<short>(short.MinValue, defaults.Short);
        Assert.Equal<uint>(uint.MaxValue, defaults.Uint);
        Assert.Equal<int>(int.MinValue, defaults.Int);
        Assert.Equal<float>(3.14f, defaults.Float);
        Assert.Equal<ulong>(long.MaxValue, defaults.Ulong); // FlatC issue where it can't represent defaults over long.MaxValue
        Assert.Equal<long>(long.MinValue, defaults.Long);
        Assert.Equal<double>(3.14159d, defaults.Double);
        Assert.Equal<SimpleEnum>(SimpleEnum.B, defaults.SimpleEnum);

        Assert.Null(defaults.OptBool);
        Assert.Null(defaults.OptByte);
        Assert.Null(defaults.OptSbyte);
        Assert.Null(defaults.OptUshort);
        Assert.Null(defaults.OptShort);
        Assert.Null(defaults.OptInt);
        Assert.Null(defaults.OptUint);
        Assert.Null(defaults.OptFloat);
        Assert.Null(defaults.OptUlong);
        Assert.Null(defaults.OptLong);
        Assert.Null(defaults.OptDouble);
        Assert.Null(defaults.OptSimpleEnum);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void DeprecatedValue_IgnoreOnRead(FlatBufferDeserializationOption option)
    {
        // hand-craft a table here:
        byte[] data =
        {
            4, 0, 0, 0,               // uoffset to the start of the table.
            236, 255, 255, 255,       // soffset_t to the vtable
            123, 0, 0, 0,             // index 0 value
            0, 0, 0, 0,               // padding to 8 byte alignment
            255, 0, 0, 0, 0, 0, 0, 0, // index 1 value

            8, 0,                     // vtable size
            20, 0,                    // table length
            4, 0,                     // index 0 offset (deprecated)
            12, 0,                    // index 1 offset
        };

        var parsed = DeprecatedItemTable.Serializer.Parse(data, option);

        Assert.Equal(0, parsed.Value);
        Assert.Equal(255L, parsed.Other);

        var nonDeprecatedParsed = NonDeprecatedItemTable.Serializer.Parse(data, option);

        Assert.Equal(123, nonDeprecatedParsed.Value);
        Assert.Equal(255L, nonDeprecatedParsed.Other);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void DeprecatedValue_IgnoreOnWrite(FlatBufferDeserializationOption option)
    {
        var deprecatedTable = new DeprecatedItemTable
        {
            Value = 123,
            Other = 255,
        };

        byte[] data =
        {
            4, 0, 0, 0,               // uoffset to the start of the table.
            244, 255, 255, 255,       // soffset_t to the vtable
            255, 0, 0, 0, 0, 0, 0, 0, // index 1 value

            8, 0,                     // vtable size
            12, 0,                    // table length
            0, 0,                     // index 0 offset (deprecated)
            4, 0,                     // index 1 offset
        };

        byte[] buffer = deprecatedTable.AllocateAndSerialize();
        Assert.True(data.AsSpan().SequenceEqual(buffer));

        var nonDeprecatedParsed = NonDeprecatedItemTable.Serializer.Parse(data, option);

        Assert.Equal(0, nonDeprecatedParsed.Value);
        Assert.Equal(255L, nonDeprecatedParsed.Other);
    }

    [Fact]
    public void ForceWriteTable_WritesDefault()
    {
        var table = new ForceWriteTable();
        byte[] buffer = table.AllocateAndSerialize();

        Assert.Equal(1, table.Value);

        byte[] expectedData =
        {
            4, 0, 0, 0,
            248, 255, 255, 255,
            1, 0, 0, 0,
            6, 0,
            8, 0,
            4, 0,
        };

        Assert.True(buffer.AsSpan().SequenceEqual(expectedData));
    }

    [Fact]
    public void NonForceWriteTable_DoesNotWriteDefault()
    {
        var table = new NonForceWriteTable();
        Assert.Equal(1, table.Value);
        byte[] buffer = table.AllocateAndSerialize();

        byte[] expectedData =
        {
            4, 0, 0, 0,
            252, 255, 255, 255,
            4, 0,
            4, 0,
        };

        Assert.True(buffer.AsSpan().SequenceEqual(expectedData));
    }
}