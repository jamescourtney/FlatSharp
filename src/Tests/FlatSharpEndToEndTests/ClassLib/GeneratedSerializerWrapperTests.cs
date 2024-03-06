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

using FlatSharp;

namespace FlatSharpEndToEndTests.ClassLib;

[TestClass]
public class GeneratedSerializerWrapperTests
{
    [TestMethod]
    public void Serialize_DestinationBuffer_TooShort()
    {
        FlatBufferSerializerNonGenericTests.SomeTable table = new();
        table.A = 3;

        var serializer = FlatBufferSerializerNonGenericTests.SomeTable.Serializer;

        byte[] destination = new byte[7];
        Assert.ThrowsException<BufferTooSmallException>(() => serializer.Write(destination, table));
    }

    [TestMethod]
    public void Serialize_RootTable_Null()
    {
        var serializer = FlatBufferSerializerNonGenericTests.SomeTable.Serializer;

        byte[] destination = new byte[7];
        var ex = Assert.ThrowsException<ArgumentNullException>(() => serializer.Write(destination, null));
        Assert.IsTrue(ex.Message.Contains("The root table may not be null."));
    }
}
