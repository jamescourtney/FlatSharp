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
        private static readonly IReadOnlyList<int> ExpectedIntContents = Enumerable.Range(1, 2000).ToList();

        private TableVector<string> stringVector;
        private TableVector<string> progressiveStringVector;

        private TableVector<int> intVector;
        private TableVector<int> progressiveIntVector;

        public FlatBufferVectorTests()
        {
            var originalVector = new TableVector<string>
            {
                Vector = ExpectedStringContents.ToList()
            };

            var originalIntVector = new TableVector<int> { Vector = ExpectedIntContents.ToList() };

            Span<byte> buffer = new byte[1024 * 1024];

            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));
            var progressiveSerializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Progressive));

            int bytesWritten = serializer.Serialize(originalVector, buffer);
            this.stringVector = serializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());
            this.progressiveStringVector = progressiveSerializer.Parse<TableVector<string>>(buffer.Slice(0, bytesWritten).ToArray());

            bytesWritten = serializer.Serialize(originalIntVector, buffer);
            this.intVector = serializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
            this.progressiveIntVector = progressiveSerializer.Parse<TableVector<int>>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [Fact]
        public void FlatBufferVector_OutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => this.stringVector.Vector[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => this.stringVector.Vector[5]);

            Assert.Throws<IndexOutOfRangeException>(() => this.progressiveStringVector.Vector[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => this.progressiveStringVector.Vector[5]);
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

            Assert.True(this.progressiveStringVector.Vector.IsReadOnly);
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector[0] = "foobar");
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector.Add("foobar"));
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector.Clear());
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector.Insert(0, "foobar"));
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector.Remove("foobar"));
            Assert.Throws<NotMutableException>(() => this.progressiveStringVector.Vector.RemoveAt(0));
        }

        [Fact]
        public void FlatBufferVector_GetEnumerator()
        {
            int i = 0; 
            foreach (var item in this.stringVector.Vector)
            {
                Assert.Equal(item, ExpectedStringContents[i]);
                i++;
            }

            i = 0;
            foreach (var item in this.progressiveStringVector.Vector)
            {
                Assert.Equal(item, ExpectedStringContents[i]);
                i++;
            }

            i = 0;
            foreach (var item in this.intVector.Vector)
            {
                Assert.Equal(item, ExpectedIntContents[i]);
                i++;
            }

            i = 0;
            foreach (var item in this.progressiveIntVector.Vector)
            {
                Assert.Equal(item, ExpectedIntContents[i]);
                i++;
            }
        }

        [Fact]
        public void FlatBufferVector_Contains()
        {
            Assert.True(this.stringVector.Vector.Contains("one"));
            Assert.False(this.stringVector.Vector.Contains("foobar"));
            Assert.False(this.stringVector.Vector.Contains(null));

            Assert.True(this.progressiveStringVector.Vector.Contains("one"));
            Assert.False(this.progressiveStringVector.Vector.Contains("foobar"));
            Assert.False(this.progressiveStringVector.Vector.Contains(null));

            Assert.False(this.intVector.Vector.Contains(-1));
            Assert.False(this.progressiveIntVector.Vector.Contains(-1));

            foreach (var i in ExpectedIntContents)
            {
                Assert.True(this.intVector.Vector.Contains(i));
                Assert.True(this.progressiveIntVector.Vector.Contains(i));
            }
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

            array = new string[100];

            this.progressiveStringVector.Vector.CopyTo(array, 50);

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

            array = new string[this.progressiveStringVector.Vector.Count];
            this.progressiveStringVector.Vector.CopyTo(array, 0);

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
                Assert.Equal(i, this.progressiveStringVector.Vector.IndexOf(ExpectedStringContents[i]));
            }

            for (int i = 0; i < ExpectedIntContents.Count; ++i)
            {
                Assert.Equal(i, this.intVector.Vector.IndexOf(ExpectedIntContents[i]));
                Assert.Equal(i, this.progressiveIntVector.Vector.IndexOf(ExpectedIntContents[i]));
            }

            Assert.Equal(-1, this.stringVector.Vector.IndexOf("foobar"));
            Assert.Equal(-1, this.stringVector.Vector.IndexOf(null));
            Assert.Equal(-1, this.progressiveStringVector.Vector.IndexOf("foobar"));
            Assert.Equal(-1, this.progressiveStringVector.Vector.IndexOf(null));
            Assert.Equal(-1, this.intVector.Vector.IndexOf(int.MinValue));
        }

        [Fact]
        public void FlatBufferVector_Caching()
        {
            Assert.NotSame(this.stringVector.Vector[0], this.stringVector.Vector[0]);
            Assert.Same(this.progressiveStringVector.Vector[0], this.progressiveStringVector.Vector[0]);
        }

        [FlatBufferTable]
        public class TableVector<T>
        {
            [FlatBufferItem(0)]
            public virtual IList<T>? Vector { get; set; }
        }
    }
}
