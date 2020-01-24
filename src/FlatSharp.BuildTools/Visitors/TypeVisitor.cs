namespace FlatSharp.Compiler
{
    using Antlr4.Runtime.Misc;
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeVisitor : FlatBuffersBaseVisitor<TableOrStructDefinition>
    {
        private readonly TableOrStructDefinition definition;

        public TypeVisitor(string namespaceName)
        {
            this.definition = new TableOrStructDefinition { Namespace = namespaceName };
        }

        public override TableOrStructDefinition VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            this.definition.TypeName = context.IDENT().GetText();

            ErrorContext.Current.WithScope(this.definition.TypeName, () =>
            {
                this.definition.IsTable = context.GetChild(0).GetText() == "table";

                var fields = context.field_decl();
                if (fields != null)
                {
                    this.definition.Fields = fields.Select(x => new FieldVisitor().VisitField_decl(x)).ToList();
                }
            });

            return this.definition;
        }
    }
}
