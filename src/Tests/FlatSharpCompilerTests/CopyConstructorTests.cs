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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

#if NETCOREAPP3_1_OR_GREATER

    
    public class CopyConstructorTests
    {
        [Fact]
        public void CopyConstructorsTest()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace CopyConstructorTest;

union Union {{ OuterTable, InnerTable, OuterStruct, InnerStruct }} // Optionally add more tables.

table OuterTable ({MetadataKeys.SerializerKind}: ""Greedy"") {{
  A:string (id: 0);

  B:byte   (id: 1);
  C:ubyte  (id: 2);
  D:int16  (id: 3); 
  E:uint16 (id: 4);
  F:int32  (id: 5);
  G:uint32 (id: 6);
  H:int64  (id: 7);
  I:uint64 (id: 8);
  
  IntVector_List:[int] ({MetadataKeys.VectorKind}:""IList"", id: 9);
  IntVector_RoList:[int] ({MetadataKeys.VectorKind}:""IReadOnlyList"", id: 10);
  IntVector_Array:[int] ({MetadataKeys.VectorKind}:""Array"", id: 11);
  
  TableVector_List:[InnerTable] ({MetadataKeys.VectorKind}:""IList"", id: 12);
  TableVector_RoList:[InnerTable] ({MetadataKeys.VectorKind}:""IReadOnlyList"", id: 13);
  TableVector_Indexed:[InnerTable] ({MetadataKeys.VectorKind}:""IIndexedVector"", id: 14);
  TableVector_Array:[InnerTable] ({MetadataKeys.VectorKind}:""Array"", id: 15);

  ByteVector:[ubyte] ({MetadataKeys.VectorKind}:""Memory"", id: 16);
  ByteVector_RO:[ubyte] ({MetadataKeys.VectorKind}:""ReadOnlyMemory"", id: 17);
  UnionVal : Union (id: 19);

  VectorOfUnion : [Union] (id: 21);
  VectorOfUnion_RoList : [Union] (id: 23, {MetadataKeys.VectorKind}:""IReadOnlyList"");
  VectorOfUnion_Array : [Union] (id: 25, {MetadataKeys.VectorKind}:""Array"");
}}

struct InnerStruct {{
    LongValue:int64;
}}

struct OuterStruct {{
    Value:int;
    InnerStructVal:InnerStruct;
}}

table InnerTable {{
  Name:string ({MetadataKeys.Key});
  OuterStructVal:OuterStruct;
}}

";
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

                UnionVal = new FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>(new OuterStruct()),
                VectorOfUnion = new List<FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>>
                {
                    new(new OuterTable()),
                    new(new InnerTable()),
                    new(new OuterStruct()),
                    new(new InnerStruct()),
                },
                VectorOfUnion_RoList = new List<FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>>
                {
                    new(new OuterTable()),
                    new(new InnerTable()),
                    new(new OuterStruct()),
                    new(new InnerStruct()),
                },
                VectorOfUnion_Array = new FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>[]
                {
                    new(new OuterTable()),
                    new(new InnerTable()),
                    new(new OuterStruct()),
                    new(new InnerStruct()),
                }
            };

            byte[] data = new byte[FlatBufferSerializer.Default.GetMaxSize(original)];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(original, data);

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type outerTableType = asm.GetType("CopyConstructorTest.OuterTable");
            dynamic serializer = outerTableType.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            object parsedObj = serializer.Parse(new ArrayInputBuffer(data));
            dynamic parsed = parsedObj;
            dynamic copied = Activator.CreateInstance(outerTableType, (object)parsed);
            //dynamic copied = new CopyConstructorTest.OuterTable((CopyConstructorTest.OuterTable)parsedObj);

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
            Assert.Equal((byte)3, parsed.UnionVal.Discriminator);
            Assert.Equal((byte)3, copied.UnionVal.Discriminator);
            Assert.Equal("CopyConstructorTest.OuterStruct", copied.UnionVal.Item3.GetType().FullName);
            Assert.NotEqual("CopyConstructorTest.OuterStruct", parsed.UnionVal.Item3.GetType().FullName);
            Assert.NotSame(parsed.UnionVal, copied.UnionVal);
            Assert.NotSame(parsed.UnionVal.Item3, copied.UnionVal.Item3);

            for (int i = 1; i <= 4; ++i)
            {
                Assert.Equal((byte)i, original.VectorOfUnion[i - 1].Discriminator);
                Assert.Equal((byte)i, (byte)parsed.VectorOfUnion[i - 1].Discriminator);
                Assert.Equal((byte)i, (byte)copied.VectorOfUnion[i - 1].Discriminator);
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
                int count = original.TableVector_Array.Length;
                Assert.NotSame(parsed.TableVector_Array, copied.TableVector_Array);
                for (int i = 0; i < count; ++i)
                {
                    var p = parsed.TableVector_Array[i];
                    var c = copied.TableVector_Array[i];
                    DeepCompareInnerTable(original.TableVector_Array[i], p, c);
                }
            }

            // list of table.
            {
                Assert.False(object.ReferenceEquals(parsed.TableVector_List, copied.TableVector_List));

                IEnumerable<object> parsedEnum = AsObjectEnumerable(parsed.TableVector_List);
                IEnumerable<object> copiedEnum = AsObjectEnumerable(copied.TableVector_List);

                foreach (var ((p, c), o) in parsedEnum.Zip(copiedEnum).Zip(original.TableVector_List))
                {
                    DeepCompareInnerTable(o, p, c);
                }
            }

            // read only list of table.
            {
                Assert.False(object.ReferenceEquals(parsed.TableVector_RoList, copied.TableVector_RoList));

                IEnumerable<object> parsedEnum = AsObjectEnumerable(parsed.TableVector_RoList);
                IEnumerable<object> copiedEnum = AsObjectEnumerable(copied.TableVector_RoList);

                foreach (var ((p, c), o) in parsedEnum.Zip(copiedEnum).Zip(original.TableVector_RoList))
                {
                    DeepCompareInnerTable(o, p, c);
                }
            }

            // indexed vector of table.
            {
                Assert.False(object.ReferenceEquals(parsed.TableVector_Indexed, copied.TableVector_Indexed));
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

        private void DeepCompareIntVector(IEnumerable<int> a, IEnumerable<int> b, IEnumerable<int> c)
        {
            foreach (var pair in a.Zip(b).Zip(c))
            {
                int i = pair.First.First;
                int ii = pair.First.Second;
                int iii = pair.Second;

                Assert.Equal(i, ii);
                Assert.Equal(ii, iii);
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
                    OuterStruct = new OuterStruct
                    {
                        Value = 1,
                        InnerStruct = new InnerStruct
                        {
                            LongValue = 2,
                        },
                    }
                };
            }

            return table;
        }

        private static void DeepCompareInnerTable(InnerTable a, dynamic p, dynamic c)
        {
            Assert.NotSame(p, c);

            Assert.NotEqual("CopyConstructorTest.InnerTable", (string)p.GetType().FullName);
            Assert.Equal("CopyConstructorTest.InnerTable", (string)c.GetType().FullName);

            Assert.Equal(a.Name, p.Name);
            Assert.Equal(a.Name, c.Name);

            var pOuter = p.OuterStructVal;
            var cOuter = c.OuterStructVal;

            Assert.NotSame(pOuter, cOuter);
            Assert.NotEqual("CopyConstructorTest.OuterStruct", (string)pOuter.GetType().FullName);
            Assert.Equal("CopyConstructorTest.OuterStruct", (string)cOuter.GetType().FullName);

            Assert.Equal(a.OuterStruct.Value, pOuter.Value);
            Assert.Equal(a.OuterStruct.Value, cOuter.Value);

            var pInner = pOuter.InnerStructVal;
            var cInner = cOuter.InnerStructVal;

            Assert.NotSame(pInner, cInner);
            Assert.NotEqual("CopyConstructorTest.InnerStruct", (string)pInner.GetType().FullName);
            Assert.Equal("CopyConstructorTest.InnerStruct", (string)cInner.GetType().FullName);

            Assert.Equal(a.OuterStruct.InnerStruct.LongValue, pInner.LongValue);
            Assert.Equal(a.OuterStruct.InnerStruct.LongValue, cInner.LongValue);
        }

        [FlatBufferTable]
        public class OuterTable
        {
            [FlatBufferItem(0)]
            public string? A { get; set; }

            [FlatBufferItem(1)]
            public sbyte B { get; set; }

            [FlatBufferItem(2)]
            public byte C { get; set; }

            [FlatBufferItem(3)]
            public short D { get; set; }

            [FlatBufferItem(4)]
            public ushort E { get; set; }

            [FlatBufferItem(5)]
            public int F { get; set; }

            [FlatBufferItem(6)]
            public uint G { get; set; }

            [FlatBufferItem(7)]
            public long H { get; set; }

            [FlatBufferItem(8)]
            public ulong I { get; set; }

            [FlatBufferItem(9)]
            public IList<int>? IntVector_List { get; set; }

            [FlatBufferItem(10)]
            public IReadOnlyList<int>? IntVector_RoList { get; set; }

            [FlatBufferItem(11)]
            public int[]? IntVector_Array { get; set; }

            [FlatBufferItem(12)]
            public IList<InnerTable>? TableVector_List { get; set; }

            [FlatBufferItem(13)]
            public IReadOnlyList<InnerTable>? TableVector_RoList { get; set; }

            [FlatBufferItem(14)]
            public IIndexedVector<string, InnerTable>? TableVector_Indexed { get; set; }

            [FlatBufferItem(15)]
            public InnerTable[]? TableVector_Array { get; set; }

            [FlatBufferItem(16)]
            public Memory<byte>? ByteVector { get; set; }

            [FlatBufferItem(17)]
            public ReadOnlyMemory<byte>? ByteVector_RO { get; set; }

            [FlatBufferItem(18)]
            public FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>? UnionVal { get; set; }

            [FlatBufferItem(20)]
            public IList<FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>>? VectorOfUnion { get; set; }

            [FlatBufferItem(22)]
            public IReadOnlyList<FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>>? VectorOfUnion_RoList { get; set; }

            [FlatBufferItem(24)]
            public FlatBufferUnion<OuterTable, InnerTable, OuterStruct, InnerStruct>[]? VectorOfUnion_Array { get; set; }
        }

        [FlatBufferTable]
        public class InnerTable
        {
            [FlatBufferItem(0, Key = true, Required = true)]
            public string Name { get; set; } = "Foo";

            [FlatBufferItem(1)]
            public OuterStruct? OuterStruct { get; set; }
        }

        [FlatBufferStruct]
        public class OuterStruct
        {
            [FlatBufferItem(0)]
            public int Value { get; set; }

            [FlatBufferItem(1)]
            public InnerStruct InnerStruct { get; set; }
        }

        [FlatBufferStruct]
        public class InnerStruct
        {
            [FlatBufferItem(0)]
            public long LongValue { get; set; }
        }

        private static IEnumerable<object> AsObjectEnumerable(dynamic d)
        {
            System.Collections.IEnumerable enumerator = d;
            foreach (var item in enumerator)
            {
                yield return item;
            }
        }
    }

#endif
}