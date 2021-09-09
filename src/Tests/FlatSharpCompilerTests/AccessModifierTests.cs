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

#if NET5_0_OR_GREATER
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
#endif

        private void Test(SetterKind setterKind, FlatBufferDeserializationOption option)
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace VirtualTests;
            table VirtualTable ({MetadataKeys.SerializerKind}:""{option}"") {{
                Default:int ({MetadataKeys.Setter}:""{setterKind}"");
                ForcedVirtual:int ({MetadataKeys.NonVirtualProperty}:""false"", {MetadataKeys.Setter}:""{setterKind}"");
                ForcedNonVirtual:int ({MetadataKeys.NonVirtualProperty}:""true"", {MetadataKeys.Setter}:""{(setterKind != SetterKind.None ? setterKind : SetterKind.Public)}"");
                Struct:VirtualStruct;
            }}

            struct VirtualStruct {{
                Default:int ({MetadataKeys.Setter}:""{setterKind}"");
                ForcedVirtual:int ({MetadataKeys.NonVirtualProperty}:""false"", {MetadataKeys.Setter}:""{setterKind}"");
                ForcedNonVirtual:int ({MetadataKeys.NonVirtualProperty}:""true"", {MetadataKeys.Setter}:""{(setterKind != SetterKind.None ? setterKind : SetterKind.Public)}"");
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            foreach (var typeName in new[] { "VirtualTests.VirtualTable", "VirtualTests.VirtualStruct" })
            {
                Type type = asm.GetType(typeName);
                Assert.True(type.IsPublic);
                var defaultProperty = type.GetProperty("Default");
                var forcedVirtualProperty = type.GetProperty("ForcedVirtual");
                var forcedNonVirtualProperty = type.GetProperty("ForcedNonVirtual");

                Assert.NotNull(defaultProperty);
                Assert.NotNull(forcedVirtualProperty);
                Assert.NotNull(forcedNonVirtualProperty);

                Assert.True(defaultProperty.GetMethod.IsPublic);
                Assert.True(forcedVirtualProperty.GetMethod.IsPublic);
                Assert.True(forcedNonVirtualProperty.GetMethod.IsPublic);

                if (setterKind == SetterKind.PublicInit ||
                    setterKind == SetterKind.ProtectedInit ||
                    setterKind == SetterKind.ProtectedInternalInit)
                {
                    Assert.Contains(defaultProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers(), x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
                    Assert.Contains(forcedVirtualProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers(), x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
                    Assert.Contains(forcedNonVirtualProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers(), x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
                }

                if (setterKind == SetterKind.None)
                {
                    Assert.Null(defaultProperty.SetMethod);
                    Assert.Null(forcedVirtualProperty.SetMethod);
                    Assert.NotNull(forcedNonVirtualProperty.SetMethod); // non-virtual can't have null setters.
                }
                else if (setterKind == SetterKind.Protected || setterKind == SetterKind.ProtectedInit)
                {
                    Assert.True(defaultProperty.SetMethod.IsFamily);
                    Assert.True(forcedVirtualProperty.SetMethod.IsFamily);
                    Assert.True(forcedNonVirtualProperty.SetMethod.IsFamily);
                }
                else if (setterKind == SetterKind.ProtectedInternal || setterKind == SetterKind.ProtectedInternalInit)
                {
                    Assert.True(defaultProperty.SetMethod.IsFamilyOrAssembly);
                    Assert.True(forcedVirtualProperty.SetMethod.IsFamilyOrAssembly);
                    Assert.True(forcedNonVirtualProperty.SetMethod.IsFamilyOrAssembly);
                }
                else if (setterKind == SetterKind.Public || setterKind == SetterKind.PublicInit)
                {
                    Assert.True(defaultProperty.SetMethod.IsPublic);
                    Assert.True(forcedVirtualProperty.SetMethod.IsPublic);
                    Assert.True(forcedNonVirtualProperty.SetMethod.IsPublic);
                }
                else
                {
                    Assert.False(true);
                }
            }
        }
    }
}
