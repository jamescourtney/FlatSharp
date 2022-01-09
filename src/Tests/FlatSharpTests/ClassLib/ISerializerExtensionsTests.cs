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

namespace FlatSharpTests;

#if NETCOREAPP3_1_OR_GREATER

/// <summary>
/// Tests for the FlatBufferVector class that implements IList.
/// </summary>

public class ISerializerExtensionsTests
{
    [Fact]
    public void BufferWriter_Generic()
    {
        ISerializer<Table> serializer = FlatBufferSerializer.Default.Compile<Table>();
        ArrayBufferWriter<byte> bw = new();

        int written = serializer.Write(bw, new Table { A = true, B = 3 });
        Assert.Equal(written, bw.WrittenCount);

        Table t = serializer.Parse(bw.WrittenMemory);
        Assert.True(t.A);
        Assert.Equal(3, t.B);
    }

    [Fact]
    public void BufferWriter_NonGeneric()
    {
        ISerializer serializer = FlatBufferSerializer.Default.Compile(typeof(Table));
        ArrayBufferWriter<byte> bw = new();

        int written = serializer.Write(bw, new Table { A = true, B = 3 });
        Assert.Equal(written, bw.WrittenCount);

        Table t = (Table)serializer.Parse(bw.WrittenMemory);
        Assert.True(t.A);
        Assert.Equal(3, t.B);
    }

    [FlatBufferTable]
    public class Table
    {
        [FlatBufferItem(0)]
        public virtual bool A { get; set; }

        [FlatBufferItem(1)]
        public virtual byte B { get; set; }
    }
}

#endif
