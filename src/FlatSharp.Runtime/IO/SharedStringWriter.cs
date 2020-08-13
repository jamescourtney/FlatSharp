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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A pseudo LRU string writer that uses a flat array as a hash table.
    /// On conflict, a string is flushed to the span.
    /// </summary>
    public class SharedStringWriter : ISharedStringWriter
    {
       
        const int LINE_SIZE = 128;
        private readonly WriteCacheEntry[] sharedStringOffsetCache;

        /// <summary>
        /// Creates a new PseudoLruStringWriter with the given capacity.
        /// </summary>
        public SharedStringWriter(int hashTableCapacity)
        {
            this.sharedStringOffsetCache = new WriteCacheEntry[hashTableCapacity];
        }

        /// <summary>
        /// Resets the internal state to prepare for a new write operation.
        /// </summary>
        public void PrepareWrite()
        {
            var cache = this.sharedStringOffsetCache;
            for (int i = 0; i < cache.Length; ++i)
            {
                ref WriteCacheEntry entry = ref cache[i];
                entry.OffsetsLength = 0;
                entry.String = null;
                if (entry.Offsets == null)
                {
                    entry.Offsets = new int[LINE_SIZE];
                }
            }
        }

        /// <summary>
        /// Writes a shared string.
        /// </summary>
        public void WriteSharedString(
            SpanWriter spanWriter, 
            Span<byte> data, 
            int offset, 
            SharedString value, 
            SerializationContext context)
        {
            var cache = this.sharedStringOffsetCache;
            int index = (int.MaxValue & value.GetHashCode()) % cache.Length;

            ref WriteCacheEntry item = ref cache[index];

            ref int offsetCount = ref item.OffsetsLength;
            var offsets = item.Offsets;
            SharedString sharedString = item.String;

            // Value is guaranteed non-null. Using .Equals is slightly faster than ==
            // since it allows skipping a null check.
            if (value.Equals(sharedString))
            {
                if (offsetCount >= LINE_SIZE)
                {
                    // If we're full, then flush.
                    FlushSharedString(spanWriter, data, sharedString, offsets.AsSpan(0, offsetCount), context);
                    offsetCount = 0;
                }

                offsets[offsetCount++] = offset;
            }
            else
            {
                // The strings are not equal. So we need to evict the current one.
                if (sharedString != null)
                {
                    FlushSharedString(spanWriter, data, sharedString, offsets.AsSpan(0, offsetCount), context);
                    offsetCount = 0;
                }

                item.String = value;
                offsets[offsetCount++] = offset;
            }
        }

        /// <summary>
        /// Flush any pending writes.
        /// </summary>
        public void FlushWrites(SpanWriter writer, Span<byte> data, SerializationContext context)
        {
            var cache = this.sharedStringOffsetCache;
            for (int i = 0; i < cache.Length; ++i)
            {
                ref WriteCacheEntry item = ref cache[i];
                ref int offsetCount = ref item.OffsetsLength;

                var str = item.String;

                if (!object.ReferenceEquals(str, null))
                {
                    FlushSharedString(writer, data, str, item.Offsets.AsSpan(0, offsetCount), context);
                    item.String = null;
                }

                offsetCount = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FlushSharedString(SpanWriter spanWriter, Span<byte> span, string value, Span<int> offsets, SerializationContext context)
        {
            int stringOffset = spanWriter.WriteAndProvisionString(span, value, context);
            for (int i = 0; i < offsets.Length; ++i)
            {
                spanWriter.WriteUOffset(span, offsets[i], stringOffset, context);
            }
        }

        // Cache entry. Stored as struct to increase data locality in the array.
        private struct WriteCacheEntry
        {
            // The string
            public SharedString String;

            // Array of offsets we need to write for this string.
            public int[] Offsets;

            // The number of offsets to write, starting from 0. Allows us "grow" the list.
            public int OffsetsLength;
        }
    }
}
