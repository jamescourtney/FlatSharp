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
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SharedStringCompilerTests
    {
        [TestMethod, Ignore]
        public void SharedStringInlineTypeTest()
        {
            string schema = $@"
            namespace SharedStringTests;
            table Table {{
                foo:SharedString;
                bar:[SharedString] ({MetadataKeys.VectorKind}:array);
                baz:[SharedString] ({MetadataKeys.VectorKindLegacy}:ilist);
            }}"; 

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type tableType = asm.GetTypes().Single(x => x.FullName == "SharedStringTests.Table");
            var property = tableType.GetProperty("foo");

            Assert.AreEqual(typeof(SharedString), property.PropertyType);
            Assert.AreEqual(typeof(SharedString[]), tableType.GetProperty("bar").PropertyType);
            Assert.AreEqual(typeof(IList<SharedString>), tableType.GetProperty("baz").PropertyType);
        }

        [TestMethod]
        public void SharedStringMetadataTypeTest()
        {
            string schema = $@"
            namespace SharedStringTests;
            table Table {{
                foo:string ({MetadataKeys.SharedString});
                bar:[string] ({MetadataKeys.VectorKind}:array, {MetadataKeys.SharedString});
                baz:[string] ({MetadataKeys.VectorKind}:ilist, {MetadataKeys.SharedStringLegacy});
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type tableType = asm.GetTypes().Single(x => x.FullName == "SharedStringTests.Table");
            var property = tableType.GetProperty("foo");

            Assert.AreEqual(typeof(SharedString), property.PropertyType);
            Assert.AreEqual(typeof(SharedString[]), tableType.GetProperty("bar").PropertyType);
            Assert.AreEqual(typeof(IList<SharedString>), tableType.GetProperty("baz").PropertyType);
        }
    }
}
