/*
 * Copyright 2021 James Courtney
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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Channels;

    /// <summary>
    /// A simple object pool.
    /// </summary>
    public interface IFlatSharpObjectPool<T>
    {
        /// <summary>
        /// Attempts to remove an item from the pool.
        /// </summary>
        /// <returns>True if an item was removed from the pool.</returns>
        bool TryTake([NotNullWhen(true)] out T? item);

        /// <summary>
        /// Returns the given item to the pool (or discards it if the pool is full).
        /// </summary>
        void Return(T item);
    }

    /// <summary>
    /// A factory interface for <see cref="IFlatSharpObjectPool{T}"/>.
    /// </summary>
    public interface IFlatSharpObjectPoolFactory
    {
        /// <summary>
        /// Creates a new object pool for the given type <typeparamref name="T"/>
        /// with the specified bounded capacity.
        /// </summary>
        /// <param name="capacity">The capacity, or null for unbounded.</param>
        IFlatSharpObjectPool<T> Create<T>(int? capacity);
    }

    public class ChannelBasedObjectPoolFactory : IFlatSharpObjectPoolFactory
    {
        public IFlatSharpObjectPool<T> Create<T>(int? capacity)
        {
            if (capacity is null)
            {
                return new ChannelBasedObjectPool<T>(Channel.CreateUnbounded<T>());
            }
            else
            {
                return new ChannelBasedObjectPool<T>(Channel.CreateBounded<T>(capacity.Value));
            }
        }

        private class ChannelBasedObjectPool<T> : IFlatSharpObjectPool<T>
        {
            private readonly ChannelReader<T> reader;
            private readonly ChannelWriter<T> writer;

            public ChannelBasedObjectPool(Channel<T> channel)
            {
                this.reader = channel.Reader;
                this.writer = channel.Writer;
            }

            public void Return(T item)
            {
                this.writer.TryWrite(item);
            }

            public bool TryTake(out T? item)
            {
                return this.reader.TryRead(out item);
            }
        }
    }
}
