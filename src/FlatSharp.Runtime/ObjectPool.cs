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

using System.Collections.Concurrent;
using System.Threading;

namespace FlatSharp.Internal
{
    /// <summary>
    /// Internal disposal state.
    /// </summary>
    public static class ObjectPoolDisposalState
    {
        /// <summary>
        /// Extension method to indicate if we should always dispose or not.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShouldReturnToPool(this FlatBufferDeserializationOption option, bool force)
        {
            return ObjectPool.Instance is not null && (force || option == FlatBufferDeserializationOption.Lazy);
        }
    }

    /// <summary>
    /// A static singleton that defines the FlatSharp object pool to use.
    /// </summary>
    public static class ObjectPool
    {
        /// <summary>
        /// Gets or sets the object pool implementation to use.
        /// </summary>
        public static DefaultObjectPool? Instance { get; set; } = new DefaultObjectPool(100);

        /// <summary>
        /// Attempts to get an instance of <typeparamref name="T"/> from the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>([NotNullWhen(true)] out T? value)
        {
            value = default;

            return Instance?.TryGet(out value) == true;
        }

        /// <summary>
        /// Returns the given item to the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return<T>(T item)
        {
            Instance?.Return(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? GetCount<T>(T item)
        {
            return Instance?.GetCount<T>();
        }
    }

    /// <summary>
    /// A default implementation of the <see cref="IObjectPool"/> interface.
    /// </summary>
    public sealed class DefaultObjectPool : IObjectPool
    {
        private readonly int maxToRetain;

        /// <summary>
        /// Initializes a new <see cref="DefaultObjectPool"/> with the given retention.
        /// </summary>
        /// <param name="maxToRetain">The maximum number of items of each type to retain.</param>
        public DefaultObjectPool(int maxToRetain)
        {
            this.maxToRetain = maxToRetain;
        }

        public void Return<T>(T item)
        {
            Pool<T>.Return(item, this.maxToRetain);
        }

        public bool TryGet<T>([NotNullWhen(true)] out T? value)
        {
            return Pool<T>.TryGet(out value);
        }

        /// <summary>
        /// Returns the number of items in the pool for the given type.
        /// </summary>
        public int GetCount<T>()
        {
            return Pool<T>.FastCount;
        }

        private static class Pool<T>
        {
            private static readonly ConcurrentQueue<T> pool = new();

            /// <summary>
            /// ConcurrentQueue's Count property is quite slow. FastCount just uses interlocked operations.
            /// </summary>
            internal static int FastCount = 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryGet([NotNullWhen(true)] out T? item)
            {
                item = default;

                if (FastCount > 0)
                {
                    if (pool.TryDequeue(out item))
                    {
                        Interlocked.Decrement(ref FastCount);
                        Debug.Assert(item is not null);
                        return true;
                    }
                }

                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Return(T item, int limit)
            {
                if (FastCount < limit)
                {
                    pool.Enqueue(item);
                    Interlocked.Increment(ref FastCount);
                }
            }
        }
    }
}