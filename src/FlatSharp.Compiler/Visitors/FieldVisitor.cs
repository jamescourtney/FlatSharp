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

namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Misc;

    internal class FieldVisitor : FlatBuffersBaseVisitor<FieldDefinition?>
    {
        private readonly TableOrStructDefinition parent;

        public FieldVisitor(TableOrStructDefinition parent)
        {
            this.parent = parent;
        }

        public override FieldDefinition? VisitField_decl([NotNull] FlatBuffersParser.Field_declContext context)
        {
            string name = context.IDENT().GetText();

            return ErrorContext.Current.WithScope(name, () =>
            {
                Dictionary<string, string?> metadata = new MetadataVisitor().VisitMetadata(context.metadata());

                var (fieldType, vectorType) = GetFbsFieldType(context, metadata);

                var definition = new FieldDefinition(this.parent, name, fieldType)
                {
                    VectorType = vectorType
                };

                string? defaultValue = context.defaultValue_decl()?.GetText();
                if (defaultValue == "null")
                {
                    definition.IsOptionalScalar = true;
                }
                else if (!string.IsNullOrEmpty(defaultValue))
                {
                    definition.DefaultValue = defaultValue;
                }

                definition.Deprecated = metadata.ParseBooleanMetadata("deprecated");
                definition.IsKey = metadata.ParseBooleanMetadata("key");
                definition.NonVirtual = metadata.ParseNullableBooleanMetadata("nonVirtual");
                definition.SortedVector = metadata.ParseBooleanMetadata("sortedvector"); 
                definition.SharedString = metadata.ParseBooleanMetadata("sharedstring");

                this.ParseIdMetadata(definition, metadata);

                definition.SetterKind = metadata.ParseMetadata(
                    "setter",
                    ParseSetterKind,
                    SetterKind.Public,
                    SetterKind.Public);

                // Attributes from FlatBuffers that we don't support.
                string[] unsupportedAttributes =
                {
                    "required", "force_align", "bit_flags", "flexbuffer", "hash", "original_order"
                };

                foreach (var unsupportedAttribute in unsupportedAttributes)
                {
                    if (metadata.ContainsKey(unsupportedAttribute))
                    {
                        ErrorContext.Current?.RegisterError($"FlatSharpCompiler does not support the '{unsupportedAttribute}' attribute in FBS files.");
                    }
                }

                return definition;
            });
        }

        private void ParseIdMetadata(
            FieldDefinition definition,
            IDictionary<string, string?> metadata)
        {
            if (!metadata.TryParseIntegerMetadata("id", out int index))
            {
                if (index == MetadataHelpers.DefaultIntegerAttributeValueIfPresent)
                {
                    ErrorContext.Current?.RegisterError("Value of 'id' attribute should be set if attribute present.");
                }

                return;
            }

            if (index < 0)
            {
                ErrorContext.Current?.RegisterError($"Value of 'id' attribute {index} of '{definition.Name}' field is negative.");
            }

            definition.Index = index;
            definition.IsIndexSetManually = true;
        }

        private static bool ParseSetterKind(string value, out SetterKind setter)
        {
            return Enum.TryParse<SetterKind>(value, true, out setter);
        }

        private (string fieldType, VectorType vectorType) GetFbsFieldType(FlatBuffersParser.Field_declContext context, Dictionary<string, string?> metadata)
        {
            string fbsFieldType = context.type().GetText();
            VectorType vectorType = VectorType.None;

            if (fbsFieldType.StartsWith("["))
            {
                vectorType = VectorType.IList;

                // Trim the starting and ending square brackets.
                fbsFieldType = fbsFieldType.Substring(1, fbsFieldType.Length - 2);

                if (metadata.TryGetValue("vectortype", out string? vectorTypeString))
                {
                    if (!Enum.TryParse<VectorType>(vectorTypeString, true, out vectorType))
                    {
                        ErrorContext.Current?.RegisterError(
                            $"Unable to parse '{vectorTypeString}' as a vector type. Valid choices are: {string.Join(", ", Enum.GetNames(typeof(VectorType)))}.");
                    }
                }
            }
            else if (metadata.ContainsKey("vectortype"))
            {
                ErrorContext.Current?.RegisterError(
                    $"Non-vectors may not have the 'vectortype' attribute.");
            }

            return (fbsFieldType, vectorType);
        }
    }
}
