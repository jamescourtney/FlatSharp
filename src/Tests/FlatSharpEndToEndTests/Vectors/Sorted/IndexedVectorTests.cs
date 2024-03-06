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
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Vectors.Sorted;

[TestClass]
public class IndexedVectorTests
{
    private static readonly Random Rng = new();

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Bool(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<bool, BoolKey>(
        opt,
        x => x.IndexedVectorOfBool,
        (x, v) => x.IndexedVectorOfBool = v,
        () => new BoolKey { Key = Rng.Next() % 2 == 0 },
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Byte(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<byte, ByteKey>(
        opt,
        x => x.IndexedVectorOfByte,
        (x, v) => x.IndexedVectorOfByte = v,
        GetValueFactory<byte, ByteKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SByte(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<sbyte, SByteKey>(
        opt,
        x => x.IndexedVectorOfSbyte,
        (x, v) => x.IndexedVectorOfSbyte = v,
        GetValueFactory<sbyte, SByteKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Short(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<short, ShortKey>(
        opt,
        x => x.IndexedVectorOfShort,
        (x, v) => x.IndexedVectorOfShort = v,
        GetValueFactory<short, ShortKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UShort(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<ushort, UShortKey>(
        opt,
        x => x.IndexedVectorOfUshort,
        (x, v) => x.IndexedVectorOfUshort = v,
        GetValueFactory<ushort, UShortKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Int(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<int, IntKey>(
        opt,
        x => x.IndexedVectorOfInt,
        (x, v) => x.IndexedVectorOfInt = v,
        GetValueFactory<int, IntKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void UInt(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<uint, UIntKey>(
        opt,
        x => x.IndexedVectorOfUint,
        (x, v) => x.IndexedVectorOfUint = v,
        GetValueFactory<uint, UIntKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Long(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<long, LongKey>(
        opt,
        x => x.IndexedVectorOfLong,
        (x, v) => x.IndexedVectorOfLong = v,
        GetValueFactory<long, LongKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void ULong(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<ulong, ULongKey>(
        opt,
        x => x.IndexedVectorOfUlong,
        (x, v) => x.IndexedVectorOfUlong = v,
        GetValueFactory<ulong, ULongKey>((k, v) => k.Key = v),
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Float(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<float, FloatKey>(
        opt,
        x => x.IndexedVectorOfFloat,
        (x, v) => x.IndexedVectorOfFloat = v,
        () => new FloatKey { Key = (float)Rng.NextDouble() },
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Double(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<double, DoubleKey>(
        opt,
        x => x.IndexedVectorOfDouble,
        (x, v) => x.IndexedVectorOfDouble = v,
        () => new DoubleKey { Key = Rng.NextDouble() },
        k => k.Key);

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void String(FlatBufferDeserializationOption opt) => this.IndexedVectorTest<string, StringKey>(
        opt,
        x => x.IndexedVectorOfString,
        (x, v) => x.IndexedVectorOfString = v,
        () => 
        {
            int length = Rng.Next(0, 2048);
            byte[] data = new byte[length];
            Rng.NextBytes(data);
            return new StringKey { Key = Convert.ToBase64String(data) };
        },
        k => k.Key);

    private static Func<TValue> GetValueFactory<TKey, TValue>(Action<TValue, TKey> setKey)
        where TKey : struct
        where TValue : class, ISortableTable<TKey>, new()
    {
        return () =>
        {
            byte[] data = new byte[8];
            Rng.NextBytes(data);
            TKey key = MemoryMarshal.Cast<byte, TKey>(data.AsSpan())[0];
            var val = new TValue();
            setKey(val, key);
            return val;
        };
    } 

    private void IndexedVectorTest<TKey, TValue>(
        FlatBufferDeserializationOption option,
        Func<RootTableIndexed, IIndexedVector<TKey, TValue>> getVector,
        Action<RootTableIndexed, IIndexedVector<TKey, TValue>> setVector,
        Func<TValue> createValue,
        Func<TValue, TKey> getKey)
        where TValue : class, ISortableTable<TKey>
    {
        RootTableIndexed root = new();

        HashSet<TKey> knownKeys = new();

        IIndexedVector<TKey, TValue> originalVector = new IndexedVector<TKey, TValue>();
        for (int i = 0; i < 100; ++i)
        {
            var toAdd = createValue();

            // Some key types may have a small range of values
            // such as bool and byte.
            if (knownKeys.Add(getKey(toAdd)))
            {
                originalVector.Add(toAdd);
            }
        }

        setVector(root, originalVector);

        byte[] data = root.AllocateAndSerialize();
        var parsed = RootTableIndexed.Serializer.Parse(data, option);

        IIndexedVector<TKey, TValue> vector = getVector(parsed);

        Assert.AreEqual(knownKeys.Count, vector.Count);

        foreach (var key in knownKeys)
        {
            Assert.IsTrue(vector.TryGetValue(key, out TValue value));
            Assert.IsTrue(vector.ContainsKey(key));
            Assert.IsNotNull(vector[key]);
            Assert.IsNotNull(value);
            Assert.AreEqual(key, getKey(value));
        }

        for (int i = 0; i < 100; ++i)
        {
            TKey key = getKey(createValue());
            if (!knownKeys.Contains(key))
            {
                bool result = vector.TryGetValue(key, out TValue value);

                Assert.IsFalse(result);
                Assert.IsNull(value);
                Assert.IsFalse(vector.ContainsKey(key));
                Assert.ThrowsException<KeyNotFoundException>(() => vector[key]);
            }
        }
    }
}
