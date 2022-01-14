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

namespace FlatSharp;

/// <summary>
/// Represents a vtable for a table with 7 fields.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 14)]
public struct VTable7 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    [FieldOffset(2)]
    private ushort offset1;

    [FieldOffset(4)]
    private ushort offset2;

    [FieldOffset(6)]
    private ushort offset3;

    [FieldOffset(8)]
    private ushort offset4;

    [FieldOffset(10)]
    private ushort offset5;

    [FieldOffset(12)]
    private ushort offset6;

    [FieldOffset(0)]
    private uint offset0ui;

    [FieldOffset(8)]
    private uint offset2ui;

    [FieldOffset(0)]
    private ulong offset0ul;

    public int MaxSupportedIndex => 6;

    public static void Create<TInputBuffer>(
        TInputBuffer inputBuffer,
        int offset,
        out VTable7 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        if (maxVtableIndex > 6)
        {
            maxVtableIndex = 6;
        }

        item = new VTable7();
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

            case 2:
            {
                vtable = vtable.Slice(4, 6);
                item.offset0ui = ScalarSpanReader.ReadUInt(vtable);
                item.offset2 = ScalarSpanReader.ReadUShort(vtable.Slice(4, 2));
            }
            break;

            case 3:
            {
                vtable = vtable.Slice(4, 8);
                item.offset0ul = ScalarSpanReader.ReadULong(vtable);
            }
            break;

            case 4:
            {
                vtable = vtable.Slice(4, 10);
                item.offset0ul = ScalarSpanReader.ReadULong(vtable);
                item.offset4 = ScalarSpanReader.ReadUShort(vtable.Slice(8, 2));
            }
            break;

            case 5:
            {
                vtable = vtable.Slice(4, 12);
                item.offset0ul = ScalarSpanReader.ReadULong(vtable);
                item.offset2ui = ScalarSpanReader.ReadUInt(vtable.Slice(8, 4));
            }
            break;

            case 6:
            {
                vtable = vtable.Slice(4, 14);
                item.offset0ul = ScalarSpanReader.ReadULong(vtable);
                item.offset2ui = ScalarSpanReader.ReadUInt(vtable.Slice(8, 4));
                item.offset6 = ScalarSpanReader.ReadUShort(vtable.Slice(12, 2));
            }
            break;
        }
    }

    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
        where TInputBuffer : IInputBuffer
    {
        switch (index)
        {
            case 0: return this.offset0;
            case 1: return this.offset1;
            case 2: return this.offset2;
            case 3: return this.offset3;
            case 4: return this.offset4;
            case 5: return this.offset5;
            case 6: return this.offset6;
            default: return 0;
        }
    }
}