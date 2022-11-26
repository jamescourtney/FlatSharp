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
}