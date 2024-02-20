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
/// Defines a vector type model for a memory vector of byte.
/// </summary>
public class MemoryVectorTypeModel : BaseVectorTypeModel
{
    private bool isReadOnly;

    internal MemoryVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
    {
    }

    public override string LengthPropertyName => nameof(Memory<byte>.Length);

    public override Type OnInitialize()
    {
        if (this.ClrType != typeof(Memory<byte>) && this.ClrType != typeof(ReadOnlyMemory<byte>))
        {
            throw new InvalidFlatBufferDefinitionException($"Memory vectors may only be ReadOnlyMemory<byte> or Memory<byte>.");
        }

        this.isReadOnly = this.ClrType == typeof(ReadOnlyMemory<byte>);
        return typeof(byte);
    }

    protected override string CreateLoop(FlatBufferSerializerOptions options, string vectorVariableName, string numberOfItemsVariableName, string expectedVariableName, string body)
    {
        FlatSharpInternal.Assert(false, "Not expecting to do loop get max size for memory vector");
        throw new Exception();
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        ValidateWriteThrough(
            writeThroughSupported: false,
            this,
            this.typeModelContainer,
            context.AllFieldContexts);

        string method = nameof(InputBufferExtensions.ReadByteMemoryBlock);
        if (this.isReadOnly)
        {
            method = nameof(InputBufferExtensions.ReadByteReadOnlyMemoryBlock);
        }

        string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";

        string body;
        if (context.Options.GreedyDeserialize)
        {
            body = $"return {memoryVectorRead}.ToArray().AsMemory();";
        }
        else
        {
            body = $"return {memoryVectorRead};";
        }

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        string body =
            $"{context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteReadOnlyByteMemoryBlock)}({context.SpanVariableName}, {context.ValueVariableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});";

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        string body = $"return {context.ItemVariableName}.ToArray().AsMemory();";
        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
