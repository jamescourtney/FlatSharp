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

namespace FlatSharp;


/// <summary>
/// Represents a vtable for a table with 1 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct VTable1 : IVTable
{
        
    [FieldOffset(0)]
    private ushort offset0;

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable1 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)6)
        {
            item = MemoryMarshal.Cast<byte, VTable1>(vtable.Slice(4, 2))[0];
        }
        else
        {
            item = new VTable1();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

            }
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

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable2 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)8)
        {
            item = MemoryMarshal.Cast<byte, VTable2>(vtable.Slice(4, 4))[0];
        }
        else
        {
            item = new VTable2();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

            }
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


/// <summary>
/// Represents a vtable for a table with 3 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 6)]
public struct VTable3 : IVTable
{
        
    [FieldOffset(0)]
    private ushort offset0;

        
    [FieldOffset(2)]
    private ushort offset1;

        
    [FieldOffset(4)]
    private ushort offset2;

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable3 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)10)
        {
            item = MemoryMarshal.Cast<byte, VTable3>(vtable.Slice(4, 6))[0];
        }
        else
        {
            item = new VTable3();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

            }
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


/// <summary>
/// Represents a vtable for a table with 4 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct VTable4 : IVTable
{
        
    [FieldOffset(0)]
    private ushort offset0;

        
    [FieldOffset(2)]
    private ushort offset1;

        
    [FieldOffset(4)]
    private ushort offset2;

        
    [FieldOffset(6)]
    private ushort offset3;

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable4 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)12)
        {
            item = MemoryMarshal.Cast<byte, VTable4>(vtable.Slice(4, 8))[0];
        }
        else
        {
            item = new VTable4();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

                case 3:
                {
                    vtable = vtable.Slice(4, 8);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                }
                break;

            }
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
            case 3: return this.offset3;
            default: return 0;
        }
    }
}


/// <summary>
/// Represents a vtable for a table with 5 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 10)]
public struct VTable5 : IVTable
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

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable5 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)14)
        {
            item = MemoryMarshal.Cast<byte, VTable5>(vtable.Slice(4, 10))[0];
        }
        else
        {
            item = new VTable5();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

                case 3:
                {
                    vtable = vtable.Slice(4, 8);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                }
                break;

                case 4:
                {
                    vtable = vtable.Slice(4, 10);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                }
                break;

            }
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
            case 3: return this.offset3;
            case 4: return this.offset4;
            default: return 0;
        }
    }
}


/// <summary>
/// Represents a vtable for a table with 6 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct VTable6 : IVTable
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

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable6 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)16)
        {
            item = MemoryMarshal.Cast<byte, VTable6>(vtable.Slice(4, 12))[0];
        }
        else
        {
            item = new VTable6();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

                case 3:
                {
                    vtable = vtable.Slice(4, 8);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                }
                break;

                case 4:
                {
                    vtable = vtable.Slice(4, 10);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                }
                break;

                case 5:
                {
                    vtable = vtable.Slice(4, 12);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                }
                break;

            }
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
            case 3: return this.offset3;
            case 4: return this.offset4;
            case 5: return this.offset5;
            default: return 0;
        }
    }
}


/// <summary>
/// Represents a vtable for a table with 7 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
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

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable7 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)18)
        {
            item = MemoryMarshal.Cast<byte, VTable7>(vtable.Slice(4, 14))[0];
        }
        else
        {
            item = new VTable7();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

                case 3:
                {
                    vtable = vtable.Slice(4, 8);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                }
                break;

                case 4:
                {
                    vtable = vtable.Slice(4, 10);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                }
                break;

                case 5:
                {
                    vtable = vtable.Slice(4, 12);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                }
                break;

                case 6:
                {
                    vtable = vtable.Slice(4, 14);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                    item.offset6 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(12, 2));
                }
                break;

            }
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
            case 3: return this.offset3;
            case 4: return this.offset4;
            case 5: return this.offset5;
            case 6: return this.offset6;
            default: return 0;
        }
    }
}


/// <summary>
/// Represents a vtable for a table with 8 fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct VTable8 : IVTable
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

        
    [FieldOffset(14)]
    private ushort offset7;

    
    public static void Create<TInputBuffer>(TInputBuffer inputBuffer, int offset, out VTable8 item)
        where TInputBuffer : IInputBuffer
    {
        inputBuffer.InitializeVTable(offset, out int vtableOffset, out int maxVtableIndex, out ushort vtableLength);
        ReadOnlySpan<byte> vtable = inputBuffer.GetReadOnlyByteMemory(vtableOffset, vtableLength).Span;

        
        if (BitConverter.IsLittleEndian && (uint)vtable.Length >= (nuint)20)
        {
            item = MemoryMarshal.Cast<byte, VTable8>(vtable.Slice(4, 16))[0];
        }
        else
        {
            item = new VTable8();
            switch (maxVtableIndex)
            {
                case 0:
                {
                    vtable = vtable.Slice(4, 2);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                }
                break;

                case 1:
                {
                    vtable = vtable.Slice(4, 4);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                }
                break;

                case 2:
                {
                    vtable = vtable.Slice(4, 6);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                }
                break;

                case 3:
                {
                    vtable = vtable.Slice(4, 8);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                }
                break;

                case 4:
                {
                    vtable = vtable.Slice(4, 10);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                }
                break;

                case 5:
                {
                    vtable = vtable.Slice(4, 12);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                }
                break;

                case 6:
                {
                    vtable = vtable.Slice(4, 14);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                    item.offset6 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(12, 2));
                }
                break;

                case 7:
                {
                    vtable = vtable.Slice(4, 16);
                    item.offset0 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(0, 2));
                    item.offset1 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(2, 2));
                    item.offset2 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(4, 2));
                    item.offset3 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(6, 2));
                    item.offset4 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(8, 2));
                    item.offset5 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(10, 2));
                    item.offset6 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(12, 2));
                    item.offset7 = BinaryPrimitives.ReadUInt16LittleEndian(vtable.Slice(14, 2));
                }
                break;

            }
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
            case 3: return this.offset3;
            case 4: return this.offset4;
            case 5: return this.offset5;
            case 6: return this.offset6;
            case 7: return this.offset7;
            default: return 0;
        }
    }
}

