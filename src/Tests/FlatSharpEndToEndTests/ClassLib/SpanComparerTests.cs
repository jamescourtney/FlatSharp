/*
 * Copyright 2020 James Courtney
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
using System.Runtime.InteropServices;
using System.Text;

namespace FlatSharpEndToEndTests.ClassLib;

[TestClass]
public class SpanComparerTests
{
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void RandomFlatBufferStringComparison()
    {
        Random rng = new Random();
        int min = char.MinValue;
        int max = char.MaxValue;

        for (int i = 0; i < 10000; ++i)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            for (int j = 0; j < 100; ++j)
            {
                sb.Append((char)rng.Next(min, max));
                sb2.Append((char)rng.Next(min, max));
            }

            string str = sb.ToString();
            string str2 = sb2.ToString();

            Span<byte> span = SerializationHelpers.Encoding.GetBytes(str);
            Span<byte> span2 = SerializationHelpers.Encoding.GetBytes(str2);

            Utf8StringComparer utf8Comparer = new Utf8StringComparer();
            Assert.AreEqual(0, StringSpanComparer.Instance.Compare(true, span, true, span));
            Assert.AreEqual(0, utf8Comparer.Compare(str, str));

            Assert.AreEqual(0, StringSpanComparer.Instance.Compare(true, span2, true, span2));
            Assert.AreEqual(0, utf8Comparer.Compare(str2, str2));

            int expected = utf8Comparer.Compare(str, str2);
            int actual = StringSpanComparer.Instance.Compare(true, span, true, span2);
            Assert.AreEqual(expected, actual);

            expected = utf8Comparer.Compare(str2, str);
            actual = StringSpanComparer.Instance.Compare(true, span2, true, span);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void StringComparer_NullItems()
    {
        StringSpanComparer comp = default;

        var ex = Assert.ThrowsException<InvalidOperationException>(() => comp.Compare(false, default, false, default));
        Assert.AreEqual("Strings may not be null when used as sorted vector keys.", ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => comp.Compare(false, default, true, default));
        Assert.AreEqual("Strings may not be null when used as sorted vector keys.", ex.Message);

        ex = Assert.ThrowsException<InvalidOperationException>(() => comp.Compare(true, default, false, default));
        Assert.AreEqual("Strings may not be null when used as sorted vector keys.", ex.Message);
    }

    [TestMethod]
    public void TestBoolComparer()
    {
        BoolSpanComparer comparer = new BoolSpanComparer(default);

        Span<byte> f = new byte[1] { SerializationHelpers.False };
        Span<byte> t = new byte[1] { SerializationHelpers.True };

        Assert.AreEqual(false.CompareTo(true), comparer.Compare(true, f, true, t));
        Assert.AreEqual(false.CompareTo(false), comparer.Compare(true, f, true, f));
        Assert.AreEqual(true.CompareTo(true), comparer.Compare(true, t, true, t));
        Assert.AreEqual(true.CompareTo(false), comparer.Compare(true, t, true, f));
    }

    [TestMethod]
    public void TestByteComparer() => this.Compare<byte>(new ByteSpanComparer(default), sizeof(byte));

    [TestMethod]
    public void TestSByteComparer() => this.Compare<sbyte>(new SByteSpanComparer(default), sizeof(sbyte));

    [TestMethod]
    public void TestUShortComparer() => this.Compare<ushort>(new UShortSpanComparer(default), sizeof(ushort));

    [TestMethod]
    public void TestShortComparer() => this.Compare<short>(new ShortSpanComparer(default), sizeof(short));

    [TestMethod]
    public void TestUIntComaprer() => this.Compare<uint>(new UIntSpanComparer(default), sizeof(uint));

    [TestMethod]
    public void TestIntComparer() => this.Compare<int>(new IntSpanComparer(default), sizeof(int));

    [TestMethod]
    public void TestULongComparer() => this.Compare<ulong>(new ULongSpanComparer(default), sizeof(ulong));

    [TestMethod]
    public void TestLongComparer() => this.Compare<long>(new LongSpanComparer(default), sizeof(long));

    [TestMethod]
    public void TestDoubleComparer() => this.Compare<double>(new DoubleSpanComparer(default), sizeof(double));

    [TestMethod]
    public void TestFloatComparer() => this.Compare<float>(new FloatSpanComparer(default), sizeof(float));

    private void Compare<T>(ISpanComparer comparer, int size) where T : struct, IComparable<T>
    {
        Random rng = new Random();

        for (int i = 0; i < 100; ++i)
        {
            byte[] leftData = new byte[32];
            byte[] rightData = new byte[32];
            rng.NextBytes(leftData);
            rng.NextBytes(rightData);

            T a = MemoryMarshal.Cast<byte, T>(leftData.AsSpan())[0];
            T b = MemoryMarshal.Cast<byte, T>(rightData.AsSpan())[0];

            var leftSpan = leftData.AsSpan().Slice(0, size);
            var rightSpan = rightData.AsSpan().Slice(0, size);

            Assert.AreEqual(
                a.CompareTo(b),
                comparer.Compare(true, leftSpan, true, rightSpan));

            Assert.AreEqual(
                b.CompareTo(a),
                comparer.Compare(true, rightSpan, true, leftSpan));

            Assert.AreEqual(0, comparer.Compare(true, leftSpan, true, leftSpan));
            Assert.AreEqual(0, comparer.Compare(true, rightSpan, true, rightSpan));
        }

        // Verify that the not-present case works as expected.
        byte[] defaultData = new byte[32];
        Assert.AreEqual(0, comparer.Compare(true, defaultData, false, default));
        Assert.AreEqual(0, comparer.Compare(false, default, true, defaultData));
        Assert.AreEqual(0, comparer.Compare(false, default, false, default));
    }
}
