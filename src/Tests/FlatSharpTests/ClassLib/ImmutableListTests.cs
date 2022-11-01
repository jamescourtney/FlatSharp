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

namespace FlatSharpTests;

/// <summary>
/// Tests for the FlatBufferVector class that implements IList.
/// </summary>

public class ImmutableListTests
{
    private readonly ImmutableList<int> items = new ImmutableList<int>(new[] { 0, 1, 2, 3, 4, 5, });

    [Fact]
    public void Clear_NotSupported()
    {
        Assert.Throws<NotMutableException>(this.items.Clear);
    }

    [Fact]
    public void RemoveAt_NotSupported()
    {
        Assert.Throws<NotMutableException>(() => this.items.RemoveAt(0));
    }

    [Fact]
    public void Remove_NotSupported()
    {
        Assert.Throws<NotMutableException>(() => this.items.Remove(0));
    }

    [Fact]
    public void Setter_NotSupported()
    {
        Assert.Throws<NotMutableException>(() => this.items[0] = 5);
    }

    [Fact]
    public void Add_NotSupported()
    {
        Assert.Throws<NotMutableException>(() => this.items.Add(6));
    }

    [Fact]
    public void Insert_NotSupported()
    {
        Assert.Throws<NotMutableException>(() => this.items.Insert(6, 6));
    }

    [Fact]
    public void Get()
    {
        for (int i = 0; i < this.items.Count; ++i)
        {
            Assert.Equal(i, this.items[i]);
        }
    }

    [Fact]
    public void Contains()
    {
        for (int i = 0; i < this.items.Count; ++i)
        {
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
                                  // Justification: want to ensure correct method is invoked.

            Assert.True(this.items.Contains(i));

#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection

            Assert.Equal(i, this.items.IndexOf(i));
        }
    }

    [Fact]
    public void GetEnumerator()
    {
        int i = 0;
        foreach (var item in this.items)
        {
            Assert.Equal(i++, item);
        }

        Assert.Equal(i, this.items.Count);
    }

    [Fact]
    public void CopyTo()
    {
        int[] temp = new int[20];
        this.items.CopyTo(temp, 10);

        for (int i = 0; i < this.items.Count; ++i)
        {
            Assert.Equal(temp[i + 10], this.items[i]);
        }
    }

    [Fact]
    public void ReadOnly()
    {
        Assert.True(this.items.IsReadOnly);
    }
}
