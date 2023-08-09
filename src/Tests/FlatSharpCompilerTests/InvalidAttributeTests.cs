/*
 * Copyright 2020 James Courtney
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

using FlatSharp.TypeModel;

namespace FlatSharpTests.Compiler;

public class InvalidAttributeTests
{
    [Fact]
    public void UnsupportedAttribute_ForceAlign_StructField()
    {
        string schema = @"
            namespace ns;
            struct Foo { Value : int (force_align: 2); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'force_align' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_ForceAlign_StructVectorField()
    {
        string schema = @"
            namespace ns;
            struct Foo { Value : [int : 4] (force_align: 2); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'force_align' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_Vector64()
    {
        string schema = @"
            namespace ns;
            table Foo { Value : [ int ] (vector64); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'vector64' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_ForceAlign_TableField()
    {
        string schema = @"
            namespace ns;
            table Foo { Value : int (force_align: 2); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'force_align' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_ForceAlign_ValueStructField()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Foo ({MetadataKeys.ValueStruct}) {{ Value : int (force_align: 2); }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'force_align' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_ForceAlign_ValueStructVectorField()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Foo ({MetadataKeys.ValueStruct}) {{ Value : [int : 4] (force_align: 2); }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'force_align' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_FlexBuffer()
    {
        string schema = @"
            namespace ns;
            table Foo { Value : [ubyte] (flexbuffer); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'flexbuffer' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_Hash()
    {
        string schema = @"
            namespace ns;
            table Foo { Value : ulong (hash:""fnv1a_64""); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'hash' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_OriginalOrder()
    {
        string schema = @"
            namespace ns;
            table Foo { Value : [ubyte] (original_order); }
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("FlatSharp does not support the 'original_order' attribute.", ex.Message);
    }

    [Fact]
    public void UnsupportedAttribute_KeyOnStruct()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Bar {{ k : int (key); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Struct member 'ns.Bar.K' declared the 'key' attribute. FlatSharp does not support keys on struct members.", ex.Message);
    }

    [Fact]
    public void FlatSharpEnumAttribute_FailsToParse()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Foo {{ Value : [ubyte] ({MetadataKeys.VectorKind}:""banana""); }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Unable to parse 'fs_vector' value 'banana'. Valid values are: IList, IReadOnlyList, Memory, ReadOnlyMemory, IIndexedVector, UnityNativeArray.", ex.Message);
    }

    [Fact]
    public void Table_DeprecatedKey()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Foo {{ Value : string (key, deprecated); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Table ns.Foo declares a key property that is deprecated.", ex.Message);
    }


    [Theory]
    [InlineData("FunUnion", "UnityNativeArray vectors only support scalar or struct generic arguments. Type = Unity.Collections.NativeArray<ns.FunUnion>.")]
    [InlineData("FunTable", "UnityNativeArray vectors only support scalar or struct generic arguments. Type = Unity.Collections.NativeArray<ns.FunTable>.")]
    [InlineData("RefStruct", "UnityNativeArray vectors only support value types. Type = Unity.Collections.NativeArray<ns.RefStruct>.")]
    [InlineData("string", "UnityNativeArray vectors only support scalar or struct generic arguments. Type = Unity.Collections.NativeArray<System.String>.")]
    public void UnityNativeVector_InvalidType(string type, string error)
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;

            struct RefStruct {{ Value: int; }}
            table FunTable {{ Value : int; }}
            union FunUnion {{ FunTable }}

            table Foo {{ Value : [ {type} ] ({MetadataKeys.VectorKind}:""UnityNativeArray""); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains(error, ex.Message);
    }

    [Fact]
    public void Struct_RequiredMember_NotAllowed()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct RefStruct {{ Value: int (required); }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("only non-scalar fields in tables may be 'required'", ex.Message);
    }

    [Fact]
    public void Struct_SharedStringMember_NotAllowed()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct RefStruct {{ Value: int ({MetadataKeys.SharedString}); }}
        ";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("The attribute 'fs_sharedString' is never valid on StructField elements.", ex.Message);
    }

    [Theory]
    [InlineData("Memory")]
    [InlineData("ReadOnlyMemory")]
    public void MemoryVector_OfNonByte_NotAllowed(string type)
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Table {{ Value: [ int ] ({MetadataKeys.VectorKind}:""{type}""); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
    }

    [Fact]
    public void IndexedVector_NonTable()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Item {{ key : int; }}
            table Table {{ Value: [ Item ] ({MetadataKeys.VectorKind}:""IIndexedVector""); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Indexed vector values must be flatbuffer tables. Type = 'ns.Item'", ex.Message);
    }


    [Fact]
    public void VectorOfUnion_WriteThrough_NotAllowed()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Item {{ key : int; }}
            union FunUnion {{ Item }}
            table Table {{ Value: [ FunUnion ] ({MetadataKeys.WriteThrough}); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Table property 'ns.Table.Value' declared the WriteThrough on a vector. Vector WriteThrough is only valid for value type structs.", ex.Message);
    }

    [Fact]
    public void VectorOfUnion_Sorted_NotAllowed()
    {
        string schema = @$"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            struct Item {{ key : int; }}
            union FunUnion {{ Item }}
            table Table {{ Value: [ FunUnion ] ({MetadataKeys.SortedVector}); }}
        ";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains("Property 'Value' declares a sorted vector, but the member is not a table. Type = System.Collections.Generic.IList<ns.FunUnion>.", ex.Message);
    }
}
