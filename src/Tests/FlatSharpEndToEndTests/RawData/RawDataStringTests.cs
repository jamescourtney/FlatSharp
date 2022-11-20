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

namespace FlatSharpEndToEndTests.TableMembers;

public class RawDataStringTests
{
    [Fact]
    public void EmptyString()
    {
        var root = new StringTable
        {
            ItemStandard = string.Empty,
        };

        byte[] data = root.AllocateAndSerialize();

        byte[] expectedResult =
        {
            4, 0, 0, 0,          // offset to table start
            248, 255, 255, 255,  // soffset to vtable (-8)
            12, 0, 0, 0,         // uoffset_t to string
            6, 0,                // vtable length
            8, 0,                // table length
            4, 0,                // offset of index 0 field
            0, 0,                // padding to 4-byte alignment
            0, 0, 0, 0,          // vector length
            0,                   // null terminator (special case for strings).
        };

        Assert.True(expectedResult.AsSpan().SequenceEqual(data));
    }

    [Fact]
    public void DeprecatedString()
    {
        var root = new StringTable
        {
            ItemDeprecated = string.Empty,
        };

        byte[] data = root.AllocateAndSerialize();

        byte[] expectedResult =
        {
            4, 0, 0, 0,          // offset to table start
            252, 255, 255, 255,  // soffset to vtable (-4)
            4, 0,                // vtable length
            4, 0,                // table length
        };

        Assert.True(expectedResult.AsSpan().SequenceEqual(data));
    }

    [Fact]
    public void SimpleString()
    {
        var root = new StringTable
        {
            ItemStandard = new string(new char[] { (char)1, (char)2, (char)3 }),
        };

        Span<byte> target = new byte[10240];
        int offset = FlatBufferSerializer.Default.Serialize(root, target);
        target = target.Slice(0, offset);

        byte[] expectedResult =
        {
            4, 0, 0, 0,          // offset to table start
            248, 255, 255, 255,  // soffset to vtable (-8)
            12, 0, 0, 0,         // uoffset_t to vector
            6, 0,                // vtable length
            8, 0,                // table length
            4, 0,                // offset of index 0 field
            0, 0,                // padding to 4-byte alignment
            3, 0, 0, 0,          // vector length
            1, 2, 3, 0,          // data + null terminator (special case for string vectors).
        };

        Assert.True(expectedResult.AsSpan().SequenceEqual(target));
    }
}