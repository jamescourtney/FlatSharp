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

    public int MaxSupportedIndex => 7;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void CreateBigEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
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
                return;

            case 1:
                item.offset0 = ScalarSpanReader.ReadUShort(fieldData);
                return;

            case 2:
                item.offset2 = ScalarSpanReader.ReadUShort(fieldData.Slice(2, 2));
                goto case 1;

            case 3:
                item.offset4 = ScalarSpanReader.ReadUShort(fieldData.Slice(4, 2));
                goto case 2;

            case 4:
                item.offset6 = ScalarSpanReader.ReadUShort(fieldData.Slice(6, 2));
                goto case 3;

            case 5:
                item.offset8 = ScalarSpanReader.ReadUShort(fieldData.Slice(8, 2));
                goto case 4;

            case 6:
                item.offset10 = ScalarSpanReader.ReadUShort(fieldData.Slice(10, 2));
                goto case 5;

            case 7:
                item.offset12 = ScalarSpanReader.ReadUShort(fieldData.Slice(12, 2));
                goto case 6;

            default:
                item.offset14 = ScalarSpanReader.ReadUShort(fieldData.Slice(14, 2));
                goto case 7;
        }
    }

    /// <summary>
    /// An optimized load mmethod for LE architectures.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void CreateLittleEndian<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
        where TInputBuffer : IInputBuffer
    {
#if NETSTANDARD2_0
        CreateBigEndian(inputBuffer, offset, out item);
#else
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        if (fieldData.Length >= Unsafe.SizeOf<VTable8>())
        {
            item = MemoryMarshal.Read<VTable8>(fieldData);
        }
        else
        {
            item = default;
            Span<byte> target = MemoryMarshal.CreateSpan<byte>(ref Unsafe.As<VTable8, byte>(ref item), Unsafe.SizeOf<VTable8>());
            fieldData.CopyTo(target);
        }
#endif
    }
}