/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Internal;

/// <summary>
/// Represents a vtable for an arbitrary table.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public struct VTableGeneric : IVTable
{
    private int offset;
    private int maxIndex;

    public int MaxSupportedIndex => 255;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Create<TInputBuffer>(TInputBuffer buffer, int offset, out VTableGeneric item)
        where TInputBuffer : IInputBuffer
    {
        checked
        {
            item = new VTableGeneric();

            buffer.InitializeVTable(offset, out item.offset, out item.maxIndex, out _);
            item.offset += 2 * sizeof(ushort); // skip past vtable length and table length
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
       where TInputBuffer : IInputBuffer
    {
        if ((uint)index > this.maxIndex)
        {
            return 0;
        }

        return buffer.ReadUShort(this.offset + (index << 1));
    }
}
