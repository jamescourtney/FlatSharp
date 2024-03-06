using FlatSharp.Internal;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

[TestClass]
public class PrimitivesFieldsTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void StringTableField(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Fields fields = parsed.Fields;
        Assert.AreEqual("hello", fields.Str);

        Helpers.AssertMutationWorks(option, fields, false, s => s.Str, string.Empty);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ScalarTableField(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);
        Fields fields = parsed.Fields;

        Assert.AreEqual(3, fields.Memory);
        Helpers.AssertMutationWorks(option, fields, false, s => s.Memory, (byte)0);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }


    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ScalarTableField_WithDefaultValue(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);
        Fields fields = parsed.Fields;

        Assert.AreEqual(3, fields.ScalarWithDefault);
        Helpers.AssertMutationWorks(option, fields, false, s => s.ScalarWithDefault, 8);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ScalarTableField_WithoutDefaultValue(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot_NotDefault(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);
        Fields fields = parsed.Fields;

        Assert.AreEqual(root.Fields.ScalarWithDefault, fields.ScalarWithDefault);
        Helpers.AssertMutationWorks(option, fields, false, s => s.ScalarWithDefault, 8);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    public void ProgressiveStringTableField()
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);
        Fields fields = parsed.Fields;
        Assert.AreEqual("hello", Helpers.TestProgressiveFieldLoad(3, true, fields, f => f.Str));
    }

    [TestMethod]
    public void ProgressiveScalarTableField()
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);
        Fields fields = parsed.Fields;
        Assert.AreEqual(3, Helpers.TestProgressiveFieldLoad(2, true, fields, f => f.Memory));
    }

    [TestMethod]
    public void ProgressiveScalarTableFieldWithDefaultValue()
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);
        Fields fields = parsed.Fields;
        Assert.AreEqual(3, Helpers.TestProgressiveFieldLoad(6, false, fields, f => f.ScalarWithDefault));
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Fields = new()
            {
                Memory = 3,
                Str = "hello",
                ScalarWithDefault = 3,
            }
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0, 
            4, 0, 0, 0, 

            246, 255, 255, 255, // soffset to vtable
            20, 0, 0, 0,        // uoffset to string
            3, 0,               // ubyte scalar, padding
            12, 0,              // vtable length
            9, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            8, 0,               // field 2 (scalar)
            4, 0,               // field 3 (str)
            0, 0,               // padding
            5, 0, 0, 0,         // string length
            B('h'), B('e'), B('l'), B('l'),
            B('o'), 0
        };

        return root;
    }

    private Root CreateRoot_NotDefault(out byte[] expectedData)
    {
        Root root = new()
        {
            Fields = new()
            {
                ScalarWithDefault = 1,
            }
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0,
            4, 0, 0, 0,

            248, 255, 255, 255, // soffset to vtable
            1, 0, 0, 0,         // scalar
            18, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            0, 0,               // field 4
            0, 0,               // field 5
            4, 0,               // field 6
        };

        return root;
    }
}
