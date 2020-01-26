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
                Dictionary<string, string> metadata = new FieldMetadataVisitor().VisitMetadata(context.metadata());
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

                string defaultValue = context.scalar()?.GetText();
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    this.definition.DefaultValue = defaultValue;
                }

                if (metadata.ContainsKey("deprecated"))
                {
                    this.definition.Deprecated = true;
                }

                // Attributes from FlatBuffers that we don't support.
                string[] unsupportedAttributes =
                {
                    "id", "required", "force_align", "bit_flags", "nested_flatbuffer", "flexbuffer", "key", "hash", "original_order"
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
