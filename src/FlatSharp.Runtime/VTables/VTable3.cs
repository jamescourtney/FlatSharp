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
/// Represents a vtable for a table with 3 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 6)]
[ExcludeFromCodeCoverage]
public struct VTable3 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    [FieldOffset(2)]
    private ushort offset1;

    [FieldOffset(4)]
    private ushort offset2;

    [FieldOffset(0)]
    private uint offset0ui;

    public int MaxSupportedIndex => 2;

    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable3 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        if (maxVtableIndex > 2)
        {
            maxVtableIndex = 2;
        }

        item = new VTable3();
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
            case 2: return this.offset2;
            default: return 0;
        }
    }
}