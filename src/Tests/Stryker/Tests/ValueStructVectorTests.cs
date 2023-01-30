using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

public class ValueStructVectorTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Present(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] actualData);

        Assert.NotNull(parsed.Vectors.ValueStruct);

        var vsr = root.Vectors.ValueStruct;
        var vsp = parsed.Vectors.ValueStruct;

        Assert.Equal(vsr.Count, vsp.Count);

        for (int i = 0; i < vsr.Count; ++i)
        {
            var r = vsr[i];
            var p = vsp[i];

            Assert.Equal(r.A, p.A);
            Assert.Equal(r.B, p.B);
            Assert.Equal(r.C(0), p.C(0));
            Assert.Equal(r.C(1), p.C(1));
        }

        Helpers.AssertSequenceEqual(expectedData, actualData);
        Helpers.AssertMutationWorks(option, parsed.Vectors, false, v => v.ValueStruct, new List<ValueStruct>());
        Helpers.ValidateListVector(option, true, vsp, new ValueStruct());
    });

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Missing(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_Missing(out byte[] expectedData);
        root.SerializeAndParse(option, out byte[] actualData);

        Assert.Null(root.Vectors.ValueStruct);
        Helpers.AssertSequenceEqual(expectedData, actualData);
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        var vs1 = new ValueStruct { A = 1, B = 2 };
        var vs2 = new ValueStruct { A = 5, B = 6 };

        vs1.C(0) = 3;
        vs1.C(1) = 4;
        vs2.C(0) = 7;
        vs2.C(1) = 8;

        Root root = new()
        {
            Vectors = new()
            {
                ValueStruct = Helpers.CreateList<ValueStruct>(vs1, vs2)
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

            8, 0, 0, 0,         // soffset to vtable (shared)
            4, 0, 0, 0,         // uoffset to vector

            2, 0, 0, 0,

            1, 0, 0, 0,
            2, 0, 3, 0,
            4, 0, 0, 0,

            5, 0, 0, 0,
            6, 0, 7, 0,
            8, 0, 0, 0,
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
