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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines a vector type model for a list vector.
    /// </summary>
    public class ListVectorTypeModel : BaseVectorTypeModel
    {
        private bool isReadOnly;

        internal ListVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
        }

        public override string LengthPropertyName => nameof(IList<byte>.Count);

        public override Type OnInitialize()
        {
            var genericDef = this.ClrType.GetGenericTypeDefinition();

            FlatSharpInternal.Assert(
                genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>),
                $"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, ReadOnlyMemory, and Arrays are supported.");

            this.isReadOnly = genericDef == typeof(IReadOnlyList<>);
            return this.ClrType.GetGenericArguments()[0];
        }

        protected override string CreateLoop(
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string numberofItemsVariableName,
            string expectedVariableName,
            string body) => CreateLoopStatic(
                this.ItemTypeModel,
                options,
                vectorVariableName,
                numberofItemsVariableName,
                expectedVariableName,
                body);

        internal static string CreateLoopStatic(
            ITypeModel typeModel,
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string numberofItemsVariableName,
            string expectedVariableName,
            string body)
        {
            string ListBody(string variable)
            {
                return $@"
                    int i;
                    for (i = 0; i < {numberofItemsVariableName}; i = unchecked(i + 1))
                    {{
                        var {expectedVariableName} = {variable}[i];
                        {body}
                    }}";
            }

            if (options.Devirtualize)
            {
                return $@"
                if ({vectorVariableName} is {typeModel.GetCompilableTypeName()}[] array)
                {{
                    {ArrayVectorTypeModel.CreateLoopStatic(options, "array", "current", body)}
                }}
                else if ({vectorVariableName} is List<{typeModel.GetCompilableTypeName()}> realList)
                {{
                    {ListBody("realList")}
                }}
                else
                {{
                    {ListBody(vectorVariableName)}
                }}";
            }
            else
            {
                return ListBody(vectorVariableName);
            }
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            this.ValidatePreallocationSettings(context.AllFieldContexts, context.Options);

            (string vectorClassDef, string vectorClassName) = FlatBufferVectorHelpers.CreateFlatBufferVectorSubclass(
                this.ItemTypeModel.ClrType,
                context);

            string body;

            string fieldContextArg = string.Empty;
            if (!string.IsNullOrEmpty(context.TableFieldContextVariableName))
            {
                fieldContextArg = $", {context.TableFieldContextVariableName}";
            }

            string createFlatBufferVector =
                $@"new {vectorClassName}<{context.InputBufferTypeName}>(
                        {context.InputBufferVariableName}, 
                        {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}), 
                        {this.PaddedMemberInlineSize}
                        {fieldContextArg})";

            if (context.Options.GreedyDeserialize)
            {
                body = $"({createFlatBufferVector}).FlatBufferVectorToList()";
                if (!context.Options.GenerateMutableObjects)
                {
                    body += ".AsReadOnly()";
                }

                body = $"return {body};";
            }
            else if (context.Options.Lazy)
            {
                body = $"return {createFlatBufferVector};";
            }
            else
            {
                FlatSharpInternal.Assert(context.Options.Progressive, "expecting progressive");
                body = $@"
                    var vector = {createFlatBufferVector};
                    if (vector.Count >= ({context.TableFieldContextVariableName}.{nameof(TableFieldContext.VectorPreallocationLimit)} ?? 1024))
                    {{
                        return new FlatBufferProgressiveVector<{this.ItemTypeModel.GetGlobalCompilableTypeName()}>(vector);
                    }}
                    else
                    {{
                        return vector.FlatBufferVectorToList().AsReadOnly();
                    }}
                ";
            }

            return new CodeGeneratedMethod(body) { ClassDefinition = vectorClassDef };
        }
    }
}
