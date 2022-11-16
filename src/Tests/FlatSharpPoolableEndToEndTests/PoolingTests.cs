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

namespace FlatSharpEndToEndTests.PoolingTests;

public class PoolingTests
{
    [Theory]
    [InlineData(FlatBufferDeserializationOption.Progressive)]
    [InlineData(FlatBufferDeserializationOption.Greedy)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable)]
    public void NotLazy_Pools(FlatBufferDeserializationOption option)
    {
#if DEBUG
        var testPool = new TestObjectPool();
        ObjectPool.Instance = testPool;
#else
        ObjectPool.MaxToRetain = 100;
#endif

        byte[] buffer = CreateRoot(1);

        Root parsed = Root.Serializer.Parse(buffer, option);
        VerifyRoot(parsed, 1);

        HashSet<object> seenObjects = new();

        seenObjects.Add(parsed.InnerTable);
        seenObjects.Add(parsed.RefStruct);
        seenObjects.Add(parsed.VectorOfRefStruct);
        seenObjects.Add(parsed.VectorOfTable);
        seenObjects.Add(parsed.VectorOfRefStruct);
        seenObjects.Add(parsed.VectorOfValueStruct);
        seenObjects.Add(parsed.VectorOfUnion);
        seenObjects.Add(parsed.IndexedVector);

        foreach (var item in parsed.VectorOfTable)
        {
            seenObjects.Add(item);
        }

        foreach (var item in parsed.VectorOfRefStruct)
        {
            seenObjects.Add(item);
        }

        foreach (var item in parsed.VectorOfUnion)
        {
            seenObjects.Add(item);
            seenObjects.Add(item.Accept<Visitor, object>(new Visitor()));
        }

        foreach (var item in parsed.IndexedVector.Select(x => x.Value))
        {
            seenObjects.Add(item);
        }

        // Release all our stuff.
        parsed.ReturnToPool();

#if DEBUG
        foreach (var item in seenObjects)
        {
            Assert.True(testPool.IsInPool(item));

            int count = testPool.Count(item);
            Assert.True(count > 0);

            // return again -- verify that we don't double return.
            ((IPoolableObject)item).ReturnToPool(true);
            Assert.Equal(count, testPool.Count(item));
        }
#endif

        buffer = CreateRoot(2);

        // Parse again, and ensure that all the things are in the hash set.
        parsed = Root.Serializer.Parse(buffer, option);
        VerifyRoot(parsed, 2);

        Assert.Contains(parsed.InnerTable, seenObjects);
        Assert.Contains(parsed.RefStruct, seenObjects);
        Assert.Contains(parsed.VectorOfRefStruct, seenObjects);
        Assert.Contains(parsed.VectorOfTable, seenObjects);
        Assert.Contains(parsed.VectorOfValueStruct, seenObjects);
        Assert.Contains(parsed.VectorOfUnion, seenObjects);
        Assert.Contains(parsed.IndexedVector, seenObjects);

        foreach (var item in parsed.VectorOfTable)
        {
            Assert.Contains(item, seenObjects);
        }

        foreach (var item in parsed.VectorOfRefStruct)
        {
            Assert.Contains(item, seenObjects);
        }

        foreach (var item in parsed.VectorOfUnion)
        {
            Assert.Contains(item, seenObjects);
            Assert.Contains(item.Accept<Visitor, object>(new Visitor()), seenObjects);
        }

        foreach (var item in parsed.IndexedVector.Select(x => x.Value))
        {
            Assert.Contains(item, seenObjects);
        }

#if DEBUG
        foreach (var item in seenObjects)
        {
            Assert.False(testPool.IsInPool(item));
            Assert.Equal(0, testPool.Count(item));
        }
#endif
    }

