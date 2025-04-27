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
using FlatSharp.Attributes;
using FlatSharp.TypeModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FlatSharp.Internal;
using FlatSharp.CodeGen;

namespace FlatSharp.Compiler.SchemaModel;

public class ValueStructSchemaModel : BaseSchemaModel
{
    private readonly FlatBufferObject @struct;

    private readonly List<ValueStructFieldModel> fields;
    private readonly List<ValueStructVectorModel> structVectors;

    private ValueStructSchemaModel(Schema.Schema schema, FlatBufferObject @struct) : base(schema, @struct.Name, new FlatSharpAttributes(@struct.Attributes))
    {
        FlatSharpInternal.Assert(@struct.IsStruct, "Expecting struct");
        FlatSharpInternal.Assert(this.Attributes.ValueStruct == true, "Expecting value struct");

        this.@struct = @struct;
        this.DeclaringFile = @struct.DeclarationFile;
        this.fields = new();
        this.structVectors = new();

        foreach (Field field in this.@struct.Fields.OrderBy(x => x.Id))
        {
            IFlatSharpAttributes attrs = new FlatSharpAttributes(field.Attributes);

            string fieldType = field.Type.ResolveTypeOrElementTypeName(schema, this.Attributes);
            if (field.Type.BaseType == BaseType.Array)
            {
                // struct vector
                int size = field.Type.ElementType.GetScalarSize();

                List<string> vectorFields = new();
                for (int i = 0; i < field.Type.FixedLength; ++i)
                {
                    string name = $"__flatsharp__{field.Name}_{i}";

                    MutableFlatSharpAttributes tempAttrs = new MutableFlatSharpAttributes(attrs)
                    {
                        UnsafeStructVector = null,
                    };

                    vectorFields.Add(name);
                    this.fields.Add(new(field.Offset + (i * size), name, "private", fieldType, $"{field.Name}({i})", null, this, tempAttrs));
                }

                this.structVectors.Add(new(fieldType, field.Name, vectorFields, this, field.Documentation, attrs));
            }
            else
            {
                this.fields.Add(new(field.Offset, field.Name, "public", fieldType, field.Name, field.Documentation, this, attrs));
            }
        }

        this.AttributeValidator.MemoryMarshalValidator = _ => AttributeValidationResult.Valid;
        this.AttributeValidator.ExternValidator = _ => AttributeValidationResult.Valid;
    }

    public static bool TryCreate(Schema.Schema schema, FlatBufferObject @struct, [NotNullWhen(true)] out ValueStructSchemaModel? model)
    {
        model = null;
        if (!@struct.IsStruct)
        {
            return false;
        }

        if (@struct.Attributes?.Any(x => x.Key == MetadataKeys.ValueStruct) != true)
        {
            return false;
        }

        model = new ValueStructSchemaModel(schema, @struct);
        return true;
    }

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.ValueStruct;

    public override string DeclaringFile { get; }

    public override string FriendlyName => this.@struct.OriginalName ?? this.FullName;

    protected override void OnValidate()
    {
        // TODO   
    }

