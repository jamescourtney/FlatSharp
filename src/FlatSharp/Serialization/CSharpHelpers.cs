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
            else
            {
                name = t.FullName;
            }

            name = name.Replace('+', '.');
            return name;
        }

        internal static string GetNonNullCheckInvocation(RuntimeTypeModel typeModel, string variableName)
        {
            if (typeModel.SchemaType == FlatBufferSchemaType.Scalar)
            {
                return string.Empty;
            }

            return $"{GetFullMethodName(ReflectedMethods.SerializationHelpers_EnsureNonNull(typeModel.ClrType))}({variableName})";
        }

        internal static string GetDefaultValueToken(TableMemberModel memberModel)
        {
            var itemTypeModel = memberModel.ItemTypeModel;
            var clrType = itemTypeModel.ClrType;

            string defaultValue = $"default({GetCompilableTypeName(memberModel.ItemTypeModel.ClrType)})";
            if (memberModel.HasDefaultValue)
            {
                string literalSpecifier = string.Empty;
                string cast = string.Empty;
                string defaultValueLiteral = memberModel.DefaultValue.ToString();

                if (clrType == typeof(bool))
                {
                    // Bool.ToString() returns 'True', which is not the right keyword.
                    defaultValueLiteral = defaultValueLiteral.ToLower();
                }
                else if (clrType == typeof(sbyte))
                {
                    cast = "(sbyte)";
                }
                else if (clrType == typeof(byte))
                {
                    cast = "(byte)";
                }
                else if (clrType == typeof(short))
                {
                    cast = "(short)";
                }
                else if (clrType == typeof(ushort))
                {
                    cast = "(ushort)";
                }
                else if (clrType == typeof(float))
                {
                    literalSpecifier = "f";
                }
                else if (clrType == typeof(uint))
                {
                    literalSpecifier = "u";
                }
                else if (clrType == typeof(int))
                {
                    // shouldn't need this one, but let's be thorough.
                    cast = "(int)";
                }
                else if (clrType == typeof(double))
                {
                    literalSpecifier = "d";
                }
                else if (clrType == typeof(long))
                {
                    literalSpecifier = "L";
                }
                else if (clrType == typeof(ulong))
                {
                    literalSpecifier = "ul";
                }
                else if (clrType.IsEnum)
                {
                    defaultValueLiteral = $"{CSharpHelpers.GetCompilableTypeName(clrType)}.{defaultValueLiteral}";
                }
                else
                {
                    throw new InvalidOperationException("Unexpected default value type: " + clrType.FullName);
                }

                defaultValue = $"{cast}{defaultValueLiteral}{literalSpecifier}";
            }

            return defaultValue;
        }

        internal static string GetAccessModifier(PropertyInfo property)
        {
            var method = property.GetGetMethod() ?? property.GetMethod;

            if (method.IsPublic)
            {
                return "public";
            }

            if (method.IsFamilyOrAssembly)
            {
                return "protected internal";
            }

            if (method.IsFamily)
            {
                return "protected";
            }

            if (method.IsPrivate)
            {
                return "private";
            }

            if (method.IsAssembly)
            {
                return "internal";
            }

            throw new InvalidOperationException("Unexpected method visibility: " + method.Name);
        }
    }
}
