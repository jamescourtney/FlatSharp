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
/// Defines a vector type model for a list vector.
/// </summary>
public class ListVectorTypeModel : BaseVectorTypeModel
{
    private bool isReadOnly;

    internal ListVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
    {
    }

    public override string LengthPropertyName => nameof(IList<byte>.Count);

    public override Type OnInitialize()
    {
        var genericDef = this.ClrType.GetGenericTypeDefinition();

        FlatSharpInternal.Assert(
            genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>),
            $"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, and ReadOnlyMemory.");

        this.isReadOnly = genericDef == typeof(IReadOnlyList<>);
        return this.ClrType.GetGenericArguments()[0];
    }

    protected override string CreateLoop(
        FlatBufferSerializerOptions options,
        string vectorVariableName,
        string numberofItemsVariableName,
        string expectedVariableName,
        string body)
    {
        string ListBody(string variable, string length)
        {
            return $@"
                int i;
                for (i = 0; i < {length}; i = unchecked(i + 1))
                {{
                    var {expectedVariableName} = {variable}[i];
                    {body}
                }}";
        }

        return $@"
                if ({vectorVariableName} is {this.ItemTypeModel.GGCTN()}[] array)
                {{
                    {ListBody("array", "array.Length")}
                }}
                else if ({vectorVariableName} is List<{this.ItemTypeModel.GGCTN()}> realList)
                {{
                    {ListBody("realList", "realList.Count")}
                }}
                else
                {{
                    {ListBody(vectorVariableName, numberofItemsVariableName)}
                }}";
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        bool isEverWriteThrough = ValidateWriteThrough(
            writeThroughSupported: !this.isReadOnly,
            this,
            this.typeModelContainer,
            context.AllFieldContexts);

        Func<ITypeModel, int, ParserCodeGenContext, bool, (string classDef, string className)>? createVector = context.Options.DeserializationOption switch
        {
            FlatBufferDeserializationOption.Lazy => FlatBufferVectorHelpers.CreateLazyVector,
            FlatBufferDeserializationOption.Progressive => FlatBufferVectorHelpers.CreateProgressiveVector,
            FlatBufferDeserializationOption.Greedy => FlatBufferVectorHelpers.CreateGreedyVector,
            FlatBufferDeserializationOption.GreedyMutable => FlatBufferVectorHelpers.CreateGreedyMutableVector,
            _ => null,
        };

        FlatSharpInternal.Assert(createVector is not null, "unexpected deserialization mode");

        (string classDef, string className) = createVector(
            this.ItemTypeModel,
            this.PaddedMemberInlineSize,
            context,
            isEverWriteThrough);

        string body =
           $@"return {className}<{context.InputBufferTypeName}>.GetOrCreate(
                    {context.InputBufferVariableName},
                    {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}),
                    {context.RemainingDepthVariableName},
                    {context.TableFieldContextVariableName});";

        return new CodeGeneratedMethod(body)
        {
            ClassDefinition = classDef,
            IsMethodInline = true,
        };
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
