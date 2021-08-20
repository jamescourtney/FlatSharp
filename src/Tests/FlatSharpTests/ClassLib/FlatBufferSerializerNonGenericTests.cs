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

    public class FlatBufferSerializerNonGenericTests
    {
        [Fact]
        public void NonGenericSerializer_FromType()
        {
            ISerializer serializer = FlatBufferSerializer.Default.Compile(typeof(SomeTable));
            Assert.IsAssignableFrom<ISerializer<SomeTable>>(serializer);
            Assert.Equal(typeof(SomeTable), serializer.RootType);

            Assert.True(serializer.GetMaxSize(new SomeTable()) > 0);
            Assert.Throws<ArgumentNullException>(() => serializer.GetMaxSize(null));
            Assert.Throws<ArgumentException>(() => serializer.GetMaxSize(new SomeOtherTable()));

            byte[] data = new byte[1024];
            Assert.True(serializer.Write(data, new SomeTable { A = 3 }) > 0);
            Assert.Throws<ArgumentNullException>(() => serializer.Write(data, null));
            Assert.Throws<ArgumentException>(() => serializer.Write(data, new SomeOtherTable()));

            object parsed = serializer.Parse(data);
            Assert.True(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.NotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsAssignableFrom<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.Equal(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.Null(deserialized.InputBuffer); // greedy
            Assert.Equal(FlatBufferDeserializationOption.Default, deserialized.DeserializationContext.DeserializationOption);

            ISerializer parsedSerializer = FlatBufferSerializer.Default.Compile(deserialized);
            ISerializer parsedSerializer2 = FlatBufferSerializer.Default.Compile(parsed);

            Assert.Same(serializer, parsedSerializer);
            Assert.Same(serializer, parsedSerializer2);
        }

        [Fact]
        public void NonGenericSerializer_FromInstance()
        {
            var flatBufferSerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            ISerializer serializer = flatBufferSerializer.Compile(new SomeTable());
            Assert.IsAssignableFrom<ISerializer<SomeTable>>(serializer);
            Assert.Equal(typeof(SomeTable), serializer.RootType);

            Assert.True(serializer.GetMaxSize(new SomeTable()) > 0);
            Assert.Throws<ArgumentNullException>(() => serializer.GetMaxSize(null));
            Assert.Throws<ArgumentException>(() => serializer.GetMaxSize(new SomeOtherTable()));

            Memory<byte> data = new byte[1024];
            Assert.True(serializer.Write(data, new SomeTable { A = 3 }) > 0);
            Assert.Throws<ArgumentNullException>(() => serializer.Write(data, null));
            Assert.Throws<ArgumentException>(() => serializer.Write(data, new SomeOtherTable()));

            object parsed = serializer.Parse(data);
            Assert.True(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.NotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsAssignableFrom<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.Equal(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.NotNull(deserialized.InputBuffer); // lazy
            Assert.Equal(FlatBufferDeserializationOption.Lazy, deserialized.DeserializationContext.DeserializationOption);

            ISerializer parsedSerializer = flatBufferSerializer.Compile(deserialized);
            ISerializer parsedSerializer2 = flatBufferSerializer.Compile(parsed);

            Assert.Same(serializer, parsedSerializer);
            Assert.Same(serializer, parsedSerializer2);

            // Test that items deserialized from serializer 1 can be used by serializer 2. Why? Who knows!
            var flatBufferSerializer2 = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
            ISerializer serializer3 = flatBufferSerializer2.Compile(parsed);
            Assert.Equal(typeof(SomeTable), serializer3.RootType);
            Assert.NotSame(serializer, serializer3);
            serializer3.Write(data, parsed);
        }

        [FlatBufferTable]
        public class SomeTable
        {
            [FlatBufferItem(0)]
            public int A { get; set; }
        }


        [FlatBufferTable]
        public class SomeOtherTable
        {
            [FlatBufferItem(0)]
            public int A { get; set; }
        }
    }
}
