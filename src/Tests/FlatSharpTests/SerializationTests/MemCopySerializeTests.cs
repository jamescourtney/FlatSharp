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
    using Xunit;

    /// <summary>
    /// Tests serialization using the memory copy shortcut.
    /// </summary>
    
    public class MemCopySerializeTests
    {
        [Fact]
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
            var compiled = serializer.Compile<TestTable<Struct>>().WithSettings(new SerializerSettings { EnableMemoryCopySerialization = true });

            byte[] data = new byte[1024];
            Assert.Equal(70, compiled.GetMaxSize(t));
            int actualBytes = compiled.Write(data, t);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = compiled.Parse(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = compiled.Write(data2, parsed);

            Assert.Equal(35, actualBytes);
            Assert.Equal(35, bytesWritten);
            Assert.Equal(70, compiled.GetMaxSize(parsed));
        }

        [Fact]
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
            var compiled = serializer.Compile<TestTable<WriteThroughStruct>>().WithSettings(new SerializerSettings { EnableMemoryCopySerialization = true });

            byte[] data = new byte[1024];
            Assert.Equal(70, compiled.GetMaxSize(t));
            int actualBytes = compiled.Write(data, t);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = compiled.Parse(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = compiled.Write(data2, parsed);

            Assert.Equal(35, actualBytes);
            Assert.Equal(1024, bytesWritten); // We use mem copy serialization here, and we gave it 1024 bytes when we parsed. So we get 1024 bytes back out.
            Assert.Equal(1024, compiled.GetMaxSize(parsed));
            Assert.Throws<BufferTooSmallException>(() => compiled.Write(new byte[35], parsed));

            // Repeat, but now using the trimmed array.
            parsed = compiled.Parse(data.AsMemory().Slice(0, actualBytes));
            bytesWritten = compiled.Write(data2, parsed);
            Assert.Equal(35, actualBytes);
            Assert.Equal(35, bytesWritten);
            Assert.Equal(35, compiled.GetMaxSize(parsed));

            // Default still returns 70.
            Assert.Equal(70, serializer.GetMaxSize(parsed));
        }

        [Fact]
        public void ProgressiveMemoryCopySerialization()
        {
            TestTable<Struct> t = new()
            {
                Foo = "foobar",
                Struct = new Struct
                {
                    Bar = 12,
                }
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Progressive);
            var compiled = serializer.Compile<TestTable<Struct>>().WithSettings(new SerializerSettings { EnableMemoryCopySerialization = true });

            byte[] data = new byte[1024];
            Assert.Equal(70, compiled.GetMaxSize(t));
            int actualBytes = compiled.Write(data, t);

            // First test: Parse the array but don't trim the buffer. This causes the underlying
            // buffer to be much larger than the actual data.
            var parsed = compiled.Parse(data);
            byte[] data2 = new byte[2048];
            int bytesWritten = compiled.Write(data2, parsed);

            Assert.Equal(35, actualBytes);
            Assert.Equal(1024, bytesWritten); // We use mem copy serialization here, and we gave it 1024 bytes when we parsed. So we get 1024 bytes back out.
            Assert.Equal(1024, compiled.GetMaxSize(parsed));
            Assert.Throws<BufferTooSmallException>(() => compiled.Write(new byte[35], parsed));

            // Repeat, but now using the trimmed array.
            parsed = compiled.Parse(data.AsMemory().Slice(0, actualBytes));
            bytesWritten = compiled.Write(data2, parsed);
            Assert.Equal(35, actualBytes);
            Assert.Equal(35, bytesWritten);
            Assert.Equal(35, compiled.GetMaxSize(parsed));

            // Default still returns 70.
            Assert.Equal(70, serializer.GetMaxSize(parsed));
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
