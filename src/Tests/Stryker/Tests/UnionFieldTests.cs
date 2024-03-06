using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;

namespace FlatSharpStrykerTests;

[TestClass]
public class UnionFieldTests
{
    [TestMethod]
    public void InvalidConstructors()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((string)null));
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((RefStruct)null));
        Assert.ThrowsException<ArgumentNullException>(() => new FunUnion((Key)null));
    }

    [TestMethod]
    public void InvalidGetters()
    {
        FunUnion a = new FunUnion(new RefStruct());

        var ex = Assert.ThrowsException<InvalidOperationException>(() => a.ValueStruct);
        Assert.AreEqual("The union is not of the requested type.", ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => a.str);
        Assert.AreEqual("The union is not of the requested type.", ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => a.Key);
        Assert.AreEqual("The union is not of the requested type.", ex.Message);

        a = new FunUnion(new ValueStruct());
        ex = Assert.ThrowsException<InvalidOperationException>(() => a.RefStruct);
        Assert.AreEqual("The union is not of the requested type.", ex.Message);
    }

    [TestMethod]
    public void ProgressiveClear()
    {
        Root parsed = new Root { Fields = new() { Union = new FunUnion("hi") } }.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] buffer);

        Fields f = parsed.Fields;
        Assert.AreEqual("hi", f.Union.Value.str);

        buffer.AsSpan().Clear();

        Assert.AreEqual("hi", f.Union.Value.str);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void StringMember(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot_StringMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion union = parsed.Fields.Union.Value;
        Assert.AreEqual(FunUnion.ItemKind.str, union.Kind);
        Assert.AreEqual(3, union.Discriminator);
        Assert.AreEqual("hello", union.str);
        Assert.AreEqual("hello", union.Item3);
        Assert.IsTrue(union.TryGet(out string str));
        Assert.AreEqual("hello", str);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, new FunUnion(string.Empty), (a, b) =>
        {
            var av = a.Value;
            var bv = b.Value;
            Assert.AreEqual(av.Discriminator, bv.Discriminator);
            Assert.AreEqual(av.str, bv.str);
        });
    }

    [TestMethod]
    public void BrokenUnion()
    {
        string message = $"Unexpected union discriminator value '0' for Union {typeof(FunUnion).FullName}";

        Root root = new() { Fields = new() { Union = new FunUnion() } };

        var ex = Assert.ThrowsException<InvalidOperationException>(() => Root.Serializer.GetMaxSize(root));
        Assert.AreEqual(message, ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => Root.Serializer.Write(new byte[1024], root));
        Assert.AreEqual(message, ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => new FunUnion().Accept<Visitor, bool>(new Visitor()));
        Assert.AreEqual(message, ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => new FunUnion().Match<bool>(null!, null!, null!, null!));
        Assert.AreEqual(message, ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => new Root(root));
        Assert.AreEqual(message, ex.Message);
    }

    private struct Visitor : FunUnion.Visitor<bool>
    {
        public bool Visit(RefStruct item) => true;
        public bool Visit(ValueStruct item) => true;
        public bool Visit(string item) => true;
        public bool Visit(Key item) => true;
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ValueStructMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(ValueStruct expected, ValueStruct actual)
        {
            Assert.AreEqual(expected.A, actual.A);
            Assert.AreEqual(expected.B, actual.B);
            Assert.AreEqual(expected.C(0), actual.C(0));
            Assert.AreEqual(expected.C(1), actual.C(1));
        }

        Root root = this.CreateRoot_ValueStructMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.AreEqual(FunUnion.ItemKind.ValueStruct, union.Kind);
        Assert.AreEqual(2, union.Discriminator);
        VerifyVs(sourceUnion.ValueStruct, union.ValueStruct);
        VerifyVs(sourceUnion.Item2, union.Item2);

        Assert.IsTrue(union.TryGet(out ValueStruct test));
        VerifyVs(sourceUnion.Item2, test);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void RefStructMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(RefStruct expected, RefStruct actual)
        {
            Assert.AreEqual(expected.A, actual.A);
            Assert.AreEqual(expected.B, actual.B);
            Assert.AreEqual(expected.C[0], actual.C[0]);
            Assert.AreEqual(expected.C[1], actual.C[1]);
            Assert.AreEqual(expected.D[0], actual.D[0]);
            Assert.AreEqual(expected.D[1], actual.D[1]);
        }

        Root root = this.CreateRoot_RefStructMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.AreEqual(FunUnion.ItemKind.RefStruct, union.Kind);
        Assert.AreEqual(1, union.Discriminator);
        VerifyVs(sourceUnion.RefStruct, union.RefStruct);
        VerifyVs(sourceUnion.Item1, union.Item1);

        Assert.IsTrue(union.TryGet(out RefStruct test));
        VerifyVs(sourceUnion.Item1, test);

        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void KeyTableMember(FlatBufferDeserializationOption option)
    {
        void VerifyVs(Key expected, Key actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        Root root = this.CreateRoot_KeyMember(out var expectedData);
        Root parsed = root.SerializeAndParse(option, out var actualData);

        Helpers.AssertSequenceEqual(expectedData, actualData);

        FunUnion sourceUnion = root.Fields.Union.Value;
        FunUnion union = parsed.Fields.Union.Value;

        Assert.AreEqual(FunUnion.ItemKind.Key, union.Kind);
        Assert.AreEqual(4, union.Discriminator);
        VerifyVs(sourceUnion.Key, union.Key);
        VerifyVs(sourceUnion.Item4, union.Item4);

        Assert.IsTrue(union.TryGet(out Key test));
        VerifyVs(sourceUnion.Item4, test);

        Helpers.AssertMutationWorks(option, test, false, k => k.Name, string.Empty);
        Helpers.AssertMutationWorks(option, test, false, k => k.Value, Fruit.Apple);
        Helpers.AssertMutationWorks(option, parsed.Fields, false, f => f.Union, default);

        Assert.IsTrue(test.IsInitialized);
        Assert.IsTrue(Key.IsStaticInitialized);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void InvalidUnion_NoOffset(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_NoOffset(out byte[] data);

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.AreEqual("FlatBuffer table property 'FlatSharpStrykerTests.Fields.Union' was only partially included in the buffer.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void InvalidUnion_NoDiscriminator(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_NoDiscriminator(out byte[] data);

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.AreEqual("FlatBuffer table property 'FlatSharpStrykerTests.Fields.Union' was only partially included in the buffer.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void InvalidUnion_InvalidDiscriminator(FlatBufferDeserializationOption option)
    {
        this.Create_InvalidUnion_InvalidDiscriminator(out byte[] data);

        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
        {
            Root root = Root.Serializer.Parse(data, option);
            FunUnion union = root.Fields.Union.Value;
        });

        Assert.AreEqual(
            $"Unexpected union discriminator value '10' for Union {typeof(FunUnion).FullName}",
            ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionNotPresent(FlatBufferDeserializationOption option)
    {
        Root source = this.CreateRoot_UnionNotPresent(out byte[] expectedData);
        Root parsed = source.SerializeAndParse(option, out byte[] buffer);

        Assert.IsNull(parsed.Fields.Union);
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
