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
            $"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, ReadOnlyMemory, and Arrays are supported.");

        this.isReadOnly = genericDef == typeof(IReadOnlyList<>);
        return this.ClrType.GetGenericArguments()[0];
    }

    protected override string CreateLoop(
        FlatBufferSerializerOptions options,
        string vectorVariableName,
        string numberofItemsVariableName,
        string expectedVariableName,
        string body) => CreateLoopStatic(
            this.ItemTypeModel,
            options,
            vectorVariableName,
            numberofItemsVariableName,
            expectedVariableName,
            body);

    internal static string CreateLoopStatic(
        ITypeModel typeModel,
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

        if (options.Devirtualize)
        {
            return $@"
                if ({vectorVariableName} is {typeModel.GetCompilableTypeName()}[] array)
                {{
                    int length = array.Length;
                    {ListBody("array", "array.Length")}
                }}
                else if ({vectorVariableName} is List<{typeModel.GetCompilableTypeName()}> realList)
                {{
                    {ListBody("realList", "realList.Count")}
                }}
                else
                {{
                    {ListBody(vectorVariableName, numberofItemsVariableName)}
                }}";
        }
        else
        {
            return ListBody(vectorVariableName, numberofItemsVariableName);
        }
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        bool isEverWriteThrough = ValidateWriteThrough(
            writeThroughSupported: !this.isReadOnly,
            this,
            context.AllFieldContexts,
            context.Options);

        (string vectorClassDef, string vectorClassName) = FlatBufferVectorHelpers.CreateVectorItemAccessor(
            this.ItemTypeModel,
            this.PaddedMemberInlineSize,
            context,
            isEverWriteThrough);

        string accessorClassName = $"{vectorClassName}<{context.InputBufferTypeName}>";

        string createFlatBufferVector =
            $@"new FlatBufferVectorBase<{this.ItemTypeModel.GetGlobalCompilableTypeName()}, {context.InputBufferTypeName}, {accessorClassName}> (
                    {context.InputBufferVariableName}, 
                    new {accessorClassName}(
                        {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}),
                        {context.InputBufferVariableName}),
                    {context.RemainingDepthVariableName},
                    {context.TableFieldContextVariableName})";

        return new CodeGeneratedMethod(CreateParseBody(this.ItemTypeModel, createFlatBufferVector, accessorClassName, context, isEverWriteThrough)) { ClassDefinition = vectorClassDef };
    }

    internal static string CreateParseBody(
        ITypeModel itemTypeModel,
        string createFlatBufferVector,
        string itemAccessorTypeName,
        ParserCodeGenContext context,
        bool isEverWriteThrough = false)
    {
        FlatSharpInternal.Assert(!string.IsNullOrEmpty(context.TableFieldContextVariableName), "expecting table field context");

        if (context.Options.DeserializationOption == FlatBufferDeserializationOption.GreedyMutable && isEverWriteThrough)
        {
            string body = $$"""
                var result = {{createFlatBufferVector}};
                if ({{context.TableFieldContextVariableName}}.{{nameof(TableFieldContext.WriteThrough)}})
                {
                    // WriteThrough vectors are not mutable in greedymutable mode.
                    return result.ToImmutableList();
                }
                else
                {
                    return result.FlatBufferVectorToList();
                }
            """;

            return body;
        }
        else if (context.Options.GreedyDeserialize)
        {
            string transform = "ToImmutableList()";
            if (context.Options.GenerateMutableObjects)
            {
                transform = "FlatBufferVectorToList()";
            }

            return $"return ({createFlatBufferVector}).{transform};";
        }
        else if (context.Options.Lazy)
        {
            return $"return {createFlatBufferVector};";
        }
        else
        {
            FlatSharpInternal.Assert(context.Options.Progressive, "expecting progressive");
            return $"return new FlatBufferProgressiveVector<{itemTypeModel.GetGlobalCompilableTypeName()}, {context.InputBufferTypeName}, {itemAccessorTypeName}>({createFlatBufferVector});";
        }
    }
}
