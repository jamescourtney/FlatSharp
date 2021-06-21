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

        [TestMethod]
        public void TestUpwardsResolution()
        {
            string schema = $@"
                namespace A; table TA {{ V : int; }}

                namespace A.B; table TB {{ V : TA; }}

                // This test verifies that B.TB resolves as A.B.TB and not A.B.C.B.TB
                namespace A.B.C; table TC {{ V1 : B.TB; V2 : TB; V3 : TA; V4 : A.B.C.B.TB; }}

                namespace A.B.C.B; table TB {{ V : int; }}
";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type tc = asm.GetType("A.B.C.TC");

            PropertyInfo prop = tc.GetProperty("V1");
            Assert.AreEqual(asm.GetType("A.B.TB"), prop.PropertyType);

            prop = tc.GetProperty("V2");
            Assert.AreEqual(asm.GetType("A.B.TB"), prop.PropertyType);

            prop = tc.GetProperty("V3");
            Assert.AreEqual(asm.GetType("A.TA"), prop.PropertyType);

            prop = tc.GetProperty("V4");
            Assert.AreEqual(asm.GetType("A.B.C.B.TB"), prop.PropertyType);
        }

        [TestMethod]
        public void TestCousinResolution()
        {
            string schema = $@"
                namespace A; table TA {{ V : int; }}

                namespace A.B; table TB {{ V : TA; }}

                namespace A.B.C; table TABC {{ V : int; }}

                namespace A.C; table TC {{ V1 : B.TB; V2 : B.C.TABC; }}
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type tc = asm.GetType("A.C.TC");

            PropertyInfo prop = tc.GetProperty("V1");
            Assert.AreEqual(asm.GetType("A.B.TB"), prop.PropertyType);

            prop = tc.GetProperty("V2");
            Assert.AreEqual(asm.GetType("A.B.C.TABC"), prop.PropertyType);
        }

        [TestMethod]
        public void TestSameName()
        {
            string schema = $@"
                namespace A; 
                table TA 
                {{
                    //SelfRef1: A.TA;
                    //SelfRef2 : TA;
                    //ChildRef : A.A.TA;
                    //GrandChildRef : A.A.A.TA;
                }}

                namespace A.A; 
                table TA 
                {{
                    SelfRef: TA;
                    ChildRef1 : A.TA;
                    ChildRef2 : A.A.TA;
                    ChildRef3 : A.A.A.TA;
                }}

                namespace A.A.A; 
                table TA  
                {{
                    //SelfRef1: A.TA;
                    //SelfRef2 : TA;
                    //SelfRef3 : A.A.TA;
                    //SelfRef4 : A.A.A.TA;
                }}
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());


            void TestProperty(Type type, string propertyName, string expectedTypeName)
            {
                PropertyInfo? pi = type.GetProperty(propertyName);
                Type? expectedType = asm.GetType(expectedTypeName);

                Assert.IsNotNull(pi);
                Assert.IsNotNull(expectedType);

                Assert.AreEqual(expectedType, pi.PropertyType);
            }

            {
                //Type type = asm.GetType("A.TA");
                //TestProperty(type, "SelfRef1", "A.TA");
                //TestProperty(type, "SelfRef2", "A.TA");
                //TestProperty(type, "ChildRef", "A.A.TA");
                //TestProperty(type, "GrandChildRef", "A.A.A.TA");
            }

            {
                Type type = asm.GetType("A.A.TA");
                TestProperty(type, "SelfRef", "A.A.TA");
                TestProperty(type, "ChildRef1", "A.A.A.TA");
                TestProperty(type, "ChildRef2", "A.A.A.TA");
                TestProperty(type, "ChildRef3", "A.A.A.TA");
            }
        }
    }
}
