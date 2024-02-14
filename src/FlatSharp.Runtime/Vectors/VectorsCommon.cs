/*
 * Copyright 2023 James Courtney
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
/// Common methods for all vectors.
/// </summary>
public static class VectorsCommon
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(IList<T> vector, T? item)
    {
        return IndexOf<T>(vector, item) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(IList<T> vector, T? item)
    {
        if (!typeof(T).IsValueType)
        {
            if (item is null)
            {
                return -1;
            }
        }

        int count = vector.Count;
        for (int i = 0; i < count; ++i)
        {
            if (item!.Equals(vector[i]))
            {
                return i;
            }
        }

        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<T>(IList<T> vector, T[]? array, int arrayIndex)
    {
        if (array is null)
        {
            FSThrow.ArgumentNull(nameof(array));
        }

        var count = vector.Count;
        for (int i = 0; i < count; ++i)
        {
            array[arrayIndex + i] = vector[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerator<T> GetEnumerator<T>(IList<T> vector)
    {
        int count = vector.Count;
        for (int i = 0; i < count; ++i)
        {
            yield return vector[i];
        }
    }
}