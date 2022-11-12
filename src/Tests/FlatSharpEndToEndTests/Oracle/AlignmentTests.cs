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

using global::Google.FlatBuffers;

namespace FlatSharpEndToEndTests.Oracle;

public class AlignmentTests
{
    [Fact]
    public void NestedStructWithOddAlignment_Parse()
    {
        var builder = new FlatBufferBuilder(1024);
        var offset = Flatc.AlignmentTestOuter.CreateAlignmentTestOuter(builder, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Flatc.AlignmentTestOuterHoder.StartAlignmentTestOuterHoder(builder);
        Flatc.AlignmentTestOuterHoder.AddValue(builder, offset);
        var testData = Flatc.AlignmentTestOuterHoder.EndAlignmentTestOuterHoder(builder);
        builder.Finish(testData.Value);

        byte[] realBuffer = builder.SizedByteArray();
        var parsed = FS.AlignmentTestOuterHoder.Serializer.Parse(realBuffer);

        Assert.NotNull(parsed);

        var outer = parsed.Value;
        Assert.NotNull(outer);
        Assert.Equal(1, outer.A);
        Assert.Equal(2, outer.B);
        Assert.Equal(3, outer.C);
        Assert.Equal(4u, outer.D);
        Assert.Equal(5, outer.E);
        Assert.Equal(6ul, outer.F);

        var inner = outer.G;
        Assert.NotNull(inner);
        Assert.Equal(7, inner.A);
        Assert.Equal(8, inner.B);
        Assert.Equal(9, inner.C);
    }

    [Fact]
    public void NestedStructWithOddAlignment_Serialize()
    {
        FS.AlignmentTestOuterHoder holder = new()
        {
            Value = new()
            {
                A = 1,
                B = 2,
                C = 3,
                D = 4,
                E = 5,
                F = 6,
                G = new()
                {
                    A = 7,
                    B = 8,
                    C = 9,
                }
            }
        };

        Span<byte> memory = new byte[10240];
        int offset = FS.AlignmentTestOuterHoder.Serializer.Write(memory, holder);

        var parsed = Flatc.AlignmentTestOuterHoder.GetRootAsAlignmentTestOuterHoder(new ByteBuffer(memory.Slice(0, offset).ToArray()));

        var outer = parsed.Value.Value;
        Assert.Equal(1, outer.A);
        Assert.Equal(2, outer.B);
        Assert.Equal(3, outer.C);
        Assert.Equal(4u, outer.D);
        Assert.Equal(5, outer.E);
        Assert.Equal(6ul, outer.F);

        var inner = outer.G;
        Assert.Equal(7, inner.A);
        Assert.Equal(8, inner.B);
        Assert.Equal(9, inner.C);
    }

    [Fact]
    public void StructVectorDeserialize()
    {
        Flatc.StructVectorsTableT table = new Flatc.StructVectorsTableT
        {
            Vec = new Flatc.StructVectorsT
            {
                HashVec = new ulong[] { 1, 2, 3, 4 },
                AlignmentVec = new Flatc.FiveByteStructT[]
                {
                    new() { Int = 5, Byte = 6, },
                    new() { Int = 7, Byte = 8, },
                    new() { Int = 9, Byte = 10, },
                }
            }
        };

        var builder = new FlatBufferBuilder(1024);
        var offset = Flatc.StructVectorsTable.Pack(builder, table);
        builder.Finish(offset.Value);

        byte[] serialized = builder.SizedByteArray();

        var parsed = FS.StructVectorsTable.Serializer.Parse(serialized);

        Assert.Equal<ulong>(1, parsed.Vec.HashVec[0]);
        Assert.Equal<ulong>(2, parsed.Vec.HashVec[1]);
        Assert.Equal<ulong>(3, parsed.Vec.HashVec[2]);
        Assert.Equal<ulong>(4, parsed.Vec.HashVec[3]);

        Assert.Equal(5, parsed.Vec.AlignmentVec[0].Int);
        Assert.Equal(6, parsed.Vec.AlignmentVec[0].Byte);

        Assert.Equal(7, parsed.Vec.AlignmentVec[1].Int);
        Assert.Equal(8, parsed.Vec.AlignmentVec[1].Byte);

        Assert.Equal(9, parsed.Vec.AlignmentVec[2].Int);
        Assert.Equal(10, parsed.Vec.AlignmentVec[2].Byte);
    }

    [Fact]
    public void StructVectorSerialize()
    {
        FS.StructVectorsTable table = new()
        {
            Vec = new(),
        };

        table.Vec.HashVec[0] = 1;
        table.Vec.HashVec[1] = 2;
        table.Vec.HashVec[2] = 3;
        table.Vec.HashVec[3] = 4;

        table.Vec.AlignmentVec[0] = new() { Int = 5, Byte = 6 };
        table.Vec.AlignmentVec[1] = new() { Int = 7, Byte = 8 };
        table.Vec.AlignmentVec[2] = new() { Int = 9, Byte = 10 };

        byte[] data = new byte[1024];
        FS.StructVectorsTable.Serializer.Write(data, table);

        var parsed = Flatc.StructVectorsTable.GetRootAsStructVectorsTable(new ByteBuffer(data)).UnPack();

        Assert.Equal<ulong>(1, parsed.Vec.HashVec[0]);
        Assert.Equal<ulong>(2, parsed.Vec.HashVec[1]);
        Assert.Equal<ulong>(3, parsed.Vec.HashVec[2]);
        Assert.Equal<ulong>(4, parsed.Vec.HashVec[3]);

        Assert.Equal(5, parsed.Vec.AlignmentVec[0].Int);
        Assert.Equal(6, parsed.Vec.AlignmentVec[0].Byte);

        Assert.Equal(7, parsed.Vec.AlignmentVec[1].Int);
        Assert.Equal(8, parsed.Vec.AlignmentVec[1].Byte);

        Assert.Equal(9, parsed.Vec.AlignmentVec[2].Int);
        Assert.Equal(10, parsed.Vec.AlignmentVec[2].Byte);
    }
}
