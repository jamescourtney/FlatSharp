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

namespace FlatSharpTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Text;
    using FlatSharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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

                Span<byte> span = InputBuffer.Encoding.GetBytes(str);
                Span<byte> span2 = InputBuffer.Encoding.GetBytes(str2);

                Utf8StringComparer utf8Comparer = new Utf8StringComparer();
                Assert.AreEqual(0, StringSpanComparer.Instance.Compare(span, span));
                Assert.AreEqual(0, utf8Comparer.Compare(str, str));

                Assert.AreEqual(0, StringSpanComparer.Instance.Compare(span2, span2));
                Assert.AreEqual(0, utf8Comparer.Compare(str2, str2));

                int expected = utf8Comparer.Compare(str, str2);
                int actual = StringSpanComparer.Instance.Compare(span, span2);
                Assert.AreEqual(expected, actual);

                expected = utf8Comparer.Compare(str2, str);
                actual = StringSpanComparer.Instance.Compare(span2, span);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestBoolComparer()
        {
            BoolSpanComparer comparer = new BoolSpanComparer();

            Span<byte> f = new byte[1] { InputBuffer.False };
            Span<byte> t = new byte[1] { InputBuffer.True };

            Assert.AreEqual(false.CompareTo(true), comparer.Compare(f, t));
            Assert.AreEqual(false.CompareTo(false), comparer.Compare(f, f));
            Assert.AreEqual(true.CompareTo(true), comparer.Compare(t, t));
            Assert.AreEqual(true.CompareTo(false), comparer.Compare(t, f));
        }

        [TestMethod]
        public void TestByteComparer() => this.Compare<byte>(ByteSpanComparer.Instance, sizeof(byte));

        [TestMethod]
        public void TestSByteComparer() => this.Compare<sbyte>(SByteSpanComparer.Instance, sizeof(sbyte));

        [TestMethod]
        public void TestUShortComparer() => this.Compare<ushort>(UShortSpanComparer.Instance, sizeof(ushort));

        [TestMethod]
        public void TestShortComparer() => this.Compare<short>(ShortSpanComparer.Instance, sizeof(short));

        [TestMethod]
        public void TestUIntComaprer() => this.Compare<uint>(UIntSpanComparer.Instance, sizeof(uint));

        [TestMethod]
        public void TestIntComparer() => this.Compare<int>(IntSpanComparer.Instance, sizeof(int));

        [TestMethod]
        public void TestULongComparer() => this.Compare<ulong>(ULongSpanComparer.Instance, sizeof(ulong));

        [TestMethod]
        public void TestLongComparer() => this.Compare<long>(LongSpanComparer.Instance, sizeof(long));

        [TestMethod]
        public void TestDoubleComparer() => this.Compare<double>(DoubleSpanComparer.Instance, sizeof(double));

        [TestMethod]
        public void TestFloatComparer() => this.Compare<float>(FloatSpanComparer.Instance, sizeof(float));

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
                    comparer.Compare(leftSpan, rightSpan));

                Assert.AreEqual(
                    b.CompareTo(a),
                    comparer.Compare(rightSpan, leftSpan));

                Assert.AreEqual(0, comparer.Compare(leftSpan, leftSpan));
                Assert.AreEqual(0, comparer.Compare(rightSpan, rightSpan));
            }
        }
    }
}
