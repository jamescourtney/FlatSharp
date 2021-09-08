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

    public class SupportTestResult
    {
        private const string ElementTypeSubs = "__ELEMENT_TYPE__";
        private const string AttributeNameSubs = "__ATTR_NAME__";
        private const string AttributeValueSubs = "__ATTR_VALUE__";

        public static readonly SupportTestResult Valid = new(null, true);

        public static readonly SupportTestResult NeverValid = new SupportTestResult($"The attribute {AttributeNameSubs} is never valid on {ElementTypeSubs} elements.", false);

        public static readonly SupportTestResult ValueInvalid = new SupportTestResult($"The attribute {AttributeNameSubs} value {AttributeValueSubs} is not valid on {ElementTypeSubs} elements.", false);


        private SupportTestResult(string? message, bool isValid)
        {
            this.Message = message ?? string.Empty;
            this.IsValid = isValid;
        }

        public string Message { get; }

        public bool IsValid { get; }

        public string ToString(FlatBufferSchemaElementType type, string attributeName, string attributeValue)
        {
            return this.Message
                .Replace(AttributeNameSubs, $"'{attributeName}'")
                .Replace(ElementTypeSubs, type.ToString())
                .Replace(AttributeValueSubs, attributeValue);
        }
    }

    public interface IFlatSharpAttributeSupportTester
    {
        FlatBufferSchemaElementType ElementType { get; }

        string FullName { get; }

        SupportTestResult SupportsNonVirtual(bool nonVirtualValue);

        SupportTestResult SupportsVectorType(VectorType vectorType);

        SupportTestResult SupportsDeserializationOption(FlatBufferDeserializationOption option);

        SupportTestResult SupportsSortedVector(bool sortedVectorOption);

        SupportTestResult SupportsSharedString(bool sharedStringOption);

        SupportTestResult SupportsDefaultCtorKindOption(DefaultConstructorKind kind);

        SupportTestResult SupportsSetterKind(SetterKind setterKind);

        SupportTestResult SupportsForceWrite(bool forceWriteOption);

        SupportTestResult SupportsUnsafeStructVector(bool unsafeStructVector);

        SupportTestResult SupportsMemoryMarshal(MemoryMarshalBehavior option);

        SupportTestResult SupportsWriteThrough(bool writeThroughOption);

        SupportTestResult SupportsRpcInterface(bool supportsRpcInterface);

        SupportTestResult SupportsStreamingType(RpcStreamingType streamingType);
    }

    public static class IFlatSharpAttributeSupportTesterExtensions
    {
        public static void ValidateAttributes(this IFlatSharpAttributeSupportTester testable, IFlatSharpAttributes attrs)
        {
            void RegisterError(string key, SupportTestResult result, object value)
            {
                if (!result.IsValid)
                {
                    ErrorContext.Current.RegisterError(result.ToString(testable.ElementType, key, value?.ToString() ?? string.Empty));
                }
            }

            if (attrs.NonVirtual is not null)
            {
                RegisterError(MetadataKeys.NonVirtualProperty, testable.SupportsNonVirtual(attrs.NonVirtual.Value), attrs.NonVirtual.Value);
            }

            if (attrs.DeserializationOption is not null)
            {
                RegisterError(MetadataKeys.SerializerKind, testable.SupportsDeserializationOption(attrs.DeserializationOption.Value), attrs.DeserializationOption.Value);
            }

            if (attrs.SortedVector is not null)
            {
                RegisterError(MetadataKeys.SortedVector, testable.SupportsSortedVector(attrs.SortedVector.Value), attrs.SortedVector.Value);
            }

            if (attrs.SharedString is not null)
            {
                RegisterError(MetadataKeys.SharedString, testable.SupportsSharedString(attrs.SharedString.Value), attrs.SharedString.Value);
            }

            if (attrs.DefaultCtorKind is not null)
            {
                RegisterError(MetadataKeys.DefaultConstructorKind, testable.SupportsDefaultCtorKindOption(attrs.DefaultCtorKind.Value), attrs.DefaultCtorKind.Value);
            }

            if (attrs.SetterKind is not null)
            {
                RegisterError(MetadataKeys.Setter, testable.SupportsSetterKind(attrs.SetterKind.Value), attrs.SetterKind.Value);
            }

            if (attrs.ForceWrite is not null)
            {
                RegisterError(MetadataKeys.ForceWrite, testable.SupportsForceWrite(attrs.ForceWrite.Value), attrs.ForceWrite.Value);
            }

            if (attrs.UnsafeStructVector is not null)
            {
                RegisterError(MetadataKeys.UnsafeValueStructVector, testable.SupportsUnsafeStructVector(attrs.UnsafeStructVector.Value), attrs.UnsafeStructVector.Value);
            }

            if (attrs.MemoryMarshalBehavior is not null)
            {
                RegisterError(MetadataKeys.MemoryMarshalBehavior, testable.SupportsMemoryMarshal(attrs.MemoryMarshalBehavior.Value), attrs.MemoryMarshalBehavior.Value);
            }

            if (attrs.VectorKind is not null)
            {
                RegisterError(MetadataKeys.VectorKind, testable.SupportsVectorType(attrs.VectorKind.Value), attrs.VectorKind.Value);
            }

            if (attrs.WriteThrough is not null)
            {
                RegisterError(MetadataKeys.WriteThrough, testable.SupportsWriteThrough(attrs.WriteThrough.Value), attrs.WriteThrough.Value);
            }

            if (attrs.RpcInterface is not null)
            {
                RegisterError(MetadataKeys.RpcInterface, testable.SupportsRpcInterface(attrs.RpcInterface.Value), attrs.RpcInterface.Value);
            }

            if (attrs.StreamingType is not null)
            {
                RegisterError(MetadataKeys.Streaming, testable.SupportsStreamingType(attrs.StreamingType.Value), attrs.StreamingType.Value);
            }
        }
    }
}
