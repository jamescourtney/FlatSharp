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
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Reflection;

    /// <summary>
    /// Defines an Enum FlatSharp type model, which derives from the scalar type model.
    /// </summary>
    public class EnumTypeModel : RuntimeTypeModel
    {
        private ITypeModel underlyingTypeModel;

        internal EnumTypeModel(Type type, TypeModelContainer typeModelContainer) : base(type, typeModelContainer)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Type enumType = this.ClrType;
            Type underlyingType = Enum.GetUnderlyingType(enumType);

            this.underlyingTypeModel = this.typeModelContainer.CreateTypeModel(underlyingType);

            var attribute = enumType.GetCustomAttribute<FlatBufferEnumAttribute>();
            if (attribute == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Enums '{enumType.Name}' is not tagged with a [FlatBufferEnum] attribute.");
            }

            if (attribute.DeclaredUnderlyingType != Enum.GetUnderlyingType(enumType))
            {
                throw new InvalidFlatBufferDefinitionException($"Enum '{enumType.Name}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{Enum.GetUnderlyingType(enumType)}'");
            }
        }

        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Scalar;

        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingTypeModel.PhysicalLayout;

        public override bool IsFixedSize => this.underlyingTypeModel.IsFixedSize;

        public override bool IsValidStructMember => true;

        public override bool IsValidTableMember => true;

        public override bool IsValidVectorMember => true;

        public override bool IsValidUnionMember => false;

        public override bool IsValidSortedVectorKey => false;

        public override bool SerializesInline => true;

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            Type underlyingType = this.underlyingTypeModel.ClrType;
            string underlyingTypeName = CSharpHelpers.GetCompilableTypeName(underlyingType);

            return new CodeGeneratedMethod 
            { 
                MethodBody = $"return {context.MethodNameMap[underlyingType]}(({underlyingTypeName}){context.ValueVariableName});",
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
                MethodBody = $"{context.MethodNameMap[underlyingType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, ({underlyingTypeName}){context.ValueVariableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});",
                IsMethodInline = true,
            };
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            seenTypes.Add(Enum.GetUnderlyingType(this.ClrType));
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            // Enums are value types.
            return string.Empty;
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            // Enums are value types.
            return "true";
        }

        public override bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
        {
            if (this.underlyingTypeModel.TryFormatDefaultValueAsLiteral(Convert.ChangeType(defaultValue, this.underlyingTypeModel.ClrType), out literal))
            {
                literal = $"({CSharpHelpers.GetCompilableTypeName(this.ClrType)}){literal}";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            return defaultValue.GetType() == this.ClrType;
        }
    }
}
