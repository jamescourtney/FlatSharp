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

using System.Linq;
using FlatSharp.Compiler.Schema;
using FlatSharp.CodeGen;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FlatSharp.Compiler.SchemaModel;

public class ReferenceUnionSchemaModel : BaseSchemaModel
{
    private readonly FlatBufferEnum union;

    internal ReferenceUnionSchemaModel(Schema.Schema schema, FlatBufferEnum union) : base(schema, union.Name, new FlatSharpAttributes(union.Attributes))
    {
        FlatSharpInternal.Assert(union.UnderlyingType.BaseType == BaseType.UType, "Expecting utype");

        this.DeclaringFile = union.DeclarationFile;
        this.union = union;
    }

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.PoolableUnion;

    public override string DeclaringFile { get; }

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

        writer.AppendSummaryComment(this.union.Documentation);
        writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        writer.AppendLine($"public partial class {this.Name} : object, {interfaceName}, IPoolableObject");
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
            writer.AppendLine("protected int discriminator;");

            writer.AppendLine();
            writer.AppendLine("public ItemKind Kind => (ItemKind)this.Discriminator;");

            writer.AppendLine();
            writer.AppendLine("public byte Discriminator => (byte)this.discriminator;");

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

                this.WriteConstructor(writer, item.resolvedType, item.value, propertyClrType);

                string nullable = string.Empty;
                if (propertyClrType?.IsValueType == false)
                {
                    nullable = "?";
                }

                writer.AppendLine();
                writer.AppendLine($"protected {item.resolvedType}{nullable} value_{item.value.Value};");

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

                        writer.AppendLine($"return this.value_{item.value.Value}!;");
                    }

                    writer.AppendLine("protected set");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"this.value_{item.value.Value} = value;");
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

                    writer.AppendLine($"value = this.value_{item.value.Value}!;");
                    writer.AppendLine("return true;");
                }
            }

            this.WriteDefaultConstructor(writer);
            this.WriteReturnToPool(writer);
            this.WriteAcceptMethod(writer, innerTypes);
        }
    }

    private void WriteReturnToPool(CodeWriter writer)
    {
        writer.AppendInheritDoc();
        writer.AppendLine($"public virtual void ReturnToPool(bool unsafeForce = false) {{ }}");
    }

    private void WriteAcceptMethod(
        CodeWriter writer,
        List<(string resolvedType, EnumVal value)> components)
    {
        string visitorBaseType = $"IFlatBufferUnionVisitor<TReturn, {string.Join(", ", components.Select(x => x.resolvedType))}>";

        writer.AppendSummaryComment("A convenience interface for implementing a visitor.");
        writer.AppendLine($"public interface Visitor<TReturn> : {visitorBaseType} {{ }}");

        writer.AppendSummaryComment("Accepts a visitor into this FlatBufferUnion.");
        writer.AppendLine($"public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)");
        writer.AppendLine($"   where TVisitor : {visitorBaseType}");
        using (writer.WithBlock())
        {
            writer.AppendLine("var disc = this.Discriminator;");
            writer.AppendLine("switch (disc)");
            using (writer.WithBlock())
            {
                foreach (var item in components)
                {
                    long index = item.value.Value;
                    writer.AppendLine($"case {index}: return visitor.Visit(this.value_{item.value.Value}!);");
                }

                writer.AppendLine($"default: throw new {typeof(InvalidOperationException).GetCompilableTypeName()}(\"Unexpected discriminator: \" + disc);");
            }
        }
    }

    private void WriteConstructor(CodeWriter writer, string resolvedType, EnumVal unionValue, Type? propertyType)
    {
        writer.AppendLine($"public {this.Name}({resolvedType} value)");
        using (writer.WithBlock())
        {
            if (propertyType?.IsValueType == false)
            {
                writer.AppendLine("if (value is null)");
                using (writer.WithBlock())
                {
                    writer.AppendLine("throw new ArgumentNullException(nameof(value));");
                }
            }

            writer.AppendLine($"this.discriminator = {unionValue.Value};");
            writer.AppendLine($"this.value_{unionValue.Value} = value;");
        }
    }

    private void WriteDefaultConstructor(CodeWriter writer)
    {
        writer.AppendLine($"protected {this.Name}()");
        using (writer.WithBlock())
        {
        }
    }
}
