/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Compiler.SchemaModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FlatSharp;
    using FlatSharp.Compiler.Schema;
    using System.Diagnostics.CodeAnalysis;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;

    public class UnionSchemaModel : BaseSchemaModel
    {
        private readonly FlatBufferEnum union;

        private UnionSchemaModel(Schema schema, FlatBufferEnum union) : base(schema, union.Name, new FlatSharpAttributes(union.Attributes))
        {
            FlatSharpInternal.Assert(union.UnderlyingType.BaseType == BaseType.UType, "Expecting utype");

            this.union = union;
        }

        public static bool TryCreate(Schema schema, FlatBufferEnum union, [NotNullWhen(true)] out UnionSchemaModel? model)
        {
            if (union.UnderlyingType.BaseType != BaseType.UType)
            {
                model = null;
                return false;
            }

            model = new UnionSchemaModel(schema, union);
            return true;
        }

        public override string DeclaringFile => this.union.DeclarationFile;

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Union;

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            List<(string resolvedType, EnumVal value)> innerTypes = new List<(string, EnumVal)>();
            foreach (var inner in this.union.Values.Select(x => x.Value))
            {
                // Skip "none".
                if (inner.Value == 0)
                {
                    FlatSharpInternal.Assert(inner.Key == "NONE", "Expecting discriminator 0 to be 'None'");
                    continue;
                }

                FlatSharpInternal.Assert(inner.UnionType is not null, "Union type was null");

                long discriminator = inner.Value;
                string typeName = inner.UnionType.ResolveTypeOrElementTypeName(this.Schema, this.Attributes);
                innerTypes.Add((typeName, inner));
            }

            string interfaceName = $"IFlatBufferUnion<{string.Join(", ", innerTypes.Select(x => x.resolvedType))}>";

            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial struct {this.Name} : {interfaceName}");
            using (writer.WithBlock())
            {                
                // Generate an internal type enum.
                writer.AppendLine("public enum ItemKind : byte");
                using (writer.WithBlock())
                {
                    foreach (var item in this.union.Values)
                    {
                        writer.AppendLine($"{item.Value.Key} = {item.Value.Value},");
                    }
                }

                writer.AppendLine();
                writer.AppendLine("private readonly object value;");

                writer.AppendLine();
                writer.AppendLine("public ItemKind Kind => (ItemKind)this.Discriminator;");

                writer.AppendLine();
                writer.AppendLine("public byte Discriminator { get; }");

                foreach (var item in innerTypes)
                {
                    Type? propertyClrType = null;
                    if (context.CompilePass > CodeWritingPass.Initialization)
                    {
                        Type? previousType = context.PreviousAssembly?.GetType(this.FullName);
                        FlatSharpInternal.Assert(previousType is not null, "PreviousType was null");

                        propertyClrType = previousType
                            .GetProperty($"Item{item.value.Value}", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?
                            .PropertyType;

                        FlatSharpInternal.Assert(propertyClrType is not null, "Couldn't find property");
                    }

                    writer.AppendLine();
                    writer.AppendLine($"public {this.Name}({item.resolvedType} value)");
                    using (writer.WithBlock())
                    {
                        if (propertyClrType?.IsValueType == false)
                        {
                            writer.AppendLine("if (value is null)");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine("throw new ArgumentNullException(nameof(value));");
                            }
                        }

                        writer.AppendLine($"this.value = value;");
                        writer.AppendLine($"this.Discriminator = {item.value.Value};");
                    }

                    writer.AppendLine();
                    writer.AppendLine($"public {item.resolvedType} {item.value.Key} => this.Item{item.value.Value};");

                    writer.AppendLine();
                    writer.AppendLine($"public {item.resolvedType} Item{item.value.Value}");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("get");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine($"if (this.Discriminator != {item.value.Value})");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine("throw new InvalidOperationException();");
                            }

                            writer.AppendLine($"return ({item.resolvedType})this.value;");
                        }
                    }

                    string notNullWhen = string.Empty;
                    string nullableReference = string.Empty;

                    if (propertyClrType is not null)
                    {
                        if (!propertyClrType.IsValueType)
                        {
                            nullableReference = "?";

                            if (context.Options.NullableWarnings == true)
                            {
                                notNullWhen = $"[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] ";
                            }
                        }
                    }

                    writer.AppendLine();
                    writer.AppendLine($"public bool TryGet({notNullWhen}out {item.resolvedType}{nullableReference} value)");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"if (this.Discriminator != {item.value.Value})");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine("value = default;");
                            writer.AppendLine("return false;");
                        }

                        writer.AppendLine($"value = ({item.resolvedType})this.value;");
                        writer.AppendLine("return true;");
                    }
                }

                // Switch methods.
                this.WriteSwitchMethod(writer, true, true, innerTypes);
                this.WriteSwitchMethod(writer, true, false, innerTypes);
                this.WriteSwitchMethod(writer, false, true, innerTypes);
                this.WriteSwitchMethod(writer, false, false, innerTypes);
            }
        }

        private void WriteSwitchMethod(
            CodeWriter writer,
            bool hasReturn,
            bool hasState,
            List<(string resolvedType, EnumVal value)> components)
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
                    string argName = $"case{item.value.Key}";
                    args.Add(argName);

                    writer.AppendLine($", {itemDelegateType(item.resolvedType)} {argName}");
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
                            writer.AppendLine($"case {item.value.Value}: return case{item.value.Key}({stateParam}this.Item{item.value.Value});");
                        }
                        else
                        {
                            writer.AppendLine($"case {item.value.Value}: case{item.value.Key}({stateParam}this.Item{item.value.Value}); break;");
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
