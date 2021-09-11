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
    using System.Collections.Generic;

    /// <summary>
    /// Defines a vector of union type model.
    /// </summary>
    public class ListVectorOfUnionTypeModel : BaseVectorOfUnionTypeModel
    {
        public ListVectorOfUnionTypeModel(Type clrType, TypeModelContainer container)
            : base(clrType, container)
        {
        }

        public override string LengthPropertyName => nameof(List<byte>.Count);

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            var (classDef, className) = FlatBufferVectorHelpers.CreateFlatBufferVectorOfUnionSubclass(
                this.ItemTypeModel,
                context);

            string fieldContextArg = string.Empty;
            if (!string.IsNullOrEmpty(context.TableFieldContextVariableName))
            {
                fieldContextArg = $", {context.TableFieldContextVariableName}";
            }

            string createFlatBufferVector =
                $@"new {className}<{context.InputBufferTypeName}>(
                        {context.InputBufferVariableName}, 
                        {context.OffsetVariableName}.offset0 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset0), 
                        {context.OffsetVariableName}.offset1 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset1)
                        {fieldContextArg})";

            string body;

            if (context.Options.GreedyDeserialize)
            {
                // We just call .ToList(). Note that when full greedy mode is on, these items will be 
                // greedily initialized as we traverse the list. Otherwise, they'll be allocated lazily.
                body = $"({createFlatBufferVector}).FlatBufferVectorToList()";

                if (!context.Options.GenerateMutableObjects)
                {
                    // Finally, if we're not in the business of making mutable objects, then convert the list to read only.
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

            return new CodeGeneratedMethod(body) { ClassDefinition = classDef };
        }

        public override Type OnInitialize()
        {
            var genericDef = this.ClrType.GetGenericTypeDefinition();

            FlatSharpInternal.Assert(
                genericDef == typeof(IList<>) || genericDef == typeof(IReadOnlyList<>), 
                "List vector of union must be IList or IReadOnlyList.");

            return this.ClrType.GetGenericArguments()[0];
        }
    }
}
