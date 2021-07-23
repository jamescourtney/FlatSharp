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
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests serialization using the memory copy shortcut.
    /// </summary>
    [TestClass]
    public class MemCopySerializeTests
    {
        [TestMethod]
        public void GreedyMemoryCopySerialization_NoEffect()
        {
            TestTable<Struct> t = new()
            {
                Foo = "foobar",
                Struct = new Struct
                {
                    Bar = 12,
                }
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.GreedyMutable);
            serializer.Compile<TestTable<Struct>>().EnableMemoryCopySerialization = true;

            byte[] data = new byte[1024];
            Assert.AreEqual(70, serializer.GetMaxSize(t));
            int actualBytes = serializer.Serialize(t, data);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = serializer.Parse<TestTable<Struct>>(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = serializer.Serialize(parsed, data2);

            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(35, bytesWritten);
            Assert.AreEqual(70, serializer.GetMaxSize(parsed));
        }

        [TestMethod]
        public void VectorCacheMemoryCopySerialization_NoEffect()
        {
            TestTable<Struct> t = new()
            {
                Foo = "foobar",
                Struct = new Struct
                {
                    Bar = 12,
                }
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.VectorCacheMutable);
            serializer.Compile<TestTable<Struct>>().EnableMemoryCopySerialization = true;

            byte[] data = new byte[1024];
            Assert.AreEqual(70, serializer.GetMaxSize(t));
            int actualBytes = serializer.Serialize(t, data);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = serializer.Parse<TestTable<Struct>>(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = serializer.Serialize(parsed, data2);

            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(35, bytesWritten);
            Assert.AreEqual(70, serializer.GetMaxSize(parsed));
        }

        [TestMethod]
        public void LazyMemoryCopySerialization()
        {
            TestTable<WriteThroughStruct> t = new()
            {
                Foo = "foobar",
                Struct = new WriteThroughStruct
                {
                    Bar = 12,
                }
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
            serializer.Compile<TestTable<WriteThroughStruct>>().EnableMemoryCopySerialization = true;

            byte[] data = new byte[1024];
            Assert.AreEqual(70, serializer.GetMaxSize(t));
            int actualBytes = serializer.Serialize(t, data);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = serializer.Parse<TestTable<WriteThroughStruct>>(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = serializer.Serialize(parsed, data2);

            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(1024, bytesWritten); // We use mem copy serialization here, and we gave it 1024 bytes when we parsed. So we get 1024 bytes back out.
            Assert.AreEqual(1024, serializer.GetMaxSize(parsed));
            Assert.ThrowsException<BufferTooSmallException>(() => serializer.Serialize(parsed, new byte[35]));

            // Repeat, but now using the trimmed array.
            parsed = serializer.Parse<TestTable<WriteThroughStruct>>(data.AsMemory().Slice(0, actualBytes));
            bytesWritten = serializer.Serialize(parsed, data2);
            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(35, bytesWritten);
            Assert.AreEqual(35, serializer.GetMaxSize(parsed));

            serializer.Compile<TestTable<WriteThroughStruct>>().EnableMemoryCopySerialization = false;
            Assert.AreEqual(70, serializer.GetMaxSize(parsed));
        }

        [TestMethod]
        public void PropertyCacheMemoryCopySerialization()
        {
            TestTable<Struct> t = new()
            {
                Foo = "foobar",
                Struct = new Struct
                {
                    Bar = 12,
                }
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.PropertyCache);
            serializer.Compile<TestTable<Struct>>().EnableMemoryCopySerialization = true;

            byte[] data = new byte[1024];
            Assert.AreEqual(70, serializer.GetMaxSize(t));
            int actualBytes = serializer.Serialize(t, data);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = serializer.Parse<TestTable<Struct>>(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = serializer.Serialize(parsed, data2);

            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(1024, bytesWritten); // We use mem copy serialization here, and we gave it 1024 bytes when we parsed. So we get 1024 bytes back out.
            Assert.AreEqual(1024, serializer.GetMaxSize(parsed));
            Assert.ThrowsException<BufferTooSmallException>(() => serializer.Serialize(parsed, new byte[35]));

            // Repeat, but now using the trimmed array.
            parsed = serializer.Parse<TestTable<Struct>>(data.AsMemory().Slice(0, actualBytes));
            bytesWritten = serializer.Serialize(parsed, data2);
            Assert.AreEqual(35, actualBytes);
            Assert.AreEqual(35, bytesWritten);
            Assert.AreEqual(35, serializer.GetMaxSize(parsed));
        }

        [FlatBufferTable]
        public class TestTable<T> where T : IInt
        {
            [FlatBufferItem(0)]
            public virtual string? Foo { get; set; }

            [FlatBufferItem(1)]
            public virtual T? Struct { get; set; }
        }

        public interface IInt
        {
            int Bar { get; set; }
        }

        [FlatBufferStruct]
        public class WriteThroughStruct : IInt
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int Bar { get; set; }
        }

        [FlatBufferStruct]
        public class Struct : IInt
        {
            [FlatBufferItem(0)]
            public virtual int Bar { get; set; }
        }
    }
}
