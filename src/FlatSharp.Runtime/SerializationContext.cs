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

using System.Threading;

namespace FlatSharp.Internal;

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
    private readonly List<int> vtableOffsets;

    /// <summary>
    /// Initializes a new serialization context.
    /// </summary>
    public SerializationContext()
    {
        this.postSerializeActions = new List<PostSerializeAction>();
        this.vtableOffsets = new List<int>();
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InvokePostSerializeActions(Span<byte> span)
    {
        var actions = this.postSerializeActions;
        int count = actions.Count;

        for (int i = 0; i < count; ++i)
        {
            actions[i](span, this);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddPostSerializeAction(PostSerializeAction action)
    {
        this.postSerializeActions.Add(action);
    }

    /// <summary>
    /// Allocate a vector and return the index. Does not populate any details of the vector.
    /// </summary>
    public int AllocateVector(int itemAlignment, int numberOfItems, int sizePerItem)
    {
        if (numberOfItems < 0)
        {
            FSThrow.ArgumentOutOfRange(nameof(numberOfItems));
        }

        int bytesNeeded = checked(numberOfItems * sizePerItem + sizeof(uint));

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

        offset = this.AllocateSpace(bytesNeeded, alignment: 1); // already aligned correctly, no need to realign

        Debug.Assert(offset % 4 == 0);
        Debug.Assert((offset + 4) % itemAlignment == 0);

        return offset;
    }

    /// <summary>
    /// Allocates a block of memory. Returns the offset.
    /// </summary>
    public int AllocateSpace(int bytesNeeded, int alignment)
    {
        int offset = this.offset;
        Debug.Assert(alignment == 1 || alignment % 2 == 0);

        offset += SerializationHelpers.GetAlignmentError(offset, alignment);

        int finalOffset = offset + bytesNeeded;
        if (finalOffset >= this.capacity)
        {
            FSThrow.BufferTooSmall(0);
        }

        this.offset = finalOffset;
        return offset;
    }

    [MethodImpl(MethodImplOptions.NoInlining)] // Common method; don't inline
    public int FinishVTable(
        Span<byte> buffer,
        Span<byte> vtable)
    {
        var offsets = this.vtableOffsets;
        int count = offsets.Count;

        for (int i = 0; i < count; ++i)
        {
            int offset = offsets[i];

            ReadOnlySpan<byte> existingVTable = buffer.Slice(offset);
            existingVTable = existingVTable.Slice(0, ScalarSpanReader.ReadUShort(existingVTable));

            if (existingVTable.SequenceEqual(vtable))
            {
                // Slowly bubble used things towards the front of the list.
                // This is not exact, but should keep frequently used
                // items towards the front.
                Promote(i, offsets);

                return offset;
            }
        }

        // Oh, well. Write the new table.
        int newVTableOffset = this.AllocateSpace(vtable.Length, sizeof(ushort));
        vtable.CopyTo(buffer.Slice(newVTableOffset));
        offsets.Add(newVTableOffset);

        // "Insert" this item in the middle of the list.
        int maxIndex = offsets.Count - 1;
        Promote(maxIndex, offsets);

        return newVTableOffset;

        // Promote frequently-used items to be closer to the front of the list.
        // This is done with a swap to avoid shuffling the whole list by inserting
        // at a given index. An alternative might be an unrolled linked list data structure.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Promote(int i, List<int> offsets)
        {
            int swapIndex = i / 2;

            int temp = offsets[i];
            offsets[i] = offsets[swapIndex];
            offsets[swapIndex] = temp;
        }
    }
}
