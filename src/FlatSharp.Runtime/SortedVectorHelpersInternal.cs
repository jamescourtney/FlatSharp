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

/* 
This file includes sections modeled after the dotnet runtime project on github. The dotnet license file is included here:

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
*/

using System.Buffers;
using System.Buffers.Binary;

namespace FlatSharp.Internal;

/// <summary>
/// Helper methods for dealing with sorted vectors. This class provides functionality for both sorting vectors and
/// binary searching through them.
/// </summary>
public sealed class VectorSortAction<TSpanComparer> : IPostSerializeAction
    where TSpanComparer : ISpanComparer
{
    private readonly TSpanComparer comparer;
    private readonly long vectorUOffset;
    private readonly int vTableIndex;
    private readonly int? keyInlineSize;

    public VectorSortAction(
        long vectorUOffset,
        int vtableIndex,
        int? keyInlineSize,
        TSpanComparer comparer)
    {
        this.vectorUOffset = vectorUOffset;
        this.vTableIndex = vtableIndex;
        this.keyInlineSize = keyInlineSize;
        this.comparer = comparer;
    }

    /// <summary>
    /// Sorts the given flatbuffer vector. This method, used incorrectly, is a fantastic way to corrupt your buffer.
    /// </summary>
    /// <remarks>
    /// This method assumes that all vector members are tables, and each table has a defined key. Our vector looks like this:
    /// [length] [ uoffset to first table ] [ uoffset 2 ] ... [ uoffset N ]
    /// 
    /// Prior to sorting, we iterate through the vector and populate an array with tuples of (absolute key offset, key length, absolute table offset).
    /// Then, we sort that array. This saves us from having to follow unnecessary indirections from vector -> table -> vtable -> key.
    /// 
    /// After we sort our array of tuples, we go back and overwrite the vectors with the updated uoffsets, which need to be adjusted relative to the
    /// new position within the vector.
    /// 
    /// Furthermore, this method is left without checked multiply operations since this is a post-serialize action, which means the input
    /// has already been sanitized since FlatSharp wrote it.
    /// </remarks>
    public void Invoke(BigSpan buffer, SerializationContext context)
    {
        long vectorStartOffset =
            vectorUOffset + buffer.ReadUInt(vectorUOffset);
        int vectorLength = (int)buffer.ReadUInt(vectorStartOffset);
        long index0Position = vectorStartOffset + sizeof(int);

        (long, int, long)[]? pooledArray = null;

        // Traverse the vector and figure out the offsets of all the keys.
        // Store that in some local data, hopefully on the stack. 512 is somewhat arbitrary, but we want to avoid stack overflows.
        Span<(long offset, int length, long tableOffset)> keyOffsets =
            vectorLength < 512
                ? stackalloc (long, int, long)[vectorLength]
                : (pooledArray = ArrayPool<(long, int, long)>.Shared.Rent(vectorLength)).AsSpan()
                .Slice(0, vectorLength);

        for (int i = 0; i < keyOffsets.Length; ++i)
        {
            keyOffsets[i] = GetKeyOffset(buffer, index0Position, i, this.vTableIndex, keyInlineSize);
        }

        // Sort the offsets.
        IntroSort(buffer, comparer, 0, vectorLength - 1, keyOffsets);

        // Overwrite the vector with the sorted offsets. Bound the vector so we're confident we aren't 
        // partying inappropriately in the rest of the buffer.
        BigSpan boundedVector = buffer.Slice(index0Position, sizeof(uint) * vectorLength);
        long nextPosition = index0Position;
        for (int i = 0; i < keyOffsets.Length; ++i)
        {
            ref (long _, int __, long tableOffset) tuple = ref keyOffsets[i];

            boundedVector.WriteUInt(nextPosition - index0Position, (uint)(tuple.tableOffset - nextPosition));
            nextPosition += sizeof(uint);
        }

        if (pooledArray is not null)
        {
            ArrayPool<(long, int, long)>.Shared.Return(pooledArray);
        }
    }

    /// <summary>
    /// A partial introsort implementation, inspired by the Array.Sort implemenation from the CoreCLR. 
    /// Due to the amount of indirection in FlatBuffers, it's not possible to use the built-in sorting algorithms,
    /// so we do the next best thing. Note that this is not a true IntroSort, since we omit the HeapSort component.
    /// </summary>
    private static void IntroSort(
        BigSpan buffer,
        TSpanComparer keyComparer,
        int lo,
        int hi,
        Span<(long offset, int length, long tableOffset)> keyLocations)
    {
        checked
        {
            while (true)
            {
                if (hi <= lo)
                {
                    break;
                }

                int numElements = hi - lo + 1;
                if (numElements <= 16)
                {
                    switch (numElements)
                    {
                        case 1:
                            return;
                        case 2:
                            SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                            return;
                        case 3:
                            SwapIfGreater(buffer, keyComparer, lo, hi - 1, keyLocations);
                            SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                            SwapIfGreater(buffer, keyComparer, hi - 1, hi, keyLocations);
                            return;
                        default:
                            InsertionSort(buffer, keyComparer, lo, hi, keyLocations);
                            return;
                    }
                }

                // Use median-of-three partitioning.
                int middle = lo + ((hi - lo) >> 1);
                {
                    SwapIfGreater(buffer, keyComparer, lo, middle, keyLocations);
                    SwapIfGreater(buffer, keyComparer, lo, hi, keyLocations);
                    SwapIfGreater(buffer, keyComparer, middle, hi, keyLocations);
                }

                // Move the pivot to hi - 1 (since we know hi is already larger than the pivot).
                SwapVectorPositions(middle, hi - 1, keyLocations);
                var (pivotOffset, pivotLength, _) = keyLocations[hi - 1];
                bool pivotExists = pivotOffset != 0;
                var pivotSpan = buffer.ToSpan(pivotOffset, pivotLength);

                // Partition
                int num2 = lo;
                int num3 = hi - 1;
                while (num2 < num3)
                {
                    while (true)
                    {
                        ref (long keyOffset, int keyLength, long _) tuple = ref keyLocations[++num2];
                        var keySpan = buffer.ToSpan(tuple.keyOffset, tuple.keyLength);
                        if (keyComparer.Compare(tuple.keyOffset != 0, keySpan, pivotExists, pivotSpan) >= 0)
                        {
                            break;
                        }
                    }

                    while (true)
                    {
                        ref (long keyOffset, int keyLength, long _) tuple = ref keyLocations[--num3];
                        var keySpan = buffer.ToSpan(tuple.keyOffset, tuple.keyLength);
                        if (keyComparer.Compare(pivotExists, pivotSpan, tuple.keyOffset != 0, keySpan) >= 0)
                        {
                            break;
                        }
                    }

                    if (num2 < num3)
                    {
                        SwapVectorPositions(num2, num3, keyLocations);
                    }
                }

                SwapVectorPositions(num2, hi - 1, keyLocations);

                IntroSort(
                    buffer,
                    keyComparer,
                    num2 + 1,
                    hi,
                    keyLocations);

                hi = num2 - 1;
            }
        }
    }

    private static void InsertionSort(
        BigSpan buffer,
        TSpanComparer comparer,
        int lo,
        int hi,
        Span<(long offset, int length, long tableOffset)> keyLocations)
    {
        for (int i = lo; i < hi; i++)
        {
            int num = i;

            var valTuple = keyLocations[i + 1];
            ReadOnlySpan<byte> valSpan = buffer.ToSpan(valTuple.offset, valTuple.length);

            while (num >= lo)
            {
                ref (long keyOffset, int keyLength, long table) tuple = ref keyLocations[num];
                ReadOnlySpan<byte> keySpan = buffer.ToSpan(tuple.keyOffset, tuple.keyLength);

                if (comparer.Compare(valTuple.offset != 0, valSpan, tuple.keyOffset != 0, keySpan) < 0)
                {
                    keyLocations[num + 1] = keyLocations[num];
                    num--;
                }
                else
                {
                    break;
                }
            }

            keyLocations[num + 1] = valTuple;
        }
    }

    private static void SwapIfGreater(
        BigSpan target,
        TSpanComparer comparer,
        int leftIndex,
        int rightIndex,
        Span<(long, int, long)> keyOffsets)
    {
        ref (long offset, int length, long _) left = ref keyOffsets[leftIndex];
        ref (long offset, int length, long _) right = ref keyOffsets[rightIndex];

        bool leftExists = left.offset != 0;
        bool rightExists = right.offset != 0;

        Span<byte> leftSpan = default;
        Span<byte> rightSpan = default;

        if (leftExists)
        {
            leftSpan = target.ToSpan(left.offset, left.length);
        }

        if (rightExists)
        {
            rightSpan = target.ToSpan(right.offset, right.length);
        }

        if (comparer.Compare(leftExists, leftSpan, rightExists, rightSpan) > 0)
        {
            SwapVectorPositions(leftIndex, rightIndex, keyOffsets);
        }
    }

    private static void SwapVectorPositions(int leftIndex, int rightIndex, Span<(long, int, long)> keyOffsets)
    {
        checked
        {
            var temp = keyOffsets[leftIndex];
            keyOffsets[leftIndex] = keyOffsets[rightIndex];
            keyOffsets[rightIndex] = temp;
        }
    }

    /// <summary>
    /// For the given index in a vector, follow the indirection to return a tuple representing
    /// the key start offset, the key length, and the table offset. It's advantageous to return the
    /// tuple here since we can store that in a span.
    /// </summary>
    /// <remarks>
    /// Left as unchecked since this is a sort operation (not a search).
    /// </remarks>
    private static (long offset, int length, long tableOffset) GetKeyOffset(
        BigSpan buffer,
        long index0Position,
        int vectorIndex,
        int vtableIndex,
        int? inlineItemSize)
    {
        // Find offset to the table at the index.
        long tableOffset = index0Position + (sizeof(uint) * vectorIndex);
        tableOffset += (int)buffer.ReadUInt(tableOffset);

        // Consult the vtable.
        long vtableOffset = tableOffset - buffer.ReadInt(tableOffset);

        // Vtables have two extra entries: vtable length and table length. The number of entries is vtableLengthBytes / 2 - 2
        int vtableLengthEntries = (buffer.ReadUShort(vtableOffset) / 2) - 2;

        if (vtableIndex >= vtableLengthEntries)
        {
            return (0, 0, tableOffset);
        }

        // Absolute offset of the field within the table.
        long fieldOffset = tableOffset + buffer.ReadUShort(vtableOffset + 2 * (2 + vtableIndex));
        if (inlineItemSize is not null)
        {
            return (fieldOffset, inlineItemSize.Value, tableOffset);
        }

        if (fieldOffset == 0)
        {
            return (0, 0, tableOffset);
        }

        // Strings are stored as a uoffset reference. Follow the indirection one more time.
        long uoffsetToString = fieldOffset + (int)buffer.ReadUInt(fieldOffset);
        int stringLength = (int)buffer.ReadUInt(uoffsetToString);
        return (uoffsetToString + sizeof(uint), stringLength, tableOffset);
    }
}
