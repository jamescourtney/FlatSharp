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
    /// <summary>
    /// Defines the node at the root of the schema.
    /// </summary>
    internal class RootNodeDefinition : BaseSchemaMember
    {
        public RootNodeDefinition() : base("", null)
        {
        }

        protected override bool SupportsChildren => true;

        protected override void OnWriteCode(CodeWriter writer)
        {
            writer.AppendLine("using System;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using FlatSharp;");
            writer.AppendLine("using FlatSharp.Attributes;");

            foreach (var child in this.Children.Values)
            {
                child.WriteCode(writer);
            }
        }
    }
}
