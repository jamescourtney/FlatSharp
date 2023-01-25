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

namespace FlatSharpEndToEndTests.TableMembers;

public partial class TableMemberTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Bool(FlatBufferDeserializationOption option) => this.RunTest<bool, BoolTable>(true, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Byte(FlatBufferDeserializationOption option)
    {
         this.RunTest<byte, ByteTable>(1, option);

        ByteTable table = new();
        table.ItemMemory = new byte[] { 1, 2, 3, 4, 5, };
        table.ItemReadonlyMemory = new byte[] { 1, 2, 3, 4, 5, };

        ByteTable parsed = table.SerializeAndParse(option);

        Assert.NotNull(parsed.ItemMemory);
        Assert.NotNull(parsed.ItemReadonlyMemory);

        Memory<byte> mem = parsed.ItemMemory.Value;
        ReadOnlyMemory<byte> romem = parsed.ItemReadonlyMemory.Value;

        for (int i = 0; i < table.ItemMemory.Value.Length; ++i)
        {
            Assert.Equal(table.ItemMemory.Value.Span[i], mem.Span[i]);
            Assert.Equal(table.ItemMemory.Value.Span[i], romem.Span[i]);
        }
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void SByte(FlatBufferDeserializationOption option) => this.RunTest<sbyte, SByteTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Short(FlatBufferDeserializationOption option) => this.RunTest<short, ShortTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void UShort(FlatBufferDeserializationOption option) => this.RunTest<ushort, UShortTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Int(FlatBufferDeserializationOption option) => this.RunTest<int, IntTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void UInt(FlatBufferDeserializationOption option) => this.RunTest<uint, UIntTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Long(FlatBufferDeserializationOption option) => this.RunTest<long, LongTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void ULong(FlatBufferDeserializationOption option) => this.RunTest<ulong, ULongTable>(1, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Float(FlatBufferDeserializationOption option) => this.RunTest<float, FloatTable>(2.71f, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void Double(FlatBufferDeserializationOption option) => this.RunTest<double, DoubleTable>(3.14, option);

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void String(FlatBufferDeserializationOption option)
    {
        {
            byte[] emptyTable = new EmptyTable().AllocateAndSerialize();
            StringTable parsed = StringTable.Serializer.Parse(emptyTable, option);

            Assert.Null(parsed.ItemDeprecated);
            Assert.Null(parsed.ItemStandard);
            Assert.Null(parsed.ItemVectorImplicit);
            Assert.Null(parsed.ItemVectorReadonly);
        }

        {
            StringTable template = new()
            {
                ItemDeprecated = "deprecated",
                ItemStandard = "standard",
                ItemVectorImplicit = new[] { "a", "b", },
                ItemVectorReadonly = new[] { "c", "d", "e" },
            };

            StringTable parsed = template.SerializeAndParse(option);

            Assert.Null(parsed.ItemDeprecated);
            Assert.Equal("standard", parsed.ItemStandard);

            IList<string> @implicit = parsed.ItemVectorImplicit;
            Assert.Equal(2, @implicit.Count);
            Assert.Equal("a", @implicit[0]);
            Assert.Equal("b", @implicit[1]);

            IReadOnlyList<string> rolist = parsed.ItemVectorReadonly;
            Assert.Equal(3, rolist.Count);
            Assert.Equal("c", rolist[0]);
            Assert.Equal("d", rolist[1]);
            Assert.Equal("e", rolist[2]);
        }
    }

    private void RunTest<T, TTable>(T expectedDefault, FlatBufferDeserializationOption option)
        where T : struct
        where TTable : class, ITypedTable<T>, IFlatBufferSerializable<TTable>, new()
    {
        byte[] emptyTable = new EmptyTable().AllocateAndSerialize();

        TTable @default = new();

        TTable parsed = @default.Serializer.Parse(emptyTable, option);
        Assert.Null(parsed.ItemList);
        Assert.Null(parsed.ItemReadonlyList);
        Assert.Null(parsed.ItemOptional);
        Assert.Equal(default(T), parsed.ItemStandard);
        Assert.Equal(expectedDefault, @default.ItemWithDefault);
        Assert.Equal(default(T), parsed.ItemDeprecated);

        TTable table = new()
        {
            ItemDeprecated = expectedDefault,
            ItemWithDefault = default(T),
            ItemOptional = default(T),
            ItemStandard = expectedDefault,
            ItemList = new[] { expectedDefault, expectedDefault },
            ItemReadonlyList = new[] { expectedDefault, expectedDefault },
        };

        parsed = table.SerializeAndParse(option);

        Assert.NotNull(parsed.ItemList);
        Assert.NotNull(parsed.ItemReadonlyList);
        Assert.Equal(default(T), parsed.ItemOptional.Value);
        Assert.Equal(expectedDefault, parsed.ItemStandard);
        Assert.Equal(default(T), parsed.ItemWithDefault);
        Assert.Equal(default(T), parsed.ItemDeprecated);

        Assert.Equal(2, parsed.ItemList.Count);
        Assert.Equal(2, parsed.ItemReadonlyList.Count);

        for (int i = 0; i < 2; ++i)
        {
            Assert.Equal(expectedDefault, parsed.ItemReadonlyList[i]);
            Assert.Equal(expectedDefault, parsed.ItemList[i]);
        }
    }
}