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

using System.Threading;

namespace FlatSharp;

/// <summary>
/// An implementation of <see cref="ISerializer{T}"/> that wraps a <see cref="IGeneratedSerializer{T}"/>.
/// </summary>
internal class GeneratedSerializerWrapper<T> : ISerializer<T>, ISerializer where T : class
{
    private const int FileIdentifierSize = 4;

    private readonly IGeneratedSerializer<T> innerSerializer;
    private readonly Lazy<string?> lazyCSharp;

    private ThreadLocal<ISharedStringWriter?> sharedStringWriter;
    private bool enableMemoryCopySerialization;
    private short remainingDepthLimit;
    private FlatBufferDeserializationOption option;

    public GeneratedSerializerWrapper(
        FlatBufferDeserializationOption option,
        IGeneratedSerializer<T>? innerSerializer,
        Func<string?> generatedCSharp)
    {
        this.lazyCSharp = new Lazy<string?>(generatedCSharp);
        this.innerSerializer = innerSerializer ?? FSThrow.ArgumentNull<IGeneratedSerializer<T>>(nameof(innerSerializer));

        this.sharedStringWriter = new ThreadLocal<ISharedStringWriter?>(() => new SharedStringWriter());
        this.remainingDepthLimit = 1000; // sane default.
        this.option = option;
    }

    private GeneratedSerializerWrapper(GeneratedSerializerWrapper<T> template)
    {
        this.innerSerializer = template.innerSerializer;
        this.remainingDepthLimit = template.remainingDepthLimit;
        this.option = template.option;
        this.sharedStringWriter = template.sharedStringWriter;
        this.enableMemoryCopySerialization = template.enableMemoryCopySerialization;
        this.lazyCSharp = template.lazyCSharp;
    }

    public Type RootType => typeof(T);

    public FlatBufferDeserializationOption DeserializationOption => this.option;

    public int GetMaxSize(T item)
    {
        if (item is null)
        {
            FSThrow.ArgumentNull(nameof(item), "The root table may not be null.");
        }

        if (this.enableMemoryCopySerialization &&
            item is IFlatBufferDeserializedObject deserializedObj &&
            deserializedObj.CanSerializeWithMemoryCopy)
        {
            IInputBuffer? inputBuffer = deserializedObj.InputBuffer;
            FlatSharpInternal.Assert(inputBuffer is not null, "Input buffer was null");
            return inputBuffer.Length;
        }

        return sizeof(uint)                                      // uoffset to first table
             + SerializationHelpers.GetMaxPadding(sizeof(uint))  // alignment error
             + this.innerSerializer.GetMaxSize(item)             // size of item
             + FileIdentifierSize;                               // file identifier. Not present on every table, but cheaper to add as constant
                                                                 // than to introduce an 'if'.
    }

    int ISerializer.GetMaxSize(object item)
    {

        return item switch
        {
            T t => this.GetMaxSize(t),
            null => FSThrow.ArgumentNull<int>(nameof(item)),
            _ => FSThrow.Argument<int>($"Argument was not of the correct type. Type = {item.GetType().FullName}, Expected Type = {typeof(T).FullName}"),
        };
    }

    public T Parse<TInputBuffer>(TInputBuffer buffer) where TInputBuffer : IInputBuffer
    {
        return this.Parse(buffer, this.option);
    }

    public T Parse<TInputBuffer>(TInputBuffer buffer, FlatBufferDeserializationOption? option = null) where TInputBuffer : IInputBuffer
    {
        if (buffer.Length >= int.MaxValue / 2)
        {
            FSThrow.Argument("Buffer must be <= 1GB in size.");
        }

        if (buffer.Length <= 2 * sizeof(uint))
        {
            FSThrow.Argument("Buffer is too small to be valid!");
        }

        var parseArgs = new GeneratedSerializerParseArguments(0, this.remainingDepthLimit);
        var inner = this.innerSerializer;

        T item;

        switch (option ?? this.option)
        {
            case FlatBufferDeserializationOption.Lazy:
                item = inner.ParseLazy(buffer, in parseArgs);
                break;

            case FlatBufferDeserializationOption.Greedy:
                item = inner.ParseGreedy(buffer, in parseArgs);
                break;

            case FlatBufferDeserializationOption.GreedyMutable:
                item = inner.ParseGreedyMutable(buffer, in parseArgs);
                break;

            case FlatBufferDeserializationOption.Progressive:
                item = inner.ParseProgressive(buffer, in parseArgs);
                break;

            default:
                item = FSThrow.InvalidOperation<T>("Unexpected deserialization mode");
                break;
        }

        if (item is IPoolableObjectDebug deserializedObject)
        {
            deserializedObject.IsRoot = true;
        }

        return item;
    }

