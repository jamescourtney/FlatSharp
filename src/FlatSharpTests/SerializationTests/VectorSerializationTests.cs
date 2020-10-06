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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Binary format testing for vector serialization.
    /// </summary>
    [TestClass]
    public class VectorSerializationTests
    {
        [TestMethod]
        public void NullListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = null
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = new short[0],
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleListVector()
        {
            var root = new RootTable<IList<short>>
            {
                Vector = new short[] { 1, 2, 3, }
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length

                // vector data
                1, 0,
                2, 0,
                3, 0
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyMemoryVector()
        {
            var root = new RootTable<Memory<byte>>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleMemoryVector()
        {
            var root = new RootTable<Memory<byte>>
            {
                Vector = new byte[] { 1, 2, 3 },
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length
                1, 2, 3,             // True false true
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullString()
        {
            var root = new RootTable<string>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyString()
        {
            var root = new RootTable<string>
            {
                Vector = string.Empty,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to string
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
                0,                   // null terminator (special case for strings).
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleString()
        {
            var root = new RootTable<string>
            {
                Vector = new string(new char[] { (char)1, (char)2, (char)3 }),
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length
                1, 2, 3, 0,          // data + null terminator (special case for string vectors).
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void SimpleArray()
        {
            var root = new RootTable<long[]>
            {
                Vector = new[] { 1L, 2, 3 }
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length

                // vector data
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 0, 0, 0, 0,
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullArray()
        {
            var root = new RootTable<int[]>
            {
                Vector = default,
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,      // offset to table start
                252,255,255,255, // soffset to vtable (-4)
                4, 0,            // vtable length
                4, 0,            // table length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void EmptyArray()
        {
            var root = new RootTable<int[]>
            {
                Vector = new int[0],
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                0, 0, 0, 0,          // vector length
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void UnalignedStruct_5Byte()
        {
            var root = new RootTable<FiveByteStruct[]>
            {
                Vector = new[]
                {
                    new FiveByteStruct { Byte = 1, Int = 1 },
                    new FiveByteStruct { Byte = 2, Int = 2 },
                    new FiveByteStruct { Byte = 3, Int = 3 },
                },
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,          // offset to table start
                248, 255, 255, 255,  // soffset to vtable (-8)
                12, 0, 0, 0,         // uoffset_t to vector
                6, 0,                // vtable length
                8, 0,                // table length
                4, 0,                // offset of index 0 field
                0, 0,                // padding to 4-byte alignment
                3, 0, 0, 0,          // vector length
                1, 0, 0, 0,          // index 0.Int
                1,                   // index 0.Byte
                0, 0, 0,             // padding
                2, 0, 0, 0,          // index 1.Int
                2,                   // index 1.Byte
                0, 0, 0,             // padding
                3, 0, 0, 0,          // index2.Int
                3,                   // Index2.byte
                0, 0, 0,             // padding
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void UnalignedStruct_9Byte()
        {
            var root = new RootTable2<NineByteStruct[]>
            {
                Vector = new[]
                {
                    new NineByteStruct { Byte = 1, Long = 1 },
                    new NineByteStruct { Byte = 2, Long = 2 },
                },
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,                     // offset to table start
                244, 255, 255, 255,             // soffset to vtable (-12)
                0,                              // alignment imp
                0, 0, 0,                        // padding
                16, 0, 0, 0,                    // uoffset_t to vector

                8, 0,                           // vtable length
                12, 0,                          // table length
                4, 0,                           // offset to index 0 field
                8, 0,                           // offset of index 1 field

                0, 0, 0, 0,                     // padding
                2, 0, 0, 0,                     // vector length
                1, 0, 0, 0, 0, 0, 0, 0,         // index 0.Long
                1,                              // index 0.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
                2, 0, 0, 0, 0, 0, 0, 0,         // index 1.Long
                2,                              // index 1.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
            };

            Assert.IsTrue(expectedResult.AsSpan().SequenceEqual(target));
        }

        [TestMethod]
        public void NullStringInVector()
        {
            var root = new RootTable<IList<string>>
            {
                Vector = new string[] { "foobar", "banana", null, "two" },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<string>>>();

            byte[] target = new byte[10240];
            Assert.ThrowsException<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [TestMethod]
        public void NullStructInVector()
        {
            var root = new RootTable<IList<Struct>>
            {
                Vector = new[] { new Struct { Integer = 1, }, null, new Struct { Integer = 3 } },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<Struct>>>();

            byte[] target = new byte[10240];
            Assert.ThrowsException<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [TestMethod]
        public void AlignedStructVectorMaxSize()
        {
            var root = new RootTable<IList<Struct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new Struct { Integer = 1 }, new Struct { Integer = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding + 2 * itemLength
            Assert.AreEqual(3 + 4 + 3 + (2 * 4), maxSize - baselineMaxSize);
        }

        [TestMethod]
        public void UnalignedStruct_5Byte_VectorMaxSize()
        {
            var root = new RootTable<IList<FiveByteStruct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new FiveByteStruct { Int = 1 }, new FiveByteStruct { Int = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding to 4 byte alignment + (2 * (padding + itemLength))
            Assert.AreEqual(3 + 4 + 3 + (2 * (3 + 5)), maxSize - baselineMaxSize);
        }

        [TestMethod]
        public void UnalignedStruct_9Byte_VectorMaxSize()
        {
            var root = new RootTable<IList<NineByteStruct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new NineByteStruct { Long = 1 }, new NineByteStruct { Long = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding to 8 byte alignment + (2 * (padding + itemLength))
            Assert.AreEqual(3 + 4 + 7 + (2 * (7 + 9)), maxSize - baselineMaxSize);
        }

        [TestMethod]
        public void SortedVector_StringKey()
        {
            var root = new RootTableSorted<IList<TableWithKey<string>>>();

            root.Vector = new List<TableWithKey<string>>
            {
                new TableWithKey<string> { Key = "d", Value = "0" },
                new TableWithKey<string> { Key = "c", Value = "1" },
                new TableWithKey<string> { Key = "b", Value = "2" },
                new TableWithKey<string> { Key = "a", Value = "3" },
                new TableWithKey<string> { Key = "", Value = "4" },
            };

            byte[] data = new byte[1024];
            FlatBufferSerializer.Default.Serialize(root, data);

            var parsed = FlatBufferSerializer.Default.Parse<RootTableSorted<IList<TableWithKey<string>>>>(data);

            Assert.AreEqual(parsed.Vector[0].Key, "");
            Assert.AreEqual(parsed.Vector[1].Key, "a");
            Assert.AreEqual(parsed.Vector[2].Key, "b");
            Assert.AreEqual(parsed.Vector[3].Key, "c");
            Assert.AreEqual(parsed.Vector[4].Key, "d");
        }

        [TestMethod]
        public void SortedVector_SharedStringKey()
        {
            var root = new RootTableSorted<IList<TableWithKey<SharedString>>>();

            root.Vector = new List<TableWithKey<SharedString>>
            {
                new TableWithKey<SharedString> { Key = "d", Value = "0" },
                new TableWithKey<SharedString> { Key = "c", Value = "1" },
                new TableWithKey<SharedString> { Key = "b", Value = "2" },
                new TableWithKey<SharedString> { Key = "a", Value = "3" },
                new TableWithKey<SharedString> { Key = "", Value = "4" },
                new TableWithKey<SharedString> { Key = "a", Value = "5" },
            };

            byte[] data = new byte[1024];
            var serializer = FlatBufferSerializer.Default.Compile<RootTableSorted<IList<TableWithKey<SharedString>>>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringReaderFactory = () => SharedStringReader.CreateThreadSafe(),
                    SharedStringWriterFactory = () => new SharedStringWriter(),
                });

            serializer.Write(SpanWriter.Instance, data, root);

            var parsed = serializer.Parse(data);

            Assert.AreEqual(parsed.Vector[0].Key.String, "");
            Assert.AreEqual(parsed.Vector[1].Key.String, "a");
            Assert.AreEqual(parsed.Vector[2].Key.String, "a");
            Assert.AreEqual(parsed.Vector[3].Key.String, "b");
            Assert.AreEqual(parsed.Vector[4].Key.String, "c");
            Assert.AreEqual(parsed.Vector[5].Key.String, "d");

            Assert.IsNotNull(parsed.Vector.BinarySearchByFlatBufferKey((SharedString)"b"));
            Assert.IsTrue(object.ReferenceEquals(parsed.Vector[1].Key, parsed.Vector[2].Key));
        }

        [TestMethod]
        public void SortedVector_StringKey_Null()
        {
            var root = new RootTableSorted<IList<TableWithKey<string>>>();

            root.Vector = new List<TableWithKey<string>>
            {
                new TableWithKey<string> { Key = null, Value = "0" },
                new TableWithKey<string> { Key = "notnull", Value = "3" },
                new TableWithKey<string> { Key = "alsonotnull", Value = "3" },
            };

            byte[] data = new byte[1024];
            Assert.ThrowsException<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(root, data));
            Assert.ThrowsException<InvalidOperationException>(() => root.Vector.BinarySearchByFlatBufferKey("AAA"));
            Assert.ThrowsException<InvalidOperationException>(() => root.Vector.BinarySearchByFlatBufferKey(3));
            Assert.ThrowsException<ArgumentNullException>(() => root.Vector.BinarySearchByFlatBufferKey((string)null));
        }

        [TestMethod]
        public void SortedVector_Bool() => this.SortedVectorTest<bool>(rng => rng.Next() % 2 == 0, Comparer<bool>.Default);

        [TestMethod]
        public void SortedVector_Byte() => this.SortedVectorStructTest<byte>();

        [TestMethod]
        public void SortedVector_SByte() => this.SortedVectorStructTest<sbyte>();

        [TestMethod]
        public void SortedVector_UShort() => this.SortedVectorStructTest<ushort>();

        [TestMethod]
        public void SortedVector_Short() => this.SortedVectorStructTest<short>();

        [TestMethod]
        public void SortedVector_Uint() => this.SortedVectorStructTest<uint>();

        [TestMethod]
        public void SortedVector_Int() => this.SortedVectorStructTest<int>();

        [TestMethod]
        public void SortedVector_Ulong() => this.SortedVectorStructTest<ulong>();

        [TestMethod]
        public void SortedVector_Long() => this.SortedVectorStructTest<long>();

        [TestMethod]
        public void SortedVector_Double() => this.SortedVectorStructTest<double>();

        [TestMethod]
        public void SortedVector_Float() => this.SortedVectorStructTest<float>();

        [TestMethod]
        public void SortedVector_String_Base64()
        {
            this.SortedVectorTest<string>(
                rng =>
                {
                    int length = rng.Next(0, 30);
                    byte[] data = new byte[length];
                    rng.NextBytes(data);
                    return Convert.ToBase64String(data);
                },
                new Utf8StringComparer());
        }

        [TestMethod]
        public void SortedVector_String_RandomChars()
        {
            this.SortedVectorTest<string>(
                rng =>
                {
                    int length = rng.Next(0, 30);
                    string s = "";
                    for (int i = 0; i < length; ++i)
                    {
                        s += (char)rng.Next();
                    }

                    return s;
                },
                new Utf8StringComparer());
        }

        [TestMethod]
        public void SortedVector_String_Empty()
        {
            int i = 0;
            this.SortedVectorTest<string>(
                rng =>
                {
                    string s = "";
                    for (int j = 0; j < Math.Min(i, 100); ++j)
                    {
                        s += "a";
                    }

                    ++i;
                    return s;
                },
                new Utf8StringComparer());
        }

        [TestMethod]
        public void IndexedVector_Simple()
        {
            var table = new RootTableSorted<IIndexedVector<string, TableWithKey<string>>>
            {
                Vector = new IndexedVector<string, TableWithKey<string>>
                {
                    { new TableWithKey<string> { Key = "a", Value = "AAA" } },
                    { new TableWithKey<string> { Key = "b", Value = "BBB" } },
                    { new TableWithKey<string> { Key = "c", Value = "CCC" } },
                }
            };

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            byte[] data = new byte[1024 * 1024];
            serializer.Serialize(table, data);

            var parsed = serializer.Parse<RootTableSorted<IIndexedVector<string, TableWithKey<string>>>>(data);

            Assert.AreEqual("AAA", parsed.Vector["a"].Value);
            Assert.AreEqual("BBB", parsed.Vector["b"].Value);
            Assert.AreEqual("CCC", parsed.Vector["c"].Value);

            Assert.IsTrue(parsed.Vector.TryGetValue("a", out var value) && value.Value == "AAA");
            Assert.IsTrue(parsed.Vector.TryGetValue("b", out value) && value.Value == "BBB");
            Assert.IsTrue(parsed.Vector.TryGetValue("c", out value) && value.Value == "CCC");
        }

        [TestMethod]
        public void IndexedVector_RandomString()
        {
            var table = new RootTable<IIndexedVector<string, TableWithKey<string>>>
            {
                Vector = new IndexedVector<string, TableWithKey<string>>()
            };

            List<string> keys = new List<string>();
            for (int i = 0; i < 1000; ++i)
            {
                string key = Guid.NewGuid().ToString();
                keys.Add(key);

                table.Vector.AddOrReplace(new TableWithKey<string> { Key = key, Value = Guid.NewGuid().ToString() });
            }

            byte[] data = new byte[10 * 1024 * 1024];
            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
            serializer.Compile<RootTable<IIndexedVector<string, TableWithKey<string>>>>();
            int bytesWritten = serializer.Serialize(table, data);

            var parsed = serializer.Parse<RootTable<IIndexedVector<string, TableWithKey<string>>>>(data);

            foreach (var key in keys)
            {
                Assert.AreEqual(table.Vector[key].Value, parsed.Vector[key].Value);
            }
        }

        [TestMethod]
        public void IndexedVector_SharedStrings()
        {
            var table = new RootTable<IIndexedVector<SharedString, TableWithKey<SharedString>>>
            {
                Vector = new IndexedVector<SharedString, TableWithKey<SharedString>>()
            };

            List<SharedString> keys = new List<SharedString>();
            for (int i = 0; i < 50; ++i)
            {
                string key = Guid.NewGuid().ToString();
                keys.Add(key);
                table.Vector.AddOrReplace(new TableWithKey<SharedString> { Key = key, Value = Guid.NewGuid().ToString() });
            }

            byte[] data = new byte[10 * 1024 * 1024];
            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
            var sharedStringSerializer = serializer.Compile<RootTable<IIndexedVector<SharedString, TableWithKey<SharedString>>>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringReaderFactory = () => SharedStringReader.Create(5000),
                    SharedStringWriterFactory = () => new SharedStringWriter(5000),
                });

            int bytesWritten = sharedStringSerializer.Write(data, table);
            var parsed = sharedStringSerializer.Parse(data);

            foreach (var kvp in parsed.Vector)
            {
                SharedString key = kvp.Key;
                SharedString value = kvp.Value.Key;

                Assert.IsTrue(object.ReferenceEquals(key.String, value.String));
            }

            foreach (var key in keys)
            {
                SharedString expectedKey = key;
                Assert.IsTrue(parsed.Vector.TryGetValue(key, out var value));
                Assert.AreEqual(expectedKey, value.Key);
                Assert.IsFalse(object.ReferenceEquals(expectedKey.String, value.Key.String));
            }
        }

        [TestMethod]
        public void IndexedVector_RandomByte() => IndexedVectorScalarTest<byte>();

        [TestMethod]
        public void IndexedVector_RandomSByte() => IndexedVectorScalarTest<sbyte>();

        [TestMethod]
        public void IndexedVector_RandomUShort() => IndexedVectorScalarTest<ushort>();

        [TestMethod]
        public void IndexedVector_RandomShort() => IndexedVectorScalarTest<short>();

        [TestMethod]
        public void IndexedVector_RandomUInt() => IndexedVectorScalarTest<uint>();

        [TestMethod]
        public void IndexedVector_RandomInt() => IndexedVectorScalarTest<int>();

        [TestMethod]
        public void IndexedVector_RandomULong() => IndexedVectorScalarTest<ulong>();

        [TestMethod]
        public void IndexedVector_RandomLong() => IndexedVectorScalarTest<long>();

        private void IndexedVectorScalarTest<T>() where T : struct
        {
            foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                var table = new RootTable<IIndexedVector<T, TableWithKey<T>>>
                {
                    Vector = new IndexedVector<T, TableWithKey<T>>()
                };

                Random r = new Random();
                byte[] keyBuffer = new byte[8];
                List<T> keys = new List<T>();
                for (int i = 0; i < 1000; ++i)
                {
                    r.NextBytes(keyBuffer);
                    T key = MemoryMarshal.Cast<byte, T>(keyBuffer)[0];
                    keys.Add(key);
                    table.Vector.AddOrReplace(new TableWithKey<T> { Key = key, Value = Guid.NewGuid().ToString() });
                }

                byte[] data = new byte[1024 * 1024];
                var serializer = new FlatBufferSerializer(option);
                int bytesWritten = serializer.Serialize(table, data);

                var parsed = serializer.Parse<RootTable<IIndexedVector<T, TableWithKey<T>>>>(data);

                foreach (var key in keys)
                {
                    Assert.AreEqual(table.Vector[key].Value, parsed.Vector[key].Value);
                }

                // verify sorted and that we can read it when it's from a normal vector.
                var parsedList = serializer.Parse<RootTable<IList<TableWithKey<T>>>>(data);
                Assert.AreEqual(parsed.Vector.Count, parsedList.Vector.Count);
                var previous = parsedList.Vector[0];
                for (int i = 1; i < parsedList.Vector.Count; ++i)
                {
                    var item = parsedList.Vector[i];
                    Assert.IsTrue(Comparer<T>.Default.Compare(previous.Key, item.Key) <= 0);

                    Assert.IsTrue(parsed.Vector.TryGetValue(item.Key, out var fromDict));
                    Assert.AreEqual(item.Key, fromDict.Key);
                    Assert.AreEqual(item.Value, fromDict.Value);

                    previous = item;
                }
            }
        }

        private void SortedVectorStructTest<TKey>() where TKey : struct
        {
            this.SortedVectorTest(
                rng =>
                {
                    byte[] data = new byte[8];
                    rng.NextBytes(data);
                    TKey value = MemoryMarshal.Cast<byte, TKey>(data.AsSpan())[0];
                    return value;
                },
                Comparer<TKey>.Default);
        }

        private void SortedVectorTest<TKey>(
            Func<Random, TKey> createValue,
            IComparer<TKey> comparer)
        {
            Random rng = new Random();
            foreach (int length in Enumerable.Range(0, 20).Concat(Enumerable.Range(1, 25).Select(x => x * 25)))
            {
                TableWithKey<TKey>[] values = new TableWithKey<TKey>[length];
                for (int i = 0; i < values.Length; ++i)
                {
                    values[i] = new TableWithKey<TKey> { Key = createValue(rng), Value = i.ToString() };
                }

                this.SortedVectorTest(values, comparer);
            }
        }

        private void SortedVectorTest<TKey>(
            TableWithKey<TKey>[] values,
            IComparer<TKey> comparer)
        {
            RootTableSorted<TableWithKey<TKey>[]> root = new RootTableSorted<TableWithKey<TKey>[]>
            {
                Vector = values
            };

            byte[] data = new byte[1024 * 1024];
            FlatBufferSerializer.Default.Serialize(root, data);

            var parsed = FlatBufferSerializer.Default.Parse<RootTableSorted<TableWithKey<TKey>[]>>(data);
            Assert.AreEqual(parsed.Vector.Length, root.Vector.Length);

            if (parsed.Vector.Length > 0)
            {
                TableWithKey<TKey> previous = parsed.Vector[0];
                for (int i = 0; i < parsed.Vector.Length; ++i)
                {
                    Assert.IsTrue(comparer.Compare(previous.Key, parsed.Vector[i].Key) <= 0);
                    previous = parsed.Vector[i];
                }

                foreach (var originalItem in root.Vector)
                {
                    Assert.IsNotNull(parsed.Vector.BinarySearchByFlatBufferKey(originalItem.Key));
                }
            }
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }
        }

        [FlatBufferTable]
        public class RootTableSorted<TVector>
        {
            [FlatBufferItem(0, SortedVector = true)]
            public virtual TVector Vector { get; set; }
        }

        [FlatBufferTable]
        public class TableWithKey<TKey>
        {
            [FlatBufferItem(0)]
            public virtual string Value { get; set; }

            [FlatBufferItem(1, Key = true)]
            public virtual TKey Key { get; set; }
        }

        [FlatBufferTable]
        public class RootTable2<TVector>
        {
            [FlatBufferItem(0, DefaultValue = (byte)201)]
            public virtual byte AlignmentImp { get; set; }

            [FlatBufferItem(1)]
            public virtual TVector Vector { get; set; }
        }

        [FlatBufferStruct]
        public class Struct
        {
            [FlatBufferItem(0)]
            public virtual int Integer { get; set; }
        }

        [FlatBufferStruct]
        public class NineByteStruct
        {
            [FlatBufferItem(0)]
            public virtual long Long { get; set; }

            [FlatBufferItem(1)]
            public virtual byte Byte { get; set; }
        }
    }
}
