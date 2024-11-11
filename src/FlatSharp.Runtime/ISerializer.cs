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

namespace FlatSharp;

/// <summary>
/// An object that can read and write a FlatBuffer table from a buffer. The type
/// of table this ISerializer supports is indicated by the <see cref="RootType"/> property.
/// The use of other types may cause undefined behavior.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Gets the root type for this serializer.
    /// </summary>
    Type RootType { get; }

    /// <summary>
    /// Gets the default DeserializationOption for this serializer.
    /// </summary>
    FlatBufferDeserializationOption DeserializationOption { get; }

    /// <summary>
    /// Writes the given item to the buffer using the given spanwriter.
    /// </summary>
    /// <param name="target">The destination.</param>
    /// <param name="item">The object to serialize.</param>
    /// <returns>The number of bytes written.</returns>
    long Write<TBuffer>(TBuffer target, object item)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    ;

    /// <summary>
    /// Computes the maximum size necessary to serialize the given instance.
    /// </summary>
    long GetMaxSize(object item);

    /// <summary>
    /// Parses the given buffer as an instance of this ISerializer's type.
    /// </summary>
    /// <param name="buffer">The input buffer.</param>
    /// <param name="option">The deserialization option. If <c>null</c>, the serializer's default setting is used.</param>
    object Parse<TInputBuffer>(TInputBuffer buffer, FlatBufferDeserializationOption? option = null) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// Returns a new <see cref="ISerializer"/> instance based on the current one with the given settings.
    /// </summary>
    ISerializer WithSettings(Action<SerializerSettings> settingsCallback);
}

/// <summary>
/// An object that can read and write <typeparamref name="T"/> from a buffer.
/// </summary>
public interface ISerializer<T>
    where T : class
{
    /// <summary>
    /// Gets the default DeserializationOption for this serializer.
    /// </summary>
    FlatBufferDeserializationOption DeserializationOption { get; }

    /// <summary>
    /// Writes the given item to the buffer using the given spanwriter.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="item">The object to serialize.</param>
    /// <returns>The number of bytes written.</returns>
    long Write<TBuffer>(TBuffer destination, T item)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    ;

    /// <summary>
    /// Computes the exact size necessary to serialize the given instance of <typeparamref name="T"/>.
    /// </summary>
    long GetActualSize(T item);

    /// <summary>
    /// Computes the maximum size necessary to serialize the given instance of <typeparamref name="T"/>.
    /// </summary>
    long GetMaxSize(T item);

    /// <summary>
    /// Parses the given buffer as an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="buffer">The input buffer.</param>
    /// <param name="option">The deserialization option. If <c>null</c>, the serializer's default setting is used.</param>
    T Parse<TInputBuffer>(TInputBuffer buffer, FlatBufferDeserializationOption? option = null) where TInputBuffer : IInputBuffer;

    /// <summary>
    /// Returns a new <see cref="ISerializer{T}"/> instance based on the current one with the given settings.
    /// </summary>
    ISerializer<T> WithSettings(Action<SerializerSettings> settingsCallback);
}
