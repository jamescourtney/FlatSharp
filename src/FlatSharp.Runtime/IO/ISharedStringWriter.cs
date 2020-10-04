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
    /// Defines an interface capable of delay writing strings to a SpanWriter. Can be used to
    /// reduce the size of a FlatBuffer binary payload when there are many duplicate strings.
    /// Implementations of ISharedStringWriter need not be threadsafe.
    /// </summary>
    public interface ISharedStringWriter
    {
        /// <summary>
        /// Invoked before a new write operation begins.
        /// </summary>
        void PrepareWrite();

        /// <summary>
        /// Writes the given string to the span.
        /// </summary>
        /// <param name="spanWriter">The spanwriter.</param>
        /// <param name="data">The span.</param>
        /// <param name="offset">The location in the buffer of the uoffset to the string.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="context">The serialization context.</param>
        void WriteSharedString<TSpanWriter>(TSpanWriter spanWriter, Span<byte> data, int offset, SharedString value, SerializationContext context)
            where TSpanWriter : ISpanWriter;

        /// <summary>
        /// Flushes any pending writes. Invoked at the end of a serialization operation.
        /// </summary>
        void FlushWrites<TSpanWriter>(TSpanWriter writer, Span<byte> data, SerializationContext context)
            where TSpanWriter : ISpanWriter;
    }
}
