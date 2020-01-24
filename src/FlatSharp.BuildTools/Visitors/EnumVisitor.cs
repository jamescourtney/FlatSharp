namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Parses an enum definition.
    /// </summary>
    internal class EnumVisitor : FlatBuffersBaseVisitor<EnumDefinition>
    {
        private readonly string @namespace;
        private EnumDefinition enumDef;

        public EnumVisitor(string @namespace)
        {
            this.@namespace = @namespace;
        }

        public override EnumDefinition VisitEnum_decl([NotNull] FlatBuffersParser.Enum_declContext context)
        {
            string typeName = context.IDENT().GetText();
            ErrorContext.Current.WithScope(typeName, () =>
            {
                this.enumDef = new EnumDefinition(
                    typeName: typeName,
                    underlyingTypeName: context.type().GetText(),
                    ns: this.@namespace);

                base.VisitEnum_decl(context);
            });

            return this.enumDef;
        }

        public override EnumDefinition VisitEnumval_decl([NotNull] FlatBuffersParser.Enumval_declContext context)
        {
            string name = context.IDENT().GetText();
            string value = context.integer_const()?.GetText();

            this.enumDef.AddNameValuePair(name, value);
            return null;
        }
    }
}
