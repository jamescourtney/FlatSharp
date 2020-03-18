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
    /// This implemention is likely inefficient relative to the FlatBuffers algorithm. This is because FlatSharp largely operates with objects and 
    /// FlatBuffers largely operates with buffers. Optimizations may be available and worthwhile if this is really terrible.
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

            int xMax = encoding.GetMaxByteCount(x.Length);
            int yMax = encoding.GetMaxByteCount(y.Length);

#if NETSTANDARD
            // Consider threadlocal caching to avoid allocations?
            byte[] xBytes = new byte[xMax];
            byte[] yBytes = new byte[yMax];

            int xCount = encoding.GetBytes(x, 0, x.Length, xBytes, 0);
            int yCount = encoding.GetBytes(y, 0, y.Length, yBytes, 0);
#else
            Span<byte> xBytes = stackalloc byte[xMax];
            Span<byte> yBytes = stackalloc byte[yMax];
            int xCount = encoding.GetBytes(x, xBytes);
            int yCount = encoding.GetBytes(y, yBytes);
#endif

            int minLength = Math.Min(xCount, yCount);

            for (int i = 0; i < minLength; ++i)
            {
                byte xByte = xBytes[i];
                byte yByte = yBytes[i];

                if (xByte != yByte)
                {
                    return xByte - yByte;
                }
            }

            return x.Length - y.Length;
        }
    }
}
