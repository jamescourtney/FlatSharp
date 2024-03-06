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

[TestClass]
public class DeserializationOptionsTests
{
    private readonly DeserializationOptionsTable Template = new()
    {
        FirstStruct = new() { First = 1, Second = 2, SecondStruct = new() { Value = 3 } },
        SecondStruct = new() { Value = 4, },
        Str = "foo bar",
        Union = new("banana"),
    };

    [TestMethod]
    public void DeserializationOption_Lazy()
    {
        DeserializationOptionsTable parsed = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.IsNotNull(obj.InputBuffer);

        Assert.AreNotSame(parsed.Str, parsed.Str);
        Assert.AreNotSame(parsed.FirstStruct, parsed.FirstStruct);
        Assert.AreNotSame(parsed.SecondStruct, parsed.SecondStruct);

        var first = parsed.FirstStruct;
        Assert.AreNotSame(first.SecondStruct, first.SecondStruct);

        Assert.AreEqual(Template.Str, parsed.Str);
        Assert.AreEqual(Template.FirstStruct.First, parsed.FirstStruct.First);
        Assert.AreEqual(Template.FirstStruct.Second, parsed.FirstStruct.Second);
        Assert.AreEqual(Template.FirstStruct.SecondStruct.Value, parsed.FirstStruct.SecondStruct.Value);

        Assert.AreEqual(3, parsed.Union.Value.Discriminator);

        var union = parsed.Union.Value;
        Assert.AreEqual("banana", union.str);
        Assert.AreSame(union.str, union.str);

        Assert.ThrowsException<NotMutableException>(() => parsed.Str = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.First = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct.Value = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.Union = null);
    }

    [TestMethod]
    public void DeserializationOption_Progressive()
    {
        DeserializationOptionsTable parsed = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out _);
        Assert.IsNotNull(obj.InputBuffer);

        Assert.AreSame(parsed.Str, parsed.Str);
        Assert.AreSame(parsed.FirstStruct, parsed.FirstStruct);
        Assert.AreSame(parsed.SecondStruct, parsed.SecondStruct);

        var first = parsed.FirstStruct;
        Assert.AreSame(first.SecondStruct, first.SecondStruct);

        Assert.AreEqual(Template.Str, parsed.Str);
        Assert.AreEqual(Template.FirstStruct.First, parsed.FirstStruct.First);
        Assert.AreEqual(Template.FirstStruct.Second, parsed.FirstStruct.Second);
        Assert.AreEqual(Template.FirstStruct.SecondStruct.Value, parsed.FirstStruct.SecondStruct.Value);

        Assert.AreEqual(3, parsed.Union.Value.Discriminator);

        var union = parsed.Union.Value;
        Assert.AreEqual("banana", union.str);
        Assert.AreSame(union.str, union.str);

        Assert.ThrowsException<NotMutableException>(() => parsed.Str = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.First = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct.Value = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.Union = null);
    }

    [TestMethod]
    public void DeserializationOption_Greedy()
    {
        DeserializationOptionsTable parsed = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        Assert.AreSame(parsed.Str, parsed.Str);
        Assert.AreSame(parsed.FirstStruct, parsed.FirstStruct);
        Assert.AreSame(parsed.SecondStruct, parsed.SecondStruct);

        var first = parsed.FirstStruct;
        Assert.AreSame(first.SecondStruct, first.SecondStruct);

        Assert.AreEqual(Template.Str, parsed.Str);
        Assert.AreEqual(Template.FirstStruct.First, parsed.FirstStruct.First);
        Assert.AreEqual(Template.FirstStruct.Second, parsed.FirstStruct.Second);
        Assert.AreEqual(Template.FirstStruct.SecondStruct.Value, parsed.FirstStruct.SecondStruct.Value);

        Assert.AreEqual(3, parsed.Union.Value.Discriminator);

        var union = parsed.Union.Value;
        Assert.AreEqual("banana", union.str);
        Assert.AreSame(union.str, union.str);

        Assert.ThrowsException<NotMutableException>(() => parsed.Str = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.First = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct = null);
        Assert.ThrowsException<NotMutableException>(() => parsed.FirstStruct.SecondStruct.Value = 0);
        Assert.ThrowsException<NotMutableException>(() => parsed.Union = null);
    }

    [TestMethod]
    public void DeserializationOption_GreedyMutable()
    {
        DeserializationOptionsTable parsed = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        Assert.AreSame(parsed.Str, parsed.Str);
        Assert.AreSame(parsed.FirstStruct, parsed.FirstStruct);
        Assert.AreSame(parsed.SecondStruct, parsed.SecondStruct);

        var first = parsed.FirstStruct;
        Assert.AreSame(first.SecondStruct, first.SecondStruct);

        Assert.AreEqual(Template.Str, parsed.Str);
        Assert.AreEqual(Template.FirstStruct.First, parsed.FirstStruct.First);
        Assert.AreEqual(Template.FirstStruct.Second, parsed.FirstStruct.Second);
        Assert.AreEqual(Template.FirstStruct.SecondStruct.Value, parsed.FirstStruct.SecondStruct.Value);

        Assert.AreEqual(3, parsed.Union.Value.Discriminator);

        var union = parsed.Union.Value;
        Assert.AreEqual("banana", union.str);
        Assert.AreSame(union.str, union.str);

        parsed.FirstStruct.SecondStruct.Value = 0;
        parsed.FirstStruct.SecondStruct = null;
        parsed.FirstStruct.First = 0;
        parsed.FirstStruct = null;
        parsed.SecondStruct = null;
        parsed.Str = null;
        parsed.Union = null;

        // everything is default now. Reparse
        byte[] reserialized = parsed.AllocateAndSerialize();
        Assert.AreEqual(12, reserialized.Length); // empty buffer.
    }

    private DeserializationOptionsTable SerializeAndParse(FlatBufferDeserializationOption option, out IFlatBufferDeserializedObject obj, out byte[] inputBuffer)
    {
        inputBuffer = Template.AllocateAndSerialize();

        var table = DeserializationOptionsTable.Serializer.Parse(inputBuffer, option);

        obj = table as IFlatBufferDeserializedObject;

        Assert.IsNotNull(obj);
        Assert.AreEqual(option, obj.DeserializationContext.DeserializationOption);
        return table;
    }
}