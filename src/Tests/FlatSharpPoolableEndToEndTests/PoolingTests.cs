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

namespace FlatSharpEndToEndTests.PoolingTests;

public class PoolingTests
{
    [Fact]
    public void Lazy_Pools()
    {
        ObjectPool.Instance = new DefaultObjectPool(10);

        byte[] buffer = CreateRoot(1);

        Root parsed = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Progressive);
        VerifyRoot(parsed, 1);

        HashSet<IPoolableObjectDebug> seenObjects = new();

        seenObjects.Add((IPoolableObjectDebug)parsed.InnerTable);
        seenObjects.Add((IPoolableObjectDebug)parsed.RefStruct);
        seenObjects.Add((IPoolableObjectDebug)parsed.VectorOfRefStruct);
        seenObjects.Add((IPoolableObjectDebug)parsed.VectorOfTable);
        seenObjects.Add((IPoolableObjectDebug)parsed.VectorOfRefStruct);
        seenObjects.Add((IPoolableObjectDebug)parsed.VectorOfValueStruct);

        foreach (var item in parsed.VectorOfTable)
        {
            seenObjects.Add((IPoolableObjectDebug)item);
        }

        foreach (var item in parsed.VectorOfRefStruct)
        {
            seenObjects.Add((IPoolableObjectDebug)item);
        }

        // Release all our stuff.
        parsed.ReturnToPool();

        Assert.Equal(((IPoolableObjectDebug)parsed).GetPoolSize(), 1);

        buffer = CreateRoot(2);

        // Parse again, and ensure that all the things are in the hash set.
        parsed = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Progressive);
        VerifyRoot(parsed, 2);

        Assert.Contains((IPoolableObjectDebug)parsed.InnerTable, seenObjects);
        Assert.Contains((IPoolableObjectDebug)parsed.RefStruct, seenObjects);
        Assert.Contains((IPoolableObjectDebug)parsed.VectorOfRefStruct, seenObjects);
        Assert.Contains((IPoolableObjectDebug)parsed.VectorOfTable, seenObjects);
        Assert.Contains((IPoolableObjectDebug)parsed.VectorOfValueStruct, seenObjects);

        foreach (var item in parsed.VectorOfTable)
        {
            Assert.Contains((IPoolableObjectDebug)item, seenObjects);
        }

        foreach (var item in parsed.VectorOfRefStruct)
        {
            Assert.Contains((IPoolableObjectDebug)item, seenObjects);
        }
    }

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
    }
}
