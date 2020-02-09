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

    internal abstract class BaseSchemaMember
    {
        private readonly Dictionary<string, BaseSchemaMember> children;

        protected BaseSchemaMember(string name, BaseSchemaMember parent)
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

        public BaseSchemaMember Parent { get; }

        public string Name { get; }

        public string FullName { get; }

        public string GlobalName => $"global::{this.FullName}";

        public IReadOnlyDictionary<string, BaseSchemaMember> Children => this.children;

        protected abstract bool SupportsChildren { get; }
        
        public void WriteCode(CodeWriter writer, CodeWritingPass pass, IReadOnlyDictionary<string, string> precompiledSerailizers)
        {
            ErrorContext.Current.WithScope(
                this.Name, () => this.OnWriteCode(writer, pass, precompiledSerailizers));
        }

        protected virtual void OnWriteCode(CodeWriter writer, CodeWritingPass pass, IReadOnlyDictionary<string, string> precompiledSerializer)
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
        public bool TryResolveName(string name, out BaseSchemaMember node)
        {
            Span<string> parts = name.Split('.');

            // Go up to the first namespace node in the tree.
            BaseSchemaMember rootNode = this;
            while (!(rootNode is NamespaceDefinition) && !(rootNode is RootNodeDefinition))
            {
                rootNode = rootNode.Parent;
            }

            if (this.TryResolveDescendentsFromNode(rootNode, parts, out node))
            {
                return true;
            }

            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }

            return this.TryResolveDescendentsFromNode(rootNode, parts, out node);
        }

        private bool TryResolveDescendentsFromNode(BaseSchemaMember startNode, Span<string> parts, out BaseSchemaMember node)
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
