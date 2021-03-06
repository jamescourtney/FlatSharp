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
    using System.Collections.Generic;

    internal class StructVectorDefinition
    {
        public StructVectorDefinition(
            string name, 
            string typeName,
            SetterKind setterKind,
            List<string> properties)
        {
            this.PropertyNames = properties;
            this.SetterKind = setterKind;
            this.FbsTypeName = typeName;
            this.Name = name;
        }

        public List<string> PropertyNames { get; set; }

        public SetterKind SetterKind { get; set; }

        public string FbsTypeName { get; set; }

        public string Name { get; set; }

        public void EmitStructVector(TableOrStructDefinition parent, CodeWriter writer, CompileContext context)
        {
            string typeName = parent.ResolveTypeName(this.FbsTypeName, context, out ITypeModel? typeModel);

            string className = $"{this.Name}Vector";

            // two parts: property definition and class definition.
            writer.AppendLine($"private {className}? _{this.Name};");
            writer.AppendLine($"public {className} {this.Name} => (this._{this.Name} ??= new {className}(this));");
            writer.AppendLine();

            // class is next.
            writer.AppendLine($"public sealed partial class {className} : System.Collections.Generic.IEnumerable<{typeName}>");
            using (writer.WithBlock())
            {
                writer.AppendLine($"private readonly {parent.Name} item;");

                // ctor
                writer.AppendLine();
                writer.AppendLine($"public {className}({parent.Name} item)");
                using (writer.WithBlock())
                {
                    writer.AppendLine($"this.item = item;");
                }

                writer.AppendLine($"public int Count => {this.PropertyNames.Count};");

                // indexer
                writer.AppendLine();
                writer.AppendLine($"public {typeName} this[int index]");
                using (writer.WithBlock())
                {
                    writer.AppendLine("get");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("switch (index)");
                        using (writer.WithBlock())
                        {
                            for (int i = 0; i < this.PropertyNames.Count; ++i)
                            {
                                writer.AppendLine($"case {i}: return this.item.{this.PropertyNames[i]};");
                            }

                            writer.AppendLine($"default: throw new IndexOutOfRangeException();");
                        }
                    }

                    writer.AppendLine();

                    writer.AppendLine("set");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("switch (index)");
                        using (writer.WithBlock())
                        {
                            for (int i = 0; i < this.PropertyNames.Count; ++i)
                            {
                                writer.AppendLine($"case {i}: this.item.{this.PropertyNames[i]} = value; break;");
                            }

                            writer.AppendLine($"default: throw new IndexOutOfRangeException();");
                        }
                    }
                }

                writer.AppendLine("System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();");
                writer.AppendLine();
                writer.AppendLine($"public System.Collections.Generic.IEnumerator<{typeName}> GetEnumerator()");
                using (writer.WithBlock())
                {
                    for (int i = 0; i < this.PropertyNames.Count; ++i)
                    {
                        writer.AppendLine($"yield return this.item.{this.PropertyNames[i]};");
                    }

                    if (this.PropertyNames.Count == 0)
                    {
                        writer.AppendLine("yield break;");
                    }
                }

                string arrayOrSpanType = $"{typeName}[]";
                if (typeModel is not null && 
                    typeModel.ClassifyContextually(FlatBufferSchemaType.Struct).IsRequiredValue())
                {
                    arrayOrSpanType = $"ReadOnlySpan<{typeName}>";
                }

                foreach (var collectionType in new[] { arrayOrSpanType, $"IReadOnlyList<{typeName}>"})
                {
                    writer.AppendMethodSummaryComment($"Deep copies the first {this.PropertyNames.Count} items from the source into this struct vector.");
                    writer.AppendLine($"public void CopyFrom({collectionType} source)");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("var thisItem = this.item;");

                        // Load in reverse so that the JIT can just do a bounds check on the very first item.
                        for (int i = this.PropertyNames.Count - 1; i >= 0; --i)
                        {
                            writer.AppendLine($"thisItem.{this.PropertyNames[i]} = {context.FullyQualifiedCloneMethodName}(source[{i}]);");
                        }
                    }
                }
            }
        }
    }
}
