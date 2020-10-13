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
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VirtualMemberTests
    {
        [TestMethod]
        public void DefaultVirtual() => this.VirtualTest(true);

        [TestMethod]
        public void DefaultNonVirtual() => this.VirtualTest(false);

        private void VirtualTest(bool defaultVirtual)
        {
            string schema = $@"
            namespace VirtualTests;
            table VirtualTable (nonVirtual:""{(!defaultVirtual).ToString().ToLowerInvariant()}"", PrecompiledSerializer:Lazy) {{
                Default:int;
                ForcedVirtual:int (nonVirtual:""false"");
                ForcedNonVirtual:int (nonVirtual:""true"");
                Struct:VirtualStruct;
            }}

            struct VirtualStruct (nonVirtual:""{(!defaultVirtual).ToString().ToLowerInvariant()}"") {{
                Default:int;
                ForcedVirtual:int (nonVirtual:""false"");
                ForcedNonVirtual:int (nonVirtual:""true"");
            }}

            table DefaultTable (PrecompiledSerializer:lazy) {{ Default:int; Struct:DefaultStruct; }}
            struct DefaultStruct {{ Default:int; }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            foreach (var typeName in new[] { "VirtualTests.VirtualTable", "VirtualTests.VirtualStruct" })
            {
                Type type = asm.GetType(typeName);
                Assert.IsTrue(type.IsPublic);
                var defaultProperty = type.GetProperty("Default");
                var forcedVirtualProperty = type.GetProperty("ForcedVirtual");
                var forcedNonVirtualProperty = type.GetProperty("ForcedNonVirtual");

                Assert.IsNotNull(defaultProperty);
                Assert.IsNotNull(forcedVirtualProperty);
                Assert.IsNotNull(forcedNonVirtualProperty);

                Assert.AreEqual(defaultVirtual, defaultProperty.GetMethod.IsVirtual);
                Assert.IsTrue(forcedVirtualProperty.GetMethod.IsVirtual);
                Assert.IsFalse(forcedNonVirtualProperty.GetMethod.IsVirtual);
            }

            foreach (var typeName in new[] { "VirtualTests.DefaultTable", "VirtualTests.DefaultStruct" })
            {
                Type type = asm.GetType(typeName);
                Assert.IsTrue(type.IsPublic);
                var defaultProperty = type.GetProperty("Default");

                Assert.IsNotNull(defaultProperty);
                Assert.IsTrue(defaultProperty.GetMethod.IsVirtual);
            }
        }
    }
}
