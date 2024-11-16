/*
 * Copyright 2022 James Courtney
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
/// Wrapper struct to pass arguments into Parse methods on <see cref="IGeneratedSerializer{T}"/>.
/// </summary>
public readonly struct GeneratedSerializerParseArguments
{
    public GeneratedSerializerParseArguments(int offset, short depthLimit)
    {
        this.Offset = offset;
        this.DepthLimit = depthLimit;
    }

    public int Offset { get; }

    public short DepthLimit { get; }
}

/// <summary>
/// An interface implemented dynamically by FlatSharp for reading and writing data from a buffer.
/// </summary>
public interface IGeneratedSerializer<T>
{
    /// <summary>
    /// Writes the given item to the given target.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="item">The item.</param>
    /// <param name="context">The serialization context.</param>
    void Write(
        BigSpan target,
        T item,
        SerializationContext context);

    /// <summary>
    /// Computes the maximum size necessary to serialize the given instance of <typeparamref name="T"/>.
    /// </summary>
    int GetMaxSize(T item);

    /// <summary>
    /// Parses the given buffer as an instance of <typeparamref name="T"/> from the given offset.
    /// </summary>
    T ParseLazy<TInputBuffer>(
        TInputBuffer buffer,
        in GeneratedSerializerParseArguments arguments) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// Parses the given buffer as an instance of <typeparamref name="T"/> from the given offset.
    /// </summary>
    T ParseProgressive<TInputBuffer>(
        TInputBuffer buffer,
        in GeneratedSerializerParseArguments arguments) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// Parses the given buffer as an instance of <typeparamref name="T"/> from the given offset.
    /// </summary>
    T ParseGreedy<TInputBuffer>(
        TInputBuffer buffer,
        in GeneratedSerializerParseArguments arguments) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// Parses the given buffer as an instance of <typeparamref name="T"/> from the given offset.
    /// </summary>
    T ParseGreedyMutable<TInputBuffer>(
        TInputBuffer buffer,
        in GeneratedSerializerParseArguments arguments) where TInputBuffer : IInputBuffer;
}
