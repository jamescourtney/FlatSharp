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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// A serializer capable of reading and writing data to the FlatBuffer binary format.
    /// </summary>
    public sealed class FlatBufferSerializer
    {
        private static readonly ThreadLocal<SerializationContext> context = new ThreadLocal<SerializationContext>(() => new SerializationContext());
        private static readonly SpanWriter DefaultWriter = new SpanWriter();

        public static FlatBufferSerializer Default { get; } = new FlatBufferSerializer(new FlatBufferSerializerOptions());
        
        private readonly Dictionary<Type, object> serializerCache = new Dictionary<Type, object>();

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
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.Options = options;
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
            return this.GetOrCreateSerializer<T>();
        }

        /// <summary>
        /// Parses the given memory as an instance of T.
        /// </summary>
        public T Parse<T>(Memory<byte> memory) where T : class
        {
            return this.Parse<T>(new MemoryInputBuffer(memory));
        }

        /// <summary>
        /// Parses the given array as an instance of T.
        /// </summary>
        public T Parse<T>(byte[] buffer) where T : class
        {
            return this.Parse<T>(new ArraySegment<byte>(buffer));
        }

        /// <summary>
        /// Parses the given ArraySegment as an instance of T.
        /// </summary>
        public T Parse<T>(ArraySegment<byte> arraySegment) where T : class
        {
            return this.Parse<T>(new ArrayInputBuffer(arraySegment));
        }

        /// <summary>
        /// Parses the given block of memory as an instance of T. This operation is near-instant
        /// and is zero copy by default, which means that modifications to the backing buffer
        /// will modify the data in the resulting object.
        /// </summary>
        public T Parse<T>(InputBuffer buffer) where T : class
        {
            return this.GetOrCreateSerializer<T>().Parse(buffer);
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T>(T item, Span<byte> destination) where T : class
        {
            return this.Serialize(item, destination, DefaultWriter);
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T>(T item, Span<byte> destination, SpanWriter writer) where T : class
        {
            return this.GetOrCreateSerializer<T>().Write(writer, destination, item);
        }

        /// <summary>
        /// Gets the maximum serialized size of the given item.
        /// </summary>
        public int GetMaxSize<T>(T item) where T : class
        {
            return this.GetOrCreateSerializer<T>().GetMaxSize(item);
        }

        internal int ReflectionSerialize(object item, byte[] destination)
        {
            // unit test helper
            var method = this.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == nameof(this.SerializeInternal))
                .Single();

            var genericMethod = method.MakeGenericMethod(item.GetType());
            return (int)genericMethod.Invoke(this, new[] { item, destination });
        }

        internal object ReflectionParse(Type type, byte[] data)
        {
            // unit test helper
            var method = this.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == nameof(this.ParseInternal))
                .Single();

            var genericMethod = method.MakeGenericMethod(type);
            return genericMethod.Invoke(this, new[] { data });
        }

        private T ParseInternal<T>(byte[] buffer) where T : class
        {
            return this.Parse<T>(buffer);
        }

        private int SerializeInternal<T>(T item, byte[] destination) where T : class
        {
            return this.Serialize(item, destination);
        }

        private ISerializer<TRoot> GetOrCreateSerializer<TRoot>() where TRoot : class
        {
            if (!this.serializerCache.TryGetValue(typeof(TRoot), out object serializer))
            {
                lock (SharedLock.Instance)
                {
                    if (!this.serializerCache.TryGetValue(typeof(TRoot), out serializer))
                    {
                        serializer = new RoslynSerializerGenerator(this.Options).Compile<TRoot>();
                        this.serializerCache[typeof(TRoot)] = serializer;
                    }
                }
            }

            return (ISerializer<TRoot>)serializer;
        }
    }
}
