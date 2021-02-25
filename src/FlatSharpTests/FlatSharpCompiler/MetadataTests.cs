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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MetadataTests
    {
        [TestMethod]
        public void DeprecatedMetadata()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "string", MetdataKeys.Deprecated);
            Assert.IsTrue(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
        }

        [TestMethod]
        public void DeprecatedSortedVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[OtherTable]", $"{MetdataKeys.Deprecated},{MetdataKeys.SortedVector}");
            Assert.IsTrue(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
        }

        [TestMethod]
        public void VectorTypeUnquoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", $"{MetdataKeys.VectorKind}:Array");
            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
            Assert.AreEqual(typeof(string[]), prop.PropertyType);
        }

        [TestMethod]
        public void VectorTypeQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", $"{MetdataKeys.VectorKind}:\"IList\"");
            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
            Assert.AreEqual(typeof(IList<string>), prop.PropertyType);
        }

        [TestMethod]
        public void IIndexedVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[OtherTable]", $"{MetdataKeys.VectorKind}:\"IIndexedVector\",{MetdataKeys.SortedVector}");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
            Assert.AreEqual(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void IIndexedVectorVector_WithSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetdataKeys.SerializerKind, "[OtherTable]", $"{MetdataKeys.VectorKind}:\"IIndexedVector\",{MetdataKeys.SortedVector}");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
            Assert.AreEqual(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void IIndexedVector_WithSerializer_NoSortedDeclaration()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetdataKeys.SerializerKind, "[OtherTable]", $"{MetdataKeys.VectorKind}:\"IIndexedVector\"");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector); // dictionaries override this attribute.
            Assert.AreEqual(typeof(IIndexedVector<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void PrecompiledSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty(MetdataKeys.SerializerKind, "string", "");
            Assert.AreEqual(type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption, FlatBufferDeserializationOption.GreedyMutable);
        }

        [TestMethod]
        public void PrecompiledSerializerQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty($"{MetdataKeys.SerializerKind}:\"Lazy\"", "string", "");
            Assert.AreEqual(type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption, FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void PrecompiledSerializerUnquoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty($"{MetdataKeys.SerializerKind}:VectorCacheMutable", "string", "");
            Assert.AreEqual(type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption, FlatBufferDeserializationOption.VectorCacheMutable);
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

            string schema = $@"table MyTable {typeMetadata} {{ Field:{fieldType} {fieldMetadata}; Fake:string; }} table OtherTable {{ String:string (key); }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var type = asm.GetType("MyTable");
            var propInfo = type.GetProperty("Field");
            var attribute = propInfo.GetCustomAttribute<FlatBufferItemAttribute>();

            return (propInfo, type, attribute);
        }
    }
}
