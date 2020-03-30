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
    using System.Collections.Generic;

    /// <summary>
    /// Defines a Union.
    /// </summary>
    internal class UnionDefinition : BaseSchemaMember
    {
        public UnionDefinition(string name, BaseSchemaMember parent) : base(name, parent)
        {
        }

        public List<string> ComponentTypeNames { get; set; } = new List<string>();

        public string ClrTypeName
        {
            get
            {
                List<string> resolvedComponentNames = new List<string>();
                foreach (var name in this.ComponentTypeNames)
                {
                    if (this.TryResolveName(name, out var reference))
                    {
                        resolvedComponentNames.Add(reference.GlobalName);
                    }
                    else
                    {
                        resolvedComponentNames.Add(name);
                    }
                }

                return $"FlatBufferUnion<{string.Join(", ", resolvedComponentNames)}>";
            }
        }

        protected override string OnGetCopyExpression(string source)
        {
            List<string> cloners = new List<string>();
            foreach (var item in this.ComponentTypeNames)
            {
                if (this.Parent.TryResolveName(item, out var node))
                {
                    string subClone = node.GetCopyExpression("x");
                    cloners.Add($"x => {subClone}");
                }
                else if (item == "string")
                {
                    cloners.Add("x => x");
                }
                else
                {
                    ErrorContext.Current.RegisterError("Unable to resolve type: " + item);
                }
            }

            return $"{source}?.Clone({string.Join(",\r\n", cloners)})";
        }

        protected override bool SupportsChildren => false;
    }
}
