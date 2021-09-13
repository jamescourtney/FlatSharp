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

namespace FlatSharp.Compiler.SchemaModel
{
    using FlatSharp.Attributes;
    using System;

    public class FlatSharpAttributeValidator
    {
        public FlatSharpAttributeValidator(FlatBufferSchemaElementType elementType, string fullName)
        {
            this.ElementType = elementType;
            this.FullName = fullName;
        }

        public FlatBufferSchemaElementType ElementType { get; }

        public string FullName { get; }

        public void Validate(IFlatSharpAttributes attributes)
        {
            void RegisterError(string key, AttributeValidationResult result, object value)
            {
                if (!result.IsValid)
                {
                    ErrorContext.Current.RegisterError(result.ToString(this.ElementType, key, value?.ToString() ?? string.Empty));
                }
            }

            if (attributes.NonVirtual is not null)
            {
                RegisterError(MetadataKeys.NonVirtualProperty, this.NonVirtualValidator(attributes.NonVirtual.Value), attributes.NonVirtual.Value);
            }

            if (attributes.DeserializationOption is not null)
            {
                RegisterError(MetadataKeys.SerializerKind, this.DeserializationOptionValidator(attributes.DeserializationOption.Value), attributes.DeserializationOption.Value);
            }

            if (attributes.SortedVector is not null)
            {
                RegisterError(MetadataKeys.SortedVector, this.SortedVectorValidator(attributes.SortedVector.Value), attributes.SortedVector.Value);
            }

            if (attributes.SharedString is not null)
            {
                RegisterError(MetadataKeys.SharedString, this.SharedStringValidator(attributes.SharedString.Value), attributes.SharedString.Value);
            }

            if (attributes.DefaultCtorKind is not null)
            {
                RegisterError(MetadataKeys.DefaultConstructorKind, this.DefaultConstructorValidator(attributes.DefaultCtorKind.Value), attributes.DefaultCtorKind.Value);
            }

            if (attributes.SetterKind is not null)
            {
                RegisterError(MetadataKeys.Setter, this.SetterKindValidator(attributes.SetterKind.Value), attributes.SetterKind.Value);
            }

            if (attributes.ForceWrite is not null)
            {
                RegisterError(MetadataKeys.ForceWrite, this.ForceWriteValidator(attributes.ForceWrite.Value), attributes.ForceWrite.Value);
            }

            if (attributes.UnsafeStructVector is not null)
            {
                RegisterError(MetadataKeys.UnsafeValueStructVector, this.UnsafeStructVectorValidator(attributes.UnsafeStructVector.Value), attributes.UnsafeStructVector.Value);
            }

            if (attributes.MemoryMarshalBehavior is not null)
            {
                RegisterError(MetadataKeys.MemoryMarshalBehavior, this.MemoryMarshalValidator(attributes.MemoryMarshalBehavior.Value), attributes.MemoryMarshalBehavior.Value);
            }

            if (attributes.VectorKind is not null)
            {
                RegisterError(MetadataKeys.VectorKind, this.VectorTypeValidator(attributes.VectorKind.Value), attributes.VectorKind.Value);
            }

            if (attributes.WriteThrough is not null)
            {
                RegisterError(MetadataKeys.WriteThrough, this.WriteThroughValidator(attributes.WriteThrough.Value), attributes.WriteThrough.Value);
            }

            if (attributes.RpcInterface is not null)
            {
                RegisterError(MetadataKeys.RpcInterface, this.RpcInterfaceValidator(attributes.RpcInterface.Value), attributes.RpcInterface.Value);
            }

            if (attributes.StreamingType is not null)
            {
                RegisterError(MetadataKeys.Streaming, this.StreamingTypeValidator(attributes.StreamingType.Value), attributes.StreamingType.Value);
            }
        }

        public Func<bool, AttributeValidationResult> NonVirtualValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<VectorType, AttributeValidationResult> VectorTypeValidator { get; set; } = (v) => AttributeValidationResult.NeverValid;

        public Func<FlatBufferDeserializationOption, AttributeValidationResult> DeserializationOptionValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> SortedVectorValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> SharedStringValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<DefaultConstructorKind, AttributeValidationResult> DefaultConstructorValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<SetterKind, AttributeValidationResult> SetterKindValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> ForceWriteValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> UnsafeStructVectorValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<MemoryMarshalBehavior, AttributeValidationResult> MemoryMarshalValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> WriteThroughValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<bool, AttributeValidationResult> RpcInterfaceValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;

        public Func<RpcStreamingType, AttributeValidationResult> StreamingTypeValidator { get; set; } = (b) => AttributeValidationResult.NeverValid;
    }
}