    object ISerializer.Parse<TInputBuffer>(TInputBuffer buffer, FlatBufferDeserializationOption? option) => this.Parse(buffer, option);

    public int Write<TSpanWriter>(TSpanWriter writer, Span<byte> destination, T item)
        where TSpanWriter : ISpanWriter
    {
        if (item is null)
        {
            FSThrow.ArgumentNull(nameof(item), "The root table may not be null.");
        }

        if (destination.Length <= 8)
        {
            FSThrow.BufferTooSmall(this.GetMaxSize(item));
        }

        if (this.enableMemoryCopySerialization &&
            item is IFlatBufferDeserializedObject deserializedObj &&
            deserializedObj.CanSerializeWithMemoryCopy)
        {
            IInputBuffer? inputBuffer = deserializedObj.InputBuffer;
            FlatSharpInternal.Assert(inputBuffer is not null, "Input buffer was null");

            if (destination.Length < inputBuffer.Length)
            {
                FSThrow.BufferTooSmall(inputBuffer.Length);
            }

            inputBuffer.GetReadOnlySpan().CopyTo(destination);
            return inputBuffer.Length;
        }

        var serializationContext = SerializationContext.ThreadLocalContext.Value!;
        serializationContext.Reset(destination.Length);

        ISharedStringWriter? sharedStringWriter = this.sharedStringWriter.Value;
        serializationContext.SharedStringWriter = sharedStringWriter;

        try
        {
            if (sharedStringWriter?.IsDirty == true)
            {
                sharedStringWriter.Reset();
                Debug.Assert(!sharedStringWriter.IsDirty);
            }

            this.innerSerializer.Write(writer, destination, item, serializationContext);

            if (sharedStringWriter?.IsDirty == true)
            {
                writer.FlushSharedStrings(sharedStringWriter, destination, serializationContext);
                Debug.Assert(!sharedStringWriter.IsDirty);
            }

            serializationContext.InvokePostSerializeActions(destination);
        }
        catch (BufferTooSmallException ex)
        {
            ex.SizeNeeded = this.GetMaxSize(item);
            throw;
        }

        return serializationContext.Offset;
    }

    int ISerializer.Write<TSpanWriter>(TSpanWriter writer, Span<byte> destination, object item)
    {
        return item switch
        {
            T t => this.Write(writer, destination, t),
            null => FSThrow.ArgumentNull<int>(nameof(item)),
            _ => FSThrow.Argument<int>($"Argument was not of the correct type. Type = {item.GetType().FullName}, Expected Type = {typeof(T).FullName}")
        };
    }

    [ExcludeFromCodeCoverage]
    public ISerializer<T> WithSettings(Action<SerializerSettings> settingsCallback)
    {
        return this.WithSettingsCore(settingsCallback);
    }

    [ExcludeFromCodeCoverage]
    ISerializer ISerializer.WithSettings(Action<SerializerSettings> settingsCallback)
    {
        return this.WithSettingsCore(settingsCallback);
    }

    private GeneratedSerializerWrapper<T> WithSettingsCore(Action<SerializerSettings> settingsCallback)
    {
        var settings = new SerializerSettings();
        settingsCallback(settings);

        GeneratedSerializerWrapper<T> wrapper = new GeneratedSerializerWrapper<T>(this);

        if (settings.DeserializationMode is not null)
        {
            wrapper.option = settings.DeserializationMode.Value;
        }

        if (settings.EnableMemoryCopySerialization is not null)
        {
            wrapper.enableMemoryCopySerialization = settings.EnableMemoryCopySerialization.Value;
        }

        if (settings.ObjectDepthLimit is not null)
        {
            wrapper.remainingDepthLimit = settings.ObjectDepthLimit.Value;
        }

        if (settings.SharedStringWriterFactory is not null)
        {
            wrapper.sharedStringWriter = new ThreadLocal<ISharedStringWriter?>(settings.SharedStringWriterFactory);
        }

        return wrapper;
    }
}
