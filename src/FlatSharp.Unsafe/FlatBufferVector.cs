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

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlatSharp.Unsafe
{

    using Unsafe = System.Runtime.CompilerServices.Unsafe;
    public static class FlatBufferVector
    {
#if NET5_0_OR_GREATER
        [return:System.Diagnostics.CodeAnalysis.NotNullIfNotNull("vector")]
#endif
        public static IVector<T>? Clone<T>(IVector<T>? vector)
        {
            if (vector is null) return null;
            var range = vector.GetByteMemory();
            var memory = new Memory<byte>(new byte[range.Length]);
            range.CopyTo(memory);
            return new Wrapper<T>(memory);
        }

        public static IVector<T> Create<T>(Memory<byte> memory)
        {
            return new Wrapper<T>(memory);
        }

        public static IVector<T> Create<T>(byte[] data)
        {
            return new Wrapper<T>(new(data));
        }

        public static IVector<T> Create<T>(T[] items)
        {
            return new Wrapper<T>(new ArrayMemoryManager<T>(items).Memory);
        }

        public static IVector<T> Empty<T>()
        {
            return new Wrapper<T>(Memory<byte>.Empty);
        }

        private class ArrayMemoryManager<T> : MemoryManager<byte>
        {
            private readonly Memory<T> memory;
            private MemoryHandle pin;
            public ArrayMemoryManager(T[] items)
            {
                this.memory = items;
            }
            
            public override Span<byte> GetSpan()
            {
                ref var start = ref Unsafe.As<T, byte>(ref this.memory.Span.GetPinnableReference());
#if NETSTANDARD2_0
                unsafe
                {
                    return new(Unsafe.AsPointer(ref start), Unsafe.SizeOf<T>() * memory.Length);
                }
#else
                return MemoryMarshal.CreateSpan(ref start, Unsafe.SizeOf<T>() * memory.Length);
#endif
            }

            public override MemoryHandle Pin(int elementIndex = 0)
            {
                return pin = this.memory.Pin();
            }
            public override void Unpin()
            {
                pin.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                pin.Dispose();
            }
        }

        private struct Wrapper<T> : IVector<T>
        {
            private Memory<byte> memory;

            private static int Size => Unsafe.SizeOf<T>();

            public Wrapper(Memory<byte> memory)
            {
                this.memory = memory;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private ref T Ref(int offset)
            {
                var slice = this.memory.Slice(offset, Size).Span;
                ref var startOfItem = ref slice.GetPinnableReference();
                return ref Unsafe.As<byte, T>(ref startOfItem);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory()
            {
                return this.memory;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory(int index)
            {
                var size = Size;
                return this.memory.Slice(index * size, size);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory(int index, int length)
            {
                var size = Size;
                return this.memory.Slice(index * size, size * length);
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (var offset = 0; offset < this.memory.Length; offset += Size)
                {
                    yield return Ref(offset);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(T item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(T item)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    return memory.Length / Size;
                }
            }

            public int Length
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    return Count;
                }
            }

            public bool IsReadOnly
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    return true;
                }
            }

            public int IndexOf(T item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public T this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    return Ref(index);
                }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set {
                    Ref(index) = value;
                }
            }
        }
    }
}
