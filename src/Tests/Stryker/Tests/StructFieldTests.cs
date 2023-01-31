using FlatSharp.Internal;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

public class StructFieldTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ValueStructTableField(FlatBufferDeserializationOption option)
    {
        try
        {
            for (int i = 0; i < 2; ++i)
            {
                MockBitConverter.IsLittleEndian = i == 0;

                ValueStruct vs = new()
                {
                    A = 4,
                    B = 3,
                };

                vs.C(0) = 1;
                vs.C(1) = 2;

                Assert.Throws<IndexOutOfRangeException>(() => vs.C(2) = 4);
                Assert.Throws<IndexOutOfRangeException>(() => vs.C(-1) = 4);

                Root root = new Root
                {
                    Fields = new()
                    {
                        ValueStruct = vs,
                    }
                };

                Root parsed = root.SerializeAndParse(option, out byte[] buffer);
                Fields fields = parsed.Fields;

                Assert.NotNull(fields);
                Assert.Null(parsed.Vectors);

                Assert.NotNull(parsed.Fields.ValueStruct);

                ValueStruct vsParsed = parsed.Fields.ValueStruct.Value;

                Assert.Equal(vs.A, vsParsed.A);
                Assert.Equal(vs.B, vsParsed.B);
                Assert.Equal(vs.C(0), vsParsed.C(0));
                Assert.Equal(vs.C(1), vsParsed.C(1));

                byte[] expectedBytes =
                {
                    4, 0, 0, 0,         // offset to table start
                    248, 255, 255, 255, // soffset to vtable.
                    12, 0, 0, 0,        // uoffset to field 0 (fields table)
                    6, 0,               // vtable length
                    8, 0,               // table length
                    4, 0,               // offset of field 0
                    0, 0,               // padding

                    242, 255, 255, 255, // soffset to vtable
                    4, 0, 0, 0,         // valuestruct.a
                    3, 0,               // valuestruct.b, padding
                    1, 0,               // valuestruct.c[0]
                    2, 0,               // valuestruct.c[1]

                    8, 0,               // vtable length
                    14, 0,              // table length
                    0, 0,               // field 0 (not present)
                    4, 0,               // field 1
                };

                Helpers.AssertSequenceEqual(expectedBytes, buffer);
                Helpers.AssertMutationWorks(option, fields, false, p => p.ValueStruct, new ValueStruct(), (a, b) =>
                {
                    var av = a.Value;
                    var bv = b.Value;

                    Assert.Equal(av.A, bv.A);
                    Assert.Equal(av.B, bv.B);
                    Assert.Equal(av.C(0), bv.C(0));
                    Assert.Equal(av.C(1), bv.C(1));
                });
            }
        }
        finally
        {
            MockBitConverter.IsLittleEndian = BitConverter.IsLittleEndian;
        }
    }

    [Fact]
    public void ValueStructTableField_ProgressiveClear()
    {
        Root root = new Root() { Fields = new() { ValueStruct = new ValueStruct { A = 5 } } }.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        var fields = root.Fields;
        Assert.Equal(5, fields.ValueStruct.Value.A);
        buffer.AsSpan().Clear();
        Assert.Equal(5, fields.ValueStruct.Value.A);
    }

    [Fact]
    public void ValueStructStructField_ProgressiveClear()
    {
        Root root = new Root() { Fields = new() { RefStruct = new() { E = new ValueStruct { A = 5 } } } }.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        var fields = root.Fields.RefStruct;
        Assert.Equal(5, fields.E.A);
        buffer.AsSpan().Clear();
        Assert.Equal(5, fields.E.A);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ReferenceStructTableField(FlatBufferDeserializationOption option)
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Assert.Throws<IndexOutOfRangeException>(() => rs.C[-1] = 4);
        Assert.Throws<IndexOutOfRangeException>(() => rs.D[3] = 4);

        Assert.NotNull(parsed.Fields);
        Assert.Null(parsed.Vectors);
        Assert.NotNull(parsed.Fields.RefStruct);

        RefStruct rsParsed = parsed.Fields.RefStruct;
        Assert.Equal(rs.A, rsParsed.A);
        Assert.Equal(rs.B, rsParsed.B);
        Assert.Equal(rs.C[0], rsParsed.C[0]);
        Assert.Equal(rs.C[1], rsParsed.C[1]);
        Assert.Equal(rs.D[0], rsParsed.D[0]);
        Assert.Equal(rs.D[1], rsParsed.D[1]);

        ValueStruct vsParsed = rsParsed.E;
        Assert.Equal(vs.A, vsParsed.A);
        Assert.Equal(vs.B, vsParsed.B);
        Assert.Equal(vs.C(0), vsParsed.C(0));
        Assert.Equal(vs.C(1), vsParsed.C(1));

        Helpers.AssertSequenceEqual(expectedBytes, buffer);
        Helpers.AssertMutationWorks(option, parsed.Fields, false, p => p.RefStruct, new RefStruct());
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ReferenceStructWriteThrough(FlatBufferDeserializationOption option)
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        RefStruct rsp = parsed.Fields.RefStruct;

        Helpers.AssertMutationWorks(option, rsp, true, rsp => rsp.A, 10);
        Helpers.AssertMutationWorks(option, rsp, false, rsp => rsp.B, Fruit.Strawberry);
        Helpers.AssertMutationWorks(option, rsp, true, rsp => rsp.__flatsharp__C_0, (sbyte)3);
        Helpers.AssertMutationWorks(option, rsp, true, rsp => rsp.__flatsharp__C_1, (sbyte)6);
        Helpers.AssertMutationWorks(option, rsp, false, rsp => rsp.__flatsharp__D_0, (sbyte)3);
        Helpers.AssertMutationWorks(option, rsp, false, rsp => rsp.__flatsharp__D_1, (sbyte)6);
        Helpers.AssertMutationWorks(option, rsp, false, rsp => rsp.E, new ValueStruct(), (a, b) =>
        {
            Assert.Equal(a.A, b.A);
            Assert.Equal(a.B, b.B);
            Assert.Equal(a.C(0), b.C(0));
            Assert.Equal(a.C(1), b.C(1));
        });

        var parsed2 = Root.Serializer.Parse(buffer, option);

        Assert.Equal(rsp.A, parsed2.Fields.RefStruct.A);
        Assert.Equal(rsp.C[0], parsed2.Fields.RefStruct.C[0]);
        Assert.Equal(rsp.C[1], parsed2.Fields.RefStruct.C[1]);
    }

    [Fact]
    public void ReferenceStructWriteThrough_Progressive()
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        RefStruct rsp = parsed.Fields.RefStruct;

        // write first.
        rsp.A = 4;
        rsp.C[0] = 6;
        rsp.C[1] = 7;

        // clear buffer
        buffer.AsSpan().Clear();

        // now read. Expect the same values back.
        Assert.Equal(4, rsp.A);
        Assert.Equal(6, rsp.C[0]);
        Assert.Equal(7, rsp.C[1]);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ReferenceStructOnDeserialized(FlatBufferDeserializationOption option)
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);

        Root parsed = root.SerializeAndParse(option, out byte[] buffer);
        Fields fields = parsed.Fields;
        RefStruct rsp = fields.RefStruct;

        Assert.True(parsed.IsInitialized);
        Assert.True(Root.IsStaticInitialized);

        Assert.True(fields.IsInitialized);
        Assert.True(Fields.IsStaticInitialized);

        Assert.True(rsp.IsInitialized);
        Assert.True(RefStruct.IsStaticInitialized);
    }

    [Fact]
    public void ProgressiveFieldLoads()
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        Fields fields = Helpers.TestProgressiveFieldLoad(0, true, parsed, p => p.Fields);
        Assert.NotNull(fields);

        {
            RefStruct @ref = Helpers.TestProgressiveFieldLoad(0, true, fields, f => f.RefStruct);
            Assert.Equal(root.Fields.RefStruct.A, Helpers.TestProgressiveFieldLoad(0, true, @ref, r => r.A));
            Assert.Equal(root.Fields.RefStruct.B, Helpers.TestProgressiveFieldLoad(1, true, @ref, r => r.B));
            Assert.Equal(root.Fields.RefStruct.C[0], Helpers.TestProgressiveFieldLoad(2, true, @ref, r => r.__flatsharp__C_0));
            Assert.Equal(root.Fields.RefStruct.C[1], Helpers.TestProgressiveFieldLoad(3, true, @ref, r => r.__flatsharp__C_1));
            Assert.Equal(root.Fields.RefStruct.D[0], Helpers.TestProgressiveFieldLoad(4, true, @ref, r => r.__flatsharp__D_0));
            Assert.Equal(root.Fields.RefStruct.D[1], Helpers.TestProgressiveFieldLoad(5, true, @ref, r => r.__flatsharp__D_1));
            Helpers.TestProgressiveFieldLoad(6, true, @ref, r => r.E);
        }
    }

    private static Root CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBuffer)
    {
        vs = new()
        {
            A = 4,
            B = 3,
        };

        vs.C(0) = 1;
        vs.C(1) = 2;

        rs = new()
        {
            A = 12,
            B = Fruit.Pear,
            E = vs
        };

        rs.C[0] = 2;
        rs.C[1] = 3;
        rs.D[0] = 4;
        rs.D[1] = 5;

        Root root = new Root
        {
            Fields = new()
            {
                RefStruct = rs,
            }
        };

        expectedBuffer = new byte[]
        {
            4, 0, 0, 0,         // offset to table start
            248, 255, 255, 255, // soffset to vtable.
            12, 0, 0, 0,        // uoffset to field 0 (fields table)
            6, 0,               // vtable length
            8, 0,               // table length
            4, 0,               // offset of field 0
            0, 0,               // padding

            226, 255, 255, 255,  // soffset to vtable

            12, 0, 0, 0,        // refStruct.A (ulong)
            0, 0, 0, 0,
            3, 0, 0, 0,         // refStruct.Fruit (pear)
            2, 3, 4, 5,         // refStruct.C[0,1], refStruct.D[0,1]
            4, 0, 0, 0,         // valuestruct.a
            3, 0,               // valuestruct.b, padding
            1, 0,               // valuestruct.c[0]
            2, 0,               // valuestruct.c[1]

            6, 0,               // vtable length
            30, 0,              // table length
            4, 0,               // field 0
        };

        return root;
    }
}
