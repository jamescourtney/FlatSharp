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

    /// <summary>
    /// Code gen context for serialization methods.
    /// </summary>
    public class SerializationCodeGenContext
    {
        public SerializationCodeGenContext(
            string serializationContextVariableName,
            string spanVariableName,
            string spanWriterVariableName,
            string valueVariableName,
            string offsetVariableName,
            IReadOnlyDictionary<Type, string> methodNameMap,
            TypeModelContainer typeModelContainer,
            FlatBufferSerializerOptions options)
        {
            this.SerializationContextVariableName = serializationContextVariableName;
            this.SpanWriterVariableName = spanWriterVariableName;
            this.SpanVariableName = spanVariableName;
            this.ValueVariableName = valueVariableName;
            this.OffsetVariableName = offsetVariableName;
            this.MethodNameMap = methodNameMap;
            this.TypeModelContainer = typeModelContainer;
            this.Options = options;
        }

        public SerializationCodeGenContext(SerializationCodeGenContext other)
        {
            this.SerializationContextVariableName = other.SerializationContextVariableName;
            this.SpanWriterVariableName = other.SpanWriterVariableName;
            this.SpanVariableName = other.SpanVariableName;
            this.ValueVariableName = other.ValueVariableName;
            this.OffsetVariableName = other.OffsetVariableName;
            this.MethodNameMap = other.MethodNameMap;
            this.Options = other.Options;
            this.TypeModelContainer = other.TypeModelContainer;
        }

        public SerializationCodeGenContext With(
            string? serializationContextVariableName = null,
            string? spanWriterVariableName = null,
            string? spanVariableName = null,
            string? valueVariableName = null,
            string? offsetVariableName = null)
        {
            return new SerializationCodeGenContext(this)
            {
                SerializationContextVariableName = serializationContextVariableName ?? this.SerializationContextVariableName,
                SpanWriterVariableName = spanWriterVariableName ?? this.SpanWriterVariableName,
                SpanVariableName = spanVariableName ?? this.SpanVariableName,
                ValueVariableName = valueVariableName ?? this.ValueVariableName,
                OffsetVariableName = offsetVariableName ?? this.OffsetVariableName,
            };
        }

        /// <summary>
        /// The variable name of the serialization context. Represents a <see cref="SerializationContext"/> value.
        /// </summary>
        public string SerializationContextVariableName { get; private init; }

        /// <summary>
        /// The variable name of the span. Represents a <see cref="System.Span{System.Byte}"/> value.
        /// </summary>
        public string SpanVariableName { get; private init; }

        /// <summary>
        /// The variable name of the span writer. Represents a <see cref="SpanWriter"/> value.
        /// </summary>
        public string SpanWriterVariableName { get; private init; }

        /// <summary>
        /// The variable name of the current value to serialize.
        /// </summary>
        public string ValueVariableName { get; private init; }

        /// <summary>
        /// The variable name of the offset in the span. Represents a <see cref="Int32"/> value.
        /// </summary>
        public string OffsetVariableName { get; private init; }

        /// <summary>
        /// A mapping of type to serialize method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> MethodNameMap { get; private init; }

        /// <summary>
        /// Resolves Type -> TypeModel.
        /// </summary>
        public TypeModelContainer TypeModelContainer { get; private init; }

        /// <summary>
        /// Serialization options.
        /// </summary>
        public FlatBufferSerializerOptions Options { get; private init; }

        /// <summary>
        /// Gets a serialization invocation for the given type.
        /// </summary>
        public string GetSerializeInvocation(Type type)
        {
            ITypeModel typeModel = this.TypeModelContainer.CreateTypeModel(type);
            if (typeModel.SerializeMethodRequiresContext)
            {
                return $"{this.MethodNameMap[type]}({this.SpanWriterVariableName}, {this.SpanVariableName}, {this.ValueVariableName}, {this.OffsetVariableName}, {this.SerializationContextVariableName})";
            }
            else
            {
                return $"{this.MethodNameMap[type]}({this.SpanWriterVariableName}, {this.SpanVariableName}, {this.ValueVariableName}, {this.OffsetVariableName})";
            }
        }
    }
}
