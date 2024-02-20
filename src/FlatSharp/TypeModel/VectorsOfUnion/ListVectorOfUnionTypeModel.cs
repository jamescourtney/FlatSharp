/*
 * Copyright 2021 James Courtney
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
/// Defines a vector of union type model.
/// </summary>
public class ListVectorOfUnionTypeModel : BaseVectorOfUnionTypeModel
{
    public ListVectorOfUnionTypeModel(Type clrType, TypeModelContainer container)
        : base(clrType, container)
    {
    }

    public override string LengthPropertyName => nameof(List<byte>.Count);

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        Func<ITypeModel, ParserCodeGenContext, (string classDef, string className)>? createVector = context.Options.DeserializationOption switch
        {
            FlatBufferDeserializationOption.Lazy => FlatBufferVectorHelpers.CreateLazyUnionVector,
            FlatBufferDeserializationOption.Progressive => FlatBufferVectorHelpers.CreateProgressiveUnionVector,
            FlatBufferDeserializationOption.Greedy => FlatBufferVectorHelpers.CreateGreedyUnionVector,
            FlatBufferDeserializationOption.GreedyMutable => FlatBufferVectorHelpers.CreateGreedyMutableUnionVector,
            _ => null,
        };

        FlatSharpInternal.Assert(createVector is not null, "unexpected deserialization mode");

        (string classDef, string className) = createVector(this.ItemTypeModel, context);

        string body =
           $@"
                var offsets = 
                (
                    {context.OffsetVariableName}.offset0 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset0),
                    {context.OffsetVariableName}.offset1 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset1)
                );

                return {className}<{context.InputBufferTypeName}>.GetOrCreate(
                    {context.InputBufferVariableName},
                    ref offsets,
                    {context.RemainingDepthVariableName},
                    {context.TableFieldContextVariableName});";

        return new CodeGeneratedMethod(body)
        {
            ClassDefinition = classDef,
            IsMethodInline = true,
        };
    }

    public override Type OnInitialize()
    {
        var genericDef = this.ClrType.GetGenericTypeDefinition();

        FlatSharpInternal.Assert(
            genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>),
            "List vector of union must be IList or IReadOnlyList.");

        return this.ClrType.GetGenericArguments()[0];
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
