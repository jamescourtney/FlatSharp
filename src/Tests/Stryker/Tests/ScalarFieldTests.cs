using FlatSharp.Internal;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

[TestClass]
public class ScalarFieldTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ValueStructTableField(FlatBufferDeserializationOption option)
    {
        Root r = CreateTableWithScalarField(out byte[] buffer);
        Root parsed = r.SerializeAndParse(option, out byte[] actual);

        Assert.AreEqual(r.Fields.Memory, parsed.Fields.Memory);
        Helpers.AssertSequenceEqual(buffer, actual);
    }

    private static Root CreateTableWithScalarField(out byte[] expectedBuffer)
    {
        Root root = new Root
        {
            Fields = new()
            {
                Memory = 3,
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

            250, 255, 255, 255,  // soffset to vtable
            3, 0,

            10, 0,
            5, 0,
            0, 0,
            0, 0,
            4, 0,
        };

        return root;
    }
}
