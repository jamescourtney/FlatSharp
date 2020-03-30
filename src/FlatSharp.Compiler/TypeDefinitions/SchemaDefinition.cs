/*
 * Copyright 2020 James Courtney
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

namespace FlatSharp.Compiler
{

    using System;
    using System.Collections.Generic;

    internal class SchemaDefinition
    {
        public static IBuiltInScalarType ResolveBuiltInScalarType(string type)
        {
            if (!TryResolveBuiltInScalarType(type, out IBuiltInScalarType builtInType))
            {
                ErrorContext.Current?.RegisterError("Unexpected primitive type: " + type);
            }

            return builtInType;
        }

        public static bool TryResolveBuiltInType(string type, out IBuiltInType builtInType)
        {
            return TryResolve(type, BuiltInType.BuiltInTypes, out builtInType);
        }

        public static bool TryResolveBuiltInScalarType(string type, out IBuiltInScalarType builtInType)
        {
            return TryResolve(type, BuiltInType.BuiltInScalars, out builtInType);
        }

        private static bool TryResolve<T>(string type, IReadOnlyDictionary<Type, T> dict, out T builtInType) where T : class, IBuiltInType
        {
            foreach (var t in dict.Values)
            {
                if (t.FbsAliases.Contains(type))
                {
                    builtInType = t;
                    return true;
                }
            }

            builtInType = null;
            return false;
        }
    }
}
