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
        /// Tables can have vectors and other arbitrary data.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Gets the maximum used index in this vtable.
        /// </summary>
        public int MaxIndex => this.occupiedVtableSlots.Max();

        /// <summary>
        /// Maps the table index to the details about that member.
        /// </summary>
        public IReadOnlyDictionary<int, TableMemberModel> IndexToMemberMap => this.memberTypes;

        /// <summary>
        /// The default .ctor used for subclassing.
        /// </summary>
        public ConstructorInfo DefaultConstructor { get; private set; }

        /// <summary>
        /// The property type used as a key.
        /// </summary>
        public PropertyInfo KeyProperty { get; private set; }
        
        /// <summary>
        /// Gets the maximum size of a table assuming all members are populated include the vtable offset. 
        /// Does not consider alignment of the table, but does consider worst-case alignment of the members.
        /// </summary>
        internal int NonPaddedMaxTableInlineSize
        {
            // add sizeof(int) for soffset_t to vtable.
            get => this.IndexToMemberMap.Values.Sum(x => x.ItemTypeModel.MaxInlineSize) + sizeof(int);
        }

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
                })
                .Where(x => x.Attribute != null)
                .Where(x => !x.Attribute.Deprecated)
                .Select(x => new
                {
                    x.Property,
                    x.Attribute,
                    ItemTypeModel = RuntimeTypeModel.CreateFrom(x.Property.PropertyType),
                })
                .ToList();

            ushort maxIndex = 0;

            if (!properties.Any())
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create table type model from type {this.ClrType.Name} because it does not have any non-static [FlatBufferItem] properties.");
            }

            foreach (var property in properties)
            {
                bool hasDefaultValue = false;

                if (property.Attribute.Key)
                {
                    if (this.KeyProperty != null)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} has more than one [FlatBufferItemAttribute] with Key set to true.");
                    }
                    
                    if (!property.ItemTypeModel.IsBuiltInType)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} declares a key property on a type that is not built in. Only scalars and strings may be keys.");
                    }

                    if (property.ItemTypeModel is SharedStringTypeModel)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} declares a key property with a SharedString key. Shared Strings are not currently supported as keys in sorted vectors.");
                    }

                    this.KeyProperty = property.Property;
                }

                if (property.Attribute.SortedVector)
                {
                    // This must be a vector, and the inner model must have an item with the 'key' property.
                    if (property.ItemTypeModel is VectorTypeModel vectorTypeModel)
                    {
                        var vectorMemberModel = vectorTypeModel.ItemTypeModel;
                        if (vectorMemberModel is TableTypeModel tableModel)
                        {
                            if (tableModel.KeyProperty == null)
                            {
                                throw new InvalidFlatBufferDefinitionException($"Property '{property.Property.Name}' declares a sorted vector, but the member is a table that does not have a key.");
                            }
                        }
                        else
                        {
                            throw new InvalidFlatBufferDefinitionException($"Property '{property.Property.Name}' declares a sorted vector, but the member is not a table.");
                        }
                    }
                    else
                    {
                        throw new InvalidFlatBufferDefinitionException($"Property '{property.Property.Name}' declares the sortedVector option, but the property is not a vector type.");
                    }
                }

                object defaultValue = null;

                if (!object.ReferenceEquals(property.Attribute.DefaultValue, null))
                {
                    hasDefaultValue = true;
                    defaultValue = property.Attribute.DefaultValue;
                }
                
                ushort index = property.Attribute.Index;
                maxIndex = Math.Max(index, maxIndex);

                TableMemberModel model = new TableMemberModel(
                    property.ItemTypeModel,
                    property.Property,
                    index,
                    hasDefaultValue,
                    defaultValue,
                    property.Attribute.SortedVector,
                    property.Attribute.Key);

                for (int i = 0; i < model.VTableSlotCount; ++i)
                {
                    if (!this.occupiedVtableSlots.Add(index + i))
                    {
                        throw new InvalidFlatBufferDefinitionException($"FlatBuffer Table {this.ClrType.Name} already defines a property with index {index}. This may happen when unions are declared as these are double-wide members.");
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
