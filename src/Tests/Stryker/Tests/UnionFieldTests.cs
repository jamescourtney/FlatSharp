using FlatSharp.Internal;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

public class UnionFieldTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void StringMember(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot_StringMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion union = parsed.Fields.Union.Value;
        Assert.Equal(FunUnion.ItemKind.str, union.Kind);
        Assert.Equal(3, union.Discriminator);
        Assert.Equal("hello", union.str);
        Assert.Equal("hello", union.Item3);
        Assert.True(union.TryGet(out string str));
        Assert.Equal("hello", str);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ValueStructMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(ValueStruct expected, ValueStruct actual)
        {
            Assert.Equal(expected.A, actual.A);
            Assert.Equal(expected.B, actual.B);
            Assert.Equal(expected.C(0), actual.C(0));
            Assert.Equal(expected.C(1), actual.C(1));
        }

        Root root = this.CreateRoot_ValueStructMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.Equal(FunUnion.ItemKind.ValueStruct, union.Kind);
        Assert.Equal(2, union.Discriminator);
        VerifyVs(sourceUnion.ValueStruct, union.ValueStruct);
        VerifyVs(sourceUnion.Item2, union.Item2);

        Assert.True(union.TryGet(out ValueStruct test));
        VerifyVs(sourceUnion.Item2, test);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void RefStructMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(RefStruct expected, RefStruct actual)
        {
            Assert.Equal(expected.A, actual.A);
            Assert.Equal(expected.B, actual.B);
            Assert.Equal(expected.C[0], actual.C[0]);
            Assert.Equal(expected.C[1], actual.C[1]);
            Assert.Equal(expected.D[0], actual.D[0]);
            Assert.Equal(expected.D[1], actual.D[1]);
        }

        Root root = this.CreateRoot_RefStructMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.Equal(FunUnion.ItemKind.RefStruct, union.Kind);
        Assert.Equal(1, union.Discriminator);
        VerifyVs(sourceUnion.RefStruct, union.RefStruct);
        VerifyVs(sourceUnion.Item1, union.Item1);

        Assert.True(union.TryGet(out RefStruct test));
        VerifyVs(sourceUnion.Item1, test);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void KeyTableMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(Key expected, Key actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }

        Root root = this.CreateRoot_KeyMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.Equal(FunUnion.ItemKind.Key, union.Kind);
        Assert.Equal(4, union.Discriminator);
        VerifyVs(sourceUnion.Key, union.Key);
        VerifyVs(sourceUnion.Item4, union.Item4);

        Assert.True(union.TryGet(out Key test));
        VerifyVs(sourceUnion.Item4, test);

        Helpers.AssertMutationWorks(option, test, false, k => k.Name, string.Empty);
        Helpers.AssertMutationWorks(option, test, false, k => k.Value, Fruit.Apple);
        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);

        Assert.True(test.IsInitialized);
        Assert.True(Key.IsStaticInitialized);
    }

    private Root CreateRoot_RefStructMember(out byte[] expectedData)
    {
        RefStruct vs = new()
        {
            A = 3,
            B = Fruit.Apple,
            __flatsharp__C_0 = -1,
            __flatsharp__C_1 = -2,
            E = default,
        };

        Root root = new()
        {
            Fields = new()
            {
                Union = new(vs)
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
            24, 0, 0, 0,        // uoffset to union
            1, 0,               // ubyte discriminator, padding

            16, 0,              // vtable length
            9, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            8, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (uoffset)

            0, 0,               // padding
            3, 0, 0, 0,         // 3 (ulong)
            0, 0, 0, 0,
            0, 0, 0, 0,         // apple
            255, 254,           // C[0,1]
            0, 0,               // D[0,1]
            0, 0, 0, 0,         // ValueStruct (default)
            0, 0, 0, 0,
            0, 0
        };

        return root;
    }

    private Root CreateRoot_ValueStructMember(out byte[] expectedData)
    {
        ValueStruct vs = new()
        {
            A = 3,
            B = 4,
        };

        vs.C(0) = 1;
        vs.C(1) = 2;

        Root root = new()
        {
            Fields = new()
            {
                Union = new(vs)
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
            24, 0, 0, 0,        // uoffset to union
            2, 0,               // ubyte discriminator, padding

            16, 0,              // vtable length
            9, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            8, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (uoffset)

            0, 0,               // padding
            3, 0, 0, 0,
            4, 0,
            1, 0,
            2, 0,
        };

        return root;
    }

    private Root CreateRoot_StringMember(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Fields = new()
            {
                Union = new("hello")
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
            24, 0, 0, 0,        // uoffset to union
            3, 0,               // ubyte discriminator, padding

            16, 0,              // vtable length
            9, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            8, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (uoffset)

            0, 0,               // padding
            5, 0, 0, 0,         // string length
            B('h'), B('e'), B('l'), B('l'),
            B('o'), 0
        };

        return root;
    }


    private Root CreateRoot_KeyMember(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Key key = new()
        {
            Name = "fred",
            Value = Fruit.Banana,
        };

        Root root = new()
        {
            Fields = new()
            {
                Union = new(key)
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
            24, 0, 0, 0,        // uoffset to union
            4, 0,               // ubyte discriminator, padding

            16, 0,              // vtable length
            9, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            8, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (uoffset)

            0, 0,               // padding
            36, 0, 0, 0,        // soffset to vtable (note: vtable is shared since banana is default value)
            4, 0, 0, 0,         // uoffset to string (name)
            4, 0, 0, 0,         // string length
            B('f'), B('r'), B('e'), B('d'), 0
        };

        return root;
    }
}
