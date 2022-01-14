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

namespace FlatSharpTests;

public class VTableTests
{
    public delegate void CreateCallback<TVTable>(ArrayInputBuffer buffer, int offset, out TVTable vt);

    [Fact]
    public void Test_VTable4() => this.RunTests<VTable4>(3, VTable4.Create);

    [Fact]
    public void Test_VTable8() => this.RunTests<VTable8>(7, VTable8.Create);

    [Fact]
    public void Test_VTableGeneric() => this.RunTests<VTableGeneric>(255, VTableGeneric.Create);

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
