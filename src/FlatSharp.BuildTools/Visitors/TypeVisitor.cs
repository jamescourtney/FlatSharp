namespace FlatSharp.Compiler
{
    using Antlr4.Runtime.Misc;
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeVisitor : FlatBuffersBaseVisitor<TableOrStructDefinition>
    {
        private readonly BaseSchemaMember parent;

        public TypeVisitor(BaseSchemaMember parent)
        {
            this.parent = parent;
        }

        public override TableOrStructDefinition VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            TableOrStructDefinition definition = new TableOrStructDefinition(context.IDENT().GetText(), this.parent);

            ErrorContext.Current.WithScope(definition.Name, () =>
            {
                definition.IsTable = context.GetChild(0).GetText() == "table";

                var fields = context.field_decl();
                if (fields != null)
                {
                    definition.Fields = fields.Select(x => new FieldVisitor().VisitField_decl(x)).ToList();
                }
            });

            return definition;
        }
    }
}
