using FlatSharp.Internal;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

[TestClass]
public class StructFieldTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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

                Assert.ThrowsException<IndexOutOfRangeException>(() => vs.C(2) = 4);
                Assert.ThrowsException<IndexOutOfRangeException>(() => vs.C(-1) = 4);

                Root root = new Root
                {
                    Fields = new()
                    {
                        ValueStruct = vs,
                    }
                };

                Root parsed = root.SerializeAndParse(option, out byte[] buffer);
                Fields fields = parsed.Fields;

                Assert.IsNotNull(fields);
                Assert.IsNull(parsed.Vectors);

                Assert.IsNotNull(parsed.Fields.ValueStruct);

                ValueStruct vsParsed = parsed.Fields.ValueStruct.Value;

                Assert.AreEqual(vs.A, vsParsed.A);
                Assert.AreEqual(vs.B, vsParsed.B);
                Assert.AreEqual(vs.C(0), vsParsed.C(0));
                Assert.AreEqual(vs.C(1), vsParsed.C(1));

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

                    Assert.AreEqual(av.A, bv.A);
                    Assert.AreEqual(av.B, bv.B);
                    Assert.AreEqual(av.C(0), bv.C(0));
                    Assert.AreEqual(av.C(1), bv.C(1));
                });
            }
        }
        finally
        {
            MockBitConverter.IsLittleEndian = BitConverter.IsLittleEndian;
        }
    }

    [TestMethod]
    public void ValueStructTableField_ProgressiveClear()
    {
        Root root = new Root() { Fields = new() { ValueStruct = new ValueStruct { A = 5 } } }.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        var fields = root.Fields;
        Assert.AreEqual(5, fields.ValueStruct.Value.A);
        buffer.AsSpan().Clear();
        Assert.AreEqual(5, fields.ValueStruct.Value.A);
    }

    [TestMethod]
    public void ValueStructStructField_ProgressiveClear()
    {
        Root root = new Root() { Fields = new() { RefStruct = new() { E = new ValueStruct { A = 5 } } } }.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        var fields = root.Fields.RefStruct;
        Assert.AreEqual(5, fields.E.A);
        buffer.AsSpan().Clear();
        Assert.AreEqual(5, fields.E.A);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ReferenceStructTableField(FlatBufferDeserializationOption option)
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Assert.ThrowsException<IndexOutOfRangeException>(() => rs.C[-1] = 4);
        Assert.ThrowsException<IndexOutOfRangeException>(() => rs.D[3] = 4);

        Assert.IsNotNull(parsed.Fields);
        Assert.IsNull(parsed.Vectors);
        Assert.IsNotNull(parsed.Fields.RefStruct);

        RefStruct rsParsed = parsed.Fields.RefStruct;
        Assert.AreEqual(rs.A, rsParsed.A);
        Assert.AreEqual(rs.B, rsParsed.B);
        Assert.AreEqual(rs.C[0], rsParsed.C[0]);
        Assert.AreEqual(rs.C[1], rsParsed.C[1]);
        Assert.AreEqual(rs.D[0], rsParsed.D[0]);
        Assert.AreEqual(rs.D[1], rsParsed.D[1]);

        ValueStruct vsParsed = rsParsed.E;
        Assert.AreEqual(vs.A, vsParsed.A);
        Assert.AreEqual(vs.B, vsParsed.B);
        Assert.AreEqual(vs.C(0), vsParsed.C(0));
        Assert.AreEqual(vs.C(1), vsParsed.C(1));

        Helpers.AssertSequenceEqual(expectedBytes, buffer);
        Helpers.AssertMutationWorks(option, parsed.Fields, false, p => p.RefStruct, new RefStruct());
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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
            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.B, b.B);
            Assert.AreEqual(a.C(0), b.C(0));
            Assert.AreEqual(a.C(1), b.C(1));
        });

        var parsed2 = Root.Serializer.Parse(buffer, option);

        Assert.AreEqual(rsp.A, parsed2.Fields.RefStruct.A);
        Assert.AreEqual(rsp.C[0], parsed2.Fields.RefStruct.C[0]);
        Assert.AreEqual(rsp.C[1], parsed2.Fields.RefStruct.C[1]);
    }

    [TestMethod]
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
        Assert.AreEqual(4, rsp.A);
        Assert.AreEqual(6, rsp.C[0]);
        Assert.AreEqual(7, rsp.C[1]);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ReferenceStructOnDeserialized(FlatBufferDeserializationOption option)
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);

        Root parsed = root.SerializeAndParse(option, out byte[] buffer);
        Fields fields = parsed.Fields;
        RefStruct rsp = fields.RefStruct;

        Assert.IsTrue(parsed.IsInitialized);
        Assert.IsTrue(Root.IsStaticInitialized);

        Assert.IsTrue(fields.IsInitialized);
        Assert.IsTrue(Fields.IsStaticInitialized);

        Assert.IsTrue(rsp.IsInitialized);
        Assert.IsTrue(RefStruct.IsStaticInitialized);
    }

    [TestMethod]
    public void ProgressiveFieldLoads()
    {
        Root root = CreateRootReferenceStruct(out RefStruct rs, out ValueStruct vs, out byte[] expectedBytes);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        Fields fields = Helpers.TestProgressiveFieldLoad(0, true, parsed, p => p.Fields);
        Assert.IsNotNull(fields);

        {
            RefStruct @ref = Helpers.TestProgressiveFieldLoad(0, true, fields, f => f.RefStruct);
            Assert.AreEqual(root.Fields.RefStruct.A, Helpers.TestProgressiveFieldLoad(0, true, @ref, r => r.A));
            Assert.AreEqual(root.Fields.RefStruct.B, Helpers.TestProgressiveFieldLoad(1, true, @ref, r => r.B));
            Assert.AreEqual(root.Fields.RefStruct.C[0], Helpers.TestProgressiveFieldLoad(2, true, @ref, r => r.__flatsharp__C_0));
            Assert.AreEqual(root.Fields.RefStruct.C[1], Helpers.TestProgressiveFieldLoad(3, true, @ref, r => r.__flatsharp__C_1));
            Assert.AreEqual(root.Fields.RefStruct.D[0], Helpers.TestProgressiveFieldLoad(4, true, @ref, r => r.__flatsharp__D_0));
            Assert.AreEqual(root.Fields.RefStruct.D[1], Helpers.TestProgressiveFieldLoad(5, true, @ref, r => r.__flatsharp__D_1));
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
