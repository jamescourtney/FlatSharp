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
    private readonly ThreadLocal<ISharedStringWriter>? sharedStringWriter;
    private readonly bool enableMemoryCopySerialization;
    private readonly string? fileIdentifier;
    private readonly short remainingDepthLimit;
    private readonly FlatBufferDeserializationOption option;

    public GeneratedSerializerWrapper(
        FlatBufferDeserializationOption option,
        IGeneratedSerializer<T>? innerSerializer,
        Assembly? generatedAssembly,
        Func<string?> generatedCSharp,
        byte[]? generatedAssemblyBytes)
    {
        this.lazyCSharp = new Lazy<string?>(generatedCSharp);
        this.Assembly = generatedAssembly;
        this.AssemblyBytes = generatedAssemblyBytes;
        this.innerSerializer = innerSerializer ?? throw new ArgumentNullException(nameof(innerSerializer));

        var tableAttribute = typeof(T).GetCustomAttribute<Attributes.FlatBufferTableAttribute>();
        this.fileIdentifier = tableAttribute?.FileIdentifier;
        this.sharedStringWriter = new ThreadLocal<ISharedStringWriter>(() => new SharedStringWriter());
        this.remainingDepthLimit = 1000; // sane default.
        this.option = option;
    }

    private GeneratedSerializerWrapper(GeneratedSerializerWrapper<T> template, SerializerSettings settings)
    {
        this.lazyCSharp = template.lazyCSharp;
        this.Assembly = template.Assembly;
        this.AssemblyBytes = template.AssemblyBytes;
        this.innerSerializer = template.innerSerializer;
        this.fileIdentifier = template.fileIdentifier;
        this.remainingDepthLimit = template.remainingDepthLimit;
        this.option = template.option;
        this.sharedStringWriter = template.sharedStringWriter;
        this.enableMemoryCopySerialization = settings.EnableMemoryCopySerialization;

        Func<ISharedStringWriter>? writerFactory = settings.SharedStringWriterFactory;
        if (writerFactory is not null)
        {
            ISharedStringWriter writer = writerFactory();
            if (writer is not null)
            {
                this.sharedStringWriter = new ThreadLocal<ISharedStringWriter>(writerFactory);
            }
            else
            {
                this.sharedStringWriter = null;
            }
        }

        if (settings.ObjectDepthLimit is not null)
        {
            if (settings.ObjectDepthLimit <= 0)
            {
                throw new ArgumentException("ObjectDepthLimit must be nonnegative.");
            }

            this.remainingDepthLimit = settings.ObjectDepthLimit.Value;
        }

        if (settings.DeserializationMode is not null)
        {
            this.option = settings.DeserializationMode.Value;
        }
    }

    public Type RootType => typeof(T);

    public string? CSharp => this.lazyCSharp.Value;

    public Assembly? Assembly { get; }

    public byte[]? AssemblyBytes { get; }

    public FlatBufferDeserializationOption DeserializationOption => this.option;

    public int GetMaxSize(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "The root table may not be null.");
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
            null => throw new ArgumentNullException(nameof(item)),
            _ => throw new ArgumentException($"Argument was not of the correct type. Type = {item.GetType().FullName}, Expected Type = {typeof(T).FullName}")
        };
    }

    public T Parse<TInputBuffer>(TInputBuffer buffer) where TInputBuffer : IInputBuffer
    {
        if (buffer.Length >= int.MaxValue / 2)
        {
            throw new ArgumentOutOfRangeException("Buffer must be <= 1GB in size.");
        }

        if (buffer.Length <= 2 * sizeof(uint))
        {
            throw new ArgumentException("Buffer is too small to be valid!");
        }

        var parseArgs = new GeneratedSerializerParseArguments(0, this.remainingDepthLimit);
        var inner = this.innerSerializer;

        switch (this.option)
        {
            case FlatBufferDeserializationOption.Lazy:
                return buffer.InvokeLazyParse(inner, in parseArgs);

            case FlatBufferDeserializationOption.Greedy:
                return buffer.InvokeGreedyParse(inner, in parseArgs);

            case FlatBufferDeserializationOption.GreedyMutable:
                return buffer.InvokeGreedyMutableParse(inner, in parseArgs);

            case FlatBufferDeserializationOption.Progressive:
                return buffer.InvokeProgressiveParse(inner, in parseArgs);
        }

        throw new InvalidOperationException("Unexpected deserialization mode: " + this.option);
    }

    object ISerializer.Parse<TInputBuffer>(TInputBuffer buffer) => this.Parse(buffer);

    public int Write<TSpanWriter>(TSpanWriter writer, Span<byte> destination, T item)
        where TSpanWriter : ISpanWriter
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "The root table may not be null.");
        }

        if (destination.Length <= 8)
        {
            throw new BufferTooSmallException
            {
                SizeNeeded = this.GetMaxSize(item)
            };
        }

        if (this.enableMemoryCopySerialization &&
            item is IFlatBufferDeserializedObject deserializedObj &&
            deserializedObj.CanSerializeWithMemoryCopy)
        {
            IInputBuffer? inputBuffer = deserializedObj.InputBuffer;
            FlatSharpInternal.Assert(inputBuffer is not null, "Input buffer was null");

            if (destination.Length < inputBuffer.Length)
            {
                throw new BufferTooSmallException { SizeNeeded = inputBuffer.Length };
            }

            inputBuffer.GetReadOnlySpan().CopyTo(destination);
            return inputBuffer.Length;
        }

#if DEBUG
        int expectedMaxSize = this.GetMaxSize(item);
#endif

        var serializationContext = SerializationContext.ThreadLocalContext.Value!;
        serializationContext.Reset(destination.Length);

        ISharedStringWriter? sharedStringWriter = this.sharedStringWriter?.Value;
        serializationContext.SharedStringWriter = sharedStringWriter;

        serializationContext.Offset = 4; // first 4 bytes are reserved for uoffset to the first table.

        string? fileId = this.fileIdentifier;
        if (!string.IsNullOrEmpty(fileId))
        {
            destination[7] = (byte)fileId[3];
            destination[4] = (byte)fileId[0];
            destination[5] = (byte)fileId[1];
            destination[6] = (byte)fileId[2];

            serializationContext.Offset = 8;
        }

        try
        {
            if (sharedStringWriter?.IsDirty == true)
            {
                sharedStringWriter.Reset();
                Debug.Assert(!sharedStringWriter.IsDirty);
            }

            this.innerSerializer.Write(writer, destination, item, 0, serializationContext);

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

#if DEBUG
        Trace.WriteLine($"Serialized Size: {serializationContext.Offset + 1}, Max Size: {expectedMaxSize}");
        Debug.Assert(serializationContext.Offset <= expectedMaxSize - 1);
#endif

        return serializationContext.Offset;
    }

    int ISerializer.Write<TSpanWriter>(TSpanWriter writer, Span<byte> destination, object item)
    {
        return item switch
        {
            T t => this.Write(writer, destination, t),
            null => throw new ArgumentNullException(nameof(item)),
            _ => throw new ArgumentException($"Argument was not of the correct type. Type = {item.GetType().FullName}, Expected Type = {typeof(T).FullName}")
        };
    }

    public ISerializer<T> WithSettings(SerializerSettings settings)
    {
        return new GeneratedSerializerWrapper<T>(this, settings);
    }
}
