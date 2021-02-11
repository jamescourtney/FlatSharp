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
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeModelExtensionTests
    {
        [TestMethod]
        public void DateTimeOffsetExtension()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();
            
            container.WithExtension(
                new LongTypeModel(),
                l => DateTimeOffset.FromUnixTimeMilliseconds(l),
                dto => ToUnixFileTime(dto));

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy),
                container);

            byte[] destination = new byte[100];

            var compiled = serializer.Compile<ExtensionTable<DateTimeOffset>>();
            serializer.Serialize(
                new ExtensionTable<DateTimeOffset> { Item = DateTimeOffset.UtcNow },
                destination);
            var parsed = serializer.Parse<ExtensionTable<DateTimeOffset>>(destination);
            var maxSize = serializer.GetMaxSize(parsed);
        }

        [TestMethod]
        public void StringExtensionTest()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();

            container.WithExtension(
                new StringTypeModel(),
                s => FromString(s),
                s => FromString(s));

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.GreedyMutable),
                container);

            byte[] destination = new byte[100];

            var compiled = serializer.Compile<ExtensionTable<string>>();
            serializer.Serialize(
                new ExtensionTable<string> { Item = "foobar" },
                destination);

            try
            {
                var parsed = serializer.Parse<ExtensionTable<string>>(destination);
                var maxSize = serializer.GetMaxSize(parsed);
            }
            catch
            {

            }
        }

        public static long ToUnixFileTime(DateTimeOffset dto) => dto.ToUnixTimeMilliseconds();

        public static string ToString(string s) => s;

        public static string FromString(string s) => s;

        [FlatBufferTable]
        public class ExtensionTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T Item { get; set; }

            [FlatBufferItem(1)]
            public virtual IList<T> Vector { get; set; }

            [FlatBufferItem(2)]
            public virtual FlatBufferUnion<T> Union { get; set; }
        }
    }
}
