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
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A shared string writer that uses a direct-map hash table.
    /// </summary>
    public class SharedStringWriter : ISharedStringWriter
    {
        private const int DefaultCapacity = 257;
        private readonly WriteCacheEntry[] sharedStringOffsetCache;

        /// <summary>
        /// Initializes a new shared string writer with the given capacity.
        /// </summary>
        /// <param name="hashTableCapacity">The size of the hash table.</param>
        public SharedStringWriter(int hashTableCapacity = DefaultCapacity)
        {
            if (hashTableCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hashTableCapacity));
            }

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
                entry.String = null;

                if (entry.Offsets == null)
                {
                    entry.Offsets = new List<int>();
                }

                entry.Offsets.Clear();
            }
        }

        /// <summary>
        /// Writes a shared string.
        /// </summary>
        public void WriteSharedString<TSpanWriter>(
            TSpanWriter spanWriter, 
            Span<byte> data, 
            int offset, 
            SharedString value, 
            SerializationContext context) where TSpanWriter : ISpanWriter
        {
            // Find the associative set that must contain our key.
            var cache = this.sharedStringOffsetCache;
            int lineIndex = (int.MaxValue & value.GetHashCode()) % cache.Length;
            ref WriteCacheEntry line = ref cache[lineIndex];

            if (value.Equals(line.String))
            {
                line.Offsets.Add(offset);
                return;
            }

            var offsets = line.Offsets;
            if (!object.ReferenceEquals(line.String, null))
            {
                FlushSharedString(spanWriter, data, line.String, offsets, context);
            }

            line.String = value;
            offsets.Add(offset);
        }

        /// <summary>
        /// Flush any pending writes.
        /// </summary>
        public void FlushWrites<TSpanWriter>(TSpanWriter writer, Span<byte> data, SerializationContext context) where TSpanWriter : ISpanWriter
        {
            var cache = this.sharedStringOffsetCache;
            for (int i = 0; i < cache.Length; ++i)
            {
                ref WriteCacheEntry item = ref cache[i];
                var str = item.String;

                if (!object.ReferenceEquals(str, null))
                {
                    FlushSharedString(writer, data, str, item.Offsets, context);
                    item.String = null;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FlushSharedString<TSpanWriter>(
            TSpanWriter spanWriter, 
            Span<byte> span, 
            string value, 
            List<int> offsets, 
            SerializationContext context) where TSpanWriter : ISpanWriter
        {
            int stringOffset = spanWriter.WriteAndProvisionString(span, value, context);
            int count = offsets.Count;
            for (int i = 0; i < count; ++i)
            {
                spanWriter.WriteUOffset(span, offsets[i], stringOffset, context);
            }

            offsets.Clear();
        }

        // Cache entry. Stored as struct to increase data locality in the array.
        private struct WriteCacheEntry
        {
            // The string
            public SharedString String;

            public List<int> Offsets;
        }
    }
}
