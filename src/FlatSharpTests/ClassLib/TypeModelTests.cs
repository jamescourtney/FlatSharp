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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InterfaceTableFailure)));
        }

        [TestMethod]
        public void TypeModel_Table_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceTableSuccess));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(NonVirtualPropertyStruct<byte>)));
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
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(InterfaceStructFailure)));
        }

        [TestMethod]
        public void TypeModel_Struct_InterfaceImplementationVirtual()
        {
            RuntimeTypeModel.CreateFrom(typeof(InterfaceStructSuccess));
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
        public void TypeModel_Enum_UntaggedEnumNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(UntaggedEnum)));
        }

        [TestMethod]
        public void TypeModel_Enum_TaggedEnum()
        {
            var model = RuntimeTypeModel.CreateFrom(typeof(TaggedEnum));

            Assert.IsTrue(model is EnumTypeModel enumModel);
            Assert.AreEqual(typeof(TaggedEnum), model.ClrType);
            Assert.IsFalse(model.IsBuiltInType);
            Assert.IsTrue(model.IsFixedSize);
            Assert.AreEqual(FlatBufferSchemaType.Scalar, model.SchemaType);
        }

        [TestMethod]
        public void TypeModel_Vector_ListVectorOfStruct()
        {
            var model = this.VectorTest(typeof(IList<>), typeof(GenericStruct<bool>));
            Assert.IsTrue(model.IsList);
            Assert.IsFalse(model.IsMemoryVector);
            Assert.IsFalse(model.IsReadOnly);
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyListOfTable()
        {
            var model = this.VectorTest(typeof(IReadOnlyList<>), typeof(GenericTable<bool>));
            Assert.IsTrue(model.IsList);
            Assert.IsFalse(model.IsMemoryVector);
            Assert.IsTrue(model.IsReadOnly);
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfScalar()
        {
            var model = this.VectorTest(typeof(Memory<>), typeof(int));
            Assert.IsFalse(model.IsList);
            Assert.IsTrue(model.IsMemoryVector);
            Assert.IsFalse(model.IsReadOnly);
        }

        [TestMethod]
        public void TypeModel_Vector_MemoryOfEnum()
        {
            var model = this.VectorTest(typeof(Memory<>), typeof(TaggedEnum));
            Assert.IsFalse(model.IsList);
            Assert.IsTrue(model.IsMemoryVector);
            Assert.IsFalse(model.IsReadOnly);
        }

        [TestMethod]
        public void TypeModel_Vector_ReadOnlyMemoryOfScalar()
        {
            var model = this.VectorTest(typeof(ReadOnlyMemory<>), typeof(double));
            Assert.IsFalse(model.IsList);
            Assert.IsTrue(model.IsMemoryVector);
            Assert.IsTrue(model.IsReadOnly);
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
        public void TypeModel_SortedVector_OfTableWithoutKey_NotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(SortedVector<IList<GenericTable<string>>>)));
        }

        private VectorTypeModel VectorTest(Type vectorDefinition, Type innerType)
        {
            var model = (VectorTypeModel)RuntimeTypeModel.CreateFrom(vectorDefinition.MakeGenericType(innerType));

            Assert.AreEqual(FlatBufferSchemaType.Vector, model.SchemaType);
            Assert.AreEqual(model.ClrType.GetGenericTypeDefinition(), vectorDefinition);
            Assert.AreEqual(model.InlineSize, 4);
            Assert.AreEqual(model.Alignment, 4);

            var innerModel = RuntimeTypeModel.CreateFrom(innerType);
            Assert.AreEqual(innerModel, model.ItemTypeModel);

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
        public void TypeModel_Union_EnumsNotAllowed()
        {
            Assert.ThrowsException<InvalidFlatBufferDefinitionException>(() =>
                RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, TaggedEnum>>)));
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

            Assert.AreEqual(FlatBufferSchemaType.Union, model.SchemaType);
            Assert.AreEqual(3, model.UnionElementTypeModel.Length);
            Assert.AreEqual(FlatBufferSchemaType.String, model.UnionElementTypeModel[0].SchemaType);
            Assert.AreEqual(FlatBufferSchemaType.Table, model.UnionElementTypeModel[1].SchemaType);
            Assert.AreEqual(FlatBufferSchemaType.Struct, model.UnionElementTypeModel[2].SchemaType);
        }

        [TestMethod]
        public void TypeModel_StructsWithPadding()
        {
            // inner strut is float + double + bool = 4x float + 4x padding + 8x double + 1 bool = 17.
            // outer struct is inner + double + bool = 17x inner + 7x padding + 8x double + 1x bool = 33
            Type type = typeof(GenericStruct<GenericStruct<float>>);
            RuntimeTypeModel typeModel = RuntimeTypeModel.CreateFrom(type);

            Assert.AreEqual(typeModel.ClrType, type);
            Assert.AreEqual(FlatBufferSchemaType.Struct, typeModel.SchemaType);

            var structModel = (StructTypeModel)typeModel;
            Assert.AreEqual(3, structModel.Members.Count);

            Assert.AreEqual(FlatBufferSchemaType.Struct, structModel.Members[0].ItemTypeModel.SchemaType);
            Assert.AreEqual(0, structModel.Members[0].Index);
            Assert.AreEqual(0, structModel.Members[0].Offset);
            Assert.AreEqual(8, structModel.Members[0].ItemTypeModel.Alignment);
            Assert.AreEqual(17, structModel.Members[0].ItemTypeModel.InlineSize);

            Assert.AreEqual(FlatBufferSchemaType.Scalar, structModel.Members[1].ItemTypeModel.SchemaType);
            Assert.AreEqual(1, structModel.Members[1].Index);
            Assert.AreEqual(24, structModel.Members[1].Offset);
            Assert.AreEqual(8, structModel.Members[1].ItemTypeModel.Alignment);
            Assert.AreEqual(8, structModel.Members[1].ItemTypeModel.InlineSize);

            Assert.AreEqual(FlatBufferSchemaType.Scalar, structModel.Members[2].ItemTypeModel.SchemaType);
            Assert.AreEqual(2, structModel.Members[2].Index);
            Assert.AreEqual(32, structModel.Members[2].Offset);
            Assert.AreEqual(1, structModel.Members[2].ItemTypeModel.Alignment);
            Assert.AreEqual(1, structModel.Members[2].ItemTypeModel.InlineSize);

            Assert.AreEqual(33, structModel.InlineSize);
            Assert.AreEqual(8, structModel.Alignment);
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
            protected internal virtual double Double { get; set; }

            [FlatBufferItem(2)]
            protected virtual bool Bool { get; set; }
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
        public class InterfaceTableFailure : IInterface
        {
            [FlatBufferItem(0)]
            public int Foo { get; set; }
        }

        [FlatBufferTable]
        public class InterfaceTableSuccess : IInterface
        {
            [FlatBufferItem(0)]
            public virtual int Foo { get; set; }
        }

        // Properties that implement interfaces are virtual according to the property info. It's possible to be both
        // virtual and final.
        [FlatBufferStruct]
        public class InterfaceStructFailure : IInterface
        {
            [FlatBufferItem(0)]
            public int Foo { get; set; }
        }

        [FlatBufferTable]
        public class InterfaceStructSuccess : IInterface
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
        public class SortedVectorKeyTable<T> : IKeyedTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T Key { get; set; }
        }

        [FlatBufferStruct]
        public class SortedVectorKeyStruct<T> : IKeyedTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T Key { get; set; }
        }
    }
}
