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

using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Other.Namespace.Foobar;
using SChannel = System.Threading.Channels.Channel;

namespace FlatSharpEndToEndTests.MetadataAttributes;

public class MetadataAttributesTestCases
{
    [Fact]
    public void Enum_Attributes()
    {
        var attrs = GetAttributes<MyEnum>();
        Assert.Single(attrs);
        Assert.Equal("MyEnum", attrs["test"]);

        attrs = GetAttributes(typeof(MyEnum).GetMember("A").Single());
        Assert.Single(attrs);
        Assert.Equal("A", attrs["test"]);

        attrs = GetAttributes(typeof(MyEnum).GetMember("B").Single());
        Assert.Single(attrs);
        Assert.Equal("B", attrs["test"]);
    }

    [Fact]
    public void Union_Attributes()
    {
        var attrs = GetAttributes<MyUnion>();
        Assert.Single(attrs);
        Assert.Equal("MyUnion", attrs["test"]);
    }

    [Fact]
    public void Value_Struct_Attributes()
    {
        var attrs = GetAttributes<ValueStruct>();
        Assert.Equal(2, attrs.Count);
        Assert.Equal("ValueStruct", attrs["test"]);
        Assert.Equal("0", attrs["fs_valueStruct"]); // empty

        attrs = GetAttributes(typeof(ValueStruct).GetField("I"));
        Assert.Single(attrs);
        Assert.Equal("i", attrs["test"]);
    }

    [Fact]
    public void Ref_Struct_Attributes()
    {
        var attrs = GetAttributes<RefStruct>();
        Assert.Single(attrs);
        Assert.Equal("RefStruct", attrs["test"]);

        attrs = GetAttributes(typeof(RefStruct).GetProperty("I"));
        Assert.Single(attrs);
        Assert.Equal("i", attrs["test"]);
    }

    [Fact]
    public void Service_Attributes()
    {
        var attrs = GetAttributes(typeof(EchoService));
        Assert.Equal(2, attrs.Count);
        Assert.Equal("0", attrs["fs_rpcInterface"]);
        Assert.Equal("EchoService", attrs["test"]);
    }

    [Fact]
    public void Table_Attributes()
    {
        var attrs = GetAttributes<Message>();
        Assert.Equal(2, attrs.Count);
        Assert.Equal("Lazy", attrs["fs_serializer"]);
        Assert.Equal("Message", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("Value"));
        Assert.Equal(3, attrs.Count);
        Assert.Equal("0", attrs["required"]);
        Assert.Equal("0", attrs["fs_sharedString"]);
        Assert.Equal("value", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("SingleInt"));
        Assert.Single(attrs);
        Assert.Equal("single_int", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("VectorInt"));
        Assert.Equal(2, attrs.Count);
        Assert.Equal("IList", attrs["fs_vectorKind"]);
        Assert.Equal("vector_int", attrs["test"]);

        attrs = GetAttributes(typeof(Message).GetProperty("ValueStruct"));
        Assert.Equal(3, attrs.Count);
        Assert.Equal("0", attrs["required"]);
        Assert.Equal("0", attrs["fs_writeThrough"]);
        Assert.Equal("value_struct", attrs["test"]);
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