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

using global::Google.FlatBuffers;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Oracle;

/// <summary>
/// Does oracle-based testing using the Google.Flatbuffers code. These test all boil down to:
///     Can the Google Flatbuffer code parse data we generated?
/// </summary>
public partial class OracleSerializeTests
{
    [Fact]
    public void SimpleTypes_WithValues()
    {
        var simple = new FS.BasicTypes()
        {
            Bool = true,
            Byte = GetRandom<byte>(),
            SByte = GetRandom<sbyte>(),
            Double = GetRandom<double>(),
            Float = GetRandom<float>(),
            Int = GetRandom<int>(),
            Long = GetRandom<long>(),
            Short = GetRandom<short>(),
            String = "foobar",
            UInt = GetRandom<uint>(),
            ULong = GetRandom<ulong>(),
            UShort = GetRandom<ushort>(),
        };

        Span<byte> memory = new byte[10240];
        int size = FS.BasicTypes.Serializer.Write(memory, simple);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.BasicTypesVerify.Verify(new(bb), 4));
        var oracle = Flatc.BasicTypes.GetRootAsBasicTypes(bb);

        Assert.Equal(oracle.Byte, simple.Byte);
        Assert.Equal(oracle.SByte, simple.SByte);

        Assert.Equal(oracle.UShort, simple.UShort);
        Assert.Equal(oracle.Short, simple.Short);

        Assert.Equal(oracle.UInt, simple.UInt);
        Assert.Equal(oracle.Int, simple.Int);

        Assert.Equal(oracle.ULong, simple.ULong);
        Assert.Equal(oracle.Long, simple.Long);

