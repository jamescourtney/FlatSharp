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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SortedVectors
    {
        [TestMethod]
        public void SortedVector_StringKey()
        {
            string schema = @"
table Monster (PrecompiledSerializer) {
  Vector:[VectorMember] (VectorType:IReadOnlyList, SortedVector);
}

table VectorMember {
    Key:string (key);
    Data:int32;
}
"; 
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("VectorMember");

            var prop = memberType.GetProperty("Key");
            Assert.IsNotNull(prop);
            Assert.AreEqual(typeof(string), prop.PropertyType);
            Assert.IsTrue(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);

            prop = memberType.GetProperty("Data");
            Assert.IsNotNull(prop);
            Assert.AreEqual(typeof(int), prop.PropertyType);
            Assert.IsFalse(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
        }

        [TestMethod]
        public void SortedVector_IntKey()
        {
            string schema = @"
table Monster (PrecompiledSerializer) {
  Vector:[VectorMember] (VectorType:IReadOnlyList, SortedVector);
}

table VectorMember {
    Data:string;
    Key:int32 (Key);
}
";
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("VectorMember");

            var prop = memberType.GetProperty("Key");
            Assert.IsNotNull(prop);
            Assert.AreEqual(typeof(int), prop.PropertyType);
            Assert.IsTrue(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);

            prop = memberType.GetProperty("Data");
            Assert.IsNotNull(prop);
            Assert.AreEqual(typeof(string), prop.PropertyType);
            Assert.IsFalse(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
        }

        /// <summary>
        /// Tests that the compiler doesn't really care about what junk you write when a serializer
        /// is not present.
        /// </summary>
        [TestMethod]
        public void SortedVector_InvalidKeys_NoSerializer()
        {
            string schema = @"
enum TestEnum : ubyte { One = 1, Two = 2 }

union TestUnion { VectorMember }

table Monster {
  Vector:[VectorMember] (VectorType:IReadOnlyList, SortedVector, Key);
}
struct FakeStruct { Data:int32 (key); }

table VectorMember {
    Data:string (SortedVector, Key);
    Key:int32 (SortedVector, Key);
    Enum:TestEnum (SortedVector, Key);
    Struct:FakeStruct (SortedVector, key);
    Union:TestUnion (SortedVector, Key);
    StructVector:[FakeStruct] (SortedVector, key);
}

";
            // We are just verifying that the schema can be generated and compiled. The testing of the logic portion of the sorted vector code takes
            // place elsewhere.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            var memberType = asm.GetType("VectorMember");
            foreach (var prop in memberType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Assert.IsTrue(prop.GetCustomAttribute<FlatBufferItemAttribute>().Key);
                Assert.IsTrue(prop.GetCustomAttribute<FlatBufferItemAttribute>().SortedVector);
            }
        }

        [TestMethod]
        public void SortedVector_InvalidKeys_WithSerializer()
        {
            string schema = @"
enum TestEnum : ubyte { One = 1, Two = 2 }

union TestUnion { VectorMember }

table Monster (PrecompiledSerializer) {
  Vector:[VectorMember] (VectorType:IReadOnlyList, SortedVector, Key);
}
struct FakeStruct { Data:int32 (key); }

table VectorMember {
    Data:string (SortedVector, Key);
    Key:int32 (SortedVector, Key);
    Enum:TestEnum (SortedVector, Key);
    Struct:FakeStruct (SortedVector, key);
    Union:TestUnion (SortedVector, Key);
    StructVector:[FakeStruct] (SortedVector, key);
}

";
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        }

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_SharedString() => this.SortedVector_IndexedVector_KeyTypesCorrect<SharedString>("string", "SharedString");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_String() => this.SortedVector_IndexedVector_KeyTypesCorrect<string>("string");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Bool() => this.SortedVector_IndexedVector_KeyTypesCorrect<bool>("bool");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Byte() => this.SortedVector_IndexedVector_KeyTypesCorrect<byte>("ubyte");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_SByte() => this.SortedVector_IndexedVector_KeyTypesCorrect<sbyte>("byte");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_UShort() => this.SortedVector_IndexedVector_KeyTypesCorrect<ushort>("ushort");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Short() => this.SortedVector_IndexedVector_KeyTypesCorrect<short>("short");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_UInt() => this.SortedVector_IndexedVector_KeyTypesCorrect<uint>("uint");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Int() => this.SortedVector_IndexedVector_KeyTypesCorrect<int>("int");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_ULong() => this.SortedVector_IndexedVector_KeyTypesCorrect<ulong>("ulong");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Long() => this.SortedVector_IndexedVector_KeyTypesCorrect<long>("long");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Float() => this.SortedVector_IndexedVector_KeyTypesCorrect<float>("float");

        [TestMethod]
        public void SortedVector_IndexedVector_KeyTypesCorrect_Double() => this.SortedVector_IndexedVector_KeyTypesCorrect<double>("double");

        [TestMethod]
        public void SortedVector_IndexedVector_NoKey()
        {
            string schema = $@"
table Monster (PrecompiledSerializer) {{
  Vector:[VectorMember] (VectorType:IIndexedVector);
}}
table VectorMember {{
    Data:string;
}}";
            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
        }

        private void SortedVector_IndexedVector_KeyTypesCorrect<TKeyType>(string type, string metadata = null)
        {
            string schema = $@"
table Monster (PrecompiledSerializer) {{
  Vector:[VectorMember] (VectorType:IIndexedVector);
}}
table VectorMember {{
    Data:{type} (Key {(!string.IsNullOrEmpty(metadata) ? ", " : "")}{metadata});
}}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var monsterType = asm.GetTypes().Single(t => t.Name == "Monster");
            var vectorMemberType = asm.GetTypes().Single(t => t.Name == "VectorMember");

            var monsterProperties = monsterType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Assert.AreEqual(1, monsterProperties.Count);

            var vectorMemberProperties = vectorMemberType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Assert.AreEqual(1, vectorMemberProperties.Count);

            Assert.AreEqual("Data", vectorMemberProperties[0].Name);
            Assert.AreEqual(typeof(TKeyType), vectorMemberProperties[0].PropertyType);

            Assert.AreEqual("Vector", monsterProperties[0].Name);
            Assert.AreEqual(typeof(IIndexedVector<,>).MakeGenericType(typeof(TKeyType), vectorMemberType), monsterProperties[0].PropertyType);
        }
    }
}
