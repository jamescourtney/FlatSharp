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

    /// <summary>
    /// Extensions for <see cref="ITypeModel"/>.
    /// </summary>
    internal static class ITypeModelExtensions
    {
        /// <summary>
        /// Indicates if the given type model should be modeled as a nullable reference.
        /// </summary>
        /// <returns>True if the item is a nullable reference, false if it is a non-nullable reference, and null if it is a value type.</returns>
        public static bool? IsNullableReference(this ITypeModel typeModel)
        {
            if (typeModel.ClrType.IsValueType)
            {
                return null;
            }

            return !typeModel.SerializesInline;
        }
    }
}
