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

    internal static class FlatBufferVectorHelpers
    {
        public static (string classDef, string className) CreateFlatBufferVectorSubclass(
            ITypeModel itemTypeModel,
            ParserCodeGenContext context)
        {
            Type itemType = itemTypeModel.ClrType;

            string className = $"FlatBufferVector_{Guid.NewGuid():n}";

            context = context with
            {
                InputBufferTypeName = "TInputBuffer",
                InputBufferVariableName = "memory",
                IsOffsetByRef = false,
                TableFieldContextVariableName = "fieldContext",
            };

            var serializeContext = new SerializationCodeGenContext(
                "null",
                "span",
                "default(SpanWriter)",
                "item",
                "0",
                "this.fieldContext",
                false,
                context.SerializeMethodNameMap,
                context.TypeModelContainer,
                context.Options,
                context.AllFieldContexts);

            string writeThroughImpl = serializeContext.GetSerializeInvocation(itemType);
            if (itemTypeModel.SerializeMethodRequiresContext)
            {
                writeThroughImpl = "throw new NotImplementedException()";
            }

            string classDef = $@"
                public sealed class {className}<{context.InputBufferTypeName}> : FlatBufferVector<{itemType.GetGlobalCompilableTypeName()}, {context.InputBufferTypeName}>
                    where {context.InputBufferTypeName} : {nameof(IInputBuffer)}
                {{
                    public {className}(
                        {context.InputBufferTypeName} memory,
                        int offset,
                        int itemSize) : base(memory, offset, itemSize, default({nameof(TableFieldContext)}))
                    {{
                    }}

                    public {className}(
                        {context.InputBufferTypeName} memory,
                        int offset,
                        int itemSize,
                        {nameof(TableFieldContext)} fieldContext) : base(memory, offset, itemSize, fieldContext)
                    {{
                    }}

                    protected override void ParseItem(
                        {context.InputBufferTypeName} memory,
                        int offset,
                        in {nameof(TableFieldContext)} fieldContext,
                        out {itemType.GetGlobalCompilableTypeName()} item)
                    {{
                        item = {context.GetParseInvocation(itemType)};
                    }}

                    protected override void WriteThrough(
                        {itemType.GetGlobalCompilableTypeName()} {serializeContext.ValueVariableName},
                        Span<byte> {serializeContext.SpanVariableName})
                    {{
                        {writeThroughImpl};
                    }}
                }}
            ";

            return (classDef, className);
        }

        public static (string classDef, string className) CreateFlatBufferVectorOfUnionSubclass(
            ITypeModel typeModel,
            ParserCodeGenContext context)
        {
            string className = $"FlatBufferUnionVector_{Guid.NewGuid():n}";

            context = context with
            {
                InputBufferTypeName = "TInputBuffer",
                InputBufferVariableName = "memory",
                IsOffsetByRef = true,
                TableFieldContextVariableName = "fieldContext",
                OffsetVariableName = "temp",
            };

            string classDef = $@"
                public sealed class {className}<{context.InputBufferTypeName}> : FlatBufferVectorOfUnion<{typeModel.GetGlobalCompilableTypeName()}, {context.InputBufferTypeName}>
                    where {context.InputBufferTypeName} : {nameof(IInputBuffer)}
                {{
                    public {className}(
                        {context.InputBufferTypeName} memory,
                        int discriminatorOffset,
                        int offsetVectorOffset) : base(memory, discriminatorOffset, offsetVectorOffset, default({nameof(TableFieldContext)}))
                    {{
                    }}

                    public {className}(
                        {context.InputBufferTypeName} memory,
                        int discriminatorOffset,
                        int offsetVectorOffset,
                        {nameof(TableFieldContext)} fieldContext) : base(memory, discriminatorOffset, offsetVectorOffset, fieldContext)
                    {{
                    }}

                    protected override void ParseItem(
                        {context.InputBufferTypeName} memory,
                        int discriminatorOffset,
                        int offsetOffset,
                        in {nameof(TableFieldContext)} {context.TableFieldContextVariableName},
                        out {typeModel.GetGlobalCompilableTypeName()} item)
                    {{
                        var {context.OffsetVariableName} = (discriminatorOffset, offsetOffset);
                        item = {context.GetParseInvocation(typeModel.ClrType)};
                    }}
                }}
            ";

            return (classDef, className);
        }
    }
}
