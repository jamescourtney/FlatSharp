/*
 * Copyright 2022 Unity Technologies
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
    public enum NativeArrayOptions
    {
        UninitializedMemory = 0,
        ClearMemory = 1
    }

    public enum Allocator
    {
        Invalid = 0,
        None = 1,
        Temp = 2,
        TempJob = 3,
        Persistent = 4,
    }

    // for testing, we remove the 'where T : struct' constraint and enforce this in UnityNativeArrayVectorTypeModel
    public unsafe struct NativeArray<T> : IEnumerable<T>, IEquatable<NativeArray<T>> /* where T : struct */
    {
        internal Memory<T> m_Buffer;

        internal void* m_Data;
        internal int m_Length;

        public NativeArray(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
        {
            Allocate(length, allocator, out this);
        }

        public NativeArray(T[] array, Allocator allocator)
        {
            Allocate(array.Length, allocator, out this);
            array.AsSpan().CopyTo(m_Buffer.Span);
        }        
        
        public NativeArray(NativeArray<T> array, Allocator allocator)
        {
            Allocate(array.Length, allocator, out this);
            array.AsSpan().CopyTo(this.AsSpan());
        }

        static void Allocate(int length, Allocator allocator, out NativeArray<T> nativeArray)
        {
            var backing = new T[length];
            nativeArray = new NativeArray<T>()
            {
                m_Buffer = new Memory<T>(backing)
            };
        }

        struct Enumerator : IEnumerator<T>
        {
            private readonly NativeArray<T> m_Array;
            private int m_Index;

            public Enumerator(NativeArray<T> nativeArray)
            {
                m_Array = nativeArray;
                m_Index = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                m_Index++;
                return m_Index < m_Array.Length;
            }

            public void Reset()
            {
                m_Index = -1;
            }

            // Let NativeArray indexer check for out of range.
            public T Current => m_Array[m_Index];

            object IEnumerator.Current => Current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(NativeArray<T> other)
        {
            throw new NotImplementedException();
        }

        public int Length => m_Data != null ? m_Length : m_Buffer.Length;

        public T this[int index]
        {
            get => AsSpan()[index];

            set => AsSpan()[index] = value;
        }

        public readonly Span<T> AsSpan()
        {
            return m_Data != null ? new Span<T>(m_Data, m_Length) : m_Buffer.Span;
        }
    }
}

namespace Unity.Collections.LowLevel.Unsafe
{
    public static unsafe class NativeArrayUnsafeUtility
    {
        public static NativeArray<T> ConvertExistingDataToNativeArray<T>(void* dataPointer, int length, Allocator allocator) where T : struct
        {
            var newArray = new NativeArray<T>
            {
                m_Data = dataPointer,
                m_Length = length,
            };

            return newArray;
        }
    }


    public static unsafe class NativeArrayUnsafeUtilityEx
    {
        public static NativeArray<T> ConvertExistingDataToNativeArray<T>(Span<T> span, Allocator allocator) where T : struct
        {
            var newArray = new NativeArray<T>
            {
                m_Data = System.Runtime.CompilerServices.Unsafe.AsPointer(ref span.GetPinnableReference()),
                m_Length = span.Length,
            };

            return newArray;
        }
    }
}
