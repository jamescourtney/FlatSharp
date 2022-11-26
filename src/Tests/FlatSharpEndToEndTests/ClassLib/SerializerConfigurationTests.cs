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

using FlatSharp.Internal;
using Microsoft.CodeAnalysis.Options;
using System.IO;

namespace FlatSharpEndToEndTests.ClassLib.SerializerConfigurationTests;

public class SerializerConfigurationTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void UseDeserializationMode(FlatBufferDeserializationOption option)
    {
        var item = new Root { StringVector = new string[] { "a", "b", "c" } };

        ISerializer<Root> serializer = Root.Serializer.WithSettings(opt => opt.UseDeserializationMode(option));

        Root parsed = item.SerializeAndParse(serializer);
        IFlatBufferDeserializedObject obj = parsed as IFlatBufferDeserializedObject;
        Assert.Equal(option, obj.DeserializationContext.DeserializationOption);
    }

    [Fact]
    public void UseLazyDeserialization()
    {
        var item = new Root { StringVector = new string[] { "a", "b", "c" } };

        ISerializer<Root> serializer = Root.Serializer.WithSettings(opt => opt.UseLazyDeserialization());

        Root parsed = item.SerializeAndParse(serializer);
        IFlatBufferDeserializedObject obj = parsed as IFlatBufferDeserializedObject;
        Assert.Equal(FlatBufferDeserializationOption.Lazy, obj.DeserializationContext.DeserializationOption);
    }

    [Fact]
    public void UseProgressiveDeserialization()
    {
        var item = new Root { StringVector = new string[] { "a", "b", "c" } };

        ISerializer<Root> serializer = Root.Serializer.WithSettings(opt => opt.UseProgressiveDeserialization());

        Root parsed = item.SerializeAndParse(serializer);
        IFlatBufferDeserializedObject obj = parsed as IFlatBufferDeserializedObject;
        Assert.Equal(FlatBufferDeserializationOption.Progressive, obj.DeserializationContext.DeserializationOption);
    }

    [Fact]
    public void UseGreedyDeserialization()
    {
        var item = new Root { StringVector = new string[] { "a", "b", "c" } };

        ISerializer<Root> serializer = Root.Serializer.WithSettings(opt => opt.UseGreedyDeserialization());

        Root parsed = item.SerializeAndParse(serializer);
        IFlatBufferDeserializedObject obj = parsed as IFlatBufferDeserializedObject;
        Assert.Equal(FlatBufferDeserializationOption.Greedy, obj.DeserializationContext.DeserializationOption);
    }

    [Fact]
    public void UseGreedyMutableDeserialization()
    {
        var item = new Root { StringVector = new string[] { "a", "b", "c" } };

        ISerializer<Root> serializer = Root.Serializer.WithSettings(opt => opt.UseGreedyMutableDeserialization());

        Root parsed = item.SerializeAndParse(serializer);
        IFlatBufferDeserializedObject obj = parsed as IFlatBufferDeserializedObject;
        Assert.Equal(FlatBufferDeserializationOption.GreedyMutable, obj.DeserializationContext.DeserializationOption);
    }

    [Theory]
    [InlineData(FlatBufferDeserializationOption.Lazy, false, false)]
    [InlineData(FlatBufferDeserializationOption.Lazy, true, true)]
    [InlineData(FlatBufferDeserializationOption.Progressive, false, false)]
    [InlineData(FlatBufferDeserializationOption.Progressive, true, true)]
    [InlineData(FlatBufferDeserializationOption.Greedy, false, false)]
    [InlineData(FlatBufferDeserializationOption.Greedy, true, false)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable, false, false)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable, true, false)]
    public void UseMemoryCopySerialization(FlatBufferDeserializationOption option, bool enableMemCopy, bool expectMemCopy)
    {
        Root t = new() { StringVector = new[] { "A", "b", "c", } };

        var compiled = Root.Serializer.WithSettings(s => s.UseMemoryCopySerialization(enableMemCopy).UseDeserializationMode(option));

        // overallocate
        byte[] data = new byte[1024];

        int maxBytes = compiled.GetMaxSize(t);
        Assert.Equal(88, maxBytes);
        int actualBytes = compiled.Write(data, t);
        Assert.Equal(58, actualBytes);

        // First test: Parse the array but don't trim the buffer. This causes the underlying
        // buffer to be much larger than the actual data.
        var parsed = compiled.Parse(data);
        byte[] data2 = new byte[2048];
        int bytesWritten = compiled.Write(data2, parsed);

        if (expectMemCopy)
        {
            Assert.Equal(1024, bytesWritten); // We use mem copy serialization here, and we gave it 1024 bytes when we parsed. So we get 1024 bytes back out.
            Assert.Equal(1024, compiled.GetMaxSize(parsed));
            Assert.Throws<BufferTooSmallException>(() => compiled.Write(new byte[maxBytes], parsed));

            // Repeat, but now using the trimmed array.
            parsed = compiled.Parse(data.AsMemory().Slice(0, actualBytes));
            bytesWritten = compiled.Write(data2, parsed);
            Assert.Equal(58, bytesWritten);
            Assert.Equal(58, compiled.GetMaxSize(parsed));

            // Default still returns 88.
            Assert.Equal(88, Root.Serializer.GetMaxSize(parsed));
        }
        else
        {
            Assert.Equal(58, bytesWritten); 
            Assert.Equal(88, compiled.GetMaxSize(parsed));
            compiled.Write(new byte[maxBytes], parsed);
        }
    }

    [Fact]
    public void SharedStringWriters()
    {
        ISerializer<Root> noSharedStrings = Root.Serializer.WithSettings(opt => opt.DisableSharedStrings());
        ISerializer<Root> smallHashSharedStrings = Root.Serializer.WithSettings(opt => opt.UseDefaultSharedStringWriter(1));
        ISerializer<Root> largeHashSharedStrings = Root.Serializer.WithSettings(opt => opt.UseDefaultSharedStringWriter());
        ISerializer<Root> perfectSharedStrings = Root.Serializer.WithSettings(opt => opt.UseSharedStringWriter<PerfectSharedStringWriter>());

        {
            Root root = new Root { StringVector = new[] { "a", "b", "a", "b", "a", "b", "a", } };

            byte[] notShared = root.AllocateAndSerialize(noSharedStrings);
            byte[] smallShared = root.AllocateAndSerialize(smallHashSharedStrings);
            byte[] largeShared = root.AllocateAndSerialize(largeHashSharedStrings);
            byte[] perfectShared = root.AllocateAndSerialize(perfectSharedStrings);

            // Small shared doesn't accomplish anything since it alternates.
            Assert.Equal(notShared.Length, smallShared.Length);
            Assert.True(largeShared.Length < smallShared.Length);
            Assert.Equal(largeShared.Length, perfectShared.Length);
        }

        {
            Root root = new Root { StringVector = new[] { "a", "a", "a", "a", "b", "b", "b", "b" } };

            byte[] notShared = root.AllocateAndSerialize(noSharedStrings);
            byte[] smallShared = root.AllocateAndSerialize(smallHashSharedStrings);
            byte[] largeShared = root.AllocateAndSerialize(largeHashSharedStrings);
            byte[] perfectShared = root.AllocateAndSerialize(perfectSharedStrings);

            // Small shared doesn't accomplish anything since it alternates.
            Assert.True(smallShared.Length < notShared.Length);
            Assert.Equal(largeShared.Length, smallShared.Length);
            Assert.Equal(largeShared.Length, perfectShared.Length);
        }
    }

    private class PerfectSharedStringWriter : ISharedStringWriter
    {
        private readonly Dictionary<string, List<int>> offsets = new();

        public PerfectSharedStringWriter()
        {
        }

        public bool IsDirty => this.offsets.Count > 0;

        public void FlushWrites<TSpanWriter>(TSpanWriter writer, Span<byte> data, SerializationContext context) where TSpanWriter : ISpanWriter
        {
            foreach (var kvp in this.offsets)
            {
                string value = kvp.Key;

                int stringOffset = writer.WriteAndProvisionString(data, value, context);

                for (int i = 0; i < kvp.Value.Count; ++i)
                {
                    writer.WriteUOffset(data, kvp.Value[i], stringOffset);
                }
            }

            this.offsets.Clear();
        }

        public void Reset()
        {
            this.offsets.Clear();
        }

        public void WriteSharedString<TSpanWriter>(
            TSpanWriter spanWriter,
            Span<byte> data,
            int offset,
            string value,
            SerializationContext context) where TSpanWriter : ISpanWriter
        {
            if (!this.offsets.TryGetValue(value, out var list))
            {
                list = new();
                this.offsets[value] = list;
            }

            list.Add(offset);
        }
    }
}
