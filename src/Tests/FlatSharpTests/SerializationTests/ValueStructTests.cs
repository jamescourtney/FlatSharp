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
            [InlineData(typeof(ValidStruct_Marshallable), true)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), true)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), false)]
            public void ValidStructs(Type type, bool marshallable)
            {
                ITypeModel typeModel = RuntimeTypeModel.CreateFrom(type);
                var tm = Assert.IsType<ValueStructTypeModel>(typeModel);

                var children = tm.Children.ToArray();
                Assert.Equal(typeof(byte), children[0].ClrType);
                Assert.Equal(typeof(short), children[1].ClrType);
                Assert.Equal(typeof(int), children[2].ClrType);
                Assert.Equal(typeof(long), children[3].ClrType);
                Assert.Equal(typeof(ValidStruct_Inner), children[4].ClrType);
                Assert.Equal(marshallable, tm.CanMarshalWhenLittleEndian);
            }

            [Fact]
            public void SimpleStruct_Marshallable_WithoutSizeHint()
            {
                ITypeModel typeModel = RuntimeTypeModel.CreateFrom(typeof(ValidStruct_Inner));
                var tm = Assert.IsType<ValueStructTypeModel>(typeModel);
                Assert.True(tm.CanMarshalWhenLittleEndian);
            }

            [Theory]
            [InlineData(typeof(InvalidStruct_Sequential), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Sequential' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_Auto), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Auto' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_None), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_None' must have [StructLayout(LayoutKind.Explicit)] specified.")]
            [InlineData(typeof(InvalidStruct_Empty), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_Empty' is empty or has no public fields.")]
            [InlineData(typeof(InvalidStruct_NoPublicFields), "Value struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NoPublicFields' is empty or has no public fields.")]
            [InlineData(typeof(InvalidStruct_ContainsReferenceType), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_ContainsReferenceType' property Foo must be a value type if the struct is a value type.")]
            [InlineData(typeof(InvalidStruct_WrongFieldOffset), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_WrongFieldOffset' property 'B' defines invalid [FieldOffset] attribute. Expected: [FieldOffset(8)].")]
            [InlineData(typeof(InvalidStruct_NotValidStructMember), "Struct 'FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NotValidStructMember' property A cannot be part of a flatbuffer struct. Structs may only contain scalars and other structs.")]
            [InlineData(typeof(InvalidStruct_NotPublic), "Can't create type model from type FlatSharpTests.ValueStructTests.TypeModel.InvalidStruct_NotPublic because it is not public.")]
            public void InvalidStructs(Type structType, string expectedError)
            {
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(structType));
                Assert.Equal(expectedError, ex.Message);
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
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), false, false)]
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
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), false, false)]
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
        }

        public class ParseTests
        {
            [Theory]
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToSize), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_MissingSizeHint), false, false)]
            public void Parse_Nullable(Type type, bool enableMarshal, bool expectMarshal)
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
            [InlineData(typeof(ValidStruct_Marshallable), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable), false, false)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), true, true)]
            [InlineData(typeof(ValidStruct_Marshallable_HiddenField), false, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), true, false)]
            [InlineData(typeof(ValidStruct_NotMarshallable_DueToPrivateMember), false, false)]
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
        public struct ValidStruct_Marshallable_HiddenField : IValidStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(1)] private byte Hidden;
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


        [FlatBufferStruct, StructLayout(LayoutKind.Explicit)]
        public struct ValidStruct_NotMarshallable_DueToPrivateMember : IValidStruct
        {
            [FieldOffset(0)] public byte A;
            [FieldOffset(2)] public short B;
            [FieldOffset(4)] public int C;
            [FieldOffset(8)] public long D;
            [FieldOffset(16)] public ValidStruct_Inner Inner;
            [FieldOffset(20)] private int Foo;

            public byte IA { get => this.A; set => this.A = value; }
            public short IB { get => this.B; set => this.B = value; }
            public int IC { get => this.C; set => this.C = value; }
            public long ID { get => this.D; set => this.D = value; }
            public ValidStruct_Inner IInner { get => this.Inner; set => this.Inner = value; }
        }
    }
}
