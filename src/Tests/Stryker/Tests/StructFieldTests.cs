namespace FlatSharpStrykerTests;

public class StructFieldTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ValueStructTableField(FlatBufferDeserializationOption option)
    {
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

        Assert.NotNull(parsed.Fields);
        Assert.Null(parsed.Vectors);
        Assert.NotNull(parsed.Fields.ValueStruct);

        ValueStruct vsParsed = parsed.Fields.ValueStruct.Value;

        Assert.Equal(vs.A, vsParsed.A);
        Assert.Equal(vs.B, vsParsed.B);
        Assert.Equal(vs.C(0), vsParsed.C(0));
        Assert.Equal(vs.C(1), vsParsed.C(1));

        byte[] expectedBytes = new byte[]
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
    }


    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ReferenceStructTableField(FlatBufferDeserializationOption option)
    {
        ValueStruct vs = new()
        {
            A = 4,
            B = 3,
        };

        vs.C(0) = 1;
        vs.C(1) = 2;

        RefStruct rs = new()
        {
            A = 12,
            B = Fruit.Pear,
            D = vs
        };

        rs.C[0] = 2;
        rs.C[1] = 3;

        Assert.Throws<IndexOutOfRangeException>(() => rs.C[-1] = 4);
        Assert.Throws<IndexOutOfRangeException>(() => rs.C[3] = 4);

        Root root = new Root
        {
            Fields = new()
            {
                RefStruct = rs,
            }
        };

        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Assert.NotNull(parsed.Fields);
        Assert.Null(parsed.Vectors);
        Assert.NotNull(parsed.Fields.RefStruct);

        RefStruct rsParsed = parsed.Fields.RefStruct;
        Assert.Equal(rs.A, rsParsed.A);
        Assert.Equal(rs.B, rsParsed.B);
        Assert.Equal(rs.C[0], rsParsed.C[0]);
        Assert.Equal(rs.C[1], rsParsed.C[1]);

        ValueStruct vsParsed = rsParsed.D;
        Assert.Equal(vs.A, vsParsed.A);
        Assert.Equal(vs.B, vsParsed.B);
        Assert.Equal(vs.C(0), vsParsed.C(0));
        Assert.Equal(vs.C(1), vsParsed.C(1));

        byte[] expectedBytes = new byte[]
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
            2, 3, 0, 0,         // refStruct.C[0,1], padding
            4, 0, 0, 0,         // valuestruct.a
            3, 0,               // valuestruct.b, padding
            1, 0,               // valuestruct.c[0]
            2, 0,               // valuestruct.c[1]

            6, 0,               // vtable length
            30, 0,              // table length
            4, 0,               // field 0
        };

        Helpers.AssertSequenceEqual(expectedBytes, buffer);
    }
}
