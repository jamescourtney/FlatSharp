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
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    
    public class OnDeserializedMethodTests
    {
        [Fact]
        public void OnDeserializedMethods_Invoked()
        {
            OnDeserializedTable table = new OnDeserializedTable
            {
                Foo = "foo",
                Struct = new OnDeserializedStruct
                {
                    Foo = 4,
                }
            };

            byte[] data = new byte[1024];
            FlatBufferSerializer.Default.Serialize(table, data);

            var parsed = FlatBufferSerializer.Default.Parse<OnDeserializedTable>(data);

            Assert.True(parsed.OnDeserializedCalled);
            Assert.True(parsed.Struct.OnDeserializedCalled);
        }

        [FlatBufferTable]
        public class OnDeserializedTable
        {
            [FlatBufferItem(0)]
            public virtual string? Foo { get; set; }

            [FlatBufferItem(1)]
            public virtual OnDeserializedStruct? Struct { get; set; }

            protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
                this.OnDeserializedCalled = true;
            }

            public bool OnDeserializedCalled { get; set; }
        }

        [FlatBufferStruct]
        public class OnDeserializedStruct
        {
            [FlatBufferItem(0)]
            public int Foo { get; set; }

            protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
                this.OnDeserializedCalled = true;
            }

            public bool OnDeserializedCalled { get; set; }
        }
    }
}
