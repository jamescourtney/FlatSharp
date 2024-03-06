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
[TestClass]
public class OracleSerializeTests
{
    [TestMethod]
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
        Assert.IsTrue(Flatc.BasicTypesVerify.Verify(new(bb), 4));
        var oracle = Flatc.BasicTypes.GetRootAsBasicTypes(bb);

        Assert.AreEqual(oracle.Byte, simple.Byte);
        Assert.AreEqual(oracle.SByte, simple.SByte);

        Assert.AreEqual(oracle.UShort, simple.UShort);
        Assert.AreEqual(oracle.Short, simple.Short);

        Assert.AreEqual(oracle.UInt, simple.UInt);
        Assert.AreEqual(oracle.Int, simple.Int);

        Assert.AreEqual(oracle.ULong, simple.ULong);
        Assert.AreEqual(oracle.Long, simple.Long);

        Assert.AreEqual(oracle.Float, simple.Float);
        Assert.AreEqual(oracle.Double, simple.Double);
        Assert.AreEqual(oracle.String, simple.String);
    }

    [TestMethod]
    public void SimpleTypes_Defaults()
    {
        var simple = new FS.BasicTypes();

        Span<byte> memory = new byte[10240];
        int size = FS.BasicTypes.Serializer.Write(memory, simple);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.IsTrue(Flatc.BasicTypesVerify.Verify(new(bb), 4));
        var oracle = Flatc.BasicTypes.GetRootAsBasicTypes(bb);

        Assert.AreEqual(oracle.Byte, simple.Byte);
        Assert.AreEqual(oracle.SByte, simple.SByte);

        Assert.AreEqual(oracle.UShort, simple.UShort);
        Assert.AreEqual(oracle.Short, simple.Short);

        Assert.AreEqual(oracle.UInt, simple.UInt);
        Assert.AreEqual(oracle.Int, simple.Int);

        Assert.AreEqual(oracle.ULong, simple.ULong);
        Assert.AreEqual(oracle.Long, simple.Long);

        Assert.AreEqual(oracle.Float, simple.Float);
        Assert.AreEqual(oracle.Double, simple.Double);
        Assert.AreEqual(oracle.String, simple.String);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.LinkedListNodeVerify.Verify(new(bb), 4));
        var oracle = Flatc.LinkedListNode.GetRootAsLinkedListNode(bb);

        Assert.AreEqual(oracle.Value, simple.Value);
        Assert.AreEqual(oracle.Next?.Value, simple.Next.Value);
        Assert.IsNull(oracle.Next?.Next);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.LocationHolderVerify.Verify(new(bb), 4));
        var oracle = Flatc.LocationHolder.GetRootAsLocationHolder(bb);

        Assert.AreEqual(holder.Fake, oracle.Fake);

        Assert.AreEqual(holder.SingleLocation.X, oracle.SingleLocation.Value.X);
        Assert.AreEqual(holder.SingleLocation.Y, oracle.SingleLocation.Value.Y);
        Assert.AreEqual(holder.SingleLocation.Z, oracle.SingleLocation.Value.Z);

        Assert.AreEqual(holder.LocationVector.Count, oracle.LocationVectorLength);

        for (int i = 0; i < holder.LocationVector.Count; ++i)
        {
            var holderLoc = holder.LocationVector[i];
            var oracleLoc = oracle.LocationVector(i);

            Assert.AreEqual(holderLoc.X, oracleLoc.Value.X);
            Assert.AreEqual(holderLoc.Y, oracleLoc.Value.Y);
            Assert.AreEqual(holderLoc.Z, oracleLoc.Value.Z);
        }
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.VectorsVerify.Verify(new(bb), 4));
        var oracle = Flatc.Vectors.GetRootAsVectors(bb);

        Assert.AreEqual(table.IntVector.Count, oracle.IntVectorLength);
        Assert.AreEqual(table.LongVector.Count, oracle.LongVectorLength);
        Assert.AreEqual(table.ByteVector1.Value.Length, oracle.ByteVector1Length);
        Assert.AreEqual(table.ByteVector2.Value.Length, oracle.ByteVector2Length);
        Assert.AreEqual(table.ByteVector3.Length, oracle.ByteVector3Length);

        for (int i = 0; i < table.IntVector.Count; ++i)
        {
            Assert.AreEqual(table.IntVector[i], oracle.IntVector(i));
        }

        for (int i = 0; i < table.LongVector.Count; ++i)
        {
            Assert.AreEqual(table.LongVector[i], oracle.LongVector(i));
        }

        for (int i = 0; i < table.ByteVector1.Value.Length; ++i)
        {
            Assert.AreEqual(table.ByteVector1.Value.Span[i], oracle.ByteVector1(i));
        }

        for (int i = 0; i < table.ByteVector2.Value.Length; ++i)
        {
            Assert.AreEqual(table.ByteVector2.Value.Span[i], oracle.ByteVector2(i));
        }
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.FiveByteStructTableVerify.Verify(new Verifier(bb), 4));
        var oracle = Flatc.FiveByteStructTable.GetRootAsFiveByteStructTable(bb);

        Assert.AreEqual(3, oracle.VectorLength);

        Assert.AreEqual(1, oracle.Vector(0).Value.Int);
        Assert.AreEqual((byte)1, oracle.Vector(0).Value.Byte);

        Assert.AreEqual(2, oracle.Vector(1).Value.Int);
        Assert.AreEqual((byte)2, oracle.Vector(1).Value.Byte);

        Assert.AreEqual(3, oracle.Vector(2).Value.Int);
        Assert.AreEqual((byte)3, oracle.Vector(2).Value.Byte);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.AreEqual(1, (int)oracle.ValueType);
        var bt = oracle.Value<Flatc.BasicTypes>().Value;

        Assert.IsTrue(bt.Bool);
        Assert.AreEqual(bt.Byte, simple.Byte);
        Assert.AreEqual(bt.SByte, simple.SByte);

        Assert.AreEqual(bt.UShort, simple.UShort);
        Assert.AreEqual(bt.Short, simple.Short);

        Assert.AreEqual(bt.UInt, simple.UInt);
        Assert.AreEqual(bt.Int, simple.Int);

        Assert.AreEqual(bt.ULong, simple.ULong);
        Assert.AreEqual(bt.Long, simple.Long);

        Assert.AreEqual(bt.Float, simple.Float);
        Assert.AreEqual(bt.Double, simple.Double);
        Assert.AreEqual(bt.String, simple.String);
    }

    [TestMethod]
    public void Union_String()
    {
        var table = new FS.UnionTable
        {
            Value = new("foobar")
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.IsTrue(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.AreEqual(3, (int)oracle.ValueType);
        Assert.AreEqual("foobar", oracle.ValueAsString());
    }

    [TestMethod]
    public void Union_NotSet()
    {
        var table = new FS.UnionTable
        {
            Value = null,
        };

        Span<byte> memory = new byte[10240];
        int size = FS.UnionTable.Serializer.Write(memory, table);

        var bb = new ByteBuffer(memory.Slice(0, size).ToArray());
        Assert.IsTrue(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.AreEqual(0, (int)oracle.ValueType);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.UnionTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionTable.GetRootAsUnionTable(bb);

        Assert.AreEqual(2, (int)oracle.ValueType);
        var bt = oracle.Value<Flatc.Location>().Value;

        Assert.AreEqual(bt.X, location.X);
        Assert.AreEqual(bt.Y, location.Y);
        Assert.AreEqual(bt.Z, location.Z);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.NestedStructsVerify.Verify(new(bb), 4));
        var oracle = Flatc.NestedStructs.GetRootAsNestedStructs(bb);

        Assert.IsNotNull(oracle.Outer);

        var outer = oracle.Outer.Value;
        Assert.AreEqual(100, outer.A);
        Assert.AreEqual(401, outer.Inner.A);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.NestedStructsVerify.Verify(new(bb), 4));
        var oracle = Flatc.NestedStructs.GetRootAsNestedStructs(bb);

        Assert.IsNotNull(oracle.Outer);

        var outer = oracle.Outer.Value;
        Assert.AreEqual(100, outer.A);
        Assert.AreEqual(0, outer.Inner.A);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.UnionVectorTableVerify.Verify(new(bb), 4));
        var oracle = Flatc.UnionVectorTable.GetRootAsUnionVectorTable(bb);

        Assert.AreEqual(3, oracle.ValueLength);

        Assert.AreEqual(Flatc.Union.BasicTypes, oracle.ValueType(0));
        Assert.AreEqual(Flatc.Union.Location, oracle.ValueType(1));
        Assert.AreEqual(Flatc.Union.stringValue, oracle.ValueType(2));

        var basicTypes = oracle.Value<Flatc.BasicTypes>(0).Value;
        Assert.AreEqual(7, basicTypes.Int);

        var location = oracle.Value<Flatc.Location>(1).Value;
        Assert.AreEqual(1, location.X);
        Assert.AreEqual(2, location.Y);
        Assert.AreEqual(3, location.Z);

        string stringValue = oracle.ValueAsString(2);
        Assert.AreEqual("foobar", stringValue);
    }

    [TestMethod]
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
        Assert.IsTrue(Flatc.SortedVectorTestVerify.Verify(new(bb), 4));
        var table = Flatc.SortedVectorTest.GetRootAsSortedVectorTest(bb);

        VerifySorted<IList<FS.SortedVectorStringTable>, FS.SortedVectorStringTable, string>(parsed.String, i => i.Value, i => table.String(i)?.Value, t => table.StringByKey(t) != null, new Utf8StringComparer());
        VerifySorted<IList<FS.SortedVectorInt32Table>, FS.SortedVectorInt32Table, int>(parsed.Int32, i => i.Value, i => table.Int32(i).Value.Value, t => table.Int32ByKey(t) != null, Comparer<int>.Default);
        VerifySorted<IList<FS.SortedVectorDoubleTable>, FS.SortedVectorDoubleTable, double>(parsed.Double, i => i.Value, i => table.Double(i).Value.Value, t => table.DoubleByKey(t) != null, Comparer<double>.Default);
    }

    [TestMethod]
    public void SortedVectors_DefaultValue()
    {
        var test = new FS.SortedVectorTest
        {
            Int32 = new List<FS.SortedVectorInt32Table>(),
        };

        var myItem = new FS.SortedVectorInt32Table();
        Assert.AreEqual(5, myItem.Value);

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
        Assert.IsTrue(Flatc.SortedVectorTestVerify.Verify(new(bb), 4));
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
        Assert.IsTrue(oracleContains(previous));

        for (int i = 0; i < items.Count; ++i)
        {
            TKey current = keyGet(items[i]);
            TKey oracle = oracleGet(i);

            Assert.IsTrue(comparer.Compare(previous, current) <= 0, $"Expect: {previous} <= {current}");
            Assert.IsTrue(comparer.Compare(previous, oracle) <= 0, $"Expect: {previous} <= {oracle}");
            Assert.IsTrue(comparer.Compare(current, oracle) == 0, $"Expect: {current} == {oracle}");

            // FlatBuffers c# has a bug where binary search is broken when using default values.
            Assert.IsTrue(oracleContains(current));
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
