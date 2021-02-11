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
    using System.Collections.Immutable;
    using System.Linq.Expressions;
    using System.Reflection;

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

        public TypeModelContainer WithExtension<TUnderlyingType, TType>(
            ITypeModel<TUnderlyingType> underlyingTypeModel,
            Expression<Func<TUnderlyingType, TType>> convertFromLambda,
            Expression<Func<TType, TUnderlyingType>> convertToLambda)
        {
            var fromUnderlying = GetMethod(convertFromLambda);
            var toUnderlying = GetMethod(convertToLambda);

            var model = new ExtensionTypeModel<TType, TUnderlyingType>(
                toUnderlying,
                fromUnderlying,
                underlyingTypeModel);

            this.providers.Insert(0, new ExtensionTypeModelProvider<TType>(model));
            return this;
        }

        private static MethodInfo GetMethod<TInput, TOutput>(Expression<Func<TInput, TOutput>> expr)
        {
            static void Throw()
            {
                throw new ArgumentException("Lambda body must be a public static method call in a publicly visible class. The method must accept one argument that is the argument to the lambda");
            }

            if (!(expr is LambdaExpression lambda))
            {
                Throw();
            }

            if (!(lambda.Body is MethodCallExpression methodExpr))
            {
                Throw();
            }

            if (!methodExpr.Method.IsStatic || 
                !(methodExpr.Method.DeclaringType.IsPublic || methodExpr.Method.DeclaringType.IsNestedPublic) ||
                methodExpr.Method.IsSpecialName)
            {
                Throw();
            }

            if (methodExpr.Arguments.Count != 1 || methodExpr.Arguments[0] != lambda.Parameters[0])
            {
                Throw();
            }

            return methodExpr.Method;
        }

        private class ExtensionTypeModelProvider<T> : ITypeModelProvider
        {
            private ITypeModel<T> model;

            public ExtensionTypeModelProvider(
                ITypeModel<T> model)
            {
                this.model = model;
            }

            public bool TryCreateTypeModel(TypeModelContainer container, Type type, out ITypeModel typeModel)
            {
                if (type == typeof(T))
                {
                    typeModel = this.model;
                    return true;
                }

                typeModel = null;
                return false;
            }

            public bool TryResolveFbsAlias(TypeModelContainer container, string alias, out ITypeModel typeModel)
            {
                typeModel = null;
                return false;
            }
        }

        private class ExtensionTypeModel<T, TUnderlying> : ITypeModel<T>
        {
            private readonly ITypeModel<TUnderlying> underlyingModel;
            private readonly MethodInfo toUnderlying;
            private readonly MethodInfo fromUnderlying;

            public ExtensionTypeModel(
                MethodInfo toUnderlying,
                MethodInfo fromUnderlying,
                ITypeModel<TUnderlying> underlyingModel)
            {
                this.toUnderlying = toUnderlying;
                this.fromUnderlying = fromUnderlying;
                this.underlyingModel = underlyingModel;
            }

            public FlatBufferSchemaType SchemaType => this.underlyingModel.SchemaType;

            public Type ClrType => typeof(T);

            public ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingModel.PhysicalLayout;

            public bool IsFixedSize => this.underlyingModel.IsFixedSize;

            public bool IsValidStructMember => this.underlyingModel.IsValidStructMember;

            public bool IsValidTableMember => this.underlyingModel.IsValidTableMember;

            public bool IsValidVectorMember => this.underlyingModel.IsValidVectorMember;

            public bool IsValidUnionMember => this.underlyingModel.IsValidUnionMember;

            public bool IsValidSortedVectorKey => this.underlyingModel.IsValidSortedVectorKey;

            public int MaxInlineSize => this.underlyingModel.MaxInlineSize;

            public bool MustAlwaysSerialize => this.underlyingModel.MustAlwaysSerialize;

            public bool SerializesInline => this.underlyingModel.SerializesInline;

            public TableMemberModel AdjustTableMember(TableMemberModel source) => this.underlyingModel.AdjustTableMember(source);

            public CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
            {
                string baseTypeName = CSharpHelpers.GetCompilableTypeName(this.toUnderlying.DeclaringType);
                string methodName = this.toUnderlying.Name;

                return this.underlyingModel.CreateGetMaxSizeMethodBody(
                    context.With($"{baseTypeName}.{methodName}({context.ValueVariableName})"));
            }

            public CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
            {
                string baseTypeName = CSharpHelpers.GetCompilableTypeName(this.fromUnderlying.DeclaringType);
                string methodName = this.fromUnderlying.Name;

                // Parse buffer as underlying type name.
                string parseUnderlying = context.MethodNameMap[typeof(TUnderlying)];

                return new CodeGeneratedMethod
                {
                    MethodBody = $@"
                        return {baseTypeName}.{methodName}(
                            {parseUnderlying}(
                                {context.InputBufferVariableName},
                                {context.OffsetVariableName}));"
                };
            }

            public CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
            {
                string baseTypeName = CSharpHelpers.GetCompilableTypeName(this.toUnderlying.DeclaringType);
                string methodName = this.toUnderlying.Name;

                return this.underlyingModel.CreateSerializeMethodBody(
                    context.With(valueVariableName: $"{baseTypeName}.{methodName}({context.ValueVariableName})"));
            }

            public string GetNonNullConditionExpression(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(typeof(T)) != null;

                if (typeof(T).IsClass || isNullable)
                {
                    return $"{itemVariableName} != null";
                }

                return "true";
            }

            public string GetThrowIfNullInvocation(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(typeof(T)) != null;

                if (typeof(T).IsClass || isNullable)
                {
                    return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
                }

                return string.Empty;
            }

            public void Initialize()
            {
            }

            public void TraverseObjectGraph(HashSet<Type> seenTypes)
            {
                seenTypes.Add(typeof(T));
                seenTypes.Add(typeof(TUnderlying));
            }

            public bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
            {
                literal = null;
                return false;
            }

            public bool TryFormatStringAsLiteral(string value, out string literal)
            {
                literal = null;
                return false;
            }

            public bool TryGetSpanComparerType(out Type comparerType) => this.underlyingModel.TryGetSpanComparerType(out comparerType);

            public bool TryGetTableKeyMember(out TableMemberModel tableMember) => this.underlyingModel.TryGetTableKeyMember(out tableMember);

            public bool TryGetUnderlyingVectorType(out ITypeModel typeModel) => this.underlyingModel.TryGetUnderlyingVectorType(out typeModel);

            public bool ValidateDefaultValue(object defaultValue) => false;
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
