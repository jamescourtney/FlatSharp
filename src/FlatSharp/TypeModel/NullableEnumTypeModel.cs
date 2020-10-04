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
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Reflection;

    using FlatSharp.Attributes;

    /// <summary>
    /// Defines a Nullable Enum FlatSharp type model, which derives from the scalar type model.
    /// </summary>
    public class NullableEnumTypeModel : RuntimeTypeModel
    {
        private ITypeModel underlyingTypeModel;
        private Type enumType;

        internal NullableEnumTypeModel(Type type, TypeModelContainer typeModelProvider) : base(type, typeModelProvider)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Scalar;

        public override void Initialize()
        {
            base.Initialize();

            Type nullableType = this.ClrType;
            this.enumType = Nullable.GetUnderlyingType(nullableType);
            Type underlyingType = Enum.GetUnderlyingType(this.enumType);

            this.underlyingTypeModel = this.typeModelContainer.CreateTypeModel(underlyingType);

            var attribute = this.enumType.GetCustomAttribute<FlatBufferEnumAttribute>();
            if (attribute == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Enums '{enumType.Name}' is not tagged with a [FlatBufferEnum] attribute.");
            }

            if (attribute.DeclaredUnderlyingType != underlyingType)
            {
                throw new InvalidFlatBufferDefinitionException($"Enum '{this.enumType.Name}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{underlyingType}'");
            }
        }

        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingTypeModel.PhysicalLayout;

        public override bool IsFixedSize => this.underlyingTypeModel.IsFixedSize;

        public override bool IsValidStructMember => false;

        public override bool IsValidTableMember => true;

        public override bool IsValidVectorMember => false;

        public override bool IsValidUnionMember => false;

        public override bool IsValidSortedVectorKey => false;

        public override bool SerializesInline => true;

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
            seenTypes.Add(this.enumType);
            seenTypes.Add(this.underlyingTypeModel.ClrType);
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
