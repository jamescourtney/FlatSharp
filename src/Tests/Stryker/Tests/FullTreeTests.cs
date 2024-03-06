using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class FullTreeTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void RootMutations(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot().SerializeAndParse(option);

        Helpers.AssertMutationWorks(option, root, false, r => r.Fields, new Fields());
        Helpers.AssertMutationWorks(option, root, false, r => r.Vectors, new Vectors());
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void VectorFieldMutations(FlatBufferDeserializationOption option)
    {
        static void AssertMemoryEqual(Memory<byte>? a, Memory<byte>? b)
        {
            Assert.AreEqual(a is null, b is null);
            if (a is null)
            {
                return;
            }

            Helpers.AssertSequenceEqual(a.Value.Span, b.Value.Span);
        }

        Vectors vectors = this.CreateRoot().SerializeAndParse(option).Vectors;

        Helpers.AssertMutationWorks(option, vectors, false, r => r.Indexed, null);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.Memory, null, AssertMemoryEqual);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.RefStruct, null);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.Str, null);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.Table, null);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.Union, null);
        Helpers.AssertMutationWorks(option, vectors, false, r => r.ValueStruct, null);
    }

    [TestMethod]
    public void VectorFieldTests_ProgressiveClear()
    {
        Vectors vectors = this.CreateRoot().SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] data).Vectors;
        Helpers.AssertSequenceEqual(new byte[] { 1, 2, 3, 4, }, vectors.Memory.Value.Span);

        data.AsSpan().Clear();

        Helpers.AssertSequenceEqual(new byte[] { 0, 0, 0, 0, }, vectors.Memory.Value.Span);
    }

    [TestMethod]
    public void GetMaxSize()
    {
        Root root = this.CreateRoot();
        int maxSize = Root.Serializer.GetMaxSize(root);

        Assert.AreEqual(898, maxSize);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Clone(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot().SerializeAndParse(option);
        Root copy = new Root(root);

        Assert.IsTrue(root.IsInitialized);
        Assert.IsTrue(copy.IsInitialized);

        Assert.AreNotSame(root, copy);
        Assert.AreEqual(typeof(Root), copy.GetType());
        Assert.AreNotEqual(typeof(Root), root.GetType());

        {
            Vectors rv = root.Vectors;
            Vectors cv = copy.Vectors;

            Assert.AreNotSame(rv, cv);
            Assert.AreNotSame(rv.GetType(), cv.GetType());
            Assert.AreEqual(typeof(Vectors), cv.GetType());
            Assert.IsTrue(rv.IsInitialized);
            Assert.IsTrue(cv.IsInitialized);

            Verify(rv.RefStruct, cv.RefStruct, Verify);
            Verify(rv.ValueStruct, cv.ValueStruct, Verify);
            Verify(rv.Str, cv.Str, Assert.AreEqual);
            Verify(rv.Union, cv.Union, Verify);
            Verify(rv.Indexed, cv.Indexed, Verify);
            Verify(rv.Memory, cv.Memory, Verify);
            Verify(rv.Table, cv.Table, Verify);
        }

        {
            Fields rf = root.Fields;
            Fields cf = copy.Fields;

            Assert.AreNotSame(rf, cf);
            Assert.IsTrue(rf.IsInitialized);
            Assert.IsTrue(cf.IsInitialized);
            Assert.AreNotSame(rf.GetType(), cf.GetType());
            Assert.AreEqual(typeof(Fields), cf.GetType());

            Assert.AreEqual(rf.Memory, cf.Memory);
            Assert.AreEqual(rf.ScalarWithDefault, cf.ScalarWithDefault);
            Assert.AreEqual(rf.Str, cf.Str);
            Verify(rf.RefStruct, cf.RefStruct);
            Verify(rf.ValueStruct, cf.ValueStruct, Verify);
            Verify(rf.Union, cf.Union, Verify);
            Assert.AreEqual(rf.Memory, cf.Memory);
        }
    }

    private static void Verify<TKey, T>(IIndexedVector<TKey, T> a, IIndexedVector<TKey, T> b, Action<T, T> verify)
        where T : class
    {
        Assert.AreNotSame(a, b);
        Assert.AreEqual(a.Count, b.Count);

        foreach (var kvp in a)
        {
            TKey key = kvp.Key;
            T value = kvp.Value;

            Assert.IsTrue(b.TryGetValue(key, out T? bValue));
            verify(value, bValue);
        }
    }

    private static void Verify<T>(IList<T> a, IList<T> b, Action<T, T> comparer)
    {
        Assert.AreNotSame(a, b);
        Assert.AreEqual(a.Count, b.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            comparer(a[i], b[i]);
        }
    }

    private static void Verify<T>(T? a, T? b, Action<T, T> action) where T : struct
    {
        Assert.AreEqual(a.HasValue, b.HasValue);
        if (a.HasValue)
        {
            action(a.Value, b.Value);
        }
    }

    private static void Verify(Memory<byte> a, Memory<byte> b)
    {
        Assert.IsTrue(a.Span.SequenceEqual(b.Span));
    }

    private static void Verify(ValueStruct a, ValueStruct b)
    {
        Assert.AreEqual(a.A, b.A);
        Assert.AreEqual(a.B, b.B);
        Assert.AreEqual(a.C(0), b.C(0));
        Assert.AreEqual(a.C(1), b.C(1));
    }

    private static void Verify(FunUnion a, FunUnion b)
    {
        Assert.AreEqual(a.Discriminator, b.Discriminator);

        switch (a.Kind)
        {
            case FunUnion.ItemKind.Key:
                Verify(a.Key, b.Key);
                return;

            case FunUnion.ItemKind.RefStruct:
                Verify(a.RefStruct, b.RefStruct);
                return;

            case FunUnion.ItemKind.str:
                Assert.AreEqual(a.str, b.str);
                return;

            case FunUnion.ItemKind.ValueStruct:
                Verify(a.ValueStruct, b.ValueStruct);
                return;

            default:
                Assert.IsFalse(true);
                return;
        }
    }

    private static void Verify(Key a, Key b)
    {
        Assert.AreNotSame(a, b);
        Assert.IsTrue(a.IsInitialized);
        Assert.IsTrue(b.IsInitialized);
        Assert.AreEqual(a.Name, b.Name);
        Assert.AreEqual(a.Value, b.Value);
    }

    private static void Verify(RefStruct a, RefStruct b)
    {
        Assert.AreNotSame(a, b);
        Assert.IsTrue(a.IsInitialized);
        Assert.IsTrue(b.IsInitialized);
        Assert.AreEqual(a.A, b.A);
        Assert.AreEqual(a.B, b.B);
        Assert.AreEqual(a.C[0], b.C[0]);
        Assert.AreEqual(a.C[1], b.C[1]);
        Assert.AreEqual(a.D[0], b.D[0]);
        Assert.AreEqual(a.D[1], b.D[1]);

        Assert.IsTrue(Enumerable.SequenceEqual(new[] { a.C[0], a.C[1] }, b.C));
        Assert.IsTrue(Enumerable.SequenceEqual(new[] { a.D[0], a.D[1] }, b.D));

        Verify(a.E, b.E);
    }

    private Root CreateRoot()
    {
        static RefStruct CreateRefStruct()
        {
            RefStruct rs = new()
            {
                A = 1,
                B = Fruit.Strawberry,
                E = new ValueStruct
                {
                    A = 4,
                    B = 5,
                }
            };

            rs.C.CopyFrom(new sbyte[] { 1, 2 }.AsSpan());
            rs.D.CopyFrom(new sbyte[] { 1, 2 }.ToList());

            return rs;
        }

        return new()
        {
            Fields = new()
            {
                Memory = 1,
                RefStruct = CreateRefStruct(),
                ScalarWithDefault = 1234,
                Str = "hello",
                Union = new(new Key() { Name = "fred", Value = Fruit.Strawberry }),
                ValueStruct = CreateRefStruct().E
            },

            Vectors = new()
            {
                Indexed = new IndexedVector<string, Key>
                {
                    { new Key { Name = "velma", Value = Fruit.Banana } },
                    { new Key { Name = "scooby", Value = Fruit.Pear } },
                    { new Key { Name = "shaggy", Value = Fruit.Apple } },
                },

                Memory = new byte[] { 1, 2, 3, 4, },
                RefStruct = Helpers.CreateList(CreateRefStruct(), CreateRefStruct()),
                Str = Helpers.CreateList("abc", "def"),
                Union = Helpers.CreateList(new FunUnion("hi"), new FunUnion(new ValueStruct()), new FunUnion(new RefStruct())),
                ValueStruct = Helpers.CreateList(CreateRefStruct().E, CreateRefStruct().E),
                Table = Helpers.CreateList(new Key() { Name = "bar" }, new Key() { Name = "foo" }),
            },
        };
    }
}
