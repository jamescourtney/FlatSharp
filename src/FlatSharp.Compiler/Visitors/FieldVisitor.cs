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

    internal class FieldVisitor : FlatBuffersBaseVisitor<bool>
    {
        private readonly TableOrStructDefinition parent;

        public FieldVisitor(TableOrStructDefinition parent)
        {
            this.parent = parent;
        }

        public override bool VisitField_decl([NotNull] FlatBuffersParser.Field_declContext context)
        {
            string name = context.IDENT().GetText();

            ErrorContext.Current.WithScope(name, () =>
            {
                Dictionary<string, string?> metadata = new MetadataVisitor().VisitMetadata(context.metadata());
                
                var (fieldType, vectorType, structVectorLength) = GetFbsFieldType(context, metadata);

                var definition = new FieldDefinition(this.parent, name, fieldType)
                {
                    VectorType = vectorType,
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

                if (structVectorLength == null)
                {
                    this.parent.Fields.Add(definition);
                }
                else if (this.parent.IsTable)
                {
                    ErrorContext.Current.RegisterError("Only structs may contain fixed-length vectors");
                }
                else
                {
                    List<string> groupNames = new List<string>();

                    for (int i = 0; i < structVectorLength.Value; ++i)
                    {
                        string name = $"__flatsharp__{definition.Name}_{i}";
                        this.parent.Fields.Add(definition with { Name = name, SetterKind = SetterKind.Public, IsHidden = true });
                        groupNames.Add(name);
                    }

                    this.parent.StructVectors.Add(new StructVectorDefinition(definition.Name, definition.FbsFieldType, definition.SetterKind, groupNames));
                }
            });

            return true;
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

        private (string fieldType, VectorType vectorType, int? structVectorLength) GetFbsFieldType(FlatBuffersParser.Field_declContext context, Dictionary<string, string?> metadata)
        {
            VectorType vectorType = VectorType.None;
            int? structVectorLength = null;
            FlatBuffersParser.Core_typeContext? typeContext = null;

            if (context.type().vector_type() is not null)
            {
                vectorType = VectorType.IList;
                typeContext = context.type().vector_type().core_type();

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

            if (context.type().structvector_type() is not null)
            {
                typeContext = context.type().structvector_type().core_type();
                string toParse = context.type().structvector_type().INTEGER_CONSTANT().GetText();

                if (!int.TryParse(toParse, out var length) || length < 0)
                {
                    ErrorContext.Current?.RegisterError(
                        $"Unable to parse '{toParse}' as a struct vector length. Lengths should be a nonnegative base 10 integer.");
                }
                else
                {
                    structVectorLength = length;
                }
            }

            if (context.type().core_type() is not null)
            {
                typeContext = context.type().core_type();
            }

            if (typeContext is null)
            {
                throw new InvalidOperationException("Flatsharp encountered an internal error: Type context was null when parsing");
            }

            string fbsFieldType = typeContext.GetText();

            return (fbsFieldType, vectorType, structVectorLength);
        }
    }
}
