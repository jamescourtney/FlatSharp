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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    using Union = FlatSharp.FlatBufferUnion<string, FlatBufferVectorOfUnionTests.Struct, FlatBufferVectorOfUnionTests.Table>;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    
    public class FlatBufferVectorOfUnionTests
    {
        private TableVector vector;

        public FlatBufferVectorOfUnionTests()
        {
            var original = new TableVector
            {
                Vector = new[]
                {
                    new Union("foobar"),
                    new Union(new Struct { Value = 6 }),
                    new Union(new Table { Value = 5 }),
                }
            };

            Span<byte> buffer = new byte[1024];

            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));
            int bytesWritten = serializer.Serialize(original, buffer);
            this.vector = serializer.Parse<TableVector>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [Fact]
        public void FlatBufferVector_OutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => this.vector.Vector[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => this.vector.Vector[5]);
        }

        [Fact]
        public void FlatBufferVector_NotMutable()
        {
            Assert.True(this.vector.Vector.IsReadOnly);
            Assert.Throws<NotMutableException>(() => this.vector.Vector[0] = new Union("foobar"));
            Assert.Throws<NotMutableException>(() => this.vector.Vector.Add(new Union("foobar")));
            Assert.Throws<NotMutableException>(() => this.vector.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.vector.Vector.Insert(0, new Union("foobar")));
            Assert.Throws<NotMutableException>(() => this.vector.Vector.Remove(new Union("foobar")));
            Assert.Throws<NotMutableException>(() => this.vector.Vector.RemoveAt(0));
        }

        [Fact]
        public void FlatBufferVector_GetEnumerator()
        {
            int i = 0; 
            foreach (var item in this.vector.Vector)
            {
                Assert.Equal(i + 1, item.Discriminator);
                i++;
            }
        }

        [Fact]
        public void FlatBufferVector_Contains()
        {
            Assert.True(this.vector.Vector.Contains(new Union("foobar")));
            Assert.False(this.vector.Vector.Contains(new Union("blah")));
        }

        [Fact]
        public void FlatBufferVector_IndexOf()
        {
            Assert.Equal(0, this.vector.Vector.IndexOf(new Union("foobar")));
            Assert.Equal(-1, this.vector.Vector.IndexOf(new Union("monster")));
        }

        [FlatBufferTable]
        public class TableVector
        {
            [FlatBufferItem(0)]
            public virtual IList<Union>? Vector { get; set; }
        }

        [FlatBufferTable]
        public class Table
        {
            [FlatBufferItem(0)]
            public int Value { get; set; }
        }

        [FlatBufferStruct]
        public class Struct
        {
            [FlatBufferItem(0)]
            public int Value { get; set; }
        }
    }
}
