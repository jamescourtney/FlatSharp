using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

public class IndexedVectorTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Present(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.True(vectors.IsInitialized);
        Assert.True(Vectors.IsStaticInitialized);

        IIndexedVector<string, Key> vector = vectors.Indexed;

        var expected = root.Vectors.Indexed.ToArray();

        foreach (var pair in expected)
        {
            Fruit f = pair.Value.Value;
            string name = pair.Value.Name;

            Assert.True(vector.TryGetValue(name, out Key key));
            Assert.Equal(f, key.Value);
            Assert.Equal(name, key.Name);

            key = vector[name];
            Assert.Equal(f, key.Value);

            Assert.True(vector.ContainsKey(name));
        }

        Assert.False(vector.ContainsKey("z"));
        Assert.False(vector.TryGetValue("z", out _));

        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.True(vectors.IsInitialized);
        Assert.True(Vectors.IsStaticInitialized);

        Assert.Null(vectors.Indexed);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        var vector = new IndexedVector<string, Key>
        {
            new Key { Name = "a", Value = Fruit.Apple },
            new Key { Name = "b", Value = Fruit.Banana },
            new Key { Name = "p", Value = Fruit.Pear },
            new Key { Name = "s", Value = Fruit.Strawberry },
        };

        Root root = new()
        {
            Vectors = new()
            {
                Indexed = vector,
            }
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            248, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to vector

            18, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            0, 0,               // field 4, 5 (union)
            0, 0,
            4, 0,               // field 6 (indexed)
            0, 0,

            4, 0, 0, 0,         // vector length
            16, 0, 0, 0,
            40, 0, 0, 0,
            60, 0, 0, 0,
            76, 0, 0, 0,

            244, 255, 255, 255, // soffset to vtable.
            0, 0, 0, 0,         // apple
            12, 0, 0, 0,        // offset to 'a'

            8, 0, 12, 0,        // vtable
            8, 0, 4, 0,

            1, 0, 0, 0,
            B('a'), 0, 0, 0,   

            248, 255, 255, 255,  // non-shared vtable. (banana is default)
            12, 0, 0, 0, 

            6, 0, 8, 0,          // vtable
            4, 0, 0, 0,

            1, 0, 0, 0,
            B('b'), 0, 0, 0,

            40, 0, 0, 0,        // shares vtable
            3, 0, 0, 0,         // pear
            4, 0, 0, 0,

            1, 0, 0, 0,
            B('p'), 0, 0, 0,

            60, 0, 0, 0,        // shares vtable
            2, 0, 0, 0,         // strawberry
            4, 0, 0, 0,

            1, 0, 0, 0,
            B('s'), 0,
        };

        return root;
    }


    private Root CreateRoot_Missing(out byte[] expectedData)
    {
        Root root = new()
        {
            Vectors = new()
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            252, 255, 255, 255, // soffset to vtable

            4, 0,              // vtable length
            4, 0,               // table length
        };

        return root;
    }
}
