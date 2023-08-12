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

using FlatSharp;
using FlatSharp.Internal;
using FlatSharpEndToEndTests.ClassLib.FlatBufferSerializerNonGenericTests;
using System.Text;

namespace FlatSharpEndToEndTests.ClassLib;

public class GeneratedSerializerWrapperTests
{
    [Fact]
    public void Serialize_DestinationBuffer_TooShort()
    {
        FlatBufferSerializerNonGenericTests.SomeTable table = new();
        table.A = 3;

        var serializer = FlatBufferSerializerNonGenericTests.SomeTable.Serializer;

        byte[] destination = new byte[7];
        Assert.Throws<BufferTooSmallException>(() => serializer.Write(destination, table));
    }

    [Fact]
    public void Parse_SourceBuffer_TooShort()
    {
        var ex = Assert.Throws<ArgumentException>(() => FlatBufferSerializerNonGenericTests.SomeTable.Serializer.Parse(new byte[7]));
        Assert.Equal("Buffer is too small to be valid!", ex.Message);
    }

    [Theory]
    [InlineData(int.MaxValue / 2)]
    [InlineData(int.MaxValue)]
    public void Parse_SourceBuffer_TooLong(int length)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => FlatBufferSerializerNonGenericTests.SomeTable.Serializer.Parse(new FakeInputBuffer(length)));
        Assert.Contains("Buffer must be <= 1GB in size.", ex.Message);
    }

    [Fact]
    public void Parse_UnknownMode()
    {
        SomeTable someTable = new();
        byte[] buffer = someTable.AllocateAndSerialize();

        var ex = Assert.Throws<InvalidOperationException>(() => SomeTable.Serializer.Parse(buffer, (FlatBufferDeserializationOption)36));
        Assert.Contains("Unexpected deserialization mode: ", ex.Message);
    }

    [Fact]
    public void Serialize_RootTable_Null()
    {
        var serializer = FlatBufferSerializerNonGenericTests.SomeTable.Serializer;

        byte[] destination = new byte[7];
        var ex = Assert.Throws<ArgumentNullException>(() => serializer.Write(destination, null));
        Assert.Contains("The root table may not be null.", ex.Message);
    }

    [Fact]
    public void GeneratedSerializer_GetMaxSize_Null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => FlatBufferSerializerNonGenericTests.SomeTable.Serializer.GetMaxSize(null!));
        Assert.Equal("The root table may not be null. (Parameter 'item')", ex.Message);
    }

    private class FakeInputBuffer : IInputBuffer
    {
        public FakeInputBuffer(int size)
        {
            this.Length = size;
        }

        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsPinned => throw new NotImplementedException();

        public int Length { get; private set; }

        public Memory<byte> GetMemory()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyMemory<byte> GetReadOnlyMemory()
        {
            throw new NotImplementedException();
        }

        public ReadOnlySpan<byte> GetReadOnlySpan()
        {
            throw new NotImplementedException();
        }

        public Span<byte> GetSpan()
        {
            throw new NotImplementedException();
        }

        public TItem InvokeGreedyMutableParse<TItem>(IGeneratedSerializer<TItem> serializer, in GeneratedSerializerParseArguments arguments)
        {
            throw new NotImplementedException();
        }

        public TItem InvokeGreedyParse<TItem>(IGeneratedSerializer<TItem> serializer, in GeneratedSerializerParseArguments arguments)
        {
            throw new NotImplementedException();
        }

        public TItem InvokeLazyParse<TItem>(IGeneratedSerializer<TItem> serializer, in GeneratedSerializerParseArguments arguments)
        {
            throw new NotImplementedException();
        }

        public TItem InvokeProgressiveParse<TItem>(IGeneratedSerializer<TItem> serializer, in GeneratedSerializerParseArguments arguments)
        {
            throw new NotImplementedException();
        }

        public byte ReadByte(int offset)
        {
            throw new NotImplementedException();
        }

        public double ReadDouble(int offset)
        {
            throw new NotImplementedException();
        }

        public float ReadFloat(int offset)
        {
            throw new NotImplementedException();
        }

        public int ReadInt(int offset)
        {
            throw new NotImplementedException();
        }

        public long ReadLong(int offset)
        {
            throw new NotImplementedException();
        }

        public sbyte ReadSByte(int offset)
        {
            throw new NotImplementedException();
        }

        public short ReadShort(int offset)
        {
            throw new NotImplementedException();
        }

        public string ReadString(int offset, int byteLength, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt(int offset)
        {
            throw new NotImplementedException();
        }

        public ulong ReadULong(int offset)
        {
            throw new NotImplementedException();
        }

        public ushort ReadUShort(int offset)
        {
            throw new NotImplementedException();
        }
    }
}
