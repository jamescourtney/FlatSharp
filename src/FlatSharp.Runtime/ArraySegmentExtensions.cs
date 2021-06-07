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
namespace FlatSharp.Internal
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Array segment extensions for FlatSharp.
    /// </summary>
    /// <remarks>
    /// NetStandard2.0 and .NET Framework do not support the indexer on ArraySegment. This is unfortunate.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)] // prevent this from leaking into other scopes, hopefully.
    public static class ArraySegmentExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(this ArraySegment<T> segment, int index)
        {
#if NETCOREAPP
            return segment[index];
#else
            if ((uint)index >= (uint)segment.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return segment.Array[checked(segment.Offset + index)];
#endif
        }
    }
}