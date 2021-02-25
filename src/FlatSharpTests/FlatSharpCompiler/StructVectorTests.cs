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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StructVectorTests
    {
        [TestMethod]
        public void StructVector_Byte_NegativeLength() 
            => Assert.ThrowsException<InvalidFbsFileException>(() => this.RunTest<byte>("ubyte", -3, FlatBufferDeserializationOption.Greedy));

        [TestMethod]
        public void StructVector_Byte_ZeroLength() => this.RunTest<byte>("ubyte", 0, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_Byte() => this.RunTest<byte>("ubyte", 1, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_SByte() => this.RunTest<sbyte>("byte", 2, FlatBufferDeserializationOption.GreedyMutable);

        [TestMethod]
        public void StructVector_Bool() => this.RunTest<bool>("bool", 3, FlatBufferDeserializationOption.VectorCache);

        [TestMethod]
        public void StructVector_UShort() => this.RunTest<ushort>("ushort", 4, FlatBufferDeserializationOption.VectorCacheMutable);

        [TestMethod]
        public void StructVector_Short() => this.RunTest<short>("short", 5, FlatBufferDeserializationOption.PropertyCache);

        [TestMethod]
        public void StructVector_UInt() => this.RunTest<uint>("uint", 6, FlatBufferDeserializationOption.Lazy);

        [TestMethod]
        public void StructVector_Int() => this.RunTest<int>("int", 7, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_ULong() => this.RunTest<ulong>("ulong", 8, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_Long() => this.RunTest<long>("long", 9, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_Float() => this.RunTest<float>("float", 10, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_Double() => this.RunTest<double>("double", 11, FlatBufferDeserializationOption.Greedy);

        [TestMethod]
        public void StructVector_NestedStruct()
        {
            int length = 7;

            string schema = $@"
            namespace StructVectorTests;

            table Table ({MetdataKeys.SerializerKind}) {{
                foo:Foo;
            }}
            struct Bar {{ A:ubyte; B:ulong; }}
            struct Foo {{
              V:[Bar:{length}] ({MetdataKeys.NonVirtualProperty});
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new());

            Type tableType = asm.GetType("StructVectorTests.Table");
            Type fooType = asm.GetType("StructVectorTests.Foo");

            Assert.IsFalse(fooType.GetProperty("__flatsharp__V_0").GetMethod.IsVirtual);
            Assert.IsFalse(fooType.GetProperty("__flatsharp__V_0").SetMethod.IsVirtual);

            Type barType = asm.GetType("StructVectorTests.Bar");

            dynamic table = Activator.CreateInstance(tableType);
            dynamic foo = Activator.CreateInstance(fooType);
            table.foo = foo;

            Assert.AreEqual(length, foo.V.Count);

            for (int i = 0; i < length; ++i)
            {
                dynamic bar = Activator.CreateInstance(barType);
                bar.A = (byte)i;
                bar.B = (ulong)i;

                foo.V[i] = bar;
            }

            byte[] data = new byte[1024];
            var serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy) { EnableAppDomainInterceptOnAssemblyLoad = true });

            serializer.ReflectionSerialize((object)table, data);
            dynamic parsed = serializer.ReflectionParse(tableType, data);
            dynamic copy = Activator.CreateInstance(tableType, (object)parsed);

            Assert.AreEqual(length, parsed.foo.V.Count);
            for (int i = 0; i < length; ++i)
            {
                Assert.AreEqual((byte)i, parsed.foo.V[i].A);
                Assert.AreEqual((byte)i, copy.foo.V[i].A);
                Assert.AreEqual((ulong)i, parsed.foo.V[i].B);
                Assert.AreEqual((ulong)i, copy.foo.V[i].B);
            }
        }

        private void RunTest<T>(string fbsType, int length, FlatBufferDeserializationOption option) where T : struct
        {
            string schema = $@"
            namespace StructVectorTests;

            table Table ({MetdataKeys.SerializerKind}) {{
                foo:Foo;
            }}
            struct Foo {{
              V:[{fbsType}:{length}];
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema, 
                new());

            Type tableType = asm.GetType("StructVectorTests.Table");
            Type fooType = asm.GetType("StructVectorTests.Foo");
            var typeModel = TypeModelContainer.CreateDefault().CreateTypeModel(fooType);
            Assert.AreEqual(FlatBufferSchemaType.Struct, typeModel.SchemaType);

            for (int i = 0; i < length; ++i)
            {
                PropertyInfo p = fooType.GetProperty($"__flatsharp__V_{i}");
                Assert.IsTrue(p.GetMethod.IsPublic);
                Assert.IsTrue(p.SetMethod.IsPublic);
                Assert.IsTrue(p.GetMethod.IsVirtual);
                Assert.IsTrue(p.SetMethod.IsVirtual);

                var attr = p.GetCustomAttribute<FlatBufferItemAttribute>();
                Assert.IsNotNull(attr);
                Assert.AreEqual(i, attr.Index);

                var browsableAttr = p.GetCustomAttribute<EditorBrowsableAttribute>();
                Assert.IsNotNull(browsableAttr);
                Assert.AreEqual(EditorBrowsableState.Advanced, browsableAttr.State);
            }

            var vectorProperty = fooType.GetProperty("V");
            Assert.IsNull(vectorProperty.GetCustomAttribute<FlatBufferItemAttribute>()); // pseudo-item, not actual.
            Assert.IsNull(vectorProperty.GetCustomAttribute<EditorBrowsableAttribute>());
            Assert.IsTrue(vectorProperty.GetMethod.IsPublic);
            Assert.IsFalse(vectorProperty.GetMethod.IsVirtual);
            Assert.IsNull(vectorProperty.SetMethod);

            dynamic table = Activator.CreateInstance(tableType);
            dynamic foo = Activator.CreateInstance(fooType);
            table.foo = foo;

            Assert.AreEqual(length, foo.V.Count);

            for (int i = 0; i < length; ++i)
            {
                foo.V[i] = GetRandom<T>();
            }

            byte[] data = new byte[1024];
            var serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(option) { EnableAppDomainInterceptOnAssemblyLoad = true });

            serializer.ReflectionSerialize((object)table, data);
            dynamic parsed = serializer.ReflectionParse(tableType, data);
            dynamic copy = Activator.CreateInstance(tableType, (object)parsed);

            Assert.AreEqual(length, parsed.foo.V.Count);
            for (int i = 0; i < length; ++i)
            {
                CheckRandom<T>(foo.V[i], parsed.foo.V[i]);
                CheckRandom<T>(foo.V[i], copy.foo.V[i]);
            }

            bool isMutable = option is FlatBufferDeserializationOption.VectorCacheMutable or FlatBufferDeserializationOption.GreedyMutable;

            if (length == 0)
            {
                return;
            }

            try
            {
                for (int i = 0; i < length; ++i)
                {
                    parsed.foo.V[i] = GetRandom<T>();
                }

                Assert.IsTrue(isMutable);
            }
            catch (NotMutableException)
            {
                Assert.IsFalse(isMutable);
            }
        }

        private static bool CheckRandom<T>(T left, T right) where T : struct
        {
            T[] leftItems = new T[] { left };
            T[] rightItems = new T[] { right };

            return MemoryMarshal.Cast<T, byte>(leftItems).SequenceEqual(MemoryMarshal.Cast<T, byte>(rightItems));
        }

        private static T GetRandom<T>() where T : struct
        {
            int bytes = Marshal.SizeOf<T>();
            byte[] random = new byte[bytes];
            new Random().NextBytes(random);

            return MemoryMarshal.Cast<byte, T>(random)[0];
        }
    }
}
