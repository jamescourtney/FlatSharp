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
    using Xunit;

    public class StructTests
    {
        [Fact]
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

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

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
            Assert.Equal(3, dTable.defaultInt);
            dTable.foo = dFoo;

            byte[] destination = new byte[1024];

            var serializer = CompilerTestHelpers.CompilerTestSerializer.Compile(table);
            int bytesWritten = serializer.Write(destination, table);
            object parsed = serializer.Parse(destination);

            Assert.True(tableType.IsAssignableFrom(parsed.GetType()));

            dynamic dParsedTable = parsed;
            Assert.Equal(3, dParsedTable.defaultInt);

            dynamic dParsedFoo = dParsedTable.foo;
            Assert.Equal(dFoo.id, dParsedFoo.id);
            Assert.Equal(dFoo.count, dParsedFoo.count);
            Assert.Equal(dFoo.prefix, dParsedFoo.prefix);
            Assert.Equal(dFoo.length, dParsedFoo.length);
        }
    }
}
