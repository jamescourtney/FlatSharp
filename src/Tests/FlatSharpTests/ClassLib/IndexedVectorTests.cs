/*
 * Copyright 2020 James Courtney
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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    
    public class IndexedVectorTests
    {
        private List<string> stringKeys;

        private TableVector<string> stringVectorSource;
        private TableVector<string> stringVectorParsed;
        private TableVector<string> stringVectorProgressive;

        private TableVector<int> intVectorSource;
        private TableVector<int> intVectorParsed;
        private TableVector<int> intVectorProgressive;

        public IndexedVectorTests()
        {
            this.stringVectorSource = new TableVector<string> { Vector = new IndexedVector<string, VectorMember<string>>() };
            this.intVectorSource = new TableVector<int> { Vector = new IndexedVector<int, VectorMember<int>>() };
            this.stringKeys = new List<string>();

            for (int i = 0; i < 10; ++i)
            {
                string key = i.ToString();
                stringKeys.Add(key);

                this.stringVectorSource.Vector.AddOrReplace(new VectorMember<string> { Key = key, Value = Guid.NewGuid().ToString() });
                this.intVectorSource.Vector.AddOrReplace(new VectorMember<int> { Key = i, Value = Guid.NewGuid().ToString() });
            }

            this.stringVectorSource.Vector.Freeze();
            this.intVectorSource.Vector.Freeze();

            Span<byte> buffer = new byte[1024];

            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));
            var progressiveSerializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Progressive));

            int bytesWritten = serializer.Serialize(this.stringVectorSource, buffer);
            this.stringVectorParsed = serializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());
            this.stringVectorProgressive = progressiveSerializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());

            bytesWritten = serializer.Serialize(this.intVectorSource, buffer);
            this.intVectorParsed = serializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
            this.intVectorProgressive = progressiveSerializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [Fact]
        public void IndexedVector_KeyNotFound()
        {
            Assert.Throws<ArgumentNullException>(() => this.stringVectorSource.Vector[null]);
            Assert.Throws<KeyNotFoundException>(() => this.stringVectorSource.Vector[string.Empty]);
            Assert.Throws<ArgumentNullException>(() => this.stringVectorParsed.Vector[null]);
            Assert.Throws<KeyNotFoundException>(() => this.stringVectorParsed.Vector[string.Empty]);
            Assert.Throws<ArgumentNullException>(() => this.stringVectorProgressive.Vector[null]);
            Assert.Throws<KeyNotFoundException>(() => this.stringVectorProgressive.Vector[string.Empty]);

            Assert.Throws<KeyNotFoundException>(() => this.intVectorSource.Vector[int.MinValue]);
            Assert.Throws<KeyNotFoundException>(() => this.intVectorSource.Vector[int.MaxValue]);
            Assert.Throws<KeyNotFoundException>(() => this.intVectorParsed.Vector[int.MinValue]);
            Assert.Throws<KeyNotFoundException>(() => this.intVectorParsed.Vector[int.MaxValue]);
            Assert.Throws<KeyNotFoundException>(() => this.intVectorProgressive.Vector[int.MinValue]);
            Assert.Throws<KeyNotFoundException>(() => this.intVectorProgressive.Vector[int.MaxValue]);
        }

        [Fact]
        public void IndexedVector_NotMutable()
        {
            Assert.True(this.stringVectorParsed.Vector.IsReadOnly);
            Assert.True(this.stringVectorSource.Vector.IsReadOnly);

            Assert.Throws<NotMutableException>(() => this.stringVectorSource.Vector.AddOrReplace(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorSource.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.stringVectorSource.Vector.Remove(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorSource.Vector.Add(null));

            Assert.Throws<NotMutableException>(() => this.stringVectorParsed.Vector.AddOrReplace(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorParsed.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.stringVectorParsed.Vector.Remove(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorParsed.Vector.Add(null));

            Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.Vector.AddOrReplace(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.Vector.Remove(null));
            Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.Vector.Add(null));
        }

        [Fact]
        public void IndexedVector_GetEnumerator()
        {
            EnumeratorTest(this.stringVectorParsed.Vector);
            EnumeratorTest(this.stringVectorSource.Vector);
            EnumeratorTest(this.stringVectorProgressive.Vector);
        }

        private void EnumeratorTest(IIndexedVector<string, VectorMember<string>> vector)
        {
            int i = 0;
            foreach (var item in vector)
            {
                Assert.Equal(item.Key, i.ToString());
                Assert.Equal(item.Value.Key, i.ToString());
                i++;
            }

            var keys = new HashSet<string>(this.stringKeys);
            Assert.Equal(vector.Count, keys.Count);
            foreach (string key in keys)
            {
                Assert.True(vector.ContainsKey(key));
            }

            foreach (var kvp in vector)
            {
                Assert.True(vector.ContainsKey(kvp.Key));
                Assert.True(vector.TryGetValue(kvp.Key, out var value));
                Assert.Equal(kvp.Key, value.Key);

                keys.Remove(value.Key);
            }

            Assert.Empty(keys);
        }

        [Fact]
        public void IndexedVector_ContainsKey()
        {
            Assert.True(this.stringVectorSource.Vector.ContainsKey("1"));
            Assert.True(this.stringVectorSource.Vector.ContainsKey("5"));
            Assert.False(this.stringVectorSource.Vector.ContainsKey("20"));
            Assert.Throws<ArgumentNullException>(() => this.stringVectorSource.Vector.ContainsKey(null));

            Assert.True(this.stringVectorParsed.Vector.ContainsKey("1"));
            Assert.True(this.stringVectorParsed.Vector.ContainsKey("5"));
            Assert.False(this.stringVectorParsed.Vector.ContainsKey("20"));
            Assert.Throws<ArgumentNullException>(() => this.stringVectorParsed.Vector.ContainsKey(null));

            Assert.True(this.stringVectorProgressive.Vector.ContainsKey("1"));
            Assert.True(this.stringVectorProgressive.Vector.ContainsKey("5"));
            Assert.False(this.stringVectorProgressive.Vector.ContainsKey("20"));
            Assert.Throws<ArgumentNullException>(() => this.stringVectorProgressive.Vector.ContainsKey(null));
        }

        [Fact]
        public void IndexedVector_Caching() 
        {
            foreach (var key in this.stringKeys)
            {
                Assert.True(this.stringVectorParsed.Vector.TryGetValue(key, out var value));
                Assert.True(this.stringVectorParsed.Vector.TryGetValue(key, out var value2));
                Assert.NotSame(value, value2);

                Assert.True(this.stringVectorProgressive.Vector.TryGetValue(key, out value));
                Assert.True(this.stringVectorProgressive.Vector.TryGetValue(key, out value2));
                Assert.Same(value, value2);
            }
        }

        [Fact]
        public void IndexedVector_Mutations()
        {
            IndexedVector<string, VectorMember<string>> vector = new IndexedVector<string, VectorMember<string>>();
            var item = new VectorMember<string> { Key = "foobar" };

            Assert.False(vector.ContainsKey(item.Key));
            Assert.False(vector.TryGetValue(item.Key, out _));
            Assert.Equal(0, vector.Count);
            Assert.Empty(vector);
            Assert.True(vector.Add(item));
            Assert.False(vector.Add(item));
            Assert.True(vector.ContainsKey(item.Key));
            Assert.True(vector.TryGetValue(item.Key, out _));
            Assert.Equal(1, vector.Count);
            Assert.Single(vector);
            Assert.True(vector.Remove(item.Key));
            Assert.False(vector.Remove(item.Key));
            Assert.Equal(0, vector.Count);
            Assert.Empty(vector);
            Assert.True(vector.Add(item));
            Assert.Equal(1, vector.Count);
            Assert.Single(vector);
            vector.Clear();
            Assert.Equal(0, vector.Count);
            Assert.Empty(vector);
            Assert.False(vector.ContainsKey(item.Key));
            Assert.False(vector.TryGetValue(item.Key, out _));
        }

        [FlatBufferTable]
        public class TableVector<TKey> where TKey : notnull
        {
            [FlatBufferItem(0)]
            public virtual IIndexedVector<TKey, VectorMember<TKey>>? Vector { get; set; }
        }

        [FlatBufferTable]
        public class VectorMember<TKey> where TKey : notnull
        {
            [FlatBufferItem(0, Key = true)]
            public virtual TKey? Key { get; set; }

            [FlatBufferItem(1)]
            public virtual string? Value { get; set; }
        }
    }
}
