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

    /// <summary>
    /// Tracks and builds vtables during serialization. This object can be reused between serialization operations,
    /// but is not threadsafe.
    /// </summary>
    public sealed class VTableBuilder
    {
        private byte[] vTableBuffer = new byte[128];
        private List<int> vtableOffsets = new List<int>();

        private SerializationContext context;
        private bool isNested;
        private int maxIndex;
        private int maxIndexWithValue;

        public VTableBuilder(SerializationContext context)
        {
            this.context = context;
        }

        public void Reset()
        {
            // Reset on initialization.
            this.vtableOffsets.Clear();
            this.vTableBuffer.AsSpan().Fill(0);
            this.isNested = false;
        }

        /// <summary>
        /// Starts a new vtable.
        /// </summary>
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

        /// <summary>
        /// Sets the given index to the given offset.
        /// </summary>
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
            // TODO: Include some sort of hash function here so we don't have to do a linear traversal each time?
            Debug.Assert(this.isNested);

            this.isNested = false;
            int vtableLength = 4 + 2 * (this.maxIndexWithValue + 1);

            Memory<byte> vtableMemory = this.vTableBuffer.AsMemory().Slice(0, vtableLength);
            Span<byte> currentVTable = vtableMemory.Span;

            var context = this.context;
            writer.WriteUShort(currentVTable, checked((ushort)vtableLength), 0, context);
            writer.WriteUShort(currentVTable, checked((ushort)tableLength), sizeof(ushort), context);

            var offsets = this.vtableOffsets;
            int offsetCount = offsets.Count;

            for (int i = 0; i < offsetCount; ++i)
            {
                int offset = offsets[i];
                ReadOnlySpan<byte> existingVTable = buffer.Slice(offset);
                existingVTable = existingVTable.Slice(0, BinaryPrimitives.ReadUInt16LittleEndian(existingVTable));

                if (existingVTable.SequenceEqual(currentVTable))
                {
                    // We already have a vtable that matches this specification. Return that offset.
                    return offset;
                }
            }

            // Oh, well. Write the new table.
            int newVTableOffset = this.context.AllocateSpace(vtableLength, sizeof(ushort));
            currentVTable.CopyTo(buffer.Slice(newVTableOffset, vtableLength));
            offsets.Add(newVTableOffset);

            return newVTableOffset;
        }
    }
}
