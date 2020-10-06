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
            var (prop, type, attribute) = this.CompileAndGetProperty("", "string", "deprecated");
            Assert.IsTrue(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
        }

        [TestMethod]
        public void DeprecatedSortedVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", "deprecated,sortedvector");
            Assert.IsTrue(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
        }

        [TestMethod]
        public void VectorTypeUnquoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", "VectorType:Array");
            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
            Assert.AreEqual(typeof(string[]), prop.PropertyType);
        }

        [TestMethod]
        public void VectorTypeQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[string]", "VectorType:\"IList\"");
            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector);
            Assert.AreEqual(typeof(IList<string>), prop.PropertyType);
        }

        [TestMethod]
        public void DictionaryVector()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("", "[OtherTable]", "VectorType:\"IDictionary\",SortedVector");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
            Assert.AreEqual(typeof(IDictionary<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void DictionaryVector_WithSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("PrecompiledSerializer", "[OtherTable]", "VectorType:\"IDictionary\",SortedVector");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsTrue(attribute.SortedVector);
            Assert.AreEqual(typeof(IDictionary<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void DictionaryVector_WithSerializer_NoSortedDeclaration()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("PrecompiledSerializer", "[OtherTable]", "VectorType:\"IDictionary\"");

            Assert.IsFalse(attribute.Deprecated);
            Assert.IsFalse(attribute.SortedVector); // dictionaries override this attribute.
            Assert.AreEqual(typeof(IDictionary<,>), prop.PropertyType.GetGenericTypeDefinition());
            Assert.AreEqual(typeof(string), prop.PropertyType.GetGenericArguments()[0]);
            Assert.IsTrue(prop.PropertyType.GetGenericArguments()[1].FullName.Contains("OtherTable"));
        }

        [TestMethod]
        public void PrecompiledSerializer()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("PrecompiledSerializer", "string", "");
            Assert.AreEqual(type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption, FlatBufferDeserializationOption.GreedyMutable);
        }

        [TestMethod]
        public void PrecompiledSerializerQuoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("PrecompiledSerializer:\"Lazy\"", "string", "");
            Assert.AreEqual(type.GetNestedType("GeneratedSerializer", BindingFlags.NonPublic).GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>().DeserializationOption, FlatBufferDeserializationOption.Lazy);
        }

        [TestMethod]
        public void PrecompiledSerializerUnquoted()
        {
            var (prop, type, attribute) = this.CompileAndGetProperty("PrecompiledSerializer:VectorCacheMutable", "string", "");
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

            string schema = $@"table MyTable {typeMetadata} {{ Field:{fieldType} {fieldMetadata}; }} table OtherTable {{ String:string (key); }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);
            var type = asm.GetType("MyTable");
            var propInfo = type.GetProperty("Field");
            var attribute = propInfo.GetCustomAttribute<FlatBufferItemAttribute>();

            return (propInfo, type, attribute);
        }
    }
}
