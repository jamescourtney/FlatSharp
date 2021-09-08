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
    using System;
    using FlatSharp.Attributes;

    public class FlatSharpAttributes : IFlatSharpAttributes
    {
        private readonly IIndexedVector<string, Schema.KeyValue> rawAttributes;

        public FlatSharpAttributes(IIndexedVector<string, Schema.KeyValue>? attrs)
        {
            this.rawAttributes = attrs ?? new IndexedVector<string, Schema.KeyValue>();

            foreach (var unsupported in MetadataKeys.UnsupportedStandardAttributes)
            {
                if (this.rawAttributes.ContainsKey(unsupported))
                {
                    ErrorContext.Current.RegisterError($"FlatSharp does not support the '{unsupported}' attribute.");
                }
            }
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

        public RpcStreamingType? StreamingType => this.TryParseEnum(MetadataKeys.Streaming, RpcStreamingType.None);

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
                if (value.Value == "0") // seems to indicate that value isn't present.
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
