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
    using FlatSharp.Compiler;
    using Xunit;

    
    public class EnumTests
    {
        [Fact]
        public void BasicEnumTest_Byte()
        {
            this.EnumTest<byte>("uint8");
            this.EnumTest<byte>("ubyte");
        }

        [Fact]
        public void BasicEnumTest_SByte()
        {
            this.EnumTest<sbyte>("int8");
            this.EnumTest<sbyte>("byte");
        }

        [Fact]
        public void BasicEnumTest_Short()
        {
            this.EnumTest<short>("int16");
            this.EnumTest<short>("short");
        }

        [Fact]
        public void BasicEnumTest_UShort()
        {
            this.EnumTest<ushort>("uint16");
            this.EnumTest<ushort>("ushort");
        }

        [Fact]
        public void BasicEnumTest_Int()
        {
            this.EnumTest<int>("int32");
            this.EnumTest<int>("int");
        }

        [Fact]
        public void BasicEnumTest_UInt()
        {
            this.EnumTest<uint>("uint32");
            this.EnumTest<uint>("uint");
        }

        [Fact]
        public void BasicEnumTest_Long()
        {
            this.EnumTest<long>("int64");
            this.EnumTest<long>("long");
        }

        [Fact]
        public void BasicEnumTest_ULong()
        {
            this.EnumTest<ulong>("uint64");
            this.EnumTest<ulong>("ulong");
        }

        [Fact]
        public void FlagsTest_OK()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ Red, Blue, Green, Yellow }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");

            Assert.NotNull(t.GetCustomAttribute(typeof(FlagsAttribute)));
            Assert.True(t.IsEnum);
            Assert.Equal(typeof(byte), Enum.GetUnderlyingType(t));
            Assert.True(t.GetCustomAttribute<FlatSharp.Attributes.FlatBufferEnumAttribute>() != null);

            string[] names = Enum.GetNames(t);
            Assert.Equal(4, names.Length);
            Assert.Equal("Red", names[0]);
            Assert.Equal("Blue", names[1]);
            Assert.Equal("Green", names[2]);
            Assert.Equal("Yellow", names[3]);

            Array values = Enum.GetValues(t);
            Assert.Equal(1, (byte)values.GetValue(0));
            Assert.Equal(2, (byte)values.GetValue(1));
            Assert.Equal(4, (byte)values.GetValue(2));
            Assert.Equal(8, (byte)values.GetValue(3));
        }

        [Fact]
        public void FlagsTest_TooMany()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ A, B, C, D, E, F, G, H, I }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: bit flag out of range of underlying integral type", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_WrongUnderlyingType_Bool()
        {
            var ex = Assert.Throws<InvalidFbsFileException>(() => this.EnumTest<bool>("bool")); 
            Assert.Contains("error: underlying enum type must be integral", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_WrongUnderlyingType_Double()
        {
            var ex = Assert.Throws<InvalidFbsFileException>(() => this.EnumTest<double>("double"));
            Assert.Contains("error: underlying enum type must be integral", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_WrongUnderlyingType_Float()
        {
            var ex = Assert.Throws<InvalidFbsFileException>(() => this.EnumTest<float>("float"));
            Assert.Contains("error: underlying enum type must be integral", ex.Message);
        }

        [Fact]
        public void DuplicateValues()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 0X10, Yellow = 16 }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type enumType = asm.GetType("Foo.Bar.MyEnum");
            Assert.NotNull(enumType);

            Array values = Enum.GetValues(enumType);
            string[] names = Enum.GetNames(enumType);

            Assert.Equal("Red", names[0]);
            Assert.Equal((byte)0, (byte)values.GetValue(0));

            Assert.Equal("Yellow", names[1]);
            Assert.Equal((byte)16, (byte)values.GetValue(1));

            Assert.Equal(2, names.Length);
            Assert.Equal(2, values.GetLength(0));
        }

        [Fact]
        public void InvalidEnumTest_NaturalOverflow()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 255, Blue }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value does not fit, \"255 + 1\" out of [0; 255]", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_DuplicateNames()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte {{ Red, Blue, Yellow, Red }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value already exists: Red", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_DuplicateNames_BitFlags()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ Red, Blue, Yellow, Red }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value already exists: Red", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_ValueOutOfRangeOfUnderlyingType_Above()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 255, Yellow = 256 }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value does not fit, \"256\" out of [0; 255]", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_ValueOutOfRangeOfUnderlyingType_Below()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ulong {{ Red = -1, Blue = 255, Yellow = 256 }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value does not fit, \"-1\"", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_UnknownType()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : foobar {{ Red = 0x0, Blue = 255, Yellow = 256 }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: underlying enum type must be integral", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_String()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : string {{ Red, Blue, Yellow }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: underlying enum type must be integral", ex.Message);
        }

        [Fact]
        public void InvalidEnumTest_NotAnInt()
        {
            // Quirk of the grammar allows this.
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : uint {{ Red = 1.2 }}";
            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.Contains("error: enum value does not fit, \"1.2\"", ex.Message);
        }

        [Fact]
        public void BasicEnumTest_HexNumber()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 0X01, Green = 2, Yellow = 0X10 }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");
            Assert.True(t.IsEnum);
            Assert.Equal(typeof(byte), Enum.GetUnderlyingType(t));

            Array values = Enum.GetValues(t);

            Assert.Equal((byte)0, (byte)values.GetValue(0));
            Assert.Equal((byte)1, (byte)values.GetValue(1));
            Assert.Equal((byte)2, (byte)values.GetValue(2));
            Assert.Equal((byte)16, (byte)values.GetValue(3));
        }

        [Fact]
        public void BasicEnumTest_NegativeNumbers()
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : byte {{ Red = -0x02, Blue = -1, Green = 0, Yellow = 0X1, Purple = 0x02 }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");
            Assert.True(t.IsEnum);
            Assert.Equal(typeof(sbyte), Enum.GetUnderlyingType(t));

            var values = Enum.GetValues(t).OfType<object>().Select(x => new { Name = x.ToString(), Value = (sbyte)x });

            Assert.Equal((sbyte)-2, values.Single(x => x.Name == "Red").Value);
            Assert.Equal((sbyte)-1, values.Single(x => x.Name == "Blue").Value);
            Assert.Equal((sbyte)0, values.Single(x => x.Name == "Green").Value);
            Assert.Equal((sbyte)1, values.Single(x => x.Name == "Yellow").Value);
            Assert.Equal((sbyte)2, values.Single(x => x.Name == "Purple").Value);
        }

        private void EnumTest<T>(string flatBufferType) where T : struct
        {
            string fbs = $"{MetadataHelpers.AllAttributes}namespace Foo.Bar; enum MyEnum : {flatBufferType} {{ Red, Blue = 3, Green, Yellow }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");

            Assert.Null(t.GetCustomAttribute(typeof(FlagsAttribute)));
            Assert.True(t.IsEnum);
            Assert.Equal(typeof(T), Enum.GetUnderlyingType(t));
            Assert.True(t.GetCustomAttribute<FlatSharp.Attributes.FlatBufferEnumAttribute>() != null);

            string[] names = Enum.GetNames(t);
            Assert.Equal(4, names.Length);
            Assert.Equal("Red", names[0]);
            Assert.Equal("Blue", names[1]);
            Assert.Equal("Green", names[2]);
            Assert.Equal("Yellow", names[3]);

            Array values = Enum.GetValues(t);

            Assert.Equal(Convert.ChangeType(0, typeof(T)), (T)values.GetValue(0));
            Assert.Equal(Convert.ChangeType(3, typeof(T)), (T)values.GetValue(1));
            Assert.Equal(Convert.ChangeType(4, typeof(T)), (T)values.GetValue(2));
            Assert.Equal(Convert.ChangeType(5, typeof(T)), (T)values.GetValue(3));
        }
    }
}
