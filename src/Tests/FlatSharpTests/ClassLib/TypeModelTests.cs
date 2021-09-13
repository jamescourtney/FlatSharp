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
    using Xunit;

    
    public class TypeModelTests
    {
        [Fact]
        public void TypeModel_Table_UnrecognizedPropertyType()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<Dictionary<string, string>>)));

            Assert.Equal("Failed to create or find type model for type 'System.Collections.Generic.Dictionary<System.String, System.String>'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_DuplicateIndex()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberTable)));

            Assert.Equal("FlatBuffer Table FlatSharpTests.TypeModelTests.DuplicateNumberTable already defines a property with index 0. This may happen when unions are declared as these are double-wide members.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_PrivateSetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(PrivateSetter)));

            Assert.Equal("Property FlatSharpTests.TypeModelTests.PrivateSetter.String (Index 0) declared a set method, but it was not public/protected and virtual.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_InternalSetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalSetter)));

            Assert.Equal("Property FlatSharpTests.TypeModelTests.InternalSetter.String (Index 0) declared a set method, but it was not public/protected and virtual.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_InterfaceImplementationNonVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceTableNonVirtual));
        }

        [Fact]
        public void TypeModel_Table_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceTableVirtual));
        }

        [Fact]
        public void TypeModel_Table_NoGetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NoGetter)));
            Assert.Equal("Property FlatSharpTests.TypeModelTests.Table_NoGetter.Prop (Index 0) on did not declare a getter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_Empty()
        {
            RuntimeTypeModel.CreateFrom(typeof(Table_Empty));
        }

        [Fact]
        public void TypeModel_Table_NonPublicGetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonPublicGetter)));
            Assert.Equal("Property FlatSharpTests.TypeModelTests.Table_NonPublicGetter.Prop (Index 0) must declare a public getter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_NonVirtual_NoSetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonVirtual_NoSetter)));
            Assert.Equal("Non-virtual property FlatSharpTests.TypeModelTests.Table_NonVirtual_NoSetter.Prop (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_FileIdentifiers()
        {
            Assert.IsType<TableTypeModel>(RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierOK)));

            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooShort)));
            Assert.Equal("File identifier 'abc' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooLong)));
            Assert.Equal("File identifier 'abcde' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooFancy)));
            Assert.Equal("File identifier '😍😚😙😵‍' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierOutOfRange)));
            Assert.Equal("File identifier 'abcµ' contains non-ASCII characters. Character 'µ' is invalid.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_InternalConstructor()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalConstructorTable<byte>)));
            Assert.Equal("Default constructor for 'FlatSharpTests.TypeModelTests.InternalConstructorTable<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_PrivateConstructor()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(PrivateConstructorTable<byte>)));
            Assert.Equal("Default constructor for 'FlatSharpTests.TypeModelTests.PrivateConstructorTable<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [Fact]
        public void TypeModel_Table_ForceWrite()
        {
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<bool>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<byte>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<sbyte>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<ushort>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<short>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<uint>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<int>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<ulong>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<long>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<float>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<double>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<TaggedEnum>));
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<FlatBufferUnion<string>>));

            // This is a special case and is allowed since memory is a struct
            // and is therefore non-null. It will be written as a 0 byte vector.
            RuntimeTypeModel.CreateFrom(typeof(Table_ForceWrite<Memory<byte>>));

            static void ValidateError<T>()
            {
                var type = typeof(Table_ForceWrite<T>);
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                    RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"Property 'Item' on table '{CSharpHelpers.GetCompilableTypeName(type)}' declares the ForceWrite option, but the type is not supported for force write.",
                    ex.Message);
            }

            ValidateError<bool?>();
            ValidateError<int?>();
            ValidateError<TaggedEnum?>();
            ValidateError<double?>();
            ValidateError<GenericStruct<int>>();
            ValidateError<int[]>();
            ValidateError<FlatBufferUnion<string>?>();
            ValidateError<Table_ForceWrite<int>>();
            ValidateError<Memory<byte>?>();
        }

        [Fact]
        public void TypeModel_Table_ProtectedConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(ProtectedConstructorTable<byte>));
        }

        [Fact]
        public void TypeModel_Table_SpecialConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(SpecialConstructorTable<byte>));
        }

        [Fact]
        public void TypeModel_Table_NoValidCtor()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Table_NoCtor)));

            Assert.Equal(
                "Unable to find a usable constructor for 'FlatSharpTests.TypeModelTests.Table_NoCtor'. The type must supply a default constructor or single parameter constructor accepting 'FlatBufferDeserializationContext' that is visible to subclasses outside the assembly.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_Table_Required_Scalar_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<long>)));

            Assert.Equal(
                "Table property 'FlatSharpTests.TypeModelTests.TableRequiredField<System.Int64>.Value' declared the Required attribute. Required is only valid on non-scalar table fields.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_Table_Required_Enum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<TaggedEnum>)));

            Assert.Equal(
                "Table property 'FlatSharpTests.TypeModelTests.TableRequiredField<FlatSharpTests.TypeModelTests.TaggedEnum>.Value' declared the Required attribute. Required is only valid on non-scalar table fields.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_Table_Required()
        {
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<IList<string>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<string[]>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<IIndexedVector<string, SortedVectorKeyTable<string>>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<string>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<Memory<byte>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<ReadOnlyMemory<byte>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<IList<FlatBufferUnion<string>>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<FlatBufferUnion<string>[]>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<GenericTable<int>>));
            RuntimeTypeModel.CreateFrom(typeof(TableRequiredField<GenericStruct<int>>));
        }

        [Fact]
        public void TypeModel_Table_OnDeserialized()
        {
            string CreateError<T>() => $"Type '{CSharpHelpers.GetCompilableTypeName(typeof(T))}' provides an unusable 'OnFlatSharpDeserialized' method. 'OnFlatSharpDeserialized' must be protected, have a return type of void, and accept a single parameter of type 'FlatBufferDeserializationContext'.";

            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_WrongParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_WrongParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_NoParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_NoParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_OutParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_OutParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_InParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_InParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_RefParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_RefParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_OptionalParameter)));
            Assert.Equal(CreateError<TableOnDeserialized_OptionalParameter>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_NotProtected)));
            Assert.Equal(CreateError<TableOnDeserialized_NotProtected>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_ReturnsNonVoid)));
            Assert.Equal(CreateError<TableOnDeserialized_ReturnsNonVoid>(), ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_Multiple)));
            Assert.Equal("Type 'FlatSharpTests.TypeModelTests.TableOnDeserialized_Multiple' provides more than one 'OnFlatSharpDeserialized' method.", ex.Message);

            RuntimeTypeModel.CreateFrom(typeof(TableOnDeserialized_OK));
        }

        [Fact]
        public void TypeModel_Table_WriteThrough_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(TableWriteThrough_NotSupported)));

            Assert.Equal(
                "Table property 'FlatSharpTests.TypeModelTests.TableWriteThrough_NotSupported.Property' declared the WriteThrough attribute. WriteThrough on tables is only supported for value type structs.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_NonVirtual_NoSetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonVirtual_NoSetter)));
            Assert.Equal("Non-virtual property FlatSharpTests.TypeModelTests.Struct_NonVirtual_NoSetter.Prop (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_NoGetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NoGetter)));
            Assert.Equal("Property FlatSharpTests.TypeModelTests.Struct_NoGetter.Prop (Index 0) on did not declare a getter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_NonPublicGetter()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonPublicGetter)));
            Assert.Equal("Property FlatSharpTests.TypeModelTests.Struct_NonPublicGetter.Prop (Index 0) must declare a public getter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_StringNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<string>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.String>' property Value (Index 0) with type System.String cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_VectorNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<Memory<byte>>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Memory<System.Byte>>' property Value (Index 0) with type System.Memory<System.Byte> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_TableNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<GenericTable<int>>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<FlatSharpTests.TypeModelTests.GenericTable<System.Int32>>' property Value (Index 0) with type FlatSharpTests.TypeModelTests.GenericTable<System.Int32> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_UnionNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<FlatBufferUnion<string, GenericTable<string>>>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>>>' property Value (Index 0) with type FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_Misnumbered()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(MisnumberedStruct<byte>)));
            Assert.Equal("FlatBuffer struct FlatSharpTests.TypeModelTests.MisnumberedStruct<System.Byte> does not declare an item with index 1. Structs must have sequenential indexes starting at 0.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_DeprecatedNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(StructDeprecatedField)));
            Assert.Equal(
                "FlatBuffer struct FlatSharpTests.TypeModelTests.StructDeprecatedField may not have deprecated properties", 
                ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_InaccessibleConstructor()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InaccessibleConstructorStruct<byte>)));
            Assert.Equal("Default constructor for 'FlatSharpTests.TypeModelTests.InaccessibleConstructorStruct<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_Empty()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_Empty)));
            Assert.Equal("Can't create struct type model from type FlatSharpTests.TypeModelTests.Struct_Empty because it does not have any non-static [FlatBufferItem] properties. Structs cannot be empty.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_ProtectedConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(ProtectedConstructorStruct<byte>));
        }

        [Fact]
        public void TypeModel_Struct_SpecialConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(SpecialConstructorStruct<byte>));
        }

        [Fact]
        public void TypeModel_Struct_InternalAccess()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalAccessStruct<byte>)));
            Assert.Equal("Can't create type model from type FlatSharpTests.TypeModelTests.InternalAccessStruct<System.Byte> because it is not public.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_NonVirtualProperty()
        {
            RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStruct<byte>));
        }

        [Fact]
        public void TypeModel_Struct_NonVirtualProperty_PrivateSetter_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStructPrivateSetter<byte>)));
            Assert.Equal("Non-virtual property FlatSharpTests.TypeModelTests.NonVirtualPropertyStructPrivateSetter<System.Byte>.Value (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_Sealed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SealedStruct<byte>)));
            Assert.Equal("Can't create type model from type FlatSharpTests.TypeModelTests.SealedStruct<System.Byte> because it is sealed.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_DoesNotInheritFromObject()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DoesNotInheritFromObjectStruct<byte>)));
            Assert.Equal("Can't create type model from type FlatSharpTests.TypeModelTests.DoesNotInheritFromObjectStruct<System.Byte> its base class is not System.Object.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_Abstract()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(AbstractStruct<byte>)));
            Assert.Equal("Can't create type model from type FlatSharpTests.TypeModelTests.AbstractStruct<System.Byte> because it is abstract.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_DuplicateNumbered()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberedStruct<byte>)));
            Assert.Equal("FlatBuffer struct FlatSharpTests.TypeModelTests.DuplicateNumberedStruct<System.Byte> does not declare an item with index 2. Structs must have sequenential indexes starting at 0.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_DefaultValue()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(StructWithDefaultValue)));
            Assert.Equal("FlatBuffer struct FlatSharpTests.TypeModelTests.StructWithDefaultValue declares default value on index 0. Structs may not have default values.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_InterfaceImplementationNonVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceStructNonVirtual));
        }

        [Fact]
        public void TypeModel_Struct_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceStructVirtual));
        }

        [Fact]
        public void TypeModel_Struct_OptionalField_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<int?>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Nullable<System.Int32>>' property Value (Index 0) with type System.Nullable<System.Int32> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_OptionalEnum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<TaggedEnum?>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>>' property Value (Index 0) with type System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_ForceWrite_NotAllowed()
        {
            static void Validate<T>()
            {
                var type = typeof(Struct_ForceWrite<T>);
                var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                    RuntimeTypeModel.CreateFrom(type));

                Assert.Equal(
                    $"FlatBuffer struct {CSharpHelpers.GetCompilableTypeName(type)} may not have properties with the ForceWrite option set to true.",
                    ex.Message);
            }

            Validate<bool>();
            Validate<byte>();
            Validate<sbyte>();
            Validate<ushort>();
            Validate<short>();
            Validate<uint>();
            Validate<int>();
            Validate<ulong>();
            Validate<long>();
            Validate<float>();
            Validate<double>();
            Validate<TaggedEnum>();
            Validate<GenericStruct<int>>();
        }

        [Fact]
        public void TypeModel_Struct_WriteThrough_NonVirtual_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(StructWriteThroughNonVirtual)));

            Assert.Equal(
                "Struct member 'FlatSharpTests.TypeModelTests.StructWriteThroughNonVirtual.Property' declared the WriteThrough attribute, but WriteThrough is only supported on virtual fields.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_Struct_WriteThrough_Virtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(StructWriteThrough));
        }

        [Fact]
        public void TypeModel_Struct_Required_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(StructRequiredField<long>)));

            Assert.Equal(
                "Struct member 'FlatSharpTests.TypeModelTests.StructRequiredField<System.Int64>.Value' declared the Required attribute. Required is not valid inside structs.",
                ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(StructRequiredField<TaggedEnum>)));

            Assert.Equal(
                "Struct member 'FlatSharpTests.TypeModelTests.StructRequiredField<FlatSharpTests.TypeModelTests.TaggedEnum>.Value' declared the Required attribute. Required is not valid inside structs.",
                ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(StructRequiredField<GenericStruct<long>>)));

            Assert.Equal(
                "Struct member 'FlatSharpTests.TypeModelTests.StructRequiredField<FlatSharpTests.TypeModelTests.GenericStruct<System.Int64>>.Value' declared the Required attribute. Required is not valid inside structs.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_TypeCantBeTableAndStruct()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InvalidTableAndStruct)));
            Assert.Equal("Type 'FlatSharpTests.TypeModelTests.InvalidTableAndStruct' is declared as both [FlatBufferTable] and [FlatBufferStruct].", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_VectorNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<IList<int>>>)));
            Assert.Equal("Type 'System.Collections.Generic.IList<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [Fact]
        public void TypeModel_MemoryVector_UnionNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<Memory<FlatBufferUnion<string, GenericTable<string>>>>)));
            Assert.Equal("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_OptionalScalarNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<int?>>)));
            Assert.Equal("Type 'System.Nullable<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_OptionalEnumNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<TaggedEnum?>>)));
            Assert.Equal("Type 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>' is not a valid vector member.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_Indexed()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(GenericTable<IIndexedVector<string, SortedVectorKeyTable<string>>>));
            Assert.Equal(FlatBufferSchemaType.Table, model.SchemaType);

            TableTypeModel tableModel = (TableTypeModel)model;
            var firstMember = tableModel.IndexToMemberMap[0];

            Assert.True(firstMember.IsSortedVector); // sorted vector is set even though it's not explicitly declared. Indexed Vectors force it to be set.

            var vectorModel = firstMember.ItemTypeModel;
            Assert.Equal(FlatBufferSchemaType.Vector, vectorModel.SchemaType);
        }

        [Fact]
        public void TypeModel_Vector_IndexedVector_MismatchedKeyTypes_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, SortedVectorKeyTable<string>>)));
            Assert.Equal("FlatSharp indexed vector keys must have the same type as the key of the value. KeyType = System.Int32, Value Key Type = 'FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.String>'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_IndexedVector_UnkeyedTable_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericTable<string>>)));
            Assert.Equal("Indexed vector values must have a property with the key attribute defined. Table = 'FlatSharpTests.TypeModelTests.GenericTable<System.String>'", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_IndexedVector_Struct_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericStruct<int>>)));
            Assert.Equal("Indexed vector values must be flatbuffer tables. Type = 'FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>'", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_IndexedVector_Vector_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, IList<string>>)));
            Assert.Equal("Indexed vector values must be flatbuffer tables. Type = 'System.Collections.Generic.IList<System.String>'", ex.Message);
        }

        [Fact]
        public void TypeModel_Enum_UntaggedEnumNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum)));
            Assert.Equal("Enum 'FlatSharpTests.TypeModelTests.UntaggedEnum' is not tagged with a [FlatBufferEnum] attribute.", ex.Message);
        }

        [Fact]
        public void TypeModel_Enum_NullableUntaggedEnumNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum?)));
            Assert.Equal("Enum 'FlatSharpTests.TypeModelTests.UntaggedEnum' is not tagged with a [FlatBufferEnum] attribute.", ex.Message);
        }

        [Fact]
        public void TypeModel_Enum_TaggedEnum()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(TaggedEnum));

            Assert.True(model is EnumTypeModel enumModel);
            Assert.Equal(typeof(TaggedEnum), model.ClrType);
            Assert.True(model.IsFixedSize);
        }

        [Fact]
        public void TypeModel_Enum_NullableTaggedEnum()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(TaggedEnum?));

            Assert.True(model is NullableTypeModel nullableModel);
            Assert.Equal(typeof(TaggedEnum?), model.ClrType);
            Assert.True(model.IsFixedSize);
        }

        [Fact]
        public void TypeModel_Vector_ListVectorOfStruct()
        {
            var model = this.VectorTest(typeof(IList<>), typeof(GenericStruct<bool>));
            Assert.IsType<ListVectorTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_ReadOnlyListOfTable()
        {
            var model = this.VectorTest(typeof(IReadOnlyList<>), typeof(GenericTable<bool>));
            Assert.IsType<ListVectorTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_MemoryOfByte()
        {
            var model = this.VectorTest(typeof(Memory<>), typeof(byte));
            Assert.IsType<MemoryVectorTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_NullableMemoryOfByte()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(Memory<byte>?));
            Assert.IsType<NullableTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_NullableReadOnlyMemoryOfByte()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<byte>?));
            Assert.IsType<NullableTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_MemoryOfScalar_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<int>)));
            Assert.Equal("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_MemoryOfEnum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<TaggedEnum>)));
            Assert.Equal("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_ReadOnlyMemoryOfByte()
        {
            var model = this.VectorTest(typeof(ReadOnlyMemory<>), typeof(byte));
            Assert.IsType<MemoryVectorTypeModel>(model);
        }

        [Fact]
        public void TypeModel_Vector_ReadOnlyMemoryOfScalar_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<int>)));
            Assert.Equal("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [Fact]
        public void TypeModel_Vector_ReadOnlyMemoryOfEnum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<TaggedEnum>)));
            Assert.Equal("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfStruct_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyStruct<int>[]>)));
            Assert.Equal("Property 'Vector' declares a sorted vector, but the member is not a table. Type = FlatSharpTests.TypeModelTests.SortedVectorKeyStruct<System.Int32>[].", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfEnum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum[]>)));
            Assert.Equal("Property 'Vector' declares a sorted vector, but the member is not a table. Type = FlatSharpTests.TypeModelTests.TaggedEnum[].", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfNullableEnum_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum?[]>)));
            Assert.Equal("Type 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>' is not a valid vector member.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfString_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<string[]>)));
            Assert.Equal("Property 'Vector' declares a sorted vector, but the member is not a table. Type = System.String[].", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfScalar_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int[]>)));
            Assert.Equal("Property 'Vector' declares a sorted vector, but the member is not a table. Type = System.Int32[].", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfNullableScalar_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int?[]>)));
            Assert.Equal("Type 'System.Nullable<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithBuiltInKey()
        {
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<string>>>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<IReadOnlyList<SortedVectorKeyTable<bool>>>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<byte>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<sbyte>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ushort>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<short>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<uint>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<int>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ulong>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<long>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<float>[]>));
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<double>[]>));
        }

        [Fact]
        public void TypeModel_SortedVector_DeprecatedKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorDeprecatedKeyTable<string>[]>)));
            Assert.Equal(
                "Table FlatSharpTests.TypeModelTests.SortedVectorDeprecatedKeyTable<System.String> declares a key property that is deprecated.",
                ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithOptionalKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<bool?>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Boolean>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<byte?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Byte>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<sbyte?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.SByte>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ushort?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt16>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<short?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int16>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<uint?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt32>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<int?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int32>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ulong?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt64>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<long?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int64>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<float?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Single>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<double?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Double>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<TaggedEnum?>[]>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithStructKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericStruct<string>>>>)));
            Assert.Equal("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.String>' property Value (Index 0) with type System.String cannot be part of a flatbuffer struct.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithTableKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericTable<string>>>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<FlatSharpTests.TypeModelTests.GenericTable<System.String>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithVectorKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<string[]>>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.String[]> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithEnumKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum>>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<FlatSharpTests.TypeModelTests.TaggedEnum> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithNullableEnumKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum?>>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithoutKey_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<GenericTable<string>>>)));
            Assert.Equal("Property 'Vector' declares a sorted vector, but the member does not have a key defined. Type = System.Collections.Generic.IList<FlatSharpTests.TypeModelTests.GenericTable<System.String>>.", ex.Message);
        }

        [Fact]
        public void TypeModel_SortedVector_OfTableWithMultipleKeys_NotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorMultiKeyTable<string>>>)));
            Assert.Equal("Table FlatSharpTests.TypeModelTests.SortedVectorMultiKeyTable<System.String> has more than one [FlatBufferItemAttribute] with Key set to true.", ex.Message);
        }

        private BaseVectorTypeModel VectorTest(Type vectorDefinition, Type innerType)
        {
            var model = (BaseVectorTypeModel)RuntimeTypeModel.CreateFrom(vectorDefinition.MakeGenericType(innerType));

            Assert.Equal(model.ClrType.GetGenericTypeDefinition(), vectorDefinition);
            Assert.Equal(4, model.PhysicalLayout.Single().InlineSize);
            Assert.Equal(4, model.PhysicalLayout.Single().Alignment);

            var innerModel = RuntimeTypeModel.CreateFrom(innerType);
            Assert.Equal(innerModel.ClrType, model.ItemTypeModel.ClrType);

            return model;
        }

        /// <summary>
        /// This scenario actually works with flatsharp, but the flatc compiler does not support this, so it doesn't seem to be an officially
        /// sanctioned feature.
        /// </summary>
        [Fact]
        public void TypeModel_Union_VectorsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IList<string>>>)));
            Assert.Equal("Unions may not store 'System.Collections.Generic.IList<System.String>'.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IReadOnlyList<string>>>)));
            Assert.Equal("Unions may not store 'System.Collections.Generic.IReadOnlyList<System.String>'.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, Memory<byte>>>)));
            Assert.Equal("Unions may not store 'System.Memory<System.Byte>'.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, ReadOnlyMemory<byte>>>)));
            Assert.Equal("Unions may not store 'System.ReadOnlyMemory<System.Byte>'.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, string[]>>)));
            Assert.Equal("Unions may not store 'System.String[]'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_UnionsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, FlatBufferUnion<string, GenericStruct<int>>?>?>)));
            Assert.Equal("Unions may not store 'System.Nullable<FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>>>'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_ScalarsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int>?>)));
            Assert.Equal("Unions may not store 'System.Int32'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_OptionalScalarsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int?>>)));
            Assert.Equal("Unions may not store 'System.Nullable<System.Int32>'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_EnumsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum>?>)));
            Assert.Equal("Unions may not store 'FlatSharpTests.TypeModelTests.TaggedEnum'.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_OptionalEnumsNotAllowed()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum?>?>)));
            Assert.Equal("Unions may not store 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>'.", ex.Message);
        }

        [Fact]
        public void FlatBufferSerializer_OnlyTablesAllowedAsRootType()
        {
            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<GenericStruct<int>>());
            Assert.Equal("Can only compile [FlatBufferTable] elements as root types. Type 'FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>' is a Struct.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<IList<int>>());
            Assert.Equal("Can only compile [FlatBufferTable] elements as root types. Type 'System.Collections.Generic.IList<System.Int32>' is a Vector.", ex.Message);

            ex = Assert.Throws<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<string>());
            Assert.Equal("Can only compile [FlatBufferTable] elements as root types. Type 'System.String' is a String.", ex.Message);
        }

        [Fact]
        public void TypeModel_Union_StructsTablesStringsAllowed()
        {
            var tableModel = (TableTypeModel)RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, GenericTable<string>, GenericStruct<int>>?>));
            Assert.Equal(1, tableModel.IndexToMemberMap.Count);

            var nullableModel= (NullableTypeModel)tableModel.IndexToMemberMap[0].ItemTypeModel;
            var model = (UnionTypeModel)nullableModel.Children.Single();

            Assert.Equal(3, model.UnionElementTypeModel.Length);
            Assert.IsType<StringTypeModel>(model.UnionElementTypeModel[0]);
            Assert.IsType<TableTypeModel>(model.UnionElementTypeModel[1]);
            Assert.IsType<StructTypeModel>(model.UnionElementTypeModel[2]);
        }

        [Fact]
        public void TypeModel_StructsWithPadding()
        {
            // inner strut is float + double + bool = 4x float + 4x padding + 8x double + 1 bool = 17.
            // outer struct is inner + double + bool = 17x inner + 7x padding + 8x double + 1x bool = 33
            Type type = typeof(GenericStruct<GenericStruct<float>>);
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            Assert.Equal(typeModel.ClrType, type);
            Assert.IsType<StructTypeModel>(typeModel);

            var structModel = (StructTypeModel)typeModel;
            Assert.Equal(3, structModel.Members.Count);

            Assert.IsType<StructTypeModel>(structModel.Members[0].ItemTypeModel);
            Assert.Equal(0, structModel.Members[0].Index);
            Assert.Equal(0, structModel.Members[0].Offset);
            Assert.Equal(8, structModel.Members[0].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.Equal(17, structModel.Members[0].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.IsAssignableFrom<ScalarTypeModel>(structModel.Members[1].ItemTypeModel);
            Assert.Equal(1, structModel.Members[1].Index);
            Assert.Equal(24, structModel.Members[1].Offset);
            Assert.Equal(8, structModel.Members[1].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.Equal(8, structModel.Members[1].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.IsAssignableFrom<ScalarTypeModel>(structModel.Members[2].ItemTypeModel);
            Assert.Equal(2, structModel.Members[2].Index);
            Assert.Equal(32, structModel.Members[2].Offset);
            Assert.Equal(1, structModel.Members[2].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.Equal(1, structModel.Members[2].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.Equal(33, structModel.PhysicalLayout.Single().InlineSize);
            Assert.Equal(8, structModel.PhysicalLayout.Single().Alignment);
        }

        public enum UntaggedEnum
        {
            Foo, Bar
        }

        [FlatBufferEnum(typeof(int))]
        public enum TaggedEnum
        {
            Foo, Bar
        }

        [FlatBufferTable, FlatBufferStruct]
        public class InvalidTableAndStruct
        {
            [FlatBufferItem(0)]
            public virtual string String { get; set; }
        }

        [FlatBufferTable]
        public class GenericTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferTable]
        public class DuplicateNumberTable
        {
            [FlatBufferItem(0)]
            public virtual string Value { get; set; }

            [FlatBufferItem(0)]
            public virtual double Value2 { get; set; }
        }

        [FlatBufferStruct]
        public class GenericStruct<T>
        {
            [FlatBufferItem(0)]
            public virtual T Value { get; set; }

            [FlatBufferItem(1)]
            public virtual double Double { get; protected internal set; }

            [FlatBufferItem(2)]
            public virtual bool Bool { get; protected set; }
        }

        [FlatBufferStruct]
        public class DuplicateNumberedStruct<T>
        {
            [FlatBufferItem(0)]
            public virtual T Value { get; set; }

            [FlatBufferItem(1)]
            public virtual double Double { get; set; }

            [FlatBufferItem(1)]
            public virtual double Double2 { get; set; }
        }

        [FlatBufferStruct]
        public class MisnumberedStruct<T>
        {
            [FlatBufferItem(0)]
            public virtual T Value { get; set; }

            [FlatBufferItem(2)]
            public virtual double Double { get; set; }
        }

        [FlatBufferStruct]
        public class InaccessibleConstructorStruct<T>
        {
            internal InaccessibleConstructorStruct() { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        public class ProtectedConstructorStruct<T>
        {
            protected ProtectedConstructorStruct() { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        public class SpecialConstructorStruct<T>
        {
            protected SpecialConstructorStruct(FlatBufferDeserializationContext context) { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferTable]
        public class PrivateConstructorTable<T>
        {
            internal PrivateConstructorTable() { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferTable]
        public class InternalConstructorTable<T>
        {
            internal InternalConstructorTable() { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferTable]
        public class ProtectedConstructorTable<T>
        {
            protected ProtectedConstructorTable() { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferTable]
        public class SpecialConstructorTable<T>
        {
            protected SpecialConstructorTable(FlatBufferDeserializationContext context) { }

            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        internal class InternalAccessStruct<T>
        {
            [FlatBufferItem(0)]
            public virtual T Value { get; set; }
        }

        [FlatBufferStruct]
        public class NonVirtualPropertyStruct<T>
        {
            [FlatBufferItem(0)]
            public T Value { get; set; }
        }

        [FlatBufferStruct]
        public class NonVirtualPropertyStructPrivateSetter<T>
        {
            [FlatBufferItem(0)]
            public T Value { get; private set; }
        }

        [FlatBufferStruct]
        public abstract class AbstractStruct<T>
        {
            [FlatBufferItem(0)]
            public abstract T Value { get; set; }
        }

        [FlatBufferStruct]
        public class StructWithDefaultValue
        {
            [FlatBufferItem(0, DefaultValue = 100)]
            public virtual int Int { get; set; }
        }

        [FlatBufferStruct]
        public sealed class SealedStruct<T>
        {
            [FlatBufferItem(0)]
            public T Value { get; set; }
        }

        [FlatBufferStruct]
        public class DoesNotInheritFromObjectStruct<T> : GenericStruct<T>
        {
        }

        [FlatBufferTable]
        public class OverlappingUnionIndex
        {
            [FlatBufferItem(0)]
            public virtual FlatBufferUnion<string, IList<string>> Union { get; set; }

            // Invalid -- this should be at index 2.
            [FlatBufferItem(1)]
            public virtual string FooBar { get; set; }
        }

        [FlatBufferTable]
        public class PrivateSetter
        {
            [FlatBufferItem(0)]
            public virtual string String { get; private set; }
        }

        [FlatBufferTable]
        public class InternalSetter
        {
            [FlatBufferItem(0)]
            public virtual string String { get; internal set; }
        }

        public interface IInterface
        {
            int Foo { get; set; }
        }

        // Properties that implement interfaces are virtual according to the property info. It's possible to be both
        // virtual and final.
        [FlatBufferTable]
        public class InterfaceTableNonVirtual : IInterface
        {
            [FlatBufferItem(0)]
            public int Foo { get; set; }
        }

        [FlatBufferTable]
        public class InterfaceTableVirtual : IInterface
        {
            [FlatBufferItem(0)]
            public virtual int Foo { get; set; }
        }

        // Properties that implement interfaces are virtual according to the property info. It's possible to be both
        // virtual and final.
        [FlatBufferStruct]
        public class InterfaceStructNonVirtual : IInterface
        {
            [FlatBufferItem(0)]
            public int Foo { get; set; }
        }

        [FlatBufferTable]
        public class InterfaceStructVirtual : IInterface
        {
            [FlatBufferItem(0)]
            public virtual int Foo { get; set; }
        }

        [FlatBufferTable]
        public class SortedVector<T>
        {
            [FlatBufferItem(0, SortedVector = true)]
            public virtual T Vector { get; set; }
        }

        [FlatBufferTable]
        public class SortedVectorKeyTable<T>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual T Key { get; set; }
        }

        [FlatBufferTable]
        public class SortedVectorDeprecatedKeyTable<T>
        {
            [FlatBufferItem(0, Key = true, Deprecated = true)]
            public virtual T Key { get; set; }
        }

        [FlatBufferTable]
        public class SortedVectorMultiKeyTable<T>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual T Key { get; set; }

            [FlatBufferItem(1, Key = true)]
            public virtual T Key2 { get; set; }
        }

        [FlatBufferStruct]
        public class SortedVectorKeyStruct<T>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual T Key { get; set; }
        }

        [FlatBufferTable]
        public class Table_NoGetter
        {
            private int value;

            [FlatBufferItem(0)]
            public virtual int Prop { set => this.value = value; }
        }

        [FlatBufferStruct]
        public class Struct_NoGetter
        {
            private int value;

            [FlatBufferItem(0)]
            public virtual int Prop { set => this.value = value; }
        }

        [FlatBufferTable]
        public class Table_NonPublicGetter
        {
            [FlatBufferItem(0)]
            public virtual int Prop { protected get; set; }
        }

        [FlatBufferStruct]
        public class Struct_NonPublicGetter
        {
            [FlatBufferItem(0)]
            public virtual int Prop { protected get; set; }
        }

        [FlatBufferTable]
        public class Table_NonVirtual_NoSetter
        {
            [FlatBufferItem(0)]
            public int Prop { get; }
        }

        [FlatBufferTable]
        public class Table_ForceWrite<T>
        {
            [FlatBufferItem(0, ForceWrite = true)]
            public virtual T Item { get; set; }
        }

        [FlatBufferStruct]
        public class Struct_ForceWrite<T>
        {
            [FlatBufferItem(0, ForceWrite = true)]
            public virtual T Item { get; set; }
        }

        [FlatBufferStruct]
        public class Struct_NonVirtual_NoSetter
        {
            [FlatBufferItem(0)]
            public int Prop { get; }
        }

        [FlatBufferTable(FileIdentifier = "abc")]
        public class Table_FileIdentifierTooShort
        {
            [FlatBufferItem(0)]
            public int Prop { get; set; }
        }

        [FlatBufferTable(FileIdentifier = "abcd")]
        public class Table_FileIdentifierOK
        {
            [FlatBufferItem(0)]
            public int Prop { get; set; }
        }

        [FlatBufferTable(FileIdentifier = "abcde")]
        public class Table_FileIdentifierTooLong
        {
            [FlatBufferItem(0)]
            public int Prop { get; set; }
        }

        [FlatBufferTable(FileIdentifier = "😍😚😙😵‍")]
        public class Table_FileIdentifierTooFancy
        {
            [FlatBufferItem(0)]
            public int Prop { get; set; }
        }

        [FlatBufferTable(FileIdentifier = "abcµ")]
        public class Table_FileIdentifierOutOfRange
        {
            [FlatBufferItem(0)]
            public int Prop { get; set; }
        }

        [FlatBufferTable]
        public class Table_Empty
        {
        }

        [FlatBufferStruct]
        public class Struct_Empty
        {
        }

        [FlatBufferTable]
        public class TableOnDeserialized_OK
        {
            protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_NotProtected
        {
            public void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_WrongParameter
        {
            protected void OnFlatSharpDeserialized(string s)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_NoParameter
        {
            protected void OnFlatSharpDeserialized()
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_OutParameter
        {
            protected void OnFlatSharpDeserialized(out FlatBufferDeserializationContext context)
            {
                context = null;
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_InParameter
        {
            protected void OnFlatSharpDeserialized(in FlatBufferDeserializationContext context)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_RefParameter
        {
            protected void OnFlatSharpDeserialized(ref FlatBufferDeserializationContext context)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_OptionalParameter
        {
            protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context = null)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_Multiple
        {
            protected void OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
            }

            protected void OnFlatSharpDeserialized(string s)
            {
            }
        }

        [FlatBufferTable]
        public class TableOnDeserialized_ReturnsNonVoid
        {
            protected string OnFlatSharpDeserialized(FlatBufferDeserializationContext context)
            {
                return "foo";
            }
        }

        [FlatBufferTable]
        public class TableWriteThrough_NotSupported
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int Property { get; set; }
        }

        [FlatBufferStruct]
        public class StructWriteThroughNonVirtual
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public int Property { get; set; }
        }

        [FlatBufferStruct]
        public class StructWriteThrough
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int Property { get; set; }

            [FlatBufferItem(1, WriteThrough = false)]
            public int Property2 { get; set; }
        }

        [FlatBufferTable]
        public class Table_NoCtor
        {
            public Table_NoCtor(string foo)
            {
            }

            [FlatBufferItem(0)] public virtual int Value { get; set; }
        }

        [FlatBufferStruct]
        public class StructDeprecatedField
        {
            [FlatBufferItem(0)]
            public virtual int A { get; set; }

            [FlatBufferItem(1, Deprecated = true)]
            public virtual int B { get; set; }
        }

        [FlatBufferTable]
        public class TableRequiredField<T>
        {
            [FlatBufferItem(0, Required = true)]
            public T Value { get; set; }
        }

        [FlatBufferStruct]
        public class StructRequiredField<T>
        {
            [FlatBufferItem(0, Required = true)]
            public T Value { get; set; }
        }
    }
}
