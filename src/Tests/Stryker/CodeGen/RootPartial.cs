using FlatSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatSharpStrykerTests;

public delegate IList<T> CreateListCallback<T>(params T[] items);

public interface ICreateCreateListFactory
{
    static abstract CreateListCallback<T> GetCallback<T>();
}

public partial class Root : IReferenceItem
{
    public static bool IsStaticInitialized { get; set; }

    public bool IsInitialized { get; set; }

    partial void OnInitialized(FlatBufferDeserializationContext? context) => this.IsInitialized = true;

    static partial void OnStaticInitialize() => IsStaticInitialized = true;

    public static Root CreateTestRoot<TFactory>()
        where TFactory : ICreateCreateListFactory
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
                RefStruct = TFactory.GetCallback<RefStruct>()(CreateRefStruct(), CreateRefStruct()),
                Str = TFactory.GetCallback<string>()("abc", "def"),
                Union = TFactory.GetCallback<FunUnion>()(new FunUnion("hi"), new FunUnion(new ValueStruct()), new FunUnion(new RefStruct())),
                ValueStruct = TFactory.GetCallback<ValueStruct>()(CreateRefStruct().E, CreateRefStruct().E),
                Table = TFactory.GetCallback<Key>()(new Key() { Name = "bar" }, new Key() { Name = "foo" }),
            },
        };
    }
}
