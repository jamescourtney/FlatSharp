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
    /// Serializer extension methods.
    /// </summary>
    public static class ISerializerExtensions
    {
        /// <summary>
        /// Wraps the given generated serializer as an ISerializer.
        /// </summary>
        public static ISerializer<T> AsISerializer<T>(this IGeneratedSerializer<T> generatedSerializer) where T : class
        {
            return new GeneratedSerializerWrapper<T>(generatedSerializer, null, () => null, null);
        }

        /// <summary>
        /// Parses the given byte array as an instance of T.
        /// </summary>
        /// <returns>The parsed instance.</returns>
        public static T Parse<T>(this ISerializer<T> serializer, byte[] data) where T : class
        {
            return serializer.Parse(new ArrayInputBuffer(data));
        }

        /// <summary>
        /// Parses the given ArraySegment as an instance of T.
        /// </summary>
        /// <returns>The parsed instance.</returns>
        public static T Parse<T>(this ISerializer<T> serializer, ArraySegment<byte> data) where T : class
        {
            return serializer.Parse(new ArrayInputBuffer(data));
        }

        /// <summary>
        /// Parses the given Memory as an instance of T.
        /// </summary>
        /// <returns>The parsed instance.</returns>
        public static T Parse<T>(this ISerializer<T> serializer, Memory<byte> data) where T : class
        {
            return serializer.Parse(new MemoryInputBuffer(data));
        }

        /// <summary>
        /// Parses the given ReadOnlyMemory as an instance of T.
        /// </summary>
        /// <returns>The parsed instance.</returns>
        public static T Parse<T>(this ISerializer<T> serializer, ReadOnlyMemory<byte> data) where T : class
        {
            return serializer.Parse(new ReadOnlyMemoryInputBuffer(data));
        }

        /// <summary>
        /// Writes the given item to the given buffer using the default SpanWriter.
        /// </summary>
        /// <returns>The number of bytes written.</returns>
        public static int Write<T>(this ISerializer<T> serializer, byte[] buffer, T item) where T : class
        {
            return Write(serializer, buffer.AsSpan(), item);
        }

        /// <summary>
        /// Writes the given item to the given buffer using the default SpanWriter.
        /// </summary>
        /// <returns>The number of bytes written.</returns>
        public static int Write<T>(this ISerializer<T> serializer, ArraySegment<byte> buffer, T item) where T : class
        {
            return Write(serializer, buffer.AsSpan(), item);
        }

        /// <summary>
        /// Writes the given item to the given buffer using the default SpanWriter.
        /// </summary>
        /// <returns>The number of bytes written.</returns>
        public static int Write<T>(this ISerializer<T> serializer, Memory<byte> buffer, T item) where T : class
        {
            return Write(serializer, buffer.Span, item);
        }

        /// <summary>
        /// Writes the given item to the given buffer using the default SpanWriter.
        /// </summary>
        /// <returns>The number of bytes written.</returns>
        public static int Write<T>(this ISerializer<T> serializer, Span<byte> buffer, T item) where T : class
        {
            return serializer.Write(SpanWriter.Instance, buffer, item);
        }
    }
}
