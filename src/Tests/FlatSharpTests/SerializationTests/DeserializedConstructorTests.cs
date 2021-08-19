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
    using FlatSharp.TypeModel;
    using Xunit;

    
    public class DeserializedConstructorTests
    {
        [Fact]
        public void Deserialized_PublicContextConstructor()
        {
            this.RunSuccessTest<
                PublicContextConstructorTable<PublicContextConstructorStruct>, 
                PublicContextConstructorStruct>();
        }

        [Fact]
        public void Deserialized_MixedContextConstructor_Public_Protected()
        {
            this.RunSuccessTest<
                PublicContextConstructorTable<ProtectedContextConstructorStruct>,
                ProtectedContextConstructorStruct>();
            
            this.RunSuccessTest<
                ProtectedContextConstructorTable<PublicContextConstructorStruct>,
                PublicContextConstructorStruct>();
        }

        [Fact]
        public void Deserialized_MixedContextConstructor_Public_ProtectedInternal()
        {
            this.RunSuccessTest<
                PublicContextConstructorTable<ProtectedInternalContextConstructorStruct>,
                ProtectedInternalContextConstructorStruct>();

            this.RunSuccessTest<
                ProtectedInternalContextConstructorTable<PublicContextConstructorStruct>,
                PublicContextConstructorStruct>();
        }

        [Fact]
        public void Deserialized_ProtectedContextConstructor()
        {
            this.RunSuccessTest<
                ProtectedContextConstructorTable<ProtectedContextConstructorStruct>,
                ProtectedContextConstructorStruct>();
        }

        [Fact]
        public void Deserialized_ProtectedInternalContextConstructor()
        {
            this.RunSuccessTest<
                ProtectedInternalContextConstructorTable<ProtectedInternalContextConstructorStruct>,
                ProtectedInternalContextConstructorStruct>();
        }

        [Fact]
        public void Deserialized_PrivateContextConstructor()
        {
            Assert.Throws<InvalidFlatBufferDefinitionException>(() => 
                this.RunSuccessTest<
                    PrivateContextConstructorTable<PrivateContextConstructorStruct>,
                    PrivateContextConstructorStruct>());
        }

        [Fact]
        public void Deserialized_MixedContextConstructor_Public_Private()
        {
            Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                this.RunSuccessTest<
                    PublicContextConstructorTable<PrivateContextConstructorStruct>,
                    PrivateContextConstructorStruct>());

            Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                this.RunSuccessTest<
                    PrivateContextConstructorTable<PublicContextConstructorStruct>,
                    PublicContextConstructorStruct>());
        }

        private void RunSuccessTest<TTable, TStruct>()
            where TTable : class, IContextItem, IContextTable<TStruct>, new()
            where TStruct : class, IContextItem, new()
        {
            TTable table = new TTable();

            byte[] data = new byte[1024];

            foreach (FlatBufferDeserializationOption item in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                var serializer = new FlatBufferSerializer(item);

                serializer.Serialize(table, data);
                TTable result = serializer.Parse<TTable>(data);

                Assert.Null(table.Context);
                Assert.Equal(item, result.Context.DeserializationOption);
                Assert.Equal(item, result.Struct.Context.DeserializationOption);
                Assert.False(object.ReferenceEquals(result.Context, result.Struct.Context));
            }
        }

        public interface IContextItem
        {
            FlatBufferDeserializationContext Context { get; }
        }

        public interface IContextTable<TStruct>
            where TStruct : IContextItem, new()
        {
            TStruct? Struct { get; set; }
        }

        [FlatBufferTable]
        public class PublicContextConstructorTable<TStruct> : IContextItem, IContextTable<TStruct>
            where TStruct : IContextItem, new()
        {
            public FlatBufferDeserializationContext Context { get; }

            public PublicContextConstructorTable() { }

            public PublicContextConstructorTable(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public string? Name { get; set; } = "Rocket";

            [FlatBufferItem(1)]
            public TStruct? Struct { get; set; } = new TStruct();
        }

        [FlatBufferStruct]
        public class PublicContextConstructorStruct : IContextItem
        {
            public FlatBufferDeserializationContext Context { get; }

            public PublicContextConstructorStruct() { }

            public PublicContextConstructorStruct(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public long Value { get; set; } = 123;
        }


        [FlatBufferTable]
        public class ProtectedContextConstructorTable<TStruct> : IContextItem, IContextTable<TStruct>
            where TStruct : IContextItem, new()
        {
            public FlatBufferDeserializationContext Context { get; }

            public ProtectedContextConstructorTable() { }

            protected ProtectedContextConstructorTable(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public string? Name { get; set; } = "Rocket";

            [FlatBufferItem(1)]
            public TStruct? Struct { get; set; } = new TStruct();
        }

        [FlatBufferStruct]
        public class ProtectedContextConstructorStruct : IContextItem
        {
            public FlatBufferDeserializationContext Context { get; }

            public ProtectedContextConstructorStruct() { }

            protected ProtectedContextConstructorStruct(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public long Value { get; set; } = 123;
        }

        [FlatBufferTable]
        public class ProtectedInternalContextConstructorTable<TStruct> : IContextItem, IContextTable<TStruct>
            where TStruct : IContextItem, new()
        {
            public FlatBufferDeserializationContext Context { get; }

            public ProtectedInternalContextConstructorTable() { }

            protected internal ProtectedInternalContextConstructorTable(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public string? Name { get; set; } = "Rocket";

            [FlatBufferItem(1)]
            public TStruct? Struct { get; set; } = new TStruct();
        }

        [FlatBufferStruct]
        public class ProtectedInternalContextConstructorStruct : IContextItem
        {
            public FlatBufferDeserializationContext Context { get; }

            public ProtectedInternalContextConstructorStruct() { }

            protected internal ProtectedInternalContextConstructorStruct(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public long Value { get; set; } = 123;
        }


        [FlatBufferTable]
        public class PrivateContextConstructorTable<TStruct> : IContextItem, IContextTable<TStruct>
            where TStruct : IContextItem, new()
        {
            public FlatBufferDeserializationContext Context { get; }

            public PrivateContextConstructorTable() { }

            private PrivateContextConstructorTable(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public string? Name { get; set; } = "Rocket";

            [FlatBufferItem(1)]
            public TStruct? Struct { get; set; } = new TStruct();
        }

        [FlatBufferStruct]
        public class PrivateContextConstructorStruct : IContextItem
        {
            public FlatBufferDeserializationContext Context { get; }

            public PrivateContextConstructorStruct() { }

            private PrivateContextConstructorStruct(FlatBufferDeserializationContext context)
            {
                this.Context = context;
            }

            [FlatBufferItem(0)]
            public long Value { get; set; } = 123;
        }
    }
}