#if DEBUG
    [Theory]
    [InlineData(FlatBufferDeserializationOption.Progressive)]
    [InlineData(FlatBufferDeserializationOption.Greedy)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable)]
    public void NotLazy_NonRoot_NoOp(FlatBufferDeserializationOption option)
    {
        var testPool = new TestObjectPool();
        ObjectPool.Instance = testPool;

        void AssertNotInPool(object item)
        {
            Assert.False(testPool.IsInPool(item));

            IPoolableObject? obj = item as IPoolableObject;

            Assert.NotNull(obj);

            // Verify call is ignored for non-root objects.
            obj.ReturnToPool();
            Assert.False(testPool.IsInPool(item));
        }

        byte[] buffer = CreateRoot(1);

        Root parsed = Root.Serializer.Parse(buffer, option);
        VerifyRoot(parsed, 1);

        AssertNotInPool(parsed.InnerTable);
        AssertNotInPool(parsed.RefStruct);
        AssertNotInPool(parsed.VectorOfRefStruct);
        AssertNotInPool(parsed.VectorOfValueStruct);
        AssertNotInPool(parsed.VectorOfTable);
        AssertNotInPool(parsed.VectorOfUnion);
        AssertNotInPool(parsed.IndexedVector);

        foreach (var item in parsed.VectorOfRefStruct)
        {
            AssertNotInPool(item);
        }

        foreach (var item in parsed.VectorOfTable)
        {
            AssertNotInPool(item);
        }

        foreach (var item in parsed.VectorOfUnion)
        {
            AssertNotInPool(item);
            AssertNotInPool(item.Accept<Visitor, object>(default));
        }

        foreach (var value in parsed.IndexedVector.Select(x => x.Value))
        {
            AssertNotInPool(value);
        }
    }

    [Fact]
    public void Lazy_MultipleReturn()
    {
        var testPool = new TestObjectPool();
        ObjectPool.Instance = testPool;

        byte[] buffer = CreateRoot(1);
        byte[] buffer2 = CreateRoot(2);

        Root parsedOriginal = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Lazy);
        VerifyRoot(parsedOriginal, 1);

        Assert.False(testPool.IsInPool(parsedOriginal));
        Assert.Equal(0, testPool.Count(parsedOriginal));

        parsedOriginal.ReturnToPool();
        
        Assert.True(testPool.IsInPool(parsedOriginal));
        Assert.Equal(1, testPool.Count(parsedOriginal));

        Root parsed2 = Root.Serializer.Parse(buffer2, FlatBufferDeserializationOption.Lazy);

        Assert.Same(parsedOriginal, parsed2);
        VerifyRoot(parsed2, 2);
        VerifyRoot(parsedOriginal, 2);

        Root parsed3 = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Lazy);
        VerifyRoot(parsed3, 1);

        Assert.NotSame(parsedOriginal, parsed3);

        Assert.Equal(0, testPool.Count(parsedOriginal));
        Assert.False(testPool.IsInPool(parsedOriginal));
        Assert.False(testPool.IsInPool(parsed2));
        Assert.False(testPool.IsInPool(parsed3));

        // Return works.
        parsedOriginal.ReturnToPool();
        Assert.Equal(1, testPool.Count(parsedOriginal));
        Assert.True(testPool.IsInPool(parsedOriginal));
        Assert.True(testPool.IsInPool(parsed2));
        Assert.False(testPool.IsInPool(parsed3));

        // Won't have any effect. 
        parsed2.ReturnToPool();
        Assert.Equal(1, testPool.Count(parsedOriginal));
        Assert.True(testPool.IsInPool(parsedOriginal));
        Assert.True(testPool.IsInPool(parsed2));
        Assert.False(testPool.IsInPool(parsed3));

        parsed3.ReturnToPool();
        Assert.Equal(2, testPool.Count(parsedOriginal));
        Assert.True(testPool.IsInPool(parsedOriginal));
        Assert.True(testPool.IsInPool(parsed2));
        Assert.True(testPool.IsInPool(parsed3));
    }

    [Fact]
    public void Lazy_Vectors()
    {
        var testPool = new TestObjectPool();
        ObjectPool.Instance = testPool;

        byte[] buffer = CreateRoot(1);

        Root parsedOriginal = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Lazy);
        VerifyRoot(parsedOriginal, 1);

        {
            var structVector = parsedOriginal.VectorOfRefStruct;

            HashSet<RefStruct> seenInstances = new();

            foreach (RefStruct refStruct in structVector)
            {
                seenInstances.Add(refStruct);
                Assert.False(testPool.IsInPool(refStruct));

                refStruct.ReturnToPool();
                Assert.True(testPool.IsInPool(refStruct));
            }

            Assert.Single(seenInstances);
        }

        {
            var unionVector = parsedOriginal.VectorOfUnion;
            HashSet<object> seenObjects = new();

            foreach (var item in unionVector)
            {
                seenObjects.Add(item);
                Assert.False(testPool.IsInPool(item));

                object value = item.Accept<Visitor, object>(default);
                seenObjects.Add(value);
                Assert.False(testPool.IsInPool(value));

                // returns the underlying item, even in lazy mode.
                item.ReturnToPool();

                Assert.True(testPool.IsInPool(item));
                Assert.True(testPool.IsInPool(value));
            }

            Assert.Equal(3, seenObjects.Count);
        }

        {
            var indexedVector = parsedOriginal.IndexedVector;
            HashSet<object> seenObjects = new();

            foreach (var item in indexedVector)
            {
                KeyValue value = item.Value;

                Assert.False(testPool.IsInPool(value));
                seenObjects.Add(value);

                // returns the underlying item, even in lazy mode.
                value.ReturnToPool();
                Assert.True(testPool.IsInPool(value));
            }

            indexedVector.ReturnToPool();
            Assert.True(testPool.IsInPool(indexedVector));
        }
    }
