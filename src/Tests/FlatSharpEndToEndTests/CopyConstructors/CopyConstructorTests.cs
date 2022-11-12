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
            ByteVectorRO = new byte[] { 4, 5, 6, }.AsMemory(),

            IntVectorArray = new[] { 7, 8, 9, },
            IntVectorList = new[] { 10, 11, 12, }.ToList(),
            IntVectorRoList = new[] { 13, 14, 15 }.ToList(),

            TableVectorArray = CreateInner("Rocket", "Molly", "Clementine"),
            TableVectorIndexed = new IndexedVector<string, InnerTable>(CreateInner("Pudge", "Sunshine", "Gypsy"), false),
            TableVectorList = CreateInner("Finnegan", "Daisy"),
            TableVectorRoList = CreateInner("Gordita", "Lunchbox"),

            UnionVal = new Union(new OuterStruct()),
            VectorOfUnion = new List<Union>
            {
                new(new OuterTable()),
                new(new InnerTable() { Name = "a" }),
                new(new OuterStruct()),
                new(new InnerStruct()),
            },
            VectorOfUnionRoList = new List<Union>
            {
                new(new OuterTable()),
                new(new InnerTable() { Name = "b" }),
                new(new OuterStruct()),
                new(new InnerStruct()),
            },
            VectorOfUnionArray = new Union[]
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

        DeepCompareIntVector(original.IntVectorArray, parsed.IntVectorArray, copied.IntVectorArray);
        DeepCompareIntVector(original.IntVectorList, parsed.IntVectorList, copied.IntVectorList);
        DeepCompareIntVector(original.IntVectorRoList, parsed.IntVectorRoList, copied.IntVectorRoList);

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
            Assert.Equal((byte)i, original.VectorOfUnionRoList[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)parsed.VectorOfUnionRoList[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)copied.VectorOfUnionRoList[i - 1].Discriminator);
        }

        Assert.NotSame(parsed.VectorOfUnionRoList[0].Item1, copied.VectorOfUnionRoList[0].Item1);
        Assert.NotSame(parsed.VectorOfUnionRoList[1].Item2, copied.VectorOfUnionRoList[1].Item2);
        Assert.NotSame(parsed.VectorOfUnionRoList[2].Item3, copied.VectorOfUnionRoList[2].Item3);
        Assert.NotSame(parsed.VectorOfUnionRoList[3].Item4, copied.VectorOfUnionRoList[3].Item4);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.Equal((byte)i, original.VectorOfUnionArray[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)parsed.VectorOfUnionArray[i - 1].Discriminator);
            Assert.Equal((byte)i, (byte)copied.VectorOfUnionArray[i - 1].Discriminator);
        }

        Assert.NotSame(parsed.VectorOfUnionArray[0].Item1, copied.VectorOfUnionArray[0].Item1);
        Assert.NotSame(parsed.VectorOfUnionArray[1].Item2, copied.VectorOfUnionArray[1].Item2);
        Assert.NotSame(parsed.VectorOfUnionArray[2].Item3, copied.VectorOfUnionArray[2].Item3);
        Assert.NotSame(parsed.VectorOfUnionArray[3].Item4, copied.VectorOfUnionArray[3].Item4);

        Memory<byte>? mem = copied.ByteVector;
        Memory<byte>? pMem = parsed.ByteVector;
        Assert.True(original.ByteVector.Value.Span.SequenceEqual(mem.Value.Span));
        Assert.False(mem.Value.Span.Overlaps(pMem.Value.Span));

        ReadOnlyMemory<byte>? roMem = copied.ByteVectorRO;
        ReadOnlyMemory<byte>? pRoMem = parsed.ByteVectorRO;
        Assert.True(original.ByteVectorRO.Value.Span.SequenceEqual(roMem.Value.Span));
        Assert.False(roMem.Value.Span.Overlaps(pRoMem.Value.Span));

        // array of table
        {
            Assert.NotSame(parsed.TableVectorArray, copied.TableVectorArray);
            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVectorArray,
                (IReadOnlyList<InnerTable>)parsed.TableVectorArray,
                (IReadOnlyList<InnerTable>)copied.TableVectorArray);
        }

        // list of table.
        {
            Assert.NotSame(parsed.TableVectorList, copied.TableVectorList);

            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVectorList,
                (IReadOnlyList<InnerTable>)parsed.TableVectorList,
                (IReadOnlyList<InnerTable>)copied.TableVectorList);
        }

        // read only list of table.
        {
            Assert.NotSame(parsed.TableVectorRoList, copied.TableVectorRoList);

            DeepCompareInnerTableList(
                original.TableVectorRoList,
                parsed.TableVectorRoList,
                copied.TableVectorRoList);
        }

        // indexed vector of table.
        {
            Assert.NotSame(parsed.TableVectorIndexed, copied.TableVectorIndexed);
            foreach (var kvp in original.TableVectorIndexed)
            {
                string key = kvp.Key;
                InnerTable? value = kvp.Value;

                var parsedValue = parsed.TableVectorIndexed[key];
                var copiedValue = copied.TableVectorIndexed[key];

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
