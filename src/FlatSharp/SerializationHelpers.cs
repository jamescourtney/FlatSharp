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

namespace FlatSharp
{
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Collection of methods that help to serialize objects. It's kind of a hodge-podge,
    /// but mostly focuses on getting the size of things when serializing and data alignment.
    /// </summary>
    internal static class SerializationHelpers
    {
        #region ILSizers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeBool(bool value, bool defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(bool));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(bool);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeByte(byte value, byte defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(byte));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(byte);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeSByte(sbyte value, sbyte defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(sbyte));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(sbyte);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeUInt16(ushort value, ushort defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(ushort));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(ushort);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeInt16(short value, short defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(short));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(short);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeUInt32(uint value, uint defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(uint));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(uint);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeInt32(int value, int defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(int));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(int);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeUInt64(ulong value, ulong defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(ulong));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(ulong);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeInt64(long value, long defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(long));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(long);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeSingle(float value, float defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(float));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(float);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeDouble(double value, double defaultValue, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != defaultValue)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(double));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(double);
                }
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILGetSizeString(string value, string notUsed, ref int sizeNeeded)
        {
            int offset = 0;
            if (value != null)
            {
                checked
                {
                    int localSizeNeeded = sizeNeeded;
                    localSizeNeeded += SerializationHelpers.GetAlignmentError(localSizeNeeded, sizeof(uint));
                    offset = localSizeNeeded;
                    sizeNeeded = localSizeNeeded + sizeof(uint);
                }
            }
            return offset;
        }

        /// <summary>
        /// Returns the number of padding bytes to be added to the given offset to acheive the given alignment.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAlignmentError(int offset, int alignment)
        {
            Debug.Assert(alignment == 1 || alignment % 2 == 0);
            return (~offset + 1) & (alignment - 1);
        }

        #endregion
    }
}
