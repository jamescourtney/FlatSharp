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

using FlatSharp.TypeModel;
using System.Runtime.InteropServices;

namespace FlatSharpTests.Compiler;

public class ValueStructTests
{
    [Fact]
    public void ValueStruct_BasicDefinition()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            table Table ({MetadataKeys.SerializerKind}:""GreedyMutable"") {{ Struct:Struct; }}
            struct Struct ({MetadataKeys.ValueStruct}) {{ foo:int; }} 
        ";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
        Type tableType = asm.GetType("ValueStructTests.Table");
        Type structType = asm.GetType("ValueStructTests.Struct");

        Assert.NotNull(tableType);
        Assert.NotNull(structType);

        Assert.False(tableType.IsValueType);
        Assert.True(structType.IsValueType);
        Assert.True(structType.IsExplicitLayout);
    }

    [Fact]
    public void ValueStruct_Nested()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            table Table ({MetadataKeys.SerializerKind}:""GreedyMutable"") {{ StructVector:[Struct]; Item : Struct; }}
            struct Inner ({MetadataKeys.ValueStruct}) {{ B : ulong; }}
            struct Struct ({MetadataKeys.ValueStruct}) {{ A:int; B : Inner; C : ubyte; }} 
        ";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
        Type tableType = asm.GetType("ValueStructTests.Table");
        Type structType = asm.GetType("ValueStructTests.Struct");
        Type innerStructType = asm.GetType("ValueStructTests.Inner");

        Assert.NotNull(tableType);
        Assert.NotNull(structType);

        Assert.False(tableType.IsValueType);
        Assert.True(structType.IsValueType);
        Assert.True(innerStructType.IsValueType);

        Assert.True(structType.IsExplicitLayout);
        Assert.True(innerStructType.IsExplicitLayout);

        PropertyInfo structVectorProperty = tableType.GetProperty("StructVector");
        Assert.Equal(typeof(IList<>).MakeGenericType(structType), structVectorProperty.PropertyType);

        PropertyInfo structProperty = tableType.GetProperty("Item");
        Assert.Equal(typeof(Nullable<>).MakeGenericType(structType), structProperty.PropertyType);

        FieldInfo structA = structType.GetField("A");
        Assert.Equal(typeof(int), structA.FieldType);
        Assert.Equal(0, structA.GetCustomAttribute<FieldOffsetAttribute>().Value);

        FieldInfo structB = structType.GetField("B");
        Assert.Equal(innerStructType, structB.FieldType);
        Assert.Equal(8, structB.GetCustomAttribute<FieldOffsetAttribute>().Value);

        FieldInfo structC = structType.GetField("C");
        Assert.Equal(typeof(byte), structC.FieldType);
        Assert.Equal(16, structC.GetCustomAttribute<FieldOffsetAttribute>().Value);
    }

    [Fact]
    public void ValueStruct_Vectors()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            struct StructA ({MetadataKeys.ValueStruct}) {{ 
                Safe : [int : 12];
            }} 
            struct StructB ({MetadataKeys.ValueStruct}) {{
                NotSafe : [int : 12] ({MetadataKeys.UnsafeValueStructVector});
            }}";

        (Assembly asm, string csharp) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(
            schema,
            new());

        // Syntax for "safe" struct vectors is a giant switch followed by "case {index}: return ref item.__flatsharp__{vecName}_index;
        // Syntax for unsafe struct vectors is an indexed unsafe field reference access.

        // Todo: is there a better way to test this? Attribute? Roslyn? Something else?
        Assert.Contains("case 9: return ref item.__flatsharp__Safe_9", csharp);
        Assert.DoesNotContain("case 9: return ref item.__flatsharp__NotSafe_9", csharp);

        Assert.DoesNotContain("return ref System.Runtime.CompilerServices.Unsafe.Add(ref item.__flatsharp__Safe_0, index)", csharp);
        Assert.Contains("return ref System.Runtime.CompilerServices.Unsafe.Add(ref item.__flatsharp__NotSafe_0, index)", csharp);
    }

    [Theory]
    [InlineData(MemoryMarshalBehavior.Always, MemoryMarshalBehavior.Always)]
    [InlineData(MemoryMarshalBehavior.Never, MemoryMarshalBehavior.Never)]
    [InlineData(MemoryMarshalBehavior.Parse, MemoryMarshalBehavior.Parse)]
    [InlineData(MemoryMarshalBehavior.Serialize, MemoryMarshalBehavior.Serialize)]
    [InlineData(MemoryMarshalBehavior.Default, MemoryMarshalBehavior.Default)]
    [InlineData(null, MemoryMarshalBehavior.Default)]
    public void ValueStruct_MarshalOptions(MemoryMarshalBehavior? behavior, MemoryMarshalBehavior expected)
    {
        string attribute = string.Empty;
        if (behavior is not null)
        {
            attribute = $", {MetadataKeys.MemoryMarshalBehavior}:\"{behavior}\"";
        }

        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            table Table {{ A : StructA; }}
            struct StructA ({MetadataKeys.ValueStruct}{attribute}) {{ 
                Value : int;
            }}";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

        Type type = asm.GetType("ValueStructTests.StructA");
        Assert.NotNull(type);

        var flatBufferStruct = type.GetCustomAttribute<FlatBufferStructAttribute>();
        Assert.Equal(expected, flatBufferStruct.MemoryMarshalBehavior);
    }

    [Fact]
    public void ValueStruct_WriteThrough_NotAllowed_OnStruct()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            struct StructB ({MetadataKeys.ValueStruct}, {MetadataKeys.WriteThrough}) {{
                A : int;
            }}";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains($"The attribute 'fs_writeThrough' is never valid on ValueStruct elements.", ex.Message);
    }

    [Fact]
    public void ValueStruct_WriteThrough_NotAllowed_OnField()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            struct StructB ({MetadataKeys.ValueStruct}) {{
                A : int ({MetadataKeys.WriteThrough});
            }}";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains($"The attribute 'fs_writeThrough' is never valid on ValueStructField elements.", ex.Message);
    }

    [Fact]
    public void ValueStruct_SortedVectorKey_NotAllowed()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            struct StructB ({MetadataKeys.ValueStruct}) {{
                A : int;
            }}

            table Table ({MetadataKeys.SerializerKind}) {{
                A : [ Item ] ({MetadataKeys.SortedVector});
            }}

            table Item {{
                k : StructB (key);
            }}";

        var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains($"Table ValueStructTests.Item declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
    }

    [Fact]
    public void ValueStruct_Empty_NotAllowed()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ValueStructTests;
            struct StructB ({MetadataKeys.ValueStruct}) {{
            }}";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        Assert.Contains($"size 0 structs not allowed", ex.Message);
    }
}
