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

internal struct VirtualSerializationTarget : IFlatBufferSerializationTarget<VirtualSerializationTarget>
{
    public byte this[long index]
    {
        get => 0;
        set { }
    }

    public long Length => long.MaxValue;

    public VirtualSerializationTarget Slice(long start, long length)
    {
        return this;
    }

    public VirtualSerializationTarget Slice(long start)
    {
        return this;
    }

    public Span<byte> AsSpan(long start, int length)
    {
        return default;
    }
}