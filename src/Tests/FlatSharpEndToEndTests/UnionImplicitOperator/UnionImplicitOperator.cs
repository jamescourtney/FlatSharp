namespace FlatSharpEndToEndTests.UnionImplicitOperator;

[TestClass]
public class UnionImplicitOperatorTests
{
    [TestMethod]
    public void ImplicitOperator_Struct()
    {
        A a = new A { V = 1 };

        StructUnion u = a;

        Assert.AreEqual(u.GetType(), typeof(StructUnion));
        Assert.AreEqual(a.V, u.A.V);
    }

    [TestMethod]
    public void ImplicitOperator_ValueStruct()
    {
        Vec3 v = new Vec3 { X = 1, Y = 2, Z = 3 };

        StructUnion u = v;

        Assert.AreEqual(u.GetType(), typeof(StructUnion));
        Assert.AreEqual(v.X, u.Vec3.X);
    }

    [TestMethod]
    public void ImplicitOperator_Table()
    {
        MyTable t = new MyTable { Field = "hello", Value = 42 };

        TableUnion u = t;

        Assert.AreEqual(u.GetType(), typeof(TableUnion));
        Assert.AreEqual(t.Field, u.MyTable.Field);
        Assert.AreEqual(t.Value, u.MyTable.Value);
    }

    [TestMethod]
    public void ImplicitOperator_Vector()
    {
        VectorTable vt = new VectorTable { InnerVector = new[] { "A", "B", "C" } };

        TableUnion u = vt;

        Assert.AreEqual(u.GetType(), typeof(TableUnion));
        Assert.AreEqual(vt.InnerVector, u.VectorTable.InnerVector);
    }
}
