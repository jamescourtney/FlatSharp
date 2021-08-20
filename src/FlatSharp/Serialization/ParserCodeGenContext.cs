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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Code gen context for parse methods.
    /// </summary>
    public record ParserCodeGenContext
    {
        public ParserCodeGenContext(
            string inputBufferVariableName,
            string offsetVariableName,
            string inputBufferTypeName,
            bool isOffsetByRef,
            IReadOnlyDictionary<Type, string> methodNameMap,
            IReadOnlyDictionary<Type, string> serializeMethodNameMap,
            FlatBufferSerializerOptions options)
        {
            this.InputBufferVariableName = inputBufferVariableName;
            this.OffsetVariableName = offsetVariableName;
            this.InputBufferTypeName = inputBufferTypeName;
            this.MethodNameMap = methodNameMap;
            this.SerializeMethodNameMap = serializeMethodNameMap;
            this.IsOffsetByRef = isOffsetByRef;
            this.Options = options;
        }

        /// <summary>
        /// The variable name of the span. Represents a <see cref="System.Span{System.Byte}"/> value.
        /// </summary>
        public string InputBufferVariableName { get; init; }

        /// <summary>
        /// The type of the input buffer.
        /// </summary>
        public string InputBufferTypeName { get; init; }

        /// <summary>
        /// The variable name of the span writer. Represents a <see cref="SpanWriter"/> value.
        /// </summary>
        public string OffsetVariableName { get; init; }

        /// <summary>
        /// Indicates if the offset variable is passed by reference.
        /// </summary>
        public bool IsOffsetByRef { get; init; }

        /// <summary>
        /// A mapping of type to serialize method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> MethodNameMap { get; }

        /// <summary>
        /// A mapping of type to serialize method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> SerializeMethodNameMap { get; }

        /// <summary>
        /// Serialization options.
        /// </summary>
        public FlatBufferSerializerOptions Options { get; }

        /// <summary>
        /// Gets a parse invocation for the given type.
        /// </summary>
        public string GetParseInvocation(Type type)
        {
            string byRef = string.Empty;
            if (this.IsOffsetByRef)
            {
                byRef = "ref ";
            }

            return $"{this.MethodNameMap[type]}({this.InputBufferVariableName}, {byRef}{this.OffsetVariableName})";
        }
    }
}
