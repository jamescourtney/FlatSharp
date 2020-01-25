namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Parses a union definition. Returns a FlatBuffer union type declaration (FlatBufferUnion{T1, T2, T3}).
    /// </summary>
    internal class UnionVisitor : FlatBuffersBaseVisitor<UnionDefinition>
    {
        private readonly BaseSchemaMember parent;
        private UnionDefinition unionDef;

        public UnionVisitor(BaseSchemaMember parent)
        {
            this.parent = parent;
        }

        public override UnionDefinition VisitUnion_decl([NotNull] FlatBuffersParser.Union_declContext context)
        {
            this.unionDef = new UnionDefinition(context.IDENT().GetText(), this.parent);

            ErrorContext.Current.WithScope(this.unionDef.Name, () =>
            {
                base.VisitUnion_decl(context);
            });

            return this.unionDef;
        }

        public override UnionDefinition VisitUnionval_decl([NotNull] FlatBuffersParser.Unionval_declContext context)
        {
            string type = context.type().GetText();
            this.unionDef.ComponentTypeNames.Add(type);

            return null;
        }
    }
}
