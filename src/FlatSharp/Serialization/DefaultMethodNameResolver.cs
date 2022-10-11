/*
 * Copyright 2022 James Courtney
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

using System.Security.Cryptography;
using System.Text;
using FlatSharp.TypeModel;

namespace FlatSharp.CodeGen;

/// <summary>
/// A default method name resolver, which resolves method and class
/// names for generated serializers.
/// </summary>
public class DefaultMethodNameResolver : IMethodNameResolver
{
    private readonly Dictionary<Type, string> namespaceMapping = new();

    private const string FlatSharpGenerated = "FlatSharp.Compiler.Generated";
    private const string HelperClassName = "Helpers";
    private const string GeneratedSerializer = "Serializer";

    public (string @namespace, string name) ResolveGeneratedSerializerClassName(ITypeModel type)
    {
        return (this.GetNamespace(type), GeneratedSerializer);
    }

    public (string @namespace, string name) ResolveHelperClassName(ITypeModel type)
    {
        return (this.GetNamespace(type), HelperClassName);
    }

    public (string @namespace, string className, string methodName) ResolveGetMaxSize(ITypeModel type)
    {
        return (this.GetGlobalNamespace(type), HelperClassName, "GetMaxSize");
    }

    public (string @namespace, string className, string methodName) ResolveParse(FlatBufferDeserializationOption option, ITypeModel type)
    {
        if (type.IsParsingInvariant)
        {
            return (this.GetGlobalNamespace(type), HelperClassName, $"Parse");
        }

        return (this.GetGlobalNamespace(type), HelperClassName, $"Parse_{option}");
    }

    public (string @namespace, string className, string methodName) ResolveSerialize(ITypeModel type)
    {
        return (this.GetGlobalNamespace(type), HelperClassName, $"Serialize");
    }

    private string GetGlobalNamespace(ITypeModel type)
    {
        return $"global::{this.GetNamespace(type)}";
    }

    private string GetNamespace(ITypeModel type)
    {
        if (!this.namespaceMapping.TryGetValue(type.ClrType, out string? ns))
        {
            byte[] data = Encoding.UTF8.GetBytes(type.GetGlobalCompilableTypeName());
            using var sha = SHA256.Create();

            byte[] hash = sha.ComputeHash(data);

            ns = $"{FlatSharpGenerated}.N{BitConverter.ToString(hash).Replace("-", string.Empty)}";
            this.namespaceMapping[type.ClrType] = ns;
        }

        return ns;
    }
}
