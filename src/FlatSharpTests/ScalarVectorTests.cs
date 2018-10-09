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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for primitive types.
    /// </summary>
    [TestClass]
    public class ScalarVectorTests
    {
        private static readonly Random r = new Random();

        [TestMethod]
        public void BoolVector()
        {
            this.TestType(() => r.Next() % 2 == 0);
        }

        [TestMethod]
        public void ByteVector()
        {
            this.TestType(() => (byte)r.Next());
        }

        [TestMethod]
        public void SByteVector()
        {
            this.TestType(() => (sbyte)r.Next());
        }

        [TestMethod]
        public void UShortVector()
        {
            this.TestType(() => (ushort)r.Next());
        }

        [TestMethod]
        public void ShortVector()
        {
            this.TestType(() => (short)r.Next());
        }

        [TestMethod]
        public void UintVector()
        {
            this.TestType(() => (uint)r.Next());
        }

        [TestMethod]
        public void IntVector()
        {
            this.TestType(() => r.Next());
        }

        [TestMethod]
        public void ULongVector()
        {
            this.TestType(() => (ulong)r.Next());
        }

        [TestMethod]
        public void LongVector()
        {
            this.TestType(() => r.Next());
        }

        [TestMethod]
        public void FloatVector()
        {
            this.TestType(() => (float)r.NextDouble());
        }

        [TestMethod]
        public void DoubleVector()
        {
            this.TestType(() => r.NextDouble());
        }

        private void TestType<T>(Func<T> generator) where T : IEquatable<T>
        {
            {
                var memoryTable = new RootTable<Memory<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<Memory<T>>
                    {
                        Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<Memory<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.AreEqual(memoryTable.Vector.Span[i], resultVector.Span[i]);
                }
            }

            {
                var memoryTable = new RootTable<ReadOnlyMemory<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<ReadOnlyMemory<T>>
                    {
                        Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<ReadOnlyMemory<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.AreEqual(memoryTable.Vector.Span[i], resultVector.Span[i]);
                }
            }

            {
                var memoryTable = new RootTable<IList<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<IList<T>>
                    {
                        Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<IList<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.AreEqual(memoryTable.Vector[i], resultVector[i]);
                }
            }

            {
                var memoryTable = new RootTable<IReadOnlyList<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<IReadOnlyList<T>>
                    {
                        Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<IReadOnlyList<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.AreEqual(memoryTable.Vector[i], resultVector[i]);
                }
            }

            {
                var memoryTable = new RootTable<T[]>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    Inner = new InnerTable<T[]>
                    {
                        Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray(),
                    }
                };

                Span<byte> memory = new byte[10240];
                int offset = FlatBufferSerializer.Default.Serialize(memoryTable, memory);
                var memoryTableResult = FlatBufferSerializer.Default.Parse<RootTable<T[]>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.AreEqual(memoryTable.Vector[i], resultVector[i]);
                }

                var inner = memoryTableResult.Inner;
                var innerVector = inner.Vector;
                for (int i = 0; i < innerVector.Length; ++i)
                {
                    Assert.AreEqual(memoryTable.Inner.Vector[i], innerVector[i]);
                }
            }
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }

            [FlatBufferItem(1)]
            public virtual InnerTable<TVector> Inner { get; set; }
        }

        [FlatBufferTable]
        public class InnerTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }
        }
    }
}
