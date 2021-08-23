/*
 * Copyright 2021 James Courtney
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
    /// An interface applied to structs deserialized by Flatsharp when using
    /// non-greedy deserialization. It should not be implemented externally.
    /// </summary>
    [Obsolete("FlatSharp version 5.7 supports value type structs. This interface is being evaluated for removal in FlatSharp version 6.")]
    public interface IFlatBufferAddressableStruct : IFlatBufferDeserializedObject
    {
        /// <summary>
        /// Gets the absolute offset of the struct within the <see cref="IFlatBufferDeserializedObject.InputBuffer"/>.
        /// </summary>
        int Offset { get; }

        /// <summary>
        /// Gets the size of the struct within the <see cref="IFlatBufferDeserializedObject.InputBuffer"/>. All items are fixed size.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the alignment of the struct within the <see cref="IFlatBufferDeserializedObject.InputBuffer"/>.
        /// </summary>
        int Alignment { get; }
    }
}