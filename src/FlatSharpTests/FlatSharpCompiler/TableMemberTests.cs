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
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TableMemberTests
    {
        [TestMethod]
        public void TableMember_bool() => this.RunCompoundTest<bool>("bool");

        [TestMethod]
        public void TableMember_byte() => this.RunCompoundTest<byte>("ubyte");

        [TestMethod]
        public void TableMember_sbyte() => this.RunCompoundTest<sbyte>("byte");

        [TestMethod]
        public void TableMember_short_alias() => this.RunCompoundTest<short>("short");

        [TestMethod]
        public void TableMember_short() => this.RunCompoundTest<short>("int16");

        [TestMethod]
        public void TableMember_ushort_alias() => this.RunCompoundTest<ushort>("ushort");

        [TestMethod]
        public void TableMember_ushort() => this.RunCompoundTest<ushort>("uint16");

        [TestMethod]
        public void TableMember_int_alias() => this.RunCompoundTest<int>("int");

        [TestMethod]
        public void TableMember_int() => this.RunCompoundTest<int>("int32");

        [TestMethod]
        public void TableMember_uint_alias() => this.RunCompoundTest<uint>("uint");

        [TestMethod]
        public void TableMember_uint() => this.RunCompoundTest<uint>("uint32");

        [TestMethod]
        public void TableMember_long_alias() => this.RunCompoundTest<long>("long");

        [TestMethod]
        public void TableMember_long() => this.RunCompoundTest<long>("int64");

        [TestMethod]
        public void TableMember_ulong_alias() => this.RunCompoundTest<ulong>("ulong");

        [TestMethod]
        public void TableMember_ulong() => this.RunCompoundTest<ulong>("uint64");

        [TestMethod]
        public void TableMember_float_alias() => this.RunCompoundTest<float>("float");

        [TestMethod]
        public void TableMember_float() => this.RunCompoundTest<float>("float32");

        [TestMethod]
        public void TableMember_double_alias() => this.RunCompoundTest<double>("double");

        [TestMethod]
        public void TableMember_double() => this.RunCompoundTest<double>("float64");

        [TestMethod]
        public void TableMember_string() => this.RunCompoundTest<string>("string");

        private void RunCompoundTest<T>(string fbsType)
        {
            this.RunSingleTest<T>(fbsType);
            this.RunSingleTest<IList<T>>($"[{fbsType}]");
            this.RunSingleTest<IList<T>>($"[{fbsType}]  (vectortype: IList)");
            this.RunSingleTest<IReadOnlyList<T>>($"[{fbsType}]  (vectortype: IReadOnlyList)");

            if (typeof(T).IsValueType)
            {
                this.RunSingleTest<Memory<T>>($"[{fbsType}]  (vectortype: Memory)");
                this.RunSingleTest<ReadOnlyMemory<T>>($"[{fbsType}]  (vectortype: ReadOnlyMemory)");
            }
            else
            {
                Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => this.RunSingleTest<Memory<T>>($"[{fbsType}]  (vectortype: Memory)"));
                Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => this.RunSingleTest<ReadOnlyMemory<T>>($"[{fbsType}]  (vectortype: ReadOnlyMemory)"));
            }
        }

        private void RunSingleTest<T>(string fbsType)
        {
            try
            {
                string schema = $@"namespace TableMemberTests; table Table {{ member:{fbsType}; }}";
                Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

                Type tableType = asm.GetType("TableMemberTests.Table");
                PropertyInfo property = tableType.GetProperty("member");

                Assert.AreEqual(typeof(T), property.PropertyType);
                var attribute = property.GetCustomAttribute<FlatBufferItemAttribute>();

                Assert.AreEqual(0, attribute.Index);
                Assert.AreEqual(false, attribute.Deprecated);
                Assert.AreEqual(null, attribute.DefaultValue);

                byte[] data = new byte[100];
                CompilerTestHelpers.CompilerTestSerializer.ReflectionSerialize(Activator.CreateInstance(tableType), data);
                CompilerTestHelpers.CompilerTestSerializer.ReflectionParse(tableType, data);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
