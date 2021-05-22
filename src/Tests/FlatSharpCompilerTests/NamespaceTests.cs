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
    public class NamespaceTests
    {
        // https://github.com/google/flatbuffers/tree/master/tests/namespace_test
        private const string Schema = @"
namespace NamespaceA.NamespaceB;

table TableInNestedNS
{
    foo:int;
}

enum EnumInNestedNS:byte
{
	A, B, C
}

struct StructInNestedNS
{
    a:int;
	b:int;
}

namespace NamespaceA;

table TableInFirstNS
{
    foo_table:NamespaceB.TableInNestedNS;
	foo_enum:NamespaceB.EnumInNestedNS;
	foo_struct:NamespaceB.StructInNestedNS;
}

// Test switching namespaces inside a file.
namespace NamespaceC;

table TableInC {
    refer_to_a1:NamespaceA.TableInFirstNS;
    refer_to_a2:NamespaceA.SecondTableInA;
    refer_to_enum:NamespaceA.NamespaceB.EnumInNestedNS = A;
}

namespace NamespaceA;

table SecondTableInA {
    refer_to_c:NamespaceC.TableInC;
}";

        [TestMethod]
        public void TestMultipleNamespaces()
        {
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema, new());

            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.TableInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.EnumInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.StructInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.TableInFirstNS"));
            Assert.IsNotNull(asm.GetType("NamespaceC.TableInC"));
            Assert.IsNotNull(asm.GetType("NamespaceA.SecondTableInA"));
        }

        [TestMethod]
        public void TestCrossNamespaceUnion()
        {
            string schema = @"
namespace A;

union Either { A1, A2 }

table A1
{
	Foobar:string;
	Test:int64;
	Memory:[ubyte];
	List:[int];
}

table A2
{
   Name:string;
   Age:int;
}

table A3
{
    Union:Either;
    Union2:A.Either;
}

namespace B;

table T3
{
	Union:A.Either;
}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
        }
    }
}
