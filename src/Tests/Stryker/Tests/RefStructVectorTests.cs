using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

public class RefStructVectorTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Present(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] actualData);

        Assert.NotNull(root.Vectors.RefStruct);

        var vsr = root.Vectors.RefStruct;
        var vsp = parsed.Vectors.RefStruct;

        Assert.Equal(vsr.Count, vsp.Count);

        for (int i = 0; i < vsr.Count; ++i)
        {
            var r = vsr[i];
            var p = vsp[i];

            Assert.Equal(r.A, p.A);
            Assert.Equal(r.B, p.B);
            Assert.Equal(r.C[0], p.C[0]);
            Assert.Equal(r.C[1], p.C[1]);
            Assert.Equal(r.D[0], p.D[0]);
            Assert.Equal(r.D[1], p.D[1]);
        }

        Helpers.AssertSequenceEqual(expectedData, actualData);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        root.SerializeAndParse(option, out byte[] actualData);

        Assert.Null(root.Vectors.RefStruct);
        Helpers.AssertSequenceEqual(expectedData, actualData);
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        var rs1 = new RefStruct { A = 1, B = Fruit.Banana };
        rs1.C.CopyFrom(new sbyte[] { 3, 4 }.AsSpan());
        rs1.D.CopyFrom(new List<sbyte> { 5, 6, });

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
