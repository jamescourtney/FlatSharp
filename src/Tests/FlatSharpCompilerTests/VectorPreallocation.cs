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
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    public class VectorPreallocationTests
    {
        [Theory]
        [InlineData("Default", null)]
        [InlineData("Always", long.MaxValue)]
        [InlineData("Never", 0)]
        [InlineData("10", 10)]
        [InlineData("00", 0)]
        [InlineData("1024", 1024)]
        public void Parsing(string value, long? expected)
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace PreallocationTests;
                table Table {{ V : [ string ] ({MetadataKeys.VectorPreallocation}:""{value}""); }}
                ";

            var asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type type = asm.GetType("PreallocationTests.Table");
            PropertyInfo prop = type.GetProperty("V");
            FlatBufferItemAttribute item = prop.GetCustomAttribute<FlatBufferItemAttribute>();

            Assert.Equal(expected is not null, item.HasVectorPreallocationLimit);
            if (expected is not null)
            {
                Assert.Equal(expected.Value, item.VectorPreallocationLimit);
            }
        }

        [Fact]
        public void Invalid_OnTable()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace PreallocationTests;
                table Table ({MetadataKeys.VectorPreallocation}:""1024"") {{ V : [ string ]; }}
                ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains($"The attribute '{MetadataKeys.VectorPreallocation}' is never valid on Table elements.", ex.Message);
        }

        [Fact]
        public void Invalid_NotANumber()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace PreallocationTests;
                table Table {{ V : [ string ] ({MetadataKeys.VectorPreallocation}:""aardvark""); }}
                ";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(schema, new()));
            Assert.Contains($"Unable to parse '{MetadataKeys.VectorPreallocation}' value 'aardvark' as an int64.", ex.Message);
        }
    }
}
