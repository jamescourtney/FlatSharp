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
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp.Attributes;

    /// <summary>
    /// The default type model provider for FlatSharp.
    /// </summary>
    public class FlatSharpTypeModelProvider : ITypeModelProvider
    {
        /// <summary>
        /// Tries to create a type model based on the given type.
        /// </summary>
        public bool TryCreateTypeModel(TypeModelContainer container, Type type, [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            if (type == typeof(string))
            {
                typeModel = new StringTypeModel(container);
                return true;
            }

            if (type.IsArray)
            {
                if (typeof(IFlatBufferUnion).IsAssignableFrom(type.GetElementType()))
                {
                    typeModel = new ArrayVectorOfUnionTypeModel(type, container);
                }
                else
                {
                    typeModel = new ArrayVectorTypeModel(type, container);
                }

                return true;
            }

            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>))
                {
                    if (typeof(IFlatBufferUnion).IsAssignableFrom(type.GetGenericArguments()[0]))
                    {
                        typeModel = new ListVectorOfUnionTypeModel(type, container);
                    }
                    else
                    {
                        typeModel = new ListVectorTypeModel(type, container);
                    }

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

            if (typeof(IFlatBufferUnion).IsAssignableFrom(type))
            {
                typeModel = new UnionTypeModel(type, container);
                return true;
            }

            if (type.IsEnum)
            {
                typeModel = new EnumTypeModel(type, container);
                return true;
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType is not null)
            {
                typeModel = new NullableTypeModel(container, type);
                return true;
            }

            var tableAttribute = type.GetCustomAttribute<FlatBufferTableAttribute>();
            var structAttribute = type.GetCustomAttribute<FlatBufferStructAttribute>();

            if (tableAttribute is not null && structAttribute is not null)
            {
                throw new InvalidFlatBufferDefinitionException($"Type '{CSharpHelpers.GetCompilableTypeName(type)}' is declared as both [FlatBufferTable] and [FlatBufferStruct].");
            }
            else if (tableAttribute is not null)
            {
                typeModel = new TableTypeModel(type, container);
                return true;
            }
            else if (structAttribute is not null)
            {
                if (type.IsValueType)
                {
                    typeModel = new ValueStructTypeModel(type, container);
                }
                else
                {
                    typeModel = new StructTypeModel(type, container);
                }

                return true;
            }

            typeModel = null;
            return false;
        }
    }
}
