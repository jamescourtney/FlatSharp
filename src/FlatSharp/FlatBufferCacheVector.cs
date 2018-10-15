/*
 * Copyright 2018 James Courtney
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
    using System.Collections;

    /// <summary>
    /// A base class that FlatBuffersNet implements to deserialize vectors. FlatBufferCacheVector{T} is a lazy implementation
    /// that caches the results of read operations in an internal array. For this reason, read performance is as good as greedily
    /// allocating all members, but does require more memory than the zero-copy approach.
    /// </summary>
    public sealed class FlatBufferCacheVector<T> : FlatBufferVector<T>
    {
        private BitArray hasCachedValue;
        private T[] cachedValues;

        public FlatBufferCacheVector(
            InputBuffer memory,
            int offset,
            int itemSize,
            Func<InputBuffer, int, T> parseItem) : base(memory, offset, itemSize, parseItem)
        {
            int count = base.Count;
            this.hasCachedValue = new BitArray(count);
            this.cachedValues = new T[count];
        }

        /// <summary>
        /// Gets or sets the value of items in the vector.
        /// </summary>
        public override T this[int index]
        {
            get
            {
                var hasCachedValue = this.hasCachedValue;
                var cachedValues = this.cachedValues;

                if (hasCachedValue.Get(index))
                {
                    return cachedValues[index];
                }

                T item = base[index];

                cachedValues[index] = item;
                hasCachedValue[index] = true;

                return item;
            }

            set
            {
                base[index] = value;
                this.hasCachedValue[index] = true;
                this.cachedValues[index] = value;
            }
        }
    }
}
