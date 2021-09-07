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
        public void Multiple_Root_Types_Different_Namespaces()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            root_type Table;

            namespace FileIdTests.B;
            table TableB {{ foo:int; }}
            root_type TableB;
            file_identifier ""food"";
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Duplicate root types: 'Table' and 'TableB'.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void Multiple_Root_Types_Same_Namespace()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            file_identifier ""food"";
            root_type Table;
            root_type Table;
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Duplicate root types: 'Table' and 'Table'.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void Multiple_File_Identifiers_Different_Namespaces()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            file_identifier ""doof"";

            namespace FileIdTests.B;
            table TableB {{ foo:int; }}
            root_type TableB;
            file_identifier ""food"";
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Duplicate file identifiers: 'doof' and 'food'.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void Multiple_File_Identifiers_Same_Namespace()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            file_identifier ""food"";
            file_identifier ""doof"";
            root_type Table;
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Duplicate file identifiers: 'food' and 'doof'.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void Unknown_Root_Type_No_File_Identifier()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            root_type Something;
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Unable to resolve root_type 'Something'.', Scope=$", ex.Errors[0]);
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
            Assert.StartsWith("Message='root_type 'Struct' does not reference a table.', Scope=$", ex.Errors[0]);
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
            Assert.StartsWith("Message='root_type 'Enum' does not reference a table.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void Unknown_Root_Type_With_File_Identifier()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            root_type Something;
            file_identifier ""abcd"";
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.StartsWith("Message='Unable to resolve root_type 'Something'.', Scope=$", ex.Errors[0]);
        }

        [Fact]
        public void File_Identifier_With_No_Root_Type()
        {
            string schema = $@"
            namespace FileIdTests.A;
            table Table {{ foo:int; }}
            file_identifier ""abcd"";
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            Assert.Null(
                asm.GetType("FileIdTests.A.Table").GetCustomAttribute<FlatBufferTableAttribute>().FileIdentifier);
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
