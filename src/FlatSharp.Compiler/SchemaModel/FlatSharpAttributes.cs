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

namespace FlatSharp.Compiler
{
    using FlatSharp.Attributes;
    using FlatSharp.Compiler.SchemaModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class FlatSharpAttributes
    {
        private readonly IIndexedVector<string, Schema.KeyValue> rawAttributes;

        public FlatSharpAttributes(IIndexedVector<string, Schema.KeyValue>? attrs)
        {
            this.rawAttributes = attrs ?? new IndexedVector<string, Schema.KeyValue>();
        }

        public FlatBufferDeserializationOption? DeserializationOption => this.TryParseEnum(MetadataKeys.SerializerKind, FlatBufferDeserializationOption.Default);

        public bool? NonVirtual => this.TryParseBoolean(MetadataKeys.NonVirtualProperty);

        public bool? SortedVector => this.TryParseBoolean(MetadataKeys.SortedVector);

        public bool? SharedString => this.TryParseBoolean(MetadataKeys.SharedString);

        public bool? ValueStruct => this.TryParseBoolean(MetadataKeys.ValueStruct);

        public DefaultConstructorKind? DefaultCtorKind => this.TryParseEnum(MetadataKeys.DefaultConstructorKind, DefaultConstructorKind.Public);

        public VectorType? VectorKind => this.TryParseEnum(MetadataKeys.VectorKind, VectorType.IList);

        public SetterKind? SetterKind => this.TryParseEnum(MetadataKeys.Setter, Compiler.SetterKind.Public);

        public bool? ForceWrite => this.TryParseBoolean(MetadataKeys.ForceWrite);

        public bool? UnsafeStructVector => this.TryParseBoolean(MetadataKeys.UnsafeValueStructVector);

        public MemoryMarshalBehavior? MemoryMarshalBehavior => this.TryParseEnum(MetadataKeys.MemoryMarshalBehavior, Attributes.MemoryMarshalBehavior.Default);

        public bool? WriteThrough => this.TryParseBoolean(MetadataKeys.WriteThrough);

        public bool? RpcInterface => this.TryParseBoolean(MetadataKeys.RpcInterface);

        public void Validate(IFlatSharpAttributeSupportTester testable)
        {
            void RegisterError(string key)
            {
                ErrorContext.Current.RegisterError($"Attribute '{key}' declared on '{testable.FullName}' is not valid on {testable.ElementType} elements.");
            }

            if (this.NonVirtual is not null && !testable.SupportsNonVirtual(this.NonVirtual.Value))
            {
                RegisterError(MetadataKeys.NonVirtualProperty);
            }

            if (this.DeserializationOption is not null && !testable.SupportsDeserializationOption(this.DeserializationOption.Value))
            {
                RegisterError(MetadataKeys.SerializerKind);
            }

            if (this.SortedVector is not null && !testable.SupportsSortedVector(this.SortedVector.Value))
            {
                RegisterError(MetadataKeys.SortedVector);
            }

            if (this.SharedString is not null && !testable.SupportsSharedString(this.SharedString.Value))
            {
                RegisterError(MetadataKeys.SharedString);
            }

            if (this.DefaultCtorKind is not null && !testable.SupportsDefaultCtorKindOption(this.DefaultCtorKind.Value))
            {
                RegisterError(MetadataKeys.DefaultConstructorKind);
            }

            if (this.SetterKind is not null && !testable.SupportsSetterKind(this.SetterKind.Value))
            {
                RegisterError(MetadataKeys.Setter);
            }

            if (this.ForceWrite is not null && !testable.SupportsForceWrite(this.ForceWrite.Value))
            {
                RegisterError(MetadataKeys.ForceWrite);
            }

            if (this.UnsafeStructVector is not null && !testable.SupportsUnsafeStructVector(this.UnsafeStructVector.Value))
            {
                RegisterError(MetadataKeys.UnsafeValueStructVector);
            }

            if (this.MemoryMarshalBehavior is not null && !testable.SupportsMemoryMarshal(this.MemoryMarshalBehavior.Value))
            {
                RegisterError(MetadataKeys.MemoryMarshalBehavior);
            }

            if (this.VectorKind is not null && !testable.SupportsVectorType(this.VectorKind.Value))
            {
                RegisterError(MetadataKeys.VectorKind);
            }

            if (this.WriteThrough is not null && !testable.SupportsWriteThrough(this.WriteThrough.Value))
            {
                RegisterError(MetadataKeys.WriteThrough);
            }
        }

        private bool? TryParseBoolean(string key)
        {
            if (this.rawAttributes.TryGetValue(key, out Schema.KeyValue? value))
            {
                if (value.Value is null)
                {
                    return true;
                }

                bool? result = value.Value.ToLowerInvariant() switch 
                {
                    // seems that "0" can mean "included but no value set".
                    "true" => true,
                    "0" => true,
                    "false" => false,
                    _ => null,
                };

                
                if (result is null)
                {
                    ErrorContext.Current.RegisterError($"Unable to parse '{value.Value}' as a boolean. Expecting 'true' or 'false'.");
                    result = true;
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        private TEnum? TryParseEnum<TEnum>(string key, TEnum defaultIfPresent) where TEnum : struct, Enum
        {
            if (this.rawAttributes.TryGetValue(key, out Schema.KeyValue? value))
            {
                if (value.Value is null)
                {
                    return defaultIfPresent;
                }

                if (!Enum.TryParse<TEnum>(value.Value, ignoreCase: true, out var result))
                {
                    ErrorContext.Current.RegisterError($"Unable to parse '{value.Value}' as an instance of {typeof(TEnum).GetCompilableTypeName()}.");
                    return defaultIfPresent;
                }

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
