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
using System.Reflection.Metadata;

namespace FlatSharp.Compiler.SchemaModel;

public class ValueUnionSchemaModel : BaseSchemaModel
{
    private readonly FlatBufferEnum union;

    private ValueUnionSchemaModel(Schema.Schema schema, FlatBufferEnum union) : base(schema, union.Name, new FlatSharpAttributes(union.Attributes))
    {
        FlatSharpInternal.Assert(union.UnderlyingType.BaseType == BaseType.UType, "Expecting utype");

        this.DeclaringFile = union.DeclarationFile;
        this.union = union;

        this.AttributeValidator.UnsafeUnionValidator = b => AttributeValidationResult.Valid;
    }

    public static bool TryCreate(
        Schema.Schema schema,
        FlatBufferEnum union,
        CompilerOptions context,
        [NotNullWhen(true)] out BaseSchemaModel? model)
    {
        if (union.UnderlyingType.BaseType != BaseType.UType)
        {
            model = null;
            return false;
        }

        if (context.GeneratePoolableObjects)
        {
            model = new ReferenceUnionSchemaModel(schema, union);
        }
        else
        {
            model = new ValueUnionSchemaModel(schema, union);
        }

        return true;
    }

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Union;

    public override string DeclaringFile { get; }

    protected override void OnWriteCode(CodeWriter writer, CompileContext context)
    {
        int maxSize = 0;
        bool generateUnsafeItemsOriginal = context.CompilePass >= CodeWritingPass.LastPass && this.Attributes.UnsafeUnion == true;
        bool generateUnsafeItems = generateUnsafeItemsOriginal;

        HashSet<string> seenTypes = new();
        List<(string resolvedType, EnumVal value, int? size)> innerTypes = new List<(string, EnumVal, int?)>();
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
            if (!seenTypes.Add(typeName))
            {
                ErrorContext.Current.RegisterError($"FlatSharp unions may not contain duplicate types. Union = {this.FullName}");
                continue;
            }

            int? size = null;
            Type? type = context.PreviousAssembly?.GetType(typeName);

            // Must be a struct.
            if (type?.IsValueType == true && generateUnsafeItems)
            {
                FlatSharpInternal.Assert(type.StructLayoutAttribute is not null, "Struct layout attribute should not be null");
                FlatSharpInternal.Assert(type.StructLayoutAttribute.Size != 0, "Struct layout attribute should have nonzero size");
                size = type.StructLayoutAttribute.Size;
                maxSize = Math.Max(size.Value, maxSize);
            }
            else
            {
                generateUnsafeItems = false;
            }

            innerTypes.Add((typeName, inner, size));
        }

        generateUnsafeItems = generateUnsafeItems && maxSize > 0;

        if (generateUnsafeItemsOriginal && !generateUnsafeItems)
        {
            ErrorContext.Current.RegisterError($"FlatBufferion '{this.FullName}' declares the '{MetadataKeys.UnsafeUnion}' attribute. '{MetadataKeys.UnsafeUnion}' is only valid on unions consisting only of value types.");
        }

        string @unsafe = string.Empty;
        if (generateUnsafeItems)
        {
            @unsafe = "unsafe";
        }

        string interfaceName = $"IFlatBufferUnion<{string.Join(", ", innerTypes.Select(x => x.resolvedType))}>";

        writer.AppendSummaryComment(this.union.Documentation);
        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        writer.AppendLine($"public {@unsafe} partial struct {this.Name} : {interfaceName}");
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

            if (generateUnsafeItems)
            {
                writer.AppendLine($"private fixed byte valueBytes[{maxSize}];");
            }
            else
            {
                writer.AppendLine("private readonly object value;");
            }

            writer.AppendLine();
            writer.AppendLine("public ItemKind Kind => (ItemKind)this.Discriminator;");

            writer.AppendLine();
            writer.AppendLine("public byte Discriminator { get; }");

            if (!generateUnsafeItems && context.Options.GenerateMethods)
            {
                string item = this.union.Values.Count == 0 ? " " : $" this.value ";
                writer.AppendLine($"public override string ToString() => $\"{this.Name} {{{{ {{{item}}} }}}}\";");
            }

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

