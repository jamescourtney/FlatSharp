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

    /// <summary>
    /// Defines a Union.
    /// </summary>
    internal class UnionDefinition : BaseSchemaMember
    {
        public UnionDefinition(string name, BaseSchemaMember parent) : base(name, parent)
        {
        }

        public bool GenerateCustomUnionType { get; set; } = true;

        public List<(string alias, string type)> Components { get; set; } = new List<(string alias, string type)>();

        public string ClrTypeName
        {
            get
            {
                if (this.GenerateCustomUnionType)
                {
                    return this.GlobalName;
                }

                List<string> genericParts = new List<string>();
                foreach (var item in this.GetResolvedComponents())
                {
                    genericParts.Add(item.fullyQualifiedType);
                }

                return $"FlatBufferUnion<{string.Join(", ", genericParts)}>";
            }
        }

        private List<(int index, string alias, string fullyQualifiedType)> GetResolvedComponents()
        {
            var resolvedComponentNames = new List<(int, string, string)>();
            int index = 1;
            foreach (var component in this.Components)
            {
                string fallbackName = component.type.Split('.').Last();

                if (this.TryResolveName(component.type, out var reference))
                {
                    resolvedComponentNames.Add((index++, component.alias ?? fallbackName, reference.GlobalName));
                }
                else
                {
                    resolvedComponentNames.Add((index++, component.alias ?? fallbackName, component.type));
                }
            }

            return resolvedComponentNames;
        }

        protected override void OnWriteCode(
            CodeWriter writer, 
            CodeWritingPass pass, 
            string forFile, 
            IReadOnlyDictionary<string, string> precompiledSerializer)
        {
            if (!this.GenerateCustomUnionType)
            {
                return;
            }


            var resolvedComponents = this.GetResolvedComponents();
            string baseTypeName = string.Join(", ", resolvedComponents.Select(x => x.fullyQualifiedType));

            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name} : FlatBufferUnion<{baseTypeName}>");
            using (writer.WithBlock())
            {
                // Generate an internal type enum.
                writer.AppendLine("public enum ItemKind : byte");
                using (writer.WithBlock())
                {
                    foreach (var item in resolvedComponents)
                    {
                        writer.AppendLine($"{item.alias} = {item.index},");
                    }
                }

                writer.AppendLine();
                writer.AppendLine("public ItemKind Kind => (ItemKind)base.Discriminator;");

                foreach (var item in resolvedComponents)
                {
                    writer.AppendLine();
                    writer.AppendLine($"public {this.Name}({item.fullyQualifiedType} value) : base(value) {{ }}");

                    writer.AppendLine();
                    writer.AppendLine($"public {item.fullyQualifiedType} {item.alias} => base.Item{item.index};");
                }

                // Clone method
                this.WriteCloneMethod(writer, resolvedComponents);
                this.WriteSwitchMethod(writer, true, true, resolvedComponents);
                this.WriteSwitchMethod(writer, true, false, resolvedComponents);
                this.WriteSwitchMethod(writer, false, true, resolvedComponents); 
                this.WriteSwitchMethod(writer, false, false, resolvedComponents);
            }
        }

        private void WriteCloneMethod(CodeWriter writer, List<(int index, string alias, string fullyQualifiedName)> components)
        {
            writer.AppendLine();
            writer.AppendLine($"public new {this.Name} Clone(");
            using (writer.IncreaseIndent())
            {
                bool first = true;
                foreach (var item in components)
                {
                    string line = $"Func<{item.fullyQualifiedName}, {item.fullyQualifiedName}> clone{item.alias}";
                    if (!first)
                    {
                        line = $", {line}";
                    }
                    first = false;

                    writer.AppendLine(line);
                }
            }
            writer.AppendLine(")");

            using (writer.WithBlock())
            {
                writer.AppendLine("switch (base.Discriminator)");
                using (writer.WithBlock())
                {
                    foreach (var item in components)
                    {
                        writer.AppendLine($"case {item.index}:");
                        using (writer.IncreaseIndent())
                        {
                            writer.AppendLine($"return new {this.Name}(clone{item.alias}(base.item{item.index}));");
                        }
                    }
                }

                writer.AppendLine("throw new System.InvalidOperationException();");
            }
            writer.AppendLine();
        }

        private void WriteSwitchMethod(CodeWriter writer, bool hasReturn, bool hasState, List<(int index, string alias, string fullyQualifiedName)> components)
        {
            const string GenericReturnType = "TReturn";
            const string VoidReturnType = "void";
            const string GenericStateType = "TState";

            string genericArgsWithEnds;
            Func<string, string> itemDelegateType;
            string defaultDelegateType;
            string returnType;

            if (hasReturn && hasState)
            {
                genericArgsWithEnds = $"<{GenericStateType}, {GenericReturnType}>";
                itemDelegateType = x => $"Func<{GenericStateType}, {x}, {GenericReturnType}>";
                defaultDelegateType = $"Func<{GenericStateType}, {GenericReturnType}>";
                returnType = GenericReturnType;
            }
            else if (hasReturn)
            {
                genericArgsWithEnds = $"<{GenericReturnType}>";
                itemDelegateType = x => $"Func<{x}, {GenericReturnType}>";
                defaultDelegateType = $"Func<{GenericReturnType}>";
                returnType = GenericReturnType;
            }
            else if (hasState)
            {
                genericArgsWithEnds = $"<{GenericStateType}>";
                itemDelegateType = x => $"Action<{GenericStateType}, {x}>";
                defaultDelegateType = $"Action<{GenericStateType}>";
                returnType = VoidReturnType;
            }
            else
            {
                genericArgsWithEnds = string.Empty;
                itemDelegateType = x => $"Action<{x}>";
                defaultDelegateType = $"Action";
                returnType = VoidReturnType;
            }

            List<string> args = new List<string>();

            writer.AppendLine($"public new {returnType} Switch{genericArgsWithEnds}(");
            using (writer.IncreaseIndent())
            {
                if (hasState)
                {
                    writer.AppendLine("TState state, ");
                }

                writer.AppendLine($"{defaultDelegateType} caseDefault");
                foreach (var item in components)
                {
                    string argName = $"case{item.alias}";
                    args.Add(argName);

                    writer.AppendLine($", {itemDelegateType(item.fullyQualifiedName)} {argName}");
                }
            }

            string stateParam = hasState ? "state, " : string.Empty;

            writer.AppendLine($") => base.Switch{genericArgsWithEnds}({stateParam}caseDefault, {string.Join(", ", args)});");
        }

        protected override string OnGetCopyExpression(string source)
        {
            List<string> cloners = new List<string>();
            foreach (var item in this.Components)
            {
                if (this.TryResolveName(item.type, out var node))
                {
                    string subClone = node.GetCopyExpression("x");
                    cloners.Add($"x => {subClone}");
                }
                else if (item.alias == "string")
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
