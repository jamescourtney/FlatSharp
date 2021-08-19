/*
 * Copyright 2018 James Courtney
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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for primitive types.
    /// </summary>
    
    public class ScalarVectorTests
    {
        private static readonly Random r = new Random();

        [Fact]
        public void BoolVector()
        {
            this.TestType(() => r.Next() % 2 == 0, 0);
            this.TestType(() => r.Next() % 2 == 0, 1);
            this.TestType(() => r.Next() % 2 == 0, 10);
            this.TestType(() => r.Next() % 2 == 0, 500);
        }

        [Fact]
        public void ByteVector()
        {
            this.TestType(() => (byte)r.Next(), 0);
            this.TestType(() => (byte)r.Next(), 1);
            this.TestType(() => (byte)r.Next(), 10);
            this.TestType(() => (byte)r.Next(), 500);
        }

        [Fact]
        public void SByteVector()
        {
            this.TestType(() => (sbyte)r.Next(), 0);
            this.TestType(() => (sbyte)r.Next(), 1);
            this.TestType(() => (sbyte)r.Next(), 10);
            this.TestType(() => (sbyte)r.Next(), 500);
        }

        [Fact]
        public void UShortVector()
        {
            this.TestType(() => (ushort)r.Next(), 0);
            this.TestType(() => (ushort)r.Next(), 1);
            this.TestType(() => (ushort)r.Next(), 10);
            this.TestType(() => (ushort)r.Next(), 500);
        }

        [Fact]
        public void ShortVector()
        {
            this.TestType(() => (short)r.Next(), 0);
            this.TestType(() => (short)r.Next(), 1);
            this.TestType(() => (short)r.Next(), 10);
            this.TestType(() => (short)r.Next(), 500);
        }

        [Fact]
        public void UintVector()
        {
            this.TestType(() => (uint)r.Next(), 0);
            this.TestType(() => (uint)r.Next(), 1);
            this.TestType(() => (uint)r.Next(), 10);
            this.TestType(() => (uint)r.Next(), 500);
        }

        [Fact]
        public void IntVector()
        {
            this.TestType(() => r.Next(), 0);
            this.TestType(() => r.Next(), 1);
            this.TestType(() => r.Next(), 10);
            this.TestType(() => r.Next(), 500);
        }

        [Fact]
        public void ULongVector()
        {
            this.TestType(() => (ulong)r.Next(), 0);
            this.TestType(() => (ulong)r.Next(), 1);
            this.TestType(() => (ulong)r.Next(), 10);
            this.TestType(() => (ulong)r.Next(), 500);
        }

        [Fact]
        public void LongVector()
        {
            this.TestType(() => (long)r.Next(), 0);
            this.TestType(() => (long)r.Next(), 1);
            this.TestType(() => (long)r.Next(), 10);
            this.TestType(() => (long)r.Next(), 500);
        }

        [Fact]
        public void FloatVector()
        {
            this.TestType(() => (float)r.NextDouble(), 0);
            this.TestType(() => (float)r.NextDouble(), 1);
            this.TestType(() => (float)r.NextDouble(), 10);
            this.TestType(() => (float)r.NextDouble(), 500);
        }

        [Fact]
        public void DoubleVector()
        {
            this.TestType(() => r.NextDouble(), 0);
            this.TestType(() => r.NextDouble(), 1);
            this.TestType(() => r.NextDouble(), 10);
            this.TestType(() => r.NextDouble(), 500);
        }

        [Fact]
        public void EnumVector()
        {
            this.TestType(() => SimpleEnum.Bar, 0);
            this.TestType(() => SimpleEnum.Foo, 1);
            this.TestType(() => SimpleEnum.Bar, 10);
            this.TestType(() => SimpleEnum.Foo, 500);
        }

        private void TestType<T>(Func<T> generator, int length)
        {
            if (typeof(T) == typeof(byte))
            {
                var memoryTable = new RootTable<Memory<T>>
                {
                    Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<Memory<T>>
                    {
                        Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<Memory<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                Assert.Equal(length, resultVector.Length);
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.Equal(memoryTable.Vector.Span[i], resultVector.Span[i]);
                }
            }

            if (typeof(T) == typeof(byte))
            {
                var memoryTable = new RootTable<ReadOnlyMemory<T>>
                {
                    Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<ReadOnlyMemory<T>>
                    {
                        Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<ReadOnlyMemory<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                Assert.Equal(length, resultVector.Length);
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.Equal(memoryTable.Vector.Span[i], resultVector.Span[i]);
                }
            }

            {
                var memoryTable = new RootTable<IList<T>>
                {
                    Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<IList<T>>
                    {
                        Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<IList<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                Assert.Equal(length, resultVector.Count);
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.Equal(memoryTable.Vector[i], resultVector[i]);
                }
            }

            {
                var memoryTable = new RootTable<IReadOnlyList<T>>
                {
                    Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<IReadOnlyList<T>>
                    {
                        Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<IReadOnlyList<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                Assert.Equal(length, resultVector.Count);
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.Equal(memoryTable.Vector[i], resultVector[i]);
                }
            }

            {
                var memoryTable = new RootTable<T[]>
                {
                    Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<T[]>
                    {
                        Vector = Enumerable.Range(0, length).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<T[]>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                Assert.Equal(length, resultVector.Length);
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.Equal(memoryTable.Vector[i], resultVector[i]);
                }

                var inner = memoryTableResult.Inner;
                var innerVector = inner.Vector;
                Assert.Equal(length, innerVector.Length);
                for (int i = 0; i < innerVector.Length; ++i)
                {
                    Assert.Equal(memoryTable.Inner.Vector[i], innerVector[i]);
                }
            }
        }

        [FlatBufferEnum(typeof(long))]
        public enum SimpleEnum : long
        {
            Foo = 0,
            Bar = 1,
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }

            [FlatBufferItem(1)]
            public virtual InnerTable<TVector>? Inner { get; set; }
        }

        [FlatBufferTable]
        public class InnerTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }
        }
    }
}
