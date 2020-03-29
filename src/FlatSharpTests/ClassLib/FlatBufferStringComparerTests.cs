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
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Unsafe;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    [TestClass]
    public class FlatBufferStringComparerTests
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

                Assert.AreEqual(0, StringSpanComparer.Instance.Compare(span, span));
                Assert.AreEqual(0, this.Compare(str, str));

                Assert.AreEqual(0, StringSpanComparer.Instance.Compare(span2, span2));
                Assert.AreEqual(0, this.Compare(str2, str2));

                int expected = this.Compare(str, str2);
                int actual = StringSpanComparer.Instance.Compare(span, span2);
                Assert.AreEqual(expected, actual);

                expected = this.Compare(str2, str);
                actual = StringSpanComparer.Instance.Compare(span2, span);
                Assert.AreEqual(expected, actual);
            }
        }

        private int Compare(string x, string y)
        {
            byte[] xBytes = Encoding.UTF8.GetBytes(x);
            byte[] yBytes = Encoding.UTF8.GetBytes(y);

            int minLength = Math.Min(xBytes.Length, yBytes.Length);
            for (int i = 0; i < minLength; ++i)
            {
                byte xByte = xBytes[i];
                byte yByte = yBytes[i];
                if (xByte != yByte)
                {
                    return xByte - yByte;
                }
            }

            return xBytes.Length - yBytes.Length;
        }
    }
}
