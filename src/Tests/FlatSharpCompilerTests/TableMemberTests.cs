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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Xunit;
        
    public class TableMemberTests
    {
        [Fact]
        public void TableMember_bool() => this.RunCompoundTestWithDefaultValue_Bool("bool");

        [Fact]
        public void TableMember_byte() => this.RunCompoundTestWithDefaultValue<byte>("ubyte");

        [Fact]
        public void TableMember_sbyte() => this.RunCompoundTestWithDefaultValue<sbyte>("byte");

        [Fact]
        public void TableMember_short() => this.RunCompoundTestWithDefaultValue<short>("int16");

        [Fact]
        public void TableMember_ushort() => this.RunCompoundTestWithDefaultValue<ushort>("uint16");

        [Fact]
        public void TableMember_int() => this.RunCompoundTestWithDefaultValue<int>("int32");

        [Fact]
        public void TableMember_uint() => this.RunCompoundTestWithDefaultValue<uint>("uint32");

        [Fact]
        public void TableMember_long() => this.RunCompoundTestWithDefaultValue<long>("int64");

        [Fact]
        public void TableMember_ulong() => this.RunCompoundTestWithDefaultValue<ulong>("uint64");

        [Fact]
        public void TableMember_float() => this.RunCompoundTestWithDefaultValue<float>("float32", "G3", 3.14f);

        [Fact]
        public void TableMember_double() => this.RunCompoundTestWithDefaultValue<double>("float64", "G17");

        [Fact]
        public void TableMember_string() => this.RunCompoundTest<string>("string");

        private void RunCompoundTestWithDefaultValue_Bool(string fbsType)
        {
            this.RunSingleTest<bool>($"{fbsType} = true", hasDefaultValue: true, expectedDefaultValue: true);
            this.RunSingleTest<bool>($"{fbsType} = false", hasDefaultValue: false, expectedDefaultValue: false);
            this.RunCompoundTest<bool>(fbsType);
        }

        private void RunCompoundTestWithDefaultValue<T>(string fbsType, string format = null, T? value = null) where T : struct, IFormattable
        {
            T randomValue;
            if (value is null)
            {
                Random random = new Random();
                byte[] data = new byte[16];
                random.NextBytes(data);

                if (typeof(T) == typeof(ulong))
                {
                    // FlatBuffers doesn't represent default values over long.maxvalue. Trim the leading
                    // bit off the high and low values so we don't have to care about endianness.
                    data[0] &= 0x7F;
                    data[7] &= 0x7F;
                }

                randomValue = MemoryMarshal.Cast<byte, T>(data)[0];
            }
            else
            {
                randomValue = value.Value;
            }

            this.RunSingleTest<T>($"{fbsType} = {randomValue.ToString(format, null).ToLowerInvariant()}", hasDefaultValue: true, expectedDefaultValue: randomValue);
            this.RunCompoundTest<T>(fbsType);
        }

        private void RunCompoundTest<T>(string fbsType)
        {
            this.RunSingleTest<T>(fbsType);
            this.RunSingleTest<T>($"{fbsType} ({MetadataKeys.Deprecated})", deprecated: true);
            this.RunSingleTest<IList<T>>($"[{fbsType}]");
            this.RunSingleTest<IList<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"IList\")");
            this.RunSingleTest<T[]>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"Array\")");
            this.RunSingleTest<IReadOnlyList<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"IReadOnlyList\")");

            if (typeof(T) == typeof(byte))
            {
                this.RunSingleTest<Memory<T>?>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"Memory\")");
                this.RunSingleTest<ReadOnlyMemory<T>?>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"ReadOnlyMemory\")");
            }
            else
            {
                var ex1 = Assert.Throws<InvalidFlatBufferDefinitionException>(() => this.RunSingleTest<Memory<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"Memory\")"));
                var ex2 = Assert.Throws<InvalidFlatBufferDefinitionException>(() => this.RunSingleTest<ReadOnlyMemory<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}:\"ReadOnlyMemory\")"));

                Assert.Contains("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>", ex1.Message);
                Assert.Contains("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>", ex2.Message);
            }
        }

        private void RunSingleTest<T>(string fbsType, bool deprecated = false, bool hasDefaultValue = false, T expectedDefaultValue = default)
        {
            try
            {
                string schema = $@"{MetadataHelpers.AllAttributes} namespace TableMemberTests; table Table {{ member:{fbsType}; member2:int; }}";
                Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

                Type tableType = asm.GetType("TableMemberTests.Table");
                PropertyInfo property = tableType.GetProperty("member");

                Assert.Equal(typeof(T), property.PropertyType);
                var attribute = property.GetCustomAttribute<FlatBufferItemAttribute>();

                Assert.Equal(0, attribute.Index);
                Assert.Equal(deprecated, attribute.Deprecated);

                Assert.Equal(hasDefaultValue, attribute.DefaultValue != null);
                if (hasDefaultValue)
                {
                    Assert.IsType<T>(attribute.DefaultValue);

                    T actualDefault = (T)attribute.DefaultValue;
                    Assert.Equal(0, Comparer<T>.Default.Compare(expectedDefaultValue, actualDefault));
                }

                byte[] data = new byte[100];
                var compiled = CompilerTestHelpers.CompilerTestSerializer.Compile(tableType);

                compiled.Write(data, Activator.CreateInstance(tableType));
                compiled.Parse(data);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
