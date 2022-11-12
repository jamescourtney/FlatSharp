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
public class IndexedVectorTests
{
    private List<string> stringKeys;

    private Container stringVectorSource;
    private Container stringVectorLazy;
    private Container stringVectorProgressive;

    private Container intVectorSource;
    private Container intVectorParsed;
    private Container intVectorProgressive;

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

        this.stringVectorLazy = stringVectorSource.SerializeAndParse(FlatBufferDeserializationOption.Lazy);
        this.stringVectorProgressive = stringVectorSource.SerializeAndParse(FlatBufferDeserializationOption.Progressive);

        this.intVectorParsed = intVectorSource.SerializeAndParse(FlatBufferDeserializationOption.Lazy);
        this.intVectorProgressive = intVectorSource.SerializeAndParse(FlatBufferDeserializationOption.Progressive);
    }

    [Fact]
    public void IndexedVector_KeyNotFound()
    {
        Assert.Throws<ArgumentNullException>(() => this.stringVectorSource.StringVector[null]);
        Assert.Throws<KeyNotFoundException>(() => this.stringVectorSource.StringVector[string.Empty]);
        Assert.Throws<ArgumentNullException>(() => this.stringVectorLazy.StringVector[null]);
        Assert.Throws<KeyNotFoundException>(() => this.stringVectorLazy.StringVector[string.Empty]);
        Assert.Throws<ArgumentNullException>(() => this.stringVectorProgressive.StringVector[null]);
        Assert.Throws<KeyNotFoundException>(() => this.stringVectorProgressive.StringVector[string.Empty]);

        Assert.Throws<KeyNotFoundException>(() => this.intVectorSource.IntVector[int.MinValue]);
        Assert.Throws<KeyNotFoundException>(() => this.intVectorSource.IntVector[int.MaxValue]);
        Assert.Throws<KeyNotFoundException>(() => this.intVectorParsed.IntVector[int.MinValue]);
        Assert.Throws<KeyNotFoundException>(() => this.intVectorParsed.IntVector[int.MaxValue]);
        Assert.Throws<KeyNotFoundException>(() => this.intVectorProgressive.IntVector[int.MinValue]);
        Assert.Throws<KeyNotFoundException>(() => this.intVectorProgressive.IntVector[int.MaxValue]);
    }

    [Fact]
    public void IndexedVector_NotMutable()
    {
        Assert.True(this.stringVectorLazy.StringVector.IsReadOnly);
        Assert.True(this.stringVectorSource.StringVector.IsReadOnly);

        Assert.Throws<NotMutableException>(() => this.stringVectorSource.StringVector.AddOrReplace(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorSource.StringVector.Clear());
        Assert.Throws<NotMutableException>(() => this.stringVectorSource.StringVector.Remove(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorSource.StringVector.Add(null));

        Assert.Throws<NotMutableException>(() => this.stringVectorLazy.StringVector.AddOrReplace(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorLazy.StringVector.Clear());
        Assert.Throws<NotMutableException>(() => this.stringVectorLazy.StringVector.Remove(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorLazy.StringVector.Add(null));

        Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.StringVector.AddOrReplace(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.StringVector.Clear());
        Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.StringVector.Remove(null));
        Assert.Throws<NotMutableException>(() => this.stringVectorProgressive.StringVector.Add(null));
    }

    [Fact]
    public void IndexedVector_GetEnumerator()
    {
        EnumeratorTest(this.stringVectorLazy.StringVector);
        EnumeratorTest(this.stringVectorSource.StringVector);
        EnumeratorTest(this.stringVectorProgressive.StringVector);
    }

    private void EnumeratorTest(IIndexedVector<string, StringKey> vector)
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
        Assert.True(this.stringVectorSource.StringVector.ContainsKey("1"));
        Assert.True(this.stringVectorSource.StringVector.ContainsKey("5"));
        Assert.False(this.stringVectorSource.StringVector.ContainsKey("20"));
        Assert.Throws<ArgumentNullException>(() => this.stringVectorSource.StringVector.ContainsKey(null));

        Assert.True(this.stringVectorLazy.StringVector.ContainsKey("1"));
        Assert.True(this.stringVectorLazy.StringVector.ContainsKey("5"));
        Assert.False(this.stringVectorLazy.StringVector.ContainsKey("20"));
        Assert.Throws<ArgumentNullException>(() => this.stringVectorLazy.StringVector.ContainsKey(null));

        Assert.True(this.stringVectorProgressive.StringVector.ContainsKey("1"));
        Assert.True(this.stringVectorProgressive.StringVector.ContainsKey("5"));
        Assert.False(this.stringVectorProgressive.StringVector.ContainsKey("20"));
        Assert.Throws<ArgumentNullException>(() => this.stringVectorProgressive.StringVector.ContainsKey(null));
    }

    [Fact]
    public void IndexedVector_Caching()
    {
        foreach (var key in this.stringKeys)
        {
            Assert.True(this.stringVectorLazy.StringVector.TryGetValue(key, out var value));
            Assert.True(this.stringVectorLazy.StringVector.TryGetValue(key, out var value2));
            Assert.NotSame(value, value2);

            Assert.True(this.stringVectorProgressive.StringVector.TryGetValue(key, out value));
            Assert.True(this.stringVectorProgressive.StringVector.TryGetValue(key, out value2));
            Assert.Same(value, value2);
        }
    }

    [Fact]
    public void IndexedVector_Mutations()
    {
        IndexedVector<string, StringKey> vector = new IndexedVector<string, StringKey>();
        var item = new StringKey { Key = "foobar" };

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
}
