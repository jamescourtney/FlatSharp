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
    using Xunit;

    /// <summary>
    /// Binary format testing for vector serialization.
    /// </summary>
    
    public class VectorSerializationTests
    {
        [Fact]
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

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
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

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
        public void Simple_Scalar_Vectors()
        {
            static void Test<T>(Func<byte[], T> factory)
            {
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
                    1, 2, 3,
                };

                var root = new RootTable<T>
                {
                    Vector = factory(new byte[] { 1, 2, 3 })
                };

                Span<byte> target = new byte[1024];
                int offset = FlatBufferSerializer.Default.Serialize(root, target);
                string csharp = FlatBufferSerializer.Default.Compile(root).CSharp;

                target = target.Slice(0, offset);

                Assert.True(expectedResult.AsSpan().SequenceEqual(target));
            }

            Test<IList<byte>>(a => a.ToList());
            Test<IReadOnlyList<byte>>(a => a.ToList());
            Test<byte[]>(a => a);
            Test<Memory<byte>>(a => a.AsMemory());
            Test<ReadOnlyMemory<byte>>(a => a.AsMemory());
            Test<Memory<byte>?>(a => a.AsMemory());
            Test<ReadOnlyMemory<byte>?>(a => a.AsMemory());
        }

        [Fact]
        public void Empty_Vectors()
        {
            static void Test<T>(T instance)
            {
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

                var root = new RootTable<T>
                {
                    Vector = instance
                };

                Span<byte> target = new byte[1024];
                int offset = FlatBufferSerializer.Default.Serialize(root, target);
                string csharp = FlatBufferSerializer.Default.Compile(root).CSharp;

                target = target.Slice(0, offset);

                Assert.True(expectedResult.AsSpan().SequenceEqual(target));
            }

            Test<IList<int>>(new List<int>());
            Test<IReadOnlyList<int>>(new List<int>());
            Test<int[]>(new int[0]);
            Test<Memory<byte>>(new Memory<byte>(new byte[0]));
            Test<ReadOnlyMemory<byte>>(new ReadOnlyMemory<byte>(new byte[0]));
            Test<Memory<byte>?>(new Memory<byte>(new byte[0]));
            Test<ReadOnlyMemory<byte>?>(new ReadOnlyMemory<byte>(new byte[0]));
            Test<IIndexedVector<string, TableWithKey<string>>>(new IndexedVector<string, TableWithKey<string>>());
        }

        [Fact]
        public void Null_Vectors()
        {
            static void Test<T>()
            {
                byte[] expectedResult =
                {
                    4, 0, 0, 0,      // offset to table start
                    252,255,255,255, // soffset to vtable (-4)
                    4, 0,            // vtable length
                    4, 0,            // table length
                };

                var root = new RootTable<T>
                {
                    Vector = default(T)
                };

                Span<byte> target = new byte[1024];
                int offset = FlatBufferSerializer.Default.Serialize(root, target);
                string csharp = FlatBufferSerializer.Default.Compile(root).CSharp;

                target = target.Slice(0, offset);

                Assert.True(expectedResult.AsSpan().SequenceEqual(target));
            }

            Test<IList<int>>();
            Test<IReadOnlyList<int>>();
            Test<int[]>();
            Test<Memory<byte>?>();
            Test<ReadOnlyMemory<byte>?>();
            Test<IIndexedVector<string, TableWithKey<string>>>();

            Test<FlatBufferUnion<string>[]>();
            Test<IList<FlatBufferUnion<string>>>();
            Test<IReadOnlyList<FlatBufferUnion<string>>>();

            Test<string>();
        }

        [Fact]
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

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
        public void UnalignedStruct_Value5Byte()
        {
            var root = new RootTable<ValueFiveByteStruct[]>
            {
                Vector = new[]
                {
                    new ValueFiveByteStruct { Byte = 1, Int = 1 },
                    new ValueFiveByteStruct { Byte = 2, Int = 2 },
                    new ValueFiveByteStruct { Byte = 3, Int = 3 },
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

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
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
                246, 255, 255, 255,             // soffset to vtable (-10)
                20, 0, 0, 0,                    // uoffset_t to vector
                0,                              // alignment imp
                0,                              // padding
                8, 0,                           // vtable length
                9, 0,                           // table length
                8, 0,                           // offset to index 0 field
                4, 0,                           // offset of index 1 field

                0, 0, 0, 0, 0, 0,               // padding to 8 byte alignment for struct.
                2, 0, 0, 0,                     // vector length
                1, 0, 0, 0, 0, 0, 0, 0,         // index 0.Long
                1,                              // index 0.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
                2, 0, 0, 0, 0, 0, 0, 0,         // index 1.Long
                2,                              // index 1.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
            };

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
        public void UnalignedStruct_Value9Byte()
        {
            var root = new RootTable2<ValueNineByteStruct[]>
            {
                Vector = new[]
                {
                    new ValueNineByteStruct { Byte = 1, Long = 1 },
                    new ValueNineByteStruct { Byte = 2, Long = 2 },
                },
            };

            Span<byte> target = new byte[10240];
            int offset = FlatBufferSerializer.Default.Serialize(root, target);
            target = target.Slice(0, offset);

            byte[] expectedResult =
            {
                4, 0, 0, 0,                     // offset to table start
                246, 255, 255, 255,             // soffset to vtable (-10)
                20, 0, 0, 0,                    // uoffset_t to vector
                0,                              // alignment imp
                0,                              // padding
                8, 0,                           // vtable length
                9, 0,                           // table length
                8, 0,                           // offset to index 0 field
                4, 0,                           // offset of index 1 field

                0, 0, 0, 0, 0, 0,               // padding to 8 byte alignment for struct.
                2, 0, 0, 0,                     // vector length
                1, 0, 0, 0, 0, 0, 0, 0,         // index 0.Long
                1,                              // index 0.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
                2, 0, 0, 0, 0, 0, 0, 0,         // index 1.Long
                2,                              // index 1.Byte
                0, 0, 0, 0, 0, 0, 0,            // padding
            };

            Assert.True(expectedResult.AsSpan().SequenceEqual(target));
        }

        [Fact]
        public void NullStringInVector()
        {
            var root = new RootTable<IList<string>>
            {
                Vector = new string[] { "foobar", "banana", null, "two" },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<string>>>();

            byte[] target = new byte[10240];
            Assert.Throws<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [Fact]
        public void NullStructInVector()
        {
            var root = new RootTable<IList<Struct>>
            {
                Vector = new[] { new Struct { Integer = 1, }, null, new Struct { Integer = 3 } },
            };

            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IList<Struct>>>();

            byte[] target = new byte[10240];
            Assert.Throws<InvalidDataException>(() => FlatBufferSerializer.Default.Serialize(root, target));
        }

        [Fact]
        public void AlignedStructVectorMaxSize()
        {
            var root = new RootTable<IList<Struct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new Struct { Integer = 1 }, new Struct { Integer = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding + 2 * itemLength
            Assert.Equal(3 + 4 + 3 + (2 * 4), maxSize - baselineMaxSize);
        }

        [Fact]
        public void UnalignedStruct_5Byte_VectorMaxSize()
        {
            var root = new RootTable<IList<FiveByteStruct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new FiveByteStruct { Int = 1 }, new FiveByteStruct { Int = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding to 4 byte alignment + (2 * (padding + itemLength))
            Assert.Equal(3 + 4 + 3 + (2 * (3 + 5)), maxSize - baselineMaxSize);
        }

        [Fact]
        public void UnalignedStruct_9Byte_VectorMaxSize()
        {
            var root = new RootTable<IList<NineByteStruct>>();

            // Empty table max size (vector not included here).
            var baselineMaxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            root.Vector = new[] { new NineByteStruct { Long = 1 }, new NineByteStruct { Long = 2 } };

            var maxSize = FlatBufferSerializer.Default.GetMaxSize(root);

            // padding + length + padding to 8 byte alignment + (2 * (padding + itemLength))
            Assert.Equal(3 + 4 + 7 + (2 * (7 + 9)), maxSize - baselineMaxSize);
        }

        [Fact]
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

            Assert.Equal("", parsed.Vector[0].Key);
            Assert.Equal("a", parsed.Vector[1].Key);
            Assert.Equal("b", parsed.Vector[2].Key);
            Assert.Equal("c", parsed.Vector[3].Key);
            Assert.Equal("d", parsed.Vector[4].Key);
        }

        [Fact]
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
            Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(root, data));
            Assert.Throws<InvalidOperationException>(() => root.Vector.BinarySearchByFlatBufferKey("AAA"));
            Assert.Throws<InvalidOperationException>(() => root.Vector.BinarySearchByFlatBufferKey(3));
            Assert.Throws<ArgumentNullException>(() => root.Vector.BinarySearchByFlatBufferKey((string)null));
        }

        [Fact]
        public void SortedVector_Bool() => this.SortedVectorTest<bool>(rng => rng.Next() % 2 == 0, Comparer<bool>.Default);

        [Fact]
        public void SortedVector_Byte() => this.SortedVectorStructTest<byte>();

        [Fact]
        public void SortedVector_SByte() => this.SortedVectorStructTest<sbyte>();

        [Fact]
        public void SortedVector_UShort() => this.SortedVectorStructTest<ushort>();

        [Fact]
        public void SortedVector_Short() => this.SortedVectorStructTest<short>();

        [Fact]
        public void SortedVector_Uint() => this.SortedVectorStructTest<uint>();

        [Fact]
        public void SortedVector_Int() => this.SortedVectorStructTest<int>();

        [Fact]
        public void SortedVector_Ulong() => this.SortedVectorStructTest<ulong>();

        [Fact]
        public void SortedVector_Long() => this.SortedVectorStructTest<long>();

        [Fact]
        public void SortedVector_Double() => this.SortedVectorStructTest<double>();

        [Fact]
        public void SortedVector_Float() => this.SortedVectorStructTest<float>();

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            Assert.Equal("AAA", parsed.Vector["a"].Value);
            Assert.Equal("BBB", parsed.Vector["b"].Value);
            Assert.Equal("CCC", parsed.Vector["c"].Value);

            Assert.True(parsed.Vector.TryGetValue("a", out var value) && value.Value == "AAA");
            Assert.True(parsed.Vector.TryGetValue("b", out value) && value.Value == "BBB");
            Assert.True(parsed.Vector.TryGetValue("c", out value) && value.Value == "CCC");
        }

        [Fact]
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
                Assert.Equal(table.Vector[key].Value, parsed.Vector[key].Value);
            }
        }

        [Fact]
        public void IndexedVector_RandomByte() => IndexedVectorScalarTest<byte>();

        [Fact]
        public void IndexedVector_RandomSByte() => IndexedVectorScalarTest<sbyte>();

        [Fact]
        public void IndexedVector_RandomUShort() => IndexedVectorScalarTest<ushort>();

        [Fact]
        public void IndexedVector_RandomShort() => IndexedVectorScalarTest<short>();

        [Fact]
        public void IndexedVector_RandomUInt() => IndexedVectorScalarTest<uint>();

        [Fact]
        public void IndexedVector_RandomInt() => IndexedVectorScalarTest<int>();

        [Fact]
        public void IndexedVector_RandomULong() => IndexedVectorScalarTest<ulong>();

        [Fact]
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
                    Assert.Equal(table.Vector[key].Value, parsed.Vector[key].Value);
                }

                // verify sorted and that we can read it when it's from a normal vector.
                var parsedList = serializer.Parse<RootTable<IList<TableWithKey<T>>>>(data);
                Assert.Equal(parsed.Vector.Count, parsedList.Vector.Count);
                var previous = parsedList.Vector[0];
                for (int i = 1; i < parsedList.Vector.Count; ++i)
                {
                    var item = parsedList.Vector[i];
                    Assert.True(Comparer<T>.Default.Compare(previous.Key, item.Key) <= 0);

                    Assert.True(parsed.Vector.TryGetValue(item.Key, out var fromDict));
                    Assert.Equal(item.Key, fromDict.Key);
                    Assert.Equal(item.Value, fromDict.Value);

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
            Assert.Equal(parsed.Vector.Length, root.Vector.Length);

            if (parsed.Vector.Length > 0)
            {
                TableWithKey<TKey> previous = parsed.Vector[0];
                for (int i = 0; i < parsed.Vector.Length; ++i)
                {
                    Assert.True(comparer.Compare(previous.Key, parsed.Vector[i].Key) <= 0);
                    previous = parsed.Vector[i];
                }

                foreach (var originalItem in root.Vector)
                {
                    Assert.NotNull(parsed.Vector.BinarySearchByFlatBufferKey(originalItem.Key));
                }
            }
        }

        [Fact]
        public void VectorOfUnion_List() => this.VectorOfUnionTest<RootTable<IList<FlatBufferUnion<string, Struct, TableWithKey<int>>>>>(
            (l, v) => v.Vector = l.ToList());

        [Fact]
        public void VectorOfUnion_ReadOnlyList() => this.VectorOfUnionTest<RootTable<IReadOnlyList<FlatBufferUnion<string, Struct, TableWithKey<int>>>>>(
            (l, v) => v.Vector = l.ToList());

        [Fact]
        public void VectorOfUnion_Array() => this.VectorOfUnionTest<RootTable<FlatBufferUnion<string, Struct, TableWithKey<int>>[]>>(
            (l, v) => v.Vector = l);

        private void VectorOfUnionTest<V>(Action<FlatBufferUnion<string, Struct, TableWithKey<int>>[], V> setValue)
            where V : class, new()
        {
            var items = new[]
            {
                new FlatBufferUnion<string, Struct, TableWithKey<int>>("foo"),
                new FlatBufferUnion<string, Struct, TableWithKey<int>>(new Struct { Integer = 3 }),
                new FlatBufferUnion<string, Struct, TableWithKey<int>>(new TableWithKey<int> { Key = 1 }),
            };

            V value = new V();
            setValue(items, value);

            byte[] expectedData =
            {
                4, 0, 0, 0,
                244, 255, 255, 255,
                16, 0, 0, 0, // uoffset to discriminator vector
                20, 0, 0, 0, // uoffset to offset vector
                8, 0,        // vtable
                12, 0,
                4, 0,
                8, 0,
                3, 0, 0, 0, // discriminator vector length
                1, 2, 3, 0, // values + 1 byte padding
                3, 0, 0, 0, // offset vector length
                12, 0, 0, 0, // value 0
                16, 0, 0, 0, // value 1
                16, 0, 0, 0, // value 2
                3, 0, 0, 0,  // string length
                102, 111, 111, 0, // foo + null terminator
                3, 0, 0, 0,       // struct value ('3')
                248, 255, 255, 255, // table vtable offset
                1, 0, 0, 0,         // value of 'key'
                8, 0,               // table vtable start
                8, 0,
                0, 0,
                4, 0,
            };

            byte[] data = new byte[1024];
            int written = FlatBufferSerializer.Default.Serialize(value, data);
            data = data.AsSpan().Slice(0, written).ToArray();

            Assert.True(data.SequenceEqual(expectedData));
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }
        }

        [FlatBufferTable]
        public class RootTableSorted<TVector>
        {
            [FlatBufferItem(0, SortedVector = true)]
            public virtual TVector? Vector { get; set; }
        }

        [FlatBufferTable]
        public class TableWithKey<TKey>
        {
            [FlatBufferItem(0)]
            public virtual string? Value { get; set; }

            [FlatBufferItem(1, Key = true)]
            public virtual TKey? Key { get; set; }
        }

        [FlatBufferTable]
        public class RootTable2<TVector>
        {
            [FlatBufferItem(0, DefaultValue = (byte)201)]
            public virtual byte AlignmentImp { get; set; }

            [FlatBufferItem(1)]
            public virtual TVector? Vector { get; set; }
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
      
        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 9)]
        public struct ValueNineByteStruct
        {
            [FieldOffset(0)] public long Long;

            [FieldOffset(8)] public byte Byte;
        }
    }
}
