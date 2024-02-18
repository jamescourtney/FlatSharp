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

using System.IO;

namespace FlatSharp.Internal;

[ExcludeFromCodeCoverage]
public static class FSThrow
{
    [DoesNotReturn]
    public static void InvalidOperation(string message) => throw new InvalidOperationException(message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T InvalidOperation<T>(string message)
    {
        InvalidOperation(message);
        return default;
    }

    [DoesNotReturn]
    public static void InvalidData(string message) => throw new InvalidDataException(message);

    [DoesNotReturn]
    public static void ArgumentOutOfRange(string paramName) => throw new ArgumentOutOfRangeException(paramName);

    [DoesNotReturn]
    public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ArgumentNull<T>(string paramName)
    {
        ArgumentNull(paramName);
        return default;
    }

    [DoesNotReturn]
    public static void ArgumentNull(string paramName, string message) => throw new ArgumentNullException(paramName, message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ArgumentNull<T>(string paramName, string message)
    {
        ArgumentNull(paramName, message);
        return default;
    }

    [DoesNotReturn]
    public static void Argument(string message) => throw new ArgumentException(message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Argument<T>(string message)
    {
        Argument(message);
        return default;
    }

    [DoesNotReturn]
    public static void BufferTooSmall(int sizeNeeded)
    {
        throw new BufferTooSmallException
        {
            SizeNeeded = sizeNeeded
        };
    }

    [DoesNotReturn]
    public static void NotMutable(string message) => throw new NotMutableException(message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotMutable<T>(string message)
    {
        NotMutable(message);
        return default;
    }

    [DoesNotReturn]
    public static void NotMutable() => throw new NotMutableException();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotMutable<T>()
    {
        NotMutable();
        return default;
    }

    [DoesNotReturn]
    public static void KeyNotFound() => throw new KeyNotFoundException();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T KeyNotFound<T>()
    {
        KeyNotFound();
        return default;
    }

    [DoesNotReturn]
    public static void IndexOutOfRange() => throw new IndexOutOfRangeException();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T IndexOutOfRange<T>()
    {
        IndexOutOfRange();
        return default;
    }

    [DoesNotReturn]
    public static void NotSupported(string s) => throw new NotSupportedException(s);
}
