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
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    
    public class FileIdentifierTests
    {
        [Fact]
        public void Table_Root_Type()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            struct Struct {{ foo:int; }}

            file_identifier ""abcd"";
            root_type Table;
            ";

            var asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Type tabletype = asm.GetType("FileIdTests.A.Table");

            FlatBufferTableAttribute table = tabletype.GetCustomAttribute<FlatBufferTableAttribute>();
            Assert.NotNull(table);
            Assert.Equal("abcd", table.FileIdentifier);
        }

        [Fact]
        public void Struct_Root_Type()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            struct Struct {{ foo:int; }}
            root_type Struct;
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains("error: root type must be a table", ex.Errors[0]);
        }

        [Fact]
        public void Enum_Root_Type()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            enum Enum : ubyte {{ Foo, Bar, Baz }}
            root_type Enum;
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains("error: unknown root type: Enum", ex.Errors[0]);
        }

        [Fact]
        public void Root_Type_With_No_File_Identifier()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            root_type Table;
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Assert.Null(
                asm.GetType("FileIdTests.A.Table").GetCustomAttribute<FlatBufferTableAttribute>().FileIdentifier);
        }
    }
}
