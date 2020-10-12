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

    internal class FieldVisitor : FlatBuffersBaseVisitor<FieldDefinition>
    {
        private readonly FieldDefinition definition;

        public FieldVisitor()
        {
            this.definition = new FieldDefinition();
        }

        public override FieldDefinition VisitField_decl([NotNull] FlatBuffersParser.Field_declContext context)
        {
            this.definition.Name = context.IDENT().GetText();

            ErrorContext.Current.WithScope(this.definition.Name, () =>
            {
                Dictionary<string, string> metadata = new MetadataVisitor().VisitMetadata(context.metadata());
                string fbsFieldType = context.type().GetText();

                this.definition.VectorType = VectorType.None;

                if (fbsFieldType.StartsWith("["))
                {
                    this.definition.VectorType = VectorType.IList;

                    // Trim the starting and ending square brackets.
                    fbsFieldType = fbsFieldType.Substring(1, fbsFieldType.Length - 2);

                    this.definition.VectorType = VectorType.IList;
                    if (metadata.TryGetValue("vectortype", out string vectorTypeString))
                    {
                        if (!Enum.TryParse<VectorType>(vectorTypeString, true, out var vectorType))
                        {
                            ErrorContext.Current?.RegisterError($"Unable to parse '{vectorTypeString}' as a vector type. Valid choices are: {string.Join(", ", Enum.GetNames(typeof(VectorType)))}.");
                        }

                        this.definition.VectorType = vectorType;
                    }
                }
                else if (metadata.ContainsKey("vectortype"))
                {
                    ErrorContext.Current?.RegisterError($"Non-vectors may not have the 'vectortype' attribute. Field = '{this.definition.Name}'");
                }

                this.definition.FbsFieldType = fbsFieldType;

                string defaultValue = context.defaultValue_decl()?.GetText();
                if (defaultValue == "null")
                {
                    this.definition.IsOptionalScalar = true;
                }
                else if (!string.IsNullOrEmpty(defaultValue))
                {
                    this.definition.DefaultValue = defaultValue;
                }

                if (metadata.ContainsKey("deprecated"))
                {
                    this.definition.Deprecated = true;
                }

                if (metadata.ContainsKey("key"))
                {
                    this.definition.IsKey = true;
                }

                if (metadata.TryGetValue("virtual", out string virtualValue))
                {
                    if (!bool.TryParse(virtualValue, out bool isVirtual))
                    {
                        ErrorContext.Current?.RegisterError($"The 'virtual' attribute must have a boolean value.");
                    }

                    this.definition.Virtual = isVirtual;
                }

                if (metadata.TryGetValue("setter", out string setterStyle))
                {
                    if (!Enum.TryParse<SetterKind>(setterStyle, true, out SetterKind kind))
                    {
                        ErrorContext.Current?.RegisterError($"Unable to parse '{setterStyle}' as a Setter kind. Valid values are: {string.Join(", ", Enum.GetValues(typeof(SetterKind)))}");
                    }

                    definition.SetterKind = kind;
                }

                if (metadata.ContainsKey("sortedvector"))
                {
                    this.definition.SortedVector = true;
                }

                // override the given type and use shared string instead.
                if (metadata.ContainsKey("sharedstring"))
                {
                    this.definition.SharedString = true;
                }

                // Attributes from FlatBuffers that we don't support.
                string[] unsupportedAttributes =
                {
                    "id", "required", "force_align", "bit_flags", "flexbuffer", "hash", "original_order"
                };

                foreach (var unsupportedAttribute in unsupportedAttributes)
                {
                    if (metadata.ContainsKey(unsupportedAttribute))
                    {
                        ErrorContext.Current?.RegisterError($"FlatSharpCompiler does not support the '{unsupportedAttribute}' attribute in FBS files.");
                    }
                }
            });

            return this.definition;
        }
    }
}
