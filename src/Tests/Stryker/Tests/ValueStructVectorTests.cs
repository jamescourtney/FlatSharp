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

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Big(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = new Root { Vectors = new() { ValueStruct = new List<ValueStruct>() } };

        for (int i = 0; i < 1000; ++i)
        {
            root.Vectors.ValueStruct.Add(new ValueStruct { A = i });
        }

        Root parsed = root.SerializeAndParse(option, out byte[] actualData);
        IList<ValueStruct> parsedList = parsed.Vectors.ValueStruct;

        for (int i = 0; i < 1000; ++i)
        {
            Assert.Equal(root.Vectors.ValueStruct[i].A, parsedList[i].A);
        }
    });

    [Theory]
    [InlineData(FlatBufferDeserializationOption.Lazy)]
    [InlineData(FlatBufferDeserializationOption.Progressive)]
    public void WriteThroughValidation(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = new Root { Vectors = new() { ValueStruct = new List<ValueStruct> { new ValueStruct { A = 1 }, new ValueStruct { A = 2 } } } };
        Root parsed1 = root.SerializeAndParse(option, out byte[] rawData);

        Assert.Equal(1, parsed1.Vectors.ValueStruct[0].A);
        Assert.Equal(2, parsed1.Vectors.ValueStruct[1].A);

        parsed1.Vectors.ValueStruct[0] = new() { A = 5 };
        parsed1.Vectors.ValueStruct[1] = new() { A = 10 };

        Assert.Throws<IndexOutOfRangeException>(() => parsed1.Vectors.ValueStruct[-1] = default);

        Assert.Equal(5, parsed1.Vectors.ValueStruct[0].A);
        Assert.Equal(10, parsed1.Vectors.ValueStruct[1].A);

        Root parsed2 = Root.Serializer.Parse(rawData, option);
        Assert.Equal(5, parsed2.Vectors.ValueStruct[0].A);
        Assert.Equal(10, parsed2.Vectors.ValueStruct[1].A);
    });

    [Fact]
    public void ProgressiveClear()
    {
        Root root = new Root { Vectors = new() { ValueStruct = new[] { new ValueStruct { A = 1, B = 2 } } } };

        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var buffer);

        var vectors = parsed.Vectors.ValueStruct;

        // Load item 0 into memory.
        Assert.Equal(1, vectors[0].A);
        Assert.Equal(2, vectors[0].B);

        // Clear the span.
        buffer.AsSpan().Clear();

        // Verify item 0 is still cached.
        Assert.Equal(1, vectors[0].A);
        Assert.Equal(2, vectors[0].B);
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
