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

    public class UnionTests
    {
        [Fact]
        public void TestUnionCustomClassGeneration()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace Foobar;

table A {{ Value:int32; }}
table B {{ Value:int32; }}
struct C {{ Value:int32; }}

union TestUnion {{ First:A, B, Foobar.C }}

table D ({MetadataKeys.SerializerKind}) {{ Union:TestUnion; }}

";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type unionType = asm.GetType("Foobar.TestUnion");
            Type aType = asm.GetType("Foobar.A");
            Type bType = asm.GetType("Foobar.B");
            Type cType = asm.GetType("Foobar.C");
            Type dType = asm.GetType("Foobar.D");

            Assert.Equal(unionType, Nullable.GetUnderlyingType(dType.GetProperty("Union").PropertyType));

            // Custom union derives from FlatBufferUnion
            Assert.True(typeof(IFlatBufferUnion<,,>).MakeGenericType(aType, bType, cType).IsAssignableFrom(unionType));
            Type[] types = new[] { aType, bType, cType };
            string[] expectedAliases = new[] { "First", "B", "Foobar_C" };

            // Validate nested enum
            Type nestedEnum = unionType.GetNestedTypes().Single();
            Assert.True(nestedEnum.IsEnum);
            Assert.Equal("ItemKind", nestedEnum.Name);
            Assert.Equal(typeof(byte), Enum.GetUnderlyingType(nestedEnum));
            Assert.Equal(types.Length + 1, Enum.GetValues(nestedEnum).Length);

            Assert.Equal("NONE", Enum.GetName(nestedEnum, (byte)0));
            for (int i = 0; i < types.Length; ++i)
            {
                Assert.Equal(expectedAliases[i], Enum.GetName(nestedEnum, (byte)(i + 1)));
            }

            // Custom union defines ctors for all input types.
            foreach (var type in types)
            {
                var ctor = unionType.GetConstructor(new[] { type });
                Assert.NotNull(ctor);
            }

            var switchMethods = unionType.GetMethods().Where(m => m.Name == "Switch").Where(m => m.DeclaringType == unionType).ToArray();
            Assert.Equal(4, switchMethods.Length);
            Assert.Equal(2, switchMethods.Count(x => x.ReturnType.FullName != "System.Void")); // 2 of them return something.
            Assert.True(switchMethods.All(x => x.IsHideBySig)); // all of them hide the method from the base class.

            // Validate parameter names on switch method.
            var switchMethod = switchMethods.Single(x => x.ReturnType.FullName == "System.Void" && !x.IsGenericMethod);
            var parameters = switchMethod.GetParameters();
            Assert.Equal(types.Length + 1, parameters.Length);
            Assert.Equal(typeof(Action), parameters[0].ParameterType);
            Assert.Equal("caseDefault", parameters[0].Name);

            for (int i = 0; i < types.Length; ++i)
            {
                Assert.Equal(typeof(Action<>).MakeGenericType(types[i]), parameters[i + 1].ParameterType);
                Assert.Equal("case" + expectedAliases[i], parameters[i + 1].Name);
            }

            // Now let's use it a little bit.
            for (int i = 0; i < types.Length; ++i)
            {
                object member = Activator.CreateInstance(types[i]);
                dynamic union = Activator.CreateInstance(unionType, member);
                byte discriminator = union.Discriminator;
                object kind = union.Kind;

                byte kindValue = Convert.ToByte((object)union.Kind);

                Assert.Equal(i + 1, discriminator);
                Assert.Equal(i + 1, kindValue);
                Assert.Equal(expectedAliases[i], kind.ToString());
            }
        }
    }
}
