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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading;

    /// <summary>
    /// A serializer capable of reading and writing data to the FlatBuffer binary format.
    /// </summary>
    public sealed class FlatBufferSerializer
    {
        public static FlatBufferSerializer Default { get; } = new FlatBufferSerializer(new FlatBufferSerializerOptions());

        private readonly Dictionary<Type, object> serializerCache = new Dictionary<Type, object>();

        private readonly object syncRoot = new();

        private TypeModelContainer container;

        /// <summary>
        /// Creates a new flatbuffer serializer using the default options.
        /// </summary>
        public FlatBufferSerializer()
            : this(new FlatBufferSerializerOptions())
        {
        }

        /// <summary>
        /// Creates a new FlatBufferSerializer using the given options.
        /// </summary>
        public FlatBufferSerializer(FlatBufferSerializerOptions options) 
            : this(options, TypeModelContainer.CreateDefault())
        {
        }

        /// <summary>
        /// Creates a new FlatBufferSerializer using the given options and type model provider.
        /// </summary>
        public FlatBufferSerializer(FlatBufferSerializerOptions options, TypeModelContainer typeModelContainer)
        {
            this.container = typeModelContainer ?? throw new ArgumentNullException(nameof(typeModelContainer));
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Creates a new FlatBufferSerializer using the given deserialization option.
        /// </summary>
        public FlatBufferSerializer(FlatBufferDeserializationOption deserializerOption)
            : this(new FlatBufferSerializerOptions(deserializerOption))
        {
        }

        /// <summary>
        /// Gets the set of options used to create this serializer.
        /// </summary>
        public FlatBufferSerializerOptions Options { get; }

        /// <summary>
        /// Compiles and returns the serializer instance for <typeparamref name="T"/>.
        /// </summary>
        public ISerializer<T> Compile<T>() where T : class
        {
            return this.GetOrCreateTypedSerializer<T>();
        }

        /// <summary>
        /// Gets or creates a serializer that will satisfy the item that
        /// is a FlatBuffer table.
        /// </summary>
        public ISerializer Compile(object item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Let's be helpful if user is referencing an object that we deserialized.
            // The type of a deserialized object won't match the type of the parent object,
            // but this interface allows us to query.
            Type actualType;
            if (item is IFlatBufferDeserializedObject deserialized)
            {
                actualType = deserialized.TableOrStructType;
            }
            else
            {
                actualType = item.GetType();
            }

            return this.GetOrCreateUntypedSerializer(actualType);
        }

        /// <summary>
        /// Gets or creates a serializr that will satisfy the given table type.
        /// </summary>
        public ISerializer Compile(Type itemType)
        {
            return this.GetOrCreateUntypedSerializer(itemType);
        }

        /// <summary>
        /// Parses the given memory as an instance of T.
        /// </summary>
        public T Parse<T>(Memory<byte> memory) where T : class
        {
            return this.Parse<T, MemoryInputBuffer>(
                new MemoryInputBuffer(memory));
        }

        /// <summary>
        /// Parses the given ReadOnlyMemory as an instance of T.
        /// </summary>
        public T Parse<T>(ReadOnlyMemory<byte> memory) where T : class
        {
            return this.Parse<T, ReadOnlyMemoryInputBuffer>(
                new ReadOnlyMemoryInputBuffer(memory));
        }

        /// <summary>
        /// Parses the given array as an instance of T.
        /// </summary>
        public T Parse<T>(byte[] buffer) where T : class
        {
            return this.Parse<T, ArrayInputBuffer>(new ArrayInputBuffer(buffer));
        }

        /// <summary>
        /// Parses the given ArraySegment as an instance of T.
        /// </summary>
        public T Parse<T>(ArraySegment<byte> arraySegment) where T : class
        {
            return this.Parse<T, ArraySegmentInputBuffer>(
                new ArraySegmentInputBuffer(arraySegment));
        }

        /// <summary>
        /// Parses the given input buffer as an instance of <typeparamref name="T"/>.
        /// </summary>
        public T Parse<T>(IInputBuffer buffer)
            where T : class
        {
            return this.Parse<T, IInputBuffer>(buffer);
        }

        /// <summary>
        /// Parses the given input buffer as an instance of <typeparamref name="T"/>.
        /// </summary>
        public T Parse<T, TInputBuffer>(TInputBuffer buffer) where TInputBuffer : IInputBuffer
            where T : class
        {
            return this.GetOrCreateTypedSerializer<T>().Parse(buffer);
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T>(T item, Span<byte> destination) where T : class
        {
            return this.Serialize(item, destination, default(SpanWriter));
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T, TSpanWriter>(T item, Span<byte> destination, TSpanWriter writer) 
            where T : class 
            where TSpanWriter : ISpanWriter
        {
            return this.GetOrCreateTypedSerializer<T>().Write(writer, destination, item);
        }

        /// <summary>
        /// Gets the maximum serialized size of the given item.
        /// </summary>
        public int GetMaxSize<T>(T item) where T : class
        {
            return this.GetOrCreateUntypedSerializer(typeof(T)).GetMaxSize(item);
        }

        private ISerializer<TRoot> GetOrCreateTypedSerializer<TRoot>() where TRoot : class
        {
            if (!this.serializerCache.TryGetValue(typeof(TRoot), out object? serializer))
            {
                lock (this.syncRoot)
                {
                    if (!this.serializerCache.TryGetValue(typeof(TRoot), out serializer))
                    {
                        serializer = new RoslynSerializerGenerator(this.Options, this.container).Compile<TRoot>();
                        this.serializerCache[typeof(TRoot)] = serializer;
                    }
                }
            }

            return (ISerializer<TRoot>)serializer;
        }

        /// <summary>
        /// Reflection to call into the method above. This is a one-time call per type of item,
        /// so not the end of the world.
        /// </summary>
        private ISerializer GetOrCreateUntypedSerializer(Type itemType)
        {
            if (!this.serializerCache.TryGetValue(itemType, out object? serializer))
            {
                var method = 
                    this.GetType()
                        .GetMethod(
                            nameof(GetOrCreateTypedSerializer), 
                            BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(itemType);

                try
                {
                    serializer = method.Invoke(this, new object[0]);
                }
                catch (TargetInvocationException ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex.InnerException!);
                    edi.Throw();
                }
            }

            return (ISerializer)serializer!;
        }
    }
}
