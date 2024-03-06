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

using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using FlatSharp.Internal;

namespace FlatSharpEndToEndTests.IO.InputBufferTests;

[TestClass]
public class InputBufferTests
{
    private const string StringData = "how now brown cow";

    private static readonly Random Random = new Random();
    private static readonly byte[] Input = new byte[100 * 8];
    private static readonly byte[] StringInput;

    static InputBufferTests()
    {
        Random.NextBytes(Input);

        Input[0] = 0;
        Input[1] = 1;

        byte[] stringData = new byte[1024];
        int byteCount = Encoding.UTF8.GetBytes(StringData, 0, StringData.Length, stringData, 8);

        stringData[0] = 4;
        stringData[4] = (byte)byteCount;

        StringInput = stringData;
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void SerializationInvocations(FlatBufferDeserializationOption option)
    {
        var serializer = PrimitiveTypesTable.Serializer.WithSettings(settings => settings.UseDeserializationMode(option));

        byte[] data = new byte[1024];
        serializer.Write(data, new PrimitiveTypesTable());

        IInputBuffer[] inputBuffers = new IInputBuffer[]
        {
            new ArrayInputBuffer(data),
            new ArraySegmentInputBuffer(new ArraySegment<byte>(data)),
            new MemoryInputBuffer(data),
            new ReadOnlyMemoryInputBuffer(data),
        };

        foreach (IInputBuffer buffer in inputBuffers)
        {
            var item = serializer.Parse(buffer);

            if (item is IFlatBufferDeserializedObject obj)
            {
                Assert.AreEqual(option, obj.DeserializationContext.DeserializationOption);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }
    }

    [TestMethod]
    public void MemoryInputBuffer()
    {
        this.InputBufferTest(new MemoryInputBuffer(Input));
        this.StringInputBufferTest(new MemoryInputBuffer(StringInput));
        this.TestDeserializeBoth(b => new MemoryInputBuffer(b));
        this.TestReadByteArray(false, b => new MemoryInputBuffer(b));
        this.TableSerializationTest(
            default(SpanWriter),
            (s, b) => s.Parse(b.AsMemory()));
    }

    [TestMethod]
    public void ReadOnlyMemoryInputBuffer()
    {
        this.InputBufferTest(new ReadOnlyMemoryInputBuffer(Input));
        this.StringInputBufferTest(new ReadOnlyMemoryInputBuffer(StringInput));

        this.TestDeserialize<ReadOnlyMemoryInputBuffer, ReadOnlyMemoryTable>(b => new ReadOnlyMemoryInputBuffer(b));
        this.TestReadByteArray(true, b => new ReadOnlyMemoryInputBuffer(b));
        var ex = Assert.ThrowsException<InvalidOperationException>(
            () => this.TestDeserialize<ReadOnlyMemoryInputBuffer, MemoryTable>(b => new ReadOnlyMemoryInputBuffer(b)));
        Assert.AreEqual("ReadOnlyMemory inputs may not deserialize writable memory.", ex.Message);

        this.TableSerializationTest(
            default(SpanWriter),
            (s, b) => s.Parse((ReadOnlyMemory<byte>)b.AsMemory()));
    }

    [TestMethod]
    public void ArrayInputBuffer()
    {
        this.InputBufferTest(new ArrayInputBuffer(Input));
        this.StringInputBufferTest(new ArrayInputBuffer(StringInput));
        this.TestDeserializeBoth(b => new ArrayInputBuffer(b));
        this.TestReadByteArray(false, b => new ArrayInputBuffer(b));
        this.TableSerializationTest(
            default(SpanWriter),
            (s, b) => s.Parse(b));
    }

    [TestMethod]
    public void ArraySegmentInputBuffer()
    {
        this.InputBufferTest(new ArraySegmentInputBuffer(new ArraySegment<byte>(Input)));
        this.StringInputBufferTest(new ArraySegmentInputBuffer(new ArraySegment<byte>(StringInput)));
        this.TestDeserializeBoth(b => new ArraySegmentInputBuffer(new ArraySegment<byte>(b)));
        this.TestReadByteArray(false, b => new ArraySegmentInputBuffer(new ArraySegment<byte>(b)));
        this.TableSerializationTest(
            default(SpanWriter),
            (s, b) => s.Parse(new ArraySegment<byte>(b)));
    }

    private void TableSerializationTest(
        ISpanWriter writer,
        Func<ISerializer<PrimitiveTypesTable>, byte[], PrimitiveTypesTable> parse)
    {
        PrimitiveTypesTable original = new()
        {
            A = true,
            B = 1,
            C = 2,
            D = 3,
            E = 4,
            F = 5,
            G = 6,
            H = 7,
            I = 8,
            J = 9.1f,
            K = 10.2,
            L = "foobar",
            M = "foobar2",
        };

        byte[] data = new byte[1024];
        PrimitiveTypesTable.Serializer.Write(writer, data, original);
        PrimitiveTypesTable parsed = parse(PrimitiveTypesTable.Serializer, data);

        Assert.AreEqual(original.A, parsed.A);
        Assert.AreEqual(original.B, parsed.B);
        Assert.AreEqual(original.C, parsed.C);
        Assert.AreEqual(original.D, parsed.D);
        Assert.AreEqual(original.E, parsed.E);
        Assert.AreEqual(original.F, parsed.F);
        Assert.AreEqual(original.G, parsed.G);
        Assert.AreEqual(original.H, parsed.H);
        Assert.AreEqual(original.I, parsed.I);
        Assert.AreEqual(original.J, parsed.J);
        Assert.AreEqual(original.K, parsed.K);
        Assert.AreEqual(original.L, parsed.L);
        Assert.AreEqual(original.M, parsed.M);
    }

    private void InputBufferTest(IInputBuffer ib)
    {
        var mem = new Memory<byte>(Input);

        Assert.IsFalse(ib.ReadBool(0));
        Assert.IsTrue(ib.ReadBool(1));

        this.CompareEqual<byte>(sizeof(byte), i => mem.Span[i], i => ib.ReadByte(i));
        this.CompareEqual<sbyte>(sizeof(sbyte), i => (sbyte)mem.Span[i], i => ib.ReadSByte(i));

        this.CompareEqual<ushort>(sizeof(ushort), i => BinaryPrimitives.ReadUInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadUShort(i));
        this.CompareEqual<short>(sizeof(short), i => BinaryPrimitives.ReadInt16LittleEndian(mem.Span.Slice(i)), i => ib.ReadShort(i));

        this.CompareEqual<uint>(sizeof(uint), i => BinaryPrimitives.ReadUInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadUInt(i));
        this.CompareEqual<int>(sizeof(int), i => BinaryPrimitives.ReadInt32LittleEndian(mem.Span.Slice(i)), i => ib.ReadInt(i));

#if NETCOREAPP2_1_OR_GREATER
        this.CompareEqual<float>(sizeof(float), i => BitConverter.ToSingle(mem.Span.Slice(i)), i => ib.ReadFloat(i));
#endif

        this.CompareEqual<ulong>(sizeof(ulong), i => BinaryPrimitives.ReadUInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadULong(i));
        this.CompareEqual<long>(sizeof(long), i => BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i)), i => ib.ReadLong(i));
        this.CompareEqual<double>(sizeof(double), i => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(mem.Span.Slice(i))), i => ib.ReadDouble(i));
    }

    private void StringInputBufferTest(IInputBuffer ib)
    {
        Assert.AreEqual(StringData, ib.ReadString(0));
    }

    private void CompareEqual<T>(
        int size,
        Func<int, T> readMemoryAtIndex,
        Func<int, T> readAtIndex)
    {
        for (int i = 0; i < Input.Length - size; i += size)
        {
            var memory = readMemoryAtIndex(i);
            var bufferValue = readAtIndex(i);
            Assert.AreEqual(memory, bufferValue);
        }
    }

    private void TestDeserializeBoth<TBuffer>(Func<byte[], TBuffer> bufferBuilder) where TBuffer : IInputBuffer
    {
        this.TestDeserialize<TBuffer, ReadOnlyMemoryTable>(bufferBuilder);
        this.TestDeserialize<TBuffer, MemoryTable>(bufferBuilder);
    }

    private void TestReadByteArray<TBuffer>(bool isReadOnly, Func<byte[], TBuffer> bufferBuilder) where TBuffer : IInputBuffer
    {
        byte[] buffer = new byte[] { 4, 0, 0, 0, 7, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 };
        TBuffer inputBuffer = bufferBuilder(buffer);

        byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, };

        Assert.IsTrue(inputBuffer.ReadByteReadOnlyMemoryBlock(0).Span.SequenceEqual(expected));
        Assert.AreEqual(isReadOnly, inputBuffer.IsReadOnly);
        
        Assert.IsTrue(inputBuffer.GetReadOnlySpan().SequenceEqual(buffer));
        Assert.IsTrue(inputBuffer.GetReadOnlyMemory().Span.SequenceEqual(buffer));

        if (isReadOnly)
        {
            Assert.ThrowsException<InvalidOperationException>(() => inputBuffer.GetSpan());
            Assert.ThrowsException<InvalidOperationException>(() => inputBuffer.GetMemory());
        }
        else
        {
            Assert.IsTrue(inputBuffer.GetMemory().Span.SequenceEqual(buffer));
            Assert.IsTrue(inputBuffer.GetSpan().SequenceEqual(buffer));
            Assert.IsTrue(inputBuffer.ReadByteMemoryBlock(0).Span.SequenceEqual(expected));
        }
    }

    private void TestDeserialize<TBuffer, TType>(Func<byte[], TBuffer> bufferBuilder)
        where TBuffer : IInputBuffer
        where TType : class, IMemoryTable, IFlatBufferSerializable<TType>, new()
    {
        TType table = new TType
        {
            Memory = new byte[] { 6, 7, 8, 9, 10 }
        };

        byte[] dest = new byte[1024];
        table.Serializer.Write(dest, table);

        TBuffer buffer = bufferBuilder(dest);
        var parsed = table.Serializer.Parse(buffer);
        for (int i = 1; i <= 5; ++i)
        {
            Assert.AreEqual(i + 5, parsed.Memory.Span[i - 1]);
        }
    }
}

internal interface IMemoryTable
{
    ReadOnlyMemory<byte> Memory { get; set; }
}

public partial class MemoryTable : IMemoryTable
{
    ReadOnlyMemory<byte> IMemoryTable.Memory { get => this.Memory.Value; set => this.Memory = MemoryMarshal.AsMemory(value); }
}

public partial class ReadOnlyMemoryTable : IMemoryTable
{
    ReadOnlyMemory<byte> IMemoryTable.Memory { get => this.Memory.Value; set => this.Memory = value; }
}

