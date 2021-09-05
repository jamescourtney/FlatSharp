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
    using System.Linq;

    /// <summary>
    /// Defines a Union.
    /// </summary>
    internal class UnionDefinition : BaseSchemaMember
    {
        public UnionDefinition(string name, BaseSchemaMember? parent) : base(name, parent)
        {
        }

        protected override bool SupportsChildren => false;

        public List<(string? alias, string type)> Components { get; set; } = new List<(string? alias, string type)>();

        private List<(int index, string alias, string globalName, string fullTypeName, BaseSchemaMember? member)> GetResolvedComponents()
        {
            var resolvedComponentNames = new List<(int, string, string, string, BaseSchemaMember? member)>();
            int index = 1;
            foreach (var component in this.Components)
            {
                string fallbackName = component.type.Split('.').Last();

                if (fallbackName == "string")
                {
                    fallbackName = "String";
                }

                if (this.TryResolveName(component.type, out var reference))
                {
                    resolvedComponentNames.Add((index++, component.alias ?? fallbackName, reference.GlobalName, reference.FullName, reference));
                }
                else
                {
                    resolvedComponentNames.Add((index++, component.alias ?? fallbackName, component.type, component.type, null));
                }
            }

            return resolvedComponentNames;
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            var resolvedComponents = this.GetResolvedComponents();
            string baseTypeName = string.Join(", ", resolvedComponents.Select(x => x.globalName));

            string interfaceName = $"IFlatBufferUnion<{baseTypeName}>";

            string structOrClass = "struct";
            if (context.CompilePass == CodeWritingPass.Initialization)
            {
                structOrClass = "class";
            }

            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial {structOrClass} {this.Name} : {interfaceName}");
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
                writer.AppendLine("private readonly object value;");

                writer.AppendLine();
                writer.AppendLine("public ItemKind Kind => (ItemKind)this.Discriminator;");

                writer.AppendLine();
                writer.AppendLine("public byte Discriminator { get; }");

                foreach (var item in resolvedComponents)
                {
                    writer.AppendLine();
                    writer.AppendLine($"public {this.Name}({item.globalName} value)");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"object temp = value;");
                        writer.AppendLine($"this.value = temp ?? throw new ArgumentNullException(nameof(value));");
                        writer.AppendLine($"this.Discriminator = {item.index};");
                    }

                    writer.AppendLine();
                    writer.AppendLine($"public {item.globalName} {item.alias} => this.Item{item.index};");

                    writer.AppendLine();
                    writer.AppendLine($"public {item.globalName} Item{item.index}");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("get");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine($"if (this.Discriminator != {item.index})");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine("throw new InvalidOperationException();");
                            }

                            writer.AppendLine($"return ({item.globalName})this.value;");
                        }
                    }

                    string notNullWhen = string.Empty;
                    string nullableReference = string.Empty;

                    if (item.member is TableOrStructDefinition referenceDef ||
                        (context.TypeModelContainer.TryResolveFbsAlias(item.fullTypeName, out ITypeModel? typeModel) && typeModel.SchemaType == FlatBufferSchemaType.String))
                    {
                        if (context.Options.NullableWarnings == true)
                        {
                            notNullWhen = $"[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] ";
                        }

                        nullableReference = "?";
                    }

                    writer.AppendLine();
                    writer.AppendLine($"public bool TryGet({notNullWhen}out {item.globalName}{nullableReference} value)");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"if (this.Discriminator != {item.index})");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine("value = default;");
                            writer.AppendLine("return false;");
                        }

                        writer.AppendLine($"value = ({item.globalName})this.value;");
                        writer.AppendLine("return true;");
                    }
                }

                // Clone method
                this.WriteSwitchMethod(writer, true, true, resolvedComponents);
                this.WriteSwitchMethod(writer, true, false, resolvedComponents);
                this.WriteSwitchMethod(writer, false, true, resolvedComponents); 
                this.WriteSwitchMethod(writer, false, false, resolvedComponents);
            }
        }

        private void WriteSwitchMethod(CodeWriter writer, bool hasReturn, bool hasState, List<(int index, string alias, string globalName, string fullName, BaseSchemaMember? member)> components)
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

            writer.AppendLine($"public {returnType} Switch{genericArgsWithEnds}(");
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

                    writer.AppendLine($", {itemDelegateType(item.globalName)} {argName}");
                }
            }

            string stateParam = hasState ? "state, " : string.Empty;

            writer.AppendLine(")");
            using (writer.WithBlock())
            {
                writer.AppendLine("switch (this.Discriminator)");
                using (writer.WithBlock())
                {
                    foreach (var item in components)
                    {
                        if (hasReturn)
                        {
                            writer.AppendLine($"case {item.index}: return case{item.alias}({stateParam}this.Item{item.index});");
                        }
                        else
                        {
                            writer.AppendLine($"case {item.index}: case{item.alias}({stateParam}this.Item{item.index}); break;");
                        }
                    }

                    if (hasReturn)
                    {
                        if (hasState)
                        {
                            writer.AppendLine($"default: return caseDefault(state);");
                        }
                        else
                        {
                            writer.AppendLine($"default: return caseDefault();");
                        }
                    }
                    else
                    {
                        if (hasState)
                        {
                            writer.AppendLine($"default: caseDefault(state); break;");
                        }
                        else
                        {
                            writer.AppendLine($"default: caseDefault(); break;");
                        }
                    }
                }
            }
        }
    }
}
