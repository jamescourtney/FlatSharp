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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccessModifierTests
    {
        [TestMethod]
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

#if NET5_0
        [TestMethod]
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
            namespace VirtualTests;
            table VirtualTable (PrecompiledSerializer:""{option}"") {{
                Default:int (setter:""{setterKind}"");
                ForcedVirtual:int (nonVirtual:""false"", setter:""{setterKind}"");
                ForcedNonVirtual:int (nonVirtual:""true"", setter:""{(setterKind != SetterKind.None ? setterKind : SetterKind.Public)}"");
                Struct:VirtualStruct;
            }}

            struct VirtualStruct {{
                Default:int (setter:""{setterKind}"");
                ForcedVirtual:int (nonVirtual:""false"", setter:""{setterKind}"");
                ForcedNonVirtual:int (nonVirtual:""true"", setter:""{(setterKind != SetterKind.None ? setterKind : SetterKind.Public)}"");
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            foreach (var typeName in new[] { "VirtualTests.VirtualTable", "VirtualTests.VirtualStruct" })
            {
                Type type = asm.GetType(typeName);
                Assert.IsTrue(type.IsPublic);
                var defaultProperty = type.GetProperty("Default");
                var forcedVirtualProperty = type.GetProperty("ForcedVirtual");
                var forcedNonVirtualProperty = type.GetProperty("ForcedNonVirtual");

                Assert.IsNotNull(defaultProperty);
                Assert.IsNotNull(forcedVirtualProperty);
                Assert.IsNotNull(forcedNonVirtualProperty);

                Assert.IsTrue(defaultProperty.GetMethod.IsPublic);
                Assert.IsTrue(forcedVirtualProperty.GetMethod.IsPublic);
                Assert.IsTrue(forcedNonVirtualProperty.GetMethod.IsPublic);

                if (setterKind == SetterKind.PublicInit ||
                    setterKind == SetterKind.ProtectedInit ||
                    setterKind == SetterKind.ProtectedInternalInit)
                {
                    Assert.IsNotNull(defaultProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit"));
                    Assert.IsNotNull(forcedVirtualProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit"));
                    Assert.IsNotNull(forcedNonVirtualProperty.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit"));
                }

                if (setterKind == SetterKind.None)
                {
                    Assert.IsNull(defaultProperty.SetMethod);
                    Assert.IsNull(forcedVirtualProperty.SetMethod);
                    Assert.IsNotNull(forcedNonVirtualProperty.SetMethod); // non-virtual can't have null setters.
                }
                else if (setterKind == SetterKind.Protected || setterKind == SetterKind.ProtectedInit)
                {
                    Assert.IsTrue(defaultProperty.SetMethod.IsFamily);
                    Assert.IsTrue(forcedVirtualProperty.SetMethod.IsFamily);
                    Assert.IsTrue(forcedNonVirtualProperty.SetMethod.IsFamily);
                }
                else if (setterKind == SetterKind.ProtectedInternal || setterKind == SetterKind.ProtectedInternalInit)
                {
                    Assert.IsTrue(defaultProperty.SetMethod.IsFamilyOrAssembly);
                    Assert.IsTrue(forcedVirtualProperty.SetMethod.IsFamilyOrAssembly);
                    Assert.IsTrue(forcedNonVirtualProperty.SetMethod.IsFamilyOrAssembly);
                }
                else if (setterKind == SetterKind.Public || setterKind == SetterKind.PublicInit)
                {
                    Assert.IsTrue(defaultProperty.SetMethod.IsPublic);
                    Assert.IsTrue(forcedVirtualProperty.SetMethod.IsPublic);
                    Assert.IsTrue(forcedNonVirtualProperty.SetMethod.IsPublic);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
