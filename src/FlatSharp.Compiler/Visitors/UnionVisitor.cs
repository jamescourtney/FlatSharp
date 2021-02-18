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

            var metadata = new MetadataVisitor().VisitMetadata(context.metadata());

            ErrorContext.Current.WithScope(this.unionDef.Name, () =>
            {
                base.VisitUnion_decl(context);
            });

            return this.unionDef;
        }

        public override UnionDefinition VisitUnionval_decl([NotNull] FlatBuffersParser.Unionval_declContext context)
        {
            string type = context.type().GetText();
            string alias = context.IDENT()?.GetText();
            this.unionDef.Components.Add((alias, type));

            return null;
        }
    }
}
