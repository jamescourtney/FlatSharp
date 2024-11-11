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
/// A serialization target that wraps an instance of <see cref="IInputBuffer"/>.
/// </summary>
public struct InputBufferSerializationTarget<TInputBuffer> : IFlatBufferSerializationTarget<InputBufferSerializationTarget<TInputBuffer>>
    where TInputBuffer : IInputBuffer
{
    private readonly long start;
    private readonly TInputBuffer buffer;

    public InputBufferSerializationTarget(TInputBuffer buffer)
    {
        this.buffer = buffer;
        this.start = 0;
        this.Length = buffer.Length;
    }

    public InputBufferSerializationTarget(TInputBuffer buffer, long start, long length)
    {
        this.buffer = buffer;
        this.start = start;
        this.Length = length;
    }
    
    public byte this[long index]
    {
        get => this.buffer.ReadByte(this.start + index);
        set => this.buffer.GetSpan(this.start + index, sizeof(byte))[0] = value;
    }

    public long Length { get; }

    public InputBufferSerializationTarget<TInputBuffer> Slice(long start, long length)
    {
        return new(this.buffer, this.start + start, length);
    }

    public InputBufferSerializationTarget<TInputBuffer> Slice(long start)
    {
        return new(this.buffer, this.start + start, this.Length - start);
    }

    public Span<byte> AsSpan(long start, int length)
    {
        checked
        {
            return this.buffer.GetSpan(this.start + start, length);
        }
    }
}