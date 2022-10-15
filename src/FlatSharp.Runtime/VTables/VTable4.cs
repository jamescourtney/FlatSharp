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
/// Represents a vtable for a table with up to 4 fields.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct VTable4 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    [FieldOffset(2)]
    private ushort offset2;

    [FieldOffset(4)]
    private ushort offset4;

    [FieldOffset(6)]
    private ushort offset6;

    [FieldOffset(0)]
    private ulong offset0ul;

    [FieldOffset(0)]
    private uint offset0ui;

    public int MaxSupportedIndex => 3;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable4 item)
        where TInputBuffer : IInputBuffer
    {
        if (BitConverter.IsLittleEndian)
        {
            CreateLittleEndian(inputBuffer, offset, out item);
        }
        else
        {
            CreateBigEndian(inputBuffer, offset, out item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
        where TInputBuffer : IInputBuffer
    {
        switch (index)
        {
            case 0: return this.offset0;
            case 1: return this.offset2;
            case 2: return this.offset4;
            case 3: return this.offset6;
            default: return 0;
        }
    }

    /// <summary>
    /// A generic/safe initialize method for BE archtectures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CreateBigEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable4 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        item = new VTable4();
        switch (fieldCount)
        {
            case 0:
                break;

            case 1:
                item.offset0 = ScalarSpanReader.ReadUShort(fieldData);
                return;

            case 2:
                item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                goto case 1;

            case 3:
                item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                goto case 2;

            default:    
                item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                goto case 3;
        }
    }

    /// <summary>
    /// An optimized load mmethod for LE architectures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void CreateLittleEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable4 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        item = new VTable4();
        switch (fieldCount)
        {
            case 0:
                return;

            case 1:
                 item.offset0 = ScalarSpanReader.ReadUShort(fieldData);
                return;

            case 2:
                item.offset0ui = ScalarSpanReader.ReadUInt(fieldData);
                return;

            case 3:
                item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                goto case 2;

            default:
                item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                return;
        }
    }
}
