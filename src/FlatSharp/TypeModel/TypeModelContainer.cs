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
    using FlatSharp.Runtime;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A <see cref="TypeModelContainer"/> describes how FlatSharp resolves types. Each container contains 
    /// <see cref="ITypeModelProvider"/> instances, which are capable of resolving a CLR Type
    /// into an <see cref="ITypeModel"/> instance. A <see cref="ITypeModel"/> describes formatting
    /// rules and implements C# methods for serializing, parsing, and computing the max size
    /// of the type that it describes.
    /// 
    /// Type Models are resolved by the container in the order by which their providers were registered.
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
        /// Registers a type facade for the given type. Facades are a convenience mechanism to
        /// expose types to FlatSharp that are based on some well-known underlying type.
        /// An example of a Facade would be a DateTimeOffset that stores its value as ticks 
        /// in the underlying FlatBuffer. Another example of a Facade would be a Guid that stores its
        /// value as a string or byte array.
        /// 
        /// The binary format of the Facade will be the same as that of the underlying type, but will allow writing
        /// code using the Facade type.
        /// </summary>
        /// <typeparam name="TUnderlyingType">The underlying type.</typeparam>
        /// <typeparam name="TFacadeType">The Facade (exposed) type.</typeparam>
        /// <typeparam name="TConverter">The converter between <typeparamref name="TUnderlyingType"/> and <typeparamref name="TFacadeType"/>.</typeparam>
        /// <param name="throwOnTypeConflict">
        /// When set, throws an exception when registring a Facade for a type already resolved by this container.
        /// It is recommended to pass 'true' as this parameter to make you aware if future versions of FlatSharp
        /// begin supporting a type for which you have defined a facade.
        /// </param>
        public void RegisterTypeFacade<TUnderlyingType, TFacadeType, TConverter>(bool throwOnTypeConflict = true)
            where TConverter : struct, ITypeFacadeConverter<TUnderlyingType, TFacadeType>
        {
            if (!this.TryCreateTypeModel(typeof(TUnderlyingType), out ITypeModel? model))
            {
                throw new InvalidOperationException($"Unable to resolve type model for type '{typeof(TUnderlyingType).FullName}'.");
            }

            if (throwOnTypeConflict && this.TryCreateTypeModel(typeof(TFacadeType), throwOnError: false, out _))
            {
                throw new InvalidOperationException($"The Type model container already contains a type model for '{typeof(TUnderlyingType).FullName}', which may lead to unexpected behaviors.");
            }

            ITypeModelProvider provider;
            if (typeof(TUnderlyingType).IsValueType && Nullable.GetUnderlyingType(typeof(TUnderlyingType)) == null)
            {
                // non-nullable value type: omit the null check
                provider = new TypeFacadeTypeModelProvider<TConverter, TUnderlyingType, TFacadeType>(model);
            }
            else
            {
                // reference type or nullable value type.
                provider = new TypeFacadeTypeModelProvider<NullCheckingTypeFacadeConverter<TUnderlyingType, TFacadeType, TConverter>, TUnderlyingType, TFacadeType>(model);
            }

            // add first.
            this.providers.Insert(0, provider);
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
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.providers.Add(provider);
        }

        /// <summary>
        /// Attempts to resolve a type model from the given type.
        /// </summary>
        public bool TryCreateTypeModel(
            Type type,
            [NotNullWhen(true)] out ITypeModel? typeModel) => this.TryCreateTypeModel(type, true, out typeModel);

        /// <summary>
        /// Attempts to resolve a type model from the given type.
        /// </summary>
        public bool TryCreateTypeModel(
            Type type, 
            bool throwOnError,
            [NotNullWhen(true)] out ITypeModel? typeModel)
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

                    if (throwOnError)
                    {
                        throw;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the a type model for the given type or throws an exception.
        /// </summary>
        public ITypeModel CreateTypeModel(Type type)
        {
            if (!this.TryCreateTypeModel(type, out var typeModel))
            {
                throw new InvalidFlatBufferDefinitionException($"Failed to create or find type model for type '{CSharpHelpers.GetCompilableTypeName(type)}'.");
            }

            return typeModel;
        }
    }
}
