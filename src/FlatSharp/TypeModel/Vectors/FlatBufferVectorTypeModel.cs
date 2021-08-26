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
 
namespace FlatSharp.TypeModel
{
    using System;

    /// <summary>
    /// Defines a vector type model for a memory vector of byte.
    /// </summary>
    public class FlatBufferVectorTypeModel : BaseVectorTypeModel
    {
        internal FlatBufferVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
        }

        public override string LengthPropertyName => nameof(Memory<byte>.Length);

        public override Type OnInitialize()
        {
            var genericDef = this.ClrType.GetGenericTypeDefinition();
            if (genericDef != typeof(IVector<>))
            {
                throw new InvalidFlatBufferDefinitionException($"IVector vectors must be IVector<> types.");
            }

            return typeof(byte);
        }

        protected override string CreateLoop(FlatBufferSerializerOptions options, string vectorVariableName, string numberOfItemsVariableName, string expectedVariableName, string body)
        {
            FlatSharpInternal.Assert(false, "Not expecting to do loop get max size for memory vector");
            throw new Exception();
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            string method = nameof(InputBufferExtensions.ReadByteMemoryBlock);

            string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";

            string body;

            var resultTypeName = this.ClrType.GetGenericArguments()[0].GetCompilableTypeName();
            body = WrapFlatSharpUnsafe($"return global::FlatSharp.Unsafe.FlatBufferVector.Create<{resultTypeName}>({memoryVectorRead});");

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            string body = 
                $"{context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteReadOnlyByteMemoryBlock)}({context.SpanVariableName}, {context.ValueVariableName}.GetByteMemory(), {context.OffsetVariableName}, {context.SerializationContextVariableName});";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
        {
            string body = WrapFlatSharpUnsafe($"return global::FlatSharp.Unsafe.FlatBufferVector.Clone({context.ItemVariableName});");
            return new CodeGeneratedMethod(body)
            {
                IsMethodInline = true,
            };
        }

        private string WrapFlatSharpUnsafe(string body)
        {
            return "#if FLATSHARP_UNSAFE\n"
                + $"{body}\n"
                + "#else\n"
                + "#warning Must be compiled with FLATSHARP_UNSAFE to use this functionality.\n"
                + "throw new NotSupportedException(\"Must be compiled with FLATSHARP_UNSAFE to use this functionality.\");\n"
                + "#endif";
        }
    }
}
