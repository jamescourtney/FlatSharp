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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Tests the 'required' attribute.
    /// </summary>
    
    public class RequiredTests
    {
        [Fact]
        public void Required_FieldNotPresent_ThrowsOnSerialize()
        {
            static void RunTest<T>()
            {
                var item = new RequiredTable<T>() { Item = default };
                var ex = Assert.Throws<InvalidOperationException>(() => FlatBufferSerializer.Default.Serialize(item, new byte[1024]));

                Assert.Equal(
                    $"Table property '{item.GetType().GetCompilableTypeName()}.Item' is marked as required, but was not set.",
                    ex.Message);
            }

            RunTest<string>();
            RunTest<IList<string>>();
            RunTest<Table<int>>();
            RunTest<Memory<byte>?>();
        }

        [Fact]
        public void Required_FieldNotPresent_ThrowsOnParse_Greedy()
        {
            static void RunTest<T>()
            {
                var item = new Table<T>() { Item = default };
                byte[] data = new byte[1024];

                var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Greedy);
                serializer.Serialize(item, data);
                var ex = Assert.Throws<System.IO.InvalidDataException>(() => serializer.Parse<RequiredTable<T>>(data));

                Assert.Equal(
                    $"Table property '{typeof(RequiredTable<T>).GetCompilableTypeName()}.Item' is marked as required, but was missing from the buffer.",
                    ex.Message);
            }

            RunTest<string>();
            RunTest<IList<string>>();
            RunTest<Table<int>>();
            RunTest<Memory<byte>?>();
        }

        [FlatBufferTable]
        public class RequiredTable<T>
        {
            [FlatBufferItem(0, Required = true)]
            public virtual T Item { get; set; }
        }

        [FlatBufferTable]
        public class Table<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }
        }
    }
}
