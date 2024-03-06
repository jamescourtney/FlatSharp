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

[TestClass]
public class CopyConstructorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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

        var parsed = original.SerializeAndParse(option);
        OuterTable copied = new OuterTable(parsed);

        // Strings can be copied by reference since they are immutable.
        Assert.AreEqual(original.A, copied.A);
        Assert.AreEqual(original.A, parsed.A);

        Assert.AreEqual(original.B, copied.B);
        Assert.AreEqual(original.C, copied.C);
        Assert.AreEqual(original.D, copied.D);
        Assert.AreEqual(original.E, copied.E);
        Assert.AreEqual(original.F, copied.F);
        Assert.AreEqual(original.G, copied.G);
        Assert.AreEqual(original.H, copied.H);
        Assert.AreEqual(original.I, copied.I);

        DeepCompareIntVector(original.IntVectorArray, parsed.IntVectorArray, copied.IntVectorArray);
        DeepCompareIntVector(original.IntVectorList, parsed.IntVectorList, copied.IntVectorList);
        DeepCompareIntVector(original.IntVectorRoList, parsed.IntVectorRoList, copied.IntVectorRoList);

        Assert.AreEqual((byte)3, original.UnionVal.Value.Discriminator);
        Assert.AreEqual((byte)3, parsed.UnionVal.Value.Discriminator);
        Assert.AreEqual((byte)3, copied.UnionVal.Value.Discriminator);

        Assert.IsTrue(copied.UnionVal.Value.TryGet(out OuterStruct? _));
        Assert.IsNotNull(copied.UnionVal.Value.Item3);

        Assert.IsInstanceOfType<OuterStruct>(copied.UnionVal.Value.Item3);
        Assert.AreNotEqual(typeof(OuterStruct), parsed.UnionVal.Value.Item3.GetType());

        for (int i = 1; i <= 4; ++i)
        {
            Assert.AreEqual((byte)i, original.VectorOfUnion[i - 1].Discriminator);
            Assert.AreEqual((byte)i, parsed.VectorOfUnion[i - 1].Discriminator);
            Assert.AreEqual((byte)i, copied.VectorOfUnion[i - 1].Discriminator);
        }

        Assert.AreNotSame(parsed.VectorOfUnion[0].Item1, copied.VectorOfUnion[0].Item1);
        Assert.AreNotSame(parsed.VectorOfUnion[1].Item2, copied.VectorOfUnion[1].Item2);
        Assert.AreNotSame(parsed.VectorOfUnion[2].Item3, copied.VectorOfUnion[2].Item3);
        Assert.AreNotSame(parsed.VectorOfUnion[3].Item4, copied.VectorOfUnion[3].Item4);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.AreEqual((byte)i, original.VectorOfUnionRoList[i - 1].Discriminator);
            Assert.AreEqual((byte)i, (byte)parsed.VectorOfUnionRoList[i - 1].Discriminator);
            Assert.AreEqual((byte)i, (byte)copied.VectorOfUnionRoList[i - 1].Discriminator);
        }

        Assert.AreNotSame(parsed.VectorOfUnionRoList[0].Item1, copied.VectorOfUnionRoList[0].Item1);
        Assert.AreNotSame(parsed.VectorOfUnionRoList[1].Item2, copied.VectorOfUnionRoList[1].Item2);
        Assert.AreNotSame(parsed.VectorOfUnionRoList[2].Item3, copied.VectorOfUnionRoList[2].Item3);
        Assert.AreNotSame(parsed.VectorOfUnionRoList[3].Item4, copied.VectorOfUnionRoList[3].Item4);

        for (int i = 1; i <= 4; ++i)
        {
            Assert.AreEqual((byte)i, original.VectorOfUnionArray[i - 1].Discriminator);
            Assert.AreEqual((byte)i, (byte)parsed.VectorOfUnionArray[i - 1].Discriminator);
            Assert.AreEqual((byte)i, (byte)copied.VectorOfUnionArray[i - 1].Discriminator);
        }

        Assert.AreNotSame(parsed.VectorOfUnionArray[0].Item1, copied.VectorOfUnionArray[0].Item1);
        Assert.AreNotSame(parsed.VectorOfUnionArray[1].Item2, copied.VectorOfUnionArray[1].Item2);
        Assert.AreNotSame(parsed.VectorOfUnionArray[2].Item3, copied.VectorOfUnionArray[2].Item3);
        Assert.AreNotSame(parsed.VectorOfUnionArray[3].Item4, copied.VectorOfUnionArray[3].Item4);

        Memory<byte>? mem = copied.ByteVector;
        Memory<byte>? pMem = parsed.ByteVector;
        Assert.IsTrue(original.ByteVector.Value.Span.SequenceEqual(mem.Value.Span));
        Assert.IsFalse(mem.Value.Span.Overlaps(pMem.Value.Span));

        ReadOnlyMemory<byte>? roMem = copied.ByteVectorRO;
        ReadOnlyMemory<byte>? pRoMem = parsed.ByteVectorRO;
        Assert.IsTrue(original.ByteVectorRO.Value.Span.SequenceEqual(roMem.Value.Span));
        Assert.IsFalse(roMem.Value.Span.Overlaps(pRoMem.Value.Span));

        // array of table
        {
            Assert.AreNotSame(parsed.TableVectorArray, copied.TableVectorArray);
            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVectorArray,
                (IReadOnlyList<InnerTable>)parsed.TableVectorArray,
                (IReadOnlyList<InnerTable>)copied.TableVectorArray);
        }

        // list of table.
        {
            Assert.AreNotSame(parsed.TableVectorList, copied.TableVectorList);

            DeepCompareInnerTableList(
                (IReadOnlyList<InnerTable>)original.TableVectorList,
                (IReadOnlyList<InnerTable>)parsed.TableVectorList,
                (IReadOnlyList<InnerTable>)copied.TableVectorList);
        }

        // read only list of table.
        {
            Assert.AreNotSame(parsed.TableVectorRoList, copied.TableVectorRoList);

            DeepCompareInnerTableList(
                original.TableVectorRoList,
                parsed.TableVectorRoList,
                copied.TableVectorRoList);
        }

        // indexed vector of table.
        {
            Assert.AreNotSame(parsed.TableVectorIndexed, copied.TableVectorIndexed);
            foreach (var kvp in original.TableVectorIndexed)
            {
                string key = kvp.Key;
                InnerTable? value = kvp.Value;

                var parsedValue = parsed.TableVectorIndexed[key];
                var copiedValue = copied.TableVectorIndexed[key];

                Assert.IsNotNull(parsedValue);
                Assert.IsNotNull(copiedValue);

                DeepCompareInnerTable(value, parsedValue, copiedValue);
            }
        }
    }

    private void DeepCompareIntVector(IReadOnlyList<int> a, IReadOnlyList<int> b, IReadOnlyList<int> c)
    {
        Assert.AreEqual(a.Count, b.Count);
        Assert.AreEqual(a.Count, c.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            Assert.AreEqual(a[i], b[i]);
            Assert.AreEqual(a[i], c[i]);
        }
    }

    private void DeepCompareIntVector(IList<int> a, IList<int> b, IList<int> c)
    {
        Assert.AreEqual(a.Count, b.Count);
        Assert.AreEqual(a.Count, c.Count);

        for (int i = 0; i < a.Count; ++i)
        {
            Assert.AreEqual(a[i], b[i]);
            Assert.AreEqual(a[i], c[i]);
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
        Assert.AreEqual(aList.Count, pList.Count);
        Assert.AreEqual(pList.Count, cList.Count);

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
        Assert.AreNotSame(p, c);

        Assert.AreNotEqual(typeof(InnerTable), p.GetType());
        Assert.IsInstanceOfType<InnerTable>(c);
        Assert.IsInstanceOfType<InnerTable>(a);

        Assert.AreEqual(a.Name, p.Name);
        Assert.AreEqual(a.Name, c.Name);

        var aOuter = a.OuterStructVal;
        var pOuter = p.OuterStructVal;
        var cOuter = c.OuterStructVal;

        Assert.AreNotSame(pOuter, cOuter);

        Assert.IsInstanceOfType<OuterStruct>(aOuter);
        Assert.IsInstanceOfType<OuterStruct>(cOuter);
        Assert.AreNotEqual(typeof(OuterStruct), pOuter.GetType());

        Assert.AreEqual(aOuter.Value, pOuter.Value);
        Assert.AreEqual(aOuter.Value, cOuter.Value);

        var pInner = pOuter.InnerStructVal;
        var cInner = cOuter.InnerStructVal;
        var aInner = aOuter.InnerStructVal;

        Assert.AreNotSame(pInner, cInner);

        Assert.IsInstanceOfType<InnerStruct>(aInner);
        Assert.IsInstanceOfType<InnerStruct>(cInner);
        Assert.AreNotEqual(typeof(InnerStruct), pInner.GetType());

        Assert.AreEqual(aInner.LongValue, pInner.LongValue);
        Assert.AreEqual(aInner.LongValue, cInner.LongValue);
    }
}
