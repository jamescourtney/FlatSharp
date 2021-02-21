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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<Dictionary<string, string>>)));
        }

        [TestMethod]
        public void TypeModel_Table_DuplicateIndex()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberTable)));
        }

        [TestMethod]
        public void TypeModel_Table_PrivateSetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(PrivateSetter)));
        }

        [TestMethod]
        public void TypeModel_Table_InternalSetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalSetter)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NoGetter)));
        }

        [TestMethod]
        public void TypeModel_Table_NonPublicGetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonPublicGetter)));
        }

        [TestMethod]
        public void TypeModel_Table_NonVirtual_NoSetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Table_NonVirtual_NoSetter)));
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtual_NoSetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonVirtual_NoSetter)));
        }

        [TestMethod]
        public void TypeModel_Struct_NoGetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NoGetter)));
        }

        [TestMethod]
        public void TypeModel_Struct_NonPublicGetter()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(Struct_NonPublicGetter)));
        }

        [TestMethod]
        public void TypeModel_Struct_StringNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<string>)));
        }

        [TestMethod]
        public void TypeModel_Struct_VectorNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<Memory<byte>>)));
        }

        [TestMethod]
        public void TypeModel_Struct_TableNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<GenericTable<int>>)));
        }

        [TestMethod]
        public void TypeModel_Struct_UnionNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<FlatBufferUnion<string, GenericTable<string>>>)));
        }

        [TestMethod]
        public void TypeModel_Struct_Misnumbered()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(MisnumberedStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_InaccessibleConstructor()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InaccessibleConstructorStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_InternalAccess()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InternalAccessStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtualProperty()
        {
            RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStruct<byte>));
        }

        [TestMethod]
        public void TypeModel_Struct_NonVirtualProperty_PrivateSetter_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStructPrivateSetter<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_Sealed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SealedStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_DoesNotInheritFromObject()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DoesNotInheritFromObjectStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_Abstract()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(AbstractStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_DuplicateNumbered()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(DuplicateNumberedStruct<byte>)));
        }

        [TestMethod]
        public void TypeModel_Struct_DefaultValue()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(StructWithDefaultValue)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<int?>)));
        }

        [TestMethod]
        public void TypeModel_Struct_OptionalEnum_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericStruct<TaggedEnum?>)));
        }

        [TestMethod]
        public void TypeModel_TypeCantBeTableAndStruct()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InvalidTableAndStruct)));
        }

        [TestMethod]
        public void TypeModel_Vector_VectorNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<IList<int>>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_UnionNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<FlatBufferUnion<string, GenericTable<string>>>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_OptionalScalarNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<int?>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_OptionalEnumNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<IList<TaggedEnum?>>)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, SortedVectorKeyTable<string>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_UnkeyedTable_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericTable<string>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Struct_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, GenericStruct<int>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Vector_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, IList<string>>)));
        }

        [TestMethod]
        public void TypeModel_Vector_IndexedVector_Union_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(IIndexedVector<int, FlatBufferUnion<string, GenericTable<string>>>)));
        }

        [TestMethod]
        public void TypeModel_Enum_UntaggedEnumNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum)));
        }

        [TestMethod]
        public void TypeModel_Enum_NullableUntaggedEnumNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum?)));
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
        public void TypeModel_Vector_MemoryOfScalar_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<int>)));
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfEnum_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(Memory<TaggedEnum>)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<int>)));
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyMemoryOfEnum_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(
                () => RuntimeTypeModel.CreateFrom(typeof(ReadOnlyMemory<TaggedEnum>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfStruct_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyStruct<int>[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfEnum_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfNullableEnum_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<TaggedEnum?[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfString_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<string[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfScalar_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfNullableScalar_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                   RuntimeTypeModel.CreateFrom(typeof(SortedVector<int?[]>)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<bool?>>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<byte?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<sbyte?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ushort?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<short?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<uint?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<int?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<ulong?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<long?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<float?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<double?>[]>)));
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() => RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<TaggedEnum?>[]>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithStructKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericStruct<string>>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithTableKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<GenericTable<string>>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithVectorKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<string[]>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithEnumKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithNullableEnumKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorKeyTable<TaggedEnum?>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithoutKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<GenericTable<string>>>)));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithSharedStringKeyKey_Allowed()
        {
            RuntimeTypeModel.CreateFrom(typeof(SortedVector<SortedVectorKeyTable<SharedString>[]>));
        }

        [TestMethod]
        public void TypeModel_SortedVector_OfTableWithMultipleKeys_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<SortedVectorMultiKeyTable<string>>>)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IList<string>>>)));

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IReadOnlyList<string>>>)));

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, Memory<int>>>)));

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, ReadOnlyMemory<int>>>)));

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, string[]>>)));
        }

        [TestMethod]
        public void TypeModel_Union_UnionsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, FlatBufferUnion<string, GenericStruct<int>>>>)));
        }

        [TestMethod]
        public void TypeModel_Union_ScalarsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int>>)));
        }

        [TestMethod]
        public void TypeModel_Union_OptionalScalarsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, int?>>)));
        }

        [TestMethod]
        public void TypeModel_Union_EnumsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum>>)));
        }

        [TestMethod]
        public void TypeModel_Union_OptionalEnumsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum?>>)));
        }

        [TestMethod]
        public void TypeModel_Union_StringAndSharedStringNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(FlatBufferUnion<string, SharedString>)));

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(FlatBufferUnion<string, GenericTable<string>, SharedString>)));
        }

        [TestMethod]
        public void FlatBufferSerializer_OnlyTablesAllowedAsRootType()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<GenericStruct<int>>());

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<IList<int>>());

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<string>());

            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                FlatBufferSerializer.Default.Compile<FlatBufferUnion<string>>());
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
    }
}
