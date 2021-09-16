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
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Tests that generated deserializers behave the way we expect.
    /// </summary>
    
    public class DeserializationOptionsTests
    {
        private static readonly string[] Strings = new[] { string.Empty, "a", "ab", "abc", "abcd", "abcde" };
        private static readonly byte[] Bytes = new byte[] { 1, 2, 3, 4, 5, 6 };
        private static readonly byte[] InputBuffer = new byte[10240];

        [Fact]
        public void DeserializationOption_Lazy_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.Lazy, Strings);

            Assert.Equal(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.False(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.False(object.ReferenceEquals(vector[5], vector[5]));
            Assert.False(object.ReferenceEquals(table.First, table.First));
            Assert.False(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Strings.Length, table.Vector.Count);
            Assert.Throws<NotMutableException>(() => table.Vector[0] = "foobar");
            Assert.Throws<NotMutableException>(() => table.Vector.Clear());
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Lazy_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.Lazy, Strings);

            Assert.Equal(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.False(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.False(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.False(object.ReferenceEquals(table.First, table.First));
            Assert.False(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Lazy_Array()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.Lazy, Strings));
            Assert.Equal("Array vectors may only be used with Greedy serializers.", ex.Message);
        }

        [Fact]
        public void DeserializationOption_Lazy_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.Lazy, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.True(table.Vector.Span.Overlaps(InputBuffer));
            Assert.False(object.ReferenceEquals(table.First, table.First));
            Assert.False(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Lazy_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.Lazy, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.True(table.Vector.Span.Overlaps(InputBuffer));
            Assert.False(object.ReferenceEquals(table.First, table.First));
            Assert.False(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Progressive_Array()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.Progressive, Strings));
            Assert.Equal("Array vectors may only be used with Greedy serializers.", ex.Message);
        }

        [Fact]
        public void DeserializationOption_Progressive_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.Progressive, Strings);
            string originalHash = this.GetInputBufferHash();

            Assert.Equal(typeof(FlatBufferProgressiveVector<string, ArrayInputBuffer>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.True(object.ReferenceEquals(vector[5], vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);

            Assert.Throws<NotMutableException>(() => table.Vector[0] = "foobar");
            Assert.Throws<NotMutableException>(() => table.Vector.Clear());
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);

            Assert.Equal(originalHash, this.GetInputBufferHash());
        }

        [Fact]
        public void DeserializationOption_Progressive_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.Progressive, Strings);
            string originalHash = this.GetInputBufferHash();

            Assert.Equal(typeof(FlatBufferProgressiveVector<string, ArrayInputBuffer>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.True(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);

            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);

            Assert.Equal(originalHash, this.GetInputBufferHash());
        }

        [Fact]
        public void DeserializationOption_Progressive_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.Progressive, Bytes);
            string originalHash = this.GetInputBufferHash();

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.True(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);

            Assert.Throws<NotMutableException>(() => table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana"));
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = new());

            Assert.Equal(originalHash, this.GetInputBufferHash());

            table.Vector.Span[0] = byte.MaxValue;
            Assert.NotEqual(originalHash, this.GetInputBufferHash());
        }

        [Fact]
        public void DeserializationOption_Progressive_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.Progressive, Bytes);
            string originalHash = this.GetInputBufferHash();

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.True(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);

            Assert.Throws<NotMutableException>(() => table.First = new FirstStruct { First = 10 });
            Assert.Throws<NotMutableException>(() => table.Second.Second = 3);
            Assert.Throws<NotMutableException>(() => table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana"));

            Assert.Equal(originalHash, this.GetInputBufferHash());
        }

        [Fact]
        public void DeserializationOption_Greedy_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.Equal(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.True(object.ReferenceEquals(vector[5], vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Strings.Length, table.Vector.Count);
            Assert.Throws<NotSupportedException>(() => table.Vector[0] = "foobar");
            Assert.Throws<NotSupportedException>(() => table.Vector.Clear());
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Greedy_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.Equal(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.True(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Greedy_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.True(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Length);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Greedy_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.Greedy, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Greedy makes a copy of the input memory.
            Assert.False(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_Greedy_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.Greedy, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Greedy makes a copy of the input memory.
            Assert.False(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);
            Assert.Throws<NotMutableException>(() => table.First.First = 3);
            Assert.Throws<NotMutableException>(() => table.First = null);
        }

        [Fact]
        public void DeserializationOption_GreedyMutable_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.Equal(typeof(List<string>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.True(object.ReferenceEquals(vector[5], vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);

            table.Vector[0] = "foobar";
            table.First = new FirstStruct { First = 10 };
            table.Vector[1] = "turkey";
            table.Vector.Clear();
            table.Second.Second = 3;
        }

        [Fact]
        public void DeserializationOption_GreedyMutable_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.Equal(typeof(List<string>), table.Vector.GetType());
            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.True(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Count);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
        }

        [Fact]
        public void DeserializationOption_GreedyMutable_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.True(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.True(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));
            Assert.Equal(Strings.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        [Fact]
        public void DeserializationOption_GreedyMutable_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.GreedyMutable, Bytes); 
            InputBuffer.AsSpan().Fill(0);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.False(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        [Fact]
        public void DeserializationOption_GreedyMutable_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.GreedyMutable, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.False(table.Vector.Span.Overlaps(InputBuffer));
            Assert.True(object.ReferenceEquals(table.First, table.First));
            Assert.True(object.ReferenceEquals(table.Second, table.Second));

            Assert.Equal(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        private string GetInputBufferHash()
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(InputBuffer);
                return Convert.ToBase64String(hash);
            }
        }

        private InnerTable<T> SerializeAndParse<T>(FlatBufferDeserializationOption option, T item)
        {
            FlatBufferSerializer serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(option));

            InnerTable<T> value = new InnerTable<T>
            {
                First = new FirstStruct { First = 1 },
                Second = new SecondStruct { Second = 2, },
                String = "Foo bar baz bat",
                Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>(new SecondStruct { Second = 3 }),
                Vector = item,
            };

            serializer.Serialize(value, InputBuffer);
            InnerTable<T> parsed = serializer.Parse<InnerTable<T>>(InputBuffer); 

            Assert.Equal(1, parsed.First.First);
            Assert.Equal(2, parsed.Second.Second);
            Assert.Equal("Foo bar baz bat", parsed.String);
            Assert.Equal(3, parsed.Union.Value.Item2.Second);

            return parsed;
        }

        [FlatBufferTable]
        public class InnerTable<TVector> where TVector : notnull
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }

            [FlatBufferItem(1)]
            public virtual string? String { get; set; }

            [FlatBufferItem(2)]
            public virtual FirstStruct? First { get; set; }

            [FlatBufferItem(3)]
            public virtual SecondStruct? Second { get; set; }

            [FlatBufferItem(4)]
            public virtual FlatBufferUnion<FirstStruct, SecondStruct, string>? Union { get; set; }

            [FlatBufferItem(6)]
            public virtual ulong NoSetter { get; }
        }

        [FlatBufferStruct]
        public class FirstStruct
        {
            [FlatBufferItem(0)]
            public virtual int First { get; set; }

            [FlatBufferItem(1)]
            public virtual ulong NoSetter { get; }

            [FlatBufferItem(2)]
            public SecondStruct SecondStruct { get; set; }
        }

        [FlatBufferStruct]
        public class SecondStruct
        {
            [FlatBufferItem(0)]
            public virtual int Second { get; set; }
        }
    }
}
