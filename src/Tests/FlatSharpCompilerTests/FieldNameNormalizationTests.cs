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

public class FieldNameNormalizationTests
{
    [Fact]
    public void NormalizeFieldNames()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace FieldNameNormalizationTests;

            table Table {{
                item_one : int32;
                item_two : int32;
                item__three : int32;
                lowerPascalCase : int32;
                item_f : int32;
            }}

            struct Struct {{
                item_one : int32;
                item_two : int32;
                ____item__three__ : int32;
                lowerPascalCase : int32;
                item_f : int32;
            }}";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new CompilerOptions
            {
                NormalizeFieldNames = true,
            });

        foreach (string typeName in new[] { "FieldNameNormalizationTests.Table", "FieldNameNormalizationTests.Struct" })
        {
            Type type = asm.GetType(typeName);

            Assert.NotNull(type);
            Assert.NotNull(type.GetProperty("ItemOne"));
            Assert.NotNull(type.GetProperty("ItemTwo"));
            Assert.NotNull(type.GetProperty("ItemThree"));
            Assert.NotNull(type.GetProperty("ItemF"));
            Assert.NotNull(type.GetProperty("LowerPascalCase"));
        }
    }

    [Fact]
    public void PreserveFieldCasingOnField()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace FieldNameNormalizationTests;

            table Table {{
                item_one : int32;
                item_two : int32 ({MetadataKeys.PreserveFieldCasing});
            }}

            struct Struct {{
                item_one : int32;
                item_two : int32 ({MetadataKeys.PreserveFieldCasing});
            }}";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new CompilerOptions
            {
                NormalizeFieldNames = true,
            });

        foreach (string typeName in new[] { "FieldNameNormalizationTests.Table", "FieldNameNormalizationTests.Struct" })
        {
            Type type = asm.GetType(typeName);

            Assert.NotNull(type);
            Assert.NotNull(type.GetProperty("ItemOne"));
            Assert.NotNull(type.GetProperty("item_two"));
        }
    }

    [Fact]
    public void PreserveFieldCasingOnParent()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace FieldNameNormalizationTests;

            table Table ({MetadataKeys.PreserveFieldCasing}) {{
                item_one : int32 ({MetadataKeys.PreserveFieldCasing}:""false"");
                item_two : int32;
            }}

            struct Struct ({MetadataKeys.PreserveFieldCasing}) {{
                item_one : int32 ({MetadataKeys.PreserveFieldCasing}:""false"");
                item_two : int32;
            }}";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new CompilerOptions
            {
                NormalizeFieldNames = true,
            });

        foreach (string typeName in new[] { "FieldNameNormalizationTests.Table", "FieldNameNormalizationTests.Struct" })
        {
            Type type = asm.GetType(typeName);

            Assert.NotNull(type);
            Assert.NotNull(type.GetProperty("ItemOne"));
            Assert.NotNull(type.GetProperty("item_two"));
        }
    }
}
