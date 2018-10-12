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
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// A serializer capable of reading and writing data to the FlatBuffer binary format.
    /// </summary>
    public sealed class FlatBufferSerializer
    {
        private static readonly ThreadLocal<SerializationContext> context = new ThreadLocal<SerializationContext>(() => new SerializationContext());
        private static readonly SpanWriter DefaultWriter = new SpanWriter();

        public static FlatBufferSerializer Default { get; } = new FlatBufferSerializer(new FlatBufferSerializerOptions());

        private readonly Dictionary<Type, object> parserCache = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> serializerCache = new Dictionary<Type, object>();
        private readonly ParserGenerator parserGenerator;
        private bool implementIDeserializedObject;

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
            this.CacheListVectorData = options.CacheListVectorData;
            this.implementIDeserializedObject = options.ImplementIDeserializedObject;

            this.parserGenerator = new ParserGenerator(this.implementIDeserializedObject, this.CacheListVectorData);
        }

        /// <summary>
        /// Indicates if list vectors should have their data cached after reading. This option will cause more allocations
        /// on deserializing, but will improve performance in cases of duplicate accesses to the same indices.
        /// </summary>
        public bool CacheListVectorData { get; }

        /// <summary>
        /// Precompiles a parser and serializer for the given type.
        /// </summary>
        public void PreCompile<T>()
        {
            this.GetOrCreateParser<T>();
            this.GetOrCreateSerializer<T>();
        }

        /// <summary>
        /// Parses the given memory as an instance of T.
        /// </summary>
        public T Parse<T>(Memory<byte> memory)
        {
            return this.Parse<T>(new MemoryInputBuffer(memory));
        }

        /// <summary>
        /// Parses the given array as an instance of T.
        /// </summary>
        public T Parse<T>(byte[] buffer)
        {
            return this.Parse<T>(new ArraySegment<byte>(buffer));
        }

        /// <summary>
        /// Parses the given ArraySegment as an instance of T.
        /// </summary>
        public T Parse<T>(ArraySegment<byte> arraySegment)
        {
            return this.Parse<T>(new ArrayInputBuffer(arraySegment));
        }

        /// <summary>
        /// Parses the given block of memory as an instance of T. This operation is near-instant
        /// and is zero copy by default, which means that modifications to the backing buffer
        /// will modify the data in the resulting object.
        /// </summary>
        public T Parse<T>(InputBuffer buffer)
        {
            if (buffer.Length >= int.MaxValue / 2)
            {
                throw new ArgumentOutOfRangeException("Memory must be <= 1GB in size.");
            }

            if (buffer.Length <= 2 * sizeof(uint))
            {
                throw new ArgumentException("Buffer is too small to be valid!");
            }

            var parser = this.GetOrCreateParser<T>();
            return parser(buffer, 0);
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T>(T item, Span<byte> destination)
        {
            return this.Serialize<T>(item, destination, DefaultWriter);
        }

        /// <summary>
        /// Writes the given object to the given memory block.
        /// </summary>
        /// <returns>The length of data that was written to the memory block.</returns>
        public int Serialize<T>(T item, Span<byte> destination, SpanWriter writer)
        {
#if DEBUG
            int expectedMaxSize = this.GetMaxSize<T>(item);
#endif

            var serializer = this.GetOrCreateSerializer<T>();

            var serializationContext = context.Value;

            serializationContext.Reset(destination.Length);
            serializationContext.Offset = 4; // first 4 bytes are reserved for uoffset to the first table.

            try
            {
                serializer.Write(writer, destination, item, 0, serializationContext);
            }
            catch (BufferTooSmallException ex)
            {
                ex.SizeNeeded = this.GetMaxSize(item);
                throw;
            }

#if DEBUG
            Debug.Assert(serializationContext.Offset <= expectedMaxSize - 1);
#endif

            return serializationContext.Offset;
        }

        /// <summary>
        /// Gets the maximum serialized size of the given item.
        /// </summary>
        public int GetMaxSize<T>(T item)
        {
            var serializer = this.GetOrCreateSerializer<T>();
            return 4 + SerializationHelpers.GetMaxPadding(4) + serializer.GetMaxSize(item);
        }

        private Func<InputBuffer, int, TRoot> GetOrCreateParser<TRoot>()
        {
            if (!this.parserCache.TryGetValue(typeof(TRoot), out object parser))
            {
                lock (CompilerLock.Instance)
                {
                    if (!this.parserCache.TryGetValue(typeof(TRoot), out parser))
                    {
                        parser = this.parserGenerator.GenerateParser<TRoot>();
                        this.parserCache[typeof(TRoot)] = parser;
                    }
                }
            }

            return (Func<InputBuffer, int, TRoot>)parser;
        }

        private ISerializer<TRoot> GetOrCreateSerializer<TRoot>()
        {
            if (!this.serializerCache.TryGetValue(typeof(TRoot), out object serializer))
            {
                lock (CompilerLock.Instance)
                {
                    if (!this.serializerCache.TryGetValue(typeof(TRoot), out serializer))
                    {
                        serializer = new SerializerGenerator().Compile<TRoot>();
                        this.serializerCache[typeof(TRoot)] = serializer;
                    }
                }
            }

            return (ISerializer<TRoot>)serializer;
        }

#if NET47
        /// <summary>
        /// Test hook for investigating IL generation issues.
        /// </summary>
        internal static void SaveDynamicAssembly()
        {
            CompilerLock.DynamicAssembly.Save("dynamic_" + Guid.NewGuid().ToString("n") + ".dll");
        }
#endif
    }
}
