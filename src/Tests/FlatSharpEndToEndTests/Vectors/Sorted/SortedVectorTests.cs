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
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Vectors.Sorted;

public class SortedVectorTests
{
    [Fact]
    public void Bool() => this.SortedVectorTest<bool, BoolKey>(
        rng => rng.Next() % 2 == 0,
        rt => rt.ListVectorOfBool,
        (rt, l) => rt.ListVectorOfBool = l,
        () => new BoolKey(),
        k => k.Key,
        (k, v) => k.Key = v,
        Comparer<bool>.Default);

    [Fact]
    public void Byte() => this.SortedVectorStructTest<byte, ByteKey>(
        rt => rt.ListVectorOfByte,
        (rt, l) => rt.ListVectorOfByte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void SByte() => this.SortedVectorStructTest<sbyte, SByteKey>(
        rt => rt.ListVectorOfSbyte,
        (rt, l) => rt.ListVectorOfSbyte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Short() => this.SortedVectorStructTest<short, ShortKey>(
        rt => rt.ListVectorOfShort,
        (rt, l) => rt.ListVectorOfShort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void UShort() => this.SortedVectorStructTest<ushort, UShortKey>(
        rt => rt.ListVectorOfUshort,
        (rt, l) => rt.ListVectorOfUshort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Int() => this.SortedVectorStructTest<int, IntKey>(
        rt => rt.ListVectorOfInt,
        (rt, l) => rt.ListVectorOfInt = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void UInt() => this.SortedVectorStructTest<uint, UIntKey>(
        rt => rt.ListVectorOfUint,
        (rt, l) => rt.ListVectorOfUint = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Long() => this.SortedVectorStructTest<long, LongKey>(
        rt => rt.ListVectorOfLong,
        (rt, l) => rt.ListVectorOfLong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void ULong() => this.SortedVectorStructTest<ulong, ULongKey>(
        rt => rt.ListVectorOfUlong,
        (rt, l) => rt.ListVectorOfUlong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Float() => this.SortedVectorStructTest<float, FloatKey>(
        rt => rt.ListVectorOfFloat,
        (rt, l) => rt.ListVectorOfFloat = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Double() => this.SortedVectorStructTest<double, DoubleKey>(
        rt => rt.ListVectorOfDouble,
        (rt, l) => rt.ListVectorOfDouble = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void String_Base64() => this.SortedVectorTest<string, StringKey>(
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

    [Fact]
    public void Bool_ReadOnly() => this.SortedVectorTestReadOnly<bool, BoolKey>(
        rng => rng.Next() % 2 == 0,
        rt => rt.ListVectorOfBool,
        (rt, l) => rt.ListVectorOfBool = l,
        () => new BoolKey(),
        k => k.Key,
        (k, v) => k.Key = v,
        Comparer<bool>.Default);

    [Fact]
    public void Byte_ReadOnly() => this.SortedVectorStructTestReadOnly<byte, ByteKey>(
        rt => rt.ListVectorOfByte,
        (rt, l) => rt.ListVectorOfByte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void SByte_ReadOnly() => this.SortedVectorStructTestReadOnly<sbyte, SByteKey>(
        rt => rt.ListVectorOfSbyte,
        (rt, l) => rt.ListVectorOfSbyte = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Short_ReadOnly() => this.SortedVectorStructTestReadOnly<short, ShortKey>(
        rt => rt.ListVectorOfShort,
        (rt, l) => rt.ListVectorOfShort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void UShort_ReadOnly() => this.SortedVectorStructTestReadOnly<ushort, UShortKey>(
        rt => rt.ListVectorOfUshort,
        (rt, l) => rt.ListVectorOfUshort = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Int_ReadOnly() => this.SortedVectorStructTestReadOnly<int, IntKey>(
        rt => rt.ListVectorOfInt,
        (rt, l) => rt.ListVectorOfInt = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void UInt_ReadOnly() => this.SortedVectorStructTestReadOnly<uint, UIntKey>(
        rt => rt.ListVectorOfUint,
        (rt, l) => rt.ListVectorOfUint = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Long_ReadOnly() => this.SortedVectorStructTestReadOnly<long, LongKey>(
        rt => rt.ListVectorOfLong,
        (rt, l) => rt.ListVectorOfLong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void ULong_ReadOnly() => this.SortedVectorStructTestReadOnly<ulong, ULongKey>(
        rt => rt.ListVectorOfUlong,
        (rt, l) => rt.ListVectorOfUlong = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Float_ReadOnly() => this.SortedVectorStructTestReadOnly<float, FloatKey>(
        rt => rt.ListVectorOfFloat,
        (rt, l) => rt.ListVectorOfFloat = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void Double_ReadOnly() => this.SortedVectorStructTestReadOnly<double, DoubleKey>(
        rt => rt.ListVectorOfDouble,
        (rt, l) => rt.ListVectorOfDouble = l,
        k => k.Key,
        (k, v) => k.Key = v);

    [Fact]
    public void String_Base64_ReadOnly() => this.SortedVectorTestReadOnly<string, StringKey>(
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

    private void SortedVectorStructTestReadOnly<TKey, TValue>(
        Func<RootTableReadOnly, IReadOnlyList<TValue>> getList,
        Action<RootTableReadOnly, IReadOnlyList<TValue>> setList,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey)

        where TValue : class, ISortableTable<TKey>, new()
        where TKey : struct
    {
        this.SortedVectorTestReadOnly(
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
        Func<RootTable, IList<TValue>> getList,
        Action<RootTable, IList<TValue>> setList,
        Func<TValue, TKey> getKey,
        Action<TValue, TKey> setKey)

        where TValue : class, ISortableTable<TKey>, new()
        where TKey : struct
    {
        this.SortedVectorTest(
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

            this.SortedVectorTestReadOnly(table, getList, getKey, comparer);
        }
    }

    private  void SortedVectorTest<TKey, TValue>(
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

            this.SortedVectorTest(table, getList, getKey, comparer);
        }
    }

    private void SortedVectorTestReadOnly<TKey, TValue>(
        RootTableReadOnly root,
        Func<RootTableReadOnly, IReadOnlyList<TValue>> getList,
        Func<TValue, TKey> getKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        IReadOnlyList<TValue> rootList = getList(root);
        byte[] data = root.AllocateAndSerialize();

        void RunTest(
            FlatBufferDeserializationOption option)
        {
            var parsed = RootTableReadOnly.Serializer.Parse(data, option);

            IReadOnlyList<TValue> vector = getList(parsed);

            Assert.Equal(rootList.Count, vector.Count);

            if (rootList.Count > 0)
            {
                TValue previous = vector[0];

                for (int i = 0; i < rootList.Count; ++i)
                {
                    var item = vector[i];
                    Assert.True(comparer.Compare(getKey(previous), getKey(item)) <= 0);
                    previous = item;
                }

                foreach (var originalItem in rootList)
                {
                    var item = vector.BinarySearchByFlatBufferKey(getKey(originalItem));
                    Assert.NotNull(item);
                    Assert.Equal(getKey(originalItem).ToString(), getKey(item).ToString());
                }
            }
        }

        foreach (FlatBufferDeserializationOption mode in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
        {
            RunTest(mode);
        }
    }

    private void SortedVectorTest<TKey, TValue>(
        RootTable root,
        Func<RootTable, IList<TValue>> getList,
        Func<TValue, TKey> getKey,
        IComparer<TKey> comparer)
        where TValue : class, ISortableTable<TKey>
    {
        IList<TValue> rootList = getList(root);
        byte[] data = root.AllocateAndSerialize();

        void RunTest(
            FlatBufferDeserializationOption option)
        {
            var parsed = RootTable.Serializer.Parse(data, option);

            IList<TValue> vector = getList(parsed);

            Assert.Equal(rootList.Count, vector.Count);

            if (rootList.Count > 0)
            {
                TValue previous = vector[0];

                for (int i = 0; i < rootList.Count; ++i)
                {
                    var item = vector[i];
                    Assert.True(comparer.Compare(getKey(previous), getKey(item)) <= 0);
                    previous = item;
                }

                foreach (var originalItem in rootList)
                {
                    var item = vector.BinarySearchByFlatBufferKey(getKey(originalItem));
                    Assert.NotNull(item);
                    Assert.Equal(getKey(originalItem).ToString(), getKey(item).ToString());
                }
            }
        }

        foreach (FlatBufferDeserializationOption mode in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
        {
            RunTest(mode);
        }
    }
}
