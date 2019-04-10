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
 
 namespace FlatSharp.TypeModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.Attributes;

    /// <summary>
    /// Describes the schema of a FlatBuffer table. Tables, analgous to classes, provide for mutable schema definitions over time
    /// by declaring a vatble mapping of field indexes to offsets.
    /// </summary>
    public class TableTypeModel : RuntimeTypeModel
    {
        /// <summary>
        /// Maps vtable index -> type model.
        /// </summary>
        private readonly Dictionary<int, TableMemberModel> memberTypes = new Dictionary<int, TableMemberModel>();

        /// <summary>
        /// Contains the vtable indices that have already been occupied.
        /// </summary>
        private readonly HashSet<int> occupiedVtableSlots = new HashSet<int>();

        internal TableTypeModel(Type clrType) : base(clrType)
        {
        }

        /// <summary>
        /// Schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Table;

        /// <summary>
        /// Tables are always addressed by reference, so the alignment is uoffset_t.
        /// </summary>
        public override int Alignment => sizeof(uint);

        /// <summary>
        /// The inline size. Tables are always "reference" types, so this is a fixed uoffset_t.
        /// </summary>
        public override int InlineSize => sizeof(uint);

        /// <summary>
        /// Maps the table index to the details about that member.
        /// </summary>
        public IReadOnlyDictionary<int, TableMemberModel> IndexToMemberMap => this.memberTypes;

        /// <summary>
        /// The default .ctor used for subclassing.
        /// </summary>
        public ConstructorInfo DefaultConstructor { get; private set; }

        protected override void Initialize()
        {
            var tableAttribute = this.ClrType.GetCustomAttribute<FlatBufferTableAttribute>();
            if (tableAttribute == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create table type model from type {this.ClrType.Name} because it does not have a [FlatBufferTable] attribute.");
            }

            EnsureClassCanBeInheritedByOutsideAssembly(this.ClrType, out var ctor);
            this.DefaultConstructor = ctor;

            var properties = this.ClrType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(x => new
                {
                    Property = x,
                    Attribute = x.GetCustomAttribute<FlatBufferItemAttribute>(),
                    DefaultValue = x.GetCustomAttribute<DefaultValueAttribute>(),
                })
                .Where(x => x.Attribute != null)
                .Where(x => !x.Attribute.Deprecated)
                .Select(x => new
                {
                    x.Property,
                    x.Attribute,
                    x.DefaultValue,
                    ItemTypeModel = RuntimeTypeModel.CreateFrom(x.Property.PropertyType),
                })
                .ToList();

            ushort maxIndex = 0;
            foreach (var property in properties)
            {
                bool hasDefaultValue = false;
                object defaultValue = null;

                if (property.DefaultValue != null)
                {
                    hasDefaultValue = true;
                    defaultValue = property.DefaultValue.Value;
                }
                
                ushort index = property.Attribute.Index;
                maxIndex = Math.Max(index, maxIndex);

                TableMemberModel model = new TableMemberModel(
                    property.ItemTypeModel,
                    property.Property,
                    index,
                    hasDefaultValue,
                    defaultValue);

                for (int i = 0; i < model.VTableSlotCount; ++i)
                {
                    if (!this.occupiedVtableSlots.Add(index + i))
                    {
                        throw new InvalidFlatBufferDefinitionException($"FlatBuffer Table {this.ClrType.Name} already defines a property with index {index}");
                    }
                }

                this.memberTypes[index] = model;
            }
        }

        internal static void EnsureClassCanBeInheritedByOutsideAssembly(Type type, out ConstructorInfo defaultConstructor)
        {
            if (!type.IsClass)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is not a class.");
            }

            if (type.IsSealed)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is sealed.");
            }

            if (type.IsAbstract)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is abstract.");
            }

            if (type.BaseType != typeof(object))
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} its base class is not System.Object.");
            }

            if (!type.IsPublic && !type.IsNestedPublic)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is not public.");
            }

            defaultConstructor =
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => c.IsPublic || c.IsFamily || c.IsFamilyOrAssembly)
                .Where(c => c.GetParameters().Length == 0)
                .SingleOrDefault();

            if (defaultConstructor == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't find a public/protected default default constructor for {type.Name}");
            }
        }
    }
}
