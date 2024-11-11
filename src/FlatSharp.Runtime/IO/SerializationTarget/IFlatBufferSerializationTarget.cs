/*
 * Copyright 2024 James Courtney
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

namespace FlatSharp;

/// <summary>
/// Represents a target for a FlatBuffer serialization operation.
/// </summary>
public interface IFlatBufferSerializationTarget<out T>
    where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{
    /// <summary>
    /// Gets or sets the value at the given index.
    /// </summary>
    byte this[long index] { get; set; }
    
    /// <summary>
    /// Gets the length.
    /// </summary>
    long Length { get; }

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start, long length);

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start);

    /// <summary>
    /// Returns a Span{byte} over the given range.
    /// </summary>
    Span<byte> AsSpan(long start, int length);
}