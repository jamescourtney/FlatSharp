namespace FlatSharpStrykerTests;

[TestClass]
public class RootObjectTests
{
    [TestMethod]
    public void InvalidConstructors()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((string)null));
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((RefStruct)null));
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((Key)null));
    }
}
