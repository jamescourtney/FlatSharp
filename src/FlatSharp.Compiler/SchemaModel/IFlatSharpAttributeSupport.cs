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

    public enum FlatBufferSchemaElementType
    {
        Unknown = 0,
        Table = 1,
        Struct = 2,
        ValueStruct = 3,
        Enum = 4,
        Union = 5,
        RpcService = 6,
        TableField = 7,
        StructField = 8,
        ValueStructField = 9,
        StructVector = 10,
        ValueStructVector = 11,
        RpcCall = 12,
    }

    public interface IFlatSharpAttributeSupportTester
    {
        FlatBufferSchemaElementType ElementType { get; }

        string FullName { get; }

        bool SupportsNonVirtual(bool nonVirtualValue);

        bool SupportsVectorType(VectorType vectorType);

        bool SupportsDeserializationOption(FlatBufferDeserializationOption option);

        bool SupportsSortedVector(bool sortedVectorOption);

        bool SupportsSharedString(bool sharedStringOption);

        bool SupportsDefaultCtorKindOption(DefaultConstructorKind kind);

        bool SupportsSetterKind(SetterKind setterKind);

        bool SupportsForceWrite(bool forceWriteOption);

        bool SupportsUnsafeStructVector(bool unsafeStructVector);

        bool SupportsMemoryMarshal(MemoryMarshalBehavior option);

        bool SupportsWriteThrough(bool writeThroughOption);

        bool SupportsRpcInterface(bool supportsRpcInterface);

        bool SupportsStreamingType(RpcStreamingType streamingType);
    }

    public static class IFlatSharpAttributeSupportTesterExtensions
    {
        public static void Validate(this IFlatSharpAttributeSupportTester testable, IFlatSharpAttributes attrs)
        {
            void RegisterError(string key)
            {
                ErrorContext.Current.RegisterError($"Attribute '{key}' declared on '{testable.FullName}' is not valid on {testable.ElementType} elements.");
            }

            if (attrs.NonVirtual is not null && !testable.SupportsNonVirtual(attrs.NonVirtual.Value))
            {
                RegisterError(MetadataKeys.NonVirtualProperty);
            }

            if (attrs.DeserializationOption is not null && !testable.SupportsDeserializationOption(attrs.DeserializationOption.Value))
            {
                RegisterError(MetadataKeys.SerializerKind);
            }

            if (attrs.SortedVector is not null && !testable.SupportsSortedVector(attrs.SortedVector.Value))
            {
                RegisterError(MetadataKeys.SortedVector);
            }

            if (attrs.SharedString is not null && !testable.SupportsSharedString(attrs.SharedString.Value))
            {
                RegisterError(MetadataKeys.SharedString);
            }

            if (attrs.DefaultCtorKind is not null && !testable.SupportsDefaultCtorKindOption(attrs.DefaultCtorKind.Value))
            {
                RegisterError(MetadataKeys.DefaultConstructorKind);
            }

            if (attrs.SetterKind is not null && !testable.SupportsSetterKind(attrs.SetterKind.Value))
            {
                RegisterError(MetadataKeys.Setter);
            }

            if (attrs.ForceWrite is not null && !testable.SupportsForceWrite(attrs.ForceWrite.Value))
            {
                RegisterError(MetadataKeys.ForceWrite);
            }

            if (attrs.UnsafeStructVector is not null && !testable.SupportsUnsafeStructVector(attrs.UnsafeStructVector.Value))
            {
                RegisterError(MetadataKeys.UnsafeValueStructVector);
            }

            if (attrs.MemoryMarshalBehavior is not null && !testable.SupportsMemoryMarshal(attrs.MemoryMarshalBehavior.Value))
            {
                RegisterError(MetadataKeys.MemoryMarshalBehavior);
            }

            if (attrs.VectorKind is not null && !testable.SupportsVectorType(attrs.VectorKind.Value))
            {
                RegisterError(MetadataKeys.VectorKind);
            }

            if (attrs.WriteThrough is not null && !testable.SupportsWriteThrough(attrs.WriteThrough.Value))
            {
                RegisterError(MetadataKeys.WriteThrough);
            }

            if (attrs.RpcInterface is not null && !testable.SupportsRpcInterface(attrs.RpcInterface.Value))
            {
                RegisterError(MetadataKeys.RpcInterface);
            }

            if (attrs.StreamingType is not null && !testable.SupportsStreamingType(attrs.StreamingType.Value))
            {
                RegisterError(MetadataKeys.Streaming);
            }
        }
    }
}
