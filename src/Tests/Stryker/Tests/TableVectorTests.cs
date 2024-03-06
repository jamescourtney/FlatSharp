using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class TableVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Present(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        IList<Key> vector = vectors.Table;
        IList<Key> expected = root.Vectors.Table;

        Assert.AreEqual(expected.Count, vector.Count);

        for (int i = 0; i < expected.Count; ++i)
        {
            var item = vector[i];
            var e = expected[i];

            Assert.AreEqual(e.Name, item.Name);
            Assert.AreEqual(e.Value, item.Value);
        }

        Helpers.AssertSequenceEqual(expectedData, buffer);
        Helpers.ValidateListVector(option, false, vector, new Key { Name = "johnny" });
    });

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        Assert.IsNull(vectors.Table);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    public void WithNull() => Helpers.Repeat(() =>
    {
        var ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.Write(new byte[1024], CreateRoot_WithNullItem()));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);

        ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.GetMaxSize(CreateRoot_WithNullItem()));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);
    });

    [TestMethod]
    public void ProgressiveSingleRead() => Helpers.Repeat(() =>
    {
        Root root = new() { Vectors = new() { Table = new[] { new Key() { Name = "foo", Value = Fruit.Strawberry } } } };
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        Key item = parsed.Vectors.Table[0];

        Assert.AreEqual("foo", item.Name);
        Assert.AreSame(item.Name, item.Name);
        Assert.AreEqual(Fruit.Strawberry, item.Value);

        buffer.AsSpan().Clear();

        Assert.AreEqual(Fruit.Strawberry, item.Value);
    });

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Vectors = new()
            {
                Table = Helpers.CreateList(
                    new Key { Name = "a", Value = Fruit.Apple },
                    new Key { Name = "b", Value = Fruit.Banana },
                    new Key { Name = "p", Value = Fruit.Pear },
                    new Key { Name = "s", Value = Fruit.Strawberry }),
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

            20, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            0, 0,               // field 4, 5 (union)
            0, 0,
            0, 0,               // field 6 (indexed)
            4, 0,               // field 7 (table)

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

    private Root CreateRoot_WithNullItem()
    {
        return new()
        {
            Vectors = new()
            {
                Table = Helpers.CreateList((Key)null),
            }
        };
    }
}
