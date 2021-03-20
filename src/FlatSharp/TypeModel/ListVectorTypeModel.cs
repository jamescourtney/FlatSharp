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
    /// Defines a vector type model for a list vector.
    /// </summary>
    public class ListVectorTypeModel : BaseVectorTypeModel
    {
        private ITypeModel itemTypeModel;
        private bool isReadOnly;

        internal ListVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
            this.itemTypeModel = null!;
        }

        public override ITypeModel ItemTypeModel => this.itemTypeModel;

        public override string LengthPropertyName => nameof(IList<byte>.Count);

        public override void OnInitialize()
        {
            var genericDef = this.ClrType.GetGenericTypeDefinition();
            if (genericDef != typeof(IList<>) && genericDef != typeof(IReadOnlyList<>))
            {
                throw new InvalidFlatBufferDefinitionException($"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, ReadOnlyMemory, and Arrays are supported.");
            }

            this.isReadOnly = genericDef == typeof(IReadOnlyList<>);
            this.itemTypeModel = this.typeModelContainer.CreateTypeModel(this.ClrType.GetGenericArguments()[0]);

            if (!this.itemTypeModel.IsValidVectorMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Type '{this.itemTypeModel.GetCompilableTypeName()}' is not a valid vector member.");
            }
        }

        protected override CodeGeneratedMethod CreateGetMaxSizeBodyWithLoop(GetMaxSizeCodeGenContext context)
        {
            var itemContext = context.With(valueVariableName: "itemTemp");

            string TypedLoop(string var, string? count = null) => $@"
                    { (count == null ? $"int count = {context.ValueVariableName}.{LengthPropertyName};" : "") }
                    for (int i = 0; i < {count ?? "count"}; ++i)
                    {{
                        var itemTemp = {context.ValueVariableName}[i];
                        {this.GetThrowIfNullStatement("itemTemp")}
                        runningSum += {itemContext.GetMaxSizeInvocation(this.ItemTypeModel.ClrType)};
                    }}";

            string body = $@"
                int runningSum = {VectorMinSize + this.MaxInlineSize};
                if ({context.ValueVariableName} is {itemTypeModel.GetCompilableTypeName()}[] array)
                {{
                    {TypedLoop("array", "array.Length")}
                }}
                else if ({context.ValueVariableName} is List<{itemTypeModel.GetCompilableTypeName()}> list)
                {{
                    {TypedLoop("list")}
                }}
                else
                {{
                    {TypedLoop(context.ValueVariableName)}
                }}

                return runningSum;";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var type = this.ClrType;
            var itemTypeModel = this.ItemTypeModel;

            string TypedLoop(string var, string? count = null) => $@"
                    for (int i = 0; i < {count ?? "count"}; ++i)
                    {{
                        var current = {var}[i];
                        {this.GetThrowIfNullStatement("current")}
                        {context.MethodNameMap[itemTypeModel.ClrType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, current, vectorOffset, {context.SerializationContextVariableName});
                        vectorOffset += {this.PaddedMemberInlineSize};
                    }}";

            string body = $@"
                int count = {context.ValueVariableName}.{this.LengthPropertyName};
                int vectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}({itemTypeModel.PhysicalLayout[0].Alignment}, count, {this.PaddedMemberInlineSize});
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, vectorOffset, {context.SerializationContextVariableName});
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, vectorOffset, {context.SerializationContextVariableName});
                vectorOffset += sizeof(int);
                
                if ({context.ValueVariableName} is {itemTypeModel.GetCompilableTypeName()}[] array)
                {{
                    {TypedLoop("array", "array.Length")}
                }}
                else if ({context.ValueVariableName} is List<{itemTypeModel.GetCompilableTypeName()}> list)
                {{
                    {TypedLoop("list")}
                }}
                else
                {{
                    {TypedLoop(context.ValueVariableName)}
                }}";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            (string vectorClassDef, string vectorClassName) = FlatBufferVectorHelpers.CreateFlatBufferVectorSubclass(
                this.itemTypeModel.ClrType,
                context.InputBufferTypeName,
                context.MethodNameMap[this.itemTypeModel.ClrType]);

            string body;

            string createFlatBufferVector =
            $@"new {vectorClassName}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}), 
                    {this.PaddedMemberInlineSize})";

            if (context.Options.PreallocateVectors)
            {
                // We just call .ToList(). Note that when full greedy mode is on, these items will be 
                // greedily initialized as we traverse the list. Otherwise, they'll be allocated lazily.
                body = $"({createFlatBufferVector}).FlatBufferVectorToList()";

                if (!context.Options.GenerateMutableObjects)
                {
                    // Finally, if we're not in the business of making mutable objects, then convert the list to read only.
                    body += ".AsReadOnly()";
                }

                body = $"return {body};";
            }
            else
            {
                body = $"return {createFlatBufferVector};";
            }

            return new CodeGeneratedMethod(body) { ClassDefinition = vectorClassDef };
        }
    }
}