    protected override void OnWriteCode(CodeWriter writer, CompileContext context)
    {
        writer.AppendSummaryComment(this.@struct.Documentation);

        string memMarshalBehavior = string.Empty;
        if (this.Attributes.MemoryMarshalBehavior is not null)
        {
            memMarshalBehavior = $"{nameof(FlatBufferStructAttribute.MemoryMarshalBehavior)} = {nameof(MemoryMarshalBehavior)}.{this.Attributes.MemoryMarshalBehavior}";
        }

        if (this.Attributes.ExternalTypeName is not null)
        {
            writer.AppendLine($"[FlatSharp.Internal.ExternalDefinitionAttribute]");
        }

        writer.AppendLine($"[FlatBufferStruct({memMarshalBehavior})]");
        string size = string.Empty;
        if (context.CompilePass > CodeWritingPass.Initialization)
        {
            // load size from type.
            Type? previousType = context.PreviousAssembly?.GetType(this.FullName);
            FlatSharpInternal.Assert(previousType is not null, "Previous type was null");

            ITypeModel model = context.TypeModelContainer.CreateTypeModel(previousType);
            FlatSharpInternal.Assert(model.PhysicalLayout.Length == 1, "Expected physical layout length of 1.");

            size = $", Size = {model.PhysicalLayout[0].InlineSize}";
        }

        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine($"[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit{size})]");
        writer.AppendLine($"public partial struct {this.Name}");
        if (context.Options.GenerateMethods)
        {
            writer.AppendLine($": System.IEquatable<{this.Name}>");
        }
        using (writer.WithBlock())
        {
            foreach (var field in this.fields)
            {
                writer.AppendSummaryComment(field.Documentation);
                writer.AppendLine($"[System.Runtime.InteropServices.FieldOffset({field.Offset})]");
                writer.AppendLine($"[FlatBufferMetadataAttribute(FlatBufferMetadataKind.Accessor, \"\", \"{field.Accessor}\")]");
                field.Attributes.EmitAsMetadata(writer);
                writer.AppendLine($"{field.Visibility} {field.TypeName} {field.Name};");
                writer.AppendLine();
            }

            if (context.Options.GenerateMethods)
            {
                string typeNames = string.Join(", ", this.fields.Select(x => x.TypeName));
                string names = string.Join(", ", this.fields.Select(x => x.Name));
                string tupleType = this.fields.Count == 0 ? "System.ValueTuple" : this.fields.Count == 1 ? $"System.ValueTuple<{typeNames}>" : $"({typeNames})";
                string tupleValue = this.fields.Count < 2 ? $"System.ValueTuple.Create({names})" : $"({names})";
                writer.AppendLine($"public {tupleType} ToTuple() => {tupleValue};");
                writer.AppendLine($"public override bool Equals(object? obj) => obj is {this.Name} other && this.Equals(other);");
                writer.AppendLine($"public bool Equals({this.Name} other) => ToTuple().Equals(other.ToTuple());");
                writer.AppendLine($"public static bool operator ==({this.Name} left, {this.Name} right) => left.Equals(right);");
                writer.AppendLine($"public static bool operator !=({this.Name} left, {this.Name} right) => !left.Equals(right);");
                writer.AppendLine("public override int GetHashCode() => ToTuple().GetHashCode();");
                // This matches C# records
                string fieldStrings = string.Join(", ", this.fields.Select(x => $"{x.Name} = {{this.{x.Name}}}"));
                string fieldStringsWithSpace = this.fields.Count == 0 ? " " : $" {fieldStrings} ";
                writer.AppendLine($"public override string ToString() => $\"{this.Name} {{{{{fieldStringsWithSpace}}}}}\";");
            }

            foreach (var sv in this.structVectors)
            {
                writer.AppendSummaryComment($"Gets the number of items in the {sv.Name} vector.");
                writer.AppendLine($"public int {sv.Name}_Length => {sv.Properties.Count};");

                writer.AppendLine();

                writer.AppendSummaryComment(sv.Documentation);
                writer.AppendLine($"public static ref {sv.TypeName} {sv.Name}_Item(ref {this.Name} item, int index)");
                using (writer.WithBlock())
                {
                    if (sv.Attributes.UnsafeStructVector == true)
                    {
                        writer.AppendLine($"if (unchecked((uint)index) >= {sv.Properties.Count})");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.IndexOutOfRange)}();");
                        }

                        writer.AppendLine($"return ref System.Runtime.CompilerServices.Unsafe.Add(ref item.{sv.Properties[0]}, index);");
                    }
                    else
                    {
                        writer.AppendLine("switch (index)");
                        using (writer.WithBlock())
                        {
                            FlatSharpInternal.Assert(sv.Properties.Count >= 1, "Expected at least 1 element in struct vector.");

                            for (int i = 0; i < sv.Properties.Count; ++i)
                            {
                                var item = sv.Properties[i];
                                writer.AppendLine($"case {i}: return ref item.{item};");
                            }

                            writer.AppendLine($"default: {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.IndexOutOfRange)}(); goto case 0;");
                        }
                    }
                }
            }
        }

        // extensions class.
        if (this.structVectors.Any())
        {
            writer.AppendLine($"public static class {this.Name}__FlatSharpExtensions");
            using (writer.WithBlock())
            {
                foreach (var sv in this.structVectors)
                {
                    writer.AppendSummaryComment(sv.Documentation);
                    writer.AppendLine($"public static ref {sv.TypeName} {sv.Name}(this ref {this.Name} item, int index)");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"return ref {this.Name}.{sv.Name}_Item(ref item, index);");
                    }
                }
            }
        }
    }

    private class ValueStructVectorModel
    {
        public ValueStructVectorModel(
            string type,
            string name,
            List<string> properties,
            ValueStructSchemaModel parent,
            IEnumerable<string>? documentation,
            IFlatSharpAttributes attributes)
        {
            this.Name = name;
            this.TypeName = type;
            this.Name = name;
            this.Properties = properties;
            this.Attributes = attributes;
            this.Documentation = documentation;

            new FlatSharpAttributeValidator(FlatBufferSchemaElementType.ValueStructField, $"{parent.Name}.{name}")
            {
                UnsafeStructVectorValidator = _ => AttributeValidationResult.Valid,
            }.Validate(attributes);
        }

        public IReadOnlyList<string> Properties { get; }

        public string Name { get; }

        public string TypeName { get; }

        public IEnumerable<string>? Documentation { get; }

        public IFlatSharpAttributes Attributes { get; }
    }

    private class ValueStructFieldModel
    {
        public ValueStructFieldModel(
            int offset,
            string name,
            string visibility,
            string type,
            string accessor,
            IEnumerable<string>? documentation,
            ValueStructSchemaModel parent,
            IFlatSharpAttributes attributes)
        {
            this.Offset = offset;
            this.Name = name;
            this.Visibility = visibility;
            this.TypeName = type;
            this.Accessor = accessor;
            this.Documentation = documentation;
            this.Attributes = attributes;

            new FlatSharpAttributeValidator(FlatBufferSchemaElementType.ValueStructField, $"{parent.Name}.{name}").Validate(attributes);
        }

        public int Offset { get; }

        public string Name { get; }

        public string Visibility { get; }

        public string TypeName { get; }

        public string Accessor { get; }

        public IEnumerable<string>? Documentation { get; }

        public IFlatSharpAttributes Attributes { get; }
    }
}
