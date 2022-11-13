/*
 * Copyright 2021 James Courtney
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
using System.Collections.Concurrent;

namespace FlatSharpEndToEndTests.PoolingTests;

public class TestObjectPool : IObjectPool
{
    private readonly ConcurrentDictionary<Type, TypedPool> pool = new();

    public void Return<T>(T item)
    {
        this.GetPool(typeof(T)).Return(item);
    }

    public bool TryGet<T>(out T? value)
    {
        bool result = this.GetPool(typeof(T)).TryGet(out var obj);

        value = (T?)obj;
        return result;
    }

    public bool IsInPool(object item)
    {
        return GetPool(item.GetType()).Contains(item);
    }

    public int Count(object item)
    {
        return GetPool(item.GetType()).Count;
    }

    private TypedPool GetPool(Type type)
    {
        return pool.GetOrAdd(type, new TypedPool());
    }

    private class TypedPool
    {
        private readonly object syncRoot = new();
        private readonly HashSet<object> members = new();
        private readonly Queue<object> dequeuePool = new();

        public int Count => members.Count;

        public void Return(object item)
        {
            lock (syncRoot)
            {
                Assert.True(members.Add(item));
                dequeuePool.Enqueue(item);
            }
        }

        public bool TryGet(out object? item)
        {
            lock (syncRoot)
            {
                if (dequeuePool.Count > 0)
                {
                    item = dequeuePool.Dequeue();
                    Assert.True(members.Remove(item));
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
        }

        public bool Contains(object item)
        {
            lock (syncRoot)
            {
                return members.Contains(item);
            }
        }
    }
}
