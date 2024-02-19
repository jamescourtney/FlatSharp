/*
 * Copyright 2018 James Courtney
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

using System.Collections.Immutable;
using System.Linq;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a FlatBuffer string type.
/// </summary>
public class StringTypeModel : RuntimeTypeModel
{
    internal StringTypeModel(TypeModelContainer container) : base(typeof(string), container)
    {
    }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.String;

    /// <summary>
    /// Layout when in a vtable.
    /// </summary>
    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => new PhysicalLayoutElement[] { new PhysicalLayoutElement(sizeof(uint), sizeof(uint)) }.ToImmutableArray();

    /// <summary>
    /// Strings are not sensitive to deserialization mode.
    /// </summary>
    public override bool IsParsingInvariant => true;

    /// <summary>
    /// Strings are arbitrary in length.
    /// </summary>
    public override bool IsFixedSize => false;

    /// <summary>
    /// Strings can be part of tables.
    /// </summary>
    public override bool IsValidTableMember => true;

    /// <summary>
    /// Strings can be part of unions.
    /// </summary>
    public override bool IsValidUnionMember => true;

    /// <summary>
    /// Strings can be part of vectors.
    /// </summary>
    public override bool IsValidVectorMember => true;

    /// <summary>
    /// Strings can be sorted vector keys.
    /// </summary>
    public override bool IsValidSortedVectorKey => true;

    /// <summary>
    /// Strings are written by reference.
    /// </summary>
    public override bool SerializesInline => false;

    /// <summary>
    /// Strings are leaf nodes.
    /// </summary>
    public override IEnumerable<ITypeModel> Children => new ITypeModel[0];

    /// <summary>
    /// Strings need the argrument due to shared strings on the write path. Nothing needed on the read or getmaxsize paths.
    /// </summary>
    public override TableFieldContextRequirements TableFieldContextRequirements => TableFieldContextRequirements.Serialize;

    /// <summary>
    /// Gets the type of span comparer for this type.
    /// </summary>
    public Type SpanComparerType => typeof(StringSpanComparer);

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {nameof(SerializationHelpers)}.{nameof(SerializationHelpers.GetMaxSize)}({context.ValueVariableName});");
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {context.InputBufferVariableName}.{nameof(IInputBuffer.ReadString)}({context.OffsetVariableName});")
        {
            IsMethodInline = true
        };
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        string body;
        if (context.AllFieldContexts.SelectMany(x => x.Value).Any(x => x.SharedString))
        {
            // If we know this schema includes shared strings, add the if statement here.
            body = $@"
                if ({context.TableFieldContextVariableName}.{nameof(TableFieldContext.SharedString)})
                {{
                    var sharedStringWriter = {context.SerializationContextVariableName}.{nameof(SerializationContext.SharedStringWriter)};
                    if (!(sharedStringWriter is null))
                    {{
                        sharedStringWriter.{nameof(ISharedStringWriter.WriteSharedString)}(
                            {context.SpanWriterVariableName}, 
                            {context.SpanVariableName},
                            {context.OffsetVariableName},
                            {context.ValueVariableName},
                            {context.SerializationContextVariableName});
                        return;
                    }}
                }}
                
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteString)}(
                    {context.SpanVariableName},
                    {context.ValueVariableName},
                    {context.OffsetVariableName},
                    {context.SerializationContextVariableName});
            ";
        }
        else
        {
            // otherwise, we can omit that code entirely.
            body = $@"
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteString)}(
                    {context.SpanVariableName},
                    {context.ValueVariableName},
                    {context.OffsetVariableName},
                    {context.SerializationContextVariableName});
            ";
        }

        return new CodeGeneratedMethod(body) { IsMethodInline = false };
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {context.ItemVariableName};")
        {
            IsMethodInline = true,
        };
    }

    public override bool TryGetSpanComparerType(out Type comparerType)
    {
        comparerType = typeof(StringSpanComparer);
        return true;
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "string";
    }
}
