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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;

    internal class SchemaVisitor : FlatBuffersBaseVisitor<BaseSchemaMember?>
    {
        private BaseSchemaMember schemaRoot;
        private readonly Stack<BaseSchemaMember> parseStack = new Stack<BaseSchemaMember>();

        public SchemaVisitor(RootNodeDefinition rootNode)
        {
            this.schemaRoot = rootNode;
        }

        public string? CurrentFileName { get; set; }

        public override BaseSchemaMember Visit([NotNull] IParseTree tree)
        {
            this.parseStack.Push(this.schemaRoot);

            base.Visit(tree);

            return this.schemaRoot;
        }

        public override BaseSchemaMember? VisitNamespace_decl([NotNull] FlatBuffersParser.Namespace_declContext context)
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

        public override BaseSchemaMember? VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                TableOrStructDefinition def = new TypeVisitor(top).Visit(context);
                def.DeclaringFile = this.CurrentFileName;
                top.AddChild(def);
            });

            return null;
        }

        public override BaseSchemaMember? VisitEnum_decl([NotNull] FlatBuffersParser.Enum_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                EnumDefinition? def = new EnumVisitor(top).Visit(context);
                if (def is null)
                {
                    throw new InvalidOperationException("FlatSharp.Internal: Enum definition visitor returned null");
                }

                def.DeclaringFile = this.CurrentFileName;
                top.AddChild(def);
            });

            return null;
        }

        public override BaseSchemaMember? VisitUnion_decl([NotNull] FlatBuffersParser.Union_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                UnionDefinition? def = new UnionVisitor(top).Visit(context);
                if (def is null)
                {
                    throw new InvalidOperationException("FlatSharp.Internal: Union definition visitor returned null");
                }

                def.DeclaringFile = this.CurrentFileName;
                top.AddChild(def);
            });

            return null;
        }

        public override BaseSchemaMember? VisitRpc_decl([NotNull] FlatBuffersParser.Rpc_declContext context)
        {
            var top = this.parseStack.Peek();
            ErrorContext.Current.WithScope(top.FullName, () =>
            {
                RpcDefinition? def = new RpcVisitor(top).Visit(context);
                if (def is null)
                {
                    throw new InvalidOperationException("FlatSharp.Internal: RPC definition visitor returned null");
                }

                def.DeclaringFile = this.CurrentFileName;
                top.AddChild(def);
            });

            return null;
        }

        private BaseSchemaMember GetOrCreateNamespace(Span<string> parts, BaseSchemaMember parent)
        {
            if (!parent.TryResolveName(parts[0], out var existingNode))
            {
                existingNode = new NamespaceDefinition(parts[0], parent);
                existingNode.DeclaringFile = this.CurrentFileName;
                parent.AddChild(existingNode);
            }

            if (parts.Length == 1)
            {
                return existingNode;
            }

            return this.GetOrCreateNamespace(parts.Slice(1), existingNode);
        }
    }
}
