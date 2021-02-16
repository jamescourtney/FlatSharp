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
    public class ObsoleteCtorTests
    {
        [TestMethod]
        public void ObsoleteConstructorImplicit()
        {
            string schema = "namespace Foo; table BaseTable (obsoleteDefaultConstructor) { Int:int; }";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type baseTableType = asm.GetTypes().Single(x => x.Name == "BaseTable");

            var constructor = baseTableType.GetConstructor(new Type[0]);
            Assert.IsNotNull(constructor.GetCustomAttribute<ObsoleteAttribute>());
        }

        [TestMethod]
        public void ObsoleteConstructorExplicitObsolete()
        {
            string schema = "namespace Foo; table BaseTable (obsoleteDefaultConstructor:\"true\") { Int:int; }";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type baseTableType = asm.GetTypes().Single(x => x.Name == "BaseTable");

            var constructor = baseTableType.GetConstructor(new Type[0]);
            Assert.IsNotNull(constructor.GetCustomAttribute<ObsoleteAttribute>());
        }

        [TestMethod]
        public void ObsoleteConstructorNone()
        {
            string schema = "namespace Foo; table BaseTable { Int:int; }";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type baseTableType = asm.GetTypes().Single(x => x.Name == "BaseTable");

            var constructor = baseTableType.GetConstructor(new Type[0]);
            Assert.IsNull(constructor.GetCustomAttribute<ObsoleteAttribute>());
        }

        [TestMethod]
        public void ObsoleteConstructorExplicitFalse()
        {
            string schema = "namespace Foo; table BaseTable (obsoleteDefaultConstructor:\"false\") { Int:int; }";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type baseTableType = asm.GetTypes().Single(x => x.Name == "BaseTable");

            var constructor = baseTableType.GetConstructor(new Type[0]);
            Assert.IsNull(constructor.GetCustomAttribute<ObsoleteAttribute>());
        }
    }
}
