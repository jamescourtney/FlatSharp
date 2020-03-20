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
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

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
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

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
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema));
        }
    }
}
