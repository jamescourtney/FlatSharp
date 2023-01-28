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

public static class VectorUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckIndex(int index, int count)
    {
        if ((uint)index >= count)
        {
            throw new IndexOutOfRangeException();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowIndexOutOfRange()
    {
        throw new IndexOutOfRangeException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ThrowInlineNotMutableException()
    {
        throw new NotMutableException("FlatBufferVector does not support this operation.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool ThrowNotMutableException()
    {
        throw new NotMutableException("FlatBufferVector does not support this operation.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowGreedyMutableWriteThroughNotSupportedException()
    {
        throw new NotMutableException("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.");
    }
}