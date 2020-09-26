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

    /// <summary>
    /// The default type model provider for FlatSharp.
    /// </summary>
    public class FlatSharpTypeModelProvider : ITypeModelProvider
    {
        private readonly ConcurrentDictionary<Type, ITypeModel> cache = new ConcurrentDictionary<Type, ITypeModel>();
        private readonly List<ITypeModelProvider> providers = new List<ITypeModelProvider> { new ScalarTypeModelProvider() };
        private readonly List<Func<ITypeModel, ITypeModel>> decorators = new List<Func<ITypeModel, ITypeModel>>();

        /// <summary>
        /// Tries to create a type model based on the given type.
        /// </summary>
        public bool TryCreateTypeModel(Type type, out ITypeModel typeModel)
        {
            if (this.cache.TryGetValue(type, out typeModel))
            {
                return true;
            }

            typeModel = this.ApplyDecorators(this.GetTypeModel(type));
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

        /// <summary>
        /// Tries to resolve an FBS alias into a type model.
        /// </summary>
        public bool TryResolveFbsAlias(string alias, out ITypeModel typeModel)
        {
            typeModel = null;

            foreach (var provider in this.providers)
            {
                if (provider.TryResolveFbsAlias(alias, out typeModel))
                {
                    typeModel = this.ApplyDecorators(typeModel);
                    return true;
                }
            }

            if (alias == "string")
            {
                typeModel = this.ApplyDecorators(new StringTypeModel());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registers a custom type model provider. Custom providers can be thorught of
        /// as plugins and used to extend FlatSharp or alter properties of the 
        /// serialization system. Custom providers are a very advanced feature and 
        /// shouldn't be used without extensive testing and knowledge of FlatBuffers.
        /// 
        /// Use of this API almost certainly means that the binary format of FlatSharp
        /// will no longer be compatible with the official FlatBuffers library.
        /// </summary>
        public void RegisterCustomModelProvider(ITypeModelProvider provider)
        {
            this.providers.Add(provider);
        }

        /// <summary>
        /// Registers a custom type model decorator that is invoked after a type
        /// model has been resolved. A decorator can be thought of as a hook to allow
        /// callers to "wrap" an existing type model to apply some custom logic, validation,
        /// or otherwise.
        /// 
        /// Use of this API almost certainly means that the binary format of FlatSharp
        /// will no longer be compatible with the official FlatBuffers library.
        /// </summary>
        public void RegisterDecorator(Func<ITypeModel, ITypeModel> callback)
        {
            this.decorators.Add(callback);
        }

        private ITypeModel ApplyDecorators(ITypeModel input)
        {
            if (input != null)
            {
                foreach (var handler in this.decorators)
                {
                    ITypeModel wrapped = handler(input);
                    if (wrapped != null)
                    {
                        return wrapped;
                    }
                }
            }

            return input;
        }

        private ITypeModel GetTypeModel(Type type)
        {
            foreach (var provider in this.providers)
            {
                if (provider.TryCreateTypeModel(type, out ITypeModel model))
                {
                    return model;
                }
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
    }
}
