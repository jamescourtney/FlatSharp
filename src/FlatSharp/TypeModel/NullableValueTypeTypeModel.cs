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
    using System.Collections.Immutable;

    public class NullableValueTypeTypeModel : RuntimeTypeModel
    {
        private ITypeModel underlyingTypeModel;

        internal NullableValueTypeTypeModel(Type type, TypeModelContainer typeModelProvider) : base(type, typeModelProvider)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => this.underlyingTypeModel.SchemaType;

        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingTypeModel.PhysicalLayout;

        public override bool IsFixedSize => this.underlyingTypeModel.IsFixedSize;

        public override bool IsValidStructMember => false;

        public override bool IsValidTableMember => this.underlyingTypeModel.IsValidTableMember;

        public override bool IsValidVectorMember => false;

        public override bool IsValidUnionMember => false;

        public override bool IsValidSortedVectorKey => false;

        public override bool SerializesInline => this.underlyingTypeModel.SerializesInline;

        public override void Initialize()
        {
            base.Initialize();
            this.underlyingTypeModel = this.typeModelContainer.CreateTypeModel(Nullable.GetUnderlyingType(this.ClrType));
        }

        public override string GetIsEqualToDefaultValueExpression(string variableName, string defaultValue)
        {
            return $"{variableName}.HasValue == false";
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            Type underlyingType = this.underlyingTypeModel.ClrType;
            string underlyingTypeName = CSharpHelpers.GetCompilableTypeName(underlyingType);

            return new CodeGeneratedMethod
            {
                MethodBody = $"return {context.MethodNameMap[underlyingType]}(({underlyingTypeName}){context.ValueVariableName}.Value);",
                IsMethodInline = true,
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            Type underlyingType = this.underlyingTypeModel.ClrType;
            string typeName = CSharpHelpers.GetCompilableTypeName(this.ClrType);

            return new CodeGeneratedMethod
            {
                MethodBody = $"return ({typeName}){context.MethodNameMap[underlyingType]}({context.InputBufferVariableName}, {context.OffsetVariableName});",
                IsMethodInline = true,
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            Type underlyingType = this.underlyingTypeModel.ClrType;
            string underlyingTypeName = CSharpHelpers.GetCompilableTypeName(underlyingType);

            return new CodeGeneratedMethod
            {
                MethodBody = $"{context.MethodNameMap[underlyingType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, ({underlyingTypeName}){context.ValueVariableName}.Value, {context.OffsetVariableName}, {context.SerializationContextVariableName});",
                IsMethodInline = true,
            };
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            if (seenTypes.Add(this.underlyingTypeModel.ClrType))
            {
                this.underlyingTypeModel.TraverseObjectGraph(seenTypes);
            }
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            return $"{itemVariableName}.HasValue";
        }
    }
}
