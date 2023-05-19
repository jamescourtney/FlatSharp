/*
 * Copyright 2023 James Courtney
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
public record ValidateCodeGenContext
{
    public ValidateCodeGenContext(
        string spanVariableName,
        string offsetVariableName,
        string outReasonVariableName,
        IMethodNameResolver methodNameResolver,
        FlatBufferSerializerOptions options,
        TypeModelContainer typeModelContainer,
        IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> allFieldContexts)
    {
        this.SpanVariableName = spanVariableName;
        this.OffsetVariableName = offsetVariableName;
        this.OutReasonVariableName = outReasonVariableName;
        this.MethodNameResolver = methodNameResolver;
        this.Options = options;
        this.TypeModelContainer = typeModelContainer;
        this.AllFieldContexts = allFieldContexts;
    }

    /// <summary>
    /// The variable name of the Input Buffer.
    /// </summary>
    public string SpanVariableName { get; init; }

    /// <summary>
    /// The variable name of the table field context. Optional.
    /// </summary>
    public string OffsetVariableName { get; init; }

    /// <summary>
    /// The variable name of the "out string reason" argument.
    /// </summary>
    public string OutReasonVariableName { get; init; }

    /// <summary>
    /// The type model container.
    /// </summary>
    public TypeModelContainer TypeModelContainer { get; init; }

    /// <summary>
    /// A mapping of type to serialize method name for that type.
    /// </summary>
    public IMethodNameResolver MethodNameResolver { get; }

    /// <summary>
    /// Serialization options.
    /// </summary>
    public FlatBufferSerializerOptions Options { get; }

    /// <summary>
    /// All contexts for the entire object graph.
    /// </summary>
    public IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> AllFieldContexts { get; }

    /// <summary>
    /// Gets a get max size invocation for the given type.
    /// </summary>
    public string GetValidateInvocation(Type type)
    {
        ITypeModel typeModel = this.TypeModelContainer.CreateTypeModel(type);

        var parts = this.MethodNameResolver.ResolveValidate(typeModel);
        StringBuilder sb = new($"{parts.@namespace}.{parts.className}.{parts.methodName}({this.SpanVariableName}, {this.OffsetVariableName}, out {this.OutReasonVariableName}");
        sb.Append(")");
        return sb.ToString();
    }
}
