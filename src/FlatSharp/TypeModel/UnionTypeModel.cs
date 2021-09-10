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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Defines a vector type model.
    /// </summary>
    public class UnionTypeModel : RuntimeTypeModel
    {
        private ITypeModel[] memberTypeModels;

        internal UnionTypeModel(Type unionType, TypeModelContainer provider) : base(unionType, provider)
        {
            this.memberTypeModels = null!;
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Union;

        /// <summary>
        /// Unions are "double-wide" vtable items.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => 
            new[]
            {
                new PhysicalLayoutElement(sizeof(byte), sizeof(byte)),
                new PhysicalLayoutElement(sizeof(uint), sizeof(uint))
            }.ToImmutableArray();

        /// <summary>
        /// Unions are not fixed because they contain tables.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Unions can't be part of structs.
        /// </summary>
        public override bool IsValidStructMember => false;

        /// <summary>
        /// Unions can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Unions can't be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => false;

        /// <summary>
        /// Unions can't be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => false;

        /// <summary>
        /// Unions can't be keys of sorted vectors.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Unions are pointers.
        /// </summary>
        public override bool SerializesInline => false;

        /// <summary>
        /// Gets the type model for this union's members. Index 0 corresponds to discriminator 1.
        /// </summary>
        public ITypeModel[] UnionElementTypeModel => this.memberTypeModels;

        /// <summary>
        /// We need it to pass through.
        /// </summary>
        public override TableFieldContextRequirements TableFieldContextRequirements => 
            this.memberTypeModels.Select(x => x.TableFieldContextRequirements).Aggregate(TableFieldContextRequirements.None, (a, b) => a | b);

        /// <summary>
        /// Unions have an implicit dependency on <see cref="byte"/> for the discriminator.
        /// </summary>
        public override IEnumerable<ITypeModel> Children => this.memberTypeModels.Concat(new[] { this.typeModelContainer.CreateTypeModel(typeof(byte)) });

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            List<string> switchCases = new List<string>();
            for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
            {
                var unionMember = this.UnionElementTypeModel[i];
                int unionIndex = i + 1;

                var itemContext = context with
                {
                    ValueVariableName = $"{context.ValueVariableName}.Item{unionIndex}",
                };

                string @case =
$@"
                    case {unionIndex}:
                        return {sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint))} + {itemContext.GetMaxSizeInvocation(unionMember.ClrType)};";

                switchCases.Add(@case);
            }
            string discriminatorPropertyName = nameof(FlatBufferUnion<int, int>.Discriminator);

            string body =
$@"
            switch ({context.ValueVariableName}.{discriminatorPropertyName})
            {{
                {string.Join("\r\n", switchCases)}
                default:
                    throw new System.InvalidOperationException(""Exception determining type of union. Discriminator = "" + {context.ValueVariableName}.{discriminatorPropertyName});
            }}
