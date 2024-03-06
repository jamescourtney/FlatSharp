using FlatSharp.Internal;
using System.IO;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class UnionVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        IList<FunUnion> sourceUnions = root.Vectors.Union;
        IList<FunUnion> unions = vectors.Union;

        Assert.IsNotNull(unions);
        Assert.AreEqual(sourceUnions.Count, unions.Count);

        for (int i = 0; i < unions.Count; ++i)
        {
            FunUnion left = sourceUnions[i];
            FunUnion right = unions[i];

            Assert.AreEqual(sourceUnions[i].Discriminator, unions[i].Discriminator);
            Assert.AreEqual(sourceUnions[i].Kind, unions[i].Kind);

            {
                object li = left.Accept<Visitor, object>(new());
                object ri = right.Accept<Visitor, object>(new());

                Assert.IsInstanceOfType(ri, li.GetType());

                li = left.Match<object>(a => a, b => b, c => c, d => d);
                ri = right.Match<object>(a => a, b => b, c => c, d => d);

                Assert.IsInstanceOfType(ri, li.GetType());
            }

            switch (left.Kind)
            {
                case FunUnion.ItemKind.Key:
                    {
                        Assert.AreEqual(left.Key.Name, right.Key.Name);
                        Assert.AreEqual(left.Item4.Value, right.Item4.Value);

                        Assert.IsTrue(left.TryGet(out Key _));
                        Assert.IsTrue(right.TryGet(out Key _));
                        Assert.IsFalse(left.TryGet(out RefStruct _));
                        Assert.IsFalse(right.TryGet(out RefStruct _));
                    }
                    break;


                case FunUnion.ItemKind.RefStruct:
                    {
                        Assert.AreEqual(left.RefStruct.A, right.RefStruct.A);
                        Assert.AreEqual(left.Item1.B, right.Item1.B);
                        Assert.IsTrue(left.TryGet(out RefStruct _));
                        Assert.IsTrue(right.TryGet(out RefStruct _));
                        Assert.IsFalse(left.TryGet(out Key _));
                        Assert.IsFalse(right.TryGet(out Key _));
                    }
                    break;

                case FunUnion.ItemKind.str:
                    {
                        Assert.AreEqual(left.str, right.str);
                        Assert.AreEqual(left.Item3, right.Item3);
                        Assert.IsTrue(left.TryGet(out string _));
                        Assert.IsTrue(right.TryGet(out string _));
                        Assert.IsFalse(left.TryGet(out ValueStruct _));
                        Assert.IsFalse(right.TryGet(out ValueStruct _));
                    }
                    break;

                case FunUnion.ItemKind.ValueStruct:
                    {
                        Assert.AreEqual(left.ValueStruct.A, right.ValueStruct.A);
                        Assert.AreEqual(left.Item2.B, right.Item2.B);
                        Assert.IsTrue(left.TryGet(out ValueStruct _));
                        Assert.IsTrue(right.TryGet(out ValueStruct _));
                        Assert.IsFalse(left.TryGet(out string _));
                        Assert.IsFalse(right.TryGet(out string _));
                    }
                    break;
            }
        }

        Helpers.AssertSequenceEqual(expectedData, buffer);
        Helpers.ValidateListVector(option, false, unions, new FunUnion("foo"));
    });

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Big(FlatBufferDeserializationOption option)
    {
        const int ChunkSize = 8; // this test needs to exercise some edge scenarios that require knowing the chunk size of the union vectors.

        Random r = new Random();

        for (int test = 1; test < 10; ++test)
        {
            List<FunUnion> expected = new();

            for (int i = 0; i < (ChunkSize * test) + 1; ++i)
            {
                FunUnion union = (r.Next() % 4) switch
                {
                    0 => new(new Key { Name = i.ToString() }),
                    1 => new(new RefStruct { A = i }),
                    2 => new(new ValueStruct { A = i }),
                    3 => new(i.ToString()),
                    _ => throw new Exception(),
                };

                expected.Add(union);
            }

            Root root = new Root { Vectors = new() { Union = expected } }.SerializeAndParse(option);
            IList<FunUnion> parsed = root.Vectors.Union;

            Assert.AreEqual(expected.Count, parsed.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                FunUnion e = expected[i];
                FunUnion p = parsed[i];

                Assert.AreEqual(e.Discriminator, p.Discriminator);
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector_Invalid_MissingOffset(FlatBufferDeserializationOption option)
    {
        CreateInvalid_MissingOffset(out byte[] buffer);

        Assert.ThrowsException<InvalidDataException>(() =>
        {
            var item = Root.Serializer.Parse(buffer, option);
            var x = item.Vectors.Union[0];
        });
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector_Invalid_InvalidDiscriminator(FlatBufferDeserializationOption option)
    {
        CreateInvalid_InvalidDiscriminator(out byte[] buffer);

        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
        {
            var item = Root.Serializer.Parse(buffer, option);
            var x = item.Vectors.Union[0];
        });

        Assert.AreEqual(
            $"Unexpected union discriminator value '7' for Union {typeof(FunUnion).FullName}",
            ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector_Invalid_MismatchedCounts(FlatBufferDeserializationOption option)
    {
        CreateInvalid_MismatchedCounts(out byte[] buffer);

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            var item = Root.Serializer.Parse(buffer, option);
            var x = item.Vectors.Union[0];
        });

        Assert.AreEqual("Union vector had mismatched number of discriminators and offsets.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector_Invalid_NoOffsetVector(FlatBufferDeserializationOption option)
    {
        CreateInvalid_NoOffsetVector(out byte[] buffer);

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            var item = Root.Serializer.Parse(buffer, option);
            var x = item.Vectors.Union[0];
        });

        Assert.AreEqual("FlatBuffer table property 'FlatSharpStrykerTests.Vectors.Union' was only partially included in the buffer.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UnionVector_Invalid_NoDiscriminatorVector(FlatBufferDeserializationOption option)
    {
        CreateInvalid_NoOffsetVector(out byte[] buffer);

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            var item = Root.Serializer.Parse(buffer, option);
            var x = item.Vectors.Union[0];
        });

        Assert.AreEqual("FlatBuffer table property 'FlatSharpStrykerTests.Vectors.Union' was only partially included in the buffer.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void NotPresent(FlatBufferDeserializationOption option)
    {
        Root root = new Root() { Vectors = new() };
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Assert.IsNull(parsed.Vectors.Union);
    }

    private struct Visitor : FunUnion.Visitor<object>
    {
        public object Visit(RefStruct item) => item;

        public object Visit(ValueStruct item) => item;

        public object Visit(string item) => item;

        public object Visit(Key item) => item;
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Vectors = new()
            {
                Union = Helpers.CreateList(
                    new FunUnion(new Key { Name = "b", Value = Fruit.Apple }),
                    new FunUnion(new ValueStruct { A = 1, B = 2 }),
                    new FunUnion(new RefStruct { A = 3, B = Fruit.Banana, __flatsharp__C_0 = 5, __flatsharp__C_1 = 6, __flatsharp__D_0 = 7, __flatsharp__D_1 = 8 }),
                    new FunUnion("c"))
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

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            4, 0,               // field 4 (union vector discriminators)
            8, 0,               // field 5 (union vector offsets)

            4, 0, 0, 0,
            4, 2, 1, 3,         // discriminators

            4, 0, 0, 0,         // union offsets
            16, 0, 0, 0,
            40, 0, 0, 0,
            52, 0, 0, 0,
            76, 0, 0, 0,

            244, 255, 255, 255, // key soffset to vtable
            0, 0, 0, 0,         // apple
            12, 0, 0, 0,         // offset to name.

            8, 0, 12, 0,        // key vtable
            8, 0, 4, 0,

            1, 0, 0, 0,         // key name.
            B('b'), 0, 0, 0,

            1, 0, 0, 0,         // valuestruct
            2, 0, 0, 0,
            0, 0, 0, 0,         // end value struct + padding

            0, 0, 0, 0,         // padding to 8 byte alignment.

            3, 0, 0, 0,         // ref struct
            0, 0, 0, 0,
            1, 0, 0, 0,
            5, 6, 7, 8,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            1, 0, 0, 0,         // string
            B('c'), 0
        };

        return root;
    }

    private void CreateInvalid_MissingOffset(out byte[] expectedData)
    {
        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            4, 0,               // field 4 (union vector discriminators)
            8, 0,               // field 5 (union vector offsets)

            1, 0, 0, 0,
            4, 0, 0, 0,         // discriminators

            1, 0, 0, 0,         // union offsets
            0, 0, 0, 0,
        };
    }

    private void CreateInvalid_InvalidDiscriminator(out byte[] expectedData)
    {
        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            4, 0,               // field 4 (union vector discriminators)
            8, 0,               // field 5 (union vector offsets)

            1, 0, 0, 0,
            7, 0, 0, 0,         // discriminators

            1, 0, 0, 0,         // union offsets
            0, 0, 0, 0,
        };
    }

    private void CreateInvalid_MismatchedCounts(out byte[] expectedData)
    {
        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            4, 0,               // field 4 (union vector discriminators)
            8, 0,               // field 5 (union vector offsets)

            3, 0, 0, 0,
            1, 2, 3, 0,         // discriminators

            1, 0, 0, 0,         // union offsets
            0, 0, 0, 0,
        };
    }

    private void CreateInvalid_NoOffsetVector(out byte[] expectedData)
    {
        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            4, 0,               // field 4 (union vector discriminators)
            0, 0,               // field 5 (union vector offsets)

            3, 0, 0, 0,
            1, 2, 3, 0,         // discriminators

            1, 0, 0, 0,         // union offsets
            0, 0, 0, 0,
        };
    }

    private void CreateInvalid_NoDiscriminatorVector(out byte[] expectedData)
    {
        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            244, 255, 255, 255, // soffset to vtable
            24, 0, 0, 0,        // uoffset to union discriminators
            28, 0, 0, 0,        // uoffset to union offsets

            16, 0,              // vtable length
            12, 0,              // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (memory)
            0, 0,               // field 3 (stirng)
            0, 0,               // field 4 (union vector discriminators)
            8, 0,               // field 5 (union vector offsets)

            3, 0, 0, 0,
            1, 2, 3, 0,         // discriminators

            1, 0, 0, 0,         // union offsets
            0, 0, 0, 0,
        };
    }
}
