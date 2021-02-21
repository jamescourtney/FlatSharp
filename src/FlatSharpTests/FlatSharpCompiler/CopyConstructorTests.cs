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
#if NET5_0

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CopyConstructorTests
    {
        [TestMethod]
        public void CopyConstructorsTest()
        {
            string schema = @"
namespace CopyConstructorTest;

union Union { OuterTable, InnerTable, OuterStruct, InnerStruct } // Optionally add more tables.

table OuterTable (PrecompiledSerializer: ""Lazy"") {
  A:string;

  B:byte;
  C:ubyte;
  D:int16;
  E:uint16;
  F:int32;
  G:uint32;
  H:int64;
  I:uint64;
  
  IntVector_List:[int] (VectorType:""IList"");
  IntVector_RoList:[int] (VectorType:""IReadOnlyList"");
  IntVector_Array:[int] (VectorType:""Array"");
  
  TableVector_List:[InnerTable] (VectorType:""IList"");
  TableVector_RoList:[InnerTable] (VectorType:""IReadOnlyList"");
  TableVector_Indexed:[InnerTable] (VectorType:""IIndexedVector"");
  TableVector_Array:[InnerTable] (VectorType:""Array"");

  ByteVector:[ubyte] (VectorType:""Memory"");
  ByteVector_RO:[ubyte] (VectorType:""ReadOnlyMemory"");
}

struct OuterStruct {
    Value:int;
    InnerStruct:InnerStruct;
}

struct InnerStruct {
    LongValue:int64;
}

table InnerTable {
  Name:string (key);
  OuterStruct:OuterStruct;
}

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
                
                TableVector_Array = CreateInner("Rocket", "Molly", "Jingle"),
                TableVector_Indexed = new IndexedVector<string, InnerTable>(CreateInner("Pudge", "Sunshine", "Gypsy"), false),
                TableVector_List = CreateInner("Finnegan", "Daisy"),
                TableVector_RoList = CreateInner("Gordita", "Lunchbox"),
            };

            byte[] data = new byte[FlatBufferSerializer.Default.GetMaxSize(original)];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(original, data);

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type outerTableType = asm.GetType("CopyConstructorTest.OuterTable");
            dynamic serializer = outerTableType.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            dynamic parsed = serializer.Parse(new ArrayInputBuffer(data));
            dynamic copied = Activator.CreateInstance(outerTableType, (object)parsed);

            Assert.AreEqual(original.A, copied.A);
            Assert.IsFalse(ReferenceEquals(parsed.A, copied.A));

            Assert.AreEqual(original.B, copied.B);
            Assert.AreEqual(original.C, copied.C);
            Assert.AreEqual(original.D, copied.D);
            Assert.AreEqual(original.E, copied.E);
            Assert.AreEqual(original.F, copied.F);
            Assert.AreEqual(original.G, copied.G);
            Assert.AreEqual(original.H, copied.H);
            Assert.AreEqual(original.I, copied.I);

            Memory<byte>? mem = copied.ByteVector;
            Assert.IsTrue(original.ByteVector.Value.Span.SequenceEqual(mem.Value.Span));

            ReadOnlyMemory<byte>? roMem = copied.ByteVector_RO;
            Assert.IsTrue(original.ByteVector_RO.Value.Span.SequenceEqual(roMem.Value.Span));

            // array of table
            {
                int count = original.TableVector_Array.Length;
                Assert.IsFalse(object.ReferenceEquals(parsed.TableVector_Array, copied.TableVector_Array));
                for (int i = 0; i < count; ++i)
                {
                    var p = parsed.TableVector_Array[i];
                    var c = copied.TableVector_Array[i];
                    DeepCompareInnerTable(original.TableVector_Array[i], p, c);
                }
            }

            // list of table.
            {
                Assert.IsFalse(object.ReferenceEquals(parsed.TableVector_List, copied.TableVector_List));

                IEnumerable<object> parsedEnum = AsObjectEnumerable(parsed.TableVector_List);
                IEnumerable<object> copiedEnum = AsObjectEnumerable(copied.TableVector_List);

                foreach (var ((p, c), o) in parsedEnum.Zip(copiedEnum).Zip(original.TableVector_List))
                {
                    DeepCompareInnerTable(o, p, c);
                }
            }

            // read only list of table.
            {
                Assert.IsFalse(object.ReferenceEquals(parsed.TableVector_RoList, copied.TableVector_RoList));

                IEnumerable<object> parsedEnum = AsObjectEnumerable(parsed.TableVector_RoList);
                IEnumerable<object> copiedEnum = AsObjectEnumerable(copied.TableVector_RoList);

                foreach (var ((p, c), o) in parsedEnum.Zip(copiedEnum).Zip(original.TableVector_RoList))
                {
                    DeepCompareInnerTable(o, p, c);
                }
            }

            // indexed vector of table.
            {
                Assert.IsFalse(object.ReferenceEquals(parsed.TableVector_Indexed, copied.TableVector_Indexed));
                foreach (var kvp in original.TableVector_Indexed)
                {
                    string key = kvp.Key;
                    InnerTable? value = kvp.Value;

                    var parsedValue = parsed.TableVector_Indexed[key];
                    var copiedValue = copied.TableVector_Indexed[key];

                    Assert.IsNotNull(parsedValue);
                    Assert.IsNotNull(copiedValue);

                    DeepCompareInnerTable(value, parsedValue, copiedValue);
                }
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
            Assert.IsFalse(object.ReferenceEquals(p, c));
            Assert.IsFalse(object.ReferenceEquals(p.OuterStruct, c.OuterStruct));
            Assert.IsFalse(object.ReferenceEquals(p.OuterStruct.InnerStruct, c.OuterStruct.InnerStruct));

            Assert.AreEqual(a.Name, p.Name);
            Assert.AreEqual(a.Name, c.Name);

            Assert.AreEqual(a.OuterStruct.Value, p.OuterStruct.Value);
            Assert.AreEqual(a.OuterStruct.InnerStruct.LongValue, p.OuterStruct.InnerStruct.LongValue);

            Assert.AreEqual(a.OuterStruct.Value, c.OuterStruct.Value);
            Assert.AreEqual(a.OuterStruct.InnerStruct.LongValue, c.OuterStruct.InnerStruct.LongValue);
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
        }

        [FlatBufferTable]
        public class InnerTable
        {
            [FlatBufferItem(0, Key = true)]
            public string? Name { get; set; }

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
}

#endif
