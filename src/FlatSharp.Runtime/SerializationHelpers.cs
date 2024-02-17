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
using System.Text;

namespace FlatSharp.Internal;

/// <summary>
/// Collection of methods that help to serialize objects. It's kind of a hodge-podge,
/// but mostly focuses on getting the size of things when serializing and data alignment.
/// </summary>
public static class SerializationHelpers
{
    /// <summary>
    /// A sealed version of UTF8Encoding that allows the JIT to emit nonvirtual method calls.
    /// </summary>
    public sealed class SealedUTF8Encoding : UTF8Encoding
    {
        public SealedUTF8Encoding(bool includeByteOrderMark) : base(includeByteOrderMark) { }
    }

    /// <summary>
    /// Encoding used for strings.
    /// </summary>
    public static readonly SealedUTF8Encoding Encoding = new SealedUTF8Encoding(false);

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
    /// A branchless Math.Max analog that works for nonnegative numbers.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int VTableMax(int current, int proposed)
    {
        Debug.Assert(current >= 0);
        Debug.Assert(proposed >= 0);

        int sign = (current - proposed) >>> 31;
        return current + ((proposed - current) * sign);
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
            Throw();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Throw() => FSThrow.InvalidData("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureDepthLimit(short remainingDepth)
    {
        if (remainingDepth < 0)
        {
            Throw();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Throw() => FSThrow.InvalidData($"FlatSharp passed the configured depth limit when deserializing. This can be configured with 'IGeneratedSerializer.WithSettings'.");
    }
}
