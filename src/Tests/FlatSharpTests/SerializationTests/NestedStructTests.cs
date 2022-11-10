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

namespace FlatSharpTests;

/// <summary>
/// Tests serialization of nested structs.
/// </summary>

public class NestedStructTests
{
    [Fact]
    public void NestedStructs_Greedy() => this.RunTest(FlatBufferDeserializationOption.Greedy);

    [Fact]
    public void NestedStructs_GreedyMutable() => this.RunTest(FlatBufferDeserializationOption.GreedyMutable);

    [Fact]
    public void NestedStructs_Progressive() => this.RunTest(FlatBufferDeserializationOption.Progressive);

    [Fact]
    public void NestedStructs_Lazy() => this.RunTest(FlatBufferDeserializationOption.Lazy);

    private void RunTest(FlatBufferDeserializationOption option)
    {
        Table table = new Table
        {
            Outer = new OuterStruct
            {
                InnerVirtual = new InnerStruct { A = 3 },
            }
        };

        ISerializer<Table> serializer = FlatBufferSerializer.Default.Compile<Table>().WithSettings(s => s.UseDeserializationMode(option));
        byte[] data = new byte[1024];
        serializer.Write(data, table);

        var parsed = serializer.Parse<Table>(data);
        Assert.Equal(3, parsed.Outer.InnerVirtual.A);
    }

    [FlatBufferTable]
    public class Table
    {
        [FlatBufferItem(0)]
        public virtual OuterStruct? Outer { get; set; }
    }

    [FlatBufferStruct]
    public class OuterStruct
    {
        [FlatBufferItem(0)]
        public virtual InnerStruct InnerVirtual { get; set; }
    }
}
