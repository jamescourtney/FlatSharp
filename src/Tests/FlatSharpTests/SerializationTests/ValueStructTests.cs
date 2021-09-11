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
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Tests for ByValue structs.
    /// </summary>
    
    public class ValueStructTests
    {
        public class TypeModel
        {
            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), true, true)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false)]
            public void ValidStructs(Type type, bool marshalSerialize, bool marshalParse)
            {
                ITypeModel typeModel = RuntimeTypeModel.CreateFrom(type);
                var tm = Assert.IsType<ValueStructTypeModel>(typeModel);

                var children = tm.Children.ToArray();
                Assert.Equal(typeof(byte), children[0].ClrType);
                Assert.Equal(typeof(short), children[1].ClrType);
                Assert.Equal(typeof(int), children[2].ClrType);
                Assert.Equal(typeof(long), children[3].ClrType);
                Assert.Equal(typeof(ValidStruct_Inner), children[4].ClrType);
                Assert.Equal(marshalSerialize, tm.CanMarshalOnSerialize);
                Assert.Equal(marshalParse, tm.CanMarshalOnParse);
            }

            [Theory]
            [InlineData(typeof(MarshalAlways), true, true, true)]
            [InlineData(typeof(MarshalParse), true, false, true)]
            [InlineData(typeof(MarshalSerialize), true, true, false)]
            [InlineData(typeof(MarshalNever), true, false, false)]
            [InlineData(typeof(MarshalDefault_Simple), true, false, false)] // simple structs are not marshalled.
            [InlineData(typeof(MarshalDefault_Complex_DueToInnerStruct), true, true, true)]
            [InlineData(typeof(MarshalDefault_Complex_DueToLotsOfFields), true, true, true)]
            [InlineData(typeof(MarshalAlways), false, true, true)]
            [InlineData(typeof(MarshalParse), false, false, true)]
            [InlineData(typeof(MarshalSerialize), false, true, false)]
            [InlineData(typeof(MarshalNever), false, false, false)]
            [InlineData(typeof(MarshalDefault_Simple), false, false, false)] // simple structs are not marshalled.
            [InlineData(typeof(MarshalDefault_Complex_DueToInnerStruct), false, true, true)]
            [InlineData(typeof(MarshalDefault_Complex_DueToLotsOfFields), false, true, true)]
            public void MarshalConfigTests(Type type, bool marshalEnable, bool marshalSerialize, bool marshalParse)
            {
                ITypeModel typeModel = RuntimeTypeModel.CreateFrom(type);
                var tm = Assert.IsType<ValueStructTypeModel>(typeModel);
                
                Assert.Equal(marshalSerialize, tm.CanMarshalOnSerialize);
                Assert.Equal(marshalParse, tm.CanMarshalOnParse);

                var options = new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshalEnable };

                CodeGeneratedMethod serializeMethod = tm.CreateSerializeMethodBody(ContextHelpers.CreateSerializeContext(options));
                CodeGeneratedMethod parseMethod = tm.CreateParseMethodBody(ContextHelpers.CreateParserContext(options));

                Assert.Equal(parseMethod.MethodBody.Contains("MemoryMarshal.Cast"), marshalEnable && marshalParse);
                Assert.Equal(serializeMethod.MethodBody.Contains("MemoryMarshal.Cast"), marshalEnable && marshalSerialize);
            }

            [Theory]
            [InlineData(typeof(InvalidStruct_Sequential), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Sequential' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_Auto), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Auto' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_None), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_None' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_Empty), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Empty' is empty or has no public fields.")]
            [InlineData(typeof(InvalidStruct_NoPublicFields), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NoPublicFields' field Foo is not public and does not declare a custom accessor. Non-public fields must also specify a custom accessor.")]
            [InlineData(typeof(InvalidStruct_ContainsReferenceType), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_ContainsReferenceType' field Foo must be a value type if the struct is a value type.")]
            [InlineData(typeof(InvalidStruct_WrongFieldOffset), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_WrongFieldOffset' property 'B' defines invalid [FieldOffset] attribute. Expected: [FieldOffset(8)].")]
            [InlineData(typeof(InvalidStruct_NotValidStructMember), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NotValidStructMember' field A cannot be part of a flatbuffer struct. Structs may only contain scalars and other structs.")]
            [InlineData(typeof(InvalidStruct_NotPublic), "Can't create type model from type FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NotPublic because it is not public.")]
            public void InvalidStructs(Type structType, string expectedError)
            {
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(structType));
                Assert.Equal(expectedError, ex.Message);
            }

            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), 20, sizeof(ulong))]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), 20, sizeof(ulong))]
            [InlineData(typeof(FiveByteStruct), 5, sizeof(byte))]
            [InlineData(typeof(NineByteStruct), 9, sizeof(ulong))]
            [InlineData(typeof(SixteenByteStruct), 16, sizeof(ulong))]
            public void LayoutTests(Type structType, int expectedSize, int expectedAlignment)
            {
                ValueStructTypeModel model = (ValueStructTypeModel)RuntimeTypeModel.CreateFrom(structType);

                var layout = Assert.Single(model.PhysicalLayout);

                Assert.False(model.SerializeMethodRequiresContext);
                Assert.True(model.SerializesInline);
                Assert.True(model.IsValidStructMember);
                Assert.True(model.IsValidTableMember);
                Assert.True(model.IsValidUnionMember);
                Assert.True(model.IsValidVectorMember);
                Assert.False(model.IsValidSortedVectorKey);
                Assert.True(model.IsFixedSize);

                Assert.Equal(FlatBufferSchemaType.Struct, model.SchemaType);
                Assert.Equal(
                    SerializationHelpers.GetMaxPadding(expectedAlignment) + expectedSize,
                    model.MaxInlineSize);

                Assert.Equal(expectedSize, layout.InlineSize);
                Assert.Equal(expectedAlignment, layout.Alignment);
                Assert.Equal(Marshal.SizeOf(structType), layout.InlineSize);
            }

            [FlatBufferStruct, StructLayout(LayoutKind.Sequential)]
            public struct InvalidStruct_Sequential { public byte A; }

            [FlatBufferStruct, StructLayout(LayoutKind.Auto)]
            public struct InvalidStruct_Auto { public byte A; }

            [FlatBufferStruct]
            public struct InvalidStruct_None { public byte A; }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            public struct InvalidStruct_Empty { }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            public struct InvalidStruct_NoPublicFields {[FieldOffset(0)] private int Foo; }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            public struct InvalidStruct_ContainsReferenceType {[FieldOffset(0)] public ReferenceStruct Foo; }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            public struct InvalidStruct_WrongFieldOffset { [FieldOffset(0)] public long A; [FieldOffset(1)] public byte B; }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            public struct InvalidStruct_NotValidStructMember {[FieldOffset(0)] public string A; }

            [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
            internal struct InvalidStruct_NotPublic {[FieldOffset(0)] public int Foo; }
        }

        public class SerializationTests
        {
            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false)]
            public void Serialize_Nullable(Type type, bool enableMarshal, bool expectMarshal)
            {
                Type tableType = typeof(SimpleTableNullable<>).MakeGenericType(type);

                ISimpleTable simpleTable = (ISimpleTable)Activator.CreateInstance(tableType);

                IValidStruct s = (IValidStruct)Activator.CreateInstance(type);
                s.IA = 1;
                s.IB = 2;
                s.IC = 3;
                s.ID = 4;
                s.IInner = new ValidStruct_Inner { Test = 5 };

                simpleTable.Item = s;

                var fbs = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = enableMarshal });

                ISerializer serializer = fbs.Compile(simpleTable);
                byte[] data = new byte[1024];
                int bytesWritten = serializer.Write(data, simpleTable);

                byte[] expectedData =
                {
                    4, 0, 0, 0,
                    232, 255, 255, 255,
                    1, 0,
                    2, 0,
                    3, 0, 0, 0,
                    4, 0, 0, 0, 0, 0, 0, 0,
                    5, 0, 0, 0,
                    6, 0,
                    24, 0,
                    4, 0,
                };

                Assert.Equal(expectedData.Length, bytesWritten);
                Assert.True(expectedData.AsSpan().SequenceEqual(data.AsSpan().Slice(0, bytesWritten)));

                Assert.Equal(
                    expectMarshal,
                    serializer.CSharp.Contains($"MemoryMarshal.Cast<byte, {type.GetGlobalCompilableTypeName()}>"));
            }

            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false)]
            public void Serialize_NonNullable(Type type, bool enableMarshal, bool expectMarshal)
            {
                Type tableType = typeof(SimpleTableNonNullable<>).MakeGenericType(type);

                ISimpleTable simpleTable = (ISimpleTable)Activator.CreateInstance(tableType);

                IValidStruct s = (IValidStruct)Activator.CreateInstance(type);
                s.IA = 1;
                s.IB = 2;
                s.IC = 3;
                s.ID = 4;
                s.IInner = new ValidStruct_Inner { Test = 5 };

                simpleTable.Item = s;

                var fbs = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = enableMarshal });

                ISerializer serializer = fbs.Compile(simpleTable);
                byte[] data = new byte[1024];
                int bytesWritten = serializer.Write(data, simpleTable);

                byte[] expectedData =
                {
                    4, 0, 0, 0,
                    232, 255, 255, 255,
                    1, 0,
                    2, 0,
                    3, 0, 0, 0,
                    4, 0, 0, 0, 0, 0, 0, 0,
                    5, 0, 0, 0,
                    6, 0,
                    24, 0,
                    4, 0,
                };

                Assert.Equal(expectedData.Length, bytesWritten);
                Assert.True(expectedData.AsSpan().SequenceEqual(data.AsSpan().Slice(0, bytesWritten)));

                Assert.Equal(
                    expectMarshal,
                    serializer.CSharp.Contains($"MemoryMarshal.Cast<byte, {type.GetGlobalCompilableTypeName()}>"));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Serialize_NineByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    242, 255, 255, 255,
                    1, 0, 0, 0, 0, 0, 0, 0,
                    2,
                    0,
                    6, 0,
                    13, 0,
                    4, 0,
                };

                var table = new SimpleTableAnything<NineByteStruct>
                {
                    Item = new NineByteStruct
                    {
                        A = 1,
                        B = 2,
                    }
                };

                byte[] buffer = new byte[1024];

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                int bytesWritten = serializer.Serialize(table, buffer);

                Assert.True(data.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, bytesWritten)));
                Assert.Equal(marshal, serializer.Compile(table.GetType()).CSharp.Contains("MemoryMarshal.Cast"));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Serialize_FiveByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    246, 255, 255, 255,
                    1, 2, 3, 4, 5, 0,
                    6, 0,
                    9, 0,
                    4, 0,
                };

                SimpleTableAnything<FiveByteStruct> source = new()
                {
                    Item = new FiveByteStruct
                    {
                        A = 1,
                        B = 2,
                        C = 3,
                        D = 4,
                        E = 5,
                    }
                };

                byte[] buffer = new byte[1024];

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                var written = serializer.Serialize(source, buffer);

                Assert.True(data.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, written)));
                Assert.Equal(marshal, serializer.Compile(source.GetType()).CSharp.Contains("MemoryMarshal.Cast"));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Serialize_SixteenByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    236, 255, 255, 255,
                    1, 0, 0, 0, 0, 0, 0, 0,
                    2, 0, 0, 0, 0, 0, 0, 0,
                    6, 0,
                    20, 0,
                    4, 0,
                };

                SimpleTableAnything<SixteenByteStruct?> source = new()
                {
                    Item = new SixteenByteStruct
                    {
                        A = 1,
                        B = 2ul,
                    }
                };

                byte[] buffer = new byte[1024];

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                var written = serializer.Serialize(source, buffer);

                Assert.True(data.AsSpan().SequenceEqual(buffer.AsSpan().Slice(0, written)));
                Assert.Equal(marshal, serializer.Compile(source.GetType()).CSharp.Contains("MemoryMarshal.Cast"));
            }
        }

        public class ParseTests
        {
            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true, FlatBufferDeserializationOption.Greedy)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false, FlatBufferDeserializationOption.Lazy)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), true, true, FlatBufferDeserializationOption.Progressive)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), false, false, FlatBufferDeserializationOption.GreedyMutable)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), true, false, FlatBufferDeserializationOption.Progressive)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false, FlatBufferDeserializationOption.Lazy)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), true, false, FlatBufferDeserializationOption.Progressive)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false, FlatBufferDeserializationOption.GreedyMutable)]
            public void Parse_Nullable(Type type, bool enableMarshal, bool expectMarshal, FlatBufferDeserializationOption option)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    232, 255, 255, 255,
                    1, 0,
                    2, 0,
                    3, 0, 0, 0,
                    4, 0, 0, 0, 0, 0, 0, 0,
                    5, 0, 0, 0,
                    6, 0,
                    24, 0,
                    4, 0,
                };

                Type tableType = typeof(SimpleTableNullable<>).MakeGenericType(type);
                var fbs = new FlatBufferSerializer(new FlatBufferSerializerOptions(option) { EnableValueStructMemoryMarshalDeserialization = enableMarshal });

                ISerializer serializer = fbs.Compile(tableType);
                ISimpleTable table = (ISimpleTable)serializer.Parse(data);

                Assert.Equal(
                    expectMarshal,
                    serializer.CSharp.Contains($"MemoryMarshal.Cast<byte, {type.GetGlobalCompilableTypeName()}>"));

                Assert.Equal(1, table.Item.IA);
                Assert.Equal(2, table.Item.IB);
                Assert.Equal(3, table.Item.IC);
                Assert.Equal(4, table.Item.ID);
                Assert.Equal(5, table.Item.IInner.Test);
            }

            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_OutOfOrder), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false)]
            public void Parse_NonNullable(Type type, bool enableMarshal, bool expectMarshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    232, 255, 255, 255,
                    1, 0,
                    2, 0,
                    3, 0, 0, 0,
                    4, 0, 0, 0, 0, 0, 0, 0,
                    5, 0, 0, 0,
                    6, 0,
                    24, 0,
                    4, 0,
                };

                Type tableType = typeof(SimpleTableNonNullable<>).MakeGenericType(type);
                var fbs = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = enableMarshal });

                ISerializer serializer = fbs.Compile(tableType);
                ISimpleTable table = (ISimpleTable)serializer.Parse(data);

                Assert.Equal(
                    expectMarshal,
                    serializer.CSharp.Contains($"MemoryMarshal.Cast<byte, {type.GetGlobalCompilableTypeName()}>"));

                Assert.Equal(1, table.Item.IA);
                Assert.Equal(2, table.Item.IB);
                Assert.Equal(3, table.Item.IC);
                Assert.Equal(4, table.Item.ID);
                Assert.Equal(5, table.Item.IInner.Test);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Parse_NineByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    242, 255, 255, 255,
                    1, 0, 0, 0, 0, 0, 0, 0,
                    2,
                    0,
                    6, 0,
                    13, 0,
                    4, 0,
                };

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                var parsed = serializer.Parse<SimpleTableAnything<NineByteStruct?>>(data);

                Assert.NotNull(parsed.Item);
                Assert.Equal(1ul, parsed.Item.Value.A);
                Assert.Equal(2, parsed.Item.Value.B);

                Assert.Equal(marshal, serializer.Compile<SimpleTableAnything<NineByteStruct?>>().CSharp.Contains("MemoryMarshal.Cast"));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Parse_FiveByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    246, 255, 255, 255,
                    1, 2, 3, 4, 5, 0,
                    6, 0,
                    9, 0,
                    4, 0,
                };

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                var parsed = serializer.Parse<SimpleTableAnything<FiveByteStruct?>>(data);

                Assert.NotNull(parsed.Item);
                Assert.Equal(1, parsed.Item.Value.A);
                Assert.Equal(2, parsed.Item.Value.B);
                Assert.Equal(3, parsed.Item.Value.C);
                Assert.Equal(4, parsed.Item.Value.D);
                Assert.Equal(5, parsed.Item.Value.E);

                Assert.Equal(marshal, serializer.Compile<SimpleTableAnything<FiveByteStruct?>>().CSharp.Contains("MemoryMarshal.Cast"));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Parse_SixteenByte(bool marshal)
            {
                byte[] data =
                {
                    4, 0, 0, 0,
                    236, 255, 255, 255,
                    1, 0, 0, 0, 0, 0, 0, 0,
                    2, 0, 0, 0, 0, 0, 0, 0,
                    6, 0,
                    20, 0,
                    4, 0,
                };

                var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = marshal });
                var parsed = serializer.Parse<SimpleTableAnything<SixteenByteStruct?>>(data);

                Assert.NotNull(parsed);
                Assert.NotNull(parsed.Item);
                Assert.Equal(1, parsed.Item.Value.A);
                Assert.Equal(2ul, parsed.Item.Value.B);

                Assert.Equal(marshal, serializer.Compile<SimpleTableAnything<SixteenByteStruct?>>().CSharp.Contains("MemoryMarshal.Cast"));
            }
        }

        public class FormattingTests
        {
            [Fact]
            public void Bool() => RoundTrip(true, (ref ValueStruct_Bool s) => ref s.Value);

            [Fact]
            public void Byte() => RoundTrip(byte.MaxValue, (ref ValueStruct_Byte s) => ref s.Value);

            [Fact]
            public void SByte() => RoundTrip(sbyte.MinValue, (ref ValueStruct_SByte s) => ref s.Value);

            [Fact]
            public void UShort() => RoundTrip(ushort.MaxValue, (ref ValueStruct_UShort s) => ref s.Value);

            [Fact]
            public void Short() => RoundTrip(short.MinValue, (ref ValueStruct_Short s) => ref s.Value);

            [Fact]
            public void UInt() => RoundTrip(uint.MaxValue, (ref ValueStruct_UInt s) => ref s.Value);

            [Fact]
            public void Int() => RoundTrip(int.MinValue, (ref ValueStruct_Int s) => ref s.Value);

            [Fact]
            public void ULong() => RoundTrip(ulong.MaxValue, (ref ValueStruct_ULong s) => ref s.Value);

            [Fact]
            public void Long() => RoundTrip(long.MinValue, (ref ValueStruct_Long s) => ref s.Value);

            [Fact]
            public void Double() => RoundTrip(Math.PI, (ref ValueStruct_Double s) => ref s.Value);

            [Fact]
            public void Float() => RoundTrip((float)Math.E, (ref ValueStruct_Float s) => ref s.Value);

            private static void RoundTrip<TStruct, TValue>(TValue value, GetValue<TStruct, TValue> getter) 
                where TValue : struct
                where TStruct : struct
            {
                SimpleTableAnything<GenericReferenceStruct<TValue>> refTable = new()
                {
                    Item = new() { Item = value }
                };

                TStruct valueItem = default;
                getter(ref valueItem) = value;

                SimpleTableAnything<TStruct?> valueTable = new()
                {
                    Item = valueItem
                };

                ValueStructTypeModel vstm = (ValueStructTypeModel)RuntimeTypeModel.CreateFrom(typeof(TStruct));
                Assert.True(vstm.CanMarshalOnSerialize);
                Assert.True(vstm.CanMarshalOnParse);

                byte[] referenceBuffer = new byte[1024];
                byte[] referenceBufferNull = new byte[1024];
                byte[] valueMarshalBufferNull = new byte[1024];
                byte[] valueNonMarshalBufferNull = new byte[1024];
                byte[] valueMarhalBuffer = new byte[1024];
                byte[] valueNonMarhalBuffer = new byte[1024];

                var marshalSerializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = true });
                var nonMarshalSerializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { EnableValueStructMemoryMarshalDeserialization = false });

                int refNullBytesWritten = FlatBufferSerializer.Default.Serialize(new SimpleTableAnything<GenericReferenceStruct<TValue>>(), referenceBufferNull);
                int valueNullMarshalBytesWritten = marshalSerializer.Serialize(new SimpleTableAnything<TStruct?>(), valueMarshalBufferNull);
                int valueNullNonMarshalBytesWritten = nonMarshalSerializer.Serialize(new SimpleTableAnything<TStruct?>(), valueNonMarshalBufferNull);

                int refBytesWritten = FlatBufferSerializer.Default.Serialize(refTable, referenceBuffer);
                int valueMarshalBytesWritten = marshalSerializer.Serialize(valueTable, valueMarhalBuffer);
                int valueNonMarshalBytesWritten = nonMarshalSerializer.Serialize(valueTable, valueNonMarhalBuffer);

                Assert.Equal(refBytesWritten, valueMarshalBytesWritten);
                Assert.Equal(refBytesWritten, valueNonMarshalBytesWritten);

                Assert.True(referenceBuffer.SequenceEqual(valueMarhalBuffer));
                Assert.True(referenceBuffer.SequenceEqual(valueNonMarhalBuffer));

                Assert.Equal(refNullBytesWritten, valueNullMarshalBytesWritten);
                Assert.Equal(refNullBytesWritten, valueNullNonMarshalBytesWritten);

                Assert.True(referenceBufferNull.SequenceEqual(valueMarshalBufferNull));
                Assert.True(referenceBufferNull.SequenceEqual(valueNonMarshalBufferNull));

                var parsed = FlatBufferSerializer.Default.Parse<SimpleTableAnything<TStruct>>(valueMarhalBuffer);
                TStruct temp = parsed.Item;
                Assert.Equal(value, getter(ref temp));

                parsed = FlatBufferSerializer.Default.Parse<SimpleTableAnything<TStruct>>(referenceBufferNull);
                temp = parsed.Item;
                Assert.Equal(default(TValue), getter(ref temp));
            }

            public delegate ref TValue GetValue<TStruct, TValue>(ref TStruct @struct) where TStruct : struct;

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Bool {[FieldOffset(0)] public bool Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Byte { [FieldOffset(0)] public byte Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_SByte {[FieldOffset(0)] public sbyte Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_UShort {[FieldOffset(0)] public ushort Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Short {[FieldOffset(0)] public short Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_UInt {[FieldOffset(0)] public uint Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Int {[FieldOffset(0)] public int Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_ULong {[FieldOffset(0)] public ulong Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Long {[FieldOffset(0)] public long Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Float {[FieldOffset(0)] public float Value; }

            [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
            public struct ValueStruct_Double {[FieldOffset(0)] public double Value; }

            [FlatBufferStruct]
            public class GenericReferenceStruct<T> where T : struct
            {
                [FlatBufferItem(0)]
                public virtual T Item { get; set; }
            }
        }

        public interface IValidStruct
        {
            byte IA { get; set; }
            short IB { get; set; }
            int IC { get; set; }
            long ID { get; set; }
            ValidStruct_Inner IInner { get; set; }
        }

        public interface ISimpleTable
        {
            IValidStruct? Item { get; set; }
        }

        [FlatBufferTable]
        public class SimpleTableAnything<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }
        }

        [FlatBufferTable]
        public class SimpleTableNonNullable<T> : ISimpleTable
            where T : struct, IValidStruct
        {
            [FlatBufferItem(0)]
            public virtual T Item { get; set; }

            IValidStruct? ISimpleTable.Item { get => this.Item; set => this.Item = (T)value; }
        }

        [FlatBufferTable]
        public class SimpleTableNullable<T> : ISimpleTable
            where T : struct, IValidStruct
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }

            IValidStruct? ISimpleTable.Item { get => this.Item; set => this.Item = (T?)value; }
        }

        [FlatBufferStruct]
        public class ReferenceStruct
        {
            [FlatBufferItem(0)]
            public virtual int Foo { get; set; }
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
        public struct ValidStruct_Inner
        {
            [FieldOffset(0)] public int Test;
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 20)]
        public struct ValidStruct_Marshallable : IValidStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(2)] public short B;
            [FieldOffset(4)] public int C;
            [FieldOffset(8)] public long D;
            [FieldOffset(16)] public ValidStruct_Inner Inner;

            public byte IA { get => this.A; set => this.A = value; }
            public short IB { get => this.B; set => this.B = value; }
            public int IC { get => this.C; set => this.C = value; }
            public long ID { get => this.D; set => this.D = value; }
            public ValidStruct_Inner IInner { get => this.Inner; set => this.Inner = value; }
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 20)]
        public struct ValidStruct_Marshallable_OutOfOrder : IValidStruct
        {
            [FieldOffset(16)] public ValidStruct_Inner Inner;
            [FieldOffset(4)] public int C;
            [FieldOffset(8)] public long D;
            [FieldOffset(2)] public short B;
            [FieldOffset(0)] public byte A;

            public byte IA { get => this.A; set => this.A = value; }
            public short IB { get => this.B; set => this.B = value; }
            public int IC { get => this.C; set => this.C = value; }
            public long ID { get => this.D; set => this.D = value; }
            public ValidStruct_Inner IInner { get => this.Inner; set => this.Inner = value; }
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
        public struct ValidStruct_NotMarshallable_MissingSizeHint : IValidStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(2)] public short B;
            [FieldOffset(4)] public int C;
            [FieldOffset(8)] public long D;
            [FieldOffset(16)] public ValidStruct_Inner Inner;

            public byte IA { get => this.A; set => this.A = value; }
            public short IB { get => this.B; set => this.B = value; }
            public int IC { get => this.C; set => this.C = value; }
            public long ID { get => this.D; set => this.D = value; }
            public ValidStruct_Inner IInner { get => this.Inner; set => this.Inner = value; }
        }

        [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 100)]
        public struct ValidStruct_NotMarshallable_DueToSize : IValidStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(2)] public short B;
            [FieldOffset(4)] public int C;
            [FieldOffset(8)] public long D;
            [FieldOffset(16)] public ValidStruct_Inner Inner;

            public byte IA { get => this.A; set => this.A = value; }
            public short IB { get => this.B; set => this.B = value; }
            public int IC { get => this.C; set => this.C = value; }
            public long ID { get => this.D; set => this.D = value; }
            public ValidStruct_Inner IInner { get => this.Inner; set => this.Inner = value; }
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct SixteenByteStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(8)] public ulong B;
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit, Size = 9)]
        public struct NineByteStruct
        {
            [FieldOffset(0)] public ulong A;
            [FieldOffset(8)] public byte B;
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit, Size = 5)]
        public struct FiveByteStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] public byte B;
            [FieldOffset(2)] public byte C;
            [FieldOffset(3)] public byte D;
            [FieldOffset(4)] public byte E;
        }

        // Small struct that shouldn't marshal (but we are making it).
        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Always), StructLayout(LayoutKind.Explicit)]
        public struct MarshalAlways { [FieldOffset(0)] public byte A; }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Serialize), StructLayout(LayoutKind.Explicit)]
        public struct MarshalSerialize {[FieldOffset(0)] public byte A; }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Parse), StructLayout(LayoutKind.Explicit)]
        public struct MarshalParse {[FieldOffset(0)] public byte A; }

        // Complex struct that should marshal (but we are not letting it).
        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Never), StructLayout(LayoutKind.Explicit)]
        public struct MarshalNever
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] public byte B;
            [FieldOffset(2)] public byte C;
            [FieldOffset(3)] public byte D;
            [FieldOffset(4)] public byte E;
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Default), StructLayout(LayoutKind.Explicit)]
        public struct MarshalDefault_Simple
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] public byte B;
            [FieldOffset(2)] public byte C;
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Default), StructLayout(LayoutKind.Explicit)]
        public struct MarshalDefault_Complex_DueToInnerStruct
        {
            [FieldOffset(0)] public FiveByteStruct A;
        }

        [FlatBufferStruct(MemoryMarshalBehavior = MemoryMarshalBehavior.Default), StructLayout(LayoutKind.Explicit)]
        public struct MarshalDefault_Complex_DueToLotsOfFields
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] public byte B;
            [FieldOffset(2)] public byte C;
            [FieldOffset(3)] public byte D;
        }
    }
}
