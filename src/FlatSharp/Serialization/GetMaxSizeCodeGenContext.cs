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

namespace FlatSharp
{
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Code gen context for serialization methods.
    /// </summary>
    public record GetMaxSizeCodeGenContext
    {
        public GetMaxSizeCodeGenContext(
            string valueVariableName,
            string tableFieldContextVariableName,
            IReadOnlyDictionary<Type, string> methodNameMap,
            FlatBufferSerializerOptions options,
            TypeModelContainer typeModelContainer,
            IReadOnlyDictionary<ITypeModel, List<TableFieldContext>> allFieldContexts)
        {
            this.ValueVariableName = valueVariableName;
            this.TableFieldContextVariableName = tableFieldContextVariableName;
            this.MethodNameMap = methodNameMap;
            this.Options = options;
            this.TypeModelContainer = typeModelContainer;
            this.AllFieldContexts = allFieldContexts;
        }

        /// <summary>
        /// The variable name of the current value to serialize.
        /// </summary>
        public string ValueVariableName { get; init; }

        /// <summary>
        /// The variable name of the table field context. Optional.
        /// </summary>
        public string TableFieldContextVariableName { get; init; }

        /// <summary>
        /// The type model container.
        /// </summary>
        public TypeModelContainer TypeModelContainer { get; init; }

        /// <summary>
        /// A mapping of type to serialize method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> MethodNameMap { get; }

        /// <summary>
        /// Serialization options.
        /// </summary>
        public FlatBufferSerializerOptions Options { get; }

        /// <summary>
        /// All contexts for the entire object graph.
        /// </summary>
        public IReadOnlyDictionary<ITypeModel, List<TableFieldContext>> AllFieldContexts { get; }

        /// <summary>
        /// Gets a get max size invocation for the given type.
        /// </summary>
        public string GetMaxSizeInvocation(Type type)
        {
            ITypeModel typeModel = this.TypeModelContainer.CreateTypeModel(type);

            StringBuilder sb = new($"{this.MethodNameMap[type]}({this.ValueVariableName}");

            if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.GetMaxSize))
            {
                sb.Append($", {this.TableFieldContextVariableName}");
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
