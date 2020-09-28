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

    /// <summary>
    /// A root type model provider that supports registering extensions.
    /// </summary>
    public sealed class TypeModelContainer
    {
        private readonly ConcurrentDictionary<Type, ITypeModel> cache = new ConcurrentDictionary<Type, ITypeModel>();
        private readonly List<ITypeModelProvider> providers = new List<ITypeModelProvider>();

        private TypeModelContainer()
        {
        }

        /// <summary>
        /// Creates an empty container with no types supported. No types will be supported unless explicitly registered. Have fun!
        /// </summary>
        public static TypeModelContainer CreateEmpty()
        {
            return new TypeModelContainer();
        }

        /// <summary>
        /// Creates a FlatSharp type model container with default support.
        /// </summary>
        public static TypeModelContainer CreateDefault()
        {
            var container = new TypeModelContainer();
            container.RegisterProvider(new ScalarTypeModelProvider());
            container.RegisterProvider(new FlatSharpTypeModelProvider());
            return container;
        }

        /// <summary>
        /// Registers a custom type model provider. Custom providers can be thought of
        /// as plugins and used to extend FlatSharp or alter properties of the 
        /// serialization system. Custom providers are a very advanced feature and 
        /// shouldn't be used without extensive testing and knowledge of FlatBuffers.
        /// 
        /// Use of this API almost certainly means that the binary format of FlatSharp
        /// will no longer be compatible with the official FlatBuffers library.
        /// 
        /// ITypeModelProvider instances are evaluated in registration order.
        /// </summary>
        public void RegisterProvider(ITypeModelProvider provider)
        {
            this.providers.Add(provider);
        }

        /// <summary>
        /// Attempts to resolve a type model from the given type.
        /// </summary>
        public bool TryCreateTypeModel(Type type, out ITypeModel typeModel)
        {
            if (this.cache.TryGetValue(type, out typeModel))
            {
                return true;
            }

            foreach (var provider in this.providers)
            {
                if (provider.TryCreateTypeModel(this, type, out typeModel))
                {
                    break;
                }
            }

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
        /// Attempts to resolve a type model from the given FBS type alias.
        /// </summary>
        public bool TryResolveFbsAlias(string alias, out ITypeModel typeModel)
        {
            typeModel = null;

            foreach (var provider in this.providers)
            {
                if (provider.TryResolveFbsAlias(this, alias, out typeModel))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates the a type model for the given type or throws an exception.
        /// </summary>
        public ITypeModel CreateTypeModel(Type type)
        {
            if (!this.TryCreateTypeModel(type, out var typeModel))
            {
                throw new InvalidFlatBufferDefinitionException($"Failed to create or find type model for type '{type.FullName}'.");
            }

            return typeModel;
        }
    }
}
