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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the FlatBufferVector class that implements IList.
    /// </summary>
    [TestClass]
    public class FlatBufferVectorTests
    {
        private static readonly IReadOnlyList<string> ExpectedContents = new List<string> { "one", "two", "three", "four", "five" };
        private TableVector parsedVector;

        [TestInitialize]
        public void TestInitialize()
        {
            var originalVector = new TableVector
            {
                StringVector = ExpectedContents.ToList()
            };

            Span<byte> buffer = new byte[1024];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(originalVector, buffer);
            this.parsedVector = FlatBufferSerializer.Default.Parse<TableVector>(buffer.Slice(0, bytesWritten).ToArray());
        }

        [TestMethod]
        public void FlatBufferVector_OutOfRange()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => this.parsedVector.StringVector[-1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => this.parsedVector.StringVector[5]);
        }

        [TestMethod]
        public void FlatBufferVector_NotMutable()
        {
            Assert.IsTrue(this.parsedVector.StringVector.IsReadOnly);
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector[0] = "foobar");
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector.Add("foobar"));
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector.Clear());
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector.Insert(0, "foobar"));
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector.Remove("foobar"));
            Assert.ThrowsException<NotSupportedException>(() => this.parsedVector.StringVector.RemoveAt(0));
        }

        [TestMethod]
        public void FlatBufferVector_GetEnumerator()
        {
            IEnumerable<string> enumerable = this.parsedVector.StringVector;

            int i = 0; 
            foreach (var item in this.parsedVector.StringVector)
            {
                Assert.AreEqual(item, ExpectedContents[i]);
                i++;
            }
        }

        [TestMethod]
        public void FlatBufferVector_Contains()
        {
            Assert.IsTrue(this.parsedVector.StringVector.Contains("one"));
            Assert.IsFalse(this.parsedVector.StringVector.Contains("foobar"));
        }

        [TestMethod]
        public void FlatBufferVector_CopyTo()
        {
            string[] array = new string[100];

            this.parsedVector.StringVector.CopyTo(array, 50);
            
            for (int i = 0; i < 50; ++i)
            {
                Assert.IsNull(array[i]);
            }

            for (int i = 0; i < ExpectedContents.Count; ++i)
            {
                Assert.AreEqual(ExpectedContents[i], array[i + 50]);
            }

            for (int i = 50 + ExpectedContents.Count; i < array.Length; ++i)
            {
                Assert.IsNull(array[i]);
            }
        }

        [TestMethod]
        public void FlatBufferVector_IndexOf()
        {
            for (int i = 0; i < ExpectedContents.Count; ++i)
            {
                Assert.AreEqual(i, this.parsedVector.StringVector.IndexOf(ExpectedContents[i]));
            }

            Assert.AreEqual(-1, this.parsedVector.StringVector.IndexOf("foobar"));
        }

        [FlatBufferTable]
        public class TableVector
        {
            [FlatBufferItem(0)]
            public virtual IList<string> StringVector { get; set; }
        }
    }
}
