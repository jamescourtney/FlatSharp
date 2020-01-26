namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Parses metadata as a set of key value pairs. Case insensitive on keys.
    /// </summary>
    internal class FieldMetadataVisitor : FlatBuffersBaseVisitor<Dictionary<string, string>>
    {
        public override Dictionary<string, string> VisitMetadata([NotNull] FlatBuffersParser.MetadataContext context)
        {
            return this.VisitCommasep_ident_with_opt_single_value(context.commasep_ident_with_opt_single_value());
        }

        public override Dictionary<string, string> VisitCommasep_ident_with_opt_single_value([NotNull] FlatBuffersParser.Commasep_ident_with_opt_single_valueContext context)
        {
            var pairs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (context?.ident_with_opt_single_value() != null)
            {
                foreach (var item in context.ident_with_opt_single_value())
                {
                    string identifier = item.IDENT().GetText();
                    string value = item.single_value()?.GetText();

                    pairs[identifier] = value;
                }
            }

            return pairs;
        }
    }
}
