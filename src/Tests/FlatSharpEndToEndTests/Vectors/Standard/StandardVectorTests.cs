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

namespace FlatSharpEndToEndTests.Vectors.Standard;

[TestClass]
public class StandardVectorTests
{
    private static readonly string[] Strings = new[] { string.Empty, "a", "ab", "abc", "abcd", "abcde" };
    private static readonly byte[] Bytes = new byte[] { 1, 2, 3, 4, 5, 6 };
    private static readonly int[] Ints = new int[] { 1, 2, 3, 4, 5, 6 };

    #region Lazy

    [TestMethod]
    public void Lazy_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.IsNotNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.IsFalse(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.IsFalse(object.ReferenceEquals(list[5], list[5]));

        Assert.AreEqual(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Lazy_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.IsNotNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.IsFalse(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.IsFalse(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Lazy_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.IsNotNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.IsFalse(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.IsFalse(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
    }

    [TestMethod]
    public void Lazy_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Lazy_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Lazy_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Lazy_Unity_Pinned() => this.ValidateUnity(FlatBufferDeserializationOption.Lazy, true, true);

    [TestMethod]
    public void Lazy_Unity_NotPinned() => this.ValidateUnity_ExpectPinningError(FlatBufferDeserializationOption.Lazy);

    #endregion

    #region Progressive

    [TestMethod]
    public void Progressive_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        Assert.AreEqual(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Progressive_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.IsTrue(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Progressive_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
    }

    [TestMethod]
    public void Progressive_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Progressive_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Progressive_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.IsNotNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsTrue(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Progressive_Unity_Pinned() => this.ValidateUnity(FlatBufferDeserializationOption.Progressive, true, true);

    [TestMethod]
    public void Progressive_Unity_NotPinned() => this.ValidateUnity_ExpectPinningError(FlatBufferDeserializationOption.Progressive);

    #endregion

    #region Greedy

    [TestMethod]
    public void Greedy_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        Assert.AreEqual(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Greedy_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.IsTrue(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
        Assert.ThrowsException<NotMutableException>(() => list[0] = "foobar");
        Assert.ThrowsException<NotMutableException>(() => list.Clear());
    }

    [TestMethod]
    public void Greedy_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
    }

    [TestMethod]
    public void Greedy_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Greedy_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Greedy_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void Greedy_Unity_Pinned() => this.ValidateUnity(FlatBufferDeserializationOption.Greedy, true, false);

    [TestMethod]
    public void Greedy_Unity_NotPinned() => this.ValidateUnity(FlatBufferDeserializationOption.Greedy, false, false);

    #endregion

    #region GreedyMutable

    [TestMethod]
    public void GreedyMutable_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        Assert.AreEqual(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        list[0] = "foobar";
        list.Clear();
    }

    [TestMethod]
    public void GreedyMutable_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.IsTrue(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
        list[0] = "foobar";
        list.Clear();
    }

    [TestMethod]
    public void GreedyMutable_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.IsNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        Assert.IsTrue(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.IsTrue(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.AreEqual(Strings[i], list[i]);
        }

        Assert.AreEqual(Strings.Length, list.Count);
    }

    [TestMethod]
    public void GreedyMutable_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void GreedyMutable_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void GreedyMutable_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.IsNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.IsFalse(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.AreEqual(Bytes[i], memory.Span[i]);
        }

        Assert.AreEqual(Bytes.Length, memory.Length);
    }

    [TestMethod]
    public void GreedyMutable_Unity_Pinned() => this.ValidateUnity(FlatBufferDeserializationOption.GreedyMutable, true, false);

    [TestMethod]
    public void GreedyMutable_Unity_NotPinned() => this.ValidateUnity(FlatBufferDeserializationOption.GreedyMutable, false, false);

    #endregion

    private void ValidateUnity(FlatBufferDeserializationOption option, bool pin, bool expectOverlap)
    {
        this.SerializeAndParse(option, out _, out byte[] buffer);

        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        try
        {
            MemoryInputBuffer inputBuffer = new MemoryInputBuffer(buffer, isPinned: pin);

            var result = StandardVectorTable.Serializer.Parse(inputBuffer, option);
            Assert.IsNotNull(result.UnityNative);
            var nativeArray = result.UnityNative.Value;

            Assert.AreEqual(Ints.Length, nativeArray.Length);

            for (int i = 0; i < Ints.Length; ++i)
            {
                Assert.AreEqual(Ints[i], nativeArray[i]);
            }

            Assert.AreEqual(
                expectOverlap,
                MemoryMarshal.Cast<int, byte>(nativeArray.AsSpan()).Overlaps(buffer.AsSpan()));
        }
        finally
        {
            handle.Free();
        }
    }

    private void ValidateUnity_ExpectPinningError(FlatBufferDeserializationOption option)
    {
        var ex = Assert.ThrowsException<NotSupportedException>(() => this.ValidateUnity(option, false, false));
        Assert.AreEqual("Non-greedy parsing of a NativeArray requires a pinned buffer.", ex.Message);
    }

    private StandardVectorTable SerializeAndParse(FlatBufferDeserializationOption option, out IFlatBufferDeserializedObject obj, out byte[] inputBuffer)
    {
        inputBuffer = new StandardVectorTable
        {
            ExplicitMemory = Bytes,
            ImplicitMemory = Bytes,
            ReadOnlyMemory = Bytes,

            ExplicitStringList = Strings,
            ImplicitStringList = Strings,
            ReadOnlyStringList = Strings,

            UnityNative = new Unity.Collections.NativeArray<int>(Ints, Unity.Collections.Allocator.Temp)
        }.AllocateAndSerialize();

        var table = StandardVectorTable.Serializer.Parse(inputBuffer, option);

        obj = table as IFlatBufferDeserializedObject;

        Assert.IsNotNull(obj);
        Assert.AreEqual(option, obj.DeserializationContext.DeserializationOption);
        return table;
    }
}
