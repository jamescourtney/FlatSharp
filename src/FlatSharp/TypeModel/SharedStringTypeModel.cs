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

    /// <summary>
    /// Defines a FlatBuffer string type.
    /// </summary>
    public class SharedStringTypeModel : RuntimeTypeModel, ITypeModel
    {
        internal SharedStringTypeModel() : base(typeof(SharedString), null)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.String;

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => new PhysicalLayoutElement[] { new PhysicalLayoutElement(sizeof(uint), sizeof(uint)) }.ToImmutableArray();

        /// <summary>
        /// Strings are arbitrary in length.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Strings can't be part of structs.
        /// </summary>
        public override bool IsValidStructMember => false;

        /// <summary>
        /// Strings can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Strings can be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => true;

        /// <summary>
        /// Strings can be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => true;

        /// <summary>
        /// Strings can be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => true;

        /// <summary>
        /// Strings are written by reference.
        /// </summary>
        public override bool SerializesInline => false;

        /// <summary>
        /// Gets the type of span comparer for this type.
        /// </summary>
        public Type SpanComparerType => typeof(StringSpanComparer);

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                MethodBody = $"return {nameof(SerializationHelpers)}.{nameof(SerializationHelpers.GetMaxSize)}({context.ValueVariableName});",
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                MethodBody = $"return {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadSharedString)}({context.OffsetVariableName});",
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                MethodBody = $"{context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteSharedString)}({context.SpanVariableName}, {context.ValueVariableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});",
            };
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(StringSpanComparer);
            return true;
        }
    }
}