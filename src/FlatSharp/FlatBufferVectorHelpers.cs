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

using System.IO;

namespace FlatSharp.TypeModel;

internal static class FlatBufferVectorHelpers
{
    public static (string classDef, string className) CreateVectorItemAccessor(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        string className = $"ItemAccessor_{Guid.NewGuid():n}";
        string itemTypeName = itemTypeModel.GetGlobalCompilableTypeName();

        context = context with
        {
            InputBufferTypeName = "TInputBuffer",
            InputBufferVariableName = "buffer",
            IsOffsetByRef = false,
            TableFieldContextVariableName = "fieldContext",
            RemainingDepthVariableName = "remainingDepth",
        };

        var serializeContext = context.GetWriteThroughContext("data", "item", "0");
        string writeThroughBody = $"throw new NotMutableException(\"FlatBufferVector does not support mutation.\");";
        if (isEverWriteThrough)
        {
            writeThroughBody = @$"
                if (!context.WriteThrough)
                {{
                    {writeThroughBody}
                }}

                int offset = checked(this.offset + ({inlineSize} * index));
                Span<byte> {serializeContext.SpanVariableName} = inputBuffer.GetSpan().Slice(offset, {inlineSize});

                {serializeContext.GetSerializeInvocation(itemTypeModel.ClrType)};
            ";
        }

        string body = $@"

internal struct {className}<{context.InputBufferTypeName}> : IVectorItemAccessor<{itemTypeName}, {context.InputBufferTypeName}>
    where {context.InputBufferTypeName} : IInputBuffer
{{
    private readonly int offset;
    private readonly int count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public {className}(int offset, TInputBuffer buffer)
    {{
        this.count = checked((int)buffer.ReadUInt(offset));

        // Advance to the start of the element at index 0. Easiest to do this once
        // in the .ctor than repeatedly for each index.
        this.offset = checked(offset + sizeof(uint));
    }}

    public int Count => this.count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ParseItem(int index, {context.InputBufferTypeName} {context.InputBufferVariableName}, short {context.RemainingDepthVariableName}, TableFieldContext {context.TableFieldContextVariableName}, out {itemTypeName} item)
    {{
        int {context.OffsetVariableName} = this.offset + ({inlineSize} * index);
        item = {context.GetParseInvocation(itemTypeModel.ClrType)};
    }}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteThrough(int index, {itemTypeName} {serializeContext.ValueVariableName}, {context.InputBufferTypeName} inputBuffer, TableFieldContext context)
    {{
        {writeThroughBody}
    }}
}}";
        return (body, className);
    }

    public static (string classDef, string className) CreateVectorOfUnionItemAccessor(
        ITypeModel typeModel,
        ParserCodeGenContext context)
    {
        string className = $"FlatBufferUnionVectorAccessor_{Guid.NewGuid():n}";
        string itemTypeName = typeModel.GetGlobalCompilableTypeName();

        context = context with
        {
            InputBufferTypeName = "TInputBuffer",
            InputBufferVariableName = "memory",
            IsOffsetByRef = true,
            TableFieldContextVariableName = "fieldContext",
            OffsetVariableName = "temp",
            RemainingDepthVariableName = "remainingDepth",
        };

        string classDef = $@"

internal struct {className}<{context.InputBufferTypeName}> : IVectorItemAccessor<{itemTypeName}, {context.InputBufferTypeName}>
    where {context.InputBufferTypeName} : IInputBuffer
{{
    private readonly int discriminatorVectorOffset;
    private readonly int offsetVectorOffset;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public {className}(
        {context.InputBufferTypeName} memory,
        int discriminatorOffset,
        int offsetVectorOffset)
    {{
        uint discriminatorCount = memory.ReadUInt(discriminatorOffset);
        uint offsetCount = memory.ReadUInt(offsetVectorOffset);

        if (discriminatorCount != offsetCount)
        {{
            throw new {typeof(InvalidDataException).GetGlobalCompilableTypeName()}($""Union vector had mismatched number of discriminators and offsets."");
        }}

        this.Count = (int)offsetCount;
        this.discriminatorVectorOffset = discriminatorOffset + sizeof(int);
        this.offsetVectorOffset = offsetVectorOffset + sizeof(int);
    }}

    public int Count {{ get; }}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ParseItem(int index, {context.InputBufferTypeName} {context.InputBufferVariableName}, short {context.RemainingDepthVariableName}, TableFieldContext {context.TableFieldContextVariableName}, out {itemTypeName} item)
    {{
        var {context.OffsetVariableName} = (this.discriminatorVectorOffset + index, this.offsetVectorOffset + (index * sizeof(int)));
        item = {context.GetParseInvocation(typeModel.ClrType)};
    }}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteThrough(int index, {itemTypeName} value, {context.InputBufferTypeName} inputBuffer, TableFieldContext context)
    {{
        throw new NotMutableException();
    }}
}}
";


        return (classDef, className);
    }
}
