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

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using FlatSharp.TypeModel;

namespace FlatSharp.CodeGen;

/// <summary>
/// A default method name resolver, which resolves method and class
/// names for generated serializers.
/// </summary>
public static class DefaultMethodNameResolver
{
    private static readonly ConcurrentDictionary<Type, string> namespaceMapping = new();

    private const string FlatSharpGenerated = "FlatSharp.Compiler.Generated";
    private const string HelperClassName = "Helpers";
    private const string GeneratedSerializer = "Serializer";

    public static (string @namespace, string name) ResolveGeneratedSerializerClassName(ITypeModel type)
    {
        return (GetNamespace(type), GeneratedSerializer);
    }

    public static (string @namespace, string name) ResolveHelperClassName(ITypeModel type)
    {
        return (GetNamespace(type), HelperClassName);
    }

    public static (string @namespace, string className, string methodName) ResolveGetMaxSize(ITypeModel type)
    {
        return (GetGlobalNamespace(type), HelperClassName, "GetMaxSize");
    }

    public static (string @namespace, string className, string methodName) ResolveParse(FlatBufferDeserializationOption option, ITypeModel type)
    {
        if (type.IsParsingInvariant)
        {
            return (GetGlobalNamespace(type), HelperClassName, $"Parse");
        }

        return (GetGlobalNamespace(type), HelperClassName, $"Parse_{option}");
    }

    public static (string @namespace, string className, string methodName) ResolveSerialize(ITypeModel type)
    {
        return (GetGlobalNamespace(type), HelperClassName, "Serialize");
    }

    private static string GetGlobalNamespace(ITypeModel type)
    {
        return $"global::{GetNamespace(type)}";
    }

    private static string GetNamespace(ITypeModel type)
    {
        if (!namespaceMapping.TryGetValue(type.ClrType, out string? ns))
        {
            byte[] data = Encoding.UTF8.GetBytes(type.GetGlobalCompilableTypeName());
            using var sha = SHA256.Create();

            byte[] hash = sha.ComputeHash(data);

            ns = $"{FlatSharpGenerated}.N{BitConverter.ToString(hash).Replace("-", string.Empty)}";
            namespaceMapping[type.ClrType] = ns;
        }

        return ns;
    }
}
