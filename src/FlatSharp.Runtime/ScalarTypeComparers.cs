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
 
namespace FlatSharp
{
    using System;

    /*
     * This file contains many different span comparers for the FlatBuffer built-in scalar types.
     * It is possible to implement these generically with a callback delegate, but doing so incurs
     * a meaningful performance hit (upwards of 20%), which is why we define them explicitly here.
     */

    public class BoolSpanComparer : ISpanComparer
    {
        public static BoolSpanComparer Instance { get; } = new BoolSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadBool(left).CompareTo(ScalarSpanReader.ReadBool(right));
    }

    public class ByteSpanComparer : ISpanComparer
    {
        public static ByteSpanComparer Instance { get; } = new ByteSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right) 
            => ScalarSpanReader.ReadByte(left).CompareTo(ScalarSpanReader.ReadByte(right));
    }

    public class SByteSpanComparer : ISpanComparer
    {
        public static SByteSpanComparer Instance { get; } = new SByteSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadSByte(left).CompareTo(ScalarSpanReader.ReadSByte(right));
    }

    public class UShortSpanComparer : ISpanComparer
    {
        public static UShortSpanComparer Instance { get; } = new UShortSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadUShort(left).CompareTo(ScalarSpanReader.ReadUShort(right));
    }
    public class ShortSpanComparer : ISpanComparer
    {
        public static ShortSpanComparer Instance { get; } = new ShortSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadShort(left).CompareTo(ScalarSpanReader.ReadShort(right));
    }

    public class UIntSpanComparer : ISpanComparer
    {
        public static UIntSpanComparer Instance { get; } = new UIntSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadUInt(left).CompareTo(ScalarSpanReader.ReadUInt(right));
    }

    public class IntSpanComparer : ISpanComparer
    {
        public static IntSpanComparer Instance { get; } = new IntSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadInt(left).CompareTo(ScalarSpanReader.ReadInt(right));
    }

    public class ULongSpanComparer : ISpanComparer
    {
        public static ULongSpanComparer Instance { get; } = new ULongSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadULong(left).CompareTo(ScalarSpanReader.ReadULong(right));
    }

    public class LongSpanComparer : ISpanComparer
    {
        public static LongSpanComparer Instance { get; } = new LongSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadLong(left).CompareTo(ScalarSpanReader.ReadLong(right));
    }

    public class DoubleSpanComparer : ISpanComparer
    {
        public static DoubleSpanComparer Instance { get; } = new DoubleSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadDouble(left).CompareTo(ScalarSpanReader.ReadDouble(right));
    }

    public class FloatSpanComparer : ISpanComparer
    {
        public static FloatSpanComparer Instance { get; } = new FloatSpanComparer();

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
            => ScalarSpanReader.ReadFloat(left).CompareTo(ScalarSpanReader.ReadFloat(right));
    }
}
