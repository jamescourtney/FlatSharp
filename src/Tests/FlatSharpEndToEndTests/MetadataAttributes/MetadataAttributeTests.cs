/*
 * Copyright 2023 James Courtney
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

namespace FlatSharpEndToEndTests.MetadataAttributes;

[TestClass]
public class MetadataAttributesTestCases
{
#if !AOT
    [TestMethod]
    public void Enum_Attributes()
    {
        var attrs = GetAttributes<MyEnum>();
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("MyEnum", attrs["test"]);

        attrs = GetAttributes(typeof(MyEnum).GetMember("A").Single());
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("A", attrs["test"]);

        attrs = GetAttributes(typeof(MyEnum).GetMember("B").Single());
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("B", attrs["test"]);
    }
#endif

    [TestMethod]
    public void Union_Attributes()
    {
        var attrs = GetAttributes<MyUnion>();
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("MyUnion", attrs["test"]);

        attrs = GetAttributes(typeof(MyUnion).GetProperty("A"));
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("A", attrs["test"]);
    }

    [TestMethod]
    public void Value_Struct_Attributes()
    {
        var attrs = GetAttributes<ValueStruct>();
        Assert.AreEqual(2, attrs.Count);
        Assert.AreEqual("ValueStruct", attrs["test"]);
        Assert.AreEqual("0", attrs["fs_valueStruct"]); // empty

        attrs = GetAttributes(typeof(ValueStruct).GetField("I"));
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("i", attrs["test"]);
    }

    [TestMethod]
    public void Ref_Struct_Attributes()
    {
        var attrs = GetAttributes<RefStruct>();
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("RefStruct", attrs["test"]);

        attrs = GetAttributes(typeof(RefStruct).GetProperty("I"));
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("i", attrs["test"]);
    }

    [TestMethod]
    public void Service_Attributes()
    {
        var attrs = GetAttributes(typeof(EchoService));
        Assert.AreEqual(2, attrs.Count);
        Assert.AreEqual("0", attrs["fs_rpcInterface"]);
        Assert.AreEqual("EchoService", attrs["test"]);
    }

    [TestMethod]
    public void Table_Attributes()
    {
        var attrs = GetAttributes<Message>();
        Assert.AreEqual(2, attrs.Count);
        Assert.AreEqual("Lazy", attrs["fs_serializer"]);
        Assert.AreEqual("Message", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("Value"));
        Assert.AreEqual(3, attrs.Count);
        Assert.AreEqual("0", attrs["required"]);
        Assert.AreEqual("0", attrs["fs_sharedString"]);
        Assert.AreEqual("value", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("SingleInt"));
        Assert.AreEqual(1, attrs.Count);
        Assert.AreEqual("single_int", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("VectorInt"));
        Assert.AreEqual(2, attrs.Count);
        Assert.AreEqual("IList", attrs["fs_vectorKind"]);
        Assert.AreEqual("vector_int", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("ValueStruct"));
        Assert.AreEqual(3, attrs.Count);
        Assert.AreEqual("0", attrs["required"]);
        Assert.AreEqual("0", attrs["fs_writeThrough"]);
        Assert.AreEqual("value_struct", attrs["test"]);
    }

    private static Dictionary<string, string?> GetAttributes<T>() => GetAttributes(typeof(T));

    private static Dictionary<string, string?> GetAttributes(Type t)
    {
        return t
            .GetCustomAttributes<FlatBufferMetadataAttribute>()
            .Where(x => x.Kind == FlatBufferMetadataKind.FbsAttribute)
            .ToDictionary(x => x.Key, x => x.Value);

    }

    private static Dictionary<string, string?> GetAttributes(MemberInfo member)
    {
        return member
            .GetCustomAttributes<FlatBufferMetadataAttribute>()
            .Where(x => x.Kind == FlatBufferMetadataKind.FbsAttribute)
            .ToDictionary(x => x.Key, x => x.Value);
    }
}