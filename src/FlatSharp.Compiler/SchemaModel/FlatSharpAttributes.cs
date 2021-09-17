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
    using System.Collections.Generic;
    using FlatSharp.Attributes;

    public class FlatSharpAttributes : IFlatSharpAttributes
    {
        private readonly IIndexedVector<string, Schema.KeyValue> rawAttributes;
        private readonly Dictionary<string, object?> parsed;

        public FlatSharpAttributes(IIndexedVector<string, Schema.KeyValue>? attrs)
        {
            this.parsed = new();
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
            if (this.parsed.TryGetValue(key, out object? obj))
            {
                return (bool?)obj;
            }

            bool? result;
            if (this.rawAttributes.TryGetValue(key, out Schema.KeyValue? value))
            {
                FlatSharpInternal.Assert(value.Value is not null, "Not expecting 'null' values.");

                result = value.Value.ToLowerInvariant() switch
                {
                    // seems that "0" can mean "included but no value set".
                    "true" or "0" => true,
                    "false" => false,
                    _ => null,
                };

                if (result is null)
                {
                    ErrorContext.Current.RegisterError($"Unable to parse '{key}' value '{value.Value}' as a boolean. Expecting 'true' or 'false'.");
                    result = true;
                }
            }
            else
            {
                result = null;
            }

            this.parsed[key] = result;
            return result;
        }

        private TEnum? TryParseEnum<TEnum>(string key, TEnum defaultIfPresent) where TEnum : struct, Enum
        {
            if (this.parsed.TryGetValue(key, out object? obj))
            {
                return (TEnum?)obj;
            }

            TEnum? result;

            if (this.rawAttributes.TryGetValue(key, out Schema.KeyValue? value))
            {
                FlatSharpInternal.Assert(value.Value is not null, "Not expecting null");

                if (value.Value == "0") // seems to indicate that value isn't present.
                {
                    result = defaultIfPresent;
                }
                else if (Enum.TryParse<TEnum>(value.Value, ignoreCase: true, out TEnum temp))
                {
                    result = temp;
                }
                else
                {
                    string suggestions = string.Join(", ", Enum.GetNames(typeof(TEnum)));
                    ErrorContext.Current.RegisterError($"Unable to parse '{key}' value '{value.Value}'. Valid values are: {suggestions}.");
                    result = defaultIfPresent;
                }
            }
            else
            {
                result = null;
            }

            this.parsed[key] = result;
            return result;
        }

        /* Works fine -- just not used.
        private long? TryParseLong(string key)
        {
            if (this.parsed.TryGetValue(key, out object? obj))
            {
                return (long?)obj;
            }

            long? result;

            if (this.rawAttributes.TryGetValue(key, out Schema.KeyValue? value))
            {
                FlatSharpInternal.Assert(value.Value is not null, "Not expecting null");

                if (value.Value == "0") // seems to indicate that value isn't present.
                {
                    ErrorContext.Current.RegisterError($"Metadata key '{key}' was present, but did not include a value. If you intended to specify \"0\", please use \"00\".");
                    result = 0;
                }
                else if (long.TryParse(value.Value, out var temp))
                {
                    result = temp;
                }
                else
                {
                    ErrorContext.Current.RegisterError($"Unable to parse '{key}' value '{value.Value}' as an int64.");
                    result = 0;
                }
            }
            else
            {
                result = null;
            }

            this.parsed[key] = result;
            return result;
        }
        */
    }
}
