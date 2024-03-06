using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class IndexedVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Present(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        IIndexedVector<string, Key> vector = vectors.Indexed;

        Assert.AreEqual(option != FlatBufferDeserializationOption.GreedyMutable, vector.IsReadOnly);

        var expected = root.Vectors.Indexed.ToArray();

        foreach (var pair in expected)
        {
            Fruit f = pair.Value.Value;
            string name = pair.Value.Name;

            Assert.IsTrue(vector.TryGetValue(name, out Key key));
            Assert.AreEqual(f, key.Value);
            Assert.AreEqual(name, key.Name);

            key = vector[name];
            Assert.AreEqual(f, key.Value);

            Assert.IsTrue(vector.ContainsKey(name));
        }

        Assert.IsFalse(vector.ContainsKey("z"));
        Assert.IsFalse(vector.TryGetValue("z", out _));

        Helpers.AssertMutationWorks(option, vectors, false, v => v.Indexed, new IndexedVector<string, Key>());
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        Assert.IsNull(vectors.Indexed);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void KeyNotSet_Serialize(FlatBufferDeserializationOption option)
    {
        Key key = new Key { Name = "fred", Value = Fruit.Apple };

        Root root = new()
        {
            Vectors = new()
            {
                Indexed = new IndexedVector<string, Key>
                {
                    { key }
                }
            }
        };

        key.Name = null!;

        var ex = Assert.ThrowsException<InvalidOperationException>(() => root.SerializeAndParse(option));
        Assert.AreEqual("Table property 'FlatSharpStrykerTests.Key.Name' is marked as required, but was not set.", ex.Message);
    }

    [TestMethod]
    public void NullItem_Serialize()
    {
        Root root = new() 
        { 
            Vectors = new()
            {
                Indexed = new IndexedVector<string, Key>()
            } 
        };

        // shoudln't be possible, but let's test our defensiveness.
        var backingDictionary = (Dictionary<string, Key>)typeof(IndexedVector<string, Key>).GetField("backingDictionary", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(root.Vectors.Indexed);
        backingDictionary["foo"] = null;

        var ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.GetMaxSize(root));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);

        ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.Write(new byte[1024], root));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void KeyNotSet_Parse(FlatBufferDeserializationOption option)
    {
        // missing required field.
        byte[] data =
        {
            4, 0, 0, 0,
            248, 255, 255, 255,
            12, 0, 0, 0,

            6, 0, 8, 0,
            4, 0, 0, 0,

            246, 255, 255, 255,
            24, 0, 0, 0,
            4, 0,

            16, 0, 9, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            8, 0, 4, 0,

            0, 0,

            248, 255, 255, 255,
            0, 0, 0, 0,

            8, 0, 8, 0,
            0, 0, 4, 0,
        };

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            var root = Root.Serializer.Parse(data, option);
            string name = root.Fields.Union.Value.Key.Name;
        });

        Assert.AreEqual("Table property 'FlatSharpStrykerTests.Key.Name' is marked as required, but was missing from the buffer.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Big(FlatBufferDeserializationOption option)
    {
        Root root = new Root
        {
            Vectors = new()
            {
                Indexed = new IndexedVector<string, Key>(),
            }
        };

        List<string> expectedKeys = new();
        for (int i = 0; i < 1000; ++i)
        {
            Key key = new() { Name = Guid.NewGuid().ToString("n"), Value = Fruit.Apple };
            expectedKeys.Add(key.Name);
            root.Vectors.Indexed.Add(key);
        }

        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        var parsedIndexed = parsed.Vectors.Indexed;

        for (int i = 0; i < expectedKeys.Count; ++i)
        {
            Assert.IsTrue(parsedIndexed.ContainsKey(expectedKeys[i]));
            Assert.IsNotNull(parsedIndexed[expectedKeys[i]]);
            Assert.IsTrue(parsedIndexed.TryGetValue(expectedKeys[i], out Key key));
            Assert.IsNotNull(key);
            Assert.AreEqual(expectedKeys[i], key.Name);
        }

        for (int i = 0; i < expectedKeys.Count; ++i)
        {
            string unexpectedKey = Guid.NewGuid().ToString("n");

            Assert.IsFalse(parsedIndexed.ContainsKey(unexpectedKey));
            Assert.ThrowsException<KeyNotFoundException>(() => parsedIndexed[unexpectedKey]);
            Assert.IsFalse(parsedIndexed.TryGetValue(unexpectedKey, out Key key));
            Assert.IsNull(key);
        }
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
