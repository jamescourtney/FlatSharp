/*
 * Copyright 2021 James Courtney
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
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A type model provider for a Type Facade.
    /// </summary>
    internal class TypeFacadeTypeModelProvider<TConverter, TUnderlying, TType> : ITypeModelProvider
        where TConverter : struct, ITypeFacadeConverter<TUnderlying, TType>
    {
        private readonly ITypeModel model;

        public TypeFacadeTypeModelProvider(
            ITypeModel underlyingModel)
        {
            this.model = new TypeFacadeTypeModel(underlyingModel);
        }

        public bool TryCreateTypeModel(TypeModelContainer container, Type type, [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            if (type == typeof(TType))
            {
                typeModel = this.model;
                return true;
            }

            typeModel = null;
            return false;
        }

        public bool TryResolveFbsAlias(TypeModelContainer container, string alias, [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            typeModel = null;
            return false;
        }

        private class TypeFacadeTypeModel : ITypeModel
        {
            private readonly ITypeModel underlyingModel;

            public TypeFacadeTypeModel(
                ITypeModel underlyingModel)
            {
                this.underlyingModel = underlyingModel;
            }

            public FlatBufferSchemaType SchemaType => this.underlyingModel.SchemaType;

            public Type ClrType => typeof(TType);

            public ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingModel.PhysicalLayout;

            public bool IsFixedSize => this.underlyingModel.IsFixedSize;

            public bool IsValidStructMember => this.underlyingModel.IsValidStructMember;

            public bool IsValidTableMember => this.underlyingModel.IsValidTableMember;

            public bool IsValidVectorMember => this.underlyingModel.IsValidVectorMember;

            public bool IsValidUnionMember => this.underlyingModel.IsValidUnionMember;

            public bool IsValidSortedVectorKey => this.underlyingModel.IsValidSortedVectorKey;

            public int MaxInlineSize => this.underlyingModel.MaxInlineSize;

            public bool SerializesInline => this.underlyingModel.SerializesInline;

            public TableMemberModel AdjustTableMember(TableMemberModel source) => this.underlyingModel.AdjustTableMember(source);

            public CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
            {
                var method = this.underlyingModel.CreateGetMaxSizeMethodBody(
                    context.With(GetConvertToUnderlyingInvocation(context.ValueVariableName)));

                method.IsMethodInline = true;
                return method;
            }

            public CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
            {
                // Parse buffer as underlying type name.
                string parseUnderlying = context.GetParseInvocation(typeof(TUnderlying));
                string body = $"return {GetConvertFromUnderlyingInvocation(parseUnderlying)};";
                return new CodeGeneratedMethod(body)
                {
                    IsMethodInline = true,
                };
            }

            public CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
            {
                string invocation = context
                    .With(valueVariableName: GetConvertToUnderlyingInvocation(context.ValueVariableName))
                    .GetSerializeInvocation(typeof(TUnderlying));

                return new CodeGeneratedMethod($"{invocation};")
                {
                    IsMethodInline = true,
                };
            }

            public CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
            {
                string toUnderlying = GetConvertToUnderlyingInvocation(context.ItemVariableName);
                string clone = context.MethodNameMap[this.underlyingModel.ClrType];
                string fromUnderlying = GetConvertFromUnderlyingInvocation($"{clone}({toUnderlying})");

                return new CodeGeneratedMethod($"return {fromUnderlying};")
                {
                    IsMethodInline = true,
                };
            }

            public string GetNonNullConditionExpression(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(this.ClrType) is not null;

                if (this.ClrType.IsClass || isNullable)
                {
                    return $"{itemVariableName} is not null";
                }

                return "true";
            }

            public string GetThrowIfNullInvocation(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(this.ClrType) is not null;

                if (this.ClrType.IsClass || isNullable)
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
                seenTypes.Add(this.ClrType);
                seenTypes.Add(typeof(TUnderlying));

                this.underlyingModel.TraverseObjectGraph(seenTypes);
            }

            public string GetNotEqualToDefaultValueExpression(string itemVariableName, object? defaultValue)
            {
                if (this.ClrType.IsValueType && Nullable.GetUnderlyingType(this.ClrType) is null)
                {
                    // Structs are really weird.
                    // Value types are not guaranteed to have an equality operator defined, so we can't just assume
                    // that '==' will work, so we do a little reflection, and use the operator if it is defined.
                    // otherwise, we assume that the operands are not equal no matter what.

                    var equalityMethod = this.ClrType.GetMethod("op_Inequality", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (equalityMethod is not null)
                    {
                        // Let's hope it's rational.
                        return $"{itemVariableName} != default({CSharpHelpers.GetCompilableTypeName(this.ClrType)})";
                    }
                    else
                    {
                        // structs without an equality overload have to be assumed to be not equal.
                        return "true";
                    }
                }
                else
                {
                    // Nullable<T> and reference types are easy.
                    return $"{itemVariableName} is not null";
                }
            }

            public bool TryFormatDefaultValueAsLiteral(object? defaultValue, [NotNullWhen(true)] out string? literal)
            {
                literal = $"default({CSharpHelpers.GetCompilableTypeName(this.ClrType)})";
                return true;
            }

            public bool TryFormatStringAsLiteral(string value, [NotNullWhen(true)] out string? literal)
            {
                literal = null;
                return false;
            }

            public bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType) 
                => this.underlyingModel.TryGetSpanComparerType(out comparerType);

            public bool TryGetTableKeyMember([NotNullWhen(true)] out TableMemberModel? tableMember) 
                => this.underlyingModel.TryGetTableKeyMember(out tableMember);

            public bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel) 
                => this.underlyingModel.TryGetUnderlyingVectorType(out typeModel);

            public bool ValidateDefaultValue(object? defaultValue) => false;

            public IEnumerable<Type> GetReferencedTypes() => new[] { typeof(TConverter), this.ClrType };

            public bool TryGetUnderlyingUnionTypes([NotNullWhen(true)] out ITypeModel[]? typeModels) 
                => this.underlyingModel.TryGetUnderlyingUnionTypes(out typeModels);

            private static string GetConvertToUnderlyingInvocation(string source)
            {
                string typeName = CSharpHelpers.GetCompilableTypeName(typeof(TConverter));
                return $"default({typeName}).{nameof(ITypeFacadeConverter<byte, byte>.ConvertToUnderlyingType)}({source})";
            }

            private static string GetConvertFromUnderlyingInvocation(string source)
            {
                string typeName = CSharpHelpers.GetCompilableTypeName(typeof(TConverter));
                return $"default({typeName}).{nameof(ITypeFacadeConverter<byte, byte>.ConvertFromUnderlyingType)}({source})";
            }
        }
    }
}
