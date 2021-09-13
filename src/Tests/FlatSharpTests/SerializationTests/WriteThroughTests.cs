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
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for primitive types.
    /// </summary>

    public class WriteThroughTests
    {
        [Fact]
        public void WriteThrough_InvalidDeserializationOption()
        {
            foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
            {
                if (option == FlatBufferDeserializationOption.Progressive || option == FlatBufferDeserializationOption.Lazy)
                {
                    continue;
                }

                var serializer = new FlatBufferSerializer(option);
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => serializer.Compile<Table<WriteThroughStruct<bool>>>());

                Assert.Equal(
                    "Property 'Value' of Struct 'FlatSharpTests.WriteThroughTests.WriteThroughStruct<System.Boolean>' specifies the WriteThrough option. However, WriteThrough is only supported when using deserialization option 'Progressive' or 'Lazy'.",
                    ex.Message);
            }
        }

        // Type model tests for write through scenarios.
        public class TypeModel
        {
            [Theory]
            [InlineData(typeof(ValueStruct))]
            [InlineData(typeof(ValueStruct?))]
            public void WriteThrough_Table_RequiredStructField(Type type)
            {
                type = typeof(WriteThroughTable_Required<>).MakeGenericType(type);

                var typeModel = RuntimeTypeModel.CreateFrom(type);
                Assert.True(((TableTypeModel)typeModel).IndexToMemberMap[0].IsWriteThrough);
                Assert.True(((TableTypeModel)typeModel).IndexToMemberMap[0].Attribute.WriteThrough);
            }

            [Theory]
            [InlineData(typeof(ValueStruct))]
            [InlineData(typeof(ValueStruct?))]
            public void WriteThrough_Table_NotRequiredStructField(Type type)
            {
                type = typeof(WriteThroughTable_NotRequired<>).MakeGenericType(type);

                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                    RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"Table property '{type.GetCompilableTypeName()}.Item' declared the WriteThrough attribute, but the field is not marked as required. WriteThrough fields must also be required.",
                    ex.Message);
            }

            [Theory]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<int>>))]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<string>>))]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<OtherStruct>>))]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<FlatBufferUnion<string>>>))]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<Table<int>>>))]
            [InlineData(typeof(WriteThroughTable_NotRequired<OtherStruct[]>))]
            // TODO: [InlineData(typeof(WriteThroughTable_NotRequired<ValueStruct[]>))]
            public void WriteThrough_Table_VectorOfDisallowed(Type type)
            {
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                    () => RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"Table property '{type.GetCompilableTypeName()}.Item' declared the WriteThrough on a vector. Vector WriteThrough is only valid for value type structs.",
                    ex.Message);
            }

            [Theory]
            [InlineData(typeof(WriteThroughTable_Required<string>))]
            [InlineData(typeof(WriteThroughTable_Required<Table<int>>))]
            [InlineData(typeof(WriteThroughTable_Required<FlatBufferUnion<string>?>))]
            [InlineData(typeof(WriteThroughTable_Required<OtherStruct>))]
            public void WriteThrough_Table_DisallowedTypes(Type type)
            {
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                    () => RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"Table property '{type.GetCompilableTypeName()}.Item' declared the WriteThrough attribute. WriteThrough on tables is only supported for value type structs.",
                    ex.Message);
            }

            [Theory]
            [InlineData(typeof(ValueStruct))]
            [InlineData(typeof(ValueStruct?))]
            public void WriteThrough_Table_NonVirtualStructField(Type type)
            {
                type = typeof(WriteThroughTable_Required_NonVirtual<>).MakeGenericType(type);

                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                    RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"Table member '{type.GetCompilableTypeName()}.Item' declared the WriteThrough attribute, but WriteThrough is only supported on virtual fields.",
                    ex.Message);
            }

            [Theory]
            [InlineData(typeof(WriteThroughTable_Required<IList<ValueStruct>>), false)]
            [InlineData(typeof(WriteThroughTable_NotRequired<IList<ValueStruct>>), false)]
            [InlineData(typeof(WriteThroughTable_Required<ValueStruct>), true)]
            [InlineData(typeof(WriteThroughTable_Required_NonVirtual<IList<ValueStruct>>), false)]
            public void WriteThrough_Table_ValidCases(Type innerType, bool expectWriteThrough)
            {
                var typeModel = RuntimeTypeModel.CreateFrom(innerType);

                Assert.Equal(
                    expectWriteThrough,
                    ((TableTypeModel)typeModel).IndexToMemberMap[0].IsWriteThrough);

                Assert.True(
                    ((TableTypeModel)typeModel).IndexToMemberMap[0].Attribute.WriteThrough);
            }
        }

        // Tests for writethrough inside of reference structs.
        public class ReferenceStructInteriorWriteThrough
        {
            [Fact]
            public void WriteThrough_SimpleInt_InReferenceStruct()
            {
                static void Test(FlatBufferDeserializationOption option)
                {
                    var table = new Table<WriteThroughStruct<int>>
                    {
                        Struct = new WriteThroughStruct<int>
                        {
                            Value = 5
                        }
                    };

                    FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                    byte[] buffer = new byte[1024];
                    serializer.Serialize(table, buffer);

                    // parse
                    var parsed1 = serializer.Parse<Table<WriteThroughStruct<int>>>(buffer);

                    // mutate
                    parsed1.Struct.Value = 300;
                    Assert.Equal(300, parsed1.Struct.Value);

                    // verify
                    var parsed2 = serializer.Parse<Table<WriteThroughStruct<int>>>(buffer);
                    Assert.Equal(300, parsed2.Struct.Value);
                }

                Test(FlatBufferDeserializationOption.Progressive);
                Test(FlatBufferDeserializationOption.Lazy);
            }

            [Fact]
            public void WriteThrough_NestedStruct_InReferenceStruct()
            {
                static void Test(FlatBufferDeserializationOption option)
                {
                    var table = new Table<WriteThroughStruct>
                    {
                        Struct = new WriteThroughStruct
                        {
                            Value = new OtherStruct { Prop1 = 10, Prop2 = 10 },
                            ValueStruct = new() { Value = 3, }
                        }
                    };

                    FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                    byte[] buffer = new byte[1024];
                    serializer.Serialize(table, buffer);

                    // parse
                    var parsed1 = serializer.Parse<Table<WriteThroughStruct>>(buffer);

                    // mutate
                    Assert.Equal(10, parsed1.Struct.Value.Prop1);
                    Assert.Equal(10, parsed1.Struct.Value.Prop2);
                    Assert.Equal(3, parsed1.Struct.ValueStruct.Value);
                    parsed1.Struct.Value = new OtherStruct { Prop1 = 300, Prop2 = 300 };
                    parsed1.Struct.ValueStruct = new() { Value = -1 };
                    Assert.Equal(300, parsed1.Struct.Value.Prop1);
                    Assert.Equal(300, parsed1.Struct.Value.Prop2);
                    Assert.Equal(-1, parsed1.Struct.ValueStruct.Value);

                    // verify, set to null
                    var parsed2 = serializer.Parse<Table<WriteThroughStruct>>(buffer);
                    Assert.Equal(300, parsed2.Struct.Value.Prop1);
                    Assert.Equal(300, parsed2.Struct.Value.Prop2);
                    Assert.Equal(-1, parsed2.Struct.ValueStruct.Value);
                    parsed2.Struct.Value = null!;

                    if (option == FlatBufferDeserializationOption.Progressive)
                    {
                        // we are null temporarily until we re-parse.
                        Assert.Null(parsed2.Struct.Value);
                    }
                    else if (option == FlatBufferDeserializationOption.Lazy)
                    {
                        // lazy write through clears it out.
                        Assert.Equal(0, parsed2.Struct.Value.Prop1);
                        Assert.Equal(0, parsed2.Struct.Value.Prop2);
                    }
                    else
                    {
                        Assert.False(true);
                    }

                    // verify, set to null
                    var parsed3 = serializer.Parse<Table<WriteThroughStruct>>(buffer);
                    Assert.Equal(0, parsed3.Struct.Value.Prop1);
                    Assert.Equal(0, parsed3.Struct.Value.Prop2);
                }

                Test(FlatBufferDeserializationOption.Progressive);
                Test(FlatBufferDeserializationOption.Lazy);
            }

            [Fact]
            public void WriteThrough_Vector_List()
            {
                static void Test(FlatBufferDeserializationOption option)
                {
                    var table = new Table<IList<WriteThroughStruct<long>>>
                    {
                        Struct = new List<WriteThroughStruct<long>>
                    {
                        new WriteThroughStruct<long> { Value = 5 }
                    }
                    };

                    FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                    byte[] buffer = new byte[1024];
                    serializer.Serialize(table, buffer);

                    // parse
                    var parsed1 = serializer.Parse<Table<IList<WriteThroughStruct<long>>>>(buffer);

                    // mutate
                    parsed1.Struct[0].Value = 300;
                    Assert.Equal(300, parsed1.Struct[0].Value);

                    // verify
                    var parsed2 = serializer.Parse<Table<IList<WriteThroughStruct<long>>>>(buffer);
                    Assert.Equal(300, parsed2.Struct[0].Value);
                }

                Test(FlatBufferDeserializationOption.Progressive);
                Test(FlatBufferDeserializationOption.Lazy);
            }

            [Fact]
            public void WriteThrough_Lazy_ThrowsForOtherProperties()
            {
                var table = new Table<OtherStruct>
                {
                    Struct = new OtherStruct { Prop1 = 1, Prop2 = 2 }
                };

                var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

                byte[] buffer = new byte[1024];
                serializer.Serialize(table, buffer);
                string csharp = serializer.Compile<Table<OtherStruct>>().CSharp;

                var parsed = serializer.Parse<Table<OtherStruct>>(buffer);

                Assert.Throws<NotMutableException>(() => parsed.Struct.Prop2 = 4);
            }
        }

        public class TableField
        {
            // Tests write through within a table.
            [Theory]
            [InlineData(FlatBufferDeserializationOption.Lazy)]
            [InlineData(FlatBufferDeserializationOption.Progressive)]
            public void Success_Nullable(FlatBufferDeserializationOption option)
            {
                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                WriteThroughTable_Required<ValueStruct?> table = new()
                {
                    Item = new ValueStruct { Value = 1 }
                };

                byte[] result = new byte[1024];

                var code = serializer.Compile<WriteThroughTable_Required<ValueStruct?>>().CSharp;
                serializer.Serialize(table, result);

                var parsed = serializer.Parse<WriteThroughTable_Required<ValueStruct?>>(result);
                Assert.Equal(1, parsed.Item.Value.Value);

                parsed.Item = new ValueStruct { Value = 4 };

                // Re-read and verify the in-struct writethrough succeeded.
                parsed = serializer.Parse<WriteThroughTable_Required<ValueStruct?>>(result);
                Assert.Equal(4, parsed.Item.Value.Value);

                var ex = Assert.Throws<InvalidOperationException>(() => parsed.Item = null);
                Assert.Equal(
                    "Nullable object must have a value.",
                    ex.Message);
            }

            [Theory]
            [InlineData(FlatBufferDeserializationOption.Lazy)]
            [InlineData(FlatBufferDeserializationOption.Progressive)]
            public void Success_NonNullable(FlatBufferDeserializationOption option)
            {
                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                WriteThroughTable_Required<ValueStruct> table = new()
                {
                    Item = new ValueStruct { Value = 1 }
                };

                byte[] result = new byte[1024];

                var code = serializer.Compile<WriteThroughTable_Required<ValueStruct>>().CSharp;
                serializer.Serialize(table, result);

                var parsed = serializer.Parse<WriteThroughTable_Required<ValueStruct>>(result);
                Assert.Equal(1, parsed.Item.Value);

                parsed.Item = new ValueStruct { Value = 4 };

                // Re-read and verify the in-struct writethrough succeeded.
                parsed = serializer.Parse<WriteThroughTable_Required<ValueStruct>>(result);
                Assert.Equal(4, parsed.Item.Value);
            }

            [Theory]
            [InlineData(FlatBufferDeserializationOption.Greedy)]
            [InlineData(FlatBufferDeserializationOption.GreedyMutable)]
            public void Failure_Greedy(FlatBufferDeserializationOption option)
            {
                FlatBufferSerializer serializer = new FlatBufferSerializer(option);
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => serializer.Compile<WriteThroughTable_Required<ValueStruct>>());

                Assert.Equal(
                    "Property 'Item' of Table 'FlatSharpTests.WriteThroughTests.WriteThroughTable_Required<FlatSharpTests.WriteThroughTests.ValueStruct>' specifies the WriteThrough option. However, WriteThrough is only supported when using deserialization option 'Progressive' or 'Lazy'.",
                    ex.Message);
            }
        }

        public class Vector
        {
            // Tests write through within a struct and write through of a whole struct.
            [Theory]
            [InlineData(FlatBufferDeserializationOption.Lazy)]
            [InlineData(FlatBufferDeserializationOption.Progressive)]
            public void Success(FlatBufferDeserializationOption option)
            {
                FlatBufferSerializer serializer = new FlatBufferSerializer(option);

                WriteThroughTable_NotRequired<IList<ValueStruct>> table = new()
                {
                    Item = new List<ValueStruct>
                    { 
                        new() { Value = 1 }
                    }
                };

                byte[] result = new byte[1024];
                serializer.Serialize(table, result);

                var parsed = serializer.Parse<WriteThroughTable_NotRequired<IList<ValueStruct>>>(result);
                Assert.Equal(1, parsed.Item[0].Value);

                parsed.Item[0] = new ValueStruct { Value = 4 };

                // Re-read and verify the in-struct writethrough succeeded.
                parsed = serializer.Parse<WriteThroughTable_NotRequired<IList<ValueStruct>>>(result);
                Assert.Equal(4, parsed.Item[0].Value);
            }

            // Tests combinations that get through type model validation but fail in practice.
            [Theory]
            [InlineData(FlatBufferDeserializationOption.Greedy, typeof(ValueStruct[]))]
            [InlineData(FlatBufferDeserializationOption.GreedyMutable, typeof(ValueStruct[]))]
            [InlineData(FlatBufferDeserializationOption.Greedy, typeof(IList<ValueStruct>))]
            [InlineData(FlatBufferDeserializationOption.GreedyMutable, typeof(IList<ValueStruct>))]
            public void Failures(FlatBufferDeserializationOption option, Type vectorType)
            {
                FlatBufferSerializer serializer = new FlatBufferSerializer(option);
                var tableType = typeof(WriteThroughTable_NotRequired<>).MakeGenericType(vectorType);

                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => serializer.Compile(tableType));

                Assert.Equal(
                    $"Field '{tableType.GetCompilableTypeName()}.Item' declares the WriteThrough option. WriteThrough is not supported when using Greedy deserialization.",
                    ex.Message);
            }
        }

        [FlatBufferTable]
        public class Table<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Struct { get; set; }
        }

        [FlatBufferTable]
        public class WriteThroughTable_Required<T>
        {
            [FlatBufferItem(0, WriteThrough = true, Required = true)]
            public virtual T? Item { get; set; }
        }

        [FlatBufferTable]
        public class WriteThroughTable_Required_NonVirtual<T>
        {
            [FlatBufferItem(0, WriteThrough = true, Required = true)]
            public T? Item { get; set; }
        }

        [FlatBufferTable]
        public class WriteThroughTable_NotRequired<T>
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual T? Item { get; set; }
        }

        [FlatBufferStruct]
        public class WriteThroughStruct<T>
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        public class WriteThroughStruct
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual OtherStruct Value { get; set; }

            [FlatBufferItem(1, WriteThrough = true)]
            public virtual ValueStruct ValueStruct { get; set; }
        }

        [FlatBufferStruct]
        public class OtherStruct
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int Prop1 { get; set; }

            [FlatBufferItem(1, WriteThrough = false)]
            public virtual int Prop2 { get; set; }
        }

        [FlatBufferStruct]
        [StructLayout(LayoutKind.Explicit, Size = 4)]
        public struct ValueStruct
        {
            [FieldOffset(0)] public int Value;
        }
    }
}
