/*
 * Copyright 2020 James Courtney
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

    internal class InternalTypeModelProvider : ITypeModelProvider
    {
        private readonly ITypeModelProvider scalarProvider = new ScalarTypeModelProvider();
        private readonly ConcurrentDictionary<Type, ITypeModel> cache = new ConcurrentDictionary<Type, ITypeModel>();

        public bool TryCreateTypeModel(Type type, out ITypeModel typeModel)
        {
            if (this.cache.TryGetValue(type, out typeModel))
            {
                return true;
            }

            typeModel = this.GetTypeModel(type);
            if (typeModel != null)
            {
                this.cache[type] = typeModel;
                try
                {
                    typeModel.Initialize();
                    return true;
                }
                catch
                {
                    this.cache.TryRemove(type, out _);
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        private ITypeModel GetTypeModel(Type type)
        {
            if (this.scalarProvider.TryCreateTypeModel(type, out var typeModel))
            {
                return typeModel;
            }

            if (type == typeof(string))
            {
                return new StringTypeModel();
            }

            if (type == typeof(SharedString))
            {
                return new SharedStringTypeModel();
            }

            if (type.IsArray)
            {
                return new ArrayVectorTypeModel(type, this);
            }

            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>))
                {
                    return new ListVectorTypeModel(type, this);
                }

                if (genericDef == typeof(Memory<>) || genericDef == typeof(ReadOnlyMemory<>))
                {
                    return new MemoryVectorTypeModel(type, this);
                }
            }

            if (typeof(IUnion).IsAssignableFrom(type))
            {
                return new UnionTypeModel(type, this);
            }

            if (type.IsEnum)
            {
                return new EnumTypeModel(type, this);
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType.IsEnum)
                {
                    return new NullableEnumTypeModel(type, this);
                }
            }

            var tableAttribute = type.GetCustomAttribute<FlatBufferTableAttribute>();
            if (tableAttribute != null)
            {
                return new TableTypeModel(type, this);
            }

            var structAttribute = type.GetCustomAttribute<FlatBufferStructAttribute>();
            if (structAttribute != null)
            {
                return new StructTypeModel(type, this);
            }

            return null;
        }

        public bool TryResolveFbsAlias(string alias, out ITypeModel typeModel)
        {
            if (alias == "string")
            {
                typeModel = new StringTypeModel();
                return true;
            }

            return this.scalarProvider.TryResolveFbsAlias(alias, out typeModel);
        }
    }
}
