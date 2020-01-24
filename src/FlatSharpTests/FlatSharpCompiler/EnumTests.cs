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
            this.EnumTest<byte>("ubyte");
        }

        [TestMethod]
        public void BasicEnumTest_SByte()
        {
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
        public void InvalidEnumTest_WrongUnderlyingType_Bool()
        {
            Assert.ThrowsException<InvalidFbsFileException>(() => this.EnumTest<bool>("bool"));
        }

        [TestMethod]
        public void InvalidEnumTest_WrongUnderlyingType_Double()
        {
            Assert.ThrowsException<FlatSharpCompilationException>(() => this.EnumTest<double>("double"));
        }

        [TestMethod]
        public void InvalidEnumTest_WrongUnderlyingType_Float()
        {
            Assert.ThrowsException<FlatSharpCompilationException>(() => this.EnumTest<float>("float"));
        }

        public void EnumTest<T>(string flatBufferType) where T : struct
        {
            string fbs = $"namespace Foo.Bar; enum MyEnum : {flatBufferType} {{ Red, Blue = 3, Green, Yellow }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs);

            Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.MyEnum");

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

            Assert.AreEqual(Convert.ChangeType(1, typeof(T)), (T)values.GetValue(0));
            Assert.AreEqual(Convert.ChangeType(3, typeof(T)), (T)values.GetValue(1));
            Assert.AreEqual(Convert.ChangeType(4, typeof(T)), (T)values.GetValue(2));
            Assert.AreEqual(Convert.ChangeType(5, typeof(T)), (T)values.GetValue(3));
        }
    }
}
