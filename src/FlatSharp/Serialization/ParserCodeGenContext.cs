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

using System.Text;
using FlatSharp.TypeModel;

namespace FlatSharp.CodeGen;

/// <summary>
/// Code gen context for parse methods.
/// </summary>
public record ParserCodeGenContext
{
    public ParserCodeGenContext(
        string inputBufferVariableName,
        string offsetVariableName,
        string remainingDepthVariableName,
        string inputBufferTypeName,
        bool isOffsetByRef,
        string tableFieldContextVariableName,
        FlatBufferSerializerOptions options,
        TypeModelContainer typeModelContainer,
        IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> allFieldContexts)
    {
        this.InputBufferVariableName = inputBufferVariableName;
        this.OffsetVariableName = offsetVariableName;
        this.InputBufferTypeName = inputBufferTypeName;
        this.RemainingDepthVariableName = remainingDepthVariableName;
        this.IsOffsetByRef = isOffsetByRef;
        this.Options = options;
        this.TypeModelContainer = typeModelContainer;
        this.TableFieldContextVariableName = tableFieldContextVariableName;
        this.AllFieldContexts = allFieldContexts;
    }

    /// <summary>
    /// The variable name of the span. Represents a <see cref="System.Span{System.Byte}"/> value.
    /// </summary>
    public string InputBufferVariableName { get; init; }

    /// <summary>
    /// The type of the input buffer.
    /// </summary>
    public string InputBufferTypeName { get; init; }

    /// <summary>
    /// The variable name of the span writer. Represents a <see cref="SpanWriter"/> value.
    /// </summary>
    public string OffsetVariableName { get; init; }

    /// <summary>
    /// The name of the variable that tracks the remaining depth limit. Decremented down the stack.
    /// </summary>
    public string RemainingDepthVariableName { get; init; }

    /// <summary>
    /// Indicates if the offset variable is passed by reference.
    /// </summary>
    public bool IsOffsetByRef { get; init; }

    /// <summary>
    /// The variable name of the table field context parameter. Represents a <see cref="TableFieldContext"/> value.
    /// </summary>
    public string TableFieldContextVariableName { get; init; }

    /// <summary>
    /// Serialization options.
    /// </summary>
    public FlatBufferSerializerOptions Options { get; init; }

    /// <summary>
    /// The type model container.
    /// </summary>
    public TypeModelContainer TypeModelContainer { get; }

    /// <summary>
    /// All contexts for the entire object graph.
    /// </summary>
    public IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> AllFieldContexts { get; }

    /// <summary>
    /// Gets a parse invocation for the given type.
    /// </summary>
    public string GetParseInvocation(Type type)
    {
        ITypeModel typeModel = this.TypeModelContainer.CreateTypeModel(type);

        var parts = DefaultMethodNameResolver.ResolveParse(this.Options.DeserializationOption, typeModel);
        StringBuilder sb = new($"{parts.@namespace}.{parts.className}.{parts.methodName}({this.InputBufferVariableName}, ");

        if (this.IsOffsetByRef)
        {
            sb.Append("ref ");
        }

        sb.Append(this.OffsetVariableName);
        sb.Append(", ");
        sb.Append(this.RemainingDepthVariableName);

        if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.Parse))
        {
            sb.Append(", ");
            sb.Append(this.TableFieldContextVariableName);
        }

        sb.Append(")");

        return sb.ToString();
    }

    public SerializationCodeGenContext GetWriteThroughContext(string spanVariableName, string valueVariableName, string offsetVariableName)
    {
        return new SerializationCodeGenContext(
            "null",
            spanVariableName,
            "default(SpanWriter)",
            valueVariableName,
            offsetVariableName,
            this.TableFieldContextVariableName,
            this.IsOffsetByRef,
            this.TypeModelContainer,
            this.Options,
            this.AllFieldContexts);
    }
}
