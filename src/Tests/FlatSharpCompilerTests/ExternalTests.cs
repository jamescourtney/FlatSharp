/*
 * Copyright 2022 James Courtney
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

using ExternalTests;
using System.Runtime.InteropServices;

namespace FlatSharpTests.Compiler
{
    public class ExternalTests
    {
        [Fact]
        public void ExternalStructAndEnum_WithImplicitNames()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;

                enum ExternalEnum : ubyte ({MetadataKeys.UnsafeExternal}) {{ A, B, C }}
                struct ExternalStruct ({MetadataKeys.UnsafeExternal}, {MetadataKeys.ValueStruct}) {{ X : float32; Y : float32; Z : float32; }}
        
                table Table {{ E : ExternalEnum; S : ExternalStruct;  }}
            ";

            var asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly });
            
            Type externalTable = asm.GetType("ExternalTests.Table");
            Assert.NotNull(externalTable);

            Assert.Null(asm.GetType("ExternalTests.ExternalEnum"));
            PropertyInfo prop = externalTable.GetProperty("E");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalEnum), prop.PropertyType);

            Assert.Null(asm.GetType("ExternalTests.ExternalStruct"));
            prop = externalTable.GetProperty("S");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalStruct?), prop.PropertyType);
        }

        [Fact]
        public void ExternalStructAndEnum_WithExplicitNames()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace Something;

                enum ExternalEnum : ubyte ({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalEnum"") {{ A, B, C }}
                struct ExternalStruct ({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct}) {{ X : float32; Y : float32; Z : float32; }}
        
                table Table {{ E : ExternalEnum; S : ExternalStruct;  }}
            ";

            var asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly });

            Type externalTable = asm.GetType("Something.Table");
            Assert.NotNull(externalTable);

            Assert.Null(asm.GetType("Something.ExternalEnum"));
            PropertyInfo prop = externalTable.GetProperty("E");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalEnum), prop.PropertyType);

            Assert.Null(asm.GetType("Something.ExternalStruct"));
            prop = externalTable.GetProperty("S");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalStruct?), prop.PropertyType);
        }

        [Fact]
        public void ExternalGenericStruct()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace Something;

                struct ExternalStruct ({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalGenericStruct<System.Single>"", {MetadataKeys.ValueStruct}) {{ X : float32; }}
        
                table Table {{ S : ExternalStruct;  }}
            ";

            var asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly });

            Type externalTable = asm.GetType("Something.Table");
            Assert.NotNull(externalTable);

            var prop = externalTable.GetProperty("S");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalGenericStruct<float>?), prop.PropertyType);
        }

        [Fact]
        public void ExternalStruct_NoNamespace()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace Something;

                struct ExternalStruct ({MetadataKeys.UnsafeExternal}:""float"", {MetadataKeys.ValueStruct}) {{ X : float32; }}
        
                table Table {{ S : ExternalStruct;  }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly }));

            Assert.Contains("External types must be in a namespace. Type = 'float'.", ex.Message);
        }

        [Fact]
        public void ExternalTable_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                table ExternalTable ({MetadataKeys.UnsafeExternal}) {{ A : int; }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new()));

            Assert.Contains("The attribute 'fs_unsafeExternal' is never valid on Table elements.", ex.Message);
        }

        [Fact]
        public void ExternalReferenceStruct_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                struct ExternalTable ({MetadataKeys.UnsafeExternal}) {{ A : int; }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new()));

            Assert.Contains("The attribute 'fs_unsafeExternal' is never valid on ReferenceStruct elements.", ex.Message);
        }

        [Fact]
        public void ExternalUnion_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                
                table A {{}}
                table B {{}}

                union Test ({MetadataKeys.UnsafeExternal}) {{ A, B }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly }));

            Assert.Contains("The attribute 'fs_unsafeExternal' is never valid on Union elements.", ex.Message);
        }

        /// <summary>
        /// This tests a bug where the renaming of external types affects
        /// the order of the types alphabetically. This can lead to bugs
        /// since BFBS schemas are ordered alphabetically, so renaming
        /// 'AAAA' to 'ExternalTests.stuff' might reorder the list of types,
        /// which throws off indices.
        /// </summary>
        [Fact]
        public void ExternalStruct_WithRenamingChangingOrder()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace AAAA;

                // AAAA.BBBB -> ExternalTests.ExternalStruct
                struct BBBB({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    X : float32;
                    Y : float32;
                    Z : float32;
                }}

                // AAAA.CCCC
                table CCCC
                {{
                    Key : string (key, required);
                    Value : int;
                }}

                // AAAA.DDDD
                table DDDD ({MetadataKeys.SerializerKind}:""Lazy"")
                {{
                    Items : [ CCCC ] ({MetadataKeys.VectorKind}:""IIndexedVector"");
                }}
            ";

            // Compiling is good enough.
            var asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly });
        }

        [Fact]
        public void ExternalStruct_Duplicates_SameFile()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace Test;

                struct First({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    Data : [ float32 : 3 ];
                }}

                struct Second({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    Data : [ float32 : 3 ];
                }}

                table MyTable ({MetadataKeys.SerializerKind}:""Lazy"")
                {{
                    A : First;
                    B : Second;
                }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly }));

            Assert.Contains("Duplicate type declared in same FBS file: Test.Second (ExternalTests.ExternalStruct)", ex.Message);
        }


        [Fact]
        public void ExternalStruct_Duplicates_DifferentFiles()
        {
            var schemaA = $@"
                include ""B.fbs"";
                include ""C.fbs"";
                include ""D.fbs"";

                {MetadataHelpers.AllAttributes}

                namespace Test;

                table Root
                {{ 
                    B : StructB;
                    C : StructC;
                    D : D.StructD;
                }}
            ";

            var schemaB = $@"
                {MetadataHelpers.AllAttributes}
                namespace Test;

                struct StructB({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    Data : [ float32 : 3 ];
                }}
            ";

            var schemaC = $@"
                {MetadataHelpers.AllAttributes}
                namespace Test;

                struct StructC({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    Data : [ float32 : 3 ];
                }}
            ";

            var schemaD = $@"
                {MetadataHelpers.AllAttributes}
                namespace D;

                struct StructD({MetadataKeys.UnsafeExternal}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct})
                {{
                    Data : [ float32 : 3 ];
                }}
            ";

            var schemas = new[]
            {
                ("A.fbs", schemaA),
                ("B.fbs", schemaB),
                ("C.fbs", schemaC),
                ("D.fbs", schemaD),
            };

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schemas,
                new(),
                new Assembly[] { typeof(ExternalTests).Assembly }));

            Assert.Contains("Duplicate type declared in two different FBS files: Test.StructB (ExternalTests.ExternalStruct)", ex.Message);
        }
    }
}

namespace ExternalTests
{
    public enum ExternalEnum : byte
    {
        A = 1,
        B = 2,
        C = 3,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ExternalStruct
    {
        [FieldOffset(0)]
        public float X;

        [FieldOffset(4)]
        public float Y;

        [FieldOffset(8)]
        public float Z;
    }

    public struct ExternalGenericStruct<T> where T : struct
    {
        public T Value;
    }
}