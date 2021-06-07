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
    using System.Text;

    /// <summary>
    /// Defines a vector type model for an ArraySegment vector.
    /// </summary>
    public class ArraySegmentVectorTypeModel : BaseVectorTypeModel
    {
        private ITypeModel itemTypeModel;

        internal ArraySegmentVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
            this.itemTypeModel = null!;
        }

        public override ITypeModel ItemTypeModel => this.itemTypeModel;

        public override string LengthPropertyName => nameof(ArraySegment<int>.Count);

        public override bool IsRecyclable => true;

        public override void OnInitialize()
        {
            FlatSharpInternal.Assert(
                this.ClrType.IsGenericType && this.ClrType.GetGenericTypeDefinition() == typeof(ArraySegment<>),
                "Array segment vectors must be array segments");

            this.itemTypeModel = this.typeModelContainer.CreateTypeModel(this.ClrType.GetGenericArguments()[0]);

            if (!this.itemTypeModel.IsValidVectorMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Type '{this.itemTypeModel.GetCompilableTypeName()}' is not a valid vector member.");
            }
        }

        protected override string CreateLoop(
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string numberofItemsVariableName,
            string expectedVariableName,
            string body) => $@"
                for (int i = 0; i < {vectorVariableName}.{this.LengthPropertyName}; i = unchecked(i + 1))
                {{
                    var {expectedVariableName} = {vectorVariableName}.Get(i);
                    {body}
                }}";

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            string body;

            FlatSharpInternal.Assert(this.itemTypeModel is not null, "ItemTypeModel null");

            (string vectorClassDef, string vectorClassName) = (string.Empty, string.Empty);

            if (this.itemTypeModel.ClrType == typeof(byte))
            {
                // can handle this as memory.
                body = $@"
                    var memory = {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadByteMemoryBlock)}({context.OffsetVariableName});
                    var rented = System.Buffers.ArrayPool<byte>.Shared.Rent(memory.{nameof(Memory<byte>.Length)});
                    memory.CopyTo(rented);
                    return new ArraySegment<byte>(rented, 0, memory.{nameof(Memory<byte>.Length)});
                ";
            }
            else
            {
                (vectorClassDef, vectorClassName) = 
                    FlatBufferVectorHelpers.CreateFlatBufferVectorSubclass(
                        this.itemTypeModel.ClrType,
                        context.InputBufferTypeName,
                        context.MethodNameMap[this.itemTypeModel.ClrType]);

                body = $@"
                    var vector = new {vectorClassName}<{context.InputBufferTypeName}>(
                        {context.InputBufferVariableName}, 
                        {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}), 
                        {this.PaddedMemberInlineSize});

                    int count = vector.Count;
                    var rented = System.Buffers.ArrayPool<{this.ItemTypeModel.GetCompilableTypeName()}>.Shared.Rent(count);
                    vector.CopyTo(rented, 0);
                    return new ArraySegment<{this.ItemTypeModel.GetCompilableTypeName()}>(rented, 0, count);
                ";
            }

            return new CodeGeneratedMethod(body) { ClassDefinition = vectorClassDef };
        }

        public override CodeGeneratedMethod CreateRecycleMethodBody(RecycleCodeGenContext context)
        {
            var baseMethod = base.CreateRecycleMethodBody(context);

            return baseMethod with
            {
                MethodBody = baseMethod.MethodBody + $@"
                var array = {context.ValueVariableName}.Array;
                if (!(array is null))
                {{
                    System.Buffers.ArrayPool<{this.ItemTypeModel.GetCompilableTypeName()}>.Shared.Return(array, true);
                }}",
                IsMethodInline = false,
            };
        }
    }
}
