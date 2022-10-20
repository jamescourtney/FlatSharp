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

namespace FlatSharp.Internal;

/// <summary>
/// A greedy mutable vector, wrapper around List{T}
/// </summary>
public sealed class FlatBufferGreedyMutableVector<T>
    : IList<T>
    , IReadOnlyList<T>
{
    private readonly List<T> innerList;

    public FlatBufferGreedyMutableVector(List<T> innerList)
    {
        this.innerList = innerList;
    }

    public T this[int index]
    {
        get => this.innerList[index];
        set => this.innerList[index] = value;
    }

    public int Count => this.innerList.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        this.innerList.Add(item);
    }

    public void Clear()
    {
        this.innerList.Clear();
    }

    public bool Contains(T item)
    {
        return this.innerList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        this.innerList.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return this.innerList.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return this.innerList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        this.innerList.Insert(index, item);
    }

    public bool Remove(T item)
    {
        return this.innerList.Remove(item);
    }

    public void RemoveAt(int index)
    {
        this.innerList.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.innerList.GetEnumerator();
    }
}
