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
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines a vector of union type model.
    /// </summary>
    public abstract class BaseVectorOfUnionTypeModel : RuntimeTypeModel
    {
        internal BaseVectorOfUnionTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Vector;

        /// <summary>
        /// Layout of the vtable.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout =>
            new PhysicalLayoutElement[] 
            { 
                new PhysicalLayoutElement(sizeof(uint), sizeof(uint)),  // discriminator vector
                new PhysicalLayoutElement(sizeof(uint), sizeof(uint)),  // offset vector
            }.ToImmutableArray();

        /// <summary>
        /// Vectors are arbitrary in length.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Vectors can't be part of structs.
        /// </summary>
        public override bool IsValidStructMember => false;

        /// <summary>
        /// Vectors can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Vectors can't be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => false;

        /// <summary>
        /// Vectors can't be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => false;

        /// <summary>
        /// Vector's can't be keys of sorted vectors.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Gets the type model for this vector's elements.
        /// </summary>
        public abstract ITypeModel ItemTypeModel { get; }

        /// <summary>
        /// The name of the length property of this vector type.
        /// </summary>
        public abstract string LengthPropertyName { get; }

        /// <summary>
        /// Vectors are by-reference.
        /// </summary>
        public override bool SerializesInline => false;

        public override bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel)
        {
            typeModel = this.ItemTypeModel;
            return true;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            // 2 vectors.
            int baseSize = 2 * (sizeof(int) + SerializationHelpers.GetMaxPadding(sizeof(int)));

            string body =
            $@"
                int count = {context.ValueVariableName}.{this.LengthPropertyName};
                int length = {baseSize} + (count * (sizeof(byte) + sizeof(int)));

                for (int i = 0; i < count; ++i)
                {{
                      var current = {context.ValueVariableName}[i];
                      {this.GetThrowIfNullStatement("current")}
                      length += {context.MethodNameMap[this.ItemTypeModel.ClrType]}(current);
                }}

                return length;";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var type = this.ClrType;
            var itemTypeModel = this.ItemTypeModel;

            string innerInvocation = context.With(
                valueVariableName: "current",
                offsetVariableName: "ref tuple").GetSerializeInvocation(itemTypeModel.ClrType);

            string body = $@"
                int count = {context.ValueVariableName}.{this.LengthPropertyName};
                int discriminatorVectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}(sizeof(byte), count, sizeof(byte));
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}.offset0, discriminatorVectorOffset);
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, discriminatorVectorOffset);
                discriminatorVectorOffset += sizeof(int);

                int offsetVectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}(sizeof(int), count, sizeof(int));
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}.offset1, offsetVectorOffset);
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, offsetVectorOffset);
                offsetVectorOffset += sizeof(int);

                for (int i = 0; i < count; ++i)
                {{
                      var current = {context.ValueVariableName}[i];
                      {this.GetThrowIfNullStatement("current")}

                      var tuple = (discriminatorVectorOffset, offsetVectorOffset);
                      {innerInvocation};

                      discriminatorVectorOffset++;
                      offsetVectorOffset += sizeof(int);
                }}";

            return new CodeGeneratedMethod(body);
        }

        public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
        {
            string parameters;
            if (this.ItemTypeModel.ClrType.IsValueType)
            {
                parameters = context.ItemVariableName;
            }
            else
            {
                parameters = $"{context.ItemVariableName}, {context.MethodNameMap[this.ItemTypeModel.ClrType]}";
            }

            string body =  $"return {nameof(VectorCloneHelpers)}.{nameof(VectorCloneHelpers.Clone)}<{this.ItemTypeModel.GetCompilableTypeName()}>({parameters});";
            return new CodeGeneratedMethod(body)
            {
                IsMethodInline = true,
            };
        }

        public sealed override void Initialize()
        {
            base.Initialize();
            this.OnInitialize();
        }

        public abstract void OnInitialize();

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            seenTypes.Add(typeof(byte));

            if (seenTypes.Add(this.ItemTypeModel.ClrType))
            {
                this.ItemTypeModel.TraverseObjectGraph(seenTypes);
            }
        }

        private string GetThrowIfNullStatement(string variableName)
        {
            if (this.ItemTypeModel.IsNonNullableClrValueType())
            {
                // can't be null.
                return string.Empty;
            }

            return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({variableName});";
        }
    }
}
