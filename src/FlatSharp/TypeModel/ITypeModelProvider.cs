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

    /// <summary>
    /// Provides a type model to FlatSharp.
    /// </summary>
    public interface ITypeModelProvider
    {
        /// <summary>
        /// Creates a type model for the given type. Will only be invoked if <see cref="CanProvide(Type)"/> returns true.0
        /// </summary>
        bool TryCreateTypeModel(Type type, out ITypeModel typeModel);
    }

    /// <summary>
    /// Extensions for ITypeModelProvider.
    /// </summary>
    public static class TypeModelProviderExtensions
    {
        public static ITypeModel CreateTypeModel(this ITypeModelProvider provider, Type type)
        {
            if (!provider.TryCreateTypeModel(type, out var typeModel))
            {
                throw new InvalidFlatBufferDefinitionException($"Failed to create or find type model for type '{type.FullName}'.");
            }

            return typeModel;
        }
    }
}
