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
    using System.Text;

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
            FlatSharpInternal.Assert(this.ClrType.IsArray, $"Array vectors must be arrays. Type = {this.ClrType.FullName}.");
            FlatSharpInternal.Assert(this.ClrType.GetArrayRank() == 1, "Array vectors may only be single-dimension.");

            this.itemTypeModel = this.typeModelContainer.CreateTypeModel(this.ClrType.GetElementType()!);
            if (!this.itemTypeModel.IsValidVectorMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Type '{this.itemTypeModel.GetCompilableTypeName()}' is not a valid vector member.");
            }
        }

        protected override string CreateLoop(
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string numberofItemsVariableName,
            string expectedVariableName,
            string body) => CreateLoopStatic(options, vectorVariableName, expectedVariableName, body);

        internal static string CreateLoopStatic(
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string expectedVariableName,
            string body)
        {
            return $@"
                for (int i = 0; i < {vectorVariableName}.Length; i = unchecked(i + 1))
                {{
                    var {expectedVariableName} = {vectorVariableName}[i];
                    {body}
                }}";
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
