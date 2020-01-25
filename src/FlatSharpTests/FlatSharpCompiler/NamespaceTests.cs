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
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema);

            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.TableInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.EnumInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.NamespaceB.StructInNestedNS"));
            Assert.IsNotNull(asm.GetType("NamespaceA.TableInFirstNS"));
            Assert.IsNotNull(asm.GetType("NamespaceC.TableInC"));
            Assert.IsNotNull(asm.GetType("NamespaceA.SecondTableInA"));
        }
    }
}
