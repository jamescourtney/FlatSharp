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
    using System.Diagnostics;
    using System.Reflection;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Some C# codegen helpers.
    /// </summary>
    internal static class CSharpHelpers
    {
        internal static bool ConvertProtectedInternalToProtected = true;

        internal const string GeneratedClassInputBufferFieldName = "__buffer";
        internal const string GeneratedClassOffsetFieldName = "__offset";
        internal const string GeneratedClassMaxVtableIndexFieldName = "__maxVTableIndex";
        internal const string GeneratedClassVTableOffsetFieldName = "__vtableOffset";

        internal static string GetCompilableTypeName(this Type t)
        {
            FlatSharpInternal.Assert(!string.IsNullOrEmpty(t.FullName), $"{t} has null/empty full name.");

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
                name = $"{GetCompilableTypeName(t.GetElementType()!)}[]";
            }
            else
            {
                name = t.FullName;
            }

            name = name.Replace('+', '.');
            return name;
        }

        internal static (AccessModifier propertyModifier, AccessModifier? getModifer, AccessModifier? setModifier) GetPropertyAccessModifiers(
            AccessModifier getModifier,
            AccessModifier? setModifier)
        {
            if (setModifier is null)
            {
                return (getModifier, null, null);
            }

            if (getModifier < setModifier.Value)
            {
                return (getModifier, null, setModifier.Value);
            }
            else if (setModifier.Value < getModifier)
            {
                return (setModifier.Value, getModifier, null);
            }

            return (getModifier, null, null);
        }

        internal static (AccessModifier propertyModifier, AccessModifier? getModifer, AccessModifier? setModifier) GetPropertyAccessModifiers(
            PropertyInfo property)
        {
            FlatSharpInternal.Assert(property.GetMethod is not null, $"Property {property.DeclaringType?.Name}.{property.Name} has null get method.");
            var getModifier = GetAccessModifier(property.GetMethod);

            if (property.SetMethod is null)
            {
                return (getModifier, null, null);
            }

            var setModifier = GetAccessModifier(property.SetMethod);

            return GetPropertyAccessModifiers(getModifier, setModifier);
        }

        internal static string ToCSharpString(this AccessModifier? modifier)
        {
            return modifier switch
            {
                null => string.Empty,
                _ => modifier.Value.ToCSharpString(),
            };
        }

        internal static string ToCSharpString(this AccessModifier modifier)
        {
            return modifier switch
            {
                AccessModifier.Public => "public",
                AccessModifier.Protected => "protected",
                AccessModifier.ProtectedInternal => "protected internal",
                _ => throw new InvalidOperationException($"Unexpected access modifier: '{modifier}'.")
            };
        }

        internal static AccessModifier GetAccessModifier(this MethodInfo method)
        {
            if (method.IsPublic)
            {
                return AccessModifier.Public;
            }

            if (method.IsFamilyOrAssembly)
            {
                if (ConvertProtectedInternalToProtected)
                {
                    return AccessModifier.Protected;
                }

                return AccessModifier.ProtectedInternal;
            }

            if (method.IsFamily)
            {
                return AccessModifier.Protected;
            }

            throw new InvalidOperationException("Unexpected method visibility: " + method.Name);
        }
    }
}
