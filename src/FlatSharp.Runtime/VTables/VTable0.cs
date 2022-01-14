﻿/*
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
/// Represents a vtable for a table with no fields.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public struct VTable0 : IVTable
{
    public int MaxSupportedIndex => -1;

    public static void Create<TInputBuffer>(TInputBuffer buffer, int offset, out VTable0 item)
        where TInputBuffer : IInputBuffer
    {
        item = default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int OffsetOf<TInputBuffer>(TInputBuffer buffer, int index)
       where TInputBuffer : IInputBuffer
    {
        return 0;
    }
}
