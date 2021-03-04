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
    public class TypeModelTests
    {
        [TestMethod]
        public void TypeModel_Table_UnrecognizedPropertyType()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<Dictionary<string, string>>)));

            Assert.AreEqual("Failed to create or find type model for type 'System.Collections.Generic.Dictionary<System.String, System.String>'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_DuplicateIndex()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberTable)));

            Assert.AreEqual("FlatBuffer Table FlatSharpTests.TypeModelTests.DuplicateNumberTable already defines a property with index 0. This may happen when unions are declared as these are double-wide members.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_PrivateSetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(PrivateSetter)));

            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.PrivateSetter.String (Index 0) declared a set method, but it was not public/protected and virtual.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_InternalSetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalSetter)));

            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.InternalSetter.String (Index 0) declared a set method, but it was not public/protected and virtual.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_InterfaceImplementationNonVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceTableNonVirtual));
        }

        [TestMethod]
        public void TypeModel_Table_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceTableVirtual));
        }

        [TestMethod]
        public void TypeModel_Table_NoGetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NoGetter)));
            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.Table_NoGetter.Prop (Index 0) on did not declare a getter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_NonPublicGetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonPublicGetter)));
            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.Table_NonPublicGetter.Prop (Index 0) must declare a public getter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_NonVirtual_NoSetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonVirtual_NoSetter)));
            Assert.AreEqual("Non-virtual property FlatSharpTests.TypeModelTests.Table_NonVirtual_NoSetter.Prop (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_FileIdentifiers()
        {
            Assert.IsInstanceOfType(RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierOK)), typeof(TableTypeModel));

            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooShort)));
            Assert.AreEqual("File identifier 'abc' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooLong)));
            Assert.AreEqual("File identifier 'abcde' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_FileIdentifierTooFancy)));
            Assert.AreEqual("File identifier '😍😚😙😵‍' is invalid. FileIdentifiers must be exactly 4 ASCII characters.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_InternalConstructor()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalConstructorTable<byte>)));
            Assert.AreEqual("Default constructor for 'FlatSharpTests.TypeModelTests.InternalConstructorTable<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_PrivateConstructor()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(PrivateConstructorTable<byte>)));
            Assert.AreEqual("Default constructor for 'FlatSharpTests.TypeModelTests.PrivateConstructorTable<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Table_ProtectedConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(ProtectedConstructorTable<byte>));
        }

        [TestMethod]
        public void TypeModel_Table_SpecialConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(SpecialConstructorTable<byte>));
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtual_NoSetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonVirtual_NoSetter)));
            Assert.AreEqual("Non-virtual property FlatSharpTests.TypeModelTests.Struct_NonVirtual_NoSetter.Prop (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_NoGetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NoGetter)));
            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.Struct_NoGetter.Prop (Index 0) on did not declare a getter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_NonPublicGetter()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonPublicGetter)));
            Assert.AreEqual("Property FlatSharpTests.TypeModelTests.Struct_NonPublicGetter.Prop (Index 0) must declare a public getter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_StringNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<string>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.String>' property Value (Index 0) with type System.String cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_VectorNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<Memory<byte>>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Memory<System.Byte>>' property Value (Index 0) with type System.Memory<System.Byte> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_TableNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<GenericTable<int>>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<FlatSharpTests.TypeModelTests.GenericTable<System.Int32>>' property Value (Index 0) with type FlatSharpTests.TypeModelTests.GenericTable<System.Int32> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_UnionNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<FlatBufferUnion<string, GenericTable<string>>>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>>>' property Value (Index 0) with type FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_Misnumbered()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(MisnumberedStruct<byte>)));
            Assert.AreEqual("FlatBuffer struct FlatSharpTests.TypeModelTests.MisnumberedStruct<System.Byte> does not declare an item with index 1. Structs must have sequenential indexes starting at 0.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_InaccessibleConstructor()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InaccessibleConstructorStruct<byte>)));
            Assert.AreEqual("Default constructor for 'FlatSharpTests.TypeModelTests.InaccessibleConstructorStruct<System.Byte>' is not visible to subclasses outside the assembly.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_ProtectedConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(ProtectedConstructorStruct<byte>));
        }

        [TestMethod]
        public void TypeModel_Struct_SpecialConstructor()
        {
            RuntimeTypeModel.CreateFrom(typeof(SpecialConstructorStruct<byte>));
        }

        [TestMethod]
        public void TypeModel_Struct_InternalAccess()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalAccessStruct<byte>)));
            Assert.AreEqual("Can't create type model from type FlatSharpTests.TypeModelTests.InternalAccessStruct<System.Byte> because it is not public.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtualProperty()
        {
            RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStruct<byte>));
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtualProperty_PrivateSetter_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStructPrivateSetter<byte>)));
            Assert.AreEqual("Non-virtual property FlatSharpTests.TypeModelTests.NonVirtualPropertyStructPrivateSetter<System.Byte>.Value (Index 0) must declare a public/protected and non-abstract setter.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_Sealed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SealedStruct<byte>)));
            Assert.AreEqual("Can't create type model from type FlatSharpTests.TypeModelTests.SealedStruct<System.Byte> because it is sealed.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_DoesNotInheritFromObject()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DoesNotInheritFromObjectStruct<byte>)));
            Assert.AreEqual("Can't create type model from type FlatSharpTests.TypeModelTests.DoesNotInheritFromObjectStruct<System.Byte> its base class is not System.Object.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_Abstract()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(AbstractStruct<byte>)));
            Assert.AreEqual("Can't create type model from type FlatSharpTests.TypeModelTests.AbstractStruct<System.Byte> because it is abstract.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_DuplicateNumbered()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberedStruct<byte>)));
            Assert.AreEqual("FlatBuffer struct FlatSharpTests.TypeModelTests.DuplicateNumberedStruct<System.Byte> does not declare an item with index 2. Structs must have sequenential indexes starting at 0.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_DefaultValue()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(StructWithDefaultValue)));
            Assert.AreEqual("FlatBuffer struct FlatSharpTests.TypeModelTests.StructWithDefaultValue declares default value on index 0. Structs may not have default values.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_InterfaceImplementationNonVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceStructNonVirtual));
        }

        [TestMethod]
        public void TypeModel_Struct_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceStructVirtual));
        }

        [TestMethod]
        public void TypeModel_Struct_OptionalField_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<int?>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Nullable<System.Int32>>' property Value (Index 0) with type System.Nullable<System.Int32> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Struct_OptionalEnum_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<TaggedEnum?>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>>' property Value (Index 0) with type System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum> cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_TypeCantBeTableAndStruct()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InvalidTableAndStruct)));
            Assert.AreEqual("Type 'FlatSharpTests.TypeModelTests.InvalidTableAndStruct' is declared as both [FlatBufferTable] and [FlatBufferStruct].", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_VectorNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<IList<int>>>)));
            Assert.AreEqual("Type 'System.Collections.Generic.IList<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_UnionNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<FlatBufferUnion<string, GenericTable<string>>>>)));
            Assert.AreEqual("Type 'FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_OptionalScalarNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<int?>>)));
            Assert.AreEqual("Type 'System.Nullable<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_OptionalEnumNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<TaggedEnum?>>)));
            Assert.AreEqual("Type 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_Indexed()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(GenericTable<IIndexedVector<string, SortedVectorKeyTable<string>>>));
            Assert.AreEqual(FlatBufferSchemaType.Table, model.SchemaType);

            TableTypeModel tableModel = (TableTypeModel)model;
            var firstMember = tableModel.IndexToMemberMap[0];

            Assert.IsTrue(firstMember.IsSortedVector); // sorted vector is set even though it's not explicitly declared. Indexed Vectors force it to be set.

            var vectorModel = firstMember.ItemTypeModel;
            Assert.AreEqual(FlatBufferSchemaType.Vector, vectorModel.SchemaType);
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_MismatchedKeyTypes_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, SortedVectorKeyTable<string>>)));
            Assert.AreEqual("FlatSharp indexed vector keys must have the same type as the key of the value. KeyType = System.Int32, Value Key Type = 'FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.String>'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_UnkeyedTable_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericTable<string>>)));
            Assert.AreEqual("Indexed vector values must have a property with the key attribute defined. Table = 'FlatSharpTests.TypeModelTests.GenericTable<System.String>'", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Struct_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericStruct<int>>)));
            Assert.AreEqual("Indexed vector values must be flatbuffer tables. Type = 'FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>'", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Vector_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, IList<string>>)));
            Assert.AreEqual("Indexed vector values must be flatbuffer tables. Type = 'System.Collections.Generic.IList<System.String>'", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Union_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, FlatBufferUnion<string, GenericTable<string>>>)));
            Assert.AreEqual("Indexed vector values must be flatbuffer tables. Type = 'FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericTable<System.String>>'", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Enum_UntaggedEnumNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum)));
            Assert.AreEqual("Enum 'FlatSharpTests.TypeModelTests.UntaggedEnum' is not tagged with a [FlatBufferEnum] attribute.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Enum_NullableUntaggedEnumNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum?)));
            Assert.AreEqual("Enum 'FlatSharpTests.TypeModelTests.UntaggedEnum' is not tagged with a [FlatBufferEnum] attribute.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Enum_TaggedEnum()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(TaggedEnum));

            Assert.IsTrue(model is EnumTypeModel enumModel);
            Assert.AreEqual(typeof(TaggedEnum), model.ClrType);
            Assert.IsTrue(model.IsFixedSize);
        }

        [TestMethod]
        public void TypeModel_Enum_NullableTaggedEnum()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(TaggedEnum?));

            Assert.IsTrue(model is NullableTypeModel nullableModel);
            Assert.AreEqual(typeof(TaggedEnum?), model.ClrType);
            Assert.IsTrue(model.IsFixedSize);
        }

        [TestMethod]
        public void TypeModel_Vector_ListVectorOfStruct()
        {
            var model = this.VectorTest(typeof(IList<>), typeof(GenericStruct<bool>));
            Assert.IsInstanceOfType(model, typeof(ListVectorTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyListOfTable()
        {
            var model = this.VectorTest(typeof(IReadOnlyList<>), typeof(GenericTable<bool>));
            Assert.IsInstanceOfType(model, typeof(ListVectorTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfByte()
        {
            var model = this.VectorTest(typeof(Memory<>), typeof(byte));
            Assert.IsInstanceOfType(model, typeof(MemoryVectorTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_NullableMemoryOfByte()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(Memory<byte>?));
            Assert.IsInstanceOfType(model, typeof(NullableTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_NullableReadOnlyMemoryOfByte()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<byte>?));
            Assert.IsInstanceOfType(model, typeof(NullableTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfScalar_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<int>)));
            Assert.AreEqual("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfEnum_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<TaggedEnum>)));
            Assert.AreEqual("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyMemoryOfByte()
        {
            var model = this.VectorTest(typeof(ReadOnlyMemory<>), typeof(byte));
            Assert.IsInstanceOfType(model, typeof(MemoryVectorTypeModel));
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyMemoryOfScalar_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<int>)));
            Assert.AreEqual("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyMemoryOfEnum_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<TaggedEnum>)));
            Assert.AreEqual("Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfStruct_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyStruct<int>[]>)));
            Assert.AreEqual("Property 'Vector' declares a sorted vector, but the member is not a table. Type = FlatSharpTests.TypeModelTests.SortedVectorKeyStruct<System.Int32>[].", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfEnum_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum[]>)));
            Assert.AreEqual("Property 'Vector' declares a sorted vector, but the member is not a table. Type = FlatSharpTests.TypeModelTests.TaggedEnum[].", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfNullableEnum_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum?[]>)));
            Assert.AreEqual("Type 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfString_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<string[]>)));
            Assert.AreEqual("Property 'Vector' declares a sorted vector, but the member is not a table. Type = System.String[].", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfScalar_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int[]>)));
            Assert.AreEqual("Property 'Vector' declares a sorted vector, but the member is not a table. Type = System.Int32[].", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfNullableScalar_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int?[]>)));
            Assert.AreEqual("Type 'System.Nullable<System.Int32>' is not a valid vector member.", ex.Message);
        }

        [TestMethod]
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

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithOptionalKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<bool?>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Boolean>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<byte?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Byte>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<sbyte?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.SByte>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ushort?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt16>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<short?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int16>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<uint?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt32>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<int?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int32>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ulong?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.UInt64>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<long?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Int64>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<float?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Single>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<double?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<System.Double>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<TaggedEnum?>[]>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithStructKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericStruct<string>>>>)));
            Assert.AreEqual("Struct 'FlatSharpTests.TypeModelTests.GenericStruct<System.String>' property Value (Index 0) with type System.String cannot be part of a flatbuffer struct.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithTableKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericTable<string>>>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<FlatSharpTests.TypeModelTests.GenericTable<System.String>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithVectorKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<string[]>>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.String[]> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithEnumKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum>>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<FlatSharpTests.TypeModelTests.TaggedEnum> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithNullableEnumKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum?>>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorKeyTable<System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>> declares a key property on a type that that does not support being a key in a sorted vector.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithoutKey_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<GenericTable<string>>>)));
            Assert.AreEqual("Property 'Vector' declares a sorted vector, but the member does not have a key defined. Type = System.Collections.Generic.IList<FlatSharpTests.TypeModelTests.GenericTable<System.String>>.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithSharedStringKeyKey_Allowed()
        {
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<SharedString>[]>));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithMultipleKeys_NotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorMultiKeyTable<string>>>)));
            Assert.AreEqual("Table FlatSharpTests.TypeModelTests.SortedVectorMultiKeyTable<System.String> has more than one [FlatBufferItemAttribute] with Key set to true.", ex.Message);
        }

        private BaseVectorTypeModel VectorTest(Type vectorDefinition, Type innerType)
        {
            var model = (BaseVectorTypeModel)RuntimeTypeModel.CreateFrom(vectorDefinition.MakeGenericType(innerType));

            Assert.AreEqual(model.ClrType.GetGenericTypeDefinition(), vectorDefinition);
            Assert.AreEqual(model.PhysicalLayout.Single().InlineSize, 4);
            Assert.AreEqual(model.PhysicalLayout.Single().Alignment, 4);

            var innerModel = RuntimeTypeModel.CreateFrom(innerType);
            Assert.AreEqual(innerModel.ClrType, model.ItemTypeModel.ClrType);

            return model;
        }

        /// <summary>
        /// This scenario actually works with flatsharp, but the flatc compiler does not support this, so it doesn't seem to be an officially
        /// sanctioned feature.
        /// </summary>
        [TestMethod]
        public void TypeModel_Union_VectorsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IList<string>>>)));
            Assert.AreEqual("Unions may not store 'System.Collections.Generic.IList<System.String>'.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IReadOnlyList<string>>>)));
            Assert.AreEqual("Unions may not store 'System.Collections.Generic.IReadOnlyList<System.String>'.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, Memory<byte>>>)));
            Assert.AreEqual("Unions may not store 'System.Memory<System.Byte>'.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, ReadOnlyMemory<byte>>>)));
            Assert.AreEqual("Unions may not store 'System.ReadOnlyMemory<System.Byte>'.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, string[]>>)));
            Assert.AreEqual("Unions may not store 'System.String[]'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_UnionsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, FlatBufferUnion<string, GenericStruct<int>>>>)));
            Assert.AreEqual("Unions may not store 'FlatSharp.FlatBufferUnion<System.String, FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>>'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_ScalarsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int>>)));
            Assert.AreEqual("Unions may not store 'System.Int32'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_OptionalScalarsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int?>>)));
            Assert.AreEqual("Unions may not store 'System.Nullable<System.Int32>'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_EnumsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum>>)));
            Assert.AreEqual("Unions may not store 'FlatSharpTests.TypeModelTests.TaggedEnum'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_OptionalEnumsNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum?>>)));
            Assert.AreEqual("Unions may not store 'System.Nullable<FlatSharpTests.TypeModelTests.TaggedEnum>'.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_StringAndSharedStringNotAllowed()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(FlatBufferUnion<string, SharedString>)));
            Assert.AreEqual("Unions may only contain one string type. String and SharedString cannot cohabit the union.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(FlatBufferUnion<string, GenericTable<string>, SharedString>)));
            Assert.AreEqual("Unions may only contain one string type. String and SharedString cannot cohabit the union.", ex.Message);
        }

        [TestMethod]
        public void FlatBufferSerializer_OnlyTablesAllowedAsRootType()
        {
            var ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<GenericStruct<int>>());
            Assert.AreEqual("Can only compile [FlatBufferTable] elements as root types. Type 'FlatSharpTests.TypeModelTests.GenericStruct<System.Int32>' is a Struct.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<IList<int>>());
            Assert.AreEqual("Can only compile [FlatBufferTable] elements as root types. Type 'System.Collections.Generic.IList<System.Int32>' is a Vector.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<string>());
            Assert.AreEqual("Can only compile [FlatBufferTable] elements as root types. Type 'System.String' is a String.", ex.Message);

            ex = Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<FlatBufferUnion<string>>());
            Assert.AreEqual("Can only compile [FlatBufferTable] elements as root types. Type 'FlatSharp.FlatBufferUnion<System.String>' is a Union.", ex.Message);
        }

        [TestMethod]
        public void TypeModel_Union_StructsTablesStringsAllowed()
        {
            var tableModel = (TableTypeModel)RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, GenericTable<string>, GenericStruct<int>>>));
            Assert.AreEqual(1, tableModel.IndexToMemberMap.Count);

            var model = (UnionTypeModel)tableModel.IndexToMemberMap[0].ItemTypeModel;

            Assert.AreEqual(3, model.UnionElementTypeModel.Length);
            Assert.IsInstanceOfType(model.UnionElementTypeModel[0], typeof(StringTypeModel));
            Assert.IsInstanceOfType(model.UnionElementTypeModel[1], typeof(TableTypeModel));
            Assert.IsInstanceOfType(model.UnionElementTypeModel[2], typeof(StructTypeModel));
        }

        [TestMethod]
        public void TypeModel_StructsWithPadding()
        {
            // inner strut is float + double + bool = 4x float + 4x padding + 8x double + 1 bool = 17.
            // outer struct is inner + double + bool = 17x inner + 7x padding + 8x double + 1x bool = 33
            Type type = typeof(GenericStruct<GenericStruct<float>>);
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            Assert.AreEqual(typeModel.ClrType, type);
            Assert.IsInstanceOfType(typeModel, typeof(StructTypeModel));

            var structModel = (StructTypeModel)typeModel;
            Assert.AreEqual(3, structModel.Members.Count);

            Assert.IsInstanceOfType(structModel.Members[0].ItemTypeModel, typeof(StructTypeModel));
            Assert.AreEqual(0, structModel.Members[0].Index);
            Assert.AreEqual(0, structModel.Members[0].Offset);
            Assert.AreEqual(8, structModel.Members[0].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.AreEqual(17, structModel.Members[0].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.IsInstanceOfType(structModel.Members[1].ItemTypeModel, typeof(ScalarTypeModel));
            Assert.AreEqual(1, structModel.Members[1].Index);
            Assert.AreEqual(24, structModel.Members[1].Offset);
            Assert.AreEqual(8, structModel.Members[1].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.AreEqual(8, structModel.Members[1].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.IsInstanceOfType(structModel.Members[2].ItemTypeModel, typeof(ScalarTypeModel));
            Assert.AreEqual(2, structModel.Members[2].Index);
            Assert.AreEqual(32, structModel.Members[2].Offset);
            Assert.AreEqual(1, structModel.Members[2].ItemTypeModel.PhysicalLayout.Single().Alignment);
            Assert.AreEqual(1, structModel.Members[2].ItemTypeModel.PhysicalLayout.Single().InlineSize);

            Assert.AreEqual(33, structModel.PhysicalLayout.Single().InlineSize);
            Assert.AreEqual(8, structModel.PhysicalLayout.Single().Alignment);
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
            protected SpecialConstructorStruct(FlatSharpDeserializationContext context) { }

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
            protected SpecialConstructorTable(FlatSharpDeserializationContext context) { }

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
    }
}
