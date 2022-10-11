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

using System.Buffers;

namespace FlatSharp;

/// <summary>
/// Serializer extension methods.
/// </summary>
public static class ISerializerExtensions
{
    /// <summary>
    /// Wraps the given generated serializer as an ISerializer.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static ISerializer<T> AsISerializer<T>(this IGeneratedSerializer<T> generatedSerializer, FlatBufferDeserializationOption option) where T : class
    {
        return new GeneratedSerializerWrapper<T>(option, generatedSerializer, null, () => null, null);
    }

    /// <summary>
    /// Parses the given byte array as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static T Parse<T>(this ISerializer<T> serializer, byte[] data) where T : class
    {
        return serializer.Parse(new ArrayInputBuffer(data));
    }

    /// <summary>
    /// Parses the given byte array.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static object Parse(this ISerializer serializer, byte[] data)
    {
        return serializer.Parse(new ArrayInputBuffer(data));
    }

    /// <summary>
    /// Parses the given ArraySegment as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static T Parse<T>(this ISerializer<T> serializer, ArraySegment<byte> data) where T : class
    {
        return serializer.Parse(new ArraySegmentInputBuffer(data));
    }

    /// <summary>
    /// Parses the given ArraySegment as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static object Parse(this ISerializer serializer, ArraySegment<byte> data)
    {
        return serializer.Parse(new ArraySegmentInputBuffer(data));
    }

    /// <summary>
    /// Parses the given Memory as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static T Parse<T>(this ISerializer<T> serializer, Memory<byte> data) where T : class
    {
        return serializer.Parse(new MemoryInputBuffer(data));
    }

    /// <summary>
    /// Parses the given Memory as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    public static object Parse(this ISerializer serializer, Memory<byte> data)
    {
        return serializer.Parse(new MemoryInputBuffer(data));
    }

    /// <summary>
    /// Parses the given ReadOnlyMemory as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static T Parse<T>(this ISerializer<T> serializer, ReadOnlyMemory<byte> data) where T : class
    {
        return serializer.Parse(new ReadOnlyMemoryInputBuffer(data));
    }

    /// <summary>
    /// Parses the given ReadOnlyMemory as an instance of T.
    /// </summary>
    /// <returns>The parsed instance.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static object Parse(this ISerializer serializer, ReadOnlyMemory<byte> data)
    {
        return serializer.Parse(new ReadOnlyMemoryInputBuffer(data));
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write<T>(this ISerializer<T> serializer, byte[] buffer, T item) where T : class
    {
        return Write(serializer, buffer.AsSpan(), item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write(this ISerializer serializer, byte[] buffer, object item)
    {
        return Write(serializer, buffer.AsSpan(), item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write<T>(this ISerializer<T> serializer, ArraySegment<byte> buffer, T item) where T : class
    {
        return Write(serializer, buffer.AsSpan(), item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write(this ISerializer serializer, ArraySegment<byte> buffer, object item)
    {
        return Write(serializer, buffer.AsSpan(), item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write<T>(this ISerializer<T> serializer, Memory<byte> buffer, T item) where T : class
    {
        return Write(serializer, buffer.Span, item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write(this ISerializer serializer, Memory<byte> buffer, object item)
    {
        return Write(serializer, buffer.Span, item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write<T>(this ISerializer<T> serializer, Span<byte> buffer, T item) where T : class
    {
        return serializer.Write(default(SpanWriter), buffer, item);
    }

    /// <summary>
    /// Writes the given item to the given buffer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    [ExcludeFromCodeCoverage] // Just a helper
    public static int Write(this ISerializer serializer, Span<byte> buffer, object item)
    {
        return serializer.Write(default(SpanWriter), buffer, item);
    }

    /// <summary>
    /// Writes the given item into the given buffer writer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    public static int Write<T>(this ISerializer<T> serializer, IBufferWriter<byte> bufferWriter, T item) where T : class
    {
        int maxSize = serializer.GetMaxSize(item);
        Span<byte> buffer = bufferWriter.GetSpan(maxSize);
        int bytesWritten = serializer.Write(default(SpanWriter), buffer, item);
        bufferWriter.Advance(bytesWritten);

        return bytesWritten;
    }

    /// <summary>
    /// Writes the given item into the given buffer writer using the default SpanWriter.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    public static int Write(this ISerializer serializer, IBufferWriter<byte> bufferWriter, object item)
    {
        int maxSize = serializer.GetMaxSize(item);
        Span<byte> buffer = bufferWriter.GetSpan(maxSize);
        int bytesWritten = serializer.Write(default(SpanWriter), buffer, item);
        bufferWriter.Advance(bytesWritten);

        return bytesWritten;
    }
}
