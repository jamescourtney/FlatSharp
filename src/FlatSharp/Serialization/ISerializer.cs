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
    /// Interface that can write <typeparamref name="TRoot"/> to a buffer.
    /// </summary>
    internal interface ISerializer<TRoot>
    {
        /// <summary>
        /// Gets the raw assembly data for this serializer. Can be saved to a file and inspected/decompiled.
        /// </summary>
        byte[] AssemblyData { get; set; }

        /// <summary>
        /// Gets the raw C# code used to create this serializer.
        /// </summary>
        string CSharp { get; set; }

        /// <summary>
        /// Writes the given item to the buffer using the given spanwriter.
        /// </summary>
        void Write(
            SpanWriter writer,
            Span<byte> destination,
            TRoot item,
            int offset,
            SerializationContext context);

        /// <summary>
        /// Computes the maximum size necessary to serialize the given instance of TRoot.
        /// </summary>
        int GetMaxSize(TRoot item);

        /// <summary>
        /// Parses the given buffer as an instance of TRoot from the given offset.
        /// </summary>
        TRoot Parse(InputBuffer buffer, int offset);
    }
}
