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
    using FlatSharp.Internal;

    /// <summary>
    /// Defines a type model for a vectors of unions represented as an array segment.
    /// </summary>
    public class ArraySegmentVectorOfUnionTypeModel : BaseVectorOfUnionTypeModel
    {
        private ITypeModel itemTypeModel;

        public ArraySegmentVectorOfUnionTypeModel(Type clrType, TypeModelContainer container)
            : base(clrType, container)
        {
            this.itemTypeModel = null!;
        }

        public override ITypeModel ItemTypeModel => this.itemTypeModel;

        public override string LengthPropertyName => "Count";

        public override bool IsRecyclable => true;

        protected override string Indexer(string index) => $".{nameof(ArraySegmentExtensions.Get)}({index})";

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            var (classDef, className) = FlatBufferVectorHelpers.CreateFlatBufferVectorOfUnionSubclass(
                this.itemTypeModel.ClrType,
                this.itemTypeModel,
                context.InputBufferTypeName,
                context.MethodNameMap[this.itemTypeModel.ClrType]);

            string body = $@"
                var vector = new {className}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName}.offset0 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset0), 
                    {context.OffsetVariableName}.offset1 + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}.offset1));

                int count = vector.Count;
                var rented = System.Buffers.ArrayPool<{this.ItemTypeModel.GetCompilableTypeName()}>.Shared.Rent(count);
                vector.CopyTo(rented, 0);
                return new ArraySegment<{this.ItemTypeModel.GetCompilableTypeName()}>(rented, 0, count);
            ";

            return new CodeGeneratedMethod(body) { ClassDefinition = classDef };
        }

        public override CodeGeneratedMethod CreateRecycleMethodBody(RecycleCodeGenContext context)
        {
            var baseMethod = base.CreateRecycleMethodBody(context);

            return baseMethod with
            {
                MethodBody = 
                    baseMethod.MethodBody + $@"
                    var array = {context.ValueVariableName}.Array;
                    if (!(array is null))
                    {{
                        System.Buffers.ArrayPool<{this.ItemTypeModel.GetCompilableTypeName()}>.Shared.Return(array, true);
                    }}",
                IsMethodInline = false,
            };
        }

        public override void OnInitialize()
        {
            FlatSharpInternal.Assert(
                this.ClrType.IsGenericType && this.ClrType.GetGenericTypeDefinition() == typeof(ArraySegment<>),
                "Array segment vectors must be array segments");

            this.itemTypeModel = this.typeModelContainer.CreateTypeModel(this.ClrType.GetGenericArguments()[0]);

            FlatSharpInternal.Assert(this.itemTypeModel.SchemaType == FlatBufferSchemaType.Union, "Union vectors can't contain non-union elements.");
        }
    }
}
