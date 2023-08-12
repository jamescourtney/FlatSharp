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

using FlatSharp.Compiler;
using FlatSharp.Compiler.Schema;
using FlatSharp.Compiler.SchemaModel;
using System.Text;

/// <summary>
/// Renames external types to have unique, random names, then replaces those in the output with real names.
/// </summary>
/// <remarks>
/// This class is slightly odd. The idea for external types is that they are defined in assemblies that FlatSharp doesn't know about.
/// However, the FlatSharp type model needs full knowledge of these types in order to build serializers. So, we emit "temporary" types
/// to take the place of these external types while we build our serializer. Then, at the end, we do a big find/replace to swap
/// the temporary types for the real types.
/// 
/// We *could* use the actual external name defined by the user in FBS, but if that has generics, then that becomes tricky. The simpler
/// solution is just to substitute our own random type names during intermediate compilation passes.
/// </remarks>
internal class ExternalTypeSchemaMutator : ISchemaMutator
{
    public void Mutate(Schema schema, CompilerOptions options, List<Func<string, string>> postProcessors)
    {
        MutateNamedItems(schema.Objects, postProcessors);
        MutateNamedItems(schema.Enums, postProcessors);
    }

    private static void MutateNamedItems(IEnumerable<INamedSchemaElement> namedElements, List<Func<string, string>> postProcessors)
    {
        foreach (INamedSchemaElement item in namedElements)
        {
            FlatSharpAttributes attrs = new(item.Attributes);

            if (attrs.ExternalTypeName is null)
            {
                continue;
            }

            string externalName = attrs.ExternalTypeName.Trim();

            if (externalName == string.Empty)
            {
                externalName = item.Name;
            }

            ParseTypeName(externalName, out string actualNamespace, out string actualTypeName);

            // X for external!
            string tempNs = $"X{HashName(actualNamespace)}";
            string tempName = $"X{HashName(actualTypeName)}";

            // Store original name for error logging
            item.OriginalName = $"{item.Name} ({externalName})";
            item.Name = $"{tempNs}.{tempName}";

            postProcessors.Add(csharp =>
            {
                csharp = csharp.Replace(tempNs, actualNamespace);
                csharp = csharp.Replace(tempName, actualTypeName);

                return csharp;
            });
        }
    }

    private static string HashName(ReadOnlySpan<char> span)
    {
        byte[] data = new byte[Encoding.UTF8.GetMaxByteCount(span.Length)];
        int length = Encoding.UTF8.GetBytes(span, data.AsSpan());

        using var sha = System.Security.Cryptography.SHA256.Create();

        byte[] hash = sha.ComputeHash(data, 0, length);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }

    private static void ParseTypeName(ReadOnlySpan<char> fullName, out string @namespace, out string name)
    {
        // Trim off anything after the first <. This removes any '.' characters from generic definitions.
        ReadOnlySpan<char> nonGenericName = fullName;
        int genericIndex = nonGenericName.IndexOf('<');

        if (genericIndex >= 0)
        {
            nonGenericName = nonGenericName[..genericIndex];
        }

        int lastDotIndex = nonGenericName.LastIndexOf('.');
        if (lastDotIndex < 0)
        {
            @namespace = "FakeNs";
            name = new string(fullName);
            ErrorContext.Current.RegisterError($"External types must be in a namespace. Type = '{name}'.");
        }
        else
        {
            @namespace = new string(fullName[..lastDotIndex].ToString());
            name = new string(fullName[(lastDotIndex + 1)..]);
        }
    }
}