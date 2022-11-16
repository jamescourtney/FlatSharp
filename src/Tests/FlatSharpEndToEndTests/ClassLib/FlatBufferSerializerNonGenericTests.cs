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

namespace FlatSharpEndToEndTests.ClassLib.FlatBufferSerializerNonGenericTests;

public class FlatBufferSerializerNonGenericTests
{
    [Fact]
    public void NonGenericSerializer_FromInstance()
    {
        ISerializer serializer = (ISerializer)SomeTable.Serializer.WithSettings(s => s.UseLazyDeserialization());

        Assert.IsAssignableFrom<ISerializer<SomeTable>>(serializer);
        Assert.Equal(typeof(SomeTable), serializer.RootType);

        Assert.True(serializer.GetMaxSize(new SomeTable()) > 0);
        Assert.Throws<ArgumentNullException>(() => serializer.GetMaxSize(null));
        Assert.Throws<ArgumentException>(() => serializer.GetMaxSize(new SomeOtherTable()));

#if NET6_0_OR_GREATER
        {
            var bw = new ArrayBufferWriter<byte>();
            Assert.Throws<ArgumentNullException>(() => serializer.Write(bw, null));
            Assert.Throws<ArgumentException>(() => serializer.Write(bw, new SomeOtherTable()));

            int written = serializer.Write(bw, new SomeTable { A = 3 });
            Assert.True(written > 0);
            Assert.Equal(written, bw.WrittenCount);

            object parsed = serializer.Parse(bw.WrittenMemory);
            Assert.True(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.NotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsAssignableFrom<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.Equal(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.NotNull(deserialized.InputBuffer); // lazy
            Assert.Equal(FlatBufferDeserializationOption.Lazy, deserialized.DeserializationContext.DeserializationOption);
            Assert.Equal(FlatBufferDeserializationOption.Lazy, serializer.DeserializationOption);
        }
#endif

        {
            var bw = new byte[1024];
            Assert.Throws<ArgumentNullException>(() => serializer.Write(bw, null));
            Assert.Throws<ArgumentException>(() => serializer.Write(bw, new SomeOtherTable()));

            int written = serializer.Write(bw, new SomeTable { A = 3 });
            Assert.True(written > 0);

            object parsed = serializer.Parse(bw);
            Assert.True(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.NotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsAssignableFrom<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.Equal(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.NotNull(deserialized.InputBuffer); // lazy
            Assert.Equal(FlatBufferDeserializationOption.Lazy, deserialized.DeserializationContext.DeserializationOption);
            Assert.Equal(FlatBufferDeserializationOption.Lazy, serializer.DeserializationOption);
        }
    }
}
