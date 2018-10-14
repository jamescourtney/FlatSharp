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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using FlatSharp.Attributes;

    /// <summary>
    /// Defines a type model for an object that can be parsed and serialized to/from a FlatBuffer. While generally most useful to the 
    /// FlatSharp codegen, this can be used to introspect on how FlatSharp interprets your schema.
    /// </summary>
    public abstract class RuntimeTypeModel
    {
        private static readonly ConcurrentDictionary<Type, RuntimeTypeModel> ModelMap = new ConcurrentDictionary<Type, RuntimeTypeModel>
        {
            [typeof(bool)] = new ScalarTypeModel(typeof(bool), sizeof(bool)),
            [typeof(byte)] = new ScalarTypeModel(typeof(byte), sizeof(byte)),
            [typeof(sbyte)] = new ScalarTypeModel(typeof(sbyte), sizeof(sbyte)),

            [typeof(ushort)] = new ScalarTypeModel(typeof(ushort), sizeof(ushort)),
            [typeof(short)] = new ScalarTypeModel(typeof(short), sizeof(short)),

            [typeof(uint)] = new ScalarTypeModel(typeof(uint), sizeof(uint)),
            [typeof(int)] = new ScalarTypeModel(typeof(int), sizeof(int)),
            [typeof(float)] = new ScalarTypeModel(typeof(float), sizeof(float)),

            [typeof(ulong)] = new ScalarTypeModel(typeof(ulong), sizeof(ulong)),
            [typeof(long)] = new ScalarTypeModel(typeof(long), sizeof(long)),
            [typeof(double)] = new ScalarTypeModel(typeof(double), sizeof(double)),

            [typeof(string)] = new StringTypeModel(),
        };

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
                if (type.GetCustomAttribute<FlatBufferTableAttribute>() != null)
                {
                    newModel = new TableTypeModel(type);
                }
                else if (type.GetCustomAttribute<FlatBufferStructAttribute>() != null)
                {
                    newModel = new StructTypeModel(type);
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

                if (newModel == null)
                {
                    throw new InvalidFlatBufferDefinitionException("Unable to create runtime type model for " + type.Name);
                }

                ModelMap[type] = newModel;
                newModel.Initialize();
                return newModel;
            }
        }
    }
}
