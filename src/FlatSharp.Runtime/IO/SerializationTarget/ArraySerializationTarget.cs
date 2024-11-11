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
/// A serialization target that wraps a Span{byte}. Only supports the 32-bit address space.
/// </summary>
public readonly partial struct ArraySerializationTarget : IFlatBufferReaderWriter<ArraySerializationTarget>
{
    private readonly byte[] array;
    private readonly int start;
    
    public ArraySerializationTarget(byte[] array)
    {
        this.array = array;
        this.Length = array.Length;
        this.start = 0;
    }

    public ArraySerializationTarget(byte[] array, int start, int length)
    {
        this.start = start;
        this.Length = length;
        this.array = array;
    }

    public long Length { get; }

    public ArraySerializationTarget Slice(long start, long length)
    {
        checked
        {
            return new ArraySerializationTarget(this.array, (int)(this.start + start), (int)length);
        }
    }

    public ArraySerializationTarget Slice(long start)
    {
        checked
        {
            return new(this.array, (int)(this.start + start), (int)(this.Length - start));
        }
    }

    private partial Span<byte> AsSpan(long offset, int length)
    {
        checked
        {
            long value = this.start + offset;
            return this.array.AsSpan((int)value, length);
        }
    }

    private partial ReadOnlySpan<byte> AsReadOnlySpan(long offset, int length) => this.AsSpan(offset, length);
}
