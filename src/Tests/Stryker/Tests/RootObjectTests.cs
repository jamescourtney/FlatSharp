namespace FlatSharpStrykerTests;

public class RootObjectTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void InvalidConstructors(FlatBufferDeserializationOption option)
    {
        Assert.Throws<ArgumentNullException>(() => new FunUnion((string)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((RefStruct)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((Key)null));
    }
}
