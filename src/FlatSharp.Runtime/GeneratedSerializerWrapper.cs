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
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// An implementation of <see cref="ISerializer{T}"/> that wraps a <see cref="IGeneratedSerializer{T}"/>.
    /// </summary>
    internal class GeneratedSerializerWrapper<T> : ISerializer<T> where T : class
    {
        private readonly IGeneratedSerializer<T> innerSerializer;
        private readonly Lazy<string> lazyCSharp;

        private ThreadLocal<ISharedStringWriter> sharedStringWriter;
        private SerializerSettings settings;

        public GeneratedSerializerWrapper(
            IGeneratedSerializer<T> innerSerializer,
            Assembly generatedAssembly,
            Func<string> generatedCSharp,
            byte[] generatedAssemblyBytes)
        {
            this.lazyCSharp = new Lazy<string>(generatedCSharp);
            this.Assembly = generatedAssembly;
            this.AssemblyBytes =generatedAssemblyBytes;
            this.innerSerializer = innerSerializer;
        }

        public string CSharp => this.lazyCSharp.Value;

        public Assembly Assembly { get; }

        public byte[] AssemblyBytes { get; }

        public int GetMaxSize(T item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new InvalidDataException("The root table may not be null.");
            }

            // 4 + padding(4) + inner serializer size. We add the extra to account for the very first uoffset.
            return sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint)) + this.innerSerializer.GetMaxSize(item);
        }

        public T Parse(IInputBuffer buffer)
        {
            if (buffer.Length >= int.MaxValue / 2)
            {
                throw new ArgumentOutOfRangeException("Buffer must be <= 1GB in size.");
            }

            if (buffer.Length <= 2 * sizeof(uint))
            {
                throw new ArgumentException("Buffer is too small to be valid!");
            }

            buffer.SharedStringReader = this.settings?.SharedStringReaderFactory?.Invoke();
            return buffer.InvokeParse(this.innerSerializer, 0);
        }

        public int Write(ISpanWriter writer, Span<byte> destination, T item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new InvalidDataException("The root table may not be null.");
            }

#if DEBUG
            int expectedMaxSize = this.GetMaxSize(item);
#endif

            var serializationContext = SerializationContext.ThreadLocalContext.Value;
            serializationContext.Reset(destination.Length);

            var sharedStringWriter = this.sharedStringWriter?.Value;
            serializationContext.SharedStringWriter = sharedStringWriter;

            serializationContext.Offset = 4; // first 4 bytes are reserved for uoffset to the first table.

            try
            {
                sharedStringWriter?.PrepareWrite();
                writer.InvokeWrite(this.innerSerializer, destination, item, 0, serializationContext);

                if (sharedStringWriter != null)
                {
                    writer.FlushSharedStrings(sharedStringWriter, destination, serializationContext);
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

        public ISerializer<T> WithSettings(SerializerSettings settings)
        {
            var clone = new GeneratedSerializerWrapper<T>(
                   this.innerSerializer,
                   this.Assembly,
                   () => this.CSharp,
                   this.AssemblyBytes);

            clone.settings = settings;

            var writerFactory = settings.SharedStringWriterFactory;
            if (writerFactory != null)
            {
                clone.sharedStringWriter = new ThreadLocal<ISharedStringWriter>(writerFactory);
            }

            return clone;
        }
    }
}
