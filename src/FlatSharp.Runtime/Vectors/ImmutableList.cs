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
/// An immutable wrapper around a List{T}.
/// </summary>
/// <remarks>
/// This class serves broadly the same purpose as <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/>.
/// However, it is superior for two reasons:
/// - First, it throws the FlatSharp NotMutableException, which is consistent with Lazy and Progressive Deserialization modes
/// - Second, it does not reference <see cref="IList{T}"/> internally, which means it is able to skip a level of virtual indirection
///   by using <see cref="T[]"/> directly.
/// </remarks>
public sealed class ImmutableList<T> : IList<T>, IReadOnlyList<T>
{
    private readonly T[] list;

    public ImmutableList(T[] list)
    {
        this.list = list;
    }

    public T this[int index] 
    {
        get => this.list[index];
        set => throw new NotMutableException();
    }

    public int Count => this.list.Length;

    public bool IsReadOnly => true;

    public void Add(T item)
    {
        throw new NotMutableException();
    }

    public void Clear()
    {
        throw new NotMutableException();
    }

    public bool Contains(T item)
    {
        return Array.IndexOf(this.list, item) >= 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        this.list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IList<T>)this.list).GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return Array.IndexOf(this.list, item);
    }

    public void Insert(int index, T item)
    {
        throw new NotMutableException();
    }

    public bool Remove(T item)
    {
        throw new NotMutableException();
    }

    public void RemoveAt(int index)
    {
        throw new NotMutableException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}