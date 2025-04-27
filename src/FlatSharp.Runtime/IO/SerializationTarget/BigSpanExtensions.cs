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

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

public static partial class BigSpanExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool UnsafeReadBool(this BigSpan span, long offset)
    {
        return span.UnsafeReadByte(offset) != SerializationHelpers.False;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool UnsafeReadBool(this BigReadOnlySpan span, long offset)
    {
        return span.Span.UnsafeReadBool(offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UnsafeWriteBool(this BigSpan span, long offset, bool value)
    {
        span.WriteByte(offset, value ? SerializationHelpers.True : SerializationHelpers.False);
    }
}