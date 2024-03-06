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

[TestClass]
public class TableMemberTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Bool(FlatBufferDeserializationOption option) => this.RunTest<bool, BoolTable>(true, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Byte(FlatBufferDeserializationOption option)
    {
         this.RunTest<byte, ByteTable>(1, option);

        ByteTable table = new();
        table.ItemMemory = new byte[] { 1, 2, 3, 4, 5, };
        table.ItemReadonlyMemory = new byte[] { 1, 2, 3, 4, 5, };

        ByteTable parsed = table.SerializeAndParse(option);

        Assert.IsNotNull(parsed.ItemMemory);
        Assert.IsNotNull(parsed.ItemReadonlyMemory);

        Memory<byte> mem = parsed.ItemMemory.Value;
        ReadOnlyMemory<byte> romem = parsed.ItemReadonlyMemory.Value;

        for (int i = 0; i < table.ItemMemory.Value.Length; ++i)
        {
            Assert.AreEqual(table.ItemMemory.Value.Span[i], mem.Span[i]);
            Assert.AreEqual(table.ItemMemory.Value.Span[i], romem.Span[i]);
        }
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SByte(FlatBufferDeserializationOption option) => this.RunTest<sbyte, SByteTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Short(FlatBufferDeserializationOption option) => this.RunTest<short, ShortTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UShort(FlatBufferDeserializationOption option) => this.RunTest<ushort, UShortTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Int(FlatBufferDeserializationOption option) => this.RunTest<int, IntTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UInt(FlatBufferDeserializationOption option) => this.RunTest<uint, UIntTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Long(FlatBufferDeserializationOption option) => this.RunTest<long, LongTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ULong(FlatBufferDeserializationOption option) => this.RunTest<ulong, ULongTable>(1, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Float(FlatBufferDeserializationOption option) => this.RunTest<float, FloatTable>(2.71f, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Double(FlatBufferDeserializationOption option) => this.RunTest<double, DoubleTable>(3.14, option);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void String(FlatBufferDeserializationOption option)
    {
        {
            byte[] emptyTable = new EmptyTable().AllocateAndSerialize();
            StringTable parsed = StringTable.Serializer.Parse(emptyTable, option);

            Assert.IsNull(parsed.ItemDeprecated);
            Assert.IsNull(parsed.ItemStandard);
            Assert.IsNull(parsed.ItemVectorImplicit);
            Assert.IsNull(parsed.ItemVectorReadonly);
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

            Assert.IsNull(parsed.ItemDeprecated);
            Assert.AreEqual("standard", parsed.ItemStandard);

            IList<string> @implicit = parsed.ItemVectorImplicit;
            Assert.AreEqual(2, @implicit.Count);
            Assert.AreEqual("a", @implicit[0]);
            Assert.AreEqual("b", @implicit[1]);

            IReadOnlyList<string> rolist = parsed.ItemVectorReadonly;
            Assert.AreEqual(3, rolist.Count);
            Assert.AreEqual("c", rolist[0]);
            Assert.AreEqual("d", rolist[1]);
            Assert.AreEqual("e", rolist[2]);
        }
    }

    private void RunTest<T, TTable>(T expectedDefault, FlatBufferDeserializationOption option)
        where T : struct
        where TTable : class, ITypedTable<T>, IFlatBufferSerializable<TTable>, new()
    {
        byte[] emptyTable = new EmptyTable().AllocateAndSerialize();

        TTable @default = new();

        TTable parsed = @default.Serializer.Parse(emptyTable, option);
        Assert.IsNull(parsed.ItemList);
        Assert.IsNull(parsed.ItemReadonlyList);
        Assert.IsNull(parsed.ItemOptional);
        Assert.AreEqual(default(T), parsed.ItemStandard);
        Assert.AreEqual(expectedDefault, @default.ItemWithDefault);
        Assert.AreEqual(default(T), parsed.ItemDeprecated);

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

        Assert.IsNotNull(parsed.ItemList);
        Assert.IsNotNull(parsed.ItemReadonlyList);
        Assert.AreEqual(default(T), parsed.ItemOptional.Value);
        Assert.AreEqual(expectedDefault, parsed.ItemStandard);
        Assert.AreEqual(default(T), parsed.ItemWithDefault);
        Assert.AreEqual(default(T), parsed.ItemDeprecated);

        Assert.AreEqual(2, parsed.ItemList.Count);
        Assert.AreEqual(2, parsed.ItemReadonlyList.Count);

        for (int i = 0; i < 2; ++i)
        {
            Assert.AreEqual(expectedDefault, parsed.ItemReadonlyList[i]);
            Assert.AreEqual(expectedDefault, parsed.ItemList[i]);
        }
    }
}

public interface ITypedTable<T> where T : struct
{
    T ItemStandard { get; set; }

    T? ItemOptional { get; set; }

    T ItemWithDefault { get; set; }

    T ItemDeprecated { get; set; }

    IList<T>? ItemList { get; set; }

    IReadOnlyList<T>? ItemReadonlyList { get; set; }
}

public partial class BoolTable : ITypedTable<bool> { }
public partial class ByteTable : ITypedTable<byte> { }
public partial class SByteTable : ITypedTable<sbyte> { }
public partial class ShortTable : ITypedTable<short> { }
public partial class UShortTable : ITypedTable<ushort> { }
public partial class IntTable : ITypedTable<int> { }
public partial class UIntTable : ITypedTable<uint> { }
public partial class LongTable : ITypedTable<long> { }
public partial class ULongTable : ITypedTable<ulong> { }
public partial class FloatTable : ITypedTable<float> { }
public partial class DoubleTable : ITypedTable<double> { }