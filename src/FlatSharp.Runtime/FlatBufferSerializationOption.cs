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
    /// <summary>
    /// Defines FlatSharp serializer options.
    /// </summary>
    public enum FlatBufferDeserializationOption
    {
        /// <summary>
        /// Full Lazy parsing. Deserialized objects are immutable.
        /// </summary>
        Lazy = 0,

        /// <summary>
        /// Properties are cached as they are read. Deserialized objects are immutable.
        /// </summary>
        PropertyCache = 1,

        /// <summary>
        /// Indicates if list vectors should have their data cached after reading. This option will cause more allocations
        /// on deserializing, but will improve performance in cases of duplicate accesses to the same indices.
        /// </summary>
        VectorCache = 2,

        /// <summary>
        /// Same properties as <see cref="VectorCache"/>, but deserialized objects are mutable.
        /// </summary>
        VectorCacheMutable = 3,

        /// <summary>
        /// The entire object graph is traversed and the deserialized objects do not reference the input buffer. Deserialized objects are immutable.
        /// </summary>
        Greedy = 4,

        /// <summary>
        /// Same properties as <see cref="GreedyMutable"/>, but deserialized objects are mutable.
        /// </summary>
        GreedyMutable = 5,

        /// <summary>
        /// Default options.
        /// </summary>
        Default = GreedyMutable,
    }
}
