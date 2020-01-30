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
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// A context object for a FlatBuffer serialize operation. The context is responsible for allocating space in the buffer
    /// and managing the latest offset.
    /// </summary>
    public sealed class SerializationContext
    {
        internal static readonly ThreadLocal<SerializationContext> ThreadLocalContext = new ThreadLocal<SerializationContext>(() => new SerializationContext());

        private int offset;
        private int capacity;

        /// <summary>
        /// Initializes a new serialization context.
        /// </summary>
        public SerializationContext()
        {
            this.VTableBuilder = new VTableBuilder(this);
        }

        /// <summary>
        /// Gets the vtable builder associated with this context.
        /// </summary>
        public VTableBuilder VTableBuilder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// The maximum offset within the buffer.
        /// </summary>
        public int Offset
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.offset;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.offset = value;
        }

        /// <summary>
        /// Resets the context.
        /// </summary>
        public void Reset(int capacity)
        {
            this.offset = 0;
            this.capacity = capacity;
            this.VTableBuilder.Reset();
        }

        /// <summary>
        /// Allocate a vector and return the index. Does not populate any details of the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AllocateVector(int itemAlignment, int numberOfItems, int sizePerItem)
        {
            checked
            {
                if (numberOfItems < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(numberOfItems));
                }

                int bytesNeeded = numberOfItems * sizePerItem + sizeof(uint);
                int alignment = sizeof(uint);
                if (itemAlignment > alignment)
                {
                    alignment = itemAlignment;
                }

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
                Debug.WriteLine($"Allocating vector! NItems={numberOfItems}, SizePerItem={sizePerItem}, Offset={offset}, Length={bytesNeeded}");

                return offset;
            }
        }

        /// <summary>
        /// Allocates a block of memory. Returns the offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    }
}
