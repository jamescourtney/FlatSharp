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

using System.Buffers;
using System.Linq;
using System.Threading;

namespace FlatSharp.Internal;

/// <summary>
/// An immutable wrapper around a List{T}.
/// </summary>
/// <remarks>
/// This class serves broadly the same purpose as <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/>.
/// However, it is superior for two reasons:
/// - First, it throws the FlatSharp NotMutableException, which is consistent with Lazy and Progressive Deserialization modes
/// - Second, it does not reference <see cref="IList{T}"/> internally, which means it is able to skip a level of virtual indirection
///   by using <c><typeparamref name="T"/>[]</c> directly.
/// </remarks>
public sealed class ImmutableList<T> : IList<T>, IReadOnlyList<T>, IPoolableObject
{
    private List<T> list;
    private int isAlive;

    public ImmutableList(IList<T> template)
    {
        this.list = template.ToList();
    }

    public ImmutableList(int capacity)
    {
        this.list = new List<T>(capacity);
    }

    public static ImmutableList<T> GetOrCreate<TInputBuffer, TItemAccessor>(FlatBufferVectorBase<T, TInputBuffer, TItemAccessor> vector)
        where TInputBuffer : IInputBuffer
        where TItemAccessor : IVectorItemAccessor<T, T, TInputBuffer>
    {
        int count = vector.Count;

        if (ObjectPool.TryGet(out ImmutableList<T>? list))
        {
#if NET6_0_OR_GREATER
            list.list.EnsureCapacity(count);
#endif
        }
        else
        {
            list = new(count);
        }

        list.isAlive = 1;

        for (int i = 0; i < count; ++i)
        {
            list.list.Add(vector[i]);
        }

        // We've copied our stuff -- send the base vector back to where it came from!
        vector.ReturnToPool(true);

        return list;
    }

    public T this[int index] 
    {
        get => this.list[index];
        set => throw new NotMutableException();
    }

    public int Count => this.list.Count;

    public bool IsReadOnly => true;

    public void Add(T item)
    {
        throw new NotMutableException();
    }

    public void Clear()
    {
        throw new NotMutableException();
    }

    public bool Contains(T item) => this.list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator()
    {
        return this.list.GetEnumerator();
    }

    public int IndexOf(T item) => this.list.IndexOf(item);

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

    public void ReturnToPool(bool force)
    {
        if (force)
        {
            if (Interlocked.Exchange(ref this.isAlive, 0) != 0)
            {
                if (!typeof(T).IsValueType)
                {
                    foreach (var item in this.list)
                    {
                        if (item is IPoolableObject poolable)
                        {
                            poolable.ReturnToPool(true);
                        }
                    }
                }

                this.list.Clear();
                ObjectPool.Return(this);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}