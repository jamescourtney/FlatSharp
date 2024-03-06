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

[TestClass]
public class FlatBufferSerializerNonGenericTests
{
    [TestMethod]
    public void NonGenericSerializer_FromInstance()
    {
        ISerializer serializer = (ISerializer)SomeTable.Serializer.WithSettings(s => s.UseLazyDeserialization());

        Assert.IsInstanceOfType<ISerializer<SomeTable>>(serializer);
        Assert.AreEqual(typeof(SomeTable), serializer.RootType);

        Assert.IsTrue(serializer.GetMaxSize(new SomeTable()) > 0);
        Assert.ThrowsException<ArgumentNullException>(() => serializer.GetMaxSize(null));
        Assert.ThrowsException<ArgumentException>(() => serializer.GetMaxSize(new SomeOtherTable()));

#if NET6_0_OR_GREATER
        {
            var bw = new ArrayBufferWriter<byte>();
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Write(bw, null));
            Assert.ThrowsException<ArgumentException>(() => serializer.Write(bw, new SomeOtherTable()));

            int written = serializer.Write(bw, new SomeTable { A = 3 });
            Assert.IsTrue(written > 0);
            Assert.AreEqual(written, bw.WrittenCount);

            object parsed = serializer.Parse(bw.WrittenMemory);
            Assert.IsTrue(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.AreNotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsInstanceOfType<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.AreEqual(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.IsNotNull(deserialized.InputBuffer); // lazy
            Assert.AreEqual(FlatBufferDeserializationOption.Lazy, deserialized.DeserializationContext.DeserializationOption);
            Assert.AreEqual(FlatBufferDeserializationOption.Lazy, serializer.DeserializationOption);
        }
#endif

        {
            var bw = new byte[1024];
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Write(bw, null));
            Assert.ThrowsException<ArgumentException>(() => serializer.Write(bw, new SomeOtherTable()));

            int written = serializer.Write(bw, new SomeTable { A = 3 });
            Assert.IsTrue(written > 0);

            object parsed = serializer.Parse(bw);
            Assert.IsTrue(typeof(SomeTable).IsAssignableFrom(parsed.GetType()));
            Assert.AreNotEqual(typeof(SomeTable), parsed.GetType());
            Assert.IsInstanceOfType<IFlatBufferDeserializedObject>(parsed);

            var deserialized = (IFlatBufferDeserializedObject)parsed;
            Assert.AreEqual(typeof(SomeTable), deserialized.TableOrStructType);
            Assert.IsNotNull(deserialized.InputBuffer); // lazy
            Assert.AreEqual(FlatBufferDeserializationOption.Lazy, deserialized.DeserializationContext.DeserializationOption);
            Assert.AreEqual(FlatBufferDeserializationOption.Lazy, serializer.DeserializationOption);
        }
    }
}
