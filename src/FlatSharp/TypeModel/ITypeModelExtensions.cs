/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.TypeModel
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extensions for <see cref="ITypeModel"/>.
    /// </summary>
    internal static class ITypeModelExtensions
    {
        /// <summary>
        /// Indicates if the given type model should be modeled as a nullable reference.
        /// </summary>
        /// <returns>True if the item is a nullable reference, false if it is a non-nullable reference, and null if it is a value type.</returns>
        public static bool? IsFlatBufferNullableReference(this ITypeModel typeModel)
        {
            if (typeModel.ClrType.IsValueType)
            {
                return null;
            }

            return !typeModel.SerializesInline;
        }

        /// <summary>
        /// Shortcut for getting compilable type name.
        /// </summary>
        public static string GetCompilableTypeName(this ITypeModel typeModel) 
            => CSharpHelpers.GetCompilableTypeName(typeModel.ClrType);

        /// <summary>
        /// Gets a value indicating if the type of this type model is a non-nullable CLR value type.
        /// </summary>
        public static bool IsNonNullableClrValueType(this ITypeModel typeModel) 
            => typeModel.ClrType.IsValueType && Nullable.GetUnderlyingType(typeModel.ClrType) is null;

        /// <summary>
        /// Returns a boolean expression that compares the given variable name to the given default value literal for inequality.
        /// </summary>
        public static string GetNotEqualToDefaultValueLiteralExpression(this ITypeModel typeModel, string variableName, string defaultValueLiteral)
        {
            if (typeModel.IsNonNullableClrValueType())
            {
                // Structs are really weird.
                // Value types are not guaranteed to have an equality operator defined, so we can't just assume
                // that '==' will work, so we do a little reflection, and use the operator if it is defined.
                // otherwise, we assume that the operands are not equal no matter what.
                var notEqualMethod = typeModel.ClrType.GetMethod("op_Inequality", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (notEqualMethod is not null || typeModel.ClrType.IsPrimitive || typeModel.ClrType.IsEnum)
                {
                    // Let's hope that != is implemented rationally!
                    return $"{variableName} != {defaultValueLiteral}";
                }
                else
                {
                    // structs without an inequality overload have to be assumed to be not equal.
                    return "true";
                }
            }
            else
            {
                // Nullable<T> and reference types are easy.
                return $"!({variableName} is {defaultValueLiteral})";
            }
        }

        /// <summary>
        /// Returns a compile-time constant default expression for the given type model.
        /// </summary>
        public static string GetTypeDefaultExpression(this ITypeModel typeModel)
        {
            if (typeModel.IsNonNullableClrValueType())
            {
                return $"default({typeModel.GetCompilableTypeName()})";
            }
            else
            {
                return "null";
            }
        }
    }
}
