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
        private readonly UnionDefinition unionDef;

        public UnionVisitor(string @namespace)
        {
            this.unionDef = new UnionDefinition { Namespace = @namespace };
        }

        public override UnionDefinition VisitUnion_decl([NotNull] FlatBuffersParser.Union_declContext context)
        {
            this.unionDef.TypeName = context.IDENT().GetText();

            ErrorContext.Current.WithScope(this.unionDef.TypeName, () =>
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
