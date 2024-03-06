using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class StringVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void StringTableField(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        IList<string> theBoys = vectors.Str;
        Assert.AreEqual(3, theBoys.Count);
        Assert.AreEqual("billy", theBoys[0]);
        Assert.AreEqual("mm", theBoys[1]);
        Assert.AreEqual("frenchie", theBoys[2]);

        Helpers.AssertMutationWorks(option, vectors, false, s => s.Str, new string[0]);
        Helpers.ValidateListVector(option, false, theBoys, string.Empty);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    });

    [TestMethod]
    public void WithNull() => Helpers.Repeat(() =>
    {
        Root root = new()
        {
            Vectors = new()
            {
                Str = Helpers.CreateList((string)null),
            }
        };

        var ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.Write(new byte[1024], root));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);

        ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.GetMaxSize(root));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);
    });

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Big(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = new Root { Vectors = new() { Str = new List<string>() } };

        for (int i = 0; i < 1000; ++i)
        {
            root.Vectors.Str.Add(i.ToString());
        }

        Root parsed = root.SerializeAndParse(option, out byte[] actualData);
        IList<string> parsedList = parsed.Vectors.Str;

        for (int i = 0; i < 1000; ++i)
        {
            Assert.AreEqual(root.Vectors.Str[i], parsedList[i]);

            Action<string, string> assert = option switch
            {
                FlatBufferDeserializationOption.Lazy => Assert.AreNotSame,
                _ => Assert.AreSame,
            };

            assert(parsedList[i], parsedList[i]);
        }
    });

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Vectors = new()
            {
                Str = Helpers.CreateList("billy", "mm", "frenchie")
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
            16, 0, 0, 0,        // uoffset to vector

            12, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            4, 0,               // field 3 (str)

            3, 0, 0, 0,         // vector length
            12, 0, 0, 0,
            20, 0, 0, 0,
            24, 0, 0, 0,

            5, 0, 0, 0,
            B('b'), B('i'), B('l'), B('l'), B('y'), 0,
            0, 0,

            2, 0, 0, 0,
            B('m'), B('m'), 0, 0,

            8, 0, 0, 0,
            B('f'), B('r'), B('e'), B('n'), 
            B('c'), B('h'), B('i'), B('e'),
            0,
        };

        return root;
    }
}
