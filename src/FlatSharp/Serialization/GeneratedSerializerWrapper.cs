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
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// An implementation of <see cref="ISerializer{T}"/> that wraps a <see cref="IGeneratedSerializer{T}"/>.
    /// </summary>
    internal class GeneratedSerializerWrapper<T> : ISerializer<T>
    {
        private readonly IGeneratedSerializer<T> innerSerializer;

        public GeneratedSerializerWrapper(
            IGeneratedSerializer<T> innerSerializer,
            Assembly generatedAssembly,
            string generatedCsharp,
            byte[] generatedAssemblyBytes)
        {
            this.CSharp = generatedCsharp;
            this.Assembly = generatedAssembly;
            this.AssemblyBytes = ImmutableArray.Create(generatedAssemblyBytes);
            this.innerSerializer = innerSerializer;
        }

        public string CSharp { get; }

        public Assembly Assembly { get; }

        public ImmutableArray<byte> AssemblyBytes { get; }

        public int GetMaxSize(T item)
        {
            // 4 + padding(4) + inner serializer size. We add the extra to account for the very first uoffset.
            return sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint)) + this.innerSerializer.GetMaxSize(item);
        }

        public T Parse(InputBuffer buffer)
        {
            if (buffer.Length >= int.MaxValue / 2)
            {
                throw new ArgumentOutOfRangeException("Buffer must be <= 1GB in size.");
            }

            if (buffer.Length <= 2 * sizeof(uint))
            {
                throw new ArgumentException("Buffer is too small to be valid!");
            }

            return this.innerSerializer.Parse(buffer, 0);
        }

        public int Write(SpanWriter writer, Span<byte> destination, T item)
        {
#if DEBUG
            int expectedMaxSize = this.GetMaxSize(item);
#endif

            var serializationContext = SerializationContext.ThreadLocalContext.Value;

            serializationContext.Reset(destination.Length);
            serializationContext.Offset = 4; // first 4 bytes are reserved for uoffset to the first table.

            try
            {
                this.innerSerializer.Write(writer, destination, item, 0, serializationContext);
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
    }
}
