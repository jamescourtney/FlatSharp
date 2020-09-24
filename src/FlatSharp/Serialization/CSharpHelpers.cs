﻿/*
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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Generates a collection of methods to help serialize the given root type.
    /// Does recursive traversal of the object graph and builds a set of methods to assist with populating vtables and writing values.
    /// 
    /// Eventually, everything must reduce to a built in type of string / scalar, which this will then call out to.
    /// </summary>
    internal static class CSharpHelpers
    {
        internal static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);

        internal static string GetFullMethodName(MethodInfo info)
        {
            return $"{info.DeclaringType.FullName}.{info.Name}";
        }

        internal static string GetCompilableTypeName(Type t)
        {
            string name;
            if (t.IsGenericType)
            {
                List<string> parameters = new List<string>();
                foreach (var generic in t.GetGenericArguments())
                {
                    parameters.Add(GetCompilableTypeName(generic));
                }

                name = $"{t.FullName.Split('`')[0]}<{string.Join(", ", parameters)}>";
            }
            else if (t.IsArray)
            {
                name = $"{GetCompilableTypeName(t.GetElementType())}[]";
            }
            else
            {
                name = t.FullName;
            }

            name = name.Replace('+', '.');
            return name;
        }

        internal static string GetAccessModifier(PropertyInfo property)
        {
            var method = property.GetGetMethod() ?? property.GetMethod;

            if (method.IsPublic)
            {
                return "public";
            }

            throw new InvalidOperationException("Unexpected method visibility: " + method.Name);
        }

        internal static string CreateDeserializeClass(
            string className, 
            Type baseType, 
            IEnumerable<GeneratedProperty> propertyOverrides,
            FlatBufferSerializerOptions options)
        {
            string inputBufferFieldDef = "private readonly InputBuffer buffer;";
            string offsetFieldDef = "private readonly int offset;";

            string ctorBody =
$@"
                this.buffer = buffer;
                this.offset = offset;
";

            if (options.GreedyDeserialize)
            {
                inputBufferFieldDef = string.Empty;
                offsetFieldDef = string.Empty;
                ctorBody = string.Join("\r\n", propertyOverrides.Select(x => $"this.{x.BackingFieldName} = {x.ReadValueMethodName}(buffer, offset);"));
            }

            return
$@"
                private sealed class {className} : {CSharpHelpers.GetCompilableTypeName(baseType)}
                {{
                    {inputBufferFieldDef}
                    {offsetFieldDef}
        
                    public {className}({nameof(InputBuffer)} buffer, int offset)
                    {{
                        {ctorBody}
                    }}

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
        }
    }
}
