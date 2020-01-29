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

using System;

namespace FlatSharp
{
    /// <summary>
    /// Defines FlatSharp serializer options.
    /// </summary>
    [Flags]
    public enum FlatBufferSerializerFlags
    {
        /// <summary>
        /// Full Lazy parsing.
        /// </summary>
        Lazy = 0,

        /// <summary>
        /// Indicates if list vectors should have their data cached after reading. This option will cause more allocations
        /// on deserializing, but will improve performance in cases of duplicate accesses to the same indices.
        /// </summary>
        CacheListVectorData = 1,

        /// <summary>
        /// Indicates if the serializer should generate mutable objects. Mutable objects are "copy-on-write"
        /// and do not modify the underlying buffer.
        /// </summary>
        GenerateMutableObjects = 2,

        /// <summary>
        /// Indicates if deserialization should be greedy.
        /// </summary>
        GreedyDeserialize = 4,

        /// <summary>
        /// Default options.
        /// </summary>
        Default = GreedyDeserialize,
    }
}
