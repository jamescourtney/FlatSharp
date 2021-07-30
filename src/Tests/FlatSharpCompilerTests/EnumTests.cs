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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void BasicEnumTest_Byte()
        {
            this.EnumTest<byte>("uint8");
            this.EnumTest<byte>("ubyte");
        }

        [TestMethod]
        public void BasicEnumTest_SByte()
        {
            this.EnumTest<sbyte>("int8");
            this.EnumTest<sbyte>("byte");
        }

        [TestMethod]
        public void BasicEnumTest_Short()
        {
            this.EnumTest<short>("int16");
            this.EnumTest<short>("short");
        }

        [TestMethod]
        public void BasicEnumTest_UShort()
        {
            this.EnumTest<ushort>("uint16");
            this.EnumTest<ushort>("ushort");
        }

        [TestMethod]
        public void BasicEnumTest_Int()
        {
            this.EnumTest<int>("int32");
            this.EnumTest<int>("int");
        }

        [TestMethod]
        public void BasicEnumTest_UInt()
        {
            this.EnumTest<uint>("uint32");
            this.EnumTest<uint>("uint");
        }

        [TestMethod]
        public void BasicEnumTest_Long()
        {
            this.EnumTest<long>("int64");
            this.EnumTest<long>("long");
        }

        [TestMethod]
        public void BasicEnumTest_ULong()
        {
            this.EnumTest<ulong>("uint64");
            this.EnumTest<ulong>("ulong");
        }

        [TestMethod]
        public void FlagsTest_OK()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ Red, Blue, Green, Yellow }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");

            Assert.IsNotNull(t.GetCustomAttribute(typeof(FlagsAttribute)));
            Assert.IsTrue(t.IsEnum);
            Assert.AreEqual(typeof(byte), Enum.GetUnderlyingType(t));
            Assert.IsTrue(t.GetCustomAttribute<FlatSharp.Attributes.FlatBufferEnumAttribute>() != null);

            string[] names = Enum.GetNames(t);
            Assert.AreEqual(6, names.Length);
            Assert.AreEqual("None", names[0]);
            Assert.AreEqual("Red", names[1]);
            Assert.AreEqual("Blue", names[2]);
            Assert.AreEqual("Green", names[3]);
            Assert.AreEqual("Yellow", names[4]);
            Assert.AreEqual("All", names[5]);

            Array values = Enum.GetValues(t);
            Assert.AreEqual(0, (byte)values.GetValue(0));  // none
            Assert.AreEqual(1, (byte)values.GetValue(1));
            Assert.AreEqual(2, (byte)values.GetValue(2));
            Assert.AreEqual(4, (byte)values.GetValue(3));
            Assert.AreEqual(8, (byte)values.GetValue(4));
            Assert.AreEqual(15, (byte)values.GetValue(5)); // all
        }

        [TestMethod]
        public void FlagsTest_TooMany()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ A, B, C, D, E, F, G, H, I }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Could not format value for enum 'MyEnum'. Value = 256. Make sure that the enum type has enough space for this many flags."));
        }

        [TestMethod]
        public void FlagsTest_ExplicitValuesDisallowed()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte (bit_flags) {{ A = 3, B, C }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Enum 'MyEnum' declares the 'bit_flags' attribute. FlatSharp does not support specifying explicit values when used in conjunction with bit flags."));
        }

        [TestMethod]
        public void InvalidEnumTest_WrongUnderlyingType_Bool()
        {
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => this.EnumTest<bool>("bool")); 
            Assert.IsTrue(ex.Message.Contains("mismatched input 'bool' expecting {'byte', 'ubyte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'int8', 'uint8', 'int16', 'uint16', 'int32', 'uint32', 'int64', 'uint64'}'"));
        }

        [TestMethod]
        public void InvalidEnumTest_WrongUnderlyingType_Double()
        {
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => this.EnumTest<double>("double"));
            Assert.IsTrue(ex.Message.Contains("mismatched input 'double' expecting {'byte', 'ubyte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'int8', 'uint8', 'int16', 'uint16', 'int32', 'uint32', 'int64', 'uint64'}'"));
        }

        [TestMethod]
        public void InvalidEnumTest_WrongUnderlyingType_Float()
        {
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => this.EnumTest<float>("float"));
            Assert.IsTrue(ex.Message.Contains("mismatched input 'float' expecting {'byte', 'ubyte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'int8', 'uint8', 'int16', 'uint16', 'int32', 'uint32', 'int64', 'uint64'}'"));
        }

        [TestMethod]
        public void InvalidEnumTest_DuplicateValues()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 0X10, Yellow = 16 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Enum 'MyEnum' contains duplicate value '16'."));
        }

        [TestMethod]
        public void InvalidEnumTest_NonAscendingValues()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 3, Yellow = 2 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Enum 'MyEnum' must declare values sorted in ascending order."));
        }

        [TestMethod]
        public void InvalidEnumTest_ValueOutOfRangeOfUnderlyingType_Above()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 255, Yellow = 256 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Could not format value '256' as a System.Byte for enum 'MyEnum'. Make sure the value is expressable as 'System.Byte'"));
        }

        [TestMethod]
        public void InvalidEnumTest_ValueOutOfRangeOfUnderlyingType_Below()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ulong {{ Red = -1, Blue = 255, Yellow = 256 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("Could not format value '-1' as a System.UInt64 for enum 'MyEnum'. Make sure the value is expressable as 'System.UInt64'"));
        }

        [TestMethod]
        public void InvalidEnumTest_UnknownType()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : foobar {{ Red = 0x0, Blue = 255, Yellow = 256 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("mismatched input 'foobar' expecting {'byte', 'ubyte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'int8', 'uint8', 'int16', 'uint16', 'int32', 'uint32', 'int64', 'uint64'}'"));
        }

        [TestMethod]
        public void InvalidEnumTest_String()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : string {{ Red, Blue, Yellow }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("mismatched input 'string' expecting {'byte', 'ubyte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'int8', 'uint8', 'int16', 'uint16', 'int32', 'uint32', 'int64', 'uint64'}'"));
        }

        [TestMethod]
        public void InvalidEnumTest_NotAnInt()
        {
            // Quirk of the grammar allows this.
            string fbs = $"namespace Foo.Bar; enum MyEnum : uint {{ Red = 1.2 }}";
            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(fbs, new()));
            Assert.IsTrue(ex.Message.Contains("mismatched input '1.2' expecting {HEX_INTEGER_CONSTANT, INTEGER_CONSTANT}"));
        }

        [TestMethod]
        public void BasicEnumTest_HexNumber()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : ubyte {{ Red = 0x0, Blue = 0X01, Green = 2, Yellow = 0X10 }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");
            Assert.IsTrue(t.IsEnum);
            Assert.AreEqual(typeof(byte), Enum.GetUnderlyingType(t));

            Array values = Enum.GetValues(t);

            Assert.AreEqual((byte)0, (byte)values.GetValue(0));
            Assert.AreEqual((byte)1, (byte)values.GetValue(1));
            Assert.AreEqual((byte)2, (byte)values.GetValue(2));
            Assert.AreEqual((byte)16, (byte)values.GetValue(3));
        }

        [TestMethod]
        public void BasicEnumTest_NegativeNumbers()
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : byte {{ Red = -0x02, Blue = -1, Green = 0, Yellow = 0X1, Purple = 0x02 }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");
            Assert.IsTrue(t.IsEnum);
            Assert.AreEqual(typeof(sbyte), Enum.GetUnderlyingType(t));

            var values = Enum.GetValues(t).OfType<object>().Select(x => new { Name = x.ToString(), Value = (sbyte)x });

            Assert.AreEqual((sbyte)-2, values.Single(x => x.Name == "Red").Value);
            Assert.AreEqual((sbyte)-1, values.Single(x => x.Name == "Blue").Value);
            Assert.AreEqual((sbyte)0, values.Single(x => x.Name == "Green").Value);
            Assert.AreEqual((sbyte)1, values.Single(x => x.Name == "Yellow").Value);
            Assert.AreEqual((sbyte)2, values.Single(x => x.Name == "Purple").Value);
        }

        public void EnumTest<T>(string flatBufferType) where T : struct
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : {flatBufferType} {{ Red, Blue = 3, Green, Yellow }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new());

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");

            Assert.IsNull(t.GetCustomAttribute(typeof(FlagsAttribute)));
            Assert.IsTrue(t.IsEnum);
            Assert.AreEqual(typeof(T), Enum.GetUnderlyingType(t));
            Assert.IsTrue(t.GetCustomAttribute<FlatSharp.Attributes.FlatBufferEnumAttribute>() != null);

            string[] names = Enum.GetNames(t);
            Assert.AreEqual(4, names.Length);
            Assert.AreEqual("Red", names[0]);
            Assert.AreEqual("Blue", names[1]);
            Assert.AreEqual("Green", names[2]);
            Assert.AreEqual("Yellow", names[3]);

            Array values = Enum.GetValues(t);

            Assert.AreEqual(Convert.ChangeType(0, typeof(T)), (T)values.GetValue(0));
            Assert.AreEqual(Convert.ChangeType(3, typeof(T)), (T)values.GetValue(1));
            Assert.AreEqual(Convert.ChangeType(4, typeof(T)), (T)values.GetValue(2));
            Assert.AreEqual(Convert.ChangeType(5, typeof(T)), (T)values.GetValue(3));
        }
    }
}
