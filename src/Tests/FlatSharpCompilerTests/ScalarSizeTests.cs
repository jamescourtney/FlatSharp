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
    using FlatSharp.Compiler;
    using FlatSharp.Compiler.Schema;
    using FlatSharp.Compiler.SchemaModel;
    using Xunit;

    public class ScalarSizeTests
    {
        [Theory]
        [InlineData(BaseType.Bool, 1)]
        [InlineData(BaseType.Byte, 1)]
        [InlineData(BaseType.UByte, 1)]
        [InlineData(BaseType.Short, 2)]
        [InlineData(BaseType.UShort, 2)]
        [InlineData(BaseType.Int, 4)]
        [InlineData(BaseType.UInt, 4)]
        [InlineData(BaseType.Float, 4)]
        [InlineData(BaseType.Long, 8)]
        [InlineData(BaseType.ULong, 8)]
        [InlineData(BaseType.Double, 8)]
        public void SizesCorrect(BaseType type, int expectedSize)
        {
            Assert.True(type.IsScalar());
            Assert.Equal(expectedSize, type.GetScalarSize());
        }
    }
}
