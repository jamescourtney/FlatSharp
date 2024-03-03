/*
 * Copyright 2018 James Courtney
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

using FlatSharp.Internal;
using System.IO;

namespace FlatSharpEndToEndTests.ClassLib.VTables;

[TestClass]
public class VTableTests
{
    public delegate void CreateCallback<TVTable>(ArrayInputBuffer buffer, int offset, out TVTable vt);

    [TestMethod]
    public void Test_VTable4_Auto() => this.RunTests<VTable4>(3, VTable4.Create);

    [TestMethod]
    public void Test_VTable4_LE() => this.RunTests<VTable4>(3, VTable4.CreateLittleEndian);

    [TestMethod]
    public void Test_VTable4_BE() => this.RunTests<VTable4>(3, VTable4.CreateBigEndian);

    [TestMethod]
    public void Test_VTable8_Auto() => this.RunTests<VTable8>(7, VTable8.Create);

    [TestMethod]
    public void Test_VTable8_LE() => this.RunTests<VTable8>(7, VTable8.CreateLittleEndian);

    [TestMethod]
    public void Test_VTable8_BE() => this.RunTests<VTable8>(7, VTable8.CreateBigEndian);

    [TestMethod]
    public void Test_VTableGeneric() => this.RunTests<VTableGeneric>(255, VTableGeneric.Create);

    [TestMethod]
    public void InitializeVTable_NormalTable()
    {
        byte[] buffer =
        {
            8, 0, // vtable length
            12, 0, // table length
            4, 0, // index 0 offset
            8, 0, // index 1 offset
            8, 0, 0, 0, // soffset to vtable.
            1, 0, 0, 0,
            2, 0, 0, 0,
        };

        new ArrayInputBuffer(buffer).InitializeVTable(
            8,
            out int vtableOffset,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        Assert.AreEqual(0, vtableOffset);
        Assert.AreEqual((nuint)2, fieldCount);
        Assert.AreEqual(4, fieldData.Length);
    }

    [TestMethod]
    public void InitializeVTable_EmptyTable()
    {
        byte[] buffer =
        {
            4, 0, // vtable length
            4, 0, // table length
            4, 0, 0, 0 // soffset to vtable.
        };

        new ArrayInputBuffer(buffer).InitializeVTable(
            4,
            out int vtableOffset,
            out nuint fieldCount,
            out ReadOnlySpan<byte> fieldData);

        Assert.AreEqual(0, vtableOffset);
        Assert.AreEqual((nuint)0, fieldCount);
        Assert.AreEqual(0, fieldData.Length);
    }

    [TestMethod]
    public void InitializeVTable_InvalidLength()
    {
        byte[] buffer =
        {
            3, 0, // vtable length
            4, 0, // table length
            4, 0, 0, 0 // soffset to vtable.
        };

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
            new ArrayInputBuffer(buffer).InitializeVTable(4, out _, out _, out _));

        Assert.AreEqual(
            "FlatBuffer was in an invalid format: VTable was not long enough to be valid.",
            ex.Message);
    }

    [TestMethod]
    public void ReadUOffset()
    {
        byte[] buffer = { 4, 0, 0, 0 };
        Assert.AreEqual(4, new ArrayInputBuffer(buffer).ReadUOffset(0));

        buffer = new byte[] { 3, 0, 0, 0 };
        var ex = Assert.ThrowsException<InvalidDataException>(() => new ArrayInputBuffer(buffer).ReadUOffset(0));
        Assert.AreEqual(
            "FlatBuffer was in an invalid format: Decoded uoffset_t had value less than 4. Value = 3",
            ex.Message);
    }

    private void RunTests<TVTable>(int expectedMaxIndex, CreateCallback<TVTable> callback)
        where TVTable : struct, IVTable
    {
        for (int maxIndex = -1; maxIndex <= 20; ++maxIndex)
        {
            byte[] vtable = new byte[10 + (2 * maxIndex)];

            BinaryPrimitives.WriteInt32LittleEndian(vtable, -4);
            BinaryPrimitives.WriteUInt16LittleEndian(vtable.AsSpan().Slice(4), (ushort)(vtable.Length - 4));
            for (int i = 0; i <= maxIndex; ++i)
            {
                int start = 8 + (2 * i);
                BinaryPrimitives.WriteUInt16LittleEndian(
                    vtable.AsSpan().Slice(start, 2),
                    (ushort)i);
            }

            Test(vtable, expectedMaxIndex, maxIndex, callback);
        }
    }

    private void Test<TVTable>(
        byte[] vtable,
        int expectedMaxIndex,
        int actualMaxIndex,
        CreateCallback<TVTable> callback)
        where TVTable : struct, IVTable
    {
        var buffer = new ArrayInputBuffer(vtable);
        callback(buffer, 0, out TVTable vt);

        Assert.AreEqual(expectedMaxIndex, vt.MaxSupportedIndex);

        for (int i = 0; i <= Math.Max(actualMaxIndex, expectedMaxIndex); ++i)
        {
            if (i <= Math.Min(actualMaxIndex, expectedMaxIndex))
            {
                Assert.AreEqual(i, vt.OffsetOf(buffer, i));
            }
            else
            {
                Assert.AreEqual(0, vt.OffsetOf(buffer, i));
            }
        }

        Assert.AreEqual(0, vt.OffsetOf(buffer, vt.MaxSupportedIndex + 1));
        Assert.AreEqual(0, vt.OffsetOf(buffer, -1));
    }
}
