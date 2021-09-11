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
    using FlatSharp;
    using FlatSharp.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

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

            public bool IsValidSortedVectorKey => false;

            public int MaxInlineSize => this.underlyingModel.MaxInlineSize;

            public bool SerializesInline => this.underlyingModel.SerializesInline;

            public IEnumerable<ITypeModel> Children => new[] { this.underlyingModel };

            public ConstructorInfo? PreferredSubclassConstructor => this.underlyingModel.PreferredSubclassConstructor;

            public bool SerializeMethodRequiresContext => underlyingModel.SerializeMethodRequiresContext;

            public TableFieldContextRequirements TableFieldContextRequirements => underlyingModel.TableFieldContextRequirements;

            public void AdjustTableMember(TableMemberModel source) => this.underlyingModel.AdjustTableMember(source);

            public CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
            {
                context = context with
                {
                    ValueVariableName = GetConvertToUnderlyingInvocation(context.ValueVariableName)
                };

                return new CodeGeneratedMethod($"return {context.GetMaxSizeInvocation(this.underlyingModel.ClrType)};") { IsMethodInline = true };
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
                string invocation =
                    (context with { ValueVariableName = GetConvertToUnderlyingInvocation(context.ValueVariableName) })
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

            public string? CreateExtraClasses() => null;

            public void Initialize()
            {
            }

            public string FormatDefaultValueAsLiteral(object? defaultValue) => this.GetTypeDefaultExpression();

            public bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType) 
                => this.underlyingModel.TryGetSpanComparerType(out comparerType);

            public bool TryGetTableKeyMember([NotNullWhen(true)] out TableMemberModel? tableMember) 
                => this.underlyingModel.TryGetTableKeyMember(out tableMember);

            public bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel) 
                => this.underlyingModel.TryGetUnderlyingVectorType(out typeModel);

            public bool ValidateDefaultValue(object? defaultValue) => false;

            public IEnumerable<Type> GetReferencedTypes() => new[] { typeof(TConverter), this.ClrType };

            private static string GetConvertToUnderlyingInvocation(string source)
            {
                string typeName = CSharpHelpers.GetGlobalCompilableTypeName(typeof(TConverter));
                return $"default({typeName}).{nameof(ITypeFacadeConverter<byte, byte>.ConvertToUnderlyingType)}({source})";
            }

            private static string GetConvertFromUnderlyingInvocation(string source)
            {
                string typeName = CSharpHelpers.GetGlobalCompilableTypeName(typeof(TConverter));
                return $"default({typeName}).{nameof(ITypeFacadeConverter<byte, byte>.ConvertFromUnderlyingType)}({source})";
            }

            public List<(ITypeModel, TableFieldContext)> GetFieldContexts() => this.underlyingModel.GetFieldContexts();
        }
    }
}
