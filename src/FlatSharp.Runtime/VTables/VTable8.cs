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
/// Represents a vtable for a table with up to 8 fields.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct VTable8 : IVTable
{
    [FieldOffset(0)]
    private ushort offset0;

    [FieldOffset(2)]
    private ushort offset2;

    [FieldOffset(4)]
    private ushort offset4;

    [FieldOffset(6)]
    private ushort offset6;

    [FieldOffset(8)]
    private ushort offset8;

    [FieldOffset(10)]
    private ushort offset10;

    [FieldOffset(12)]
    private ushort offset12;

    [FieldOffset(14)]
    private ushort offset14;

    [FieldOffset(0)]
    private uint offset0ui;

    [FieldOffset(8)]
    private uint offset8ui;

    [FieldOffset(0)]
    private ulong offset0ul;

    [FieldOffset(8)]
    private ulong offset8ul;

    public int MaxSupportedIndex => 7;

    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
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
            case 4: return this.offset8;
            case 5: return this.offset10;
            case 6: return this.offset12;
            case 7: return this.offset14;
            default: return 0;
        }
    }

    /// <summary>
    /// A generic/safe initialize method for BE archtectures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void CreateBigEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        item = new VTable8();
        switch (fieldCount)
        {
            case 0:
                break;

            case 1:
                {
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData);
                }
                break;

            case 2:
                {
                    fieldData = fieldData.Slice(0, 4);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                }
                break;

            case 3:
                {
                    fieldData = fieldData.Slice(0, 6);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                }
                break;

            case 4:
                {
                    fieldData = fieldData.Slice(0, 8);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                    item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                }
                break;

            case 5:
                {
                    fieldData = fieldData.Slice(0, 10);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                    item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                    item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                }
                break;

            case 6:
                {
                    fieldData = fieldData.Slice(0, 12);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                    item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                    item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                    item.offset10 = ScalarSpanReader.ReadUShort(fieldData.Slice(10, 2));
                }
                break;

            case 7:
                {
                    fieldData = fieldData.Slice(0, 14);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                    item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                    item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                    item.offset10 = ScalarSpanReader.ReadUShort(fieldData.Slice(10, 2));
                    item.offset12 = ScalarSpanReader.ReadUShort(fieldData.Slice(12, 2));
                }
                break;

            default:
                {
                    fieldData = fieldData.Slice(0, 16);
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData.Slice(0, 2));
                    item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                    item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                    item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                    item.offset10 = ScalarSpanReader.ReadUShort(fieldData.Slice(10, 2));
                    item.offset12 = ScalarSpanReader.ReadUShort(fieldData.Slice(12, 2));
                    item.offset14 = ScalarSpanReader.ReadUShort(fieldData.Slice(14, 2));
                }
                break;
        }
    }

    /// <summary>
    /// An optimized load mmethod for LE architectures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void CreateLittleEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        item = new VTable8();
        switch (fieldCount)
        {
            case 0:
                break;

            case 1:
                {
                    item.offset0 = ScalarSpanReader.ReadUShort(fieldData);
                }
                break;

            case 2:
                {
                    item.offset0ui = ScalarSpanReader.ReadUInt(fieldData);
                }
                break;

            case 3:
                {
                    fieldData = fieldData.Slice(0, 6);
                    item.offset0ui = ScalarSpanReader.ReadUInt(fieldData);
                    item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                }
                break;

            case 4:
                {
                    item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                }
                break;

            case 5:
                {
                    fieldData = fieldData.Slice(0, 10);
                    item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                    item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                }
                break;

            case 6:
                {
                    fieldData = fieldData.Slice(0, 12);
                    item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                    item.offset8ui = ScalarSpanReader.ReadUInt(fieldData.Slice(8, 4));
                }
                break;

            case 7:
                {
                    fieldData = fieldData.Slice(0, 14);
                    item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                    item.offset8ui = ScalarSpanReader.ReadUInt(fieldData.Slice(8, 4));
                    item.offset12 = ScalarSpanReader.ReadUShort(fieldData.Slice(12, 2));
                }
                break;

            default:
                {
                    fieldData = fieldData.Slice(0, 16);
                    item.offset0ul = ScalarSpanReader.ReadULong(fieldData);
                    item.offset8ul = ScalarSpanReader.ReadULong(fieldData.Slice(8, 8));
                }
                break;
        }
    }
}