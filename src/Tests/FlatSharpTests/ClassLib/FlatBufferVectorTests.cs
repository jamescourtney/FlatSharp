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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    
    public class FlatBufferVectorTests
    {
        private static readonly IReadOnlyList<string> ExpectedStringContents = new List<string> { "one", "two", "three", "four", "five" };
        private static readonly IReadOnlyList<int> ExpectedIntContents = new List<int> { 1, 2, 3, 4, 5 };

        private TableVector<string> stringVector;
        private TableVector<int> intVector;

        public FlatBufferVectorTests()
        {
            var originalVector = new TableVector<string>
            {
                Vector = ExpectedStringContents.ToList()
            };

            var originalIntVector = new TableVector<int> { Vector = ExpectedIntContents.ToList() };

            Span<byte> buffer = new byte[1024];

            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));
            int bytesWritten = serializer.Serialize(originalVector, buffer);
            this.stringVector = serializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());

            bytesWritten = serializer.Serialize(originalIntVector, buffer);
            this.intVector = serializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [Fact]
        public void FlatBufferVector_OutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => this.stringVector.Vector[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => this.stringVector.Vector[5]);
        }

        [Fact]
        public void FlatBufferVector_NotMutable()
        {
            Assert.True(this.stringVector.Vector.IsReadOnly);
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector[0] = "foobar");
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector.Add("foobar"));
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector.Insert(0, "foobar"));
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector.Remove("foobar"));
            Assert.Throws<NotMutableException>(() => this.stringVector.Vector.RemoveAt(0));
        }

        [Fact]
        public void FlatBufferVector_GetEnumerator()
        {
            IEnumerable<string> enumerable = this.stringVector.Vector;

            int i = 0; 
            foreach (var item in this.stringVector.Vector)
            {
                Assert.Equal(item, ExpectedStringContents[i]);
                i++;
            }
        }

        [Fact]
        public void FlatBufferVector_Contains()
        {
            Assert.True(this.stringVector.Vector.Contains("one"));
            Assert.False(this.stringVector.Vector.Contains("foobar"));
            Assert.False(this.stringVector.Vector.Contains(null));

            Assert.True(this.intVector.Vector.Contains(1));
            Assert.False(this.intVector.Vector.Contains(18));
        }

        [Fact]
        public void FlatBufferVector_CopyTo()
        {
            string[] array = new string[100];

            this.stringVector.Vector.CopyTo(array, 50);
            
            for (int i = 0; i < 50; ++i)
            {
                Assert.Null(array[i]);
            }

            for (int i = 0; i < ExpectedStringContents.Count; ++i)
            {
                Assert.Equal(ExpectedStringContents[i], array[i + 50]);
            }

            for (int i = 50 + ExpectedStringContents.Count; i < array.Length; ++i)
            {
                Assert.Null(array[i]);
            }
        }

        [Fact]
        public void FlatBufferVector_CopyTo_SizedArray()
        {
            string[] array = new string[this.stringVector.Vector.Count];
            this.stringVector.Vector.CopyTo(array, 0);

            for (int i = 0; i < ExpectedStringContents.Count; ++i)
            {
                Assert.Equal(ExpectedStringContents[i], array[i]);
            }
        }

        [Fact]
        public void FlatBufferVector_IndexOf()
        {
            for (int i = 0; i < ExpectedStringContents.Count; ++i)
            {
                Assert.Equal(i, this.stringVector.Vector.IndexOf(ExpectedStringContents[i]));
                Assert.Equal(i, this.intVector.Vector.IndexOf(ExpectedIntContents[i]));
            }

            Assert.Equal(-1, this.stringVector.Vector.IndexOf("foobar"));
            Assert.Equal(-1, this.stringVector.Vector.IndexOf(null));
            Assert.Equal(-1, this.intVector.Vector.IndexOf(int.MinValue));
        }

        [FlatBufferTable]
        public class TableVector<T>
        {
            [FlatBufferItem(0)]
            public virtual IList<T>? Vector { get; set; }
        }
    }
}
