using FlatSharp.Internal;

namespace FlatSharpEndToEndTests.ToStringMethods;

[TestClass]
public class ToStringTests
{
    [TestMethod]
    public void Table_ToString()
    {
        Assert.AreEqual("MyTable { FieldA = hello, FieldB = 123 }", new MyTable { FieldA = "hello", FieldB = 123}.ToString());
    }

    [TestMethod]
    public void EmptyTable_ToString()
    {
        Assert.AreEqual("MyEmptyTable { }", new MyEmptyTable().ToString());
    }

    [TestMethod]
    public void Struct_ToString()
    {
        Assert.AreEqual("MyStruct { FieldA = 456, FieldB = 123 }", new MyStruct { FieldA = 456, FieldB = 123}.ToString());
    }

    [TestMethod]
    public void ValueStruct_ToString()
    {
        Assert.AreEqual("MyValueStruct { FieldX = 1, FieldY = 2 }", new MyValueStruct { FieldX = 1f, FieldY = 2f}.ToString());
    }

    [TestMethod]
    public void UnionStructs_ToString()
    {
        Assert.AreEqual("StructUnion { A { V = 0 } }", new StructUnion(new A { V = 0 }).ToString());
        Assert.AreEqual("StructUnion { B { V = 1 } }", new StructUnion(new B { V = 1 }).ToString());
        Assert.AreEqual("StructUnion { C { V = 2 } }", new StructUnion(new C { V = 2 }).ToString());
        Assert.AreEqual("StructUnion { D { V = 3 } }", new StructUnion(new D { V = 3 }).ToString());
    }

    [TestMethod]
    public void UnionTables_ToString()
    {
        Assert.AreEqual("TableUnion { MyTable { FieldA = hello, FieldB = 10 } }", new TableUnion(new MyTable { FieldA = "hello", FieldB = 10 }).ToString());
        Assert.AreEqual("TableUnion { MyEmptyTable { } }", new TableUnion(new MyEmptyTable()).ToString());
    }

    [TestMethod]
    public void UnionMixed_ToString()
    {
        Assert.AreEqual("MixedUnion { A { V = 0 } }", new MixedUnion(new A { V = 0 }).ToString());
        Assert.AreEqual("MixedUnion { A { V = 2 } }", new MixedUnion(new A { V = 2 }).ToString());
        Assert.AreEqual("MixedUnion { B { V = 0 } }", new MixedUnion(new B { V = 0 }).ToString());
        Assert.AreEqual("MixedUnion { MyTable { FieldA = hi, FieldB = 21 } }", new MixedUnion(new MyTable { FieldA = "hi", FieldB = 21 }).ToString());
        Assert.AreEqual("MixedUnion { MyEmptyTable { } }", new MixedUnion(new MyEmptyTable()).ToString());

    }
}