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

using System.Buffers.Binary;

namespace FlatSharp;

/// <summary>
/// A serialization target that wraps a Span{byte}. Only supports the 32-bit address space.
/// </summary>
public struct ArraySerializationTarget : IFlatBufferSerializationTarget<ArraySerializationTarget>
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
    
    public byte this[long index]
    {
        get => this.array[checked(this.start + (int)index)];
        set => this.array[checked(this.start + (int)index)] = value;
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

    public Span<byte> AsSpan(long start, int length)
    {
        checked
        {
            return this.array.AsSpan().Slice(this.start + (int)start, length);
        }
    }
}
