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
    using System.ComponentModel;

    /// <summary>
    /// An interface applied to objects deserialized by FlatSharp. FlatSharp implements this
    /// interface on deserialized objects. It should not be implemented externally.
    /// </summary>
    public interface IFlatBufferDeserializedObject
    {
        /// <summary>
        /// The actual type of the table or struct. This is generally the base class.
        /// </summary>
        Type TableOrStructType { get; }

        /// <summary>
        /// The context of the deserialized object.
        /// </summary>
        FlatBufferDeserializationContext DeserializationContext { get; }

        /// <summary>
        /// Gets the input buffer instance used to lazily read this object.
        /// This buffer will not have a value when the derserialization mode 
        /// is <see cref="FlatBufferDeserializationOption.Greedy"/> or 
        /// <see cref="FlatBufferDeserializationOption.GreedyMutable"/>.
        /// </summary>
        IInputBuffer? InputBuffer { get; }

        /// <summary>
        /// Formally releases this object and allows FlatSharp to recycle it for use in future deserialization operations. 
        /// This method should generally only be invoked from FlatSharp-generated code, though special high-performance 
        /// use cases may exist where it is useful. Improper use of this method may lead to unexpected results and invalid state.
        /// This method is invoked automatically via <see cref="ISerializer{T}.Recycle(ref T?)"/>, which traverses the full object
        /// graph and releases everything.
        /// </summary>
        /// <remarks>
        /// A case where this method may be valuable is when using <see cref="FlatBufferDeserializationOption.Lazy"/> deserialization.
        /// </remarks>
        void DangerousRelease();
    }
}
