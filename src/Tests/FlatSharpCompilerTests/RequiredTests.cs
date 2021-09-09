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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    
    public class RequiredTests
    {
        [Fact]
        public void Required_Attribute_OnParse()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace RequiredTests;
            table NonRequiredTable ({MetadataKeys.SerializerKind}:""Lazy"") {{ Struct:RefStruct; String:string; VStruct : ValueStruct; }}
            table Table ({MetadataKeys.SerializerKind}:""Lazy"") {{ Struct:RefStruct ({MetadataKeys.Required}); String:string ({MetadataKeys.Required}); VStruct : ValueStruct ({MetadataKeys.Required}); }}
            struct RefStruct {{ foo:int; }} 
            struct ValueStruct ({MetadataKeys.ValueStruct}) {{ A : int; }}
            ";

            (Assembly asm, string code) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(schema, new());
            Type requiredTableType = asm.GetType("RequiredTests.Table");
            Type nonRequiredTable = asm.GetType("RequiredTests.NonRequiredTable");

            Assert.True(requiredTableType.GetProperty("Struct").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.True(requiredTableType.GetProperty("String").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.True(requiredTableType.GetProperty("VStruct").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.Null(Nullable.GetUnderlyingType(requiredTableType.GetProperty("VStruct").PropertyType)); // required => not nullable.

            Assert.False(nonRequiredTable.GetProperty("Struct").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.False(nonRequiredTable.GetProperty("String").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.False(nonRequiredTable.GetProperty("VStruct").GetCustomAttribute<FlatBufferItemAttribute>().Required);
            Assert.NotNull(Nullable.GetUnderlyingType(nonRequiredTable.GetProperty("VStruct").PropertyType)); // not required => nullable.

            ISerializer nonRequiredSerializer = (ISerializer)nonRequiredTable.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            dynamic nonRequiredItem = Activator.CreateInstance(nonRequiredTable);

            byte[] buffer = new byte[1024];
            nonRequiredSerializer.Write(buffer, (object)nonRequiredItem);

            ISerializer serializer = (ISerializer)requiredTableType.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            dynamic parsed = serializer.Parse(buffer);

            var ex = Assert.Throws<System.IO.InvalidDataException>(
                () => parsed.String);

            Assert.Equal(
                "Table property 'RequiredTests.Table.String' is marked as required, but was missing from the buffer.",
                ex.Message);

            ex = Assert.Throws<System.IO.InvalidDataException>(
                () => parsed.Struct);

            Assert.Equal(
                "Table property 'RequiredTests.Table.Struct' is marked as required, but was missing from the buffer.",
                ex.Message);
        }

        [Fact]
        public void Required_Attribute_OnSerialize()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace RequiredTests;
            table Table ({MetadataKeys.SerializerKind}:""Lazy"") {{ Struct:Struct ({MetadataKeys.Required}); String:string ({MetadataKeys.Required}); }}
            struct Struct {{ foo:int; }} 
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type tableType = asm.GetType("RequiredTests.Table");

            byte[] buffer = new byte[1024];

            ISerializer serializer = (ISerializer)tableType.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);

            Assert.Throws<InvalidOperationException>(
                () => serializer.Write(buffer, Activator.CreateInstance(tableType)));
        }
    }
}
