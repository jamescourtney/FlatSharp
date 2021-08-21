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
        private readonly BaseTableOrStructDefinition parent;

        public FieldVisitor(BaseTableOrStructDefinition parent)
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

                string? resolvedType = null;
                if (this.parent.TryResolveName(fieldType, out var resolved))
                {
                    resolvedType = resolved.GlobalName;
                }

                var definition = new FieldDefinition(name, fieldType, resolvedType)
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

                // standard metadata
                definition.Deprecated = metadata.ParseBooleanMetadata(MetadataKeys.Deprecated);
                definition.IsKey = metadata.ParseBooleanMetadata(MetadataKeys.Key);

                // Flatsharp custom metadata
                definition.SortedVector = metadata.ParseBooleanMetadata(MetadataKeys.SortedVector, MetadataKeys.SortedVectorLegacy);
                definition.SharedString = metadata.ParseBooleanMetadata(MetadataKeys.SharedString, MetadataKeys.SharedStringLegacy);
                definition.NonVirtual = metadata.ParseNullableBooleanMetadata(MetadataKeys.NonVirtualProperty, MetadataKeys.NonVirtualPropertyLegacy);
                definition.ForceWrite = metadata.ParseNullableBooleanMetadata(MetadataKeys.ForceWrite);
                definition.WriteThrough = metadata.ParseNullableBooleanMetadata(MetadataKeys.WriteThrough);
                definition.IsRequired = metadata.ParseBooleanMetadata(MetadataKeys.Required);

                this.ParseIdMetadata(definition, metadata);

                definition.SetterKind = metadata.ParseMetadata(
                    new[] { MetadataKeys.Setter, MetadataKeys.SetterLegacy },
                    ParseSetterKind,
                    SetterKind.Public,
                    SetterKind.Public);

                if (structVectorLength == null)
                {
                    this.parent.Fields.Add(definition);
                }
                else if (this.parent.SchemaType != TypeModel.FlatBufferSchemaType.Struct)
                {
                    ErrorContext.Current.RegisterError("Only structs may contain fixed-length vectors");
                }
                else if (structVectorLength.Value <= 0)
                {
                    ErrorContext.Current.RegisterError("Struct vector lengths must be non-negative.");
                }
                else
                {
                    List<string> groupNames = new List<string>();

                    for (int i = 0; i < structVectorLength.Value; ++i)
                    {
                        string name = $"__flatsharp__{definition.Name}_{i}";
                        this.parent.Fields.Add(definition with
                        {
                            Name = name,
                            SetterKind = SetterKind.Protected,
                            GetterModifier = AccessModifier.Protected,
                            CustomGetter = $"{definition.Name}[{i}]"
                        });

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
            const int DefaultIfPresent = -1;

            int? index = metadata.ParseNullableIntegerMetadata(
                new[] { MetadataKeys.Id },
                defaultValueIfPresent: DefaultIfPresent,
                defaultValueIfNotPresent: null);

            if (index == null)
            {
                return;
            }

            if (index == DefaultIfPresent)
            {
                ErrorContext.Current.RegisterError($"Value of '{MetadataKeys.Id}' attribute should be set if attribute present.");
                return;
            }

            if (index.Value < 0)
            {
                ErrorContext.Current.RegisterError($"Value of '{MetadataKeys.Id}' attribute {index} of '{definition.Name}' field is negative.");
                return;
            }

            definition.Index = index.Value;
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

                if (metadata.TryGetValue(MetadataKeys.VectorKind, out string? vectorTypeString) ||
                    metadata.TryGetValue(MetadataKeys.VectorKindLegacy, out vectorTypeString))
                {
                    if (!Enum.TryParse<VectorType>(vectorTypeString, true, out vectorType))
                    {
                        ErrorContext.Current.RegisterError(
                            $"Unable to parse '{vectorTypeString}' as a vector type. Valid choices are: {string.Join(", ", Enum.GetNames(typeof(VectorType)))}.");
                    }
                }
            }
            else if (metadata.ContainsKey(MetadataKeys.VectorKind) || metadata.ContainsKey(MetadataKeys.VectorKindLegacy))
            {
                ErrorContext.Current.RegisterError(
                    $"Non-vectors may not have the '{MetadataKeys.VectorKind}' or '{MetadataKeys.VectorKindLegacy}' attributes.");
            }

            if (context.type().structvector_type() is not null)
            {
                typeContext = context.type().structvector_type().core_type();
                string toParse = context.type().structvector_type().INTEGER_CONSTANT().GetText();

                if (!int.TryParse(toParse, out var length) || length <= 0)
                {
                    ErrorContext.Current.RegisterError(
                        $"Unable to parse '{toParse}' as a struct vector length. Lengths should be a postive base 10 integer.");
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

            FlatSharpInternal.Assert(typeContext is not null, "Type context was null when parsing");

            string fbsFieldType = typeContext.GetText();
            return (fbsFieldType, vectorType, structVectorLength);
        }
    }
}
