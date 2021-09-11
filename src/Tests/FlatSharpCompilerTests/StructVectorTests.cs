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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Xunit;

    public class StructVectorTests
    {
        [Fact]
        public void StructVector_Byte_NegativeLength() 
            => Assert.Throws<InvalidFbsFileException>(() => this.RunTest<byte>("ubyte", -3, FlatBufferDeserializationOption.Greedy));

        [Fact]
        public void StructVector_Byte_ZeroLength()
            => Assert.Throws<InvalidFbsFileException>(() => this.RunTest<byte>("ubyte", 0, FlatBufferDeserializationOption.Greedy));

        [Fact]
        public void StructVector_Byte() => this.RunTest<byte>("ubyte", 1, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_SByte() => this.RunTest<sbyte>("byte", 2, FlatBufferDeserializationOption.GreedyMutable);

        [Fact]
        public void StructVector_Bool() => this.RunTest<bool>("bool", 3, FlatBufferDeserializationOption.Progressive);

        [Fact]
        public void StructVector_UShort() => this.RunTest<ushort>("ushort", 4, FlatBufferDeserializationOption.Progressive);

        [Fact]
        public void StructVector_Short() => this.RunTest<short>("short", 5, FlatBufferDeserializationOption.Progressive);

        [Fact]
        public void StructVector_UInt() => this.RunTest<uint>("uint", 6, FlatBufferDeserializationOption.Lazy);

        [Fact]
        public void StructVector_Int() => this.RunTest<int>("int", 7, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_ULong() => this.RunTest<ulong>("ulong", 8, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_Long() => this.RunTest<long>("long", 9, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_Float() => this.RunTest<float>("float", 10, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_Double() => this.RunTest<double>("double", 11, FlatBufferDeserializationOption.Greedy);

        [Fact]
        public void StructVector_InvalidType()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace StructVectorTests;

            table Table ({MetadataKeys.SerializerKind}) {{
                foo:Foo;
            }}
            struct Foo {{
              V:[Bar:7] ({MetadataKeys.NonVirtualProperty});
            }}";

            var ex = Assert.Throws<InvalidFbsFileException>(
                () => FlatSharpCompiler.CompileAndLoadAssembly(
                    schema,
                    new()));

            Assert.Contains("error: structs may contain only scalar or struct fields", ex.Message);
        }

        [Fact]
        public void StructVector_UnsafeVectorOnReferenceType()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace StructVectorTests;

            struct Foo {{
              V:[int:7] ({MetadataKeys.UnsafeValueStructVector});
            }}";

            var ex = Assert.Throws<InvalidFbsFileException>(
                () => FlatSharpCompiler.CompileAndLoadAssembly(
                    schema,
                    new()));

            Assert.Contains($"The attribute '{MetadataKeys.UnsafeValueStructVector}' is never valid on StructField elements.", ex.Message);
        }

        [Fact]
        public void StructVector_NestedStruct()
        {
            int length = 7;

            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace StructVectorTests;

            table Table ({MetadataKeys.SerializerKind}) {{
                foo:Foo;
            }}
            struct Bar {{ A:ubyte; B:ulong; }}
            struct Foo {{
              V:[Bar:{length}] ({MetadataKeys.NonVirtualProperty});
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new());

            Type tableType = asm.GetType("StructVectorTests.Table");
            Type fooType = asm.GetType("StructVectorTests.Foo");

            var property = fooType.GetProperty("__flatsharp__V_0", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.False(property.GetMethod.IsVirtual);
            Assert.False(property.SetMethod.IsVirtual);

            Type barType = asm.GetType("StructVectorTests.Bar");

            dynamic table = Activator.CreateInstance(tableType);
            dynamic foo = Activator.CreateInstance(fooType);
            table.foo = foo;

            Assert.Equal(length, foo.V.Count);

            dynamic dList = Activator.CreateInstance(typeof(List<>).MakeGenericType(barType));
            for (int i = 0; i < length; ++i)
            {
                dynamic bar = Activator.CreateInstance(barType);
                bar.A = (byte)i;
                bar.B = (ulong)i;

                dList.Add(bar);
            }

            // Verify deep copy.
            foo.V.CopyFrom(dList);
            for (int i = 0; i < length; ++i)
            {
                Assert.NotSame(dList[i], foo.V[i]);
                Assert.Equal<byte>(dList[i].A, foo.V[i].A);
                Assert.Equal<ulong>(dList[i].B, foo.V[i].B);
            }

            for (int i = 0; i < length; ++i)
            {
                dynamic bar = Activator.CreateInstance(barType);
                bar.A = (byte)i;
                bar.B = (ulong)i;

                foo.V[i] = bar;
            }

            byte[] data = new byte[1024]; 
            
            var serializer = CompilerTestHelpers.CompilerTestSerializer.Compile((object)table);
            serializer.Write(data, (object)table);
            dynamic parsed = serializer.Parse(data);
            dynamic copy = Activator.CreateInstance(tableType, (object)parsed);

            Assert.Equal(length, parsed.foo.V.Count);
            for (int i = 0; i < length; ++i)
            {
                Assert.Equal((byte)i, parsed.foo.V[i].A);
                Assert.Equal((byte)i, copy.foo.V[i].A);
                Assert.Equal((ulong)i, parsed.foo.V[i].B);
                Assert.Equal((ulong)i, copy.foo.V[i].B);
            }
        }

        private void RunTest<T>(string fbsType, int length, FlatBufferDeserializationOption option) where T : struct
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace StructVectorTests;

            table Table ({MetadataKeys.SerializerKind}) {{
                foo:Foo;
            }}
            struct Foo {{
              V:[{fbsType}:{length}];
            }}";

            (Assembly asm, string code) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(
                schema, 
                new());

            Type tableType = asm.GetType("StructVectorTests.Table");
            Type fooType = asm.GetType("StructVectorTests.Foo");
            var typeModel = TypeModelContainer.CreateDefault().CreateTypeModel(fooType);
            Assert.Equal(FlatBufferSchemaType.Struct, typeModel.SchemaType);

            for (int i = 0; i < length; ++i)
            {
                PropertyInfo p = fooType.GetProperty($"__flatsharp__V_{i}", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.True(p.GetMethod.IsFamily);
                Assert.True(p.SetMethod.IsFamily);
                Assert.True(p.GetMethod.IsVirtual);
                Assert.True(p.SetMethod.IsVirtual);

                var metaAttr = p.GetCustomAttribute<FlatBufferMetadataAttribute>();
                var attr = p.GetCustomAttribute<FlatBufferItemAttribute>();
                Assert.NotNull(attr);
                Assert.NotNull(metaAttr);
                Assert.Equal(i, attr.Index);
                Assert.Equal(FlatBufferMetadataKind.Accessor, metaAttr.Kind);
                Assert.Equal($"V[{attr.Index}]", metaAttr.Value);
            }

            var vectorProperty = fooType.GetProperty("V");
            Assert.Null(vectorProperty.GetCustomAttribute<FlatBufferItemAttribute>()); // pseudo-item, not actual.
            Assert.True(vectorProperty.GetMethod.IsPublic);
            Assert.False(vectorProperty.GetMethod.IsVirtual);
            Assert.Null(vectorProperty.SetMethod);

            dynamic table = Activator.CreateInstance(tableType);
            dynamic foo = Activator.CreateInstance(fooType);
            table.foo = foo;

            Assert.Equal(length, foo.V.Count);

            // Test copyFrom with full array.
            List<T> items = new List<T>();
            for (int i = 0; i < length; ++i)
            {
                items.Add(GetRandom<T>());
            }

            table.foo.V.CopyFrom((IReadOnlyList<T>)items);
            for (int i = 0; i < length; ++i)
            {
                CheckRandom<T>(items[i], table.foo.V[i]);
            }

            for (int i = 0; i < length; ++i)
            {
                foo.V[i] = GetRandom<T>();
            }

            byte[] data = new byte[1024]; 
            
            var fbs = new FlatBufferSerializer(
                 new FlatBufferSerializerOptions(option) { EnableAppDomainInterceptOnAssemblyLoad = true });

            var serializer = fbs.Compile((object)table);
            serializer.Write(data, (object)table);
            dynamic parsed = serializer.Parse(data);

            dynamic copy = Activator.CreateInstance(tableType, (object)parsed);

            Assert.Equal(length, parsed.foo.V.Count);
            for (int i = 0; i < length; ++i)
            {
                CheckRandom<T>(foo.V[i], parsed.foo.V[i]);
                CheckRandom<T>(foo.V[i], copy.foo.V[i]);
            }

            bool isMutable = option is FlatBufferDeserializationOption.GreedyMutable;

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

                Assert.True(isMutable);
            }
            catch (NotMutableException)
            {
                Assert.False(isMutable);
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
