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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Helper methods for deep-cloning vectors.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class VectorCloneHelpers
    {
        public static IList<T>? Clone<T>(IList<T>? source)
            where T : struct
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Count;
            List<T> newList = new List<T>(count);

            for (int i = 0; i < count; ++i)
            {
                newList.Add(source[i]);
            }

            return newList;
        }

        public static IReadOnlyList<T>? Clone<T>(IReadOnlyList<T>? source)
            where T : struct
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Count;
            List<T> newList = new List<T>(count);

            for (int i = 0; i < count; ++i)
            {
                newList.Add(source[i]);
            }

            return newList;
        }

        public static IReadOnlyList<T>? Clone<T>(IReadOnlyList<T>? source, Func<T, T> cloneItem)
            where T : class
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Count;
            List<T> newList = new List<T>(count);
            for (int i = 0; i < count; ++i)
            {
                newList.Add(cloneItem(source[i]));
            }

            return newList;
        }

        public static IList<T>? Clone<T>(IList<T>? source, Func<T, T> cloneItem)
            where T : class
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Count;
            List<T> newList = new List<T>(count);
            for (int i = 0; i < count; ++i)
            {
                newList.Add(cloneItem(source[i]));
            }

            return newList;
        }

        public static T[]? Clone<T>(T[]? source)
            where T : struct
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Length;
            T[] clone = new T[count];

            source.CopyTo(clone, 0);
            return clone;
        }

        public static T[]? Clone<T>(T[]? source, Func<T, T> cloneItem)
            where T : class
        {
            if (source is null)
            {
                return null;
            }

            int count = source.Length;
            T[] clone = new T[count];
            for (int i = 0; i < count; ++i)
            {
                clone[i] = cloneItem(source[i]);
            }

            return clone;
        }

        public static IIndexedVector<TKey, TValue>? Clone<TKey, TValue>(IIndexedVector<TKey, TValue>? source, Func<TValue, TValue> clone)
            where TKey : notnull
            where TValue : class
        {
            if (source is null)
            {
                return null;
            }

            IndexedVector<TKey, TValue> vector = new IndexedVector<TKey, TValue>(source.Count);
            foreach (var pair in source)
            {
                vector.Add(clone(pair.Value));
            }

            return vector;
        }
    }
}