        Assert.Equal(oracle.Float, simple.Float);
        Assert.Equal(oracle.Double, simple.Double);
        Assert.Equal(oracle.String, simple.String);
    }

    [Fact]
    public void SimpleTypes_Defaults()
    {
        var simple = new FS.BasicTypes();

        Span<byte> memory = new byte[10240];
        int size = FS.BasicTypes.Serializer.Write(memory, simple);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.BasicTypesVerify.Verify(new(bb), 4));
        var oracle = Flatc.BasicTypes.GetRootAsBasicTypes(bb);

        Assert.Equal(oracle.Byte, simple.Byte);
        Assert.Equal(oracle.SByte, simple.SByte);

        Assert.Equal(oracle.UShort, simple.UShort);
        Assert.Equal(oracle.Short, simple.Short);

        Assert.Equal(oracle.UInt, simple.UInt);
        Assert.Equal(oracle.Int, simple.Int);

        Assert.Equal(oracle.ULong, simple.ULong);
        Assert.Equal(oracle.Long, simple.Long);

        Assert.Equal(oracle.Float, simple.Float);
        Assert.Equal(oracle.Double, simple.Double);
        Assert.Equal(oracle.String, simple.String);
    }

    [Fact]
    public void LinkedList()
    {
        var simple = new FS.LinkedListNode
        {
            Value = "node 1",
            Next = new FS.LinkedListNode
            {
                Value = "node 2",
                Next = null,
            }
        };

        Span<byte> memory = new byte[10240];
        int size = FS.LinkedListNode.Serializer.Write(memory, simple);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.LinkedListNodeVerify.Verify(new(bb), 4));
        var oracle = Flatc.LinkedListNode.GetRootAsLinkedListNode(bb);

        Assert.Equal(oracle.Value, simple.Value);
        Assert.Equal(oracle.Next?.Value, simple.Next.Value);
        Assert.Null(oracle.Next?.Next);
    }

    [Fact]
    public void StructWithinTable()
    {
        FS.LocationHolder holder = new()
        {
            Fake = "foobar",
            SingleLocation = new() { X = 1.1f, Y = 1.2f, Z = 1.3f },
            LocationVector = new FS.Location[]
            {
                new() { X = 2, Y = 3, Z = 4, },
                new() { X = 5, Y = 6, Z = 7, },
                new() { X = 8, Y = 9, Z = 10, }
            },
        };

        Span<byte> memory = new byte[10240];
        int size = FS.LocationHolder.Serializer.Write(memory, holder);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.LocationHolderVerify.Verify(new(bb), 4));
        var oracle = Flatc.LocationHolder.GetRootAsLocationHolder(bb);

        Assert.Equal(holder.Fake, oracle.Fake);

        Assert.Equal(holder.SingleLocation.X, oracle.SingleLocation.Value.X);
        Assert.Equal(holder.SingleLocation.Y, oracle.SingleLocation.Value.Y);
        Assert.Equal(holder.SingleLocation.Z, oracle.SingleLocation.Value.Z);

        Assert.Equal(holder.LocationVector.Count, oracle.LocationVectorLength);

        for (int i = 0; i < holder.LocationVector.Count; ++i)
        {
            var holderLoc = holder.LocationVector[i];
            var oracleLoc = oracle.LocationVector(i);

            Assert.Equal(holderLoc.X, oracleLoc.Value.X);
            Assert.Equal(holderLoc.Y, oracleLoc.Value.Y);
            Assert.Equal(holderLoc.Z, oracleLoc.Value.Z);
        }
    }

    [Fact]
    public void ScalarVectors()
    {
        FS.Vectors table = new()
        {
            IntVector = new[] { 1, 2, 3, 4, 5 },
            LongVector = new[] { 1L, 2L, 3L, 4L, 5L, },
            ByteVector1 = new byte[] { 1, 2, 3, 4, 5, },
            ByteVector2 = new byte[] { 6, 7, 8, 9, 10 },
            ByteVector3 = new byte[0],
        };

        Span<byte> memory = new byte[10240];
        int size = FS.Vectors.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.VectorsVerify.Verify(new(bb), 4));
        var oracle = Flatc.Vectors.GetRootAsVectors(bb);

        Assert.Equal(table.IntVector.Count, oracle.IntVectorLength);
        Assert.Equal(table.LongVector.Count, oracle.LongVectorLength);
        Assert.Equal(table.ByteVector1.Value.Length, oracle.ByteVector1Length);
        Assert.Equal(table.ByteVector2.Value.Length, oracle.ByteVector2Length);
        Assert.Equal(table.ByteVector3.Length, oracle.ByteVector3Length);

        for (int i = 0; i < table.IntVector.Count; ++i)
        {
            Assert.Equal(table.IntVector[i], oracle.IntVector(i));
        }

        for (int i = 0; i < table.LongVector.Count; ++i)
        {
            Assert.Equal(table.LongVector[i], oracle.LongVector(i));
        }

        for (int i = 0; i < table.ByteVector1.Value.Length; ++i)
        {
            Assert.Equal(table.ByteVector1.Value.Span[i], oracle.ByteVector1(i));
        }

        for (int i = 0; i < table.ByteVector2.Value.Length; ++i)
        {
            Assert.Equal(table.ByteVector2.Value.Span[i], oracle.ByteVector2(i));
        }
    }

    [Fact]
    public void FiveByteStructVector()
    {
        FS.FiveByteStructTable table = new()
        {
            Vector = new FS.FiveByteStruct[]
            {
                new() { Byte = 1, Int = 1 },
                new() { Byte = 2, Int = 2 },
                new() { Byte = 3, Int = 3 },
            }
        };

        Span<byte> buffer = new byte[1024];
        var bytecount = FS.FiveByteStructTable.Serializer.Write(buffer, table);
        buffer = buffer.Slice(0, bytecount);

        var bb = new ByteBuffer(buffer.ToArray());
        Assert.True(Flatc.FiveByteStructTableVerify.Verify(new Verifier(bb), 4));
        var oracle = Flatc.FiveByteStructTable.GetRootAsFiveByteStructTable(bb);

        Assert.Equal(3, oracle.VectorLength);

        Assert.Equal(1, oracle.Vector(0).Value.Int);
        Assert.Equal((byte)1, oracle.Vector(0).Value.Byte);

        Assert.Equal(2, oracle.Vector(1).Value.Int);
        Assert.Equal((byte)2, oracle.Vector(1).Value.Byte);

        Assert.Equal(3, oracle.Vector(2).Value.Int);
        Assert.Equal((byte)3, oracle.Vector(2).Value.Byte);
    }

    [Fact]
    public void Union_Table_BasicTypes()
    {
        var simple = new FS.BasicTypes()
        {
            Bool = true,
            Byte = GetRandom<byte>(),
            SByte = GetRandom<sbyte>(),
            Double = GetRandom<double>(),
            Float = GetRandom<float>(),
            Int = GetRandom<int>(),
            Long = GetRandom<long>(),
            Short = GetRandom<short>(),
            String = "foobar",
            UInt = GetRandom<uint>(),
            ULong = GetRandom<ulong>(),
            UShort = GetRandom<ushort>(),
        };

        var table = new FS.UnionTable()
        {
            Value = new FS.Union(simple)
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.Equal(1, (int)oracle.ValueType);
        var bt = oracle.Value<Flatc.BasicTypes>().Value;

        Assert.True(bt.Bool);
        Assert.Equal(bt.Byte, simple.Byte);
        Assert.Equal(bt.SByte, simple.SByte);

        Assert.Equal(bt.UShort, simple.UShort);
        Assert.Equal(bt.Short, simple.Short);

        Assert.Equal(bt.UInt, simple.UInt);
        Assert.Equal(bt.Int, simple.Int);

        Assert.Equal(bt.ULong, simple.ULong);
        Assert.Equal(bt.Long, simple.Long);

        Assert.Equal(bt.Float, simple.Float);
        Assert.Equal(bt.Double, simple.Double);
        Assert.Equal(bt.String, simple.String);
    }

    [Fact]
    public void Union_String()
    {
        var table = new FS.UnionTable
        {
            Value = new("foobar")
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.Equal(3, (int)oracle.ValueType);
        Assert.Equal("foobar", oracle.ValueAsString());
    }

    [Fact]
    public void Union_NotSet()
    {
        var table = new FS.UnionTable
        {
            Value = null,
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.Equal(0, (int)oracle.ValueType);
    }

    [Fact]
    public void Union_Struct_Location()
    {
        var location = new FS.Location
        {
            X = 1.0f,
            Y = 2.0f,
            Z = 3.0f
        };

        var table = new FS.UnionTable
        {
            Value = new(location)
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.Equal(2, (int)oracle.ValueType);
        var bt = oracle.Value<Flatc.Location>().Value;

        Assert.Equal(bt.X, location.X);
        Assert.Equal(bt.Y, location.Y);
        Assert.Equal(bt.Z, location.Z);
    }

    [Fact]
    public void NestedStruct_InnerStructSet()
    {
        var nested = new FS.NestedStructs
        {
            Outer = new()
            {
                A = 100,
                Inner = new() { A = 401 }
            }
        };

        Span<byte> memory = new byte[10240];
        int size = FS.NestedStructs.Serializer.Write(memory, nested);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.NestedStructsVerify.Verify(new(bb), 4));
        var oracle = Flatc.NestedStructs.GetRootAsNestedStructs(bb);

        Assert.NotNull(oracle.Outer);

        var outer = oracle.Outer.Value;
        Assert.Equal(100, outer.A);
        Assert.Equal(401, outer.Inner.A);
    }

    [Fact]
    public void NestedStruct_InnerStructNotSet()
    {
        var nested = new FS.NestedStructs
        {
            Outer = new()
            {
                A = 100,
                Inner = null,
            }
        };

        Span<byte> memory = new byte[10240];
        int size = FS.NestedStructs.Serializer.Write(memory, nested);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.NestedStructsVerify.Verify(new(bb), 4));
        var oracle = Flatc.NestedStructs.GetRootAsNestedStructs(bb);

        Assert.NotNull(oracle.Outer);

        var outer = oracle.Outer.Value;
        Assert.Equal(100, outer.A);
        Assert.Equal(0, outer.Inner.A);
    }

    [Fact]
    public void VectorOfUnion()
    {
        FS.VectorOfUnionTable table = new()
        {
            Value = new FS.Union[]
            {
                new(new FS.BasicTypes { Int = 7 }),
                new(new FS.Location { X = 1, Y = 2, Z = 3 }),
                new("foobar"),
            }
        };

        Span<byte> memory = new byte[10240];
        int size = FS.VectorOfUnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.True(Flatc.UnionVectorTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionVectorTable.GetRootAsUnionVectorTable(bb);

        Assert.Equal(3, oracle.ValueLength);

        Assert.Equal(Flatc.Union.BasicTypes, oracle.ValueType(0));
        Assert.Equal(Flatc.Union.Location, oracle.ValueType(1));
        Assert.Equal(Flatc.Union.stringValue, oracle.ValueType(2));

        var basicTypes = oracle.Value<Flatc.BasicTypes>(0).Value;
        Assert.Equal(7, basicTypes.Int);

        var location = oracle.Value<Flatc.Location>(1).Value;
        Assert.Equal(1, location.X);
        Assert.Equal(2, location.Y);
        Assert.Equal(3, location.Z);

        string stringValue = oracle.ValueAsString(2);
        Assert.Equal("foobar", stringValue);
    }

    [Fact]
    public void SortedVectors()
    {
        var test = new FS.SortedVectorTest
        {
            Double = new List<FS.SortedVectorDoubleTable>(),
            Int32 = new List<FS.SortedVectorInt32Table>(),
            String = new List<FS.SortedVectorStringTable>(),
        };

        const int Iterations = 1000;
        Random random = new Random();

        for (int i = 0; i < Iterations; ++i)
        {
            string value = Guid.NewGuid().ToString();
            test.String.Add(new FS.SortedVectorStringTable { Value = value });
            test.Int32.Add(new FS.SortedVectorInt32Table { Value = random.Next() });
            test.Double.Add(new FS.SortedVectorDoubleTable { Value = random.NextDouble() * random.Next() });
        }

        byte[] data = new byte[1024 * 1024];
        int bytesWritten = FS.SortedVectorTest.Serializer.Write(data, test);

        var parsed = FS.SortedVectorTest.Serializer.Parse(data);

        var bb = new ByteBuffer(data);
        Assert.True(Flatc.SortedVectorTestVerify.Verify(new(bb), 4));
        var table = Flatc.SortedVectorTest.GetRootAsSortedVectorTest(bb);

        VerifySorted<IList<FS.SortedVectorStringTable>, FS.SortedVectorStringTable, string>(parsed.String, i => i.Value, i => table.String(i)?.Value, t => table.StringByKey(t) != null, new Utf8StringComparer());
        VerifySorted<IList<FS.SortedVectorInt32Table>, FS.SortedVectorInt32Table, int>(parsed.Int32, i => i.Value, i => table.Int32(i).Value.Value, t => table.Int32ByKey(t) != null, Comparer<int>.Default);
        VerifySorted<IList<FS.SortedVectorDoubleTable>, FS.SortedVectorDoubleTable, double>(parsed.Double, i => i.Value, i => table.Double(i).Value.Value, t => table.DoubleByKey(t) != null, Comparer<double>.Default);
    }

    [Fact]
    public void SortedVectors_DefaultValue()
    {
        var test = new FS.SortedVectorTest
        {
            Int32 = new List<FS.SortedVectorInt32Table>(),
        };

        var myItem = new FS.SortedVectorInt32Table();
        Assert.Equal(5, myItem.Value);

        byte[] data = new byte[1024 * 1024];

        const int Iterations = 1000;
        Random random = new Random();

        for (int i = 0; i < Iterations; ++i)
        {
            test.Int32.Add(new() { Value = random.Next() });
        }

        test.Int32.Add(myItem);

        int bytesWritten = FS.SortedVectorTest.Serializer.Write(data, test);

        var parsed = FS.SortedVectorTest.Serializer.Parse(data);

        var bb = new ByteBuffer(data);
        Assert.True(Flatc.SortedVectorTestVerify.Verify(new(bb), 4));
        var table = Flatc.SortedVectorTest.GetRootAsSortedVectorTest(bb);

        VerifySorted<IList<FS.SortedVectorInt32Table>, FS.SortedVectorInt32Table, int>(parsed.Int32, i => i.Value, i => table.Int32(i).Value.Value, t => table.Int32ByKey(t) != null, Comparer<int>.Default);
    }

    private static void VerifySorted<TList, TItem, TKey>(
        TList items,
        Func<TItem, TKey> keyGet,
        Func<int, TKey> oracleGet,
        Func<TKey, bool> oracleContains,
        IComparer<TKey> comparer) where TList : IList<TItem>
    {
        TKey previous = keyGet(items[0]);
        Assert.True(oracleContains(previous));

        for (int i = 0; i < items.Count; ++i)
        {
            TKey current = keyGet(items[i]);
            TKey oracle = oracleGet(i);

            Assert.True(comparer.Compare(previous, current) <= 0, $"Expect: {previous} <= {current}");
            Assert.True(comparer.Compare(previous, oracle) <= 0, $"Expect: {previous} <= {oracle}");
            Assert.True(comparer.Compare(current, oracle) == 0, $"Expect: {current} == {oracle}");

            // FlatBuffers c# has a bug where binary search is broken when using default values.
            Assert.True(oracleContains(current));
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
