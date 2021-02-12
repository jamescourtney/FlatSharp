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
    using System.Linq.Expressions;
    using System.Reflection;

    internal class FacadeTypeModelProvider<TConverter, TUnderlying, TType> : ITypeModelProvider
        where TConverter : struct, IFacadeTypeConverter<TUnderlying, TType>
    {
        private readonly ITypeModel model;

        public FacadeTypeModelProvider(
            ITypeModel underlyingModel)
        {
            this.model = new FacadeTypeModel(underlyingModel);
        }

        public bool TryCreateTypeModel(TypeModelContainer container, Type type, out ITypeModel typeModel)
        {
            if (type == typeof(TType))
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

        private class FacadeTypeModel : ITypeModel
        {
            private readonly ITypeModel underlyingModel;

            public FacadeTypeModel(
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

            public bool MustAlwaysSerialize => this.underlyingModel.MustAlwaysSerialize;

            public bool SerializesInline => this.underlyingModel.SerializesInline;

            public TableMemberModel AdjustTableMember(TableMemberModel source) => this.underlyingModel.AdjustTableMember(source);

            public CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
            {
                return this.underlyingModel.CreateGetMaxSizeMethodBody(
                    context.With(GetConvertInvocation(context.ValueVariableName)));
            }

            public CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
            {
                // Parse buffer as underlying type name.
                string parseUnderlying = context.MethodNameMap[typeof(TUnderlying)];

                string parseInvocation = 
                    $"{parseUnderlying}({context.InputBufferVariableName}, {context.OffsetVariableName})";

                return new CodeGeneratedMethod
                {
                    MethodBody = $"return {GetConvertInvocation(parseInvocation)};"
                };
            }

            public CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
            {
                return this.underlyingModel.CreateSerializeMethodBody(
                    context.With(valueVariableName: GetConvertInvocation(context.ValueVariableName)));
            }

            public string GetNonNullConditionExpression(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(typeof(TType)) != null;

                if (typeof(TType).IsClass || isNullable)
                {
                    return $"{itemVariableName} != null";
                }

                return "true";
            }

            public string GetThrowIfNullInvocation(string itemVariableName)
            {
                bool isNullable = Nullable.GetUnderlyingType(typeof(TType)) != null;

                if (typeof(TType).IsClass || isNullable)
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
                seenTypes.Add(typeof(TType));
                seenTypes.Add(typeof(TUnderlying));

                this.underlyingModel.TraverseObjectGraph(seenTypes);
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

            public IEnumerable<Type> GetReferencedTypes() => new[] { typeof(TConverter), typeof(TType) };

            private static string GetConvertInvocation(string source)
            {
                string typeName = CSharpHelpers.GetCompilableTypeName(typeof(TConverter));
                return $"default({typeName}).{nameof(IFacadeTypeConverter<byte, byte>.ConvertFromUnderlyingType)}({source})";
            }
        }
    }
}
