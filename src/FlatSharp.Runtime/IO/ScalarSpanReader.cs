/*
 * Copyright 2020 James Courtney
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

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace FlatSharp.Internal;

/// <summary>
/// A class that reads FlatBuffer scalars.
/// </summary>
public static class ScalarSpanReader
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBool(ReadOnlySpan<byte> span)
    {
        return span[0] != SerializationHelpers.False;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(ReadOnlySpan<byte> span)
    {
        return span[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ReadSByte(ReadOnlySpan<byte> span)
    {
        return (sbyte)span[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadShort(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadULong(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadUInt64LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadLong(ReadOnlySpan<byte> span)
    {
        return BinaryPrimitives.ReadInt64LittleEndian(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(ReadOnlySpan<byte> span)
    {
        FloatLayout layout = new FloatLayout
        {
            bytes = ReadUInt(span)
        };

        return layout.value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(ReadOnlySpan<byte> span)
    {
        return BitConverter.Int64BitsToDouble(ReadLong(span));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadString(ReadOnlySpan<byte> span, Encoding encoding)
    {
#if NETSTANDARD2_0
        byte[] array = ArrayPool<byte>.Shared.Rent(span.Length);
        span.CopyTo(array);
        string s = encoding.GetString(array, 0, span.Length);
        ArrayPool<byte>.Shared.Return(array);
        return s;
#else
        return encoding.GetString(span);
#endif
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct FloatLayout
    {
        [FieldOffset(0)]
        public uint bytes;

        [FieldOffset(0)]
        public float value;
    }
}
