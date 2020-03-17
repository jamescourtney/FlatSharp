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
 
namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Implements string comparison for flatbuffers.
    /// </summary>
    /// <remarks>
    /// Based on the FlatBuffer comparison here: https://github.com/google/flatbuffers/blob/3a70e0b30890ca265a33f099912762eb51ac505f/net/FlatBuffers/Table.cs
    /// This implemention is likely inefficient relative to the FlatBuffers algorithm. This is because FlatSharp largely operates with objects and FlatBuffers largely operates
    /// with buffers. Optimizations may be available.
    /// </remarks>
    public class FlatBufferStringComparer : IComparer<string>
    {
        /// <summary>
        /// Singleton access.
        /// </summary>
        public static FlatBufferStringComparer Instance { get; } = new FlatBufferStringComparer();

        private FlatBufferStringComparer()
        {
        }

        public int Compare(string x, string y)
        {
            Encoding encoding = InputBuffer.Encoding;
            int maxBytes = encoding.GetMaxByteCount(1);

#if NETCOREAPP2_1
            Span<byte> xBytes = stackalloc byte[maxBytes];
            Span<byte> yBytes = stackalloc byte[maxBytes];
            Span<char> charBuffer = stackalloc char[1];
#else
            byte[] xBytes = new byte[maxBytes];
            byte[] yBytes = new byte[maxBytes];
            char[] charBuffer = new char[1];
#endif

            int minLength = Math.Min(x.Length, y.Length);

            for (int i = 0; i < minLength; ++i)
            {
                char xChar = x[i];
                char yChar = y[i];

                if (xChar != yChar)
                {
                    charBuffer[0] = xChar;
#if NETCOREAPP2_1
                    int xCount = encoding.GetBytes(charBuffer, xBytes);
#else
                    int xCount = encoding.GetBytes(charBuffer, 0, 1, xBytes, 0);
#endif

                    charBuffer[0] = yChar;
#if NETCOREAPP2_1
                    int yCount = encoding.GetBytes(charBuffer, yBytes);
#else
                    int yCount = encoding.GetBytes(charBuffer, 0, 1, yBytes, 0);
#endif

                    int minCount = Math.Min(xCount, yCount);
                    for (int j = 0; j < minCount; ++j)
                    {
                        byte xByte = xBytes[j];
                        byte yByte = yBytes[j];

                        if (xByte != yByte)
                        {
                            return xByte - yByte;
                        }
                    }

                    return xCount - yCount;
                }
            }

            return x.Length - y.Length;
        }
    }
}
