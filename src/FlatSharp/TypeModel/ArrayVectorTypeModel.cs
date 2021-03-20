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

    /// <summary>
    /// Defines a vector type model for an array vector.
    /// </summary>
    public class ArrayVectorTypeModel : BaseVectorTypeModel
    {
        private ITypeModel itemTypeModel;

        internal ArrayVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
            this.itemTypeModel = null!;
        }

        public override ITypeModel ItemTypeModel => this.itemTypeModel;

        public override string LengthPropertyName => nameof(Array.Length);

        public override void OnInitialize()
        {
            if (!this.ClrType.IsArray)
            {
                throw new InvalidFlatBufferDefinitionException($"Array vectors must contain be arrays. Type = {this.ClrType.FullName}.");
            }

            if (this.ClrType.GetArrayRank() != 1)
            {
                throw new InvalidFlatBufferDefinitionException($"Array vectors may only be single-dimensional.");
            }

            this.itemTypeModel = this.typeModelContainer.CreateTypeModel(this.ClrType.GetElementType()!);
            if (!this.itemTypeModel.IsValidVectorMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Type '{this.itemTypeModel.GetCompilableTypeName()}' is not a valid vector member.");
            }
        }

        protected override CodeGeneratedMethod CreateGetMaxSizeBodyWithLoop(GetMaxSizeCodeGenContext context)
        {
            var itemContext = context.With(valueVariableName: "itemTemp");
            string body = $@"
                int runningSum = {VectorMinSize + this.MaxInlineSize};
                foreach (var itemTemp in {context.ValueVariableName})
                {{
                    {this.GetThrowIfNullStatement("itemTemp")}
                    runningSum += {itemContext.GetMaxSizeInvocation(this.ItemTypeModel.ClrType)};
                }}
                return runningSum;";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var type = this.ClrType;
            var itemTypeModel = this.ItemTypeModel;

            string body = $@"
                int count = {context.ValueVariableName}.{this.LengthPropertyName};
                int vectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}({itemTypeModel.PhysicalLayout[0].Alignment}, count, {this.PaddedMemberInlineSize});
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, vectorOffset, {context.SerializationContextVariableName});
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, vectorOffset, {context.SerializationContextVariableName});
                vectorOffset += sizeof(int);
                foreach (var current in {context.ValueVariableName})
                {{
                      {this.GetThrowIfNullStatement("current")}
                      {context.MethodNameMap[itemTypeModel.ClrType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, current, vectorOffset, {context.SerializationContextVariableName});
                      vectorOffset += {this.PaddedMemberInlineSize};
                }}";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            string body;

            if (this.itemTypeModel is null)
            {
                throw new InvalidOperationException($"Flatsharp internal error: ItemTypeModel null");
            }

            (string vectorClassDef, string vectorClassName) = (string.Empty, string.Empty);

            if (this.itemTypeModel.ClrType == typeof(byte))
            {
                // can handle this as memory.
                string method = nameof(InputBufferExtensions.ReadByteMemoryBlock);
                string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";
                body = $"return {memoryVectorRead}.ToArray();";
            }
            else
            {
                (vectorClassDef, vectorClassName) = FlatBufferVectorHelpers.CreateFlatBufferVectorSubclass(
                    this.itemTypeModel.ClrType,
                    context.InputBufferTypeName,
                    context.MethodNameMap[this.itemTypeModel.ClrType]);

                string createFlatBufferVector =
                $@"new {vectorClassName}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}), 
                    {this.PaddedMemberInlineSize})";

                body = $"return ({createFlatBufferVector}).ToArray();";
            }

            return new CodeGeneratedMethod(body) { ClassDefinition = vectorClassDef };
        }
    }
}
