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
    public class FlatBufferDictionaryVectorTests
    {
        private TableVector<string> stringDictionary;
        private TableVector<int> intDictionary;

        [TestInitialize]
        public void TestInitialize()
        {
            this.stringDictionary = new TableVector<string> { Vector = new Dictionary<string, VectorMember<string>>() };
            this.intDictionary = new TableVector<int> { Vector = new Dictionary<int, VectorMember<int>>() };

            for (int i = 0; i < 10; ++i)
            {
                this.stringDictionary.Vector[i.ToString()] = new VectorMember<string> { Key = i.ToString(), Value = Guid.NewGuid().ToString() };
                this.intDictionary.Vector[i] = new VectorMember<int> { Key = i, Value = Guid.NewGuid().ToString() };
            }

            Span<byte> buffer = new byte[1024];

            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));
            int bytesWritten = serializer.Serialize(this.stringDictionary, buffer);
            this.stringDictionary = serializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());

            bytesWritten = serializer.Serialize(this.intDictionary, buffer);
            this.intDictionary = serializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [TestMethod]
        public void FlatBufferDictionary_KeyNotFound()
        {
            Assert.ThrowsException<ArgumentNullException>(() => this.stringDictionary.Vector[null]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.stringDictionary.Vector[string.Empty]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.intDictionary.Vector[int.MinValue]);
            Assert.ThrowsException<KeyNotFoundException>(() => this.intDictionary.Vector[int.MaxValue]);
        }

        [TestMethod]
        public void FlatBufferDictionary_NotMutable()
        {
            Assert.IsTrue(this.stringDictionary.Vector.IsReadOnly);
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector["blah"] = null);
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Add("foobar", null));
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Remove("foobar"));

            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector["blah"] = null);
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Add("foobar", null));
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Remove("foobar"));
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Add(default));
            Assert.ThrowsException<NotMutableException>(() => this.stringDictionary.Vector.Remove(default(KeyValuePair<string, VectorMember<string>>)));
            Assert.ThrowsException<NotImplementedException>(() => this.stringDictionary.Vector.Contains(default));
        }

        [TestMethod]
        public void FlatBufferDictionary_GetEnumerator()
        {
            int i = 0; 
            foreach (var item in this.stringDictionary.Vector)
            {
                Assert.AreEqual(item.Key, i.ToString());
                Assert.AreEqual(item.Value.Key, i.ToString());
                i++;
            }

            i = 0;
            foreach (var item in this.intDictionary.Vector)
            {
                Assert.AreEqual(item.Key, i);
                Assert.AreEqual(item.Value.Key, i);
                i++;
            }

            var keys = new HashSet<string>(this.stringDictionary.Vector.Keys);
            Assert.AreEqual(this.stringDictionary.Vector.Count, keys.Count);
            foreach (string key in keys)
            {
                Assert.IsTrue(this.stringDictionary.Vector.ContainsKey(key));
            }

            var values = this.stringDictionary.Vector.Values.ToList();
            Assert.AreEqual(this.stringDictionary.Vector.Count, values.Count);

            foreach (var value in values)
            {
                Assert.IsTrue(keys.Contains(value.Key));
                keys.Remove(value.Key);
            }

            Assert.AreEqual(0, keys.Count);
        }

        [TestMethod]
        public void FlatBufferDictionary_ContainsKey()
        {
            Assert.IsTrue(this.stringDictionary.Vector.ContainsKey("1"));
            Assert.IsTrue(this.stringDictionary.Vector.ContainsKey("5"));
            Assert.IsFalse(this.stringDictionary.Vector.ContainsKey("20"));
            Assert.ThrowsException<ArgumentNullException>(() => this.stringDictionary.Vector.ContainsKey(null));

            Assert.IsTrue(this.intDictionary.Vector.ContainsKey(1));
            Assert.IsTrue(this.intDictionary.Vector.ContainsKey(5));
            Assert.IsFalse(this.intDictionary.Vector.ContainsKey(int.MinValue));
            Assert.IsFalse(this.intDictionary.Vector.ContainsKey(int.MaxValue));
        }

        [TestMethod]
        public void FlatBufferDictionary_CopyTo()
        {
            var array = new KeyValuePair<string, VectorMember<string>>[100];

            this.stringDictionary.Vector.CopyTo(array, 50);
            
            for (int i = 0; i < 50; ++i)
            {
                Assert.AreEqual(default, array[i]);
            }

            for (int i = 0; i < this.stringDictionary.Vector.Count; ++i)
            {
                Assert.AreEqual(i.ToString(), array[i + 50].Key);
                Assert.AreEqual(i.ToString(), array[i + 50].Value.Key);
            }

            for (int i = 50 + this.stringDictionary.Vector.Count; i < array.Length; ++i)
            {
                Assert.AreEqual(default, array[i]);
            }
        }

        [FlatBufferTable]
        public class TableVector<TKey>
        {
            [FlatBufferItem(0)]
            public virtual IDictionary<TKey, VectorMember<TKey>> Vector { get; set; }
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
