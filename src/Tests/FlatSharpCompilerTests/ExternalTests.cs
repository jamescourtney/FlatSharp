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
        public void ExternalItems()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}

                namespace ExternalTests;

                enum ExternalEnum : ubyte ({MetadataKeys.External}) {{ A, B, C }}
                struct ExternalStruct ({MetadataKeys.External}, {MetadataKeys.ValueStruct}) {{ X : float32; Y : float32; Z : float32; }}
                table ExternalTable ({MetadataKeys.External}) {{ A : int; B : string; }}
        
                table Table {{ E : ExternalEnum; S : ExternalStruct; T : ExternalTable; }}
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

            Assert.Null(asm.GetType("ExternalTests.ExternalTable"));
            prop = externalTable.GetProperty("T");
            Assert.NotNull(prop);
            Assert.Equal(typeof(global::ExternalTests.ExternalTable), prop.PropertyType);
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

    public class ExternalTable
    {
        public ExternalTable()
        {
        }

        public ExternalTable(ExternalTable item)
        {
            this.A = item.A;
            this.B = item.B;
        }

        protected ExternalTable(FlatBufferDeserializationContext context)
        {
        }

        protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
        {
        }

        public virtual int A { get; set; }

        public virtual string B { get; set; }
    }
}