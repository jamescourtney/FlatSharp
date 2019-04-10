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
        public void TypeModel_Vector_ReadOnlyMemoryOfScalar()
        {
            var model = this.VectorTest(typeof(ReadOnlyMemory<>), typeof(double));
            Assert.IsFalse(model.IsList);
            Assert.IsTrue(model.IsMemoryVector);
            Assert.IsTrue(model.IsReadOnly);
        }

        [TestMethod]
        public void TypeModel_Union_Vectors()
        {
            var model = (TableTypeModel)RuntimeTypeModel.CreateFrom(typeof(GenericTable<FlatBufferUnion<string, IList<string>>>));

            var index = model.IndexToMemberMap[0];

            // 1 is the double part of the union. Model shouldn't contain it.
            Assert.IsFalse(model.IndexToMemberMap.ContainsKey(1));

            Assert.AreEqual(index.ItemTypeModel.SchemaType, FlatBufferSchemaType.Union);
            Assert.AreEqual(index.VTableSlotCount, 2);
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
        public class UnionMemberType<T1, T2>
        {
            [FlatBufferItem(0)]
            public string Value { get; set; }

            // Unions are double-wide types, so indices 1 and 2 are reserved.
            [FlatBufferItem(1)]
            public FlatBufferUnion<T1, T2> Union { get; set; }

            [FlatBufferItem(3)]
            public int Int { get; set; }
        }

        [FlatBufferTable]
        public class OverlappingUnionIndex
        { 
            [FlatBufferItem(0)]
            public FlatBufferUnion<string, IList<string>> Union { get; set; }

            // Invalid -- this should be at index 2.
            [FlatBufferItem(1)]
            public string FooBar { get; set; }
        }
    }
}
