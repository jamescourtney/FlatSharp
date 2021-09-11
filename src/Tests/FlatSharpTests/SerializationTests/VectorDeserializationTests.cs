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
    using System.Collections.ObjectModel;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Binary format testing for vector deserialization.
    /// </summary>
    
    public class VectorDeserializationTests
    {
        [Fact]
        public void NullListVector()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);
            Assert.Null(item.Vector);
        }

        [Fact]
        public void EmptyListVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);
            Assert.Equal(0, item.Vector.Count);
        }

        [Fact]
        public void SimpleListVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length,

                // vector data
                1, 0,
                2, 0,
                3, 0
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<IList<short>>>(data);

            Assert.Equal(3, item.Vector.Count);
            Assert.Equal(1, item.Vector[0]);
            Assert.Equal(2, item.Vector[1]);
            Assert.Equal(3, item.Vector[2]);
        }

        [Fact]
        public void EmptyMemoryVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };
            
            var item = FlatBufferSerializer.Default.Parse<RootTable<Memory<byte>>>(data);
            Assert.True(item.Vector.IsEmpty);
        }

        [Fact]
        public void SimpleMemoryVector()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length
                1, 2, 3,     // True false true
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<Memory<byte>>>(data);
            Assert.Equal(3, item.Vector.Length);
            Assert.Equal((byte)1, item.Vector.Span[0]);
            Assert.Equal((byte)2, item.Vector.Span[1]);
            Assert.Equal((byte)3, item.Vector.Span[2]);
        }

        [Fact]
        public void NullString()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.Null(item.Vector);
        }

        [Fact]
        public void EmptyString()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
                0,           // null terminator (special case for string vectors).
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.Equal(string.Empty, item.Vector);
        }

        [Fact]
        public void SimpleString()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length
                1, 2, 3, 0,  // data + null terminator (special case for string vectors).
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<string>>(data);
            Assert.Equal(3, item.Vector.Length);
            Assert.Equal(new string(new char[] { (char)1, (char)2, (char)3 }), item.Vector);
        }

        [Fact]
        public void NullArray()
        {
            byte[] data =
            {
                8, 0, 0, 0, // offset to table start
                4, 0,       // vtable length
                4, 0,       // table length
                4, 0, 0, 0  // soffset to vtable
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<int[]>>(data);
            Assert.Null(item.Vector);
        }

        [Fact]
        public void EmptyArray()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                0, 0, 0, 0,  // vector length
            };

            // we parse non-scalar array and scalar arrays differently, so test for both.
            var item = FlatBufferSerializer.Default.Parse<RootTable<int[]>>(data);
            Assert.Empty(item.Vector);

            var stringItem = FlatBufferSerializer.Default.Parse<RootTable<string[]>>(data);
            Assert.Empty(stringItem.Vector);
        }

        [Fact]
        public void SimpleArray()
        {
            byte[] data =
            {
                12, 0, 0, 0, // offset to table start
                6, 0,        // vtable length
                8, 0,        // table length
                4, 0,        // offset of index 0 field
                0, 0,        // padding to 4-byte alignment
                8, 0, 0, 0,  // soffset to vtable
                4, 0, 0, 0,  // uoffset_t to vector
                3, 0, 0, 0,  // vector length

                // vector data
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 0, 0, 0, 0,
            };

            var item = FlatBufferSerializer.Default.Parse<RootTable<long[]>>(data);
            Assert.Equal(3, item.Vector.Length);
            Assert.Equal(1L, item.Vector[0]);
            Assert.Equal(2L, item.Vector[1]);
            Assert.Equal(3L, item.Vector[2]);
        }

        [Fact]
        public void StringVector_GreedyDeserialize_Mutable()
        {
            RootTable<IList<string>> root = new RootTable<IList<string>>
            {
                Vector = new List<string> { "one", "two", "three" }
            };

            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.GreedyMutable);
            FlatBufferSerializer serializer = new FlatBufferSerializer(options);

            byte[] buffer = new byte[100];
            serializer.Serialize(root, buffer);

            var parsed = serializer.Parse<RootTable<IList<string>>>(buffer);

            Assert.Equal(typeof(List<string>), parsed.Vector.GetType());
            Assert.False(parsed.Vector.IsReadOnly);

            // Shouldn't throw.
            parsed.Vector.Add("four");
        }

        [Fact]
        public void StringVector_GreedyDeserialize_NotMutable()
        {
            RootTable<IList<string>> root = new RootTable<IList<string>>
            {
                Vector = new List<string> { "one", "two", "three" }
            };

            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy);
            FlatBufferSerializer serializer = new FlatBufferSerializer(options);

            byte[] buffer = new byte[100];
            serializer.Serialize(root, buffer);

            var parsed = serializer.Parse<RootTable<IList<string>>>(buffer);

            Assert.Equal(typeof(ReadOnlyCollection<string>), parsed.Vector.GetType());
            Assert.True(parsed.Vector.IsReadOnly);

            Assert.Throws<NotSupportedException>(() => parsed.Vector.Add("four"));
        }

        [Fact]
        public void MemoryVector_GreedyDeserialize()
        {
            RootTable<Memory<byte>> root = new RootTable<Memory<byte>>()
            {
                Vector = new Memory<byte>(new byte[100])
            };

            root.Vector.Span.Fill(1);

            byte[] buffer = new byte[1024];
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy);
            var serializer = new FlatBufferSerializer(options);

            serializer.Serialize(root, buffer.AsSpan());

            var parsed1 = serializer.Parse<RootTable<Memory<byte>>>(buffer);
            var parsed2 = serializer.Parse<RootTable<Memory<byte>>>(buffer);

            Assert.Equal((byte)1, parsed1.Vector.Span[0]);
            Assert.Equal((byte)1, parsed2.Vector.Span[0]);

            // Asser that this change affects only the 'parsed1' object.
            parsed1.Vector.Span[0] = 2;

            Assert.Equal((byte)2, parsed1.Vector.Span[0]);
            Assert.Equal((byte)1, parsed2.Vector.Span[0]);
        }

        /// <summary>
        /// Asserts that when greedy parsing is off, a change to the memory of the parsed object represents a change in the buffer.
        /// </summary>
        [Fact]
        public void MemoryVector_LazyDeserialize()
        {
            RootTable<Memory<byte>> root = new RootTable<Memory<byte>>()
            {
                Vector = new Memory<byte>(new byte[100])
            };

            root.Vector.Span.Fill(1);

            byte[] buffer = new byte[1024];
            var options = new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy);
            var serializer = new FlatBufferSerializer(options);

            serializer.Serialize(root, buffer.AsSpan());

            var parsed1 = serializer.Parse<RootTable<Memory<byte>>>(buffer);
            var parsed2 = serializer.Parse<RootTable<Memory<byte>>>(buffer);

            Assert.Equal((byte)1, parsed1.Vector.Span[0]);
            Assert.Equal((byte)1, parsed2.Vector.Span[0]);

            // Asser that this change affects both objects.
            parsed1.Vector.Span[0] = 2;

            Assert.Equal((byte)2, parsed1.Vector.Span[0]);
            Assert.Equal((byte)2, parsed2.Vector.Span[0]);
        }

        [Fact]
        public void UnalignedStruct_Value5Byte()
        {
            byte[] data =
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

            var item = FlatBufferSerializer.Default.Parse<RootTable<ValueFiveByteStruct[]>>(data);

            Assert.Equal(3, item.Vector.Length);
            for (int i = 0; i < 3; ++i)
            {
                Assert.Equal(i + 1, item.Vector[i].Int);
                Assert.Equal((byte)(i + 1), item.Vector[i].Byte);
            }
        }

        [Fact]
        public void VectorOfUnion_List() => this.VectorOfUnionTest<RootTable<IList<FlatBufferUnion<string, Struct, TableWithKey<int>>>>>(
            FlatBufferDeserializationOption.Progressive,
            v => v.Vector.ToArray());

        [Fact]
        public void VectorOfUnion_ReadOnlyList() => this.VectorOfUnionTest<RootTable<IReadOnlyList<FlatBufferUnion<string, Struct, TableWithKey<int>>>>>(
            FlatBufferDeserializationOption.Lazy,
            v => v.Vector.ToArray());

        [Fact]
        public void VectorOfUnion_Array() => this.VectorOfUnionTest<RootTable<FlatBufferUnion<string, Struct, TableWithKey<int>>[]>>(
            FlatBufferDeserializationOption.Greedy,
            v => v.Vector);

        private void VectorOfUnionTest<V>(
            FlatBufferDeserializationOption option,
            Func<V, FlatBufferUnion<string, Struct, TableWithKey<int>>[]> getItems)
            where V : class, new()
        {
            byte[] data =
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

            var serializer = new FlatBufferSerializer(option);
            V parsed = serializer.Parse<V>(data);
            var items = getItems(parsed);

            Assert.True(items[0].TryGet(out string str));
            Assert.Equal("foo", str);

            Assert.True(items[1].TryGet(out Struct @struct));
            Assert.Equal(3, @struct.Integer);

            Assert.True(items[2].TryGet(out TableWithKey<int> table));
            Assert.Equal(1, table.Key);
            Assert.Null(table.Value);
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }
        }

        [FlatBufferStruct]
        public class Struct
        {
            [FlatBufferItem(0)]
            public virtual int Integer { get; set; }
        }

        [FlatBufferTable]
        public class TableWithKey<TKey>
        {
            [FlatBufferItem(0)]
            public virtual string? Value { get; set; }

            [FlatBufferItem(1, Key = true)]
            public virtual TKey? Key { get; set; }
        }
    }
}
