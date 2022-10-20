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

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a vector type model for an array vector.
/// </summary>
public class ArrayVectorTypeModel : BaseVectorTypeModel
{
    internal ArrayVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
    {
    }

    public override string LengthPropertyName => nameof(Array.Length);

    public override Type OnInitialize()
    {
        FlatSharpInternal.Assert(this.ClrType.IsArray, $"Array vectors must be arrays. Type = {this.ClrType.FullName}.");
        FlatSharpInternal.Assert(this.ClrType.GetArrayRank() == 1, "Array vectors may only be single-dimension.");
        return this.ClrType.GetElementType()!;
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
        FlatSharpInternal.Assert(this.ItemTypeModel is not null, "Flatsharp internal error: ItemTypeModel null");

        bool isEverWriteThrough = ValidateWriteThrough(
            writeThroughSupported: false,
            this,
            context.AllFieldContexts,
            context.Options);

        (string vectorClassDef, string vectorClassName) = (string.Empty, string.Empty);

        if (this.ItemTypeModel.ClrType == typeof(byte))
        {
            // can handle this as memory.
            string method = nameof(InputBufferExtensions.ReadByteReadOnlyMemoryBlock);
            string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";
            body = $"return {memoryVectorRead}.ToArray();";
        }
        else
        {
            FlatSharpInternal.Assert(!string.IsNullOrEmpty(context.TableFieldContextVariableName), "expecting table field context");

            (vectorClassDef, vectorClassName) = FlatBufferVectorHelpers.CreateVectorItemAccessor(
                this.ItemTypeModel,
                this.PaddedMemberInlineSize,
                context,
                isEverWriteThrough);

            string accessorClassName = $"{vectorClassName}<{context.InputBufferTypeName}>";
            string actualTypeName = this.ItemTypeModel.GetDeserializedTypeName(context.InputBufferTypeName, context.Options.DeserializationOption, context.MethodNameResolver);

            string createFlatBufferVector =
                $@"new FlatBufferVectorBase<{this.ItemTypeModel.GetGlobalCompilableTypeName()}, {context.InputBufferTypeName}, {accessorClassName}, {actualTypeName}> (
                    {context.InputBufferVariableName}, 
                    new {accessorClassName}(
                        {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}),
                        {context.InputBufferVariableName}),
                    {context.RemainingDepthVariableName},
                    {context.TableFieldContextVariableName})";

            body = $"return ({createFlatBufferVector}).ToArray();";
        }

        return new CodeGeneratedMethod(body) { ClassDefinition = vectorClassDef };
    }
}
