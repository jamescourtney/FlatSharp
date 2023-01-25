/*
 * Copyright 2022 James Courtney
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

using FlatSharp.Internal;

namespace FlatSharpEndToEndTests.PoolableList;

/// <summary>
/// Tests for the FlatBufferVector class that implements IList.
/// </summary>

public class PoolableListTests
{
    private PoolableList<int> Items => new PoolableList<int>(new[] { 0, 1, 2, 3, 4, 5, });

    [Fact]
    public void Clear()
    {
        var items = this.Items;
        Assert.NotEmpty(items);
        items.Clear();
        Assert.Empty(items);
    }

    [Fact]
    public void RemoveAt()
    {
        var items = this.Items;
        Assert.Equal(2, items[2]);
        Assert.Equal(6, items.Count);
        items.RemoveAt(2);
        Assert.Equal(3, items[2]);
        Assert.Equal(5, items.Count);
    }

    [Fact]
    public void Remove()
    {
        var items = this.Items;
        Assert.Equal(6, items.Count);
        items.Remove(4);
        Assert.Equal(5, items.Count);
        Assert.Equal(5, items[4]);
    }

    [Fact]
    public void Setter()
    {
        var items = this.Items;
        Assert.Equal(2, items[2]);
        items[2] = 10;
        Assert.Equal(10, items[2]);
    }

    [Fact]
    public void Add()
    {
        var items = this.Items;
        Assert.Equal(6, items.Count);
        items.Add(6);
        Assert.Equal(7, items.Count);
        Assert.Equal(6, items[6]);
    }

    [Fact]
    public void Insert()
    {
        var items = this.Items;
        Assert.Equal(6, items.Count);
        items.Insert(1, 10);
        Assert.Equal(7, items.Count);
        Assert.Equal(10, items[1]);
        Assert.Equal(1, items[2]);
    }

    [Fact]
    public void Get()
    {
        var items = this.Items;
        for (int i = 0; i < items.Count; ++i)
        {
            Assert.Equal(i, items[i]);
        }
    }

    [Fact]
    public void Contains()
    {
        var items = this.Items;
        for (int i = 0; i < items.Count; ++i)
        {
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
                                  // Justification: want to ensure correct method is invoked.

            Assert.True(items.Contains(i));

#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection

            Assert.Equal(i, items.IndexOf(i));
        }
    }

    [Fact]
    public void GetEnumerator()
    {
        var items = this.Items;
        int i = 0;
        foreach (var item in items)
        {
            Assert.Equal(i++, item);
        }

        Assert.Equal(i, items.Count);
    }

    [Fact]
    public void CopyTo()
    {
        var items = this.Items;
        int[] temp = new int[20];
        items.CopyTo(temp, 10);

        for (int i = 0; i < items.Count; ++i)
        {
            Assert.Equal(temp[i + 10], items[i]);
        }
    }

    [Fact]
    public void ReadOnly()
    {
        var items = this.Items;
        Assert.False(items.IsReadOnly);
    }
}
