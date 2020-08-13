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
    /// <summary>
    /// A pseudo LRU string reader that uses a flat array that maps absolute
    /// string offset to parsed value.
    /// </summary>
    public class PseudoLruSharedStringReader : ISharedStringReader
    {
        private readonly CacheEntry[] sharedStringCache;

        public PseudoLruSharedStringReader(int cacheSize)
        {
            this.sharedStringCache = new CacheEntry[cacheSize];
        }

        public SharedString ReadSharedString(InputBuffer buffer, int offset)
        {
            var cache = this.sharedStringCache;

            // Unlike spanwriter, which sits inside a synchronous method,
            // InputBuffer may be used concurrently on the same buffer.
            // Coarse-grained locking isn't great on a critical path, but adding fine-grained
            // locking adds quite a bit of initialization time and more time
            // to try to grab the lock on the bucket.
            // TODO: consider benchmarking effects of TryEnter
            // where we just reparse the string if we fail to immediately
            // enter the lock.
            lock (cache)
            {
                // uoffset guaranteed to be aligned to a 4 byte boundary, so we can easily 
                // divide by 4 as a quick and dirty hash.
                ref CacheEntry cacheItem = ref cache[(offset >> 2) % cache.Length];
                ref int cacheOffset = ref cacheItem.Offset;

                if (cacheOffset == offset)
                {
                    return cacheItem.String;
                }

                SharedString readValue = buffer.ReadStringFromUOffset(offset);
                cacheOffset = offset;
                cacheItem.String = readValue;
                return readValue;
            }
        }

        private struct CacheEntry
        {
            public SharedString String;

            public int Offset;
        }
    }
}
