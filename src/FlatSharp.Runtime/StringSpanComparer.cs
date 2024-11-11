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

using System.Buffers.Binary;

namespace FlatSharp.Internal;

/// <summary>
/// Implements string comparison for flatbuffers.
/// </summary>
/// <remarks>
/// Based on the FlatBuffer comparison here: https://github.com/google/flatbuffers/blob/3a70e0b30890ca265a33f099912762eb51ac505f/net/FlatBuffers/Table.cs
/// </remarks>
public struct StringSpanComparer : ISpanComparer
{
    /// <summary>
    /// Singleton access.
    /// </summary>
    public static StringSpanComparer Instance => default;

    public StringSpanComparer(string? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer x, bool rightExists, TBuffer y)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        if (!leftExists || !rightExists)
        {
            FSThrow.InvalidOperation("Strings may not be null when used as sorted vector keys.");
        }

        int i = 0;
        long minLength = x.Length;
        if (y.Length < minLength)
        {
            minLength = y.Length;
        }

        while (i + sizeof(ulong) <= minLength)
        {
            // Use BE since we read from left to right.
            ulong left = x.ReadUInt64(i);
            ulong right = y.ReadUInt64(i);

            int cmp = left.CompareTo(right);
            if (cmp != 0)
            {
                return cmp;
            }

            i += sizeof(ulong);
        }

        for (; i < minLength; ++i)
        {
            byte xByte = x.ReadUInt8(i);
            byte yByte = y.ReadUInt8(i);

            if (xByte != yByte)
            {
                return xByte - yByte;
            }
        }

        return (int)(x.Length - y.Length);
    }
}
