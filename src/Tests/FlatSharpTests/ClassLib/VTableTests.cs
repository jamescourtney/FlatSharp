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

using System.Linq;
using FlatSharp.TypeModel;

namespace FlatSharpTests;

public class VTableTests
{
    public delegate void CreateCallback<TVTable>(ArrayInputBuffer buffer, int offset, out TVTable vt);

    [Fact]
    public void VTable0() => this.RunTests<VTable0>(-1, FlatSharp.VTable0.Create);


    [Fact]
    public void VTable1() => this.RunTests<VTable1>(0, FlatSharp.VTable1.Create);


    [Fact]
    public void VTable2() => this.RunTests<VTable2>(1, FlatSharp.VTable2.Create);


    [Fact]
    public void VTable3() => this.RunTests<VTable3>(2, FlatSharp.VTable3.Create);

    [Fact]
    public void VTable4() => this.RunTests<VTable4>(3, FlatSharp.VTable4.Create);
    
    [Fact]
    public void VTable5() => this.RunTests<VTable5>(4, FlatSharp.VTable5.Create);

    [Fact]
    public void VTable6() => this.RunTests<VTable6>(5, FlatSharp.VTable6.Create);

    [Fact]
    public void VTable7() => this.RunTests<VTable7>(6, FlatSharp.VTable7.Create);

    [Fact]
    public void VTable8() => this.RunTests<VTable8>(7, FlatSharp.VTable8.Create);

    [Fact]
    public void VTableGeneric() => this.RunTests<VTableGeneric>(255, FlatSharp.VTableGeneric.Create);

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

        Assert.Equal(expectedMaxIndex, vt.MaxSupportedIndex);

        for (int i = 0; i <= Math.Max(actualMaxIndex, expectedMaxIndex); ++i)
        {
            if (i <= Math.Min(actualMaxIndex, expectedMaxIndex))
            {
                Assert.Equal(i, vt.OffsetOf(buffer, i));
            }
            else
            {
                Assert.Equal(0, vt.OffsetOf(buffer, i));
            }
        }

        Assert.Equal(0, vt.OffsetOf(buffer, vt.MaxSupportedIndex + 1));
        Assert.Equal(0, vt.OffsetOf(buffer, -1));
    }
}
