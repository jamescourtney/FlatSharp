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

                enum ExternalEnum : ubyte ({MetadataKeys.External}) {{ A, B, C }}
                struct ExternalStruct ({MetadataKeys.External}, {MetadataKeys.ValueStruct}) {{ X : float32; Y : float32; Z : float32; }}
        
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

                enum ExternalEnum : ubyte ({MetadataKeys.External}:""ExternalTests.ExternalEnum"") {{ A, B, C }}
                struct ExternalStruct ({MetadataKeys.External}:""ExternalTests.ExternalStruct"", {MetadataKeys.ValueStruct}) {{ X : float32; Y : float32; Z : float32; }}
        
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
        public void ExternalTable_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                table ExternalTable ({MetadataKeys.External}) {{ A : int; }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new()));

            Assert.Contains("The attribute 'fs_external' is never valid on Table elements.", ex.Message);
        }

        [Fact]
        public void ExternalReferenceStruct_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                struct ExternalTable ({MetadataKeys.External}) {{ A : int; }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new()));

            Assert.Contains("The attribute 'fs_external' is never valid on ReferenceStruct elements.", ex.Message);
        }

        [Fact]
        public void ExternalReferenceUnion_NotAllowed()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;
                
                table A {{}}
                table B {{}}

                union Test ({MetadataKeys.External}) {{ A, B }}
            ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new()));

            Assert.Contains("The attribute 'fs_external' is never valid on Union elements.", ex.Message);
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
}