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
}

namespace NamespaceA;

table SecondTableInA {
    refer_to_c:NamespaceC.TableInC;
}";

        [TestMethod]
        public void TestMultipleNamespaces()
        {
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema);
        }
    }
}
