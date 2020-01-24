namespace FlatSharpTests.Compiler
{
    using System;
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StructTests
    {
        [TestMethod]
        public void BasicStructTest()
        {
            string schema = @"
            namespace StructTests;
            table Table {
                foo:Foo;
                defaultInt:int32 = 3;
            }

            struct Foo {
              id:ulong;
              count:short;
              prefix:byte;
              length:uint;
            }

            struct Bar {
              parent:Foo;
              time:int;
              ratio:float;
              size:ushort;
            }"; 

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            Type tableType = asm.GetTypes().Single(x => x.FullName == "StructTests.Table");
            object table = Activator.CreateInstance(tableType);

            Type fooType = asm.GetTypes().Single(x => x.FullName == "StructTests.Foo");
            object foo = Activator.CreateInstance(fooType);

            dynamic dFoo = foo;
            dFoo.id = 123ul;
            dFoo.count = 4;
            dFoo.prefix = 1;
            dFoo.length = 52;

            dynamic dTable = table;
            Assert.AreEqual(3, dTable.defaultInt);
            dTable.foo = dFoo;

            byte[] destination = new byte[1024];

            int bytesWritten = CompilerTestHelpers.CompilerTestSerializer.ReflectionSerialize(table, destination);
            object parsed = CompilerTestHelpers.CompilerTestSerializer.ReflectionParse(tableType, destination);

            Assert.IsTrue(tableType.IsAssignableFrom(parsed.GetType()));

            dynamic dParsedTable = parsed;
            Assert.AreEqual(3, dParsedTable.defaultInt);

            dynamic dParsedFoo = dParsedTable.foo;
            Assert.AreEqual(dFoo.id, dParsedFoo.id);
            Assert.AreEqual(dFoo.count, dParsedFoo.count);
            Assert.AreEqual(dFoo.prefix, dParsedFoo.prefix);
            Assert.AreEqual(dFoo.length, dParsedFoo.length);
        }
    }
}
