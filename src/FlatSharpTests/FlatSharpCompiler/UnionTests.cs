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
    public class UnionTests
    {
        [TestMethod]
        public void TestUnionCustomClassGeneration()
        {
            const string Schema = @"
namespace Foobar;

table A { Value:int32; }
table B { Value:int32; }
struct C { Value:int32; }

union TestUnion { First:A, B, Foobar.C }

table D (PrecompiledSerializer) { Union:TestUnion; }

";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema, new());
            Type unionType = asm.GetType("Foobar.TestUnion");
            Type aType = asm.GetType("Foobar.A");
            Type bType = asm.GetType("Foobar.B");
            Type cType = asm.GetType("Foobar.C");
            Type dType = asm.GetType("Foobar.D");

            Assert.AreEqual(unionType, dType.GetProperty("Union").PropertyType);

            // Custom union derives from FlatBufferUnion
            Assert.IsTrue(typeof(FlatBufferUnion<,,>).MakeGenericType(aType, bType, cType).IsAssignableFrom(unionType));
            Type[] types = new[] { aType, bType, cType };
            string[] expectedAliases = new[] { "First", "B", "C" };

            // Validate nested enum
            Type nestedEnum = unionType.GetNestedTypes().Single();
            Assert.IsTrue(nestedEnum.IsEnum);
            Assert.AreEqual("ItemKind", nestedEnum.Name);
            Assert.AreEqual(typeof(byte), Enum.GetUnderlyingType(nestedEnum));
            Assert.AreEqual(types.Length, Enum.GetValues(nestedEnum).Length);
            for (int i = 0; i < types.Length; ++i)
            {
                Assert.AreEqual(expectedAliases[i], Enum.GetName(nestedEnum, (byte)(i + 1)));
            }

            // Custom union defines ctors for all input types.
            foreach (var type in types)
            {
                var ctor = unionType.GetConstructor(new[] { type });
                Assert.IsNotNull(ctor);
            }

            // Custom union defines a clone method.
            var cloneMethod = unionType.GetMethod("Clone");
            Assert.IsNotNull(cloneMethod);
            Assert.AreEqual(unionType, cloneMethod.ReturnType); // returns same type.
            Assert.IsTrue(cloneMethod.IsHideBySig); // hides base clone method.

            var parameters = cloneMethod.GetParameters();
            Assert.AreEqual(3, parameters.Length);
            for (int i = 0; i < parameters.Length; ++i)
            {
                Assert.AreEqual(typeof(Func<,>).MakeGenericType(new[] { types[i], types[i] }), parameters[i].ParameterType);
                Assert.AreEqual("clone" + expectedAliases[i], parameters[i].Name);
            }

            var switchMethods = unionType.GetMethods().Where(m => m.Name == "Switch").Where(m => m.DeclaringType == unionType).ToArray();
            Assert.AreEqual(4, switchMethods.Length);
            Assert.AreEqual(2, switchMethods.Count(x => x.ReturnType.FullName != "System.Void")); // 2 of them return something.
            Assert.IsTrue(switchMethods.All(x => x.IsHideBySig)); // all of them hide the method from the base class.

            // Validate parameter names on switch method.
            var switchMethod = switchMethods.Single(x => x.ReturnType.FullName == "System.Void" && !x.IsGenericMethod);
            parameters = switchMethod.GetParameters();
            Assert.AreEqual(types.Length + 1, parameters.Length);
            Assert.AreEqual(typeof(Action), parameters[0].ParameterType);
            Assert.AreEqual("caseDefault", parameters[0].Name);

            for (int i = 0; i < types.Length; ++i)
            {
                Assert.AreEqual(typeof(Action<>).MakeGenericType(types[i]), parameters[i + 1].ParameterType);
                Assert.AreEqual("case" + expectedAliases[i], parameters[i + 1].Name);
            }

            // Now let's use it a little bit.
            for (int i = 0; i < types.Length; ++i)
            {
                object member = Activator.CreateInstance(types[i]);
                dynamic union = Activator.CreateInstance(unionType, member);
                byte discriminator = union.Discriminator;
                object kind = union.Kind;

                byte kindValue = Convert.ToByte((object)union.Kind);

                Assert.AreEqual(i + 1, discriminator);
                Assert.AreEqual(i + 1, kindValue);
                Assert.AreEqual(expectedAliases[i], kind.ToString());
            }
        }

        [TestMethod]
        public void TestUnionWithStringGeneration()
        {
            const string Schema = @"
namespace Foobar;

table A { Value:int32; }
table B { Value:int32; }
struct C { Value:int32; }

union TestUnion { First:A, B, Foobar.C, string }

table D (PrecompiledSerializer) { Union:TestUnion; }

";
            // Simply ensure that the union is generated as FlatBufferUnion and no custom class is created.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema, new());
            Type unionType = asm.GetType("Foobar.TestUnion");

            Type dType = asm.GetType("Foobar.D");

            Assert.AreEqual(unionType, dType.GetProperty("Union").PropertyType);
        }

        [TestMethod]
        public void TestUnionWithAliasedStringGeneration()
        {
            const string Schema = @"
namespace Foobar;

table A { Value:int32; }
table B { Value:int32; }
struct C { Value:int32; }

union TestUnion { First:A, B, Foobar.C, StringAlias:string }

table D (PrecompiledSerializer) { Union:TestUnion; }

";
            // Simply ensure that the union is generated as FlatBufferUnion and no custom class is created.
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(Schema, new());
            Type unionType = asm.GetType("Foobar.TestUnion");
            Type dType = asm.GetType("Foobar.D");

            Assert.AreEqual(unionType, dType.GetProperty("Union").PropertyType);
        }
    }
}
