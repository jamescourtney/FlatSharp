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

namespace FlatSharp;

/// <summary>
/// Defines a buffer that FlatSharp can parse from. Implementations will be fastest when using a struct.
/// </summary>
public interface IInputBuffer
{
    /// <summary>
    /// Indicates if this instance is read only.
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Indicates if this instance represents pinned (non-movable) memory.
    /// </summary>
    bool IsPinned { get; }

    /// <summary>
    /// Gets the length of this input buffer.
    /// </summary>
    long Length { get; }
    
    /// <summary>
    /// Gets a read only span covering the entire input buffer.
    /// </summary>
    ReadOnlySpan<byte> GetReadOnlySpan(long offset, int length);

    /// <summary>
    /// Gets a read only memory covering the entire input buffer.
    /// </summary>
    ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length);

    /// <summary>
    /// Gets a span covering the entire input buffer.
    /// </summary>
    Span<byte> GetSpan(long offset, int length);

    /// <summary>
    /// Gets a memory covering the entire input buffer.
    /// </summary>
    Memory<byte> GetMemory(long offset, int length);
}
