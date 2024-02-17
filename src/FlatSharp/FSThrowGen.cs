/*
 * Copyright 2024 James Courtney
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

using System.Linq;
using System.Threading;

namespace FlatSharp;

internal class FSThrowGenerator
{
    private readonly Dictionary<(string method, string? message), (string body, string invocation)> generatedMethods = new();

    public string ToClassDefinition()
    {
        (string ns, string name) = OperationContext.Current.Resolver.GetThrowHelperClassName();

        return 
            $$"""
                namespace {{ns}}
                {
                    internal static class {{name}}
                    {
                        {{string.Join("\r\n", this.generatedMethods.Values.Select(x => x.body))}}
                    }
                }
            """;
    }

    public string CreateThrowMethodCall(string fsThrowMethod, string? message = null)
    {
        var key = (fsThrowMethod, message);
        if (this.generatedMethods.TryGetValue(key, out var value))
        {
            return value.invocation;
        }

        string argument = string.Empty;
        if (message is not null)
        {
            argument = $"\"{message}\"";
        }

        string name = $"ThrowHelper_{Guid.NewGuid():n}";


        // Consider [DoesNotReturn].
        string body = $$"""
            [{{typeof(MethodImplAttribute).GGCTN()}}({{typeof(MethodImplOptions).GGCTN()}}.{{nameof(MethodImplOptions.NoInlining)}})]
            internal static void {{name}}()
            {
                {{typeof(FSThrow).GGCTN()}}.{{fsThrowMethod}}({{argument}});
            }
        """;

        (string ns, string className) = OperationContext.Current.Resolver.GetThrowHelperClassName();
        string invocation =  $"global::{ns}.{className}.{name}()";

        this.generatedMethods[key] = (body, invocation);

        return invocation;
    }
}
