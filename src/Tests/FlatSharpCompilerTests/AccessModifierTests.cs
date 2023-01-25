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

public class AccessModifierTests
{
    [Fact]
    public void TestAccessModifierCombinations_Setters()
    {
        foreach (SetterKind setterKind in new[] { SetterKind.None, SetterKind.Public, SetterKind.Protected, SetterKind.ProtectedInternal })
        {
            foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                this.Test(setterKind, option);
            }
        }
    }

    [Fact]
    public void TestAccessModifierCombinations_Init()
    {
        foreach (SetterKind setterKind in new[] { SetterKind.PublicInit, SetterKind.ProtectedInit, SetterKind.ProtectedInternalInit })
        {
            foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                this.Test(setterKind, option);
            }
        }
    }

    private void Test(SetterKind setterKind, FlatBufferDeserializationOption option)
    {
        string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace VirtualTests;
            table VirtualTable ({MetadataKeys.SerializerKind}:""{option}"") {{
                Default:int ({MetadataKeys.Setter}:""{setterKind}"");
                Struct:VirtualStruct;
            }}

            struct VirtualStruct {{
                Default:int ({MetadataKeys.Setter}:""{setterKind}"");
            }}";

        bool isInit = setterKind == SetterKind.PublicInit
                   || setterKind == SetterKind.ProtectedInit
                   || setterKind == SetterKind.ProtectedInternalInit;

        Assembly asm;
#if NET5_0_OR_GREATER
        asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
#else
        try
        {
            asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
        }
        catch (InvalidFbsFileException ex) when (isInit)
        {
            // 3.1 should bail.
            Assert.Contains(
                $"The attribute '{MetadataKeys.Setter}' value {setterKind} is not supported in the current .NET Runtime. It requires .NET 5 or later.",
                ex.Message);
            return;
        }
#endif

        foreach (var typeName in new[] { "VirtualTests.VirtualTable", "VirtualTests.VirtualStruct" })
        {
            Type type = asm.GetType(typeName);
            Assert.True(type.IsPublic);
            var defaultProperty = type.GetProperty("Default");

            Assert.NotNull(defaultProperty);
            Assert.True(defaultProperty.GetMethod.IsPublic);
            Assert.True(defaultProperty.GetMethod.IsVirtual);

            if (isInit)
            {
                Assert.Contains(defaultProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers(), x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
            }

            if (setterKind == SetterKind.None)
            {
                Assert.Null(defaultProperty.SetMethod);
            }
            else if (setterKind == SetterKind.Protected || setterKind == SetterKind.ProtectedInit)
            {
                Assert.True(defaultProperty.SetMethod.IsFamily);
            }
            else if (setterKind == SetterKind.ProtectedInternal || setterKind == SetterKind.ProtectedInternalInit)
            {
                Assert.True(defaultProperty.SetMethod.IsFamilyOrAssembly);
            }
            else if (setterKind == SetterKind.Public || setterKind == SetterKind.PublicInit)
            {
                Assert.True(defaultProperty.SetMethod.IsPublic);
            }
            else
            {
                Assert.False(true);
            }
        }
    }
}
