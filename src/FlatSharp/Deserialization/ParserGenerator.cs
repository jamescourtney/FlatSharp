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
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Emits IL to override types with flatbuffer deserialization.
    /// </summary>
    internal class ParserGenerator
    {
        private readonly ParserGeneratorDependencyResolver resolver;

        public ParserGenerator(bool implementIDeserializedObject, bool cacheListVectorData)
        {
            this.resolver = new ParserGeneratorDependencyResolver(implementIDeserializedObject, cacheListVectorData);
        }

        /// <summary>
        /// Generates a delegate that invokes the parse tree for this type. Note: it may be desirable to switch this to an interface in the future.
        /// </summary>
        public Func<InputBuffer, int, TBaseType> GenerateParser<TBaseType>()
        {
            var readMethod = this.resolver.GetOrCreateParser(typeof(TBaseType));

            this.resolver.CloseAllTypes();

            readMethod = readMethod.DeclaringType.GetMethod(readMethod.Name, BindingFlags.Public | BindingFlags.Static);

            var memoryParameter = Expression.Parameter(typeof(InputBuffer));
            var offsetParameter = Expression.Parameter(typeof(int));
            Expression body = Expression.Call(null, readMethod, memoryParameter, offsetParameter);

            var expr = Expression.Lambda<Func<InputBuffer, int, TBaseType>>(
                body,
                memoryParameter,
                offsetParameter);

            return expr.Compile();
        }
    }
}
