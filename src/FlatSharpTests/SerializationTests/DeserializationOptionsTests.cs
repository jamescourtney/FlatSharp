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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests that generated deserializers behave the way we expect.
    /// </summary>
    [TestClass]
    public class DeserializationOptionsTests
    {
        private static readonly string[] Strings = new[] { string.Empty, "a", "ab", "abc", "abcd", "abcde" };
        private static readonly byte[] Bytes = new byte[] { 1, 2, 3, 4, 5, 6 };
        private static readonly byte[] InputBuffer = new byte[10240];

        [TestMethod]
        public void DeserializationOption_Lazy_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.Lazy, Strings);

            Assert.AreEqual(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.IsFalse(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsFalse(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsFalse(object.ReferenceEquals(table.First, table.First));
            Assert.IsFalse(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotSupportedException>(() => table.Vector[0] = "foobar");
            Assert.ThrowsException<NotSupportedException>(() => table.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Lazy_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.Lazy, Strings);

            Assert.AreEqual(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.IsFalse(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsFalse(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsFalse(object.ReferenceEquals(table.First, table.First));
            Assert.IsFalse(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Lazy_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.Lazy, Strings);

            Assert.IsFalse(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsFalse(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsFalse(object.ReferenceEquals(table.First, table.First));
            Assert.IsFalse(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Lazy_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.Lazy, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsFalse(object.ReferenceEquals(table.First, table.First));
            Assert.IsFalse(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Lazy_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.Lazy, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsFalse(object.ReferenceEquals(table.First, table.First));
            Assert.IsFalse(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_PropertyCache_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.PropertyCache, Strings);

            Assert.AreEqual(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsFalse(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotSupportedException>(() => table.Vector[0] = "foobar");
            Assert.ThrowsException<NotSupportedException>(() => table.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_PropertyCache_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.PropertyCache, Strings);

            Assert.AreEqual(typeof(FlatBufferVector<,>), table.Vector.GetType().BaseType.GetGenericTypeDefinition());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsFalse(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_PropertyCache_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.PropertyCache, Strings);

            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_PropertyCache_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.PropertyCache, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_PropertyCache_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.PropertyCache, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_VectorCache_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.VectorCache, Strings);

            Assert.AreEqual(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsTrue(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotSupportedException>(() => table.Vector[0] = "foobar");
            Assert.ThrowsException<NotSupportedException>(() => table.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_VectorCache_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.VectorCache, Strings);

            Assert.AreEqual(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_VectorCache_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.VectorCache, Strings);

            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_VectorCache_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.VectorCache, Bytes);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_VectorCache_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.VectorCache, Bytes);
            string originalHash = this.GetInputBufferHash();

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
            Assert.AreEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_VectorCacheMutable_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.VectorCacheMutable, Strings);
            string originalHash = this.GetInputBufferHash();

            Assert.AreEqual(typeof(List<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsTrue(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);

            table.Vector[0] = "foobar";
            table.First = new FirstStruct { First = 10 };
            table.Vector[1] = "turkey";
            table.Vector.Clear();
            table.Second.Second = 3;

            Assert.AreEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_VectorCacheMutable_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.VectorCacheMutable, Strings);
            string originalHash = this.GetInputBufferHash();

            Assert.AreEqual(typeof(List<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;

            Assert.AreEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_VectorCacheMutable_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.VectorCacheMutable, Strings);
            string originalHash = this.GetInputBufferHash();

            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");

            Assert.AreEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_VectorCacheMutable_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.VectorCacheMutable, Bytes);
            string originalHash = this.GetInputBufferHash();

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");

            Assert.AreEqual(originalHash, this.GetInputBufferHash());

            table.Vector.Span[0] = byte.MaxValue;
            Assert.AreNotEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_VectorCacheMutable_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.VectorCacheMutable, Bytes);
            string originalHash = this.GetInputBufferHash();

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsTrue(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");

            Assert.AreEqual(originalHash, this.GetInputBufferHash());
        }

        [TestMethod]
        public void DeserializationOption_Greedy_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.AreEqual(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsTrue(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotSupportedException>(() => table.Vector[0] = "foobar");
            Assert.ThrowsException<NotSupportedException>(() => table.Vector.Clear());
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Greedy_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.AreEqual(typeof(ReadOnlyCollection<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Greedy_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.Greedy, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Greedy_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.Greedy, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Greedy makes a copy of the input memory.
            Assert.IsFalse(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_Greedy_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.Greedy, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Greedy makes a copy of the input memory.
            Assert.IsFalse(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);
            Assert.ThrowsException<NotMutableException>(() => table.First.First = 3);
            Assert.ThrowsException<NotMutableException>(() => table.First = null);
        }

        [TestMethod]
        public void DeserializationOption_GreedyMutable_IList()
        {
            var table = this.SerializeAndParse<IList<string>>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.AreEqual(typeof(List<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));

            var vector = table.Vector;
            Assert.IsTrue(object.ReferenceEquals(vector[5], vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);

            table.Vector[0] = "foobar";
            table.First = new FirstStruct { First = 10 };
            table.Vector[1] = "turkey";
            table.Vector.Clear();
            table.Second.Second = 3;
        }

        [TestMethod]
        public void DeserializationOption_GreedyMutable_IReadOnlyList()
        {
            var table = this.SerializeAndParse<IReadOnlyList<string>>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.AreEqual(typeof(List<string>), table.Vector.GetType());
            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Count);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
        }

        [TestMethod]
        public void DeserializationOption_GreedyMutable_Array()
        {
            var table = this.SerializeAndParse<string[]>(FlatBufferDeserializationOption.GreedyMutable, Strings);
            InputBuffer.AsSpan().Fill(0);

            Assert.IsTrue(object.ReferenceEquals(table.Vector, table.Vector));
            Assert.IsTrue(object.ReferenceEquals(table.Vector[5], table.Vector[5]));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));
            Assert.AreEqual(Strings.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        [TestMethod]
        public void DeserializationOption_GreedyMutable_Memory()
        {
            var table = this.SerializeAndParse<Memory<byte>>(FlatBufferDeserializationOption.GreedyMutable, Bytes); 
            InputBuffer.AsSpan().Fill(0);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsFalse(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        [TestMethod]
        public void DeserializationOption_GreedyMutable_ReadOnlyMemory()
        {
            var table = this.SerializeAndParse<ReadOnlyMemory<byte>>(FlatBufferDeserializationOption.GreedyMutable, Bytes);
            InputBuffer.AsSpan().Fill(0);

            // Each span overlaps the input buffer. Means we are not eagerly copying out.
            Assert.IsFalse(table.Vector.Span.Overlaps(InputBuffer));
            Assert.IsTrue(object.ReferenceEquals(table.First, table.First));
            Assert.IsTrue(object.ReferenceEquals(table.Second, table.Second));

            Assert.AreEqual(Bytes.Length, table.Vector.Length);

            table.First = new FirstStruct { First = 10 };
            table.Second.Second = 3;
            table.Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>("banana");
        }

        private string GetInputBufferHash()
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(InputBuffer);
                return Convert.ToBase64String(hash);
            }
        }

        private InnerTable<T> SerializeAndParse<T>(FlatBufferDeserializationOption option, T item)
        {
            FlatBufferSerializer serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(option));

            InnerTable<T> value = new InnerTable<T>
            {
                First = new FirstStruct { First = 1 },
                Second = new SecondStruct { Second = 2, },
                String = "Foo bar baz bat",
                Union = new FlatBufferUnion<FirstStruct, SecondStruct, string>(new SecondStruct { Second = 3 }),
                Vector = item,
            };

            serializer.Serialize(value, InputBuffer);
            InnerTable<T> parsed = serializer.Parse<InnerTable<T>>(InputBuffer); 

            Assert.AreEqual(1, parsed.First.First);
            Assert.AreEqual(2, parsed.Second.Second);
            Assert.AreEqual("Foo bar baz bat", parsed.String);
            Assert.AreEqual(3, parsed.Union.Item2.Second);

            return parsed;
        }

        [FlatBufferTable]
        public class InnerTable<TVector>
        {
            [FlatBufferItem(0)]
            public virtual TVector Vector { get; set; }

            [FlatBufferItem(1)]
            public virtual string String { get; set; }

            [FlatBufferItem(2)]
            public virtual FirstStruct First { get; set; }

            [FlatBufferItem(3)]
            public virtual SecondStruct Second { get; set; }

            [FlatBufferItem(4)]
            public virtual FlatBufferUnion<FirstStruct, SecondStruct, string> Union { get; set; }

            [FlatBufferItem(6)]
            public virtual ulong NoSetter { get; }
        }

        [FlatBufferStruct]
        public class FirstStruct
        {
            [FlatBufferItem(0)]
            public virtual int First { get; set; }

            [FlatBufferItem(1)]
            public virtual ulong NoSetter { get; }
        }

        [FlatBufferStruct]
        public class SecondStruct
        {
            [FlatBufferItem(0)]
            public virtual int Second { get; set; }
        }
    }
}
