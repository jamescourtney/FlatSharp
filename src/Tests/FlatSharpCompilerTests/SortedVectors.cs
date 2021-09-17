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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Xunit;

    
    public class SortedVectors
    {
        [Fact]
        public void SortedVector_StringKey()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace ns;
table Monster ({MetadataKeys.SerializerKind}) {{
  Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IReadOnlyList"", {MetadataKeys.SortedVector});
}}

table VectorMember {{
    Key:string ({MetadataKeys.Key});
    Data:int32;
}}
"; 
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("ns.VectorMember");

            var prop = memberType.GetProperty("Key");
            Assert.NotNull(prop);
            Assert.Equal(typeof(string), prop.PropertyType);
            Assert.True(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);

            prop = memberType.GetProperty("Data");
            Assert.NotNull(prop);
            Assert.Equal(typeof(int), prop.PropertyType);
            Assert.False(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
        }

        [Fact]
        public void SortedVector_IntKey()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace ns;
table Monster ({MetadataKeys.SerializerKind}) {{
  Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IReadOnlyList"", {MetadataKeys.SortedVector});
}}

table VectorMember {{
    Data:string;
    Key:int32 (key);
}}
";
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("ns.VectorMember");

            var prop = memberType.GetProperty("Key");
            Assert.NotNull(prop);
            Assert.Equal(typeof(int), prop.PropertyType);
            Assert.True(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);

            prop = memberType.GetProperty("Data");
            Assert.NotNull(prop);
            Assert.Equal(typeof(string), prop.PropertyType);
            Assert.False(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
        }

        /// <summary>
        /// Tests that the compiler doesn't really care about what junk you write when a serializer
        /// is not present.
        /// </summary>
        [Fact]
        public void SortedVector_InvalidKeys_NoSerializer()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace ns;
enum TestEnum : ubyte {{ A = 0, One = 1, Two = 2 }}

union TestUnion {{ VectorMember }}

table Monster {{
  Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IReadOnlyList"", {MetadataKeys.SortedVector});
}}
struct FakeStruct {{ StructData:int32; }}

table VectorMember {{
    Data:string ({MetadataKeys.Key});
    Key:int32;
    Enum:TestEnum;
    Struct:FakeStruct;
    Union:TestUnion;
    StructVector:[FakeStruct];
}}

";
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("ns.VectorMember");
            foreach (var prop in memberType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.Name == "Vector")
                {
                    Assert.True(prop.GetCustomAttribute<FlatBufferItemAttribute>().SortedVector);
                }
                else
                {
                    Assert.False(prop.GetCustomAttribute<FlatBufferItemAttribute>().SortedVector);
                }

                if (prop.Name == "Data")
                {
                    Assert.True(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
                }
                else
                {
                    Assert.False(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
                }
            }
        }

        [Fact]
        public void SortedVector_InvalidKeys_WithSerializer()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace ns;
enum TestEnum : ubyte {{ One = 1, Two = 2 }}

union TestUnion {{ VectorMember }}

table Monster ({MetadataKeys.SerializerKind}) {{
  Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IReadOnlyList"", {MetadataKeys.SortedVector}, {MetadataKeys.Key});
}}
struct FakeStruct {{ Data:int32 ({MetadataKeys.Key}); }}

table VectorMember {{
    Data:string ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
    Key:int32 ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
    Enum:TestEnum ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
    Struct:FakeStruct ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
    Union:TestUnion ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
    StructVector:[FakeStruct] ({MetadataKeys.SortedVector}, {MetadataKeys.Key});
}}

";
            Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        }

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_String() => this.SortedVector_IndexedVector_KeyTypesCorrect<string>("string");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Bool() => this.SortedVector_IndexedVector_KeyTypesCorrect<bool>("bool");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Byte() => this.SortedVector_IndexedVector_KeyTypesCorrect<byte>("ubyte");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_SByte() => this.SortedVector_IndexedVector_KeyTypesCorrect<sbyte>("byte");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_UShort() => this.SortedVector_IndexedVector_KeyTypesCorrect<ushort>("ushort");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Short() => this.SortedVector_IndexedVector_KeyTypesCorrect<short>("short");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_UInt() => this.SortedVector_IndexedVector_KeyTypesCorrect<uint>("uint");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Int() => this.SortedVector_IndexedVector_KeyTypesCorrect<int>("int");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_ULong() => this.SortedVector_IndexedVector_KeyTypesCorrect<ulong>("ulong");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Long() => this.SortedVector_IndexedVector_KeyTypesCorrect<long>("long");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Float() => this.SortedVector_IndexedVector_KeyTypesCorrect<float>("float");

        [Fact]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Double() => this.SortedVector_IndexedVector_KeyTypesCorrect<double>("double");

        [Fact]
        public void SortedVector_IndexedVector_InvalidKey()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Monster ({MetadataKeys.SerializerKind}) {{
              Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IIndexedVector"");
            }}
            table VectorMember {{
                Data:DoesNotExist ({MetadataKeys.Key});
            }}";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains(
                "error: 'key' field must be string or scalar type",
                ex.Message);
        }

        [Fact]
        public void SortedVector_IndexedVector_UnknownTableType()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Monster ({MetadataKeys.SerializerKind}) {{
              Vector:[WhoKnows] ({MetadataKeys.VectorKind}:""IIndexedVector"");
            }}";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains(
                $"error: type referenced but not defined (check namespace): WhoKnows",
                ex.Message);
        }

        [Fact]
        public void SortedVector_IndexedVector_NoKey()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Monster ({MetadataKeys.SerializerKind}) {{
              Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IIndexedVector"");
            }}
            table VectorMember {{
                Data:string;
            }}";

            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains(
                "Indexed vector values must have a property with the key attribute defined. Table = 'ns.VectorMember'",
                ex.Message);
        }

        [Fact]
        public void SortedVector_IndexedVector_MultipleKeys()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Monster ({MetadataKeys.SerializerKind}) {{
              Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IIndexedVector"");
            }}
            table VectorMember {{
                Data:string ({MetadataKeys.Key});
                Data2:string ({MetadataKeys.Key});
            }}";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains(
                "error: only one field may be set as 'key'",
                ex.Message);
        }

        private void SortedVector_IndexedVector_KeyTypesCorrect<TKeyType>(string type, string metadata = null)
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ns;
            table Monster ({MetadataKeys.SerializerKind}) {{
              Vector:[VectorMember] ({MetadataKeys.VectorKind}:""IIndexedVector"");
            }}
            table VectorMember {{
                Data:{type} ({MetadataKeys.Key} {(!string.IsNullOrEmpty(metadata) ? ", " : "")}{metadata});
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var monsterType = asm.GetTypes().Single(t => t.Name == "Monster");
            var vectorMemberType = asm.GetTypes().Single(t => t.Name == "VectorMember");

            var monsterProperties = monsterType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Assert.Single(monsterProperties);

            var vectorMemberProperties = vectorMemberType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Assert.Single(vectorMemberProperties);

            Assert.Equal("Data", vectorMemberProperties[0].Name);
            Assert.Equal(typeof(TKeyType), vectorMemberProperties[0].PropertyType);

            Assert.Equal("Vector", monsterProperties[0].Name);
            Assert.Equal(typeof(IIndexedVector<,>).MakeGenericType(typeof(TKeyType), vectorMemberType), monsterProperties[0].PropertyType);
        }
    }
}
