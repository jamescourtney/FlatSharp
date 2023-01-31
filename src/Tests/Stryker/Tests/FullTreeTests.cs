using FlatSharp.Internal;
using NuGet.Frameworks;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading;

namespace FlatSharpStrykerTests;

public class FullTreeTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void RootMutations(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot().SerializeAndParse(option);

        Helpers.AssertMutationWorks(option, root, false, r => r.Fields, new Fields());
        Helpers.AssertMutationWorks(option, root, false, r => r.Vectors, new Vectors());
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void VectorFieldMutations(FlatBufferDeserializationOption option)
    {
        static void AssertMemoryEqual(Memory<byte>? a, Memory<byte>? b)
        {
            Assert.Equal(a is null, b is null);
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

    [Fact]
    public void VectorFieldTests_ProgressiveClear()
    {
        Vectors vectors = this.CreateRoot().SerializeAndParse(FlatBufferDeserializationOption.Progressive, out byte[] data).Vectors;
        Helpers.AssertSequenceEqual(new byte[] { 1, 2, 3, 4, }, vectors.Memory.Value.Span);

        data.AsSpan().Clear();

        Helpers.AssertSequenceEqual(new byte[] { 0, 0, 0, 0, }, vectors.Memory.Value.Span);
    }

    [Fact]
    public void GetMaxSize()
    {
        Root root = this.CreateRoot();
        int maxSize = Root.Serializer.GetMaxSize(root);

        Assert.Equal(898, maxSize);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Clone(FlatBufferDeserializationOption option)
    {
        Root root = this.CreateRoot().SerializeAndParse(option);
        Root copy = new Root(root);

        Assert.True(root.IsInitialized);
        Assert.True(copy.IsInitialized);

        Assert.NotSame(root, copy);
        Assert.Equal(typeof(Root), copy.GetType());
        Assert.NotEqual(typeof(Root), root.GetType());

        {
            Vectors rv = root.Vectors;
            Vectors cv = copy.Vectors;

            Assert.NotSame(rv, cv);
            Assert.NotSame(rv.GetType(), cv.GetType());
            Assert.Equal(typeof(Vectors), cv.GetType());
            Assert.True(rv.IsInitialized);
            Assert.True(cv.IsInitialized);

            Verify(rv.RefStruct, cv.RefStruct, Verify);
            Verify(rv.ValueStruct, cv.ValueStruct, Verify);
            Verify(rv.Str, cv.Str, Assert.Equal);
            Verify(rv.Union, cv.Union, Verify);
            Verify(rv.Indexed, cv.Indexed, Verify);
            Verify(rv.Memory, cv.Memory, Verify);
            Verify(rv.Table, cv.Table, Verify);
        }

        {
            Fields rf = root.Fields;
            Fields cf = copy.Fields;

            Assert.NotSame(rf, cf);
            Assert.True(rf.IsInitialized);
            Assert.True(cf.IsInitialized);
            Assert.NotSame(rf.GetType(), cf.GetType());
            Assert.Equal(typeof(Fields), cf.GetType());

            Assert.Equal(rf.Memory, cf.Memory);
            Assert.Equal(rf.ScalarWithDefault, cf.ScalarWithDefault);
            Assert.Equal(rf.Str, cf.Str);
            Verify(rf.RefStruct, cf.RefStruct);
            Verify(rf.ValueStruct, cf.ValueStruct, Verify);
            Verify(rf.Union, cf.Union, Verify);
            Assert.Equal(rf.Memory, cf.Memory);
        }
    }

    private static void Verify<TKey, T>(IIndexedVector<TKey, T> a, IIndexedVector<TKey, T> b, Action<T, T> verify)
        where T : class
    {
        Assert.NotSame(a, b);
        Assert.Equal(a.Count, b.Count);

        foreach (var kvp in a)
        {
            TKey key = kvp.Key;
            T value = kvp.Value;

            Assert.True(b.TryGetValue(key, out T? bValue));
            verify(value, bValue);
        }
    }

    private static void Verify<T>(IList<T> a, IList<T> b, Action<T, T> comparer)
    {
        Assert.NotSame(a, b);
        Assert.Equal(a.Count, b.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            comparer(a[i], b[i]);
        }
    }

    private static void Verify<T>(T? a, T? b, Action<T, T> action) where T : struct
    {
        Assert.Equal(a.HasValue, b.HasValue);
        if (a.HasValue)
        {
            action(a.Value, b.Value);
        }
    }

    private static void Verify(Memory<byte> a, Memory<byte> b)
    {
        Assert.True(a.Span.SequenceEqual(b.Span));
    }

    private static void Verify(ValueStruct a, ValueStruct b)
    {
        Assert.Equal(a.A, b.A);
        Assert.Equal(a.B, b.B);
        Assert.Equal(a.C(0), b.C(0));
        Assert.Equal(a.C(1), b.C(1));
    }

    private static void Verify(FunUnion a, FunUnion b)
    {
        Assert.Equal(a.Discriminator, b.Discriminator);

        switch (a.Kind)
        {
            case FunUnion.ItemKind.Key:
                Verify(a.Key, b.Key);
                return;

            case FunUnion.ItemKind.RefStruct:
                Verify(a.RefStruct, b.RefStruct);
                return;

            case FunUnion.ItemKind.str:
                Assert.Equal(a.str, b.str);
                return;

            case FunUnion.ItemKind.ValueStruct:
                Verify(a.ValueStruct, b.ValueStruct);
                return;

            default:
                Assert.False(true);
                return;
        }
    }

    private static void Verify(Key a, Key b)
    {
        Assert.NotSame(a, b);
        Assert.True(a.IsInitialized);
        Assert.True(b.IsInitialized);
        Assert.Equal(a.Name, b.Name);
        Assert.Equal(a.Value, b.Value);
    }

    private static void Verify(RefStruct a, RefStruct b)
    {
        Assert.NotSame(a, b);
        Assert.True(a.IsInitialized);
        Assert.True(b.IsInitialized);
        Assert.Equal(a.A, b.A);
        Assert.Equal(a.B, b.B);
        Assert.Equal(a.C[0], b.C[0]);
        Assert.Equal(a.C[1], b.C[1]);
        Assert.Equal(a.D[0], b.D[0]);
        Assert.Equal(a.D[1], b.D[1]);

        Assert.Equal(new[] { a.C[0], a.C[1] }, b.C);
        Assert.Equal(new[] { a.D[0], a.D[1] }, b.D);

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
