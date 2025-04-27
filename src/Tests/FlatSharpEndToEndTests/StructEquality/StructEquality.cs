using FlatSharpEndToEndTests.Unions;

namespace FlatSharpEndToEndTests.StructEquality;

[TestClass]
public class StructEqualityTests
{
    [TestMethod]
    public void ToTupleMethod()
    {
        MixedValueStruct mixedValueStruct = new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 };

        var expectedValueStructTuple = (1, 2f, (ushort)5);

        Assert.AreEqual(expectedValueStructTuple, mixedValueStruct.ToTuple());
    }

    [TestMethod]
    public void EqualsMethod()
    {
        MixedValueStruct mixedValueStruct = new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 };
        StructUnion structUnion = new StructUnion(new A { V = 1 });

        var mismatchedStruct = new MixedValueStruct { X = 20 };
        var mismatchedUnion = new StructUnion(new B { V = 1 });
        var mismatchedObject = "hi";

        Assert.IsTrue(mixedValueStruct.Equals(new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 }));
        Assert.IsFalse(mixedValueStruct.Equals(mismatchedStruct));
        Assert.IsFalse(mixedValueStruct.Equals(mismatchedObject));
        Assert.IsTrue(structUnion.Equals(new StructUnion(new A { V = 1 })));
        Assert.IsFalse(structUnion.Equals(mismatchedUnion));
        Assert.IsFalse(mixedValueStruct.Equals(mismatchedObject));
    }

    [TestMethod]
    public void EqualityOperator()
    {
        MixedValueStruct mixedValueStruct = new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 };
        StructUnion structUnion = new StructUnion(new A { V = 1 });

        var mismatchedStruct = new MixedValueStruct { X = 10 };
        var mismatchedUnion = new StructUnion(new B { V = 21 });

        Assert.IsTrue(mixedValueStruct == new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 });
        Assert.IsFalse(mixedValueStruct == mismatchedStruct);
        Assert.IsTrue(structUnion == new StructUnion(new A { V = 1 }));
        Assert.IsFalse(structUnion == mismatchedUnion);
    }

    [TestMethod]
    public void InequalityOperator()
    {
        MixedValueStruct mixedValueStruct = new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 };
        StructUnion structUnion = new StructUnion(new A { V = 1 });

        var mismatchedStruct = new MixedValueStruct { Y = 13f };
        var mismatchedUnion = new StructUnion(new C { V = 42 });
        var mirrorStruct = mixedValueStruct;
        var mirrorUnion = structUnion;

        Assert.IsTrue(mixedValueStruct != mismatchedStruct);
        Assert.IsFalse(mixedValueStruct != mirrorStruct);
        Assert.IsTrue(structUnion != mismatchedUnion);
        Assert.IsFalse(structUnion != mirrorUnion);
    }

    [TestMethod]
    public void GetHashCodeMethod()
    {
        MixedValueStruct mixedValueStruct = new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 };
        StructUnion structUnion = new StructUnion(new A { V = 1 });

        Assert.AreEqual(new MixedValueStruct { X = 1, Y = 2f, Z = (ushort)5 }.GetHashCode(), mixedValueStruct.GetHashCode());
        Assert.AreEqual(new StructUnion(new A { V = 1 }).GetHashCode(), structUnion.GetHashCode());
    }
}