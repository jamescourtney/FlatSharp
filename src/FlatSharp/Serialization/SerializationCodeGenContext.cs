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

using System.Text;
using FlatSharp.TypeModel;

namespace FlatSharp.CodeGen;

/// <summary>
/// Code gen context for serialization methods.
/// </summary>
public record SerializationCodeGenContext
{
    public SerializationCodeGenContext(
        string serializationContextVariableName,
        string spanVariableName,
        string spanWriterVariableName,
        string valueVariableName,
        string offsetVariableName,
        string tableFieldContextVariableName,
        bool isOffsetByRef,
        TypeModelContainer typeModelContainer,
        FlatBufferSerializerOptions options,
        IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> allFieldContexts)
    {
        this.SerializationContextVariableName = serializationContextVariableName;
        this.SpanWriterVariableName = spanWriterVariableName;
        this.SpanVariableName = spanVariableName;
        this.ValueVariableName = valueVariableName;
        this.OffsetVariableName = offsetVariableName;
        this.TypeModelContainer = typeModelContainer;
        this.IsOffsetByRef = isOffsetByRef;
        this.Options = options;
        this.TableFieldContextVariableName = tableFieldContextVariableName;
        this.AllFieldContexts = allFieldContexts;
    }

    /// <summary>
    /// The variable name of the serialization context. Represents a <see cref="SerializationContext"/> value.
    /// </summary>
    public string SerializationContextVariableName { get; init; }

    /// <summary>
    /// The variable name of the table field context parameter. Represents a <see cref="TableFieldContext"/> value.
    /// </summary>
    public string TableFieldContextVariableName { get; init; }

    /// <summary>
    /// The variable name of the span. Represents a <see cref="System.Span{System.Byte}"/> value.
    /// </summary>
    public string SpanVariableName { get; init; }

    /// <summary>
    /// The variable name of the span writer. Represents a <see cref="SpanWriter"/> value.
    /// </summary>
    public string SpanWriterVariableName { get; init; }

    /// <summary>
    /// The variable name of the current value to serialize.
    /// </summary>
    public string ValueVariableName { get; init; }

    /// <summary>
    /// The variable name of the offset in the span. Represents a <see cref="Int32"/> value.
    /// </summary>
    public string OffsetVariableName { get; init; }

    /// <summary>
    /// Indicates if the offset is passed by reference.
    /// </summary>
    public bool IsOffsetByRef { get; init; }

    /// <summary>
    /// Resolves Type -> TypeModel.
    /// </summary>
    public TypeModelContainer TypeModelContainer { get; private init; }

    /// <summary>
    /// Serialization options.
    /// </summary>
    public FlatBufferSerializerOptions Options { get; private init; }

    /// <summary>
    /// All contexts for the entire object graph.
    /// </summary>
    public IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> AllFieldContexts { get; }

    /// <summary>
    /// Gets a serialization invocation for the given type.
    /// </summary>
    public string GetSerializeInvocation(Type type)
    {
        ITypeModel typeModel = this.TypeModelContainer.CreateTypeModel(type);
        string byRef = string.Empty;
        if (this.IsOffsetByRef)
        {
            byRef = "ref ";
        }

        StringBuilder sb = new StringBuilder();

        var methodParts = DefaultMethodNameResolver.ResolveSerialize(typeModel);
        sb.Append($"{methodParts.@namespace}.{methodParts.className}.{methodParts.methodName}({this.SpanWriterVariableName}, {this.SpanVariableName}, {this.ValueVariableName}, {byRef}{this.OffsetVariableName}");

        if (typeModel.SerializeMethodRequiresContext)
        {
            sb.Append($", {this.SerializationContextVariableName}");
        }

        if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.Serialize))
        {
            sb.Append($", {this.TableFieldContextVariableName}");
        }

        sb.Append(")");

        return sb.ToString();
    }
}
