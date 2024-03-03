/*
 * Copyright 2022 James Courtney
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

using System.IO;

namespace FlatSharpEndToEndTests.FileIdentifiers;

[TestClass]
public class FileIdentifierTests
{
    private static byte[] EmptyTableWithId =
    {
        8, 0, 0, 0,           // uoffset to the start of the table.
        97, 98, 99, 100,      // abcd file id
        252, 255, 255, 255,   // soffset_t to the vtable
        4, 0,                 // vtable size
        4, 0,                 // table length
    };

    private static byte[] EmptyTableWithoutId =
    {
        4, 0, 0, 0,           // uoffset to the start of the table.
        252, 255, 255, 255,   // soffset_t to the vtable
        4, 0,                 // vtable size
        4, 0,                 // table length
    };

    [TestMethod]
    public void FileIdentifier_Serialized()
    {
        byte[] buffer = new HasId().AllocateAndSerialize();
        Assert.IsTrue(EmptyTableWithId.AsSpan().SequenceEqual(buffer));
    }

    [TestMethod]
    public void NoFileIdentifier_Serialized()
    {
        byte[] buffer = new NoId().AllocateAndSerialize();
        Assert.IsTrue(EmptyTableWithoutId.AsSpan().SequenceEqual(buffer));
    }
}