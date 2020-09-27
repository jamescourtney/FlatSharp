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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;

    internal static class SchemaDefinition
    {
        internal static readonly ITypeModelProvider BuiltInTypeModelProvider = new FlatSharpTypeModelProvider();

        public static ITypeModel ResolveBuiltInScalarType(string type)
        {
            if (!TryResolve(type, out ITypeModel builtInType))
            {
                ErrorContext.Current?.RegisterError("Unexpected primitive type: " + type);
            }

            return builtInType;
        }

        public static bool TryResolve(string type, out ITypeModel builtInType)
        {
            return TryResolve(type, BuiltInTypeModelProvider, out builtInType);
        }

        private static bool TryResolve(string type, ITypeModelProvider provider, out ITypeModel builtInType)
        {
            return provider.TryResolveFbsAlias(type, out builtInType);
        }
    }
}
