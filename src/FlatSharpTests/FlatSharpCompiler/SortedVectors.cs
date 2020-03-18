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

            // Implements the interface.
            Assert.IsTrue(memberType.GetInterfaces().Where(x => x == typeof(IKeyedTable<string>)).Any());

            // Explicit implementation
            var prop = memberType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.Contains("IKeyedTable")).SingleOrDefault();
            Assert.IsNotNull(prop);
            Assert.IsNull(prop.SetMethod ?? prop.GetSetMethod());
            Assert.AreEqual(typeof(string), prop.PropertyType);
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

            // Implements the interface.
            Assert.IsTrue(memberType.GetInterfaces().Where(x => x == typeof(IKeyedTable<int>)).Any());

            // Explicit implementation
            var prop = memberType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name.Contains("IKeyedTable")).SingleOrDefault();
            Assert.IsNotNull(prop);
            Assert.IsNull(prop.SetMethod ?? prop.GetSetMethod());
            Assert.AreEqual(typeof(int), prop.PropertyType);
        }
    }
}
