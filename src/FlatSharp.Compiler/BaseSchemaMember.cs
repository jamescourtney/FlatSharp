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

        public virtual HashSet<AttributeOption> Options => this.Parent.Options;

        public string Name { get; }

        public string FullName { get; }

        public string GlobalName => $"global::{this.FullName}";

        // File that declared this type.
        public string DeclaringFile { get; set; }

        public IReadOnlyDictionary<string, BaseSchemaMember> Children => this.children;

        protected abstract bool SupportsChildren { get; }
        
        public void WriteCode(
            CodeWriter writer, 
            CodeWritingPass pass, 
            string forFile,
            IReadOnlyDictionary<string, string> precompiledSerailizers)
        {
            // First pass: write all definitions (even from other files)
            //  the first pass is internal and is intended to produce a large
            //  file full of type definitions that FlatSharp can use to build serializers.
            // Second pass: only write things for the target file.
            //   the second pass generates only files in the root fbs file.
            if (pass == CodeWritingPass.FirstPass || forFile == this.DeclaringFile)
            {
                ErrorContext.Current.WithScope(
                    this.Name, () => this.OnWriteCode(writer, pass, forFile, precompiledSerailizers));
            }
        }

        public string GetCopyExpression(string source)
        {
            return ErrorContext.Current.WithScope(
                this.Name, () => this.OnGetCopyExpression(source));
        }

        protected virtual void OnWriteCode(CodeWriter writer, CodeWritingPass pass, string forFile, IReadOnlyDictionary<string, string> precompiledSerializers)
        {
        }

        protected abstract string OnGetCopyExpression(string source);

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
