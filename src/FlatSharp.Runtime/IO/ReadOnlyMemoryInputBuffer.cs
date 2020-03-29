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

namespace FlatSharp
{
    using System;

    /// <summary>
    /// An implemenation of InputBuffer that accepts ReadOnlyMemory. ReadOnlyMemoryInputBuffer
    /// behaves identically to MemoryInputBuffer with one exception, which is that it will refuse
    /// to deserialize any mutable memory (Memory{T}) instances. These will result in an exception
    /// being thrown. ReadOnlyMemoryInputBuffer guarantees that the objects returned will
    /// not modify in the input buffer (unless unsafe operations / MemoryMarshal) are used.
    /// </summary>
    public class ReadOnlyMemoryInputBuffer : SpanInputBuffer
    {
        private readonly ReadOnlyMemory<byte> memory;

        public ReadOnlyMemoryInputBuffer(ReadOnlyMemory<byte> memory)
        {
            this.memory = memory;
        }

        public override int Length => this.memory.Length;

        protected override Memory<byte> GetByteMemory(int start, int length)
        {
            throw new InvalidOperationException("ReadOnlyMemory inputs may not deserialize writable memory.");
        }

        protected override ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.memory.Slice(start, length);
        }

        protected sealed override ReadOnlySpan<byte> GetSpan(int offset) => this.memory.Span.Slice(offset);

        protected sealed override ReadOnlySpan<byte> GetSpan(int offset, int length) => this.memory.Span.Slice(offset, length);
    }
}
