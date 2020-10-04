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
    /// Defines an interface used to detect repeated strings in an input buffer and
    /// return previously-computed instances. It is highly recommended
    /// to make implementations of ISharedStringReader threadsafe.
    /// </summary>
    public interface ISharedStringReader
    {
        /// <summary>
        /// Reads a string from the given input buffer from the given offset.
        /// May perform deduplication of shared strings and return a previously-computed value.
        /// </summary>
        SharedString ReadSharedString<TBuffer>(TBuffer buffer, int offset) where TBuffer : IInputBuffer;
    }
}
