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
    using System.Collections.Generic;
    using System.Reflection;

    using FlatSharp.Attributes;

    /// <summary>
    /// The default type model provider for FlatSharp.
    /// </summary>
    public class FlatSharpTypeModelProvider : ITypeModelProvider
    {
        /// <summary>
        /// Tries to resolve an FBS alias into a type model.
        /// </summary>
        public bool TryResolveFbsAlias(TypeModelContainer container, string alias, out ITypeModel typeModel)
        {
            typeModel = null;

            if (alias == "string")
            {
                typeModel = new StringTypeModel();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to create a type model based on the given type.
        /// </summary>
        public bool TryCreateTypeModel(TypeModelContainer container, Type type, out ITypeModel typeModel)
        {
            if (type == typeof(string))
            {
                typeModel = new StringTypeModel();
                return true;
            }

            if (type == typeof(SharedString))
            {
                typeModel = new SharedStringTypeModel();
                return true;
            }

            if (type.IsArray)
            {
                typeModel = new ArrayVectorTypeModel(type, container);
                return true;
            }

            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>))
                {
                    typeModel = new ListVectorTypeModel(type, container);
                    return true;
                }

                if (genericDef == typeof(Memory<>) || genericDef == typeof(ReadOnlyMemory<>))
                {
                    typeModel = new MemoryVectorTypeModel(type, container);
                    return true;
                }

                if (genericDef == typeof(IIndexedVector<,>))
                {
                    typeModel = new IndexedVectorTypeModel(type, container);
                    return true;
                }
            }

            if (typeof(IUnion).IsAssignableFrom(type))
            {
                typeModel = new UnionTypeModel(type, container);
                return true;
            }

            if (type.IsEnum)
            {
                typeModel = new EnumTypeModel(type, container);
                return true;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType.IsEnum)
                {
                    typeModel = new NullableEnumTypeModel(type, container);
                    return true;
                }
            }

            var tableAttribute = type.GetCustomAttribute<FlatBufferTableAttribute>();
            if (tableAttribute != null)
            {
                typeModel = new TableTypeModel(type, container);
                return true;
            }

            var structAttribute = type.GetCustomAttribute<FlatBufferStructAttribute>();
            if (structAttribute != null)
            {
                typeModel = new StructTypeModel(type, container);
                return true;
            }

            typeModel = null;
            return false;
        }
    }
}
