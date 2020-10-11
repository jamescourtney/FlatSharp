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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Some C# codegen helpers.
    /// </summary>
    internal static class CSharpHelpers
    {
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
            string inputBufferFieldDef = "private readonly TInputBuffer buffer;";
            string offsetFieldDef = "private readonly int offset;";

            List<string> ctorStatements = new List<string>
            {
                "this.buffer = buffer;",
                "this.offset = offset;"
            };

            if (options.GreedyDeserialize)
            {
                inputBufferFieldDef = string.Empty;
                offsetFieldDef = string.Empty;
                ctorStatements.Clear();
            }

            foreach (var property in propertyOverrides)
            {
                if (property.MemberModel.IsVirtual)
                {
                    if (options.GreedyDeserialize)
                    {
                        ctorStatements.Add(
                            $"this.{property.BackingFieldName} = {property.ReadValueMethodName}(buffer, offset);");
                    }
                }
                else
                {
                    ctorStatements.Add(
                        $"base.{property.MemberModel.PropertyInfo.Name} = {property.ReadValueMethodName}(buffer, offset);");
                }
            }

            return
$@"
                private sealed class {className}<TInputBuffer> : {CSharpHelpers.GetCompilableTypeName(baseType)} where TInputBuffer : IInputBuffer
                {{
                    {inputBufferFieldDef}
                    {offsetFieldDef}
        
                    public {className}(TInputBuffer buffer, int offset)
                    {{
                        {string.Join("\r\n", ctorStatements)}
                    }}

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
        }
    }
}
