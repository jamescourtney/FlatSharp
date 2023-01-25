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
/// Normalizes table and struct fields names from snake_case to UpperCamelCase.
/// </summary>
internal class FieldNameNormalizerSchemaMutator : ISchemaMutator
{
    public void Mutate(Schema schema, CompilerOptions options, List<Func<string, string>> postProcessors)
    {
        if (options.NormalizeFieldNames == false)
        {
            return;
        }

        foreach (FlatBufferObject item in schema.Objects)
        {
            bool? preserveFieldCasingParent = item.Attributes != null ? new FlatSharpAttributes(item.Attributes).PreserveFieldName : default;
            foreach (Field field in item.Fields)
            {
                bool? preserveFieldCasing = field.Attributes != null ? preserveFieldCasing = new FlatSharpAttributes(field.Attributes).PreserveFieldName : default;

                var preserve = (preserveFieldCasing ?? preserveFieldCasingParent) switch
                {
                    false or null => false,
                    true => true,
                };

                if (!preserve)
                {
                    field.Name = NormalizeFieldName(field.Name);
                }
            }
        }
    }

    private static string NormalizeFieldName(string name)
    {
        StringBuilder sb = new();
        string[] parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            sb.Append(char.ToUpperInvariant(part[0]));
            if (part.Length > 1)
            {
                sb.Append(part.AsSpan()[1..]);
            }
        }

        return sb.ToString();
    }
}