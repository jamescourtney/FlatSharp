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

using System;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.ValueStructs;

[TestClass]
public class ValueStructTestCases
{
    [TestMethod]
    public void WriteThrough_ValueStruct_InVector()
    {
        WriteThroughTable t = new WriteThroughTable
        {
            Points = new Vec3[]
            {
                new() { X = 1, Y = 2, Z = 3 },
                new() { X = 4, Y = 5, Z = 6 },
            },
            Point = default,
        };

        byte[] data = new byte[1024];
        WriteThroughTable.Serializer.Write(data, t);

        var parsed = WriteThroughTable.Serializer.Parse(data);
        var parsed2 = WriteThroughTable.Serializer.Parse(data);

        Assert.AreEqual(1f, parsed2.Points[0].X);

        parsed.Points[0] = new Vec3 { X = -1, Y = -1, Z = -1 }; // triggers writethrough

        Assert.AreEqual(-1f, parsed2.Points[0].X);
    }

    [TestMethod]
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

        Assert.AreEqual(1f, parsed2.Point.X);
        Assert.AreEqual(2f, parsed2.Point.Y);
        Assert.AreEqual(3f, parsed2.Point.Z);

        parsed.Point = new Vec3 { X = -1, Y = -1, Z = -1 }; // triggers writethrough

