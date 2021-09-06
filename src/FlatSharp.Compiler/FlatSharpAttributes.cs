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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public enum FlatSharpAttributeTarget
    {
        Table,
        Struct,
        ValueStruct,
        Union,
        Enum,
        RpcService,

        TableField,
        StructField,
        ValueStructField,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FlatSharpAttributeUsageAttribute : Attribute
    {
        public FlatSharpAttributeUsageAttribute(string key, params FlatSharpAttributeTarget[] usages)
        {
            this.Key = key;
            this.Targets = usages;
        }

        public string Key { get; }

        public FlatSharpAttributeTarget[] Targets { get; }
    }

    internal class FlatSharpAttributes
    {
        private readonly IIndexedVector<string, Schema.KeyValue> rawAttributes;

        public FlatSharpAttributes(IIndexedVector<string, Schema.KeyValue>? attrs)
        {
            this.rawAttributes = attrs ?? new IndexedVector<string, Schema.KeyValue>();
        }

        [FlatSharpAttributeUsage(MetadataKeys.SerializerKind, FlatSharpAttributeTarget.Table)]
        public FlatBufferDeserializationOption? DeserializationOption => this.TryParseEnum(MetadataKeys.SerializerKind, FlatBufferDeserializationOption.Default);

        [FlatSharpAttributeUsage(
            MetadataKeys.NonVirtualProperty,
            FlatSharpAttributeTarget.Table,
            FlatSharpAttributeTarget.Struct,
            FlatSharpAttributeTarget.TableField,
            FlatSharpAttributeTarget.StructField)]
        public bool? NonVirtual => this.TryParseBoolean(MetadataKeys.NonVirtualProperty);

        [FlatSharpAttributeUsage(MetadataKeys.SortedVector, FlatSharpAttributeTarget.TableField)]
        public bool? SortedVector => this.TryParseBoolean(MetadataKeys.SortedVector);

        [FlatSharpAttributeUsage(MetadataKeys.SharedString, FlatSharpAttributeTarget.TableField)]
        public bool? SharedString => this.TryParseBoolean(MetadataKeys.SharedString);

        [FlatSharpAttributeUsage(MetadataKeys.DefaultConstructorKind, FlatSharpAttributeTarget.Table)]
        public DefaultConstructorKind? DefaultCtorKind => this.TryParseEnum(MetadataKeys.DefaultConstructorKind, DefaultConstructorKind.Public);

        [FlatSharpAttributeUsage(MetadataKeys.VectorKind, FlatSharpAttributeTarget.TableField)]
        public VectorType? VectorKind => this.TryParseEnum(MetadataKeys.VectorKind, VectorType.IList);

        [FlatSharpAttributeUsage(MetadataKeys.Setter, FlatSharpAttributeTarget.TableField, FlatSharpAttributeTarget.StructField)]
        public SetterKind? SetterKind => this.TryParseEnum(MetadataKeys.Setter, Compiler.SetterKind.Public);

        [FlatSharpAttributeUsage(MetadataKeys.Setter, FlatSharpAttributeTarget.TableField, FlatSharpAttributeTarget.Table)]
        public bool? ForceWrite => this.TryParseBoolean(MetadataKeys.ForceWrite);

        [FlatSharpAttributeUsage(MetadataKeys.FileIdentifier, FlatSharpAttributeTarget.Table)]
        public string? FileId
        {
            get
            {
                if (this.rawAttributes.TryGetValue(MetadataKeys.FileIdentifier, out var value))
                {
                    return value.Value;
                }

                return null;
            }
        }

        [FlatSharpAttributeUsage(MetadataKeys.UnsafeValueStructVector, FlatSharpAttributeTarget.ValueStructField)]
        public bool? UnsafeStructVector => this.TryParseBoolean(MetadataKeys.UnsafeValueStructVector);

        [FlatSharpAttributeUsage(MetadataKeys.MemoryMarshalBehavior, FlatSharpAttributeTarget.ValueStruct)]
        public MemoryMarshalBehavior? MemoryMarshalBehavior => this.TryParseEnum(MetadataKeys.MemoryMarshalBehavior, Attributes.MemoryMarshalBehavior.Default);

        [FlatSharpAttributeUsage(MetadataKeys.WriteThrough, FlatSharpAttributeTarget.Struct, FlatSharpAttributeTarget.StructField)]
        public bool? WriteThrough => this.TryParseBoolean(MetadataKeys.WriteThrough);

        public void Validate(FlatSharpAttributeTarget item)
        {
            foreach (var prop in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = prop.GetCustomAttribute<FlatSharpAttributeUsageAttribute>();
                FlatSharpInternal.Assert(attr is not null, "Missing attribute");

                bool isNull = prop.GetValue(this) is null;

                if (!isNull && !attr.Targets.Contains(item))
                {
                    // Value set on an item that it wasn't allowed for.
                    ErrorContext.Current.RegisterError($"Attribute '{attr.Key}' is not allowed on {item} members.");
                }
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
