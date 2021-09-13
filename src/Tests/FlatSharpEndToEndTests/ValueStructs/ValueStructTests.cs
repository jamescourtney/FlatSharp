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

namespace FlatSharpEndToEndTests.ValueStructs
{
    using FlatSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Xunit;

    public class ValueStructTestCases
    {
        [Fact]
        public void WriteThrough_ValueStruct_InVector()
        {
            WriteThroughTable t = new WriteThroughTable
            {
                Points = new Vec3[]
                {
                    new() { X = 1, Y = 2, Z = 3 },
                    new() { X = 4, Y = 5, Z = 6 },
                }
            };

            byte[] data = new byte[1024];
            WriteThroughTable.Serializer.Write(data, t);

            var parsed = WriteThroughTable.Serializer.Parse(data);
            var parsed2 = WriteThroughTable.Serializer.Parse(data);

            Assert.Equal(1f, parsed2.Points[0].X);

            parsed.Points[0] = new Vec3 { X = -1, Y = -1, Z = -1 }; // triggers writethrough

            Assert.Equal(-1f, parsed2.Points[0].X);
        }

        [Fact]
        public void WriteThrough_ValueStruct_InTable()
        {
            WriteThroughTable t = new WriteThroughTable
            {
                Point = new Vec3 { X = 1, Y = 2, Z = 3 }
            };

            byte[] data = new byte[1024];
            WriteThroughTable.Serializer.Write(data, t);

            var parsed = WriteThroughTable.Serializer.Parse(data);
            var parsed2 = WriteThroughTable.Serializer.Parse(data);

            Assert.Equal(1f, parsed2.Point.X);
            Assert.Equal(2f, parsed2.Point.Y);
            Assert.Equal(3f, parsed2.Point.Z);

            parsed.Point = new Vec3 { X = -1, Y = -1, Z = -1 }; // triggers writethrough

            Assert.Equal(-1f, parsed2.Point.X);
            Assert.Equal(-1f, parsed2.Point.Y);
            Assert.Equal(-1f, parsed2.Point.Z);
        }

        [Fact]
        public void Basics()
        {
            Assert.Equal(148, Unsafe.SizeOf<ValueStruct>());
        }

        [Fact]
        public void ExtensionMethod_WorksForVectors()
        {
            ValueStruct v = default;

            Assert.Equal(128, v.D_Length);

            for (int i = 0; i < v.D_Length; ++i)
            {
                v.D(i) = (byte)i;
            }

            for (int i = 0; i < v.D_Length; ++i)
            {
                Assert.Equal((byte)i, v.D(i));
            }
        }

        [Fact]
        public void SerializeAndParse_Full()
        {
            ValueStruct vs = new ValueStruct
            {
                A = 1,
                B = 2,
                C = 3,
                Inner = new InnerValueStruct { A = 3.14f }
            };

            for (int i = 0; i < vs.D_Length; ++i)
            {
                vs.D(i) = (byte)i;
            }

            RefStruct rs = new()
            {
                A = 1,
                VS = vs,
            };

            RootTable table = new()
            {
                refStruct = rs,
                union = new TestUnion(vs),
                valueStruct = vs,
                valueStructVector = Enumerable.Range(0, 10).Select(x => vs).ToList(),
                vectorOfUnion = Enumerable.Range(0, 10).Select(x => new TestUnion(vs)).ToList(),
            };

            ISerializer<RootTable> serializer = RootTable.Serializer;
            int maxBytes = serializer.GetMaxSize(table);
            byte[] buffer = new byte[maxBytes];
            int written = serializer.Write(buffer, table);
            RootTable parsed = serializer.Parse(buffer[..written]);

            Assert.NotNull(parsed.refStruct);
            Assert.NotNull(parsed.valueStruct);
            Assert.NotNull(parsed.valueStructVector);
            Assert.NotNull(parsed.union);
            Assert.NotNull(parsed.vectorOfUnion);

            Assert.Equal(table.vectorOfUnion.Count, parsed.vectorOfUnion.Count);
            Assert.Equal(table.valueStructVector.Count, parsed.valueStructVector.Count);

            Assert.Equal(table.refStruct.A, parsed.refStruct.A);
            AssertStructsEqual(table.refStruct.VS, parsed.refStruct.VS);

            AssertStructsEqual(table.valueStruct.Value, parsed.valueStruct.Value);
            AssertStructsEqual(table.union.Value.ValueStruct, parsed.union.Value.ValueStruct);

            for (int i = 0; i < table.vectorOfUnion.Count; ++i)
            {
                var t = table.vectorOfUnion[i];
                var p = parsed.vectorOfUnion[i];
                AssertStructsEqual(t.ValueStruct, p.ValueStruct);
            }

            for (int i = 0; i < table.valueStructVector.Count; ++i)
            {
                var t = table.valueStructVector[i];
                var p = parsed.valueStructVector[i];
                AssertStructsEqual(t, p);
            }
        }

