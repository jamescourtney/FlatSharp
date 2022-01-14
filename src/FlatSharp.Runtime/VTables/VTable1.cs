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

using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

/// <summary>
/// Represents a vtable for a table with 1 field.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct VTable1 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    public int MaxSupportedIndex => 0;

    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable1 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        item = new VTable1();
        if (maxVtableIndex >= 0)
        {
            vtable = vtable.Slice(4, 2);
            item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
        where TInputBuffer : IInputBuffer
    {
        switch (index)
        {
            case 0: return this.offset0;
            default: return 0;
        }
    }
}