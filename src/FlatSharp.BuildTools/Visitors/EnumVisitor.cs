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
        private readonly BaseSchemaMember parent;

        private EnumDefinition enumDef;

        public EnumVisitor(BaseSchemaMember parent)
        {
            this.parent = parent;
        }

        public override EnumDefinition VisitEnum_decl([NotNull] FlatBuffersParser.Enum_declContext context)
        {
            string typeName = context.IDENT().GetText();
            this.enumDef = new EnumDefinition(
                typeName: typeName,
                underlyingTypeName: context.type().GetText(),
                parent: this.parent);

            ErrorContext.Current.WithScope(this.enumDef.Name, () =>
            {
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
