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

#if NET9_0_OR_GREATER

/// <summary>
/// A serialization target that wraps a Span{byte}. Only supports the 32-bit address space.
/// </summary>
public ref struct SpanSerializationTarget : IFlatBufferSerializationTarget<SpanSerializationTarget>
{
    private readonly Span<byte> span;
    
    public SpanSerializationTarget(Span<byte> span)
    {
        this.span = span;
    }

    public SpanSerializationTarget(byte[] array)
    {
        this.span = array.AsSpan();
    }

    public SpanSerializationTarget(Memory<byte> memory)
    {
        this.span = memory.Span;
    }
    
    public byte this[long index]
    {
        get => this.span[checked((int)index)];
        set => this.span[checked((int)index)] = value;
    }

    public long Length => this.span.Length;

    public SpanSerializationTarget Slice(long start, long length)
    {
        checked
        {
            return new(this.span.Slice((int)start, (int)length));
        }
    }

    public SpanSerializationTarget Slice(long start)
    {
        return new(this.span.Slice((int)start));
    }

    public Span<byte> AsSpan(long start, int length)
    {
        checked
        {
            return this.span.Slice((int)start, (int)length);
        }
    }
}

#endif