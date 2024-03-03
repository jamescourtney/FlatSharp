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

[TestClass]
public class RawDataSharedStringTests
{
    [TestMethod]
    public void SharedStrings_Vector_Disabled()
    {
        var t = new SharedStringTable
        {
            Vector = new List<string> { "string", "string", "string" },
        };

        var noSharedStrings = SharedStringTable.Serializer.WithSettings(opt => opt.DisableSharedStrings());

        byte[] buffer = t.AllocateAndSerialize(noSharedStrings);

        byte[] expectedBytes = new byte[]
        {
            4, 0, 0, 0,         // offset to table start
            248, 255, 255, 255, // soffset to vtable.
            16, 0, 0, 0,        // uoffset to vector
            12, 0, 8, 0,        // vtable length, table length
            0, 0,               // vtable[0]
            0, 0,               // vtable[1]
            0, 0,               // vtable[2]
            4, 0,               // vtable[3]
            3, 0, 0, 0,         // vector length
            12, 0, 0, 0,        // uffset to first item
            20, 0, 0, 0,        // uoffset to second item
            28, 0, 0, 0,        // uoffset to third item
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

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void SharedStrings_Table_Disabled()
    {
        var t = new SharedStringTable
        {
            A = "string",
            B = "string",
            C = "string",
        };

        var noSharedStrings = SharedStringTable.Serializer.WithSettings(opt => opt.DisableSharedStrings());
        byte[] buffer = t.AllocateAndSerialize(noSharedStrings);

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

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void SharedStrings_Table()
    {
        var t = new SharedStringTable
        {
            A = "string",
            B = "string",
            C = "string",
        };

        byte[] buffer = t.AllocateAndSerialize();

        byte[] expectedBytes = new byte[]
        {
            4, 0, 0, 0,         // offset to table start
            240, 255, 255, 255, // soffset to vtable.
            24, 0, 0, 0,        // uoffset to string 1
            20, 0, 0 ,0,        // uoffset to string 2
            16, 0, 0, 0,        // uoffset to string 3 
            10, 0, 16, 0,       // vtable length, table length
            12, 0, 8, 0,        // vtable(2), vtable(1)
            4, 0, 0, 0,         // vtable(1), padding
            6, 0, 0, 0,         // string length
            (byte)'s', (byte)'t', (byte)'r', (byte)'i',
            (byte)'n', (byte)'g', 0, // null terminator.
        };

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void SharedStrings_Table_WithNull()
    {
        var t = new SharedStringTable
        {
            A = null,
            B = "string",
            C = null,
        };

        byte[] buffer = t.AllocateAndSerialize();

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

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void SharedStrings_Table_WithEviction()
    {
        var t = new SharedStringTable
        {
            A = "string",
            B = "foo",
            C = "string",
        };

        var serializer = SharedStringTable.Serializer.WithSettings(s => s.UseDefaultSharedStringWriter(1));
        byte[] buffer = t.AllocateAndSerialize(serializer);

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

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void SharedStrings_Vector()
    {
        var t = new SharedStringTable
        {
            Vector = new List<string> { "string", "string", "string" },
        };

        byte[] buffer = t.AllocateAndSerialize();

        byte[] expectedBytes = new byte[]
        {
            4, 0, 0, 0,         // offset to table start
            248, 255, 255, 255, // soffset to vtable.
            16, 0, 0, 0,        // uoffset to vector
            12, 0, 8, 0,        // vtable length, table length
            0, 0,               // vtable[0]
            0, 0,               // vtable[1]
            0, 0,               // vtable[2]
            4, 0,               // vtable[3]
            3, 0, 0, 0,         // vector length
            12, 0, 0, 0,        // uffset to first item
            8, 0, 0, 0,        // uoffset to second item
            4, 0, 0, 0,        // uoffset to third item
            6, 0, 0, 0,         // string length
            (byte)'s', (byte)'t', (byte)'r', (byte)'i',
            (byte)'n', (byte)'g', 0,
        };

        Assert.IsTrue(expectedBytes.AsSpan().SequenceEqual(buffer));
    }
}