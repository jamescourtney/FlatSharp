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

    /// <summary>
    /// Defines a vector of union type model.
    /// </summary>
    public class ArrayVectorOfUnionTypeModel : BaseVectorOfUnionTypeModel
    {
        public ArrayVectorOfUnionTypeModel(Type clrType, TypeModelContainer container)
            : base(clrType, container)
        {
        }

        public override string LengthPropertyName => "Length";

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            if (!context.Options.GreedyDeserialize)
            {
                throw new InvalidFlatBufferDefinitionException("Array vectors may only be used with Greedy serializers.");
            }

            var (classDef, className) = FlatBufferVectorHelpers.CreateFlatBufferVectorOfUnionSubclass(
                this.ItemTypeModel,
                context);

            FlatSharpInternal.Assert(!string.IsNullOrEmpty(context.TableFieldContextVariableName), "expecting table field context");

            string createFlatBufferVector =
            $@"new {className}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName}.offset0 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset0), 
                    {context.OffsetVariableName}.offset1 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset1),
                    {context.TableFieldContextVariableName})";

            string body = $"return ({createFlatBufferVector}).ToArray();";

            return new CodeGeneratedMethod(body) { ClassDefinition = classDef };
        }

        public override Type OnInitialize()
        {
            FlatSharpInternal.Assert(this.ClrType.IsArray, $"Array vectors must be arrays. Type = {this.ClrType.FullName}.");
            FlatSharpInternal.Assert(this.ClrType.GetArrayRank() == 1, "Array vectors may only be single-dimension.");

            return this.ClrType.GetElementType()!;
        }
    }
}
