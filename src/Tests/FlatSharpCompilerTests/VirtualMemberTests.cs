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

namespace FlatSharpTests.Compiler;

public class VirtualMemberTests
{
    [Fact]
    public void DefaultVirtual() => this.VirtualTest();

    private void VirtualTest()
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace VirtualTests;
            table VirtualTable ({MetadataKeys.SerializerKind}:""Lazy"") {{
                Default:int;
                Struct:VirtualStruct;
            }}

            struct VirtualStruct {{
                Default:int;
            }}

            table DefaultTable ({MetadataKeys.SerializerKind}:""lazy"") {{ Default:int; Struct:DefaultStruct; }}
            struct DefaultStruct {{ Default:int; }}";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

        foreach (var typeName in new[] { "VirtualTests.VirtualTable", "VirtualTests.VirtualStruct" })
        {
            Type type = asm.GetType(typeName);
            Assert.True(type.IsPublic);
            var defaultProperty = type.GetProperty("Default");

            Assert.NotNull(defaultProperty);

            Assert.True(defaultProperty.GetMethod.IsVirtual);
        }

        foreach (var typeName in new[] { "VirtualTests.DefaultTable", "VirtualTests.DefaultStruct" })
        {
            Type type = asm.GetType(typeName);
            Assert.True(type.IsPublic);
            var defaultProperty = type.GetProperty("Default");

            Assert.NotNull(defaultProperty);
            Assert.True(defaultProperty.GetMethod.IsVirtual);
        }
    }
}
