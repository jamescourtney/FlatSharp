/*
 * Copyright 2020 James Courtney
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

namespace FlatSharp
{
    using System;

    /// <summary>
    /// A shared string reader implemented using a direct-mapped cache.
    /// </summary>
    public class SharedStringReader : ISharedStringReader
    {
        private const int DefaultSize = 257;

        private readonly CacheEntry[] sharedStringCache;
        private readonly bool threadSafe;

        /// <summary>
        /// Initializes a new shared string reader with the given direct map cache capacity.
        /// </summary>
        /// <param name="hashTableCapacity">The size of the hash table.</param>
        /// <param name="threadSafe">If the string reader should be threadsafe.</param>
        private SharedStringReader(int hashTableCapacity, bool threadSafe = true)
        {
            if (hashTableCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hashTableCapacity));
            }

            this.sharedStringCache = new CacheEntry[hashTableCapacity];
            this.threadSafe = threadSafe;
        }

        /// <summary>
        /// Initializes a new shared string reader with the given direct map cache capacity.
        /// The returned instance is safe to use concurrently from multiple threads.
        /// </summary>
        /// <param name="hashTableCapacity">The size of the hash table.</param>
        public static SharedStringReader CreateThreadSafe(int hashTableCapacity = DefaultSize)
        {
            return new SharedStringReader(hashTableCapacity, threadSafe: true);
        }

        /// <summary>
        /// Initializes a new shared string reader with the given direct map cache capacity.
        /// The returned instance is not safe to use concurrently from multiple threads.
        /// Note that objects deserialized with an <see cref="ISerializer{T}"/> when used with
        /// <see cref="SharedStringReader"/> in non-threadsafe mode are also not threadsafe.
        /// </summary>
        /// <param name="hashTableCapacity">The size of the hash table.</param>
        public static SharedStringReader Create(int hashTableCapacity = DefaultSize)
        {
            return new SharedStringReader(hashTableCapacity, threadSafe: false);
        }

        public SharedString ReadSharedString<TInputBuffer>(TInputBuffer buffer, int offset) where TInputBuffer : IInputBuffer
        {
            var cache = this.sharedStringCache;

            if (this.threadSafe)
            {
                lock (cache)
                {
                    return GetSharedStringPrivate(cache, buffer, offset);
                }
            }
            else
            {
                return GetSharedStringPrivate(cache, buffer, offset);
            }
        }

        private static SharedString GetSharedStringPrivate<TInputBuffer>(CacheEntry[] cache, TInputBuffer buffer, int offset) where TInputBuffer : IInputBuffer
        {
            // uoffset guaranteed to be aligned to a 4 byte boundary, so we can easily 
            // divide by 4 as a quick and dirty hash.
            ref CacheEntry cacheEntry = ref cache[(int.MaxValue & (offset >> 2)) % cache.Length];
            ref int entryOffset = ref cacheEntry.Offset;

            if (entryOffset == offset)
            {
                return cacheEntry.String;
            }

            SharedString sharedString = buffer.ReadStringFromUOffset(offset);
            cacheEntry.String = sharedString;
            entryOffset = offset;

            return sharedString;
        }

        private struct CacheEntry
        {
            public SharedString String;

            public int Offset;
        }
    }
}
