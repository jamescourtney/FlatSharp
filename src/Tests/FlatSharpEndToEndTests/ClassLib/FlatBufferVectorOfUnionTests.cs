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

namespace FlatSharpEndToEndTests.ClassLib.FlatBufferVectorOfUnionTests;

/// <summary>
/// Tests for FlatSharp's IList of Union.
/// </summary>
[TestClass]
public class FlatBufferVectorOfUnionTests
{
    private TableVector vector;

    public FlatBufferVectorOfUnionTests()
    {
        var original = new TableVector
        {
            Vector = new[]
            {
                new MyUnion("foobar"),
                new MyUnion(new Struct { Value = 6 }),
                new MyUnion(new Table { Value = 5 }),
            }
        };

        this.vector = original.SerializeAndParse(FlatBufferDeserializationOption.Lazy);
    }

    [TestMethod]
    public void FlatBufferVector_OutOfRange()
    {
        Assert.ThrowsException<IndexOutOfRangeException>(() => this.vector.Vector[-1]);
        Assert.ThrowsException<IndexOutOfRangeException>(() => this.vector.Vector[5]);
    }

    [TestMethod]
    public void FlatBufferVector_NotMutable()
    {
        Assert.IsTrue(this.vector.Vector.IsReadOnly);
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector[0] = new MyUnion("foobar"));
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector.Add(new MyUnion("foobar")));
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector.Clear());
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector.Insert(0, new MyUnion("foobar")));
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector.Remove(new MyUnion("foobar")));
        Assert.ThrowsException<NotMutableException>(() => this.vector.Vector.RemoveAt(0));
    }

    [TestMethod]
    public void FlatBufferVector_GetEnumerator()
    {
        int i = 0;
        foreach (var item in this.vector.Vector)
        {
            Assert.AreEqual(i + 1, item.Discriminator);
            i++;
        }
    }

    [TestMethod]
    public void FlatBufferVector_Contains()
    {
        Assert.IsTrue(this.vector.Vector.Contains(new MyUnion("foobar")));
        Assert.IsFalse(this.vector.Vector.Contains(new MyUnion("blah")));
    }

    [TestMethod]
    public void FlatBufferVector_IndexOf()
    {
        Assert.AreEqual(0, this.vector.Vector.IndexOf(new MyUnion("foobar")));
        Assert.AreEqual(-1, this.vector.Vector.IndexOf(new MyUnion("monster")));
    }
}
