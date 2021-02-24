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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// A context object for a FlatBuffer serialize operation. The context is responsible for allocating space in the buffer
    /// and managing the latest offset.
    /// </summary>
    public sealed class SerializationContext
    {
        /// <summary>
        /// A delegate to invoke after the serialization process has completed. Used for sorting vectors.
        /// </summary>
        public delegate void PostSerializeAction(Span<byte> span, SerializationContext context);

        internal static readonly ThreadLocal<SerializationContext> ThreadLocalContext = new ThreadLocal<SerializationContext>(() => new SerializationContext());

        private int offset;
        private int capacity;
        private readonly List<PostSerializeAction> postSerializeActions;
        private readonly LinkedList<int> vtableOffsets;

        /// <summary>
        /// Initializes a new serialization context.
        /// </summary>
        public SerializationContext()
        {
            this.postSerializeActions = new List<PostSerializeAction>();
            this.vtableOffsets = new();
        }

        /// <summary>
        /// The maximum offset within the buffer.
        /// </summary>
        public int Offset
        {
            get => this.offset;
            set => this.offset = value;
        }

        /// <summary>
        /// The shared string writer used for this serialization operation.
        /// </summary>
        public ISharedStringWriter? SharedStringWriter { get; set; }

        /// <summary>
        /// Resets the context.
        /// </summary>
        public void Reset(int capacity)
        {
            this.offset = 0;
            this.capacity = capacity;
            this.SharedStringWriter = null;
            this.postSerializeActions.Clear();
            this.vtableOffsets.Clear();
        }

        /// <summary>
        /// Invokes any post-serialize actions.
        /// </summary>
        public void InvokePostSerializeActions(Span<byte> span)
        {
            var actions = this.postSerializeActions;
            int count = actions.Count;

            for (int i = 0; i < count; ++i)
            {
                actions[i](span, this);
            }
        }

        public void AddPostSerializeAction(PostSerializeAction action)
        {
            this.postSerializeActions.Add(action);
        }

        /// <summary>
        /// Allocate a vector and return the index. Does not populate any details of the vector.
        /// </summary>
        public int AllocateVector(int itemAlignment, int numberOfItems, int sizePerItem)
        {
            checked
            {
                if (numberOfItems < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(numberOfItems));
                }

                int bytesNeeded = numberOfItems * sizePerItem + sizeof(uint);

                // Vectors have a size uoffset_t, followed by N items. The uoffset_t needs to be 4 byte aligned, while the items need to be N byte aligned.
                // So, if the items are double or long, the length field has 4 byte alignment, but the item field has 8 byte alignment.
                // This means that we need to choose an offset such that:
                // (lengthIndex) % 4 == 0
                // (lengthIndex + 4) % N == 0
                //
                // Obviously, if N <= 4 this is trivial. If N = 8, it gets a bit more interesting.
                // First, align the offset to 4.
                int offset = this.offset;
                offset += SerializationHelpers.GetAlignmentError(offset, sizeof(uint));

                // Now, align offset + 4 to item alignment.
                offset += SerializationHelpers.GetAlignmentError(offset + sizeof(uint), itemAlignment);
                this.offset = offset;

                offset = this.AllocateSpace(bytesNeeded, sizeof(uint));

                Debug.Assert(offset % 4 == 0);
                Debug.Assert((offset + 4) % itemAlignment == 0);

                return offset;
            }
        }

        /// <summary>
        /// Allocates a block of memory. Returns the offset.
        /// </summary>
        public int AllocateSpace(int bytesNeeded, int alignment)
        {
            checked
            {
                int offset = this.offset;
                Debug.Assert(alignment == 1 || alignment % 2 == 0);

                offset += SerializationHelpers.GetAlignmentError(offset, alignment);

                int finalOffset = offset + bytesNeeded;
                if (finalOffset >= this.capacity)
                {
                    throw new BufferTooSmallException();
                }

                this.offset = finalOffset;
                return offset;
            }
        }

        public int FinishVTable<TSpanWriter>(
            TSpanWriter writer,
            int tableLength,
            Span<byte> buffer, 
            Span<byte> vtable) where TSpanWriter : ISpanWriter
        {
            checked
            {
                var offsets = this.vtableOffsets;

                // write table length.
                writer.WriteUShort(vtable, (ushort)tableLength, sizeof(ushort), this);

                // find vtable length
                while (ScalarSpanReader.ReadUShort(vtable.Slice(vtable.Length - sizeof(ushort))) == 0)
                {
                    vtable = vtable.Slice(0, vtable.Length - sizeof(ushort));
                }

                writer.WriteUShort(vtable, (ushort)vtable.Length, 0, this);

                var node = offsets.First;
                while (node is not null)
                {
                    var offset = node.Value;

                    ReadOnlySpan<byte> existingVTable = buffer.Slice(offset);
                    existingVTable = existingVTable.Slice(0, ScalarSpanReader.ReadUShort(existingVTable));

                    if (CompareEquality(existingVTable, vtable))
                    {
                        if (node.Previous is not null)
                        {
                            // Move the current vtable to the head of the list.
                            offsets.Remove(node);
                            offsets.AddFirst(node);
                        }

                        // We already have a vtable that matches this specification. Return that offset.
                        return offset;
                    }

                    node = node.Next;
                }

                // Oh, well. Write the new table.
                int newVTableOffset = this.AllocateSpace(vtable.Length, sizeof(ushort));
                vtable.CopyTo(buffer.Slice(newVTableOffset));
                offsets.AddFirst(newVTableOffset);

                return newVTableOffset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CompareEquality(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            int length = left.Length;

            if (length != right.Length)
            {
                return false;
            }

            // Experimentally, it is cheaper to do the comparison ourselves for 
            // small spans before asking the core libs to check for us.
            if (length <= 16)
            {
                while (left.Length >= sizeof(ulong))
                {
                    if (ScalarSpanReader.ReadULong(left) != ScalarSpanReader.ReadULong(right))
                    {
                        return false;
                    }

                    left = left.Slice(sizeof(ulong));
                    right = right.Slice(sizeof(ulong));
                }

                if (left.Length >= 4)
                {
                    if (ScalarSpanReader.ReadUInt(left) != ScalarSpanReader.ReadUInt(right))
                    {
                        return false;
                    }

                    left = left.Slice(sizeof(uint));
                    right = right.Slice(sizeof(uint));
                }

                for (int i = 0; i < left.Length; ++i)
                {
                    if (left[i] != right[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                // even in the fallback case, we can often fast-fail in the first word.
                // length is > 16 here by assertion of the if statement above.
                return ScalarSpanReader.ReadULong(left) == ScalarSpanReader.ReadULong(right) && left.SequenceEqual(right);
            }
        }
    }
}
