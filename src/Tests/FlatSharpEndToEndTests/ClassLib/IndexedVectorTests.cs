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

namespace FlatSharpEndToEndTests.ClassLib.IndexedVectorTests;

/// <summary>
/// Tests for the FlatBufferVector class that implements IList.
/// </summary>
[TestClass]
public class IndexedVectorTests
{
    private List<string> stringKeys;

    private Container stringVectorSource;
    private Container intVectorSource;

    public IndexedVectorTests()
    {
        this.stringVectorSource = new Container { StringVector = new IndexedVector<string, StringKey>() };
        this.intVectorSource = new Container { IntVector = new IndexedVector<int, IntKey>() };
        this.stringKeys = new List<string>();

        for (int i = 0; i < 10; ++i)
        {
            string key = i.ToString();
            stringKeys.Add(key);

            this.stringVectorSource.StringVector.AddOrReplace(new StringKey { Key = key, Value = Guid.NewGuid().ToString() });
            this.intVectorSource.IntVector.AddOrReplace(new IntKey { Key = i, Value = Guid.NewGuid().ToString() });
        }

        this.stringVectorSource.StringVector.Freeze();
        this.intVectorSource.IntVector.Freeze();
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void IndexedVector_KeyNotFound(FlatBufferDeserializationOption option)
    {
        Container stringValue = this.stringVectorSource.SerializeAndParse(option);
        Container intValue = this.intVectorSource.SerializeAndParse(option);

        Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorSource.StringVector[null]);
        Assert.ThrowsException<KeyNotFoundException>(() => this.stringVectorSource.StringVector[string.Empty]);
        Assert.ThrowsException<ArgumentNullException>(() => stringValue.StringVector[null]);
        Assert.ThrowsException<KeyNotFoundException>(() => stringValue.StringVector[string.Empty]);

        Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorSource.IntVector[int.MinValue]);
        Assert.ThrowsException<KeyNotFoundException>(() => this.intVectorSource.IntVector[int.MaxValue]);
        Assert.ThrowsException<KeyNotFoundException>(() => intValue.IntVector[int.MinValue]);
        Assert.ThrowsException<KeyNotFoundException>(() => intValue.IntVector[int.MaxValue]);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void IndexedVector_NotMutable(FlatBufferDeserializationOption option)
    {
        Container stringValue = this.stringVectorSource.SerializeAndParse(option);

        Assert.AreEqual(
            FlatBufferDeserializationOption.GreedyMutable != option,
            stringValue.StringVector.IsReadOnly);

        Assert.IsTrue(this.stringVectorSource.StringVector.IsReadOnly);

        // root is frozen.
        Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.StringVector.AddOrReplace(null));
        Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.StringVector.Clear());
        Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.StringVector.Remove(null));
        Assert.ThrowsException<NotMutableException>(() => this.stringVectorSource.StringVector.Add(null));

        Action[] actions = new Action[]
        {
            () => stringValue.StringVector.AddOrReplace(new StringKey { Key = "foo" }),
            () => stringValue.StringVector.Clear(),
            () => stringValue.StringVector.Remove("foo"),
            () => stringValue.StringVector.Add(new StringKey { Key = "foo" }),
        };

        foreach (var item in actions)
        {
            if (option == FlatBufferDeserializationOption.GreedyMutable)
            {
                item();
            }
            else
            {
                Assert.ThrowsException<NotMutableException>(item);
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void IndexedVector_GetEnumerator(FlatBufferDeserializationOption option)
    {
        Container stringValue = this.stringVectorSource.SerializeAndParse(option);

        EnumeratorTest(stringValue.StringVector);
        EnumeratorTest(this.stringVectorSource.StringVector);
    }

    private void EnumeratorTest(IIndexedVector<string, StringKey> vector)
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

        Assert.IsTrue(keys.Count == 0);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void IndexedVector_ContainsKey(FlatBufferDeserializationOption option)
    {
        Container stringValue = this.stringVectorSource.SerializeAndParse(option);

        Assert.IsTrue(this.stringVectorSource.StringVector.ContainsKey("1"));
        Assert.IsTrue(this.stringVectorSource.StringVector.ContainsKey("5"));
        Assert.IsFalse(this.stringVectorSource.StringVector.ContainsKey("20"));
        Assert.ThrowsException<ArgumentNullException>(() => this.stringVectorSource.StringVector.ContainsKey(null));

        Assert.IsTrue(stringValue.StringVector.ContainsKey("1"));
        Assert.IsTrue(stringValue.StringVector.ContainsKey("5"));
        Assert.IsFalse(stringValue.StringVector.ContainsKey("20"));
        Assert.ThrowsException<ArgumentNullException>(() => stringValue.StringVector.ContainsKey(null));
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void IndexedVector_Caching(FlatBufferDeserializationOption option)
    {
        Container stringValue = this.stringVectorSource.SerializeAndParse(option);

        foreach (var key in this.stringKeys)
        {
            Assert.IsTrue(stringValue.StringVector.TryGetValue(key, out var value));
            Assert.IsTrue(stringValue.StringVector.TryGetValue(key, out var value2));
            Assert.AreEqual(
                option != FlatBufferDeserializationOption.Lazy,
                object.ReferenceEquals(value, value2));
        }
    }

    [TestMethod]
    public void IndexedVector_Mutations()
    {
        IndexedVector<string, StringKey> vector = new IndexedVector<string, StringKey>();
        var item = new StringKey { Key = "foobar" };

        Assert.IsFalse(vector.ContainsKey(item.Key));
        Assert.IsFalse(vector.TryGetValue(item.Key, out _));
        Assert.AreEqual(0, vector.Count);
        Assert.IsTrue(vector.Add(item));
        Assert.IsFalse(vector.Add(item));
        Assert.IsTrue(vector.ContainsKey(item.Key));
        Assert.IsTrue(vector.TryGetValue(item.Key, out _));
        Assert.AreEqual(1, vector.Count);
        Assert.IsTrue(vector.Remove(item.Key));
        Assert.IsFalse(vector.Remove(item.Key));
        Assert.AreEqual(0, vector.Count);
        Assert.IsTrue(vector.Add(item));
        Assert.AreEqual(1, vector.Count);
        vector.Clear();
        Assert.AreEqual(0, vector.Count);
        Assert.IsFalse(vector.ContainsKey(item.Key));
        Assert.IsFalse(vector.TryGetValue(item.Key, out _));
    }
}
