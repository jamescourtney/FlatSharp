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
    [Fact]
    public void Lazy_Pools()
    {
        ObjectPool.Instance = new DefaultObjectPool(10);

        byte[] buffer = CreateRoot(1);

        Root parsed = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Progressive);
        VerifyRoot(parsed, 1);

        HashSet<object> seenObjects = new();

        var vectorOfRefStructOriginal = parsed.vector_of_ref_struct;
        var vectorOfTableOriginal = parsed.vector_of_table;

        seenObjects.Add(parsed.inner_table);
        seenObjects.Add(parsed.ref_struct);
        seenObjects.Add(vectorOfRefStructOriginal);
        seenObjects.Add(vectorOfTableOriginal);
        seenObjects.Add(parsed.vector_of_value_struct);
        
        foreach (var item in vectorOfTableOriginal)
        {
            seenObjects.Add(item);
        }

        foreach (var item in vectorOfRefStructOriginal)
        {
            seenObjects.Add(item);
        }

        // Release all our stuff.
        // parsed.ReturnToPool();

        buffer = CreateRoot(2);

        // Parse again, and ensure that all the things are in the hash set.
        parsed = Root.Serializer.Parse(buffer, FlatBufferDeserializationOption.Progressive);
        VerifyRoot(parsed, 2);

        Assert.Contains(parsed.inner_table, seenObjects);
        Assert.Contains(parsed.ref_struct, seenObjects);
        Assert.Contains(parsed.vector_of_ref_struct, seenObjects);
        Assert.Contains(parsed.vector_of_table, seenObjects);
        Assert.Contains(parsed.vector_of_value_struct, seenObjects);

        foreach (var item in parsed.vector_of_table)
        {
            Assert.Contains(item, seenObjects);
        }

        foreach (var item in parsed.vector_of_ref_struct)
        {
            Assert.Contains(item, seenObjects);
        }
    }

    private byte[] CreateRoot(int value)
    {
        Root root = new()
        {
            inner_table = new InnerTable() { x = value },
            ref_struct = new RefStruct() { x = value, },
            value_struct = new ValueStruct() { x = value },
            vector_of_ref_struct = new[] { new RefStruct() { x = value }, new() { x = value } },
            vector_of_table = new[] { new InnerTable() { x = value }, new() { x = value } },
            vector_of_value_struct = new[] { new ValueStruct { x = value }, new() { x = value } },
        };

        byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
        Root.Serializer.Write(buffer, root);

        return buffer;
    }

    private static void VerifyRoot(Root item, int expectedValue)
    {
        Assert.Equal(expectedValue, item.inner_table.x);
        Assert.Equal(expectedValue, item.ref_struct.x);
        Assert.Equal(expectedValue, item.value_struct.Value.x);

        foreach (var temp in item.vector_of_ref_struct)
        {
            Assert.Equal(expectedValue, temp.x);
        }

        foreach (var temp in item.vector_of_table)
        {
            Assert.Equal(expectedValue, temp.x);
        }

        foreach (var temp in item.vector_of_value_struct)
        {
            Assert.Equal(expectedValue, temp.x);
        }
    }
}
