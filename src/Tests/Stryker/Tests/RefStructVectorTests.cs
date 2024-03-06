using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class RefStructVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Present(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] actualData);

        Assert.IsNotNull(root.Vectors.RefStruct);

        var vsr = root.Vectors.RefStruct;
        var vsp = parsed.Vectors.RefStruct;

        Assert.AreEqual(vsr.Count, vsp.Count);

        for (int i = 0; i < vsr.Count; ++i)
        {
            var r = vsr[i];
            var p = vsp[i];

            Assert.AreEqual(r.A, p.A);
            Assert.AreEqual(r.B, p.B);
            Assert.AreEqual(r.C[0], p.C[0]);
            Assert.AreEqual(r.C[1], p.C[1]);
            Assert.AreEqual(r.D[0], p.D[0]);
            Assert.AreEqual(r.D[1], p.D[1]);

            Assert.AreEqual(r.C.ToArray()[0], p.C.ToArray()[0]);
            Assert.AreEqual(r.D.ToArray()[0], p.D.ToArray()[0]);

            Assert.AreSame(p.D, p.D);
            Assert.AreSame(p.C, p.C);
        }

        Helpers.AssertSequenceEqual(expectedData, actualData);
        Helpers.AssertMutationWorks(option, parsed.Vectors, false, v => v.RefStruct, new List<RefStruct>());
        Helpers.ValidateListVector(option, false, vsp, new RefStruct());
    });

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        root.SerializeAndParse(option, out byte[] actualData);

        Assert.IsNull(root.Vectors.RefStruct);
        Helpers.AssertSequenceEqual(expectedData, actualData);
    }

    [TestMethod]
    public void WithNull() => Helpers.Repeat(() =>
    {
        Root root = new()
        {
            Vectors = new()
            {
                RefStruct = Helpers.CreateList((RefStruct)null),
            }
        };

        var ex = Assert.ThrowsException<InvalidDataException>(() => Root.Serializer.Write(new byte[1024], root));
        Assert.AreEqual("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.", ex.Message);

        // no exception here. Reason is that structs are constant size, so we don't traverse the vector to figure out the max size.
        int maxSize = Root.Serializer.GetMaxSize(root);
    });

    private Root CreateRoot(out byte[] expectedData)
    {
        var rs1 = new RefStruct { A = 1, B = Fruit.Banana };
        rs1.C.CopyFrom(new sbyte[] { 3, 4 }.ToList());
        rs1.D.CopyFrom(new sbyte[] { 5, 6, }.AsSpan());

        var rs2 = new RefStruct { A = 7, B = Fruit.Pear };
        rs2.C.CopyFrom(new sbyte[] { 9, 10 }.AsSpan());
        rs2.D.CopyFrom(new List<sbyte> { 11, 12, });

        Root root = new()
        {
            Vectors = new()
            {
                RefStruct = Helpers.CreateList(rs1, rs2)
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

            248, 255, 255, 255, // soffset to vtable (shared)
            12, 0, 0, 0,        // uoffset to vector

            6, 0, 8, 0,         // vtable
            4, 0, 0, 0, 

            2, 0, 0, 0,

            1, 0, 0, 0,
            0, 0, 0, 0,
            1, 0, 0, 0,
            3, 4, 5, 6,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            7, 0, 0, 0,
            0, 0, 0, 0,
            3, 0, 0, 0,
            9, 10, 11, 12,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
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
            4, 0,              // table length
        };

        return root;
    }
}
