namespace FlatSharpStrykerTests;

public class RootObjectTests
{
    [Fact]
    public void InvalidConstructors()
    {
        Assert.Throws<ArgumentNullException>(() => new FunUnion((string)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((RefStruct)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((Key)null));
    }
}
