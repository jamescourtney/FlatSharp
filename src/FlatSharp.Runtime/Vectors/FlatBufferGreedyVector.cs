﻿/*
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
/// A greedy, immutable vector. Wraps a List{T}.
/// </summary>
public sealed class FlatBufferGreedyVector<T>
    : IList<T>
    , IReadOnlyList<T>
{
    private readonly List<T> innerList;

    public FlatBufferGreedyVector(List<T> innerList)
    {
        this.innerList = innerList;
    }

    public T this[int index]
    {
        get => this.innerList[index];
        set => this.innerList[index] = value;
    }

    public int Count => this.innerList.Count;

    public bool IsReadOnly => true;

    public void Add(T item)
    {
        throw new NotMutableException("Greedy vectors are not mutable.");
    }

    public void Clear()
    {
        throw new NotMutableException("Greedy vectors are not mutable.");
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
        throw new NotMutableException("Greedy vectors are not mutable.");
    }

    public bool Remove(T item)
    {
        throw new NotMutableException("Greedy vectors are not mutable.");
    }

    public void RemoveAt(int index)
    {
        throw new NotMutableException("Greedy vectors are not mutable.");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.innerList.GetEnumerator();
    }
}