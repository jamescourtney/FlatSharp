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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    [TestClass]
    public class IndexedVectorTests
    {
        private List<string> stringKeys;
        private TableVector<string> stringVectorSource;
        private TableVector<string> stringVectorParsed;
        private TableVector<int> intVectorSource;
        private TableVector<int> intVectorParsed;

        [TestInitialize]
        public void TestInitialize()
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
            int bytesWritten = serializer.Serialize(this.stringVectorSource, buffer);
            this.stringVectorParsed = serializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());

            bytesWritten = serializer.Serialize(this.intVectorSource, buffer);
            this.intVectorParsed = serializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [TestMethod]
        public void IndexedVector_KeyNotFound()
        {
            Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorSource.Vector[null]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.stringVectorSource.Vector[string.Empty]);
            Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorParsed.Vector[null]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.stringVectorParsed.Vector[string.Empty]);

            Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorSource.Vector[int.MinValue]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorSource.Vector[int.MaxValue]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorParsed.Vector[int.MinValue]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorParsed.Vector[int.MaxValue]);
        }

        [TestMethod]
        public void IndexedVector_NotMutable()
        {
            Assert.IsTrue(this.stringVectorParsed.Vector.IsReadOnly);
            Assert.IsTrue(this.stringVectorSource.Vector.IsReadOnly);

            Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.Vector.AddOrReplace(null));
            Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.Vector.Remove(null));
            Assert.IsFalse(this.stringVectorSource.Vector.Add(null));

            Assert.ThrowsException<NotMutableException>(() => this.stringVectorParsed.Vector.AddOrReplace(null));
            Assert.ThrowsException<NotMutableException>(() => this.stringVectorParsed.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => this.stringVectorParsed.Vector.Remove(null));
            Assert.IsFalse(this.stringVectorParsed.Vector.Add(null));
        }

        [TestMethod]
        public void IndexedVector_GetEnumerator()
        {
            EnumeratorTest(this.stringVectorParsed.Vector);
            EnumeratorTest(this.stringVectorSource.Vector);
        }

        private void EnumeratorTest(IIndexedVector<string, VectorMember<string>> vector)
        {
            int i = 0;
            foreach (var item in vector)
            {
                Assert.AreEqual(item.Key, i.ToString());
                Assert.AreEqual(item.Value.Key, i.ToString());
                i++;
            }

            var keys = new HashSet<string>(this.stringKeys);
            Assert.AreEqual(vector.Count, keys.Count);
            foreach (string key in keys)
            {
                Assert.IsTrue(vector.ContainsKey(key));
            }

            foreach (var kvp in vector)
            {
                Assert.IsTrue(vector.ContainsKey(kvp.Key));
                Assert.IsTrue(vector.TryGetValue(kvp.Key, out var value));
                Assert.AreEqual(kvp.Key, value.Key);

                keys.Remove(value.Key);
            }

            Assert.AreEqual(0, keys.Count);
        }

        [TestMethod]
        public void IndexedVector_ContainsKey()
        {
            Assert.IsTrue(this.stringVectorSource.Vector.ContainsKey("1"));
            Assert.IsTrue(this.stringVectorSource.Vector.ContainsKey("5"));
            Assert.IsFalse(this.stringVectorSource.Vector.ContainsKey("20"));
            Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorSource.Vector.ContainsKey(null));

            Assert.IsTrue(this.stringVectorParsed.Vector.ContainsKey("1"));
            Assert.IsTrue(this.stringVectorParsed.Vector.ContainsKey("5"));
            Assert.IsFalse(this.stringVectorParsed.Vector.ContainsKey("20"));
            Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorParsed.Vector.ContainsKey(null));
        }

        [TestMethod]
        public void IndexedVector_Mutations()
        {
            IndexedVector<string, VectorMember<string>> vector = new IndexedVector<string, VectorMember<string>>();
            var item = new VectorMember<string> { Key = "foobar" };

            Assert.IsFalse(vector.ContainsKey(item.Key));
            Assert.IsFalse(vector.TryGetValue(item.Key, out _));
            Assert.AreEqual(0, vector.Count);
            Assert.AreEqual(0, vector.Count());
            Assert.IsTrue(vector.Add(item));
            Assert.IsFalse(vector.Add(item));
            Assert.IsTrue(vector.ContainsKey(item.Key));
            Assert.IsTrue(vector.TryGetValue(item.Key, out _));
            Assert.AreEqual(1, vector.Count);
            Assert.AreEqual(1, vector.Count());
            Assert.IsTrue(vector.Remove(item.Key));
            Assert.IsFalse(vector.Remove(item.Key));
            Assert.AreEqual(0, vector.Count);
            Assert.AreEqual(0, vector.Count());
            Assert.IsTrue(vector.Add(item));
            Assert.AreEqual(1, vector.Count);
            Assert.AreEqual(1, vector.Count());
            vector.Clear();
            Assert.AreEqual(0, vector.Count);
            Assert.AreEqual(0, vector.Count());
            Assert.IsFalse(vector.ContainsKey(item.Key));
            Assert.IsFalse(vector.TryGetValue(item.Key, out _));
        }

        [FlatBufferTable]
        public class TableVector<TKey>
        {
            [FlatBufferItem(0)]
            public virtual IIndexedVector<TKey, VectorMember<TKey>> Vector { get; set; }
        }

        [FlatBufferTable]
        public class VectorMember<TKey>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual TKey Key { get; set; }

            [FlatBufferItem(1)]
            public virtual string Value { get; set; }
        }
    }
}
