/*
 * Copyright 2021 James Courtney
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

namespace FlatSharpTests.Compiler;

public class IncludeTests
{
    [Fact]
    public void IncludeTest()
    {
        var schemaA = $@"
            include ""B.fbs"";

            namespace Foobar;

            table A {{ TableB:B; }}
        ";

        var schemaB = $@"
            namespace Foobar;

            table B {{ Value:int32; }}
        ";

        var schemas = new[]
        {
            ("B.fbs", schemaB),
            ("A.fbs", schemaA),
        };

        var assemblies = FlatSharpCompiler.CompileAndLoadAssembly(schemas, new());

        UsingAssemblies(assemblies, () =>
        {
            Type aType = assemblies.GetType("Foobar.A");
            Type bType = assemblies.GetType("Foobar.B");

            PropertyInfo tableB = aType.GetProperty("TableB");

            Assert.Equal(bType, tableB.PropertyType);

            var compiled = CompilerTestHelpers.CompilerTestSerializer.Compile(aType);

            byte[] data = new byte[100];
            compiled.Write(data, Activator.CreateInstance(aType));
            compiled.Parse(data);
        });
    }

    [Fact]
    public void IncludeTest_IncludePaths()
    {
        var schemaA = $@"
            include ""Foo\\B.fbs"";

            namespace Foobar;

            table A {{ TableB:B; }}
        ";

        var schemaB = $@"
            include ""C.fbs"";
            include ""D.fbs"";

            namespace Foobar;

            table B {{ TableC:C; TableD:D; }}
        ";

        var schemaC = $@"
            namespace Foobar;

            table C {{ Value:int32; }}
        ";

        var schemaD = $@"
            namespace Foobar;

            table D {{ Value:int32; }}
        ";

        var schemas = new[]
        {
            (@"Baz\D.fbs", schemaD),
            (@"Bar\C.fbs", schemaC),
            (@"Foo\B.fbs", schemaB),
            (@"A.fbs", schemaA),
        };

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schemas, new() { IncludesDirectory = "Bar;Baz" });

        UsingAssemblies(asm, () =>
        {
            Type aType = asm.GetType("Foobar.A");
            Type bType = asm.GetType("Foobar.B");
            Type cType = asm.GetType("Foobar.C");
            Type dType = asm.GetType("Foobar.D");

            PropertyInfo tableB = aType.GetProperty("TableB");
            PropertyInfo tableC = bType.GetProperty("TableC");
            PropertyInfo tableD = bType.GetProperty("TableD");

            Assert.Equal(bType, tableB.PropertyType);
            Assert.Equal(cType, tableC.PropertyType);
            Assert.Equal(dType, tableD.PropertyType);

            var compiled = CompilerTestHelpers.CompilerTestSerializer.Compile(aType);

            byte[] data = new byte[100];
            compiled.Write(data, Activator.CreateInstance(aType));
            compiled.Parse(data);
        });
    }

    private void UsingAssemblies(Assembly asm, Action action)
    {
        ResolveEventHandler handler = (s, e) => asm.GetName().Name == e.Name ? asm : null;

        try
        {
            AppDomain.CurrentDomain.AssemblyResolve += handler;
            action.Invoke();
        }
        finally
        {
            AppDomain.CurrentDomain.AssemblyResolve -= handler;
        }
    }
}