";
            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            List<string> switchCases = new List<string>();
            for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
            {
                var unionMember = this.UnionElementTypeModel[i];
                int unionIndex = i + 1;

                string inlineAdjustment = string.Empty;
                if (unionMember.SerializesInline)
                {
                    inlineAdjustment = $"offsetLocation += buffer.{nameof(InputBufferExtensions.ReadUOffset)}(offsetLocation);";
                }

                var itemContext = context with
                {
                    OffsetVariableName = "offsetLocation",
                    IsOffsetByRef = false,
                };

                string @case =
$@"
                    case {unionIndex}:
                        {inlineAdjustment}
                        return new {this.GetGlobalCompilableTypeName()}({itemContext.GetParseInvocation(unionMember.ClrType)});
";
                switchCases.Add(@case);
            }

            string body = $@"
                byte discriminator = {context.InputBufferVariableName}.{nameof(IInputBuffer.ReadByte)}({context.OffsetVariableName}.offset0);
                int offsetLocation = {context.OffsetVariableName}.offset1;
                if (discriminator != 0 && offsetLocation == 0)
                    throw new System.IO.InvalidDataException(""FlatBuffer union had discriminator set but no offset."");

                switch (discriminator)
                {{
                    {string.Join("\r\n", switchCases)}
                    default:
                        throw new System.InvalidOperationException(""Exception parsing union '{this.GetCompilableTypeName()}'. Discriminator = "" + discriminator);
                }}
            ";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            List<string> switchCases = new List<string>();
            for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
            {
                var elementModel = this.UnionElementTypeModel[i];
                var unionIndex = i + 1;

                string inlineAdjustment;

                if (elementModel.SerializesInline)
                {
                    // Structs are generally written in-line, with the exception of unions.
                    // So, we need to do the normal allocate space dance here, since we're writing
                    // a pointer to a struct.
                    inlineAdjustment =
$@"
                        var writeOffset = context.{nameof(SerializationContext.AllocateSpace)}({elementModel.PhysicalLayout.Single().InlineSize}, {elementModel.PhysicalLayout.Single().Alignment});
                        {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}(span, {context.OffsetVariableName}.offset1, writeOffset);";
                }
                else
                {
                    inlineAdjustment = $"var writeOffset = {context.OffsetVariableName}.offset1;";
                }

                var caseContext = context with
                {
                    ValueVariableName = $"{context.ValueVariableName}.Item{unionIndex}",
                    OffsetVariableName = "writeOffset",
                    IsOffsetByRef = false,
                };

                string @case =
$@"
                    case {unionIndex}:
                    {{
                        {inlineAdjustment}
                        {caseContext.GetSerializeInvocation(elementModel.ClrType)};
                    }}
                        break;";

                switchCases.Add(@case);
            }

            string serializeBlock = $@"
                byte discriminatorValue = {context.ValueVariableName}.Discriminator;
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteByte)}(
                    {context.SpanVariableName}, 
                    discriminatorValue, 
                    {context.OffsetVariableName}.offset0);

                switch (discriminatorValue)
                {{
                    {string.Join("\r\n", switchCases)}
                    default: throw new InvalidOperationException(""Unexpected discriminator. Unions must be initialized."");
                }}";

            return new CodeGeneratedMethod(serializeBlock);
        }

        public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
        {
            List<string> switchCases = new List<string>();

            for (int i = 0; i < this.memberTypeModels.Length; ++i)
            {
                int discriminator = i + 1;
                string cloneMethod = context.MethodNameMap[this.memberTypeModels[i].ClrType];
                switchCases.Add($"{discriminator} => new {this.GetGlobalCompilableTypeName()}({cloneMethod}({context.ItemVariableName}.Item{discriminator})),");
            }

            switchCases.Add("_ => throw new InvalidOperationException(\"Unexpected union discriminator\")");

            string body = $@"
                return {context.ItemVariableName}.{nameof(IFlatBufferUnion.Discriminator)} switch {{
                    {string.Join("\r\n", switchCases)}
                }};";

            return new CodeGeneratedMethod(body);
        }

        public override void Initialize()
        {
            HashSet<Type> uniqueTypes = new HashSet<Type>();

            // Look for the actual FlatBufferUnion.
            Type unionType = this.ClrType.GetInterfaces()
                .Single(x => x != typeof(IFlatBufferUnion) && typeof(IFlatBufferUnion).IsAssignableFrom(x));

            this.memberTypeModels = unionType.GetGenericArguments().Select(this.typeModelContainer.CreateTypeModel).ToArray();

            foreach (var item in this.memberTypeModels)
            {
                if (!item.IsValidUnionMember)
                {
                    throw new InvalidFlatBufferDefinitionException($"Unions may not store '{item.GetCompilableTypeName()}'.");
                }
                else if (!uniqueTypes.Add(item.ClrType))
                {
                    throw new InvalidFlatBufferDefinitionException($"Unions must consist of unique types. The type '{item.GetCompilableTypeName()}' was repeated.");
                }
            }
        }
    }
}
