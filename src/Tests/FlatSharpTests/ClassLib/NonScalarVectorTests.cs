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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for primitive types.
    /// </summary>
    
    public class NonScalarVectorTests
    {
        private static readonly Random r = new Random();
        
        [Fact]
        public void StringVector()
        {
            this.TestType(
                false,
                () =>
                {
                    byte[] b = new byte[20];
                    r.NextBytes(b);
                    return Convert.ToBase64String(b);
                });
        }

        [Fact]
        public void StringVector_Cache()
        {
            this.TestType(
                true,
                () =>
                {
                    byte[] b = new byte[20];
                    r.NextBytes(b);
                    return Convert.ToBase64String(b);
                });
        }

        [Fact]
        public void TableVector()
        {
            this.TestType(
                false,
                () =>
                {
                    byte[] b = new byte[20];
                    r.NextBytes(b);
                    return new InnerTable { Value = Convert.ToBase64String(b) };
                });
        }

        [Fact]
        public void TableVector_Cache()
        {
            this.TestType(
                true,
                () =>
                {
                    byte[] b = new byte[20];
                    r.NextBytes(b);
                    return new InnerTable { Value = Convert.ToBase64String(b) };
                });
        }

        [Fact]
        public void StructVector()
        {
            this.TestType(
                false,
                () =>
                {
                    return new InnerStruct { Value = r.Next() };
                });
        }

        [Fact]
        public void StructVector_Cache()
        {
            this.TestType(
                true,
                () =>
                {
                    return new InnerStruct { Value = r.Next() };
                });
        }

        private void TestType<T>(bool listCache, Func<T> generator) where T : IEquatable<T>
        {
            var options = new FlatBufferSerializerOptions(listCache ? FlatBufferDeserializationOption.Progressive : FlatBufferDeserializationOption.Lazy);
            FlatBufferSerializer serializer = new FlatBufferSerializer(options);

            {
                var memoryTable = new RootTable<IList<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray()
                };

                Span<byte> memory = new byte[10240];
                int offset = serializer.Serialize(memoryTable, memory);
                var memoryTableResult = serializer.Parse<RootTable<IList<T>>>(memory.Slice(0, offset).ToArray());

                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.Equal<T>(memoryTable.Vector[i], resultVector[i]);

                    // reference equality should correspond to the serializer.
                    Assert.Equal(listCache, object.ReferenceEquals(resultVector[i], resultVector[i])); 
                }
            }

            {
                var memoryTable = new RootTable<IReadOnlyList<T>>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray()
                };

                Span<byte> memory = new byte[10240];
                int offset = serializer.Serialize(memoryTable, memory);
                var memoryTableResult = serializer.Parse<RootTable<IReadOnlyList<T>>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
                {
                    Assert.Equal(memoryTable.Vector[i], resultVector[i]);

                    // reference equality should correspond to the serializer.
                    Assert.Equal(listCache, object.ReferenceEquals(resultVector[i], resultVector[i]));
                }
            }

            {
                var memoryTable = new RootTable<T[]>
                {
                    Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray()
                };

                Span<byte> memory = new byte[10240];
                int offset = serializer.Serialize(memoryTable, memory);
                var memoryTableResult = serializer.Parse<RootTable<T[]>>(memory.Slice(0, offset).ToArray());
                var resultVector = memoryTableResult.Vector;
                for (int i = 0; i < memoryTableResult.Vector.Length; ++i)
                {
                    Assert.Equal(memoryTable.Vector[i], resultVector[i]);
                }
            }
        }

        [FlatBufferTable]
        public class RootTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector? Vector { get; set; }
        }

        [FlatBufferTable]
        public class InnerTable : IEquatable<InnerTable>
        {
            [FlatBufferItem(0)]
            public virtual string? Value { get; set; }

            public bool Equals(InnerTable other)
            {
                return other?.Value == this.Value;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as InnerTable);
            }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }
        }

        [FlatBufferStruct]
        public class InnerStruct : IEquatable<InnerStruct>
        {
            [FlatBufferItem(0)]
            public virtual int Value { get; set; }

            public bool Equals(InnerStruct other)
            {
                return other?.Value == this.Value;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as InnerStruct);
            }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }
        }
    }
}
