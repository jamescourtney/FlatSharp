/*
 * Copyright 2021 James Courtney
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

namespace FlatSharpEndToEndTests.CopyConstructors;

public class CopyConstructorTests
{
    [Theory]
    [InlineData(FlatBufferDeserializationOption.Greedy)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable)]
    [InlineData(FlatBufferDeserializationOption.Lazy)]
    [InlineData(FlatBufferDeserializationOption.Progressive)]
    public void CopyConstructorsTest(FlatBufferDeserializationOption option)
    {
        OuterTable original = new OuterTable
        {
            A = "string",
            B = 1,
            C = 2,
            D = 3,
            E = 4,
            F = 5,
            G = 6,
            H = 7,
            I = 8,

            ByteVector = new byte[] { 1, 2, 3, }.AsMemory(),
            ByteVector_RO = new byte[] { 4, 5, 6, }.AsMemory(),

            IntVector_Array = new[] { 7, 8, 9, },
            IntVector_List = new[] { 10, 11, 12, }.ToList(),
            IntVector_RoList = new[] { 13, 14, 15 }.ToList(),

            TableVector_Array = CreateInner("Rocket", "Molly", "Clementine"),
            TableVector_Indexed = new IndexedVector<string, InnerTable>(CreateInner("Pudge", "Sunshine", "Gypsy"), false),
            TableVector_List = CreateInner("Finnegan", "Daisy"),
            TableVector_RoList = CreateInner("Gordita", "Lunchbox"),

            UnionVal = new Union(new OuterStruct()),
            VectorOfUnion = new List<Union>
            {
                new(new OuterTable()),
                new(new InnerTable() { Name = "a" }),
                new(new OuterStruct()),
                new(new InnerStruct()),
            },
            VectorOfUnion_RoList = new List<Union>
            {
                new(new OuterTable()),
                new(new InnerTable() { Name = "b" }),
                new(new OuterStruct()),
                new(new InnerStruct()),
            },
            VectorOfUnion_Array = new Union[]
            {
                new(new OuterTable()),
                new(new InnerTable() { Name = "c" }),
                new(new OuterStruct()),
                new(new InnerStruct()),
            }
        };

        byte[] data = new byte[OuterTable.Serializer.GetMaxSize(original)];
        int bytesWritten = OuterTable.Serializer.Write(data, original);

        var parsed = OuterTable.Serializer.Parse(data, option);
        OuterTable copied = new OuterTable(parsed);

        // Strings can be copied by reference since they are immutable.
        Assert.Equal(original.A, copied.A);
        Assert.Equal(original.A, parsed.A);

        Assert.Equal(original.B, copied.B);
        Assert.Equal(original.C, copied.C);
        Assert.Equal(original.D, copied.D);
        Assert.Equal(original.E, copied.E);
        Assert.Equal(original.F, copied.F);
        Assert.Equal(original.G, copied.G);
        Assert.Equal(original.H, copied.H);
        Assert.Equal(original.I, copied.I);

        DeepCompareIntVector(original.IntVector_Array, parsed.IntVector_Array, copied.IntVector_Array);
        DeepCompareIntVector(original.IntVector_List, parsed.IntVector_List, copied.IntVector_List);
        DeepCompareIntVector(original.IntVector_RoList, parsed.IntVector_RoList, copied.IntVector_RoList);

        Assert.Equal((byte)3, original.UnionVal.Value.Discriminator);
        Assert.Equal((byte)3, parsed.UnionVal.Value.Discriminator);
        Assert.Equal((byte)3, copied.UnionVal.Value.Discriminator);

        Assert.True(copied.UnionVal.Value.TryGet(out OuterStruct? _));
        Assert.NotNull(copied.UnionVal.Value.Item3);

        Assert.IsType<OuterStruct>(copied.UnionVal.Value.Item3);
        Assert.IsNotType<OuterStruct>(parsed.UnionVal.Value.Item3);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.Equal((byte)i, original.VectorOfUnion[i - 1].Discriminator);
            Assert.Equal((byte)i, parsed.VectorOfUnion[i - 1].Discriminator);
            Assert.Equal((byte)i, copied.VectorOfUnion[i - 1].Discriminator);
        }

        Assert.NotSame(parsed.VectorOfUnion[0].Item1, copied.VectorOfUnion[0].Item1);
        Assert.NotSame(parsed.VectorOfUnion[1].Item2, copied.VectorOfUnion[1].Item2);
        Assert.NotSame(parsed.VectorOfUnion[2].Item3, copied.VectorOfUnion[2].Item3);
        Assert.NotSame(parsed.VectorOfUnion[3].Item4, copied.VectorOfUnion[3].Item4);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.Equal((byte)i, original.VectorOfUnion_RoList[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)parsed.VectorOfUnion_RoList[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)copied.VectorOfUnion_RoList[i - 1].Discriminator);
        }

        Assert.NotSame(parsed.VectorOfUnion_RoList[0].Item1, copied.VectorOfUnion_RoList[0].Item1);
        Assert.NotSame(parsed.VectorOfUnion_RoList[1].Item2, copied.VectorOfUnion_RoList[1].Item2);
        Assert.NotSame(parsed.VectorOfUnion_RoList[2].Item3, copied.VectorOfUnion_RoList[2].Item3);
        Assert.NotSame(parsed.VectorOfUnion_RoList[3].Item4, copied.VectorOfUnion_RoList[3].Item4);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.Equal((byte)i, original.VectorOfUnion_Array[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)parsed.VectorOfUnion_Array[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)copied.VectorOfUnion_Array[i - 1].Discriminator);
        }

        Assert.NotSame(parsed.VectorOfUnion_Array[0].Item1, copied.VectorOfUnion_Array[0].Item1);
        Assert.NotSame(parsed.VectorOfUnion_Array[1].Item2, copied.VectorOfUnion_Array[1].Item2);
        Assert.NotSame(parsed.VectorOfUnion_Array[2].Item3, copied.VectorOfUnion_Array[2].Item3);
        Assert.NotSame(parsed.VectorOfUnion_Array[3].Item4, copied.VectorOfUnion_Array[3].Item4);

        Memory<byte>? mem = copied.ByteVector;
        Memory<byte>? pMem = parsed.ByteVector;
        Assert.True(original.ByteVector.Value.Span.SequenceEqual(mem.Value.Span));
        Assert.False(mem.Value.Span.Overlaps(pMem.Value.Span));

        ReadOnlyMemory<byte>? roMem = copied.ByteVector_RO;
        ReadOnlyMemory<byte>? pRoMem = parsed.ByteVector_RO;
        Assert.True(original.ByteVector_RO.Value.Span.SequenceEqual(roMem.Value.Span));
        Assert.False(roMem.Value.Span.Overlaps(pRoMem.Value.Span));

        // array of table
        {
            Assert.NotSame(parsed.TableVector_Array, copied.TableVector_Array);
            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVector_Array,
                (IReadOnlyList<InnerTable>)parsed.TableVector_Array,
                (IReadOnlyList<InnerTable>)copied.TableVector_Array);
        }

        // list of table.
        {
            Assert.NotSame(parsed.TableVector_List, copied.TableVector_List);

            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVector_List,
                (IReadOnlyList<InnerTable>)parsed.TableVector_List,
                (IReadOnlyList<InnerTable>)copied.TableVector_List);
        }

        // read only list of table.
        {
            Assert.NotSame(parsed.TableVector_RoList, copied.TableVector_RoList);

            DeepCompareInnerTableList(
                original.TableVector_RoList,
                parsed.TableVector_RoList,
                copied.TableVector_RoList);
        }

        // indexed vector of table.
        {
            Assert.NotSame(parsed.TableVector_Indexed, copied.TableVector_Indexed);
            foreach (var kvp in original.TableVector_Indexed)
            {
                string key = kvp.Key;
                InnerTable? value = kvp.Value;

                var parsedValue = parsed.TableVector_Indexed[key];
                var copiedValue = copied.TableVector_Indexed[key];

                Assert.NotNull(parsedValue);
                Assert.NotNull(copiedValue);

                DeepCompareInnerTable(value, parsedValue, copiedValue);
            }
        }
    }

    private void DeepCompareIntVector(IReadOnlyList<int> a, IReadOnlyList<int> b, IReadOnlyList<int> c)
    {
        Assert.Equal(a.Count, b.Count);
        Assert.Equal(a.Count, c.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            Assert.Equal(a[i], b[i]);
            Assert.Equal(a[i], c[i]);
        }
    }

    private void DeepCompareIntVector(IList<int> a, IList<int> b, IList<int> c)
    {
        Assert.Equal(a.Count, b.Count);
        Assert.Equal(a.Count, c.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            Assert.Equal(a[i], b[i]);
            Assert.Equal(a[i], c[i]);
        }
    }

    private InnerTable[] CreateInner(params string[] names)
    {
        InnerTable[] table = new InnerTable[names.Length];
        for (int i = 0; i < names.Length; ++i)
        {
            table[i] = new InnerTable
            {
                Name = names[i],
                OuterStructVal = new OuterStruct
                {
                    Value = 1,
                    InnerStructVal = new InnerStruct
                    {
                        LongValue = 2,
                    },
                }
            };
        }

        return table;
    }

    private static void DeepCompareInnerTableList(IReadOnlyList<InnerTable> aList, IReadOnlyList<InnerTable> pList, IReadOnlyList<InnerTable> cList)
    {
        Assert.Equal(aList.Count, pList.Count);
        Assert.Equal(pList.Count, cList.Count);

        for (int i = 0; i < pList.Count; ++i)
        {
            var p = pList[i];
            var a = aList[i];
            var c = cList[i];

            DeepCompareInnerTable(a, p, c);
        }
    }

    private static void DeepCompareInnerTable(InnerTable a, InnerTable p, InnerTable c)
    {
        Assert.NotSame(p, c);

        Assert.IsNotType<InnerTable>(p);
        Assert.IsType<InnerTable>(c);
        Assert.IsType<InnerTable>(a);

        Assert.Equal(a.Name, p.Name);
        Assert.Equal(a.Name, c.Name);

        var aOuter = a.OuterStructVal;
        var pOuter = p.OuterStructVal;
        var cOuter = c.OuterStructVal;

        Assert.NotSame(pOuter, cOuter);

        Assert.IsType<OuterStruct>(aOuter);
        Assert.IsType<OuterStruct>(cOuter);
        Assert.IsNotType<OuterStruct>(pOuter);

        Assert.Equal(aOuter.Value, pOuter.Value);
        Assert.Equal(aOuter.Value, cOuter.Value);

        var pInner = pOuter.InnerStructVal;
        var cInner = cOuter.InnerStructVal;
        var aInner = aOuter.InnerStructVal;

        Assert.NotSame(pInner, cInner);

        Assert.IsType<InnerStruct>(aInner);
        Assert.IsType<InnerStruct>(cInner);
        Assert.IsNotType<InnerStruct>(pInner);

        Assert.Equal(aInner.LongValue, pInner.LongValue);
        Assert.Equal(aInner.LongValue, cInner.LongValue);
    }
}
