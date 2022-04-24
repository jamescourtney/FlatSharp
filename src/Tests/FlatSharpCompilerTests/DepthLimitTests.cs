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

using FlatSharp.TypeModel;
using System.Linq;
using System.Text;

namespace FlatSharpTests.Compiler;

public class DepthLimitTests
{
    [Fact]
    public void TableReferencesSelf()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table ListNode {{ Next : ListNode; }}
        ";

        CompileAndVerify(fbs, "Foo.Bar.ListNode", true);
    }

    [Fact]
    public void TableCycle()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table A {{ Next : B; }}
            table B {{ Next : C; }}
            table C {{ Next : A; }}
        ";

        CompileAndVerify(fbs, "Foo.Bar.A", true);
    }

    [Theory]
    [InlineData(499, false)]
    [InlineData(500, true)]
    public void DeepTable_NoCycle(int depth, bool expectCycleTracking)
    {
        StringBuilder sb = new();
        sb.Append($@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table T0 {{ Value : int; }} 
        ");

        for (int i = 1; i < depth; ++i)
        {
            sb.AppendLine($"table T{i} {{ Previous : T{i - 1}; }}");
        }

        CompileAndVerify(sb.ToString(), $"Foo.Bar.T{depth - 1}", expectCycleTracking);
    }

    [Fact]
    public void TableCycleWithListVector()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table A {{ Next : B; }}
            table B {{ Next : C; }}
            table C {{ Next : [A] (fs_vector:""IList""); }}
        ";

        CompileAndVerify(fbs, "Foo.Bar.A", true);
    }

    [Fact]
    public void TableCycleWithArrayVector()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table A {{ Next : B; }}
            table B {{ Next : C; }}
            table C {{ Next : [A] (fs_vector:""Array""); }}
        ";

        CompileAndVerify(fbs, "Foo.Bar.A", true);
    }

    [Fact]
    public void TableCycleWithIndexedVector()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table A {{ Next : B; Key : string (key); }}
            table B {{ Next : C; }}
            table C {{ Next : [A] (fs_vector:""IIndexedVector""); }}
        ";

        CompileAndVerify(fbs, "Foo.Bar.A", true);
    }

    private static void CompileAndVerify(string fbs, string typeName, bool needsTracking)
    {
        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());
        Type t = asm.GetTypes().Single(x => x.FullName == typeName);
        Assert.NotNull(t);

        ITypeModel typeModel = RuntimeTypeModel.CreateFrom(t);
        Assert.Equal(needsTracking, typeModel.IsDeepEnoughToRequireDepthTracking());
    }
}
