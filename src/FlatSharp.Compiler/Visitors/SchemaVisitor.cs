﻿/*
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
    using System.Linq;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;

    internal class SchemaVisitor : FlatBuffersBaseVisitor<BaseSchemaMember>
    {
        private BaseSchemaMember schemaRoot;
        private readonly Stack<BaseSchemaMember> parseStack = new Stack<BaseSchemaMember>();
        private readonly string inputHash;

        public SchemaVisitor(string inputHash)
        {
            this.inputHash = inputHash;
        }

        public override BaseSchemaMember Visit([NotNull] IParseTree tree)
        {
            this.schemaRoot = new RootNodeDefinition(this.inputHash);
            this.parseStack.Push(this.schemaRoot);

            base.Visit(tree);

            return this.schemaRoot;
        }

        public override BaseSchemaMember VisitNamespace_decl([NotNull] FlatBuffersParser.Namespace_declContext context)
        {
            // Namespaces reset the whole stack.
            while (this.parseStack.Peek() != this.schemaRoot)
            {
                this.parseStack.Pop();
            }

            string[] nsParts = context.IDENT().Select(x => x.GetText()).ToArray();
            this.parseStack.Push(this.GetOrCreateNamespace(nsParts, this.schemaRoot));

            return null;
        }

        private BaseSchemaMember GetOrCreateNamespace(Span<string> parts, BaseSchemaMember parent)
        {
            if (!parent.TryResolveName(parts[0], out var existingNode))
            {
                existingNode = new NamespaceDefinition(parts[0], parent);
                parent.AddChild(existingNode);
            }

            if (parts.Length == 1)
            {
                return existingNode;
            }

            return this.GetOrCreateNamespace(parts.Slice(1), existingNode);
        }

        public override BaseSchemaMember VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                TableOrStructDefinition def = new TypeVisitor(top).Visit(context);
                top.AddChild(def);
            });

            return null;
        }

        public override BaseSchemaMember VisitEnum_decl([NotNull] FlatBuffersParser.Enum_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                EnumDefinition def = new EnumVisitor(top).Visit(context);
                top.AddChild(def);
            });

            return null;
        }

        public override BaseSchemaMember VisitUnion_decl([NotNull] FlatBuffersParser.Union_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                UnionDefinition def = new UnionVisitor(top).Visit(context);
                top.AddChild(def);
            });

            return null;
        }
    }
}
