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
    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Helps to build vtables for a single serialization action.
    /// </summary>
    internal sealed class VTableHelper
    {
        private byte[] vTableBuffer = new byte[128];
        private List<int> vtableOffsets = new List<int>();

        private SerializationContext context;
        private bool isNested;
        private int maxIndex;
        private int maxIndexWithValue;

        internal bool isCalculateOnlyMode;

        public VTableHelper(SerializationContext context)
        {
            this.context = context;
        }

        public void Reset()
        {
            // Reset on initialization.
            this.vtableOffsets.Clear();
            this.vTableBuffer.AsSpan().Fill(0);
            this.isNested = false;
            this.isCalculateOnlyMode = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartObject(int maxIndex)
        {
            Debug.Assert(!this.isNested);
            Debug.Assert(maxIndex >= 0);

            this.isNested = true;
            this.maxIndex = maxIndex;
            this.maxIndexWithValue = -1;

            int neededLength = 4 + 2 * (maxIndex + 1);
            if (neededLength > this.vTableBuffer.Length)
            {
                this.vTableBuffer = new byte[neededLength];
            }
            else
            {
                // Fill with 0's.
                this.vTableBuffer.AsSpan().Fill(0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOffset(int index, int value)
        {
            Debug.Assert(this.isNested);
            Debug.Assert(index >= 0 && index <= this.maxIndex);

            if (value != 0)
            {
                if (index > this.maxIndexWithValue)
                {
                    this.maxIndexWithValue = index;
                }

                checked
                {
                    ushort actualIndex = (ushort)(4 + (2 * index));

                    BinaryPrimitives.WriteUInt16LittleEndian(
                        this.vTableBuffer.AsSpan().Slice(actualIndex, sizeof(ushort)), (ushort)value);
                }
            }
        }

        /// <summary>
        /// Ends the current object, returning the absolute location of the appropriate vtable.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int EndObject(Span<byte> buffer, SpanWriter writer, int tableLength)
        {
            Debug.Assert(this.isNested);
            this.isNested = false;
            int vtableLength = 4 + 2 * (this.maxIndexWithValue + 1);

            if (this.isCalculateOnlyMode)
            {
                return this.context.AllocateSpace(vtableLength, sizeof(ushort));
            }

            Memory<byte> vtableMemory = this.vTableBuffer.AsMemory().Slice(0, vtableLength);
            Span<byte> currentVTable = vtableMemory.Span;

            BinaryPrimitives.WriteUInt16LittleEndian(currentVTable.Slice(0, 2), checked((ushort)vtableLength));
            BinaryPrimitives.WriteUInt16LittleEndian(currentVTable.Slice(2, 2), checked((ushort)tableLength));

            var offsets = this.vtableOffsets;
            int offsetCount = offsets.Count;
            for (int i = 0; i < offsetCount; ++i)
            {
                int offset = offsets[i];
                ReadOnlySpan<byte> existingVTable = buffer.Slice(offset);
                existingVTable = existingVTable.Slice(0, BinaryPrimitives.ReadUInt16LittleEndian(existingVTable));

                if (AreContentsEqual(existingVTable, currentVTable))
                {
                    return offset;
                }
            }

            // Oh, well. Write the new table.
            int newVTableOffset = this.context.AllocateSpace(vtableLength, sizeof(ushort));
            currentVTable.CopyTo(buffer.Slice(newVTableOffset, vtableLength));
            offsets.Add(newVTableOffset);

            return newVTableOffset;
        }

        private static bool AreContentsEqual(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            int leftLength = left.Length;
            if (leftLength != right.Length)
            {
                return false;
            }

            int i = 0;

            // Compare 8 bytes at a time. Faster.
            ReadOnlySpan<ulong> leftLong = MemoryMarshal.Cast<byte, ulong>(left);
            ReadOnlySpan<ulong> rightLong = MemoryMarshal.Cast<byte, ulong>(right);

            for (; i < leftLong.Length; ++i)
            {
                if (leftLong[i] != rightLong[i])
                {
                    return false;
                }
            }

            // multiply by 8.
            i <<= 3;

            for (; i < leftLength; ++i)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
