/*
 * Copyright 2021 James Courtney
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

using Microsoft.CodeAnalysis.Options;

namespace FlatSharpTests.Compiler;

public class UnionTests
{
    [Fact]
    public void UnsafeUnion_NotAllowedOnTable()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace UnionTests;
            table Test (fs_unsafeUnion) {{ x : int; }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new()));

        Assert.Contains("The attribute 'fs_unsafeUnion' is never valid on Table elements.", ex.Errors);
    }

    [Fact]
    public void UnsafeUnion_WithReferenceStruct()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace UnionTests;
            struct RefStruct {{ x : int; }}
            struct ValueStruct (fs_valueStruct) {{ x : int; }}
            union MyUnion (fs_unsafeUnion) {{ RefStruct, ValueStruct }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new()));

        Assert.Contains("FlatBufferion 'UnionTests.MyUnion' declares the 'fs_unsafeUnion' attribute. 'fs_unsafeUnion' is only valid on unions consisting only of value types.", ex.Errors);
    }

    [Fact]
    public void UnsafeUnion_WithTable()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace UnionTests;
            struct ValueStruct (fs_valueStruct) {{ x : int; }}
            struct Table {{ x : int; }}
            union MyUnion (fs_unsafeUnion) {{ Table, ValueStruct }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new()));

        Assert.Contains("FlatBufferion 'UnionTests.MyUnion' declares the 'fs_unsafeUnion' attribute. 'fs_unsafeUnion' is only valid on unions consisting only of value types.", ex.Errors);
    }

    [Fact]
    public void UnsafeUnion_OK()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace UnionTests;
            struct ValueStruct (fs_valueStruct) {{ x : int; }}
            struct ValueStructB (fs_valueStruct) {{ x : int; }}
            union MyUnion (fs_unsafeUnion) {{ ValueStruct, ValueStructB }}
        ";

        // Usage is validated in the EndtoEndTests. Compiling is good enough for this project.
        FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new());
    }

    [Theory]
    [InlineData("ValueStruct")]
    [InlineData("string")]
    [InlineData("Table")]
    public void Union_DuplicateTypes_NotAllowed(string type)
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace UnionTests;
            struct ValueStruct (fs_valueStruct) {{ x : int; }}
            table Table {{ x : int; }}
            union MyUnion {{ a : {type}, b : {type} }}
        ";

        // Usage is validated in the EndtoEndTests. Compiling is good enough for this project.
        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new()));

        Assert.Contains("FlatSharp unions may not contain duplicate types. Union = UnionTests.MyUnion", ex.Message);
    }
}
