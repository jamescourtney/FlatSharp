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
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Xunit;

    public class MetadataTests
    {
        [Fact]
        public void DeprecatedMetadata()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "string", MetadataKeys.Deprecated);
            Assert.True(attribute.Deprecated);
            Assert.False(attribute.SortedVector);
        }

        [Fact]
        public void DeprecatedSortedVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[OtherTable]", $"{MetadataKeys.Deprecated},{MetadataKeys.SortedVector}");
            Assert.True(attribute.Deprecated);
            Assert.True(attribute.SortedVector);
        }

        [Fact]
        public void VectorTypeQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", $"{MetadataKeys.VectorKind}:\"IList\"");
            Assert.False(attribute.Deprecated);
            Assert.False(attribute.SortedVector);
            Assert.Equal(typeof(IList<string>), prop.PropertyType);
        }

        [Fact]
        public void IIndexedVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[OtherTable]", $"{MetadataKeys.VectorKind}:\"IIndexedVector\",{MetadataKeys.SortedVector}");

            Assert.False(attribute.Deprecated);
            Assert.True(attribute.SortedVector);
            Assert.Equal(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.Equal(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.Contains("OtherTable", prop.PropertyType.GetGenericArguments()[1].FullName);
        }

        [Fact]
        public void IIndexedVectorVector_WithSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetadataKeys.SerializerKind, "[OtherTable]", $"{MetadataKeys.VectorKind}:\"IIndexedVector\",{MetadataKeys.SortedVector}");

            Assert.False(attribute.Deprecated);
            Assert.True(attribute.SortedVector);
            Assert.Equal(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.Equal(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.Contains("OtherTable", prop.PropertyType.GetGenericArguments()[1].FullName);
        }

        [Fact]
        public void IIndexedVector_WithSerializer_NoSortedDeclaration()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetadataKeys.SerializerKind, "[OtherTable]", $"{MetadataKeys.VectorKind}:\"IIndexedVector\"");

            Assert.False(attribute.Deprecated);
            Assert.False(attribute.SortedVector); // dictionaries override this attribute.
            Assert.Equal(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.Equal(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.Contains("OtherTable", prop.PropertyType.GetGenericArguments()[1].FullName);
        }

        [Fact]
        public void PrecompiledSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetadataKeys.SerializerKind, "string", "");
            Assert.Equal(FlatBufferDeserializationOption.GreedyMutable, type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption);
        }

        [Fact]
        public void PrecompiledSerializerQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty($"{MetadataKeys.SerializerKind}:\"Lazy\"", "string", "");
            Assert.Equal(FlatBufferDeserializationOption.Lazy, type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption);
        }

        private (PropertyInfo, Type, FlatBufferItemAttribute) CompileAndGetProperty(string typeMetadata, string fieldType, string fieldMetadata)
        {
            if (!string.IsNullOrEmpty(typeMetadata))
            {
                typeMetadata = $"({typeMetadata})";
            }

            if (!string.IsNullOrEmpty(fieldMetadata))
            {
                fieldMetadata = $"({fieldMetadata})";
            }

            string schema = $@"{MetadataHelpers.AllAttributes} namespace ns; table MyTable {typeMetadata} {{ Field:{fieldType} {fieldMetadata}; Fake:string; }} table OtherTable {{ String:string (key); }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var type = asm.GetType("ns.MyTable");
            var propInfo = type.GetProperty("Field");
            var attribute = propInfo.GetCustomAttribute<FlatBufferItemAttribute>();

            return (propInfo, type, attribute);
        }
    }
}