                this.WriteConstructor(writer, item.resolvedType, item.value, propertyClrType, generateUnsafeItems);
                this.WriteImplicitOperator(writer, item.resolvedType);
                this.WriteUncheckedGetItemMethod(writer, item.resolvedType, item.value, propertyClrType, generateUnsafeItems);

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
                            writer.AppendLine($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_UnionIsNotOfType)}();");
                        }

                        writer.AppendLine($"return this.UncheckedGetItem{item.value.Value}();");
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

                    writer.AppendLine($"value = this.UncheckedGetItem{item.value.Value}();");
                    writer.AppendLine("return true;");
                }
            }

            this.WriteAcceptMethod(writer, innerTypes);
            this.WriteMatchMethod(writer, innerTypes);
        }
    }

    private void WriteAcceptMethod(
        CodeWriter writer,
        List<(string resolvedType, EnumVal value, int? size)> components)
    {
        string visitorBaseType = $"IFlatBufferUnionVisitor<TReturn, {string.Join(", ", components.Select(x => x.resolvedType))}>";

        writer.AppendSummaryComment("A convenience interface for implementing a visitor.");
        writer.AppendLine($"public interface Visitor<TReturn> : {visitorBaseType} {{ }}");

        writer.AppendSummaryComment("Accepts a visitor into this FlatBufferUnion. Use a value-type Visitor for maximum performance.");
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
                    writer.AppendLine($"case {index}: return visitor.Visit(this.UncheckedGetItem{item.value.Value}());");
                }

                writer.AppendLine($"default:");
                using (writer.IncreaseIndent())
                {
                    writer.AppendLine($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.Name}>(disc);");
                    writer.AppendLine("return default(TReturn);");
                }
            }
        }
    }


    private void WriteMatchMethod(
        CodeWriter writer,
        List<(string resolvedType, EnumVal value, int? size)> components)
    {
        List<string> parameters = components.Select(x => $"Func<{x.resolvedType}, TReturn> case{x.value.Key}").ToList();

        writer.AppendSummaryComment(
            "Performs a match operation on this Union.",
            "For cases where performance is important, prefer the Accept method to this one.");
        writer.AppendLine($"public TReturn Match<TReturn>({string.Join(", ", parameters)})");
        using (writer.WithBlock())
        {
            writer.AppendLine("var disc = this.Discriminator;");
            writer.AppendLine("switch (disc)");
            using (writer.WithBlock())
            {
                foreach (var item in components)
                {
                    long index = item.value.Value;
                    writer.AppendLine($"case {index}: return case{item.value.Key}(this.UncheckedGetItem{item.value.Value}());");
                }

                writer.AppendLine($"default:");
                using (writer.IncreaseIndent())
                {
                    writer.AppendLine($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.Name}>(disc);");
                    writer.AppendLine("return default(TReturn);");
                }
            }
        }
    }

    private void WriteUncheckedGetItemMethod(CodeWriter writer, string resolvedType, EnumVal unionValue, Type? propertyType, bool generateUnsafeItems)
    {
        if (propertyType?.IsValueType == true && generateUnsafeItems)
        {
            writer.AppendLine();
            writer.AppendLine($"private {resolvedType} UncheckedGetItem{unionValue.Value}()");
            using (writer.WithBlock())
            {
                writer.AppendLine($"FlatSharpInternal.AssertSizeOf<{resolvedType}>({propertyType.StructLayoutAttribute!.Size});");
                writer.AppendLine($"fixed (byte* pByte = this.valueBytes)");
                using (writer.WithBlock())
                {
                    writer.AppendLine($"var localSpan = new Span<byte>(pByte, {propertyType.StructLayoutAttribute.Size});");
                    writer.AppendLine($"return System.Runtime.InteropServices.MemoryMarshal.Read<{resolvedType}>(localSpan);");
                }
            }
        }
        else
        {
            writer.AppendLine();
            writer.AppendLine($"private {resolvedType} UncheckedGetItem{unionValue.Value}()");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return ({resolvedType})this.value;");
            }
        }
    }

    private void WriteConstructor(CodeWriter writer, string resolvedType, EnumVal unionValue, Type? propertyType, bool generateUnsafeItems)
    {
        writer.AppendLine($"public {this.Name}({resolvedType} value)");
        using (writer.WithBlock())
        {
            if (propertyType?.IsValueType == false)
            {
                writer.AppendLine("if (value is null)");
                using (writer.WithBlock())
                {
                    writer.AppendLine($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.ArgumentNull)}(nameof(value));");
                }
            }

            writer.AppendLine($"this.Discriminator = {unionValue.Value};");

            if (generateUnsafeItems && propertyType?.IsValueType == true)
            {
                writer.AppendLine($"FlatSharpInternal.AssertSizeOf<{resolvedType}>({propertyType.StructLayoutAttribute!.Size});");
                writer.AppendLine($"fixed (byte* pByte = this.valueBytes)");
                using (writer.WithBlock())
                {
                    writer.AppendLine($"var localSpan = new Span<byte>(pByte, {propertyType.StructLayoutAttribute.Size});");
                    writer.BeginPreprocessorIf(CSharpHelpers.Net8PreprocessorVariable, "System.Runtime.InteropServices.MemoryMarshal.Write(localSpan, in value);")
                          .Else("System.Runtime.InteropServices.MemoryMarshal.Write(localSpan, ref value);")
                          .Flush();
                }
            }
            else
            {
                writer.AppendLine($"this.value = value;");
            }
        }
    }

    private void WriteImplicitOperator(CodeWriter writer, string resolvedType)
    {
        writer.AppendLine($"public static implicit operator {this.Name}({resolvedType} value)");
        using (writer.WithBlock())
        {
            writer.AppendLine($"return new {this.Name}(value);");
        }
    }
}
