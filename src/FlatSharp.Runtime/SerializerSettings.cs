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
/// Settings that can be applied to an <see cref="ISerializer{T}"/> instance.
/// </summary>
public class SerializerSettings
{
    internal SerializerSettings()
    {
    }

    internal FlatBufferDeserializationOption? DeserializationMode { get; set; }

    internal Func<ISharedStringWriter?>? SharedStringWriterFactory { get; set; }

    internal bool? EnableMemoryCopySerialization { get; set; }

    internal short? ObjectDepthLimit { get; set; }

    /// <summary>
    /// Configures the serializer to use <see cref="FlatBufferDeserializationOption.Lazy"/> serialization by default.
    /// </summary>
    public SerializerSettings UseLazyDeserialization() => this.UseDeserializationMode(FlatBufferDeserializationOption.Lazy);

    /// <summary>
    /// Configures the serializer to use <see cref="FlatBufferDeserializationOption.Progressive"/> serialization by default.
    /// </summary>
    public SerializerSettings UseProgressiveDeserialization() => this.UseDeserializationMode(FlatBufferDeserializationOption.Progressive);

    /// <summary>
    /// Configures the serializer to use <see cref="FlatBufferDeserializationOption.Greedy"/> serialization by default.
    /// </summary>
    public SerializerSettings UseGreedyDeserialization() => this.UseDeserializationMode(FlatBufferDeserializationOption.Greedy);

    /// <summary>
    /// Configures the serializer to use <see cref="FlatBufferDeserializationOption.GreedyMutable"/> serialization by default.
    /// </summary>
    public SerializerSettings UseGreedyMutableDeserialization() => this.UseDeserializationMode(FlatBufferDeserializationOption.GreedyMutable);

    /// <summary>
    /// Configures the serializer to use the provided deserialization mode by default.
    /// </summary>
    public SerializerSettings UseDeserializationMode(FlatBufferDeserializationOption option)
    {
        this.DeserializationMode = option;
        return this;
    }

    /// <summary>
    /// Configures the serializer to use the given factory for creating shared string writers.
    /// </summary>
    /// <param name="factory">
    /// A factory delegate that produces <see cref="ISharedStringWriter"/> instances. The given delegate
    /// must produce a new, unique <see cref="ISharedStringWriter"/> each time it is invoked.
    /// </param>
    public SerializerSettings UseSharedStringWriter(Func<ISharedStringWriter> factory)
    {
        this.SharedStringWriterFactory = factory;
        return this;
    }

    /// <summary>
    /// Configures the serializer to use the given type for creating shared string writers.
    /// </summary>
    public SerializerSettings UseSharedStringWriter<T>() where T : ISharedStringWriter, new()
    {
        return this.UseSharedStringWriter(() => new T());
    }

    /// <summary>
    /// Configures the serializer to use the default shared string writer with, optionally, the given hash table capacity.
    /// </summary>
    public SerializerSettings UseDefaultSharedStringWriter(int? hashTableCapacity = null)
    {
        this.SharedStringWriterFactory = () => new SharedStringWriter(hashTableCapacity);
        return this;
    }

    /// <summary>
    /// Configures the serializer to not use shared strings.
    /// </summary>
    public SerializerSettings DisableSharedStrings()
    {
        this.SharedStringWriterFactory = () => null;
        return this;
    }

    /// <summary>
    /// Memory copy serialization is a performance optimization that allows serialization of a deserialized object
    /// to be implemented as a memory copy operation. This feature is experimental and may be removed in
    /// future releases of FlatSharp.
    /// </summary>
    public SerializerSettings UseMemoryCopySerialization(bool enabled = true)
    {
        this.EnableMemoryCopySerialization = enabled;
        return this;
    }

    /// <summary>
    /// When set, specifies a depth limit for nested objects. Enforced at deserialization time.
    /// If set to <c>null</c>, a default value of <c>1000</c> will be used. This setting may be used to prevent
    /// stack overflow errors and otherwise guard against malicious inputs.
    /// </summary>
    public SerializerSettings WithObjectDepthLimit(short depthLimit = 1000)
    {
        if (depthLimit <= 0)
        {
            FSThrow.Argument("ObjectDepthLimit must be nonnegative.");
        }

        this.ObjectDepthLimit = depthLimit;
        return this;
    }
}
