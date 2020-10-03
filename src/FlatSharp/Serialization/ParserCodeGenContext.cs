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
    public class ParserCodeGenContext
    {
        public ParserCodeGenContext(
            string inputBufferVariableName,
            string offsetVariableName,
            string inputBufferTypeName,
            IReadOnlyDictionary<Type, string> methodNameMap,
            FlatBufferSerializerOptions options)
        {
            this.InputBufferVariableName = inputBufferVariableName;
            this.OffsetVariableName = offsetVariableName;
            this.InputBufferTypeName = inputBufferTypeName;
            this.MethodNameMap = methodNameMap;
            this.Options = options;
        }

        public ParserCodeGenContext(ParserCodeGenContext other)
        {
            this.InputBufferVariableName = other.InputBufferVariableName;
            this.OffsetVariableName = other.OffsetVariableName;
            this.InputBufferTypeName = other.InputBufferTypeName;
            this.MethodNameMap = other.MethodNameMap;
            this.Options = other.Options;
        }

        public ParserCodeGenContext With(string offset = null, string inputBuffer = null)
        {
            ParserCodeGenContext context = new ParserCodeGenContext(this);
            context.OffsetVariableName = offset ?? this.OffsetVariableName;
            context.InputBufferVariableName = inputBuffer ?? this.InputBufferVariableName;
            return context;
        }

        /// <summary>
        /// The variable name of the span. Represents a <see cref="System.Span{System.Byte}"/> value.
        /// </summary>
        public string InputBufferVariableName { get; private set; }

        /// <summary>
        /// The type of the input buffer.
        /// </summary>
        public string InputBufferTypeName { get; private set; }

        /// <summary>
        /// The variable name of the span writer. Represents a <see cref="SpanWriter"/> value.
        /// </summary>
        public string OffsetVariableName { get; private set; }

        /// <summary>
        /// A mapping of type to serialize method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> MethodNameMap { get; private set; }

        /// <summary>
        /// Serialization options.
        /// </summary>
        public FlatBufferSerializerOptions Options { get; private set; }

        /// <summary>
        /// Gets a parse invocation for the given type.
        /// </summary>
        public string GetParseInvocation(Type type)
        {
            return $"{this.MethodNameMap[type]}({this.InputBufferVariableName}, {this.OffsetVariableName})";
        }
    }
}
