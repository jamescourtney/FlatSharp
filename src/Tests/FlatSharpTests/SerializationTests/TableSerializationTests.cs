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
using System.Runtime;

namespace FlatSharpTests;

/// <summary>
/// Verifies expected binary formats for test data.
/// </summary>

public class TableSerializationTests
{
    [Fact]
    public void RootTableNull()
    {
        Assert.Throws<ArgumentNullException>(() => FlatBufferSerializer.Default.Serialize<SimpleTable>(null, new byte[1024]));
    }

    [Fact]
    public void TableParse_NotMutable()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out _);

        Assert.Throws<NotMutableException>(() => table.String = null);
        Assert.Throws<NotMutableException>(() => table.Struct = null);
        Assert.Throws<NotMutableException>(() => table.StructVector = new List<SimpleStruct>());
        Assert.Throws<NotMutableException>(() => table.StructVector.Add(null));

        //Assert.Contains("FlatBufferVectorBase", table.StructVector.GetType().FullName);
    }

    [Fact]
    public void TableParse_Progressive()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out _);

        Assert.Throws<NotMutableException>(() => table.String = string.Empty);
        Assert.Throws<NotMutableException>(() => table.Struct.Long = 0);
        Assert.Throws<NotMutableException>(() => table.Struct = new());

        //Assert.Contains("FlatBufferProgressiveVector", table.StructVector.GetType().FullName);
    }

    [Fact]
    public void TableParse_GreedyImmutable()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var buffer);

        bool reaped = false;
        for (int i = 0; i < 5; ++i)
        {
            GC.Collect(2, GCCollectionMode.Forced);
            GCSettings.LatencyMode = GCLatencyMode.Batch;
            if (!buffer.TryGetTarget(out _))
            {
                reaped = true;
                break;
            }
        }

        Assert.True(reaped, "GC did not reclaim underlying byte buffer.");

        // The buffer has been collected. Now verify that we can read all the data as
        // we expect. This demonstrates that we've copied, as well as that we've 
        // released references.
        Assert.NotNull(table.String);
        Assert.NotNull(table.Struct);
        Assert.NotNull(table.StructVector);

        Assert.Equal("hi", table.String);
        Assert.Equal(1, table.Struct.Byte);
        Assert.Equal(2, table.Struct.Long);
        Assert.Equal(3u, table.Struct.Uint);

        Assert.Equal(4, table.StructVector[0].Byte);
        Assert.Equal(5, table.StructVector[0].Long);
        Assert.Equal(6u, table.StructVector[0].Uint);
    }

    private SimpleTable SerializeAndParse(FlatBufferDeserializationOption option, out WeakReference<byte[]> buffer)
    {
        SimpleTable table = new SimpleTable
        {
            String = "hi",
            Struct = new SimpleStruct { Byte = 1, Long = 2, Uint = 3 },
            StructVector = new List<SimpleStruct> { new SimpleStruct { Byte = 4, Long = 5, Uint = 6 } },
        };

        var serializer = FlatBufferSerializer.CompileFor(table).WithSettings(s => s.UseDeserializationMode(option));

        var rawBuffer = new byte[1024];
        serializer.Write(rawBuffer, table);
        buffer = new WeakReference<byte[]>(rawBuffer);

        return serializer.Parse<SimpleTable>(rawBuffer);
    }

    [FlatBufferTable]
    public class SimpleTable
    {
        [FlatBufferItem(0)]
        public virtual string? String { get; set; }

        [FlatBufferItem(1)]
        public virtual SimpleStruct? Struct { get; set; }

        [FlatBufferItem(2)]
        public virtual IList<SimpleStruct>? StructVector { get; set; }

        [FlatBufferItem(4)]
        public virtual SimpleTable? InnerTable { get; set; }

        [FlatBufferItem(5, DefaultValue = 0L)]
        public virtual long NoSetter { get; }
    }

    [FlatBufferStruct]
    public class SimpleStruct
    {
        [FlatBufferItem(0)]
        public virtual long Long { get; set; }

        [FlatBufferItem(1)]
        public virtual byte Byte { get; set; }

        [FlatBufferItem(2)]
        public virtual uint Uint { get; set; }
    }

    [FlatBufferTable]
    public class EmptyTable
    {
    }
}
