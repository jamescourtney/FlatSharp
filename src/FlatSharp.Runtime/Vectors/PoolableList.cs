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

using System.Linq;
using System.Threading;

namespace FlatSharp.Internal;

/// <summary>
/// A wrapper around a List{T}.
/// </summary>
public sealed class PoolableList<T> : IList<T>, IReadOnlyList<T>, IPoolableObject
{
    private List<T> list;
    private int isAlive;

    public PoolableList(IList<T> template)
    {
        this.list = template.ToList();
    }

    public PoolableList(int capacity)
    {
        this.list = new(capacity);
    }

    public static PoolableList<T> GetOrCreate<TInputBuffer, TItemAccessor>(FlatBufferVectorBase<T, TInputBuffer, TItemAccessor> item)
        where TInputBuffer : IInputBuffer
        where TItemAccessor : IVectorItemAccessor<T, TInputBuffer>
    {
        int count = item.Count;

        if (ObjectPool.TryGet(out PoolableList<T>? list))
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
            list.Add(item[i]);
        }

        // We've copied our stuff -- send the base vector back to where it came from!
        item.ReturnToPool(true);

        return list;
    }

    public T this[int index] 
    {
        get => this.list[index];
        set => this.list[index] = value;
    }

    public int Count => this.list.Count;

    public bool IsReadOnly => false;

    public void Add(T item) => this.list.Add(item);

    public void Clear() => this.list.Clear();

    public bool Contains(T item) => this.list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

    public int IndexOf(T item) => this.list.IndexOf(item);

    public void Insert(int index, T item) => this.list.Insert(index, item);

    public bool Remove(T item) => this.list.Remove(item);

    public void RemoveAt(int index) => this.list.RemoveAt(index);

    public void ReturnToPool(bool force)
    {
        if (force)
        {
            var isAlive = Interlocked.Exchange(ref this.isAlive, 0);
            if (isAlive != 0)
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