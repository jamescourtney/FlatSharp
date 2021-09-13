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

    /// <summary>
    /// Defines a vector type model for a sorted vector that looks like a Dictionary.
    /// </summary>
    public class IndexedVectorTypeModel : BaseVectorTypeModel
    {
        private ITypeModel keyTypeModel;
        private ITypeModel valueTypeModel;
        private TableMemberModel keyMemberModel;

        internal IndexedVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
            this.keyTypeModel = null!;
            this.valueTypeModel = null!;
            this.keyMemberModel = null!;
        }

        public override string LengthPropertyName => nameof(IIndexedVector<string, string>.Count);

        public override Type OnInitialize()
        {
            FlatSharpInternal.Assert(
                this.ClrType.IsGenericType && this.ClrType.GetGenericTypeDefinition() == typeof(IIndexedVector<,>),
                $"Indexed vectors must be of type IIndexedVector. Type = {this.GetCompilableTypeName()}.");

            Type keyType = this.ClrType.GetGenericArguments()[0];
            Type valueType = this.ClrType.GetGenericArguments()[1];

            this.keyTypeModel = this.typeModelContainer.CreateTypeModel(keyType);
            this.valueTypeModel = this.typeModelContainer.CreateTypeModel(valueType);

            if (this.valueTypeModel.SchemaType != FlatBufferSchemaType.Table)
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"Indexed vector values must be flatbuffer tables. Type = '{this.valueTypeModel.GetCompilableTypeName()}'");
            }

            if (!this.valueTypeModel.TryGetTableKeyMember(out TableMemberModel? tempKeyMemberModel))
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"Indexed vector values must have a property with the key attribute defined. Table = '{this.valueTypeModel.GetCompilableTypeName()}'");
            }
            else
            {
                this.keyMemberModel = tempKeyMemberModel;
            }

            if (!this.keyMemberModel.ItemTypeModel.TryGetSpanComparerType(out _))
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"FlatSharp indexed vector keys must supply a span comparer. KeyType = '{this.keyMemberModel.ItemTypeModel.GetCompilableTypeName()}'.");
            }

            if (keyMemberModel.ItemTypeModel.ClrType != this.keyTypeModel.ClrType)
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"FlatSharp indexed vector keys must have the same type as the key of the value. KeyType = {this.keyTypeModel.GetCompilableTypeName()}, Value Key Type = '{this.valueTypeModel.GetCompilableTypeName()}'.");
            }

            return valueType;
        }

        public override void AdjustTableMember(TableMemberModel source)
        {
            // Force the vector to be sorted.
            source.IsSortedVector = true;
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            ValidateWriteThrough(
                writeThroughSupported: false,
                this,
                context.AllFieldContexts,
                context.Options);

            string body;
            string keyTypeName = CSharpHelpers.GetGlobalCompilableTypeName(this.keyTypeModel.ClrType);
            string valueTypeName = CSharpHelpers.GetGlobalCompilableTypeName(this.valueTypeModel.ClrType);

            (string vectorClassDef, string vectorClassName) = FlatBufferVectorHelpers.CreateFlatBufferVectorSubclass(
                this.valueTypeModel,
                context);

            FlatSharpInternal.Assert(!string.IsNullOrEmpty(context.TableFieldContextVariableName), "field context was null/empty");

            string createFlatBufferVector =
            $@"new {vectorClassName}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}), 
                    {this.PaddedMemberInlineSize},
                    {context.TableFieldContextVariableName})";

            string mutable = context.Options.GenerateMutableObjects.ToString().ToLowerInvariant();
            if (context.Options.GreedyDeserialize)
            {
                // Eager indexed vector.
                body = $@"return new {nameof(IndexedVector<string, string>)}<{keyTypeName}, {valueTypeName}>({createFlatBufferVector}, {mutable});";
            }
            else if (context.Options.Lazy)
            {
                // Lazy indexed vector.
                body = $@"return new {nameof(FlatBufferIndexedVector<string, string, IInputBuffer>)}<{keyTypeName}, {valueTypeName}, {context.InputBufferTypeName}>({createFlatBufferVector});";
            }
            else
            {
                FlatSharpInternal.Assert(context.Options.Progressive, "expecting progressive");
                body = $@"return new {nameof(FlatBufferProgressiveIndexedVector<string, string, IInputBuffer>)}<{keyTypeName}, {valueTypeName}, {context.InputBufferTypeName}>({createFlatBufferVector});";
            }

            return new CodeGeneratedMethod(body) { IsMethodInline = true, ClassDefinition = vectorClassDef };
        }

        protected override string CreateLoop(
            FlatBufferSerializerOptions options,
            string vectorVariableName,
            string numberofItemsVariableName,
            string expectedVariableName,
            string body)
        {
            return $@"
                foreach (var kvp in {vectorVariableName})
                {{
                    var {expectedVariableName} = kvp.Value;
                    {body}
                }}";
        }

        public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
        {
            string parameters = $"{context.ItemVariableName}, {context.MethodNameMap[this.ItemTypeModel.ClrType]}";
            string genericArgs = $"{this.keyTypeModel.GetCompilableTypeName()}, {this.ItemTypeModel.GetCompilableTypeName()}";

            string body = $"return {nameof(VectorCloneHelpers)}.{nameof(VectorCloneHelpers.Clone)}<{genericArgs}>({parameters});";
            return new CodeGeneratedMethod(body)
            {
                IsMethodInline = true,
            };
        }
    }
}
