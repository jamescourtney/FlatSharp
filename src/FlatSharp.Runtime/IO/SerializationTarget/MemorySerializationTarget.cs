/*
 * Copyright 2024 James Courtney
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
/// A serialization target that wraps a Memory{byte}. Only supports the 32-bit address space.
/// </summary>
public readonly partial struct MemorySerializationTarget : IFlatBufferReaderWriter<MemorySerializationTarget>
{
    private readonly Memory<byte> memory;
    
    public MemorySerializationTarget(Memory<byte> memory)
    {
        this.memory = memory;
    }

    public long Length => this.memory.Length;

    public MemorySerializationTarget Slice(long start, long length)
    {
        checked
        {
            return new MemorySerializationTarget(this.memory.Slice((int)start, (int)length));
        }
    }

    public MemorySerializationTarget Slice(long start)
    {
        checked
        {
            return new MemorySerializationTarget(this.memory.Slice((int)start));
        }
    }
    
    private partial Span<byte> AsSpan(long offset, int length)
    {
        checked
        {
            return this.memory.Span.Slice((int)offset, length);
        }
    }
}
