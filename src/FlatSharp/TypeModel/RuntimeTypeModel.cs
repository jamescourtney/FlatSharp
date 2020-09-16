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
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a type model for an object that can be parsed and serialized to/from a FlatBuffer. While generally most useful to the 
    /// FlatSharp codegen, this can be used to introspect on how FlatSharp interprets your schema.
    /// </summary>
    public abstract class RuntimeTypeModel
    {
        private static readonly ConcurrentDictionary<Type, RuntimeTypeModel> ModelMap = new ConcurrentDictionary<Type, RuntimeTypeModel>();

        internal RuntimeTypeModel(Type clrType)
        {
            this.ClrType = clrType;
        }

        /// <summary>
        /// Initializes this runtime type model instance.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// The type of the item in the CLR.
        /// </summary>
        public Type ClrType { get; }

        /// <summary>
        /// The schema type of this item.
        /// </summary>
        public abstract FlatBufferSchemaType SchemaType { get; }

        /// <summary>
        /// The alignment of this runtime type.
        /// </summary>
        public abstract int Alignment { get; }

        /// <summary>
        /// The inline size of this runtime type. For references, this will be the size of a reference.
        /// For scalars and structs, this will be the total size of the object.
        /// </summary>
        public abstract int InlineSize { get; }

        /// <summary>
        /// Indicates if this item is fixed size or not.
        /// </summary>
        public abstract bool IsFixedSize { get; }

        /// <summary>
        /// Indicates if this type model can be part of a struct.
        /// </summary>
        public abstract bool IsValidStructMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a table.
        /// </summary>
        public abstract bool IsValidTableMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a vector.
        /// </summary>
        public abstract bool IsValidVectorMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a union.
        /// </summary>
        public abstract bool IsValidUnionMember { get; }

        /// <summary>
        /// Indicates if this type model can be a sorted vector key.
        /// </summary>
        public abstract bool IsValidSortedVectorKey { get; }

        /// <summary>
        /// Gets the maximum inline size of this item when padded for alignment, when stored in a table or vector.
        /// </summary>
        public virtual int MaxInlineSize
        {
            get => this.InlineSize + SerializationHelpers.GetMaxPadding(this.Alignment);
        }

        /// <summary>
        /// Indicates if this type is built into FlatSharp and no parser needs to be generated.
        /// </summary>
        public virtual bool IsBuiltInType
        {
            get => false;
        }

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public virtual bool ValidateDefaultValue(object defaultValue)
        {
            return false;
        }

        /// <summary>
        /// Gets or creates a runtime type model from the given type.
        /// </summary>
        public static RuntimeTypeModel CreateFrom(Type type)
        {
            if (ModelMap.TryGetValue(type, out var value))
            {
                return value;
            }

            lock (SharedLock.Instance)
            {
                RuntimeTypeModel newModel = null;

                if (BuiltInType.BuiltInTypes.TryGetValue(type, out IBuiltInType builtInType))
                {
                    newModel = builtInType.TypeModel;
                }
                else if (type.GetCustomAttribute<FlatBufferStructAttribute>() != null && type.GetCustomAttribute<FlatBufferTableAttribute>() != null)
                {
                    throw new InvalidFlatBufferDefinitionException($"A type cannot be both a class and a struct Type = '{type}'.");
                }
                else if (type.GetCustomAttribute<FlatBufferTableAttribute>() != null)
                {
                    newModel = new TableTypeModel(type);
                }
                else if (type.GetCustomAttribute<FlatBufferStructAttribute>() != null)
                {
                    newModel = new StructTypeModel(type);
                }
                else if (typeof(IUnion).IsAssignableFrom(type))
                {
                    // Keep this above the "isgeneric" check since our union types are generic.
                    newModel = new UnionTypeModel(type);
                }
                else if (type.IsGenericType)
                {
                    var genericDefinition = type.GetGenericTypeDefinition();
                    if (genericDefinition == typeof(IList<>) ||
                        genericDefinition == typeof(Memory<>) ||
                        genericDefinition == typeof(ReadOnlyMemory<>) ||
                        genericDefinition == typeof(IReadOnlyList<>))
                    {
                        newModel = new VectorTypeModel(type);
                    }
                }
                else if (type.IsArray)
                {
                    newModel = new VectorTypeModel(type);
                }
                else if (type.GetCustomAttribute<FlatBufferEnumAttribute>() != null)
                {
                    ScalarTypeModel scalarModel = (ScalarTypeModel)RuntimeTypeModel.CreateFrom(Enum.GetUnderlyingType(type));
                    newModel = new EnumTypeModel(type, scalarModel.InlineSize);
                }

                if (newModel == null)
                {
                    throw new InvalidFlatBufferDefinitionException($"Unable to create runtime type model for '{type.Name}'. This type is not supported. Please consult the documentation for a list of supported types.");
                }

                ModelMap[type] = newModel;
                try
                {
                    newModel.Initialize();
                }
                catch
                {
                    ModelMap.TryRemove(type, out _);
                    throw;
                }

                return newModel;
            }
        }
    }
}
