/*
 * Copyright 2018 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Runtime.InteropServices;
using FlatSharp.Internal;
using global::Google.FlatBuffers;

namespace FlatSharpEndToEndTests.Oracle;

/// <summary>
/// Does oracle-based testing using Google's flatbuffers code. These tests all boil down to:
///     Can we parse data we created using the official Google library?
/// </summary>
public partial class OracleDeserializeTests
{
    [Fact]
    public void SimpleTypes()
    {
        var builder = new FlatBufferBuilder(1024);
        var fbOffset = Flatc.BasicTypes.CreateBasicTypes(
            builder,
            Bool: true,
            Byte: GetRandom<byte>(),
            SByte: GetRandom<sbyte>(),
            UShort: GetRandom<ushort>(),
            Short: GetRandom<short>(),
            UInt: GetRandom<uint>(),
            Int: GetRandom<int>(),
            ULong: GetRandom<ulong>(),
            Long: GetRandom<long>(),
            Float: GetRandom<float>(),
            Double: GetRandom<double>(),
            StringOffset: builder.CreateString("foobar"));

        builder.Finish(fbOffset.Value);
        byte[] realBuffer = builder.DataBuffer.ToSizedArray();

        var oracle = Flatc.BasicTypes.GetRootAsBasicTypes(new ByteBuffer(realBuffer));

        var simple = FS.BasicTypes.Serializer.Parse(realBuffer);

        foreach (var parsed in new[] { simple })
        {
            Assert.True(parsed.Bool);
            Assert.Equal(oracle.Byte, parsed.Byte);
            Assert.Equal(oracle.SByte, parsed.SByte);

            Assert.Equal(oracle.UShort, parsed.UShort);
            Assert.Equal(oracle.Short, parsed.Short);

            Assert.Equal(oracle.UInt, parsed.UInt);
            Assert.Equal(oracle.Int, parsed.Int);

            Assert.Equal(oracle.ULong, parsed.ULong);
            Assert.Equal(oracle.Long, parsed.Long);

            Assert.Equal(oracle.Float, parsed.Float);
            Assert.Equal(oracle.Double, parsed.Double);
            Assert.Equal("foobar", parsed.String);
        }

        // Ensures the caching works correctly.
        //Assert.ReferenceEquals(cached.String, cached.String);
        Assert.True(object.ReferenceEquals(simple.String, simple.String));
    }

    [Fact]
    public void LinkedList()
    {
        var builder = new FlatBufferBuilder(1024);
        var testData = Flatc.LinkedListNode.CreateLinkedListNode(
            builder,
            builder.CreateString("node 1"),
            Flatc.LinkedListNode.CreateLinkedListNode(
                builder,
                builder.CreateString("node 2")));

        builder.Finish(testData.Value);

        byte[] realBuffer = builder.SizedByteArray();

        var linkedList = FS.LinkedListNode.Serializer.Parse(realBuffer);

        Assert.NotNull(linkedList);
        Assert.NotNull(linkedList.Next);
        Assert.Null(linkedList.Next.Next);

        Assert.Equal("node 1", linkedList.Value);
        Assert.Equal("node 2", linkedList.Next.Value);
    }

    [Fact]
    public void ScalarVectors()
    {
        var builder = new FlatBufferBuilder(1024);
        var testData = Flatc.Vectors.CreateVectors(
            builder,
            Flatc.Vectors.CreateIntVectorVector(builder, new[] { 1, 2, 3, 4, 5, 6, }),
            Flatc.Vectors.CreateLongVectorVector(builder, new[] { 7L, 8, 9, 10, 11, 12, }),
            Flatc.Vectors.CreateByteVector1Vector(builder, new byte[] { 1, 2, 3, 4, 5 }),
            Flatc.Vectors.CreateByteVector2Vector(builder, new byte[] { 1, 2, 3, 4, 5 }),
            Flatc.Vectors.CreateByteVector3Vector(builder, new byte[0]));

        builder.Finish(testData.Value);

        byte[] realBuffer = builder.SizedByteArray();

        var parsed = FS.Vectors.Serializer.Parse(realBuffer);

        IList<int> intItems = parsed.IntVector;
        Assert.Equal(6, intItems.Count);
        for (int i = 0; i < 6; ++i)
        {
            Assert.Equal(1 + i, intItems[i]);
        }

        IList<long> longItems = parsed.LongVector;
        Assert.Equal(6, longItems.Count);
        for (int i = 0; i < 6; ++i)
        {
            Assert.Equal(7 + i, longItems[i]);
        }

        Memory<byte> mem = parsed.ByteVector2.Value;
        Assert.Equal(5, mem.Length);
        for (int i = 1; i <= 5; ++i)
        {
            Assert.Equal(i, mem.Span[i - 1]);
        }

        Assert.True(parsed.ByteVector3.IsEmpty);
    }

    [Fact]
    public void LocationStruct()
    {
        var builder = new FlatBufferBuilder(1024);
        var fakeString = builder.CreateString("foobar");
        Flatc.LocationHolder.StartLocationVectorVector(builder, 3);
        Flatc.Location.CreateLocation(builder, 7f, 8f, 9f);
        Flatc.Location.CreateLocation(builder, 4f, 5f, 6f);
        Flatc.Location.CreateLocation(builder, 1f, 2f, 3f);
        var vectorOffset = builder.EndVector();

        Flatc.LocationHolder.StartLocationHolder(builder);
        Flatc.LocationHolder.AddFake(builder, fakeString);
        Flatc.LocationHolder.AddSingleLocation(builder, Flatc.Location.CreateLocation(builder, 0.1f, 0.2f, 0.3f));
        Flatc.LocationHolder.AddLocationVector(builder, vectorOffset);
        var testData = Flatc.LocationHolder.EndLocationHolder(builder);

        builder.Finish(testData.Value);

        byte[] realBuffer = builder.SizedByteArray();
        var parsed = FS.LocationHolder.Serializer.Parse(realBuffer);

        Assert.Equal("foobar", parsed.Fake);
        Assert.Equal(0.1f, parsed.SingleLocation.X);
        Assert.Equal(0.2f, parsed.SingleLocation.Y);
        Assert.Equal(0.3f, parsed.SingleLocation.Z);
        Assert.Equal(3, parsed.LocationVector.Count);

        for (int i = 0; i < 3; ++i)
        {
            Assert.Equal((float)(3 * i + 1), parsed.LocationVector[i].X);
            Assert.Equal((float)(3 * i + 2), parsed.LocationVector[i].Y);
            Assert.Equal((float)(3 * i + 3), parsed.LocationVector[i].Z);
        }
    }

    [Fact]
    public void FiveByteStructVector()
    {
        var builder = new FlatBufferBuilder(1024);
        Flatc.FiveByteStructTable.StartVectorVector(builder, 3);
        Flatc.FiveByteStruct.CreateFiveByteStruct(builder, 3, 3);
        Flatc.FiveByteStruct.CreateFiveByteStruct(builder, 2, 2);
        Flatc.FiveByteStruct.CreateFiveByteStruct(builder, 1, 1);
        var vectorOffset = builder.EndVector();

        Flatc.FiveByteStructTable.StartFiveByteStructTable(builder);
        Flatc.FiveByteStructTable.AddVector(builder, vectorOffset);
        var testData = Flatc.FiveByteStructTable.EndFiveByteStructTable(builder);

        builder.Finish(testData.Value);

        byte[] realBuffer = builder.SizedByteArray();
        var parsed = FS.FiveByteStructTable.Serializer.Parse(realBuffer);

        Assert.Equal(3, parsed.Vector.Count);

        Assert.Equal(1, parsed.Vector[0].Int);
        Assert.Equal(2, parsed.Vector[1].Int);
        Assert.Equal(3, parsed.Vector[2].Int);

        Assert.Equal((byte)1, parsed.Vector[0].Byte);
        Assert.Equal((byte)2, parsed.Vector[1].Byte);
        Assert.Equal((byte)3, parsed.Vector[2].Byte);
    }

    [Fact]
    public void Union_Table_BasicTypes()
    {
        var builder = new FlatBufferBuilder(1024);
        var basicTypesOffset = Flatc.BasicTypes.CreateBasicTypes(
            builder,
            Bool: true,
            Byte: GetRandom<byte>(),
            SByte: GetRandom<sbyte>(),
            UShort: GetRandom<ushort>(),
            Short: GetRandom<short>(),
            UInt: GetRandom<uint>(),
            Int: GetRandom<int>(),
            ULong: GetRandom<ulong>(),
            Long: GetRandom<long>(),
            Float: GetRandom<float>(),
            Double: GetRandom<double>(),
            StringOffset: builder.CreateString("foobar"));

        var offset = Flatc.UnionTable.CreateUnionTable(
            builder,
            Flatc.Union.BasicTypes,
            basicTypesOffset.Value);

        builder.Finish(offset.Value);
        byte[] realBuffer = builder.DataBuffer.ToSizedArray();

        var oracle = Flatc.UnionTable.GetRootAsUnionTable(new ByteBuffer(realBuffer)).Value<Flatc.BasicTypes>().Value;
        var unionTable = FS.UnionTable.Serializer.Parse(realBuffer);

        Assert.Equal(1, unionTable.Value.Value.Discriminator);
        var parsed = unionTable.Value.Value.Item1;
        Assert.NotNull(parsed);

        Assert.True(parsed.Bool);
        Assert.Equal(oracle.Byte, parsed.Byte);
        Assert.Equal(oracle.SByte, parsed.SByte);

        Assert.Equal(oracle.UShort, parsed.UShort);
        Assert.Equal(oracle.Short, parsed.Short);

        Assert.Equal(oracle.UInt, parsed.UInt);
        Assert.Equal(oracle.Int, parsed.Int);

        Assert.Equal(oracle.ULong, parsed.ULong);
        Assert.Equal(oracle.Long, parsed.Long);

        Assert.Equal(oracle.Float, parsed.Float);
        Assert.Equal(oracle.Double, parsed.Double);
        Assert.Equal("foobar", parsed.String);
    }

    [Fact]
    public void Union_Struct_Location()
    {
        var builder = new FlatBufferBuilder(1024);
        var locationOffset = Flatc.Location.CreateLocation(
            builder,
            1.0f,
            2.0f,
            3.0f);

        var offset = Flatc.UnionTable.CreateUnionTable(
            builder,
            Flatc.Union.Location,
            locationOffset.Value);

        builder.Finish(offset.Value);
        byte[] realBuffer = builder.DataBuffer.ToSizedArray();
        var unionTable = FS.UnionTable.Serializer.Parse(realBuffer);

        Assert.Equal(2, unionTable.Value.Value.Discriminator);
        var parsed = unionTable.Value.Value.Item2;
        Assert.NotNull(parsed);

        Assert.Equal(1.0f, parsed.X);
        Assert.Equal(2.0f, parsed.Y);
        Assert.Equal(3.0f, parsed.Z);
    }

    [Fact]
    public void Union_String()
    {
        var builder = new FlatBufferBuilder(1024);
        var stringOffset = builder.CreateString("foobar");

        var offset = Flatc.UnionTable.CreateUnionTable(
            builder,
            Flatc.Union.stringValue,
            stringOffset.Value);

        builder.Finish(offset.Value);
        byte[] realBuffer = builder.DataBuffer.ToSizedArray();
        var unionTable = FS.UnionTable.Serializer.Parse(realBuffer);

        Assert.Equal(3, unionTable.Value.Value.Discriminator);
        string parsed = unionTable.Value.Value.Item3;
        Assert.Equal("foobar", parsed);
    }

    [Fact]
    public void Union_NotSet()
    {
        var builder = new FlatBufferBuilder(1024);

        var offset = Flatc.UnionTable.CreateUnionTable(builder);

        builder.Finish(offset.Value);
        byte[] realBuffer = builder.DataBuffer.ToSizedArray();

        var unionTable = FS.UnionTable.Serializer.Parse(realBuffer);
        Assert.Null(unionTable.Value);
    }

    [Fact]
    public void NestedStruct()
    {
        var builder = new FlatBufferBuilder(1024);
        var outerOffset = Flatc.OuterStruct.CreateOuterStruct(builder, 401, 100);
        Flatc.NestedStructs.StartNestedStructs(builder);
        Flatc.NestedStructs.AddOuter(builder, outerOffset);
        var offset = Flatc.NestedStructs.EndNestedStructs(builder);
        builder.Finish(offset.Value);

        byte[] realBuffer = builder.DataBuffer.ToSizedArray();

        var parsed = FS.NestedStructs.Serializer.Parse(realBuffer);

        Assert.NotNull(parsed.Outer?.Inner);

        Assert.Equal(401, parsed.Outer.Inner.A);
        Assert.Equal(100, parsed.Outer.A);
    }


    [Fact]
    public void VectorOfUnion()
    {
        Flatc.VectorOfUnionTableT table = new Flatc.VectorOfUnionTableT
        {
            Value = new List<Flatc.UnionUnion>
            {
                new Flatc.UnionUnion { Value = new Flatc.BasicTypesT { Int = 7 }, Type = Flatc.Union.BasicTypes },
                new Flatc.UnionUnion { Value = new Flatc.LocationT { X = 1, Y = 2, Z = 3 }, Type = Flatc.Union.Location },
                new Flatc.UnionUnion { Value = "foobar", Type = Flatc.Union.stringValue },
            }
        };

        var builder = new FlatBufferBuilder(1024);
        var offset = Flatc.VectorOfUnionTable.Pack(builder, table);
        builder.Finish(offset.Value);
        byte[] data = builder.SizedByteArray();

        var parsed = FS.VectorOfUnionTable.Serializer.Parse(data);

        Assert.Equal(3, parsed.Value.Count);

        Assert.Equal(1, parsed.Value[0].Discriminator);
        Assert.Equal(2, parsed.Value[1].Discriminator);
        Assert.Equal(3, parsed.Value[2].Discriminator);

        Assert.True(parsed.Value[0].TryGet(out FS.BasicTypes basicTypes));
        Assert.Equal(7, basicTypes.Int);

        Assert.True(parsed.Value[1].TryGet(out FS.Location location));
        Assert.Equal(1, location.X);
        Assert.Equal(2, location.Y);
        Assert.Equal(3, location.Z);

        Assert.True(parsed.Value[2].TryGet(out string str));
        Assert.Equal("foobar", str);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void SortedVectors(FlatBufferDeserializationOption option)
    {
        var builder = new FlatBufferBuilder(1024 * 1024);

        var strings = new List<string>();
        var stringOffsets = new List<Offset<Flatc.SortedVectorStringTable>>();

        List<int> ints = new List<int>();
        var intOffsets = new List<Offset<Flatc.SortedVectorInt32Table>>();

        List<double> doubles = new List<double>();
        var doubleOffsets = new List<Offset<Flatc.SortedVectorDoubleTable>>();

        const int Iterations = 1000;
        Random random = new Random();

        for (int i = 0; i < Iterations; ++i)
        {
            string value = Guid.NewGuid().ToString();
            strings.Add(value);
            stringOffsets.Add(Flatc.SortedVectorStringTable.CreateSortedVectorStringTable(builder, builder.CreateString(value)));
        }

        for (int i = 0; i < Iterations; ++i)
        {
            int value = random.Next();
            ints.Add(value);
            intOffsets.Add(Flatc.SortedVectorInt32Table.CreateSortedVectorInt32Table(builder, value));
        }

        for (int i = 0; i < Iterations; ++i)
        {
            double value = random.NextDouble() * random.Next();
            doubles.Add(value);
            doubleOffsets.Add(Flatc.SortedVectorDoubleTable.CreateSortedVectorDoubleTable(builder, value));
        }

        var table = Flatc.SortedVectorTest.CreateSortedVectorTest(
            builder,
            Flatc.SortedVectorInt32Table.CreateSortedVectorOfSortedVectorInt32Table(builder, intOffsets.ToArray()),
            Flatc.SortedVectorStringTable.CreateSortedVectorOfSortedVectorStringTable(builder, stringOffsets.ToArray()),
            Flatc.SortedVectorDoubleTable.CreateSortedVectorOfSortedVectorDoubleTable(builder, doubleOffsets.ToArray()));

        builder.Finish(table.Value);
        byte[] serialized = builder.SizedByteArray();

        var serializer = FS.SortedVectorTest.Serializer.WithSettings(s => s.UseDeserializationMode(option));
        var parsed = serializer.Parse(serialized);

        VerifySorted<FS.SortedVectorStringTable, string>(parsed.String, s => s.Value, new Utf8StringComparer(), strings, new List<string> { Guid.NewGuid().ToString(), "banana" });
        VerifySorted<FS.SortedVectorInt32Table, int>(parsed.Int32, s => s.Value, Comparer<int>.Default, ints, new List<int> { -1, -3, 0 });
        VerifySorted<FS.SortedVectorDoubleTable, double>(parsed.Double, s => s.Value, Comparer<double>.Default, doubles, new List<double> { Math.PI, Math.E, Math.Sqrt(2) });
    }

    [Fact]
    public void SortedVectors_NullKey_NotAllowed()
    {
        var builder = new FlatBufferBuilder(1024 * 1024);

        var strings = new List<string>();
        var stringOffsets = new List<Offset<Flatc.SortedVectorStringTable>>();

        foreach (string s in new[] { Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString() })
        {
            strings.Add(s);
            StringOffset strOffset = default;
            if (s != null)
            {
                strOffset = builder.CreateString(s);
            }

            stringOffsets.Add(Flatc.SortedVectorStringTable.CreateSortedVectorStringTable(builder, strOffset));
        }

        Assert.Throws<InvalidOperationException>(
            () => Flatc.SortedVectorStringTable.CreateSortedVectorOfSortedVectorStringTable(builder, stringOffsets.ToArray()));
    }

    private static void VerifySorted<T, TKey>(
        IList<T> items,
        Func<T, TKey> keyGetter,
        IComparer<TKey> comparer,
        List<TKey> expectedItems,
        List<TKey> unexpectedItems)
        where T : class, ISortableTable<TKey>
    {
        TKey previous = keyGetter(items[0]);
        for (int i = 1; i < items.Count; ++i)
        {
            TKey current = keyGetter(items[i]);
            Assert.True(comparer.Compare(previous, current) <= 0);
        }

        foreach (var expectedItem in expectedItems)
        {
            var item = items.BinarySearchByFlatBufferKey(expectedItem);
            Assert.NotNull(item);
        }

        foreach (var unexpectedItem in unexpectedItems)
        {
            Assert.Null(items.BinarySearchByFlatBufferKey(unexpectedItem));
        }
    }

    private static readonly Random Random = new Random();

    private static T GetRandom<T>() where T : struct
    {
        int bytes = Marshal.SizeOf<T>();
        byte[] random = new byte[bytes];
        Random.NextBytes(random);

        return MemoryMarshal.Cast<byte, T>(random)[0];
    }
}
