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
        public void TableMember_short_alias() => this.RunCompoundTestWithDefaultValue<short>("short");

        [Fact]
        public void TableMember_short() => this.RunCompoundTestWithDefaultValue<short>("int16");

        [Fact]
        public void TableMember_ushort_alias() => this.RunCompoundTestWithDefaultValue<ushort>("ushort");

        [Fact]
        public void TableMember_ushort() => this.RunCompoundTestWithDefaultValue<ushort>("uint16");

        [Fact]
        public void TableMember_int_alias() => this.RunCompoundTestWithDefaultValue<int>("int");

        [Fact]
        public void TableMember_int() => this.RunCompoundTestWithDefaultValue<int>("int32");

        [Fact]
        public void TableMember_uint_alias() => this.RunCompoundTestWithDefaultValue<uint>("uint");

        [Fact]
        public void TableMember_uint() => this.RunCompoundTestWithDefaultValue<uint>("uint32");

        [Fact]
        public void TableMember_long_alias() => this.RunCompoundTestWithDefaultValue<long>("long");

        [Fact]
        public void TableMember_long() => this.RunCompoundTestWithDefaultValue<long>("int64");

        [Fact]
        public void TableMember_ulong_alias() => this.RunCompoundTestWithDefaultValue<ulong>("ulong");

        [Fact]
        public void TableMember_ulong() => this.RunCompoundTestWithDefaultValue<ulong>("uint64");

        [Fact]
        public void TableMember_float_alias() => this.RunCompoundTestWithDefaultValue<float>("float", "G3", 3.14f);

        [Fact]
        public void TableMember_float() => this.RunCompoundTestWithDefaultValue<float>("float32", "G3", 3.14f);

        [Fact]
        public void TableMember_double_alias() => this.RunCompoundTestWithDefaultValue<double>("double", "G17");

        [Fact]
        public void TableMember_double() => this.RunCompoundTestWithDefaultValue<double>("float64", "G17");

        [Fact]
        public void TableMember_string() => this.RunCompoundTest<string>("string");

        [Fact]
        public void TableMember_with_id()
        {
            try
            {
                const string Schema = "namespace TableMemberTests; table Table { member:string (id:1); member2:int (id:0); }";
                var assembly = FlatSharpCompiler.CompileAndLoadAssembly(Schema, new());

                var tableType = assembly.GetType("TableMemberTests.Table");
                var property = tableType.GetProperty("member");

                Assert.Equal(typeof(string), property.PropertyType);
                var attribute = property.GetCustomAttribute<FlatBufferItemAttribute>();

                Assert.Equal(1, attribute.Index);

                dynamic item = Activator.CreateInstance(tableType);
                item.member = "member";
                item.member2 = 57;

                var data = new byte[100];

                var serializer = CompilerTestHelpers.CompilerTestSerializer.Compile((object)item);
                serializer.Write(data, (object)item);
                dynamic parsed = serializer.Parse(data);

                Assert.Equal("member", parsed.member);
                Assert.Equal(57, parsed.member2);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        [Fact]
        public void TableMember_only_some_fields_have_id()
        {
            try
            {
                const string Schema = "namespace TableMemberTests; table Table { member:string (id:1); member2:int; }";
                Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(Schema, new()));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        [Fact]
        public void TableMember_negative_id()
        {
            try
            {
                const string Schema = "namespace TableMemberTests; table Table { member:string (id:0); member2:int (id:-1); }";
                Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(Schema, new()));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        [Fact]
        public void TableMember_has_id_attribute_but_with_no_value()
        {
            try
            {
                const string Schema = "namespace TableMemberTests; table Table { member:string (id:0); member2:int (id); }";
                Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(Schema, new()));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        [Fact]
        public void StructMember_with_id()
        {
            try
            {
                const string Schema = "namespace TableMemberTests; struct Struct { member:string (id:1); member2:int (id:0); }";
                Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(Schema, new()));
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        private void RunCompoundTestWithDefaultValue_Bool(string fbsType)
        {
            this.RunSingleTest<bool>($"{fbsType} = true", hasDefaultValue: true, expectedDefaultValue: true);
            this.RunSingleTest<bool>($"{fbsType} = false", hasDefaultValue: true, expectedDefaultValue: false);
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
            this.RunSingleTest<IList<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}: IList)");
            this.RunSingleTest<T[]>($"[{fbsType}]  ({MetadataKeys.VectorKind}: Array)");
            this.RunSingleTest<IReadOnlyList<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}: IReadOnlyList)");

            if (typeof(T) == typeof(byte))
            {
                this.RunSingleTest<Memory<T>?>($"[{fbsType}]  ({MetadataKeys.VectorKind}: Memory)");
                this.RunSingleTest<ReadOnlyMemory<T>?>($"[{fbsType}]  ({MetadataKeys.VectorKind}: ReadOnlyMemory)");
            }
            else
            {
                Assert.Throws<InvalidFbsFileException>(() => this.RunSingleTest<Memory<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}: Memory)"));
                Assert.Throws<InvalidFbsFileException>(() => this.RunSingleTest<ReadOnlyMemory<T>>($"[{fbsType}]  ({MetadataKeys.VectorKind}: ReadOnlyMemory)"));
            }
        }

        private void RunSingleTest<T>(string fbsType, bool deprecated = false, bool hasDefaultValue = false, T expectedDefaultValue = default)
        {
            try
            {
                string schema = $@"namespace TableMemberTests; table Table {{ member:{fbsType}; member2:int; }}";
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
