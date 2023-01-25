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

namespace FlatSharp.Internal;

/// <summary>
/// An interface implemented on buffer-backed Flatbuffer vectors. This interface is internal to FlatSharp and exposes some
/// functionality to assist with binary searching.
/// </summary>
public interface IFlatBufferDeserializedVector
{
    /// <summary>
    /// Gets the input buffer.
    /// </summary>
    IInputBuffer InputBuffer { get; }

    /// <summary>
    /// Gets the raw item size of each element in the vector.
    /// </summary>
    int ItemSize { get; }

    /// <summary>
    /// The number of items.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Returns the absolute position in the Input Buffer of the given index in the vector.
    /// </summary>
    int OffsetOf(int index);

    /// <summary>
    /// Gets the item at the given index.
    /// </summary>
    object ItemAt(int index);
}