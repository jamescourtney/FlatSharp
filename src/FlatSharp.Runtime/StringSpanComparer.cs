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
    using System.ComponentModel;

    /// <summary>
    /// Implements string comparison for flatbuffers.
    /// </summary>
    /// <remarks>
    /// Based on the FlatBuffer comparison here: https://github.com/google/flatbuffers/blob/3a70e0b30890ca265a33f099912762eb51ac505f/net/FlatBuffers/Table.cs
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct StringSpanComparer : ISpanComparer
    {
        /// <summary>
        /// Singleton access.
        /// </summary>
        public static StringSpanComparer Instance => default;

        public StringSpanComparer(string notUsed)
        {
        }

        public int Compare(bool leftExists, ReadOnlySpan<byte> x, bool rightExists, ReadOnlySpan<byte> y)
        {
            if (!leftExists || !rightExists)
            {
                throw new InvalidOperationException($"Strings may not be null when used as sorted vector keys.");
            }

            int minLength = Math.Min(x.Length, y.Length);

            for (int i = 0; i < minLength; ++i)
            {
                byte xByte = x[i];
                byte yByte = y[i];

                if (xByte != yByte)
                {
                    return xByte - yByte;
                }
            }

            return x.Length - y.Length;
        }
    }
}