#endif

    private byte[] CreateRoot(int value)
    {
        Root root = new()
        {
            InnerTable = new InnerTable() { X = value },
            RefStruct = new RefStruct() { X = value, },
            ValueStruct = new ValueStruct() { X = value },
            VectorOfRefStruct = new[] { new RefStruct() { X = value }, new() { X = value } },
            VectorOfTable = new[] { new InnerTable() { X = value }, new() { X = value } },
            VectorOfValueStruct = new[] { new ValueStruct { X = value }, new() { X = value } },
            VectorOfUnion = new[] { new InnerUnion(new RefStruct { X = value }), new InnerUnion(new InnerTable { X = value })},
            IndexedVector = new IndexedVector<int, KeyValue>
            {
                new KeyValue { Key = value, Value = value, },
                new KeyValue { Key = value + 1, Value = value + 1, },
                new KeyValue { Key = value + 2, Value = value + 2, },
            }
        };

        byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
        Root.Serializer.Write(buffer, root);

        return buffer;
    }

    private static void VerifyRoot(Root item, int expectedValue)
    {
        Assert.Equal(expectedValue, item.InnerTable.X);
        Assert.Equal(expectedValue, item.RefStruct.X);
        Assert.Equal(expectedValue, item.ValueStruct.Value.X);

        foreach (var temp in item.VectorOfRefStruct)
        {
            Assert.Equal(expectedValue, temp.X);
        }

        foreach (var temp in item.VectorOfTable)
        {
            Assert.Equal(expectedValue, temp.X);
        }

        foreach (var temp in item.VectorOfValueStruct)
        {
            Assert.Equal(expectedValue, temp.X);
        }

        foreach (var temp in item.VectorOfUnion)
        {
            if (temp.TryGet(out InnerTable? value))
            {
                Assert.Equal(expectedValue, value.X);
            }

            if (temp.TryGet(out RefStruct? s))
            {
                Assert.Equal(expectedValue, s.X);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            int expected = expectedValue + i;
            Assert.True(item.IndexedVector.TryGetValue(expected, out KeyValue? value));
            Assert.Equal(expected, value.Key);
            Assert.Equal(expected, value.Value);
        }
    }

    private struct Visitor : InnerUnion.Visitor<object>
    {
        public object Visit(RefStruct item)
        {
            return item;
        }

        public object Visit(ValueStruct item)
        {
            return item;
        }

        public object Visit(InnerTable item)
        {
            return item;
        }
    }
}
