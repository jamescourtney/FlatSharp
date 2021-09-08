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
    using FlatSharp.Compiler;
    using Xunit;

    public class OptionalScalarTests
    {
        [Fact]
        public void TestOptionalScalars()
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace OptionalScalarTests;
            
            enum TestEnum : ubyte
            {{
                A = 1,
                B = 2
            }}

            table Table ({MetadataKeys.SerializerKind}:""greedy"") {{
                Bool : bool = null;
                Byte : ubyte = null;
                SByte : byte = null;
                UShort : uint16 = null;
                Short : int16 = null;
                UInt : uint32 = null;
                Int : int32 = null;
                ULong : uint64 = null;
                Long : int64 = null;
                Double : double = null;
                Float : float32 = null;
                Enum : TestEnum = null;
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type tableType = asm.GetTypes().Single(x => x.FullName == "OptionalScalarTests.Table");

            Assert.Equal(typeof(bool?), tableType.GetProperty("Bool").PropertyType);
            Assert.Equal(typeof(byte?), tableType.GetProperty("Byte").PropertyType);
            Assert.Equal(typeof(sbyte?), tableType.GetProperty("SByte").PropertyType);
            Assert.Equal(typeof(ushort?), tableType.GetProperty("UShort").PropertyType);
            Assert.Equal(typeof(short?), tableType.GetProperty("Short").PropertyType);
            Assert.Equal(typeof(uint?), tableType.GetProperty("UInt").PropertyType);
            Assert.Equal(typeof(int?), tableType.GetProperty("Int").PropertyType);
            Assert.Equal(typeof(ulong?), tableType.GetProperty("ULong").PropertyType);
            Assert.Equal(typeof(long?), tableType.GetProperty("Long").PropertyType);
            Assert.Equal(typeof(double?), tableType.GetProperty("Double").PropertyType);
            Assert.Equal(typeof(float?), tableType.GetProperty("Float").PropertyType);

            var underlyingType = Nullable.GetUnderlyingType(tableType.GetProperty("Enum").PropertyType);
            Assert.True(underlyingType != null);
            Assert.True(underlyingType.IsEnum);
            Assert.Equal(typeof(byte), Enum.GetUnderlyingType(underlyingType));
        }
    }
}
