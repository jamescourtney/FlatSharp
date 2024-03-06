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

using FlatSharp.Internal;
using System.IO;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Vectors.Sorted;

[TestClass]
public class SortedVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Bool(FlatBufferDeserializationOption opt) => this.SortedVectorTest<bool, BoolKey>(
        opt,
        rng => rng.Next() % 2 == 0,
        rt => rt.ListVectorOfBool,
        (rt, l) => rt.ListVectorOfBool = l,
        () => new BoolKey(),
        k => k.Key,
        (k, v) => k.Key = v,
        Comparer<bool>.Default);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Byte(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<byte, ByteKey>(
        opt,
        rt => rt.ListVectorOfByte,
        (rt, l) => rt.ListVectorOfByte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SByte(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<sbyte, SByteKey>(
        opt,
        rt => rt.ListVectorOfSbyte,
        (rt, l) => rt.ListVectorOfSbyte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Short(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<short, ShortKey>(
        opt,
        rt => rt.ListVectorOfShort,
        (rt, l) => rt.ListVectorOfShort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UShort(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<ushort, UShortKey>(
        opt,
        rt => rt.ListVectorOfUshort,
        (rt, l) => rt.ListVectorOfUshort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Int(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<int, IntKey>(
        opt,
        rt => rt.ListVectorOfInt,
        (rt, l) => rt.ListVectorOfInt = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UInt(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<uint, UIntKey>(
        opt,
        rt => rt.ListVectorOfUint,
        (rt, l) => rt.ListVectorOfUint = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Long(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<long, LongKey>(
        opt,
        rt => rt.ListVectorOfLong,
        (rt, l) => rt.ListVectorOfLong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ULong(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<ulong, ULongKey>(
        opt,
        rt => rt.ListVectorOfUlong,
        (rt, l) => rt.ListVectorOfUlong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Float(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<float, FloatKey>(
        opt,
        rt => rt.ListVectorOfFloat,
        (rt, l) => rt.ListVectorOfFloat = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Double(FlatBufferDeserializationOption opt) => this.SortedVectorStructTest<double, DoubleKey>(
        opt,
        rt => rt.ListVectorOfDouble,
        (rt, l) => rt.ListVectorOfDouble = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void String_Base64(FlatBufferDeserializationOption opt) => this.SortedVectorTest<string, StringKey>(
        opt,
        rng =>
        {
            int length = rng.Next(0, 2048);
            byte[] data = new byte[length];
            rng.NextBytes(data);
            return Convert.ToBase64String(data);
        },
        rt => rt.ListVectorOfString,
        (rt, l) => rt.ListVectorOfString = l,
        () => new StringKey { Key = string.Empty },
        k => k.Key,
        (k, v) => k.Key = v,
        new Utf8StringComparer());

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Bool_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorTestReadOnly<bool, BoolKey>(
        opt,
        rng => rng.Next() % 2 == 0,
        rt => rt.ListVectorOfBool,
        (rt, l) => rt.ListVectorOfBool = l,
        () => new BoolKey(),
        k => k.Key,
        (k, v) => k.Key = v,
        Comparer<bool>.Default);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Byte_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<byte, ByteKey>(
        opt,
        rt => rt.ListVectorOfByte,
        (rt, l) => rt.ListVectorOfByte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SByte_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<sbyte, SByteKey>(
        opt,
        rt => rt.ListVectorOfSbyte,
        (rt, l) => rt.ListVectorOfSbyte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Short_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<short, ShortKey>(
        opt,
        rt => rt.ListVectorOfShort,
        (rt, l) => rt.ListVectorOfShort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UShort_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<ushort, UShortKey>(
        opt,
        rt => rt.ListVectorOfUshort,
        (rt, l) => rt.ListVectorOfUshort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Int_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<int, IntKey>(
        opt,
        rt => rt.ListVectorOfInt,
        (rt, l) => rt.ListVectorOfInt = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UInt_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<uint, UIntKey>(
        opt,
        rt => rt.ListVectorOfUint,
        (rt, l) => rt.ListVectorOfUint = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Long_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<long, LongKey>(
        opt,
        rt => rt.ListVectorOfLong,
        (rt, l) => rt.ListVectorOfLong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ULong_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<ulong, ULongKey>(
        opt,
        rt => rt.ListVectorOfUlong,
        (rt, l) => rt.ListVectorOfUlong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Float_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<float, FloatKey>(
        opt,
        rt => rt.ListVectorOfFloat,
        (rt, l) => rt.ListVectorOfFloat = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Double_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorStructTestReadOnly<double, DoubleKey>(
        opt,
        rt => rt.ListVectorOfDouble,
        (rt, l) => rt.ListVectorOfDouble = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void String_Base64_ReadOnly(FlatBufferDeserializationOption opt) => this.SortedVectorTestReadOnly<string, StringKey>(
        opt,
        rng =>
        {
            int length = rng.Next(0, 2048);
            byte[] data = new byte[length];
            rng.NextBytes(data);
            return Convert.ToBase64String(data);
        },
        rt => rt.ListVectorOfString,
        (rt, l) => rt.ListVectorOfString = l,
        () => new StringKey { Key = string.Empty },
        k => k.Key,
        (k, v) => k.Key = v,
        new Utf8StringComparer());

    [TestMethod]
    public void String_Null_Key_Is_Required()
    {
        RootTable root = new()
        {
            ListVectorOfString = new List<StringKey> { new() { Key = null, }, new() { Key = "a" }, new() { Key = "b" } }
        };

        Assert.IsTrue(typeof(StringKey).GetProperty("Key").GetCustomAttribute<FlatBufferItemAttribute>().Required);

        var ex = Assert.ThrowsException<InvalidOperationException>(() => root.AllocateAndSerialize());
        Assert.AreEqual("Table property 'FlatSharpEndToEndTests.Vectors.Sorted.StringKey.Key' is marked as required, but was not set.", ex.Message);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Int_Null_Key_OK(FlatBufferDeserializationOption option)
    {
        RootTable root = new()
        {
            ListVectorOfInt = new List<IntKey> { new() { }, new() { Key = 1 }, new() { Key = 2 } }
        };

        RootTable parsed = root.SerializeAndParse(option, out byte[] buffer);

        // ensure that the '5' is not written to the output.
        byte[] expected = 
        {
            4, 0, 0, 0,
            248, 255, 255, 255,
            20, 0, 0, 0,
            16, 0, 8, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 4, 0,
            3, 0, 0, 0,
            20, 0, 0, 0,
            32, 0, 0, 0,
            4, 0, 0, 0,
            252, 255, 255, 255,
            4, 0, 4, 0,
            248, 255, 255, 255,
            1, 0, 0, 0,
            6, 0, 8, 0,
            4, 0, 0, 0,
            8, 0, 0, 0,
            2, 0, 0, 0, 
        };

        Assert.IsTrue(buffer.SequenceEqual(expected));

        Assert.IsNotNull(parsed.ListVectorOfInt.BinarySearchByFlatBufferKey(5));
        Assert.IsNull(parsed.ListVectorOfInt.BinarySearchByFlatBufferKey(6));
    }

    [TestMethod]
    [DataRow(FlatBufferDeserializationOption.Lazy)]
    [DataRow(FlatBufferDeserializationOption.Progressive)]
    public void String_Null_Key_Fails_Binary_Search(FlatBufferDeserializationOption option)
    {
        RootTable_NonSorted root = new()
        {
            ListVectorOfString = new List<StringKey_NoKey> { new() { Key = null, }, new() { Key = "m" }, new() { Key = "z" } }
        };

        byte[] data = root.AllocateAndSerialize();
        RootTable parsed = RootTable.Serializer.Parse(data, option);

        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
        {
            var key = parsed.ListVectorOfString.BinarySearchByFlatBufferKey("a");
        });

        Assert.AreEqual("Sorted FlatBuffer vectors may not have null-valued keys.", ex.Message);
    }

    [TestMethod]
    [DataRow(FlatBufferDeserializationOption.Greedy)]
    [DataRow(FlatBufferDeserializationOption.GreedyMutable)]
    public void String_Null_Key_Fails_Greedy_Parse(FlatBufferDeserializationOption option)
    {
        RootTable_NonSorted root = new()
        {
            ListVectorOfString = new List<StringKey_NoKey> { new() { Key = null, }, new() { Key = "m" }, new() { Key = "z" } }
        };

        byte[] data = root.AllocateAndSerialize();

        var ex = Assert.ThrowsException<InvalidDataException>(() =>
        {
            RootTable parsed = RootTable.Serializer.Parse(data, option);
        });

        Assert.AreEqual("Table property 'FlatSharpEndToEndTests.Vectors.Sorted.StringKey.Key' is marked as required, but was missing from the buffer.", ex.Message);
    }

    private void SortedVectorStructTestReadOnly<TKey, TValue>(
        FlatBufferDeserializationOption option,
        Func<RootTableReadOnly, IReadOnlyList<TValue>> getList,
        Action<RootTableReadOnly, IReadOnlyList<TValue>> setList,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey)

        where TValue : class, ISortableTable<TKey>, new()
        where TKey : struct
    {
        this.SortedVectorTestReadOnly(
            option,
            rng =>
            {
                byte[] data = new byte[8];
                rng.NextBytes(data);
                TKey value = MemoryMarshal.Cast<byte, TKey>(data.AsSpan())[0];
                return value;
            },
            getList,
            setList,
            () => new TValue(),
            getKey,
            setKey,
            Comparer<TKey>.Default);
    }

    private void SortedVectorStructTest<TKey, TValue>(
        FlatBufferDeserializationOption option,
        Func<RootTable, IList<TValue>> getList,
        Action<RootTable, IList<TValue>> setList,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey)

        where TValue : class, ISortableTable<TKey>, new()
        where TKey : struct
    {
        this.SortedVectorTest(
            option,
            rng =>
            {
                byte[] data = new byte[8];
                rng.NextBytes(data);
                TKey value = MemoryMarshal.Cast<byte, TKey>(data.AsSpan())[0];
                return value;
            },
            getList,
            setList,
            () => new TValue(),
            getKey,
            setKey,
            Comparer<TKey>.Default);
    }

    private void SortedVectorTestReadOnly<TKey, TValue>(
        FlatBufferDeserializationOption opt,
        Func<Random, TKey> createKey,
        Func<RootTableReadOnly, IReadOnlyList<TValue>> getList,
        Action<RootTableReadOnly, IReadOnlyList<TValue>> setList,
        Func<TValue> createValue,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        Random rng = new Random();

        // Pick various lengths
        foreach (int length in Enumerable.Range(0, 20).Concat(Enumerable.Range(1, 5).Select(x => x * 100)))
        {
            RootTableReadOnly table = new();

            List<TValue> list = new List<TValue>();
            setList(table, list);

            for (int i = 0; i < length; ++i)
            {
                TValue value = createValue();
                setKey(value, createKey(rng));
                list.Add(value);
            }

            this.SortedVectorTestReadOnly(opt, table, getList, getKey, comparer);
        }
    }

    private void SortedVectorTest<TKey, TValue>(
        FlatBufferDeserializationOption option,
        Func<Random, TKey> createKey,
        Func<RootTable, IList<TValue>> getList,
        Action<RootTable, IList<TValue>> setList,
        Func<TValue> createValue,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        Random rng = new Random();

        // Pick various lengths
        foreach (int length in Enumerable.Range(0, 20).Concat(Enumerable.Range(1, 5).Select(x => x * 100)))
        {
            RootTable table = new();

            List<TValue> list = new List<TValue>();
            setList(table, list);

            for (int i = 0; i < length; ++i)
            {
                TValue value = createValue();
                setKey(value, createKey(rng));
                list.Add(value);
            }

            this.SortedVectorTest(option, table, getList, getKey, comparer);
        }
    }

    private void SortedVectorTestReadOnly<TKey, TValue>(
        FlatBufferDeserializationOption option,
        RootTableReadOnly root,
        Func<RootTableReadOnly, IReadOnlyList<TValue>> getList,
        Func<TValue, TKey> getKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        IReadOnlyList<TValue> rootList = getList(root);
        byte[] data = root.AllocateAndSerialize();

        var parsed = RootTableReadOnly.Serializer.Parse(data, option);

        IReadOnlyList<TValue> vector = getList(parsed);

        Assert.AreEqual(rootList.Count, vector.Count);

        if (rootList.Count > 0)
        {
            TValue previous = vector[0];

            for (int i = 0; i < rootList.Count; ++i)
            {
                var item = vector[i];
                Assert.IsTrue(comparer.Compare(getKey(previous), getKey(item)) <= 0);
                previous = item;
            }

            foreach (var originalItem in rootList)
            {
                var item = vector.BinarySearchByFlatBufferKey(getKey(originalItem));
                Assert.IsNotNull(item);
                Assert.AreEqual(getKey(originalItem).ToString(), getKey(item).ToString());
            }
        }
    }

    private void SortedVectorTest<TKey, TValue>(
        FlatBufferDeserializationOption option,
        RootTable root,
        Func<RootTable, IList<TValue>> getList,
        Func<TValue, TKey> getKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        IList<TValue> rootList = getList(root);
        byte[] data = root.AllocateAndSerialize();

        var parsed = RootTable.Serializer.Parse(data, option);

        IList<TValue> vector = getList(parsed);

        Assert.AreEqual(rootList.Count, vector.Count);

        if (rootList.Count > 0)
        {
            TValue previous = vector[0];

            for (int i = 0; i < rootList.Count; ++i)
            {
                var item = vector[i];
                Assert.IsTrue(comparer.Compare(getKey(previous), getKey(item)) <= 0);
                previous = item;
            }

            foreach (var originalItem in rootList)
            {
                var item = vector.BinarySearchByFlatBufferKey(getKey(originalItem));
                Assert.IsNotNull(item);
                Assert.AreEqual(getKey(originalItem).ToString(), getKey(item).ToString());
            }
        }
    }
}
