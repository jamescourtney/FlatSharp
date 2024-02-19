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

using System.IO;
using System.Text;

namespace FlatSharp.Internal;

/// <summary>
/// Collection of methods that help to serialize objects. It's kind of a hodge-podge,
/// but mostly focuses on getting the size of things when serializing and data alignment.
/// </summary>
public static class SerializationHelpers
{
    /// <summary>
    /// Encoding used for strings.
    /// </summary>
    public static readonly Encoding Encoding = Encoding.UTF8;

    /// <summary>
    /// Value of true as a byte.
    /// </summary>
    public const byte True = 1;

    /// <summary>
    /// Value of false as a byte.
    /// </summary>
    public const byte False = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetMaxSize(string value)
    {
        checked
        {
            return
                sizeof(uint) + GetMaxPadding(sizeof(uint)) + // string length
                Encoding.GetMaxByteCount(value.Length) + 1; // max bytes and null terminator
        }
    }

    /// <summary>
    /// Gets the maximum padding for the given alignment. This method seems simple,
    /// but is inlined and is useful for generating more comprehensible IL.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetMaxPadding(int alignment)
    {
        return alignment - 1;
    }

    /// <summary>
    /// Applies a bitwise OR to the operands.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CombineMask(ref byte source, byte mask)
    {
        source |= mask;
    }

    /// <summary>
    /// Returns the number of padding bytes to be added to the given offset to acheive the given alignment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetAlignmentError(int offset, int alignment)
    {
        Debug.Assert(alignment == 1 || alignment % 2 == 0);
        return (-offset) & (alignment - 1);
    }

    /// <summary>
    /// Throws an InvalidDataException if the given item is null.
    /// </summary>
    /// <remarks>
    /// Add generic constraint to catch errors calling this for value types.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureNonNull<T>(T? item) where T : class
    {
        if (item is null)
        {
            FSThrow.InvalidData_InvalidNull();
        }
    }

    /// <summary>
    /// Throws if the depth limit is less than 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureDepthLimit(short remainingDepth)
    {
        if (remainingDepth < 0)
        {
            FSThrow.InvalidData_DepthLimit();
        }
    }
}
