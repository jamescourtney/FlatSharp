﻿/*
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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    public class ValueStructTests
    {
        [Fact]
        public void ValueStruct_BasicDefinition()
        {
            string schema = $@"
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
                namespace ValueStructTests;
                table Table ({MetadataKeys.SerializerKind}:""GreedyMutable"") {{ StructVector:[Struct]; Struct : Struct; }}
                struct Struct ({MetadataKeys.ValueStruct}) {{ A:int; B : Inner; C : ubyte; }} 
                struct Inner ({MetadataKeys.ValueStruct}) {{ B : ulong; }}
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

            PropertyInfo structProperty = tableType.GetProperty("Struct");
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
                namespace ValueStructTests;
                struct StructA ({MetadataKeys.ValueStruct}) {{ 
                    Safe : [int : 12];
                }} 
                struct StructB ({MetadataKeys.ValueStruct}) {{
                    NotSafe : [int : 12] ({MetadataKeys.UnsafeValueStructVector});
                }}";

            string csharp = FlatSharpCompiler.TestHookCreateCSharp(schema, new());

            // Syntax for "safe" struct vectors is a giant switch followed by "case {index}: return ref item.__flatsharp__{vecName}_index;
            // Syntax for unsafe struct vectors is a fixed statement in an unsafe context.

            // Todo: is there a better way to test this? Attribute? Roslyn? Something else?
            Assert.Contains("case 9: return ref item.__flatsharp_Safe_9", csharp);
            Assert.DoesNotContain("case 9: return ref item.__flatsharp_NotSafe_9", csharp);

            Assert.DoesNotContain("fixed (StructA* pItem = &item)", csharp);
            Assert.Contains("fixed (StructB* pItem = &item)", csharp);
        }
    }
}
