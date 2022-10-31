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

using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FlatSharpTests;

/// <summary>
/// Binary format testing for vector serialization.
/// </summary>

public class VectorSerializationTests
{
    public class SimpleTests
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
                string csharp = FlatBufferSerializer.Default.Compile(root).GetCSharp();

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
                string csharp = FlatBufferSerializer.Default.Compile(root).GetCSharp();

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
                string csharp = FlatBufferSerializer.Default.Compile(root).GetCSharp();

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
        public void SortedVector_BinarySearch_ErrorCases()
        {
            IList<TableWithKey<string>> testList = new List<TableWithKey<string>>
            {
                new TableWithKey<string> { Key = null, Value = "0" },
                new TableWithKey<string> { Key = "notnull", Value = "3" },
                new TableWithKey<string> { Key = "alsonotnull", Value = "3" },
            };

            var root = new RootTable<IList<TableWithKey<string>>>()
            {
                Vector = testList
            };

            byte[] data = new byte[1024];

            // Serialize succeeds here because the "root" is unsorted.
            FlatBufferSerializer.Default.Serialize(root, data);

            // Fail to serialize sorted vector due to null key.
            // Fail to search through greedy sorted vector due to null key.
            {
                var rootSorted = new RootTableSorted<IList<TableWithKey<string>>>()
                {
                    Vector = testList
                };

                var parsed_greedy = FlatBufferSerializer.Default.Parse<RootTableSorted<TableWithKey<string>[]>>(data);
                Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(rootSorted, new byte[1024]));
                Assert.Throws<InvalidOperationException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(parsed_greedy.Vector, "AAA"));
                Assert.Throws<ArgumentNullException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(parsed_greedy.Vector, (string)null));
            }

            // Fail to use a table with uninitialized keys.
            {
                var rootSorted = new RootTableSorted<IList<TableWithUninitializedKey<string>>>
                {
                    Vector = testList.Select(x => new TableWithUninitializedKey<string> { Key = x.Key, Value = x.Value }).ToList(),
                };

                var parsed_greedy = FlatBufferSerializer.Default.Parse<RootTableSorted<IList<TableWithUninitializedKey<string>>>>(data);
                Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(rootSorted, new byte[1024]));
                Assert.Throws<InvalidOperationException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(parsed_greedy.Vector, "AAA"));
                Assert.Throws<ArgumentNullException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(parsed_greedy.Vector, (string)null));
            }

            // Fail to binary search through lazy sorted vector with null key.
            {
                // Serialize succeeds here because the "root" is unsorted.
                FlatBufferSerializer.Default.Serialize(root, data);
                var lazyCopy = FlatBufferSerializer.Default.Compile<RootTable<IList<TableWithKey<string>>>>().WithSettings(s => s.UseLazyDeserialization()).Parse(data);

                Assert.Throws<InvalidOperationException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(lazyCopy.Vector, "AAA"));
                Assert.Throws<ArgumentNullException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(lazyCopy.Vector, (string)null));
            }

            {
                var parsed = FlatBufferSerializer.Default.Compile<RootTable<IReadOnlyList<TableWithNoKey<string>>>>().WithSettings(s => s.UseLazyDeserialization()).Parse(data);
                var ex = Assert.Throws<InvalidOperationException>(() => SortedVectorHelpers.BinarySearchByFlatBufferKey(parsed.Vector, "foo"));
            }
        }
    }

    public class SortedVector_Bool : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorTest<bool>(rng => rng.Next() % 2 == 0, Comparer<bool>.Default);
    }

    public class SortedVector_Byte : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<byte>();
    }

    public class SortedVector_SByte : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<sbyte>();
    }

    public class SortedVector_UShort : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<ushort>();
    }

    public class SortedVector_Short : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<short>();
    }

    public class SortedVector_Uint : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<uint>();
    }

    public class SortedVector_Int : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<int>();
    }

    public class SortedVector_Ulong : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<ulong>();
    }

    public class SortedVector_Long : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<long>();
    }

    public class SortedVector_Double : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<double>();
    }

    public class SortedVector_Float : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorStructTest<float>();
    }

    public class SortedVector_String_Base64 : VectorSerializationTests
    {
        [Fact]
        public void Test() => this.SortedVectorTest<string>(
            rng =>
            {
                int length = rng.Next(0, 2048);
                byte[] data = new byte[length];
                rng.NextBytes(data);
                return Convert.ToBase64String(data);
            },
            new Utf8StringComparer());
    }

    public class SortedVector_String_RandomChars : VectorSerializationTests
    {
        [Fact]
        public void Test()
        {
            int i = 0;
            StringBuilder s = new StringBuilder();

            this.SortedVectorTest<string>(
                rng =>
                {
                    s.Clear();
                    for (int j = 0; j < Math.Min(i, 100); ++j)
                    {
                        s.Append("a");
                    }

                    ++i;
                    return s.ToString();
                },
                new Utf8StringComparer());
        }
    }

    public class SortedVector_String_Empty : VectorSerializationTests
    {
        [Fact]
        public void Test()
        {
            StringBuilder sb = new();
            this.SortedVectorTest<string>(
                rng =>
                {
                    sb.Clear();
                    int length = rng.Next(0, 2048);
                    for (int i = 0; i < length; ++i)
                    {
                        // pick unicode characters in the basic multilingual plane.
                        sb.Append((char)rng.Next(0x0, 0xD7FF));
                    }

                    return sb.ToString();
                },
                new Utf8StringComparer());
        }
    }

    public class IndexedVectorTests
    {
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

            var serializer = FlatBufferSerializer.Default.Compile<RootTableSorted<IIndexedVector<string, TableWithKey<string>>>>()
                .WithSettings(s => s.UseLazyDeserialization());

            byte[] data = new byte[1024 * 1024];
            serializer.Write(data, table);

            var parsed = serializer.Parse(data);

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
            var serializer = FlatBufferSerializer.Default.Compile<RootTable<IIndexedVector<string, TableWithKey<string>>>>().WithSettings(s => s.UseLazyDeserialization());
            int bytesWritten = serializer.Write(data, table);

            var parsed = serializer.Parse(data);

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
                var serializer = FlatBufferSerializer.Default.Compile<RootTable<IIndexedVector<T, TableWithKey<T>>>>().WithSettings(s => s.UseDeserializationMode(option));
                int bytesWritten = serializer.Write(data, table);

                var parsed = serializer.Parse<RootTable<IIndexedVector<T, TableWithKey<T>>>>(data);

                foreach (var key in keys)
                {
                    Assert.Equal(table.Vector[key].Value, parsed.Vector[key].Value);
                }

                // verify sorted and that we can read it when it's from a normal vector.
                var listSerializer = FlatBufferSerializer.Default.Compile<RootTable<IList<TableWithKey<T>>>>().WithSettings(s => s.UseDeserializationMode(option));
                var parsedList = listSerializer.Parse(data);
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
    }

    protected void SortedVectorStructTest<TKey>() where TKey : struct
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

    protected void SortedVectorTest<TKey>(
        Func<Random, TKey> createValue,
        IComparer<TKey> comparer)
    {
        Random rng = new Random();
        foreach (int length in Enumerable.Range(0, 20).Concat(Enumerable.Range(1, 5).Select(x => x * 100)))
        {
            TableWithKey<TKey>[] values = new TableWithKey<TKey>[length];
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = new TableWithKey<TKey> { Key = createValue(rng), Value = i.ToString() };
            }

            this.SortedVectorTest(values, comparer);
        }
    }

    protected void SortedVectorTest<TKey>(
        TableWithKey<TKey>[] values,
        IComparer<TKey> comparer)
    {
        RootTableSorted<TableWithKey<TKey>[]> root = new RootTableSorted<TableWithKey<TKey>[]>
        {
            Vector = values
        };

        byte[] data = new byte[FlatBufferSerializer.Default.GetMaxSize(root)];
        FlatBufferSerializer.Default.Serialize(root, data);

        void RunTest<TVector>(
            FlatBufferDeserializationOption option,
            Func<TVector, int> getLength,
            Func<TVector, int, TableWithKey<TKey>> indexer,
            Func<TVector, TKey, TableWithKey<TKey>?> find)
        {
            var parsed = FlatBufferSerializer.Default.Compile<RootTableSorted<TVector>>().WithSettings(s => s.UseDeserializationMode(option)).Parse(data);
            var vector = parsed.Vector;
            int length = getLength(vector);

            Assert.Equal(root.Vector.Length, length);

            if (length > 0)
            {
                TableWithKey<TKey> previous = indexer(vector, 0);
                for (int i = 0; i < length; ++i)
                {
                    var item = indexer(vector, i);
                    Assert.True(comparer.Compare(previous.Key, item.Key) <= 0);
                    previous = item;
                }

                foreach (var originalItem in root.Vector)
                {
                    var item = find(vector, originalItem.Key);
                    Assert.NotNull(item);

                    if (originalItem.Key.ToString() != item.Key.ToString())
                    {
                    }

                    Assert.Equal(originalItem.Key.ToString(), item.Key.ToString());
                }
            }
        }

        foreach (FlatBufferDeserializationOption mode in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
        {
            RunTest<TableWithKey<TKey>[]>(mode, x => x.Length, (x, i) => x[i], (x, k) => SortedVectorHelpers.BinarySearchByFlatBufferKey(x, k));
            RunTest<IList<TableWithKey<TKey>>>(mode, x => x.Count, (x, i) => x[i], (x, k) => SortedVectorHelpers.BinarySearchByFlatBufferKey(x, k));
            RunTest<IReadOnlyList<TableWithKey<TKey>>>(mode, x => x.Count, (x, i) => x[i], (x, k) => SortedVectorHelpers.BinarySearchByFlatBufferKey(x, k));
        }
    }

    public class VectorOfUnionTests
    {
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
    public class TableWithKey<TKey> : ISortableTable<TKey>
    {
        static TableWithKey()
        {
            SortedVectorHelpers.RegisterKeyLookup<TableWithKey<TKey>, TKey>(x => x.Key, 1);
        }

        [FlatBufferItem(0)]
        public virtual string? Value { get; set; }

        [FlatBufferItem(1, Key = true)]
        public virtual TKey? Key { get; set; }
    }

    [FlatBufferTable]
    public class TableWithUninitializedKey<TKey> : ISortableTable<TKey>
    {
        [FlatBufferItem(0)]
        public virtual string? Value { get; set; }

        [FlatBufferItem(1, Key = true)]
        public virtual TKey? Key { get; set; }
    }

    // Should be impossible since FlatSharp compiler only adds this to types that are sortable. However
    // a user could technically add this themselves...
    [FlatBufferTable]
    public class TableWithNoKey<TKey> : ISortableTable<TKey>
    {
        [FlatBufferItem(0)]
        public virtual string? Value { get; set; }

        [FlatBufferItem(1)]
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