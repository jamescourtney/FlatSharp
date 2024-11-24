/*
 * Copyright 2024 James Courtney
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
using System.IO;

namespace FlatSharpEndToEndTests.ClassLib;

[TestClass]
public class BigSpanTests
{
    private delegate T SpanReadValue<T>(Span<byte> span) where T : unmanaged;
    private delegate T BigSpanReadValue<T>(BigSpan span) where T : unmanaged;

    private delegate void SpanWriteValue<T>(Span<byte> span, T value) where T : unmanaged;
    private delegate void BigSpanWriteValue<T>(BigSpan span, T value) where T : unmanaged;

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void ReadWrite_Bool(bool value) => TestReadWrite(
        s => s[0] != 0,
        bs => bs.ReadBool(0),
        (s, v) => s[0] = v ? (byte)1 : (byte)0,
        (bs, v) => bs.WriteBool(0, v), 
        value);

    [TestMethod]
    [DataRow((byte)0)]
    [DataRow((byte)1)]
    [DataRow(byte.MaxValue)]
    public void ReadWrite_Byte(byte value) => TestReadWrite(
        s => s[0],
        bs => bs.ReadByte(0),
        (s, v) => s[0] = v,
        (bs, v) => bs.WriteByte(0, v),
        value);

    [TestMethod]
    [DataRow(sbyte.MinValue)]
    [DataRow((sbyte)-1)]
    [DataRow((sbyte)0)]
    [DataRow((sbyte)1)]
    [DataRow(sbyte.MaxValue)]
    public void ReadWrite_SByte(sbyte value) => TestReadWrite(
        s => (sbyte)s[0],
        bs => bs.ReadSByte(0),
        (s, v) => s[0] = (byte)v,
        (bs, v) => bs.WriteSByte(0, v),
        value);

    [TestMethod]
    [DataRow(short.MinValue)]
    [DataRow((short)-1)]
    [DataRow((short)0)]
    [DataRow((short)1)]
    [DataRow(short.MaxValue)]
    public void ReadWrite_Short(short value) => TestReadWrite(
        s => BinaryPrimitives.ReadInt16LittleEndian(s),
        bs => bs.ReadShort(0),
        (s, v) => BinaryPrimitives.WriteInt16LittleEndian(s, v),
        (bs, v) => bs.WriteShort(0, v),
        value);

    [TestMethod]
    [DataRow(ushort.MinValue)]
    [DataRow((ushort)1)]
    [DataRow(ushort.MaxValue)]
    public void ReadWrite_UShort(ushort value) => TestReadWrite(
        s => BinaryPrimitives.ReadUInt16LittleEndian(s),
        bs => bs.ReadUShort(0),
        (s, v) => BinaryPrimitives.WriteUInt16LittleEndian(s, v),
        (bs, v) => bs.WriteUShort(0, v),
        value);

    [TestMethod]
    [DataRow(int.MinValue)]
    [DataRow((int)-1)]
    [DataRow((int)0)]
    [DataRow((int)1)]
    [DataRow(int.MaxValue)]
    public void ReadWrite_Int(int value) => TestReadWrite(
        s => BinaryPrimitives.ReadInt32LittleEndian(s),
        bs => bs.ReadInt(0),
        (s, v) => BinaryPrimitives.WriteInt32LittleEndian(s, v),
        (bs, v) => bs.WriteInt(0, v),
        value);

    [TestMethod]
    [DataRow(uint.MinValue)]
    [DataRow((uint)1)]
    [DataRow(uint.MaxValue)]
    public void ReadWrite_UInt(uint value) => TestReadWrite(
        s => BinaryPrimitives.ReadUInt32LittleEndian(s),
        bs => bs.ReadUInt(0),
        (s, v) => BinaryPrimitives.WriteUInt32LittleEndian(s, v),
        (bs, v) => bs.WriteUInt(0, v),
        value);
    
    [TestMethod]
    [DataRow(long.MinValue)]
    [DataRow((long)-1)]
    [DataRow((long)0)]
    [DataRow((long)1)]
    [DataRow(long.MaxValue)]
    public void ReadWrite_Long(long value) => TestReadWrite(
        s => BinaryPrimitives.ReadInt64LittleEndian(s),
        bs => bs.ReadLong(0),
        (s, v) => BinaryPrimitives.WriteInt64LittleEndian(s, v),
        (bs, v) => bs.WriteLong(0, v),
        value);

    [TestMethod]
    [DataRow(ulong.MinValue)]
    [DataRow((ulong)1)]
    [DataRow(ulong.MaxValue)]
    public void ReadWrite_ULong(ulong value) => TestReadWrite(
        s => BinaryPrimitives.ReadUInt64LittleEndian(s),
        bs => bs.ReadULong(0),
        (s, v) => BinaryPrimitives.WriteUInt64LittleEndian(s, v),
        (bs, v) => bs.WriteULong(0, v),
        value);

#if !AOT
    [TestMethod]
    [DataRow(float.MinValue)]
    [DataRow(-1f)]
    [DataRow(0f)]
    [DataRow(1f)]
    [DataRow(float.MaxValue)]
    public void ReadWrite_Float(float value) => TestReadWrite(
        s => BinaryPrimitives.ReadSingleLittleEndian(s),
        bs => bs.ReadFloat(0),
        (s, v) => BinaryPrimitives.WriteSingleLittleEndian(s, v),
        (bs, v) => bs.WriteFloat(0, v),
        value);
#endif

    [TestMethod]
    [DataRow(double.MinValue)]
    [DataRow(-1d)]
    [DataRow(0d)]
    [DataRow(1d)]
    [DataRow(double.MaxValue)]
    public void ReadWrite_Double(double value) => TestReadWrite(
        s => BinaryPrimitives.ReadDoubleLittleEndian(s),
        bs => bs.ReadDouble(0),
        (s, v) => BinaryPrimitives.WriteDoubleLittleEndian(s, v),
        (bs, v) => bs.WriteDouble(0, v),
        value);

    private void TestReadWrite<T>(
        SpanReadValue<T> spanRead,
        BigSpanReadValue<T> bigSpanRead,
        SpanWriteValue<T> spanWrite,
        BigSpanWriteValue<T> bigSpanWrite,
        T value)
        where T : unmanaged
    {
        int size = Unsafe.SizeOf<T>();

        Span<byte> empty = stackalloc byte[size];
        empty.Clear();

        Span<byte> fullSpan = stackalloc byte[3 * size];

        Span<byte> span = fullSpan.Slice(size, size);
        BigSpan bigSpan = new(span);

        span.Fill(0);
        Assert.AreEqual(bigSpanRead(bigSpan), spanRead(span));

        spanWrite(span, value);
        Assert.AreEqual(value, bigSpanRead(bigSpan));
        Assert.AreEqual(value, spanRead(span));

        span.Fill(0);
        bigSpanWrite(bigSpan, value);

        Assert.AreEqual(value, spanRead(span));
        Assert.AreEqual(value, bigSpanRead(bigSpan));

        // Ensure the guard bytes were not written.
        Assert.IsTrue(empty.SequenceEqual(fullSpan.Slice(0, size)));
        Assert.IsTrue(empty.SequenceEqual(fullSpan.Slice(2 * size, size)));
    }
}
