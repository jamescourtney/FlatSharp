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
using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Vectors.Standard;

public class StandardVectorTests
{
    private static readonly string[] Strings = new[] { string.Empty, "a", "ab", "abc", "abcd", "abcde" };
    private static readonly byte[] Bytes = new byte[] { 1, 2, 3, 4, 5, 6 };

    #region Lazy

    [Fact]
    public void Lazy_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.NotNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.False(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.False(object.ReferenceEquals(list[5], list[5]));

        Assert.Equal(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Lazy_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.NotNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.False(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.False(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Lazy_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out _);
        Assert.NotNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("FlatBufferVectorBase", list.GetType().FullName);

        // lazy returns unique instances.
        Assert.False(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.False(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
    }

    [Fact]
    public void Lazy_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Lazy_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Lazy_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Lazy, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Lazy_Unity_Native_Pinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.Lazy, true);

    [Fact]
    public void Lazy_Unity_Native_NotPinned()
    {
        var ex = Assert.Throws<NotSupportedException>(() => this.UnityNative_Validate(FlatBufferDeserializationOption.Lazy, false));
        Assert.Equal("Non-greedy parsing of a NativeArray requires a pinned buffer.", ex.Message);
    }

    #endregion

    #region Progressive

    [Fact]
    public void Progressive_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        Assert.Equal(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Progressive_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.True(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Progressive_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("FlatBufferProgressiveVector", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
    }

    [Fact]
    public void Progressive_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Progressive_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Progressive_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Progressive, out var obj, out var inputBuffer);
        Assert.NotNull(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.True(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Progressive_Unity_Native_Pinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.Progressive, true);

    [Fact]
    public void Progressive_Unity_Native_NotPinned()
    {
        var ex = Assert.Throws<NotSupportedException>(() => this.UnityNative_Validate(FlatBufferDeserializationOption.Progressive, false));
        Assert.Equal("Non-greedy parsing of a NativeArray requires a pinned buffer.", ex.Message);
    }

    #endregion

    #region Greedy

    [Fact]
    public void Greedy_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        Assert.Equal(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Greedy_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.True(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
        Assert.Throws<NotMutableException>(() => list[0] = "foobar");
        Assert.Throws<NotMutableException>(() => list.Clear());
    }

    [Fact]
    public void Greedy_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("ImmutableList", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
    }

    [Fact]
    public void Greedy_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Greedy_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Greedy_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.Greedy, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void Greedy_Unity_Native_Pinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.Greedy, true);

    [Fact]
    public void Greedy_Unity_Native_NotPinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.Greedy, false);

    #endregion

    #region GreedyMutable

    [Fact]
    public void GreedyMutable_String_IList_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IList<string> list = table.ImplicitStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ImplicitStringList, table.ImplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        Assert.Equal(Strings.Length, list.Count);

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        list[0] = "foobar";
        list.Clear();
    }

    [Fact]
    public void GreedyMutable_String_IList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IList<string> list = table.ExplicitStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        // Progressive returns the same instance.
        Assert.True(object.ReferenceEquals(table.ExplicitStringList, table.ExplicitStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
        list[0] = "foobar";
        list.Clear();
    }

    [Fact]
    public void GreedyMutable_String_IReadOnlyList_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out _);
        Assert.Null(obj.InputBuffer);

        IReadOnlyList<string> list = table.ReadOnlyStringList;
        //Assert.Contains("PoolableList", list.GetType().FullName);

        Assert.True(object.ReferenceEquals(table.ReadOnlyStringList, table.ReadOnlyStringList));
        Assert.True(object.ReferenceEquals(list[5], list[5]));

        for (int i = 0; i < Strings.Length; ++i)
        {
            Assert.Equal(Strings[i], list[i]);
        }

        Assert.Equal(Strings.Length, list.Count);
    }

    [Fact]
    public void GreedyMutable_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ExplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void GreedyMutable_Memory_Implicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        Memory<byte>? nullableMemory = table.ImplicitMemory;
        Memory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void GreedyMutable_ReadOnly_Memory_Explicit()
    {
        var table = this.SerializeAndParse(FlatBufferDeserializationOption.GreedyMutable, out var obj, out var inputBuffer);
        Assert.Null(obj.InputBuffer);

        ReadOnlyMemory<byte>? nullableMemory = table.ReadOnlyMemory;
        ReadOnlyMemory<byte> memory = nullableMemory.Value;

        // Each span overlaps the input buffer. Means we are not eagerly copying out.
        Assert.False(memory.Span.Overlaps(inputBuffer));

        for (int i = 0; i < Bytes.Length; ++i)
        {
            Assert.Equal(Bytes[i], memory.Span[i]);
        }

        Assert.Equal(Bytes.Length, memory.Length);
    }

    [Fact]
    public void GreedyMutable_Unity_Native_Pinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.GreedyMutable, true);

    [Fact]
    public void GreedyMutable_Unity_Native_NotPinned() => this.UnityNative_Validate(FlatBufferDeserializationOption.GreedyMutable, false);

    #endregion

    private StandardVectorTable SerializeAndParse(FlatBufferDeserializationOption option, out IFlatBufferDeserializedObject obj, out byte[] inputBuffer)
    {
        inputBuffer = new StandardVectorTable
        {
            ExplicitMemory = Bytes,
            ImplicitMemory = Bytes,
            ReadOnlyMemory = Bytes,
            UnityNative = new Unity.Collections.NativeArray<byte>(Bytes, Unity.Collections.Allocator.None),

            ExplicitStringList = Strings,
            ImplicitStringList = Strings,
            ReadOnlyStringList = Strings,
        }.AllocateAndSerialize();

        var table = StandardVectorTable.Serializer.Parse(inputBuffer, option);

        obj = table as IFlatBufferDeserializedObject;

        Assert.NotNull(obj);
        Assert.Equal(option, obj.DeserializationContext.DeserializationOption);
        return table;
    }

    private void UnityNative_Validate(FlatBufferDeserializationOption option, bool pin)
    {
        var table = this.SerializeAndParse(option, out var obj, out var inputBuffer);

        GCHandle handle = GCHandle.Alloc(inputBuffer, GCHandleType.Pinned);
        try
        {
            MemoryInputBuffer ib = new MemoryInputBuffer(inputBuffer, pin);

            table = StandardVectorTable.Serializer.Parse(ib, option);

            Unity.Collections.NativeArray<byte>? nullableUnity = table.UnityNative;
            var unity = nullableUnity.Value;

            for (int i = 0; i < Bytes.Length; ++i)
            {
                Assert.Equal(Bytes[i], unity[i]);
            }
        }
        finally
        {
            handle.Free();
        }
    }
}
