using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

public class UnionFieldTests
{
    [Fact]
    public void InvalidConstructors()
    {
        Assert.Throws<ArgumentNullException>(() => new FunUnion((string)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((RefStruct)null));
        Assert.Throws<ArgumentNullException>(() => new FunUnion((Key)null));
    }

    [Fact]
    public void InvalidGetters()
    {
        FunUnion a = new FunUnion(new RefStruct());

        Assert.Throws<InvalidOperationException>(() => a.ValueStruct);
        Assert.Throws<InvalidOperationException>(() => a.Key);
        Assert.Throws<InvalidOperationException>(() => a.str);

        a = new FunUnion(new ValueStruct());
        Assert.Throws<InvalidOperationException>(() => a.RefStruct);
    }

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

    [Fact]
    public void BrokenUnion()
    {
        Root root = new() { Fields = new() { Union = new FunUnion() } };

        var ex = Assert.Throws<InvalidOperationException>(() => Root.Serializer.GetMaxSize(root));
        Assert.Equal("Exception determining type of union. Discriminator = 0", ex.Message);

        ex = Assert.Throws<InvalidOperationException>(() => Root.Serializer.Write(new byte[1024], root));
        Assert.Equal("Unexpected discriminator. Unions must be initialized.", ex.Message);

        ex = Assert.Throws<InvalidOperationException>(() => new FunUnion().Accept<Visitor, bool>(new Visitor()));
        Assert.Equal("Unexpected discriminator: 0", ex.Message);

        ex = Assert.Throws<InvalidOperationException>(() => new Root(root));
        Assert.Equal("Unexpected union discriminator", ex.Message);
    }

    private struct Visitor : FunUnion.Visitor<bool>
    {
        public bool Visit(RefStruct item) => true;
        public bool Visit(ValueStruct item) => true;
        public bool Visit(string item) => true;
        public bool Visit(Key item) => true;
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

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void InvalidUnion_NoOffset(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_NoOffset(out byte[] data);

        var ex = Assert.Throws<InvalidDataException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.Equal("FlatBuffer table property 'FlatSharpStrykerTests.Fields.Union' was only partially included in the buffer.", ex.Message);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void InvalidUnion_NoDiscriminator(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_NoDiscriminator(out byte[] data);

        var ex = Assert.Throws<InvalidDataException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.Equal("FlatBuffer table property 'FlatSharpStrykerTests.Fields.Union' was only partially included in the buffer.", ex.Message);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void InvalidUnion_InvalidDiscriminator(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_InvalidDiscriminator(out byte[] data);

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.Equal("Exception parsing union 'FlatSharpStrykerTests.FunUnion'. Discriminator = 10", ex.Message);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void UnionNotPresent(FlatBufferDeserializationOption option)
    {
        Root source = this.CreateRoot_UnionNotPresent(out byte[] expectedData);
        Root parsed = source.SerializeAndParse(option, out byte[] buffer);

        Assert.Null(parsed.Fields.Union);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    private Root CreateRoot_UnionNotPresent(out byte[] expectedData)
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
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0,
            4, 0, 0, 0,

            252, 255, 255, 255, // soffset to vtable
            4, 0, 4, 0          // vtable
        };

        return root;
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

    private void Create_InvalidUnion_NoOffset(out byte[] data)
    {
        data = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0,
            4, 0, 0, 0,

            250, 255, 255, 255, // soffset to vtable
            4, 0,               // ubyte discriminator, padding

            14, 0,              // vtable length
            5, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            4, 0,               // field 4 (discriminator)
        };
    }

    private void Create_InvalidUnion_NoDiscriminator(out byte[] data)
    {
        data = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0,
            4, 0, 0, 0,

            248, 255, 255, 255, // soffset to vtable
            4, 0, 0, 0,         // union offset

            16, 0,              // vtable length
            5, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            0, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (offset)
        };
    }

    private void Create_InvalidUnion_InvalidDiscriminator(out byte[] data)
    {
        data = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Fields subtable.

            // Vtable for Root (2 bytes padding)
            6, 0, 8, 0,
            4, 0, 0, 0,

            246, 255, 255, 255, // soffset to vtable
            4, 0, 0, 0,         // union offset
            10, 0,              // discriminator + padding

            16, 0,              // vtable length
            5, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            0, 0,               // field 3 (str)
            8, 0,               // field 4 (discriminator)
            4, 0,               // field 5 (offset)
        };
    }
}
