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

namespace FlatSharp;

/// <summary>
/// An implementation of InputBuffer for writable memory segments.
/// </summary>
internal readonly
#if NET9_0_OR_GREATER
    ref 
#endif
struct SerializationTargetInputBuffer<TBuffer> : IInputBuffer
    where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{
    private readonly TBuffer target;
    private readonly Func<TBuffer, long, int, Span<byte>> getSpan;

    public SerializationTargetInputBuffer(
        TBuffer target, 
        Func<TBuffer, long, int, Span<byte>> getSpan)
    {
        this.target = target;
        this.getSpan = getSpan;
    }

    public bool IsPinned => false;

    public bool IsReadOnly => false;

    public long Length => this.target.Length;

    public ReadOnlySpan<byte> GetReadOnlySpan(long offset, int length)
    {
        return this.GetSpan(offset, length);
    }

    public Span<byte> GetSpan(long offset, int length)
    {
        return this.getSpan(this.target, offset, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<byte> GetReadOnlyMemory(long offset, int length)
    {
        return this.GetMemory(offset, length);
    }

    public Memory<byte> GetMemory(long offset, int length)
    {
        FSThrow.NotSupported_Generic("SerializationTargetInputBuffer does not support getting a memory");
        return default;
    }
}