        Assert.AreEqual(-1f, parsed2.Point.X);
        Assert.AreEqual(-1f, parsed2.Point.Y);
        Assert.AreEqual(-1f, parsed2.Point.Z);
    }

    [TestMethod]
    public void Basics()
    {
        Assert.AreEqual(36, Unsafe.SizeOf<ValueStruct>());
    }

    [TestMethod]
    public void ExtensionMethod_WorksForVectors()
    {
        ValueStruct v = default;

        Assert.AreEqual(16, v.D_Length);

        for (int i = 0; i < v.D_Length; ++i)
        {
            v.D(i) = (byte)i;
        }

        for (int i = 0; i < v.D_Length; ++i)
        {
            Assert.AreEqual((byte)i, v.D(i));
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SerializeAndParse_Full(FlatBufferDeserializationOption option)
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
            RefStruct = rs,
            Union = new TestUnion(vs),
            ValueStruct = vs,
            ValueStructVector = Enumerable.Range(0, 10).Select(x => vs).ToList(),
            VectorOfUnion = Enumerable.Range(0, 10).Select(x => new TestUnion(vs)).ToList(),
            RequiredValueStruct = default,
        };

        ISerializer<RootTable> serializer = RootTable.Serializer;
        int maxBytes = serializer.GetMaxSize(table);
        byte[] buffer = new byte[maxBytes];
        int written = serializer.Write(buffer, table);
        RootTable parsed = serializer.Parse(buffer.AsMemory().Slice(0, written), option);

        Assert.IsNotNull(parsed.RefStruct);
        Assert.IsNotNull(parsed.ValueStruct);
        Assert.IsNotNull(parsed.ValueStructVector);
        Assert.IsNotNull(parsed.Union);
        Assert.IsNotNull(parsed.VectorOfUnion);

        Assert.AreEqual(table.VectorOfUnion.Count, parsed.VectorOfUnion.Count);
        Assert.AreEqual(table.ValueStructVector.Count, parsed.ValueStructVector.Count);

        Assert.AreEqual(table.RefStruct.A, parsed.RefStruct.A);
        AssertStructsEqual(table.RefStruct.VS, parsed.RefStruct.VS);

        AssertStructsEqual(table.ValueStruct.Value, parsed.ValueStruct.Value);
        AssertStructsEqual(table.Union.Value.ValueStruct, parsed.Union.Value.ValueStruct);

        for (int i = 0; i < table.VectorOfUnion.Count; ++i)
        {
            var t = table.VectorOfUnion[i];
            var p = parsed.VectorOfUnion[i];
            AssertStructsEqual(t.ValueStruct, p.ValueStruct);
        }

        for (int i = 0; i < table.ValueStructVector.Count; ++i)
        {
            var t = table.ValueStructVector[i];
            var p = parsed.ValueStructVector[i];
            AssertStructsEqual(t, p);
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ValueStructs_UnityNative_WellAligned_Serialize(FlatBufferDeserializationOption option)
    {
        int count = 10;

        UnityVectors_Native source = new()
        {
            WellAligned = new(Enumerable.Range(0, count).Select(x => new Vec3 { X = x, Y = x, Z = x }).ToArray(), default),
        };

        byte[] data = source.AllocateAndSerialize();

        var parsed = UnityVectors_List.Serializer.Parse(data, option);

        for (int i = 0; i < count; ++i)
        {
            Assert.AreEqual(source.WellAligned[i].X, parsed.WellAligned[i].X);
            Assert.AreEqual(source.WellAligned[i].Y, parsed.WellAligned[i].Y);
            Assert.AreEqual(source.WellAligned[i].Z, parsed.WellAligned[i].Z);
        }
    }

    [TestMethod]
    public void ValueStructs_UnityNative_PoorlyAligned_Serialize()
    {
        int count = 10;

        UnityVectors_Native source = new()
        {
            WellAligned = new Unity.Collections.NativeArray<Vec3>(Array.Empty<Vec3>(), default),
            PoorlyAligned = new(Enumerable.Range(0, count).Select(x => new PoorlyAligned { X = x, Y = 1, }).ToArray(), default),
        };

        var ex = Assert.ThrowsException<InvalidOperationException>(() => source.AllocateAndSerialize());
        Assert.AreEqual("Type 'FlatSharpEndToEndTests.ValueStructs.PoorlyAligned' does not support Unsafe Span operations because the size (5) is not a multiple of the alignment (4).", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ValueStructs_UnityNative_WellAligned_Parse(FlatBufferDeserializationOption option)
    {
        int count = 10;

        UnityVectors_List source = new()
        {
            WellAligned = Enumerable.Range(0, count).Select(x => new Vec3 { X = x, Y = x, Z = x }).ToArray(),
        };

        byte[] data = source.AllocateAndSerialize();

        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            var parsed = UnityVectors_Native.Serializer.Parse(new MemoryInputBuffer(data, true), option);

            for (int i = 0; i < count; ++i)
            {
                Assert.AreEqual(source.WellAligned[i].X, parsed.WellAligned[i].X);
                Assert.AreEqual(source.WellAligned[i].Y, parsed.WellAligned[i].Y);
                Assert.AreEqual(source.WellAligned[i].Z, parsed.WellAligned[i].Z);
            }
        }
        finally
        {
            handle.Free();
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ValueStructs_UnityNative_PoorlyAligned_Parse(FlatBufferDeserializationOption option)
    {
        int count = 10;

        UnityVectors_List source = new()
        {
            WellAligned = Array.Empty<Vec3>(),
            PoorlyAligned = Enumerable.Range(0, count).Select(x => new PoorlyAligned { X = x, Y = 1, }).ToArray(),
        };

        byte[] data = source.AllocateAndSerialize();
        GCHandle handle = GCHandle.Alloc(data);
        try
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var parsed = UnityVectors_Native.Serializer.Parse(new MemoryInputBuffer(data, true), option);
                float f = parsed.PoorlyAligned.Value[0].X;
            });

            Assert.AreEqual(
                "Type 'FlatSharpEndToEndTests.ValueStructs.PoorlyAligned' does not support Unsafe Span operations because the size (5) is not a multiple of the alignment (4).",
                ex.Message);
        }
        finally
        {
            handle.Free();
        }
    }

    [TestMethod]
    public void ValueStructs_OutOfRange()
    {
        ValueUnsafeStructVector_Byte v = default;
        Assert.ThrowsException<IndexOutOfRangeException>(() => v.Vector(v.Vector_Length));
        Assert.ThrowsException<IndexOutOfRangeException>(() => v.Vector(-1));

        ValueStruct safe = default;
        Assert.ThrowsException<IndexOutOfRangeException>(() => safe.D(safe.D_Length));
        Assert.ThrowsException<IndexOutOfRangeException>(() => safe.D(-1));
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_Byte()
    {
        ValueUnsafeStructVector_Byte v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = byte.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(byte.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_UShort()
    {
        ValueUnsafeStructVector_UShort v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = ushort.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(ushort.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_UInt()
    {
        ValueUnsafeStructVector_UInt v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = uint.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(uint.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_ULong()
    {
        ValueUnsafeStructVector_ULong v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = ulong.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(ulong.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_Float()
    {
        ValueUnsafeStructVector_Float v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = float.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(float.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    [TestMethod]
    public void ValueStructVector_Unsafe_Double()
    {
        ValueUnsafeStructVector_Double v = default;

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            v.Vector(i) = double.MaxValue;
        }

        Assert.AreEqual(0ul, v.GuardLower);
        Assert.AreEqual(0ul, v.GuardHigher);

        for (int i = 0; i < v.Vector_Length; ++i)
        {
            Assert.AreEqual(double.MaxValue, v.Vector(i));
        }

        ValidateGuardedSpan(v);
    }

    private void ValidateGuardedSpan<T>(T item) where T : struct
    {
        Span<T> span = new T[1];
        span[0] = item;
        Span<byte> bytes = MemoryMarshal.Cast<T, byte>(span);

        Span<byte> guardLow = bytes.Slice(0, sizeof(ulong));
        Span<byte> mid = bytes.Slice(sizeof(ulong), bytes.Length - (2 * sizeof(ulong)));
        Span<byte> guardHigh = bytes.Slice(bytes.Length - sizeof(ulong));

        int count = 0;
        for (int i = 0; i < guardLow.Length; ++i)
        {
            Assert.AreEqual(0, guardLow[i]);
            count++;
        }

        for (int i = 0; i < mid.Length; ++i)
        {
            Assert.AreNotEqual(0, mid[i]);
            count++;
        }

        for (int i = 0; i < guardHigh.Length; ++i)
        {
            Assert.AreEqual(0, guardHigh[i]);
            count++;
        }

        Assert.AreEqual(count, Unsafe.SizeOf<T>());
    }

    [TestMethod]
    public void SerializeAndParse_Empty()
    {
        RootTable t = new()
        {
            RequiredValueStruct = default,
        };

        Assert.IsNull(t.ValueStruct);

        ISerializer<RootTable> serializer = RootTable.Serializer;
        int maxBytes = serializer.GetMaxSize(t);
        byte[] buffer = new byte[maxBytes];
        int written = serializer.Write(buffer, t);
        RootTable parsed = serializer.Parse(buffer.AsMemory().Slice(0, written));

        Assert.IsNull(parsed.RefStruct);
        Assert.IsNull(parsed.ValueStruct);
    }

    private static void AssertStructsEqual(ValueStruct a, ValueStruct b)
    {
        Assert.AreEqual(a.A, b.A);
        Assert.AreEqual(a.B, b.B);
        Assert.AreEqual(a.C, b.C);
        Assert.AreEqual(a.Inner.A, b.Inner.A);

        for (int i = 0; i < a.D_Length; ++i)
        {
            Assert.AreEqual(a.D(i), b.D(i));
        }

        Span<byte> scratchA = stackalloc byte[Unsafe.SizeOf<ValueStruct>()];
        Span<byte> scratchB = stackalloc byte[Unsafe.SizeOf<ValueStruct>()];

        MemoryMarshal.Cast<byte, ValueStruct>(scratchA)[0] = a;
        MemoryMarshal.Cast<byte, ValueStruct>(scratchB)[0] = b;

        Assert.IsTrue(scratchA.SequenceEqual(scratchB));
    }
}
