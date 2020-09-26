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

    /// <summary>
    /// Defines a scalar FlatSharp type model.
    /// </summary>
    public abstract class ScalarTypeModel : RuntimeTypeModel, ISpanComparerProvider
    {
        protected readonly bool isNullable;

        internal ScalarTypeModel(
            Type type,
            int size) : base(type, null)
        {
            this.VTableLayout = new[] { new VTableEntry(size, size) };
            this.isNullable = Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        public override VTableEntry[] VTableLayout { get; }

        /// <summary>
        /// Scalars are fixed size.
        /// </summary>
        public override bool IsFixedSize => true;

        /// <summary>
        /// Scalars can be part of Structs.
        /// </summary>
        public override bool IsValidStructMember => !this.isNullable;

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
        public override bool IsValidVectorMember => !this.isNullable;

        /// <summary>
        /// Scalars can be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => !this.isNullable;

        /// <summary>
        /// Scalars are written inline.
        /// </summary>
        public override bool SerializesInline => true;

        /// <summary>
        /// Gets the type of the span comparer for this scalar.
        /// </summary>
        public abstract Type SpanComparerType { get; }

        /// <summary>
        /// The name of the read method for an input buffer.
        /// </summary>
        protected abstract string InputBufferReadMethodName { get; }

        /// <summary>
        /// The name of a write method for an input buffer.
        /// </summary>
        protected abstract string SpanWriterWriteMethodName { get; }

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            if (this.isNullable)
            {
                return false;
            }

            return defaultValue.GetType() == this.ClrType;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                IsMethodInline = true,
                MethodBody = $"return {this.MaxInlineSize};"
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                IsMethodInline = true,
                MethodBody = $"return {context.InputBufferVariableName}.{this.InputBufferReadMethodName}({context.OffsetVariableName});"
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            string variableName = context.ValueVariableName;
            if (this.isNullable)
            {
                variableName += ".Value";
            }

            return new CodeGeneratedMethod 
            {
                MethodBody = $"{context.SpanWriterVariableName}.{this.SpanWriterWriteMethodName}({context.SpanVariableName}, {variableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});",
                IsMethodInline = true,
            };
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            if (this.isNullable)
            {
                return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
            }
            else
            {
                return string.Empty;
            }
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            if (this.isNullable)
            {
                return $"{itemVariableName}.HasValue";
            }
            else
            {
                return "true";
            }
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            if (this.isNullable)
            {
                seenTypes.Add(Nullable.GetUnderlyingType(this.ClrType));
            }
        }

        public override string FormatDefaultValueAsLiteral(object defaultValue)
        {
            return $"({CSharpHelpers.GetCompilableTypeName(this.ClrType)})({defaultValue})";
        }
    }
}
