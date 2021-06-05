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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines a scalar FlatSharp type model.
    /// </summary>
    public abstract class ScalarTypeModel : RuntimeTypeModel
    {
        private readonly int size;

        internal ScalarTypeModel(
            TypeModelContainer container,
            Type type,
            int size) : base(type, container)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Scalar;

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => new PhysicalLayoutElement[] { new PhysicalLayoutElement(this.size, this.size) }.ToImmutableArray();

        /// <summary>
        /// Scalars are fixed size.
        /// </summary>
        public override bool IsFixedSize => true;

        /// <summary>
        /// Scalars can be part of Structs.
        /// </summary>
        public override bool IsValidStructMember => true;

        /// <summary>
        /// Scalars can be part of Tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Scalars can't be part of Unions.
        /// </summary>
        public override bool IsValidUnionMember => false;

        /// <summary>
        /// Scalars can be part of Vectors.
        /// </summary>
        public override bool IsValidVectorMember => true;

        /// <summary>
        /// Scalars can be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => true;

        /// <summary>
        /// Scalars are written inline.
        /// </summary>
        public override bool SerializesInline => true;

        /// <summary>
        /// We don't care about serialization context.
        /// </summary>
        public override bool SerializeMethodRequiresContext => false;

        /// <summary>
        /// Scalars are leaf nodes.
        /// </summary>
        public override IEnumerable<ITypeModel> Children => new ITypeModel[0];

        /// <summary>
        /// The name of the read method for an input buffer.
        /// </summary>
        protected abstract string InputBufferReadMethodName { get; }

        /// <summary>
        /// The name of a write method for an input buffer.
        /// </summary>
        protected abstract string SpanWriterWriteMethodName { get; }

        /// <summary>
        /// Force children to reimplement.
        /// </summary>
        public abstract override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType);

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            return defaultValue.GetType() == this.ClrType;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            return new CodeGeneratedMethod($"return {this.MaxInlineSize};")
            {
                IsMethodInline = true,
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            return new CodeGeneratedMethod($"return {context.InputBufferVariableName}.{this.InputBufferReadMethodName}({context.OffsetVariableName});")
            {
                IsMethodInline = true,
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            string variableName = context.ValueVariableName;
            return new CodeGeneratedMethod($"{context.SpanWriterVariableName}.{this.SpanWriterWriteMethodName}({context.SpanVariableName}, {variableName}, {context.OffsetVariableName});")
            {
                IsMethodInline = true,
            };
        }

        public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
        {
            return new CodeGeneratedMethod($"return {context.ItemVariableName};")
            {
                IsMethodInline = true,
            };
        }

        public override string FormatDefaultValueAsLiteral(object? defaultValue)
        {
            if (defaultValue is not null)
            {
                return $"({this.GetCompilableTypeName()})({defaultValue})";
            }

            return base.FormatDefaultValueAsLiteral(defaultValue);
        }
    }
}
