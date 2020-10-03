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

    /// <summary>
    /// An interface implemented dynamically by FlatSharp for reading and writing data from a buffer.
    /// </summary>
    public interface IGeneratedSerializer<T>
    {
        /// <summary>
        /// Writes the given item to the buffer using the given spanwriter.
        /// </summary>
        /// <param name="writer">The span writer.</param>
        /// <param name="destination">The span to write to.</param>
        /// <param name="item">The object to serialize.</param>
        /// <param name="offset">The offset to begin writing at.</param>
        /// <param name="context">The serialization context.</param>
        void Write<TSpanWriter>(
            TSpanWriter writer,
            Span<byte> destination,
            T item,
            int offset,
            SerializationContext context) where TSpanWriter : ISpanWriter;

        /// <summary>
        /// Computes the maximum size necessary to serialize the given instance of <typeparamref name="T"/>.
        /// </summary>
        int GetMaxSize(T item);

        /// <summary>
        /// Parses the given buffer as an instance of <typeparamref name="T"/> from the given offset.
        /// </summary>
        T Parse<TInputBuffer>(TInputBuffer buffer, int offset) where TInputBuffer : IInputBuffer;
    }
}
