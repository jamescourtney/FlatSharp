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
            return // ObjectPool.Instance is not null && 
                (force || option == FlatBufferDeserializationOption.Lazy);
        }
    }
}

namespace FlatSharp
{
    /// <summary>
    /// A static singleton that defines the FlatSharp object pool to use.
    /// </summary>
    public static class ObjectPool
    {

#if DEBUG
        public static IObjectPool? Instance
        {
            get; 
            set;
        }

        /// <summary>
        /// Attempts to get an instance of <typeparamref name="T"/> from the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>([NotNullWhen(true)] out T? value)
        {
            value = default;
            return Instance?.TryGet<T>(out value) == true;
        }

        /// <summary>
        /// Returns the given item to the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return<T>(T item)
        {
            Instance?.Return(item);
        }

#else

        /// <summary>
        /// Attempts to get an instance of <typeparamref name="T"/> from the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>([NotNullWhen(true)] out T? value)
        {
            return DefaultObjectPool.TryGet(out value);
        }

        /// <summary>
        /// Returns the given item to the pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return<T>(T item)
        {
            DefaultObjectPool.Return(item);
        }
#endif
    }

    /// <summary>
    /// A default implementation of the <see cref="IObjectPool"/> interface.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DefaultObjectPool
    {
        public static int MaxToRetain
        {
            get;
            set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return<T>(T item)
        {
            Pool<T>.Return(item, MaxToRetain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>([NotNullWhen(true)] out T? value)
        {
            return Pool<T>.TryGet(out value);
        }

        private static class Pool<T>
        {
            private static readonly ConcurrentQueue<T> pool = new();

            /// <summary>
            /// ConcurrentQueue's Count property is quite slow. FastCount just uses interlocked operations.
            /// </summary>
            private static int FastCount = 0;

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