        [Fact]
        public void ValueStructs_OutOfRange()
        {
            ValueUnsafeStructVector_Byte v = default;
            Assert.Throws<IndexOutOfRangeException>(() => v.Vector(v.Vector_Length));
            Assert.Throws<IndexOutOfRangeException>(() => v.Vector(-1));

            ValueStruct safe = default;
            Assert.Throws<IndexOutOfRangeException>(() => safe.D(safe.D_Length));
            Assert.Throws<IndexOutOfRangeException>(() => safe.D(-1));
        }

        [Fact]
        public void ValueStructVector_Unsafe_Byte()
        {
            ValueUnsafeStructVector_Byte v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = byte.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(byte.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        [Fact]
        public void ValueStructVector_Unsafe_UShort()
        {
            ValueUnsafeStructVector_UShort v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = ushort.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(ushort.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        [Fact]
        public void ValueStructVector_Unsafe_UInt()
        {
            ValueUnsafeStructVector_UInt v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = uint.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(uint.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        [Fact]
        public void ValueStructVector_Unsafe_ULong()
        {
            ValueUnsafeStructVector_ULong v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = ulong.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(ulong.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        [Fact]
        public void ValueStructVector_Unsafe_Float()
        {
            ValueUnsafeStructVector_Float v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = float.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(float.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        [Fact]
        public void ValueStructVector_Unsafe_Double()
        {
            ValueUnsafeStructVector_Double v = default;

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                v.Vector(i) = double.MaxValue;
            }

            Assert.Equal(0ul, v.GuardLower);
            Assert.Equal(0ul, v.GuardHigher);

            for (int i = 0; i < v.Vector_Length; ++i)
            {
                Assert.Equal(double.MaxValue, v.Vector(i));
            }

            ValidateGuardedSpan(v);
        }

        private void ValidateGuardedSpan<T>(T item) where T : struct
        {
            Span<T> span = new T[1];
            span[0] = item;
            Span<byte> bytes = MemoryMarshal.Cast<T, byte>(span);

            Span<byte> guardLow = bytes[..sizeof(ulong)];
            Span<byte> mid = bytes[sizeof(ulong)..^sizeof(ulong)];
            Span<byte> guardHigh = bytes[^sizeof(ulong)..];

            int count = 0;
            for (int i = 0; i < guardLow.Length; ++i)
            {
                Assert.Equal(0, guardLow[i]);
                count++;
            }

            for (int i = 0; i < mid.Length; ++i)
            {
                Assert.NotEqual(0, mid[i]);
                count++;
            }

            for (int i = 0; i < guardHigh.Length; ++i)
            {
                Assert.Equal(0, guardHigh[i]);
                count++;
            }

            Assert.Equal(count, Unsafe.SizeOf<T>());
        }

        [Fact]
        public void SerializeAndParse_Empty()
        {
            RootTable t = new();

            Assert.Null(t.valueStruct);

            ISerializer<RootTable> serializer = RootTable.Serializer;
            int maxBytes = serializer.GetMaxSize(t);
            byte[] buffer = new byte[maxBytes];
            int written = serializer.Write(buffer, t);
            RootTable parsed = serializer.Parse(buffer[..written]);

            Assert.Null(parsed.refStruct);
            Assert.Null(parsed.valueStruct);
        }

        private static void AssertStructsEqual(ValueStruct a, ValueStruct b)
        {
            Assert.Equal(a.A, b.A);
            Assert.Equal(a.B, b.B);
            Assert.Equal(a.C, b.C);
            Assert.Equal(a.Inner.A, b.Inner.A);

            for (int i = 0; i < a.D_Length; ++i)
            {
                Assert.Equal(a.D(i), b.D(i));
            }

            Span<byte> scratchA = stackalloc byte[Unsafe.SizeOf<ValueStruct>()];
            Span<byte> scratchB = stackalloc byte[Unsafe.SizeOf<ValueStruct>()];

            MemoryMarshal.Cast<byte, ValueStruct>(scratchA)[0] = a;
            MemoryMarshal.Cast<byte, ValueStruct>(scratchB)[0] = b;

            Assert.True(scratchA.SequenceEqual(scratchB));
        }
    }
}