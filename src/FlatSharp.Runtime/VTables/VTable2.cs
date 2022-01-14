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

using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

/// <summary>
/// Represents a vtable for a table with 2 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct VTable2 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    [FieldOffset(2)]
    private ushort offset1;

    [FieldOffset(0)]
    private uint offset0ui;

    public int MaxSupportedIndex => 1;

    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable2 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        if (maxVtableIndex > 1)
        {
            maxVtableIndex = 1;
        }

        item = new VTable2();
        switch (maxVtableIndex)
        {
            case 0:
            {
                vtable = vtable.Slice(4, 2);
                item.offset0 = ScalarSpanReader.ReadUShort(vtable);
            }
            break;

            case 1:
            {
                vtable = vtable.Slice(4, 4);
                item.offset0ui = ScalarSpanReader.ReadUInt(vtable);
            }
            break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
        where TInputBuffer : IInputBuffer
    {
        switch (index)
        {
            case 0: return this.offset0;
            case 1: return this.offset1;
            default: return 0;
        }
    }
}