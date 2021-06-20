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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal abstract class BaseSchemaMember
    {
        private readonly Dictionary<string, BaseSchemaMember> children;

        protected BaseSchemaMember(string name, BaseSchemaMember? parent)
        {
            this.children = new Dictionary<string, BaseSchemaMember>();
            this.Parent = parent;
            this.Name = name;
            this.FullName = string.Empty;

            if (this.Parent != null)
            {
                this.FullName =  this.Parent.GetType() != typeof(RootNodeDefinition) ? $"{this.Parent.FullName}.{this.Name}" : this.Name;
            }
        }

        public BaseSchemaMember? Parent { get; }

        public string Name { get; }

        public string FullName { get; }

        public string GlobalName => $"global::{this.FullName}";

        // File that declared this type.
        public string? DeclaringFile { get; set; }

        public IReadOnlyDictionary<string, BaseSchemaMember> Children => this.children;

        protected abstract bool SupportsChildren { get; }
        
        public void WriteCode(CodeWriter writer, CompileContext context)
        {
            // Prior to last pass: 
            //      Write all definitions (even from other files)
            //      These passes are internal are are intended to produce a bunch of type definitions 
            //      that FlatSharp can use to build serializers.
            // Last Pass: 
            //      Only write things for the target file. This pass consumes the output
            //      of previous passes but doesn't generate all types.
            if (context.CompilePass < CodeWritingPass.LastPass || context.RootFile == this.DeclaringFile)
            {
                ErrorContext.Current.WithScope(
                    this.Name, () => this.OnWriteCode(writer, context));
            }
        }

        protected virtual void OnWriteCode(CodeWriter writer, CompileContext context)
        {
        }

        public void AddChild(BaseSchemaMember child)
        {           
            ErrorContext.Current.WithScope(this.Name, () => 
            {
                if (!this.SupportsChildren)
                {
                    ErrorContext.Current?.RegisterError($"Unable to add child to current context.");
                }

                if (this.children.ContainsKey(child.Name))
                {
                    ErrorContext.Current?.RegisterError($"Duplicate member name '{child.Name}'.");
                }

                this.children[child.Name] = child;
            });
        }

        /// <summary>
        /// Resolves a name according to the relative namespace path.
        /// </summary>
        public bool TryResolveName(string name, [NotNullWhen(true)] out BaseSchemaMember? node)
        {
            Span<string> parts = name.Split('.');

            // Go up to the first namespace node in the tree.
            BaseSchemaMember? rootNode = this;
            while (!(rootNode is NamespaceDefinition) && !(rootNode is RootNodeDefinition))
            {
                rootNode = rootNode!.Parent;
            }

            FlatSharpInternal.Assert(rootNode is not null, "Root not should not be null");

            // If there is a qualification, it means we only ever search upwards.
            if (parts.Length != 1)
            {
                rootNode = rootNode.Parent;
            }

            while (rootNode is not null)
            {
                // Try to find recursively from each namespace.
                if (this.TryResolveDescendentsFromNode(rootNode, parts, out node))
                {
                    return true;
                }

                rootNode = rootNode.Parent;
            }

            node = null;
            return false;
        }

        public bool TryResolveTypeModelWithError(string name, CompileContext context, [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            // FBS alias.
            if (context.TypeModelContainer.TryResolveFbsAlias(name, out typeModel))
            {
                return true;
            }

            if (this.TryResolveName(name, out var node))
            {
                Type? type = context.PreviousAssembly?.GetType(node.FullName);
                if (type != null && context.TypeModelContainer.TryCreateTypeModel(type, out typeModel))
                {
                    return true;
                }
            }

            ErrorContext.Current.RegisterError($"Unable to resolve type: '{name}'. Context = '{this.FullName}'.");
            return false;
        }

        private bool TryResolveDescendentsFromNode(BaseSchemaMember startNode, Span<string> parts, [NotNullWhen(true)] out BaseSchemaMember? node)
        {
            node = startNode;
            while (node.Children.TryGetValue(parts[0], out node))
            {
                if (parts.Length == 1)
                {
                    return true;
                }

                parts = parts.Slice(1);
            }

            return false;
        }
    }
}
