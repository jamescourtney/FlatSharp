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

using FlatSharp.CodeGen;
using FlatSharp.Compiler.Schema;

namespace FlatSharp.Compiler.SchemaModel;

public record StructVectorPropertyFieldModel
{
    public StructVectorPropertyFieldModel(
        BaseReferenceTypeSchemaModel parent,
        Field field,
        int startIndex,
        FlatSharpAttributes attributes)
    {
        FlatSharpInternal.Assert(field.Type.FixedLength != 0, "Struct vectors must have fixed length");

        this.Field = field;
        this.Attributes = attributes;
        this.Parent = parent;
        this.FieldName = field.Name;
        this.Documentation = field.Documentation;

        List<PropertyFieldModel> propertyModels = new List<PropertyFieldModel>();
        for (int i = 0; i < field.Type.FixedLength; ++i)
        {
            var modifiedAttributes = new MutableFlatSharpAttributes(attributes)
            {
                SetterKind = null, // default
            };

            var model = new PropertyFieldModel(
                this.Parent,
                new Field
                {
                    Attributes = field.Attributes,
                    Name = $"__flatsharp__{field.Name}_{i}",
                    Type = field.Type,
                },
                startIndex + i,
                FlatBufferSchemaElementType.StructField,
                $"{field.Name}[{i}]",
                modifiedAttributes)
            {
                ProtectedGetter = FlatSharpCompiler.CommandLineOptions?.MutationTestingMode != true,
            };

            propertyModels.Add(model);
        }

        this.Properties = propertyModels;

        new FlatSharpAttributeValidator(FlatBufferSchemaElementType.StructVector, $"{this.Parent.FullName}.{this.FieldName}")
        {
            SetterKindValidator = k => k == SetterKind.Public ? AttributeValidationResult.Valid : AttributeValidationResult.ValueInvalid,
            WriteThroughValidator = _ => AttributeValidationResult.Valid,
        }.Validate(this.Attributes);
    }

    public BaseReferenceTypeSchemaModel Parent { get; init; }

    public Field Field { get; init; }

    public FlatBufferSchemaElementType ElementType { get; init; }

    public FlatSharpAttributes Attributes { get; init; }

    public string FieldName { get; init; }

    public IEnumerable<string>? Documentation { get; init; }

    public IReadOnlyList<PropertyFieldModel> Properties { get; init; }

    public static bool TryCreate(
        BaseReferenceTypeSchemaModel parent,
        Field field,
        int previousIndex,
        [NotNullWhen(true)] out StructVectorPropertyFieldModel? model)
    {
        model = null;
        if (parent.ElementType != FlatBufferSchemaElementType.ReferenceStruct)
        {
            return false;
        }

        if (field.Type.BaseType != BaseType.Array)
        {
            // handled by StructVectorSchemaModel.
            return false;
        }

        int startIndex = previousIndex + 1;
        model = new StructVectorPropertyFieldModel(parent, field, startIndex, new FlatSharpAttributes(field.Attributes));

        return true;
    }

    public void WriteCode(CodeWriter writer, CompileContext context)
    {
        string structName = $"__{this.Field.Name}_Vector";

        string typeName = this.Field.Type.ResolveTypeOrElementTypeName(this.Parent.Schema, this.Attributes);

        writer.AppendLine($"private {structName}? __{this.Field.Name};");
        writer.AppendLine();

        writer.AppendSummaryComment(this.Documentation);
        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine($"public {structName} {this.Field.Name} => (__{this.Field.Name} ??= new {structName}(this));");
        writer.AppendLine();

        // class is next.
        writer.AppendLine($"public partial class {structName} : System.Collections.Generic.IEnumerable<{typeName}>");
        using (writer.WithBlock())
        {
            writer.AppendLine($"private readonly {this.Parent.FullName} item;");

            // ctor
            writer.AppendLine();
            writer.AppendLine($"public {structName}({this.Parent.FullName} item)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"this.item = item;");
            }

            writer.AppendLine($"public int Count => {this.Field.Type.FixedLength};");

            // indexer
            writer.AppendLine();
            writer.AppendLine($"public {typeName} this[int index]");
            using (writer.WithBlock())
            {
                writer.AppendLine("get");
                using (writer.WithBlock())
                {
                    writer.AppendLine("var thisItem = this.item;");
                    writer.AppendLine("switch (index)");
                    using (writer.WithBlock())
                    {
                        for (int i = 0; i < this.Field.Type.FixedLength; ++i)
                        {
                            writer.AppendLine($"case {i}: return thisItem.{this.Properties[i].FieldName};");
                        }

                        writer.AppendLine($"default: return {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.IndexOutOfRange)}<{typeName}>();");
                    }
                }

                writer.AppendLine();

                if (this.Attributes.SetterKind != SetterKind.None)
                {
                    writer.AppendLine("set");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("var thisItem = this.item;");
                        writer.AppendLine("switch (index)");
                        using (writer.WithBlock())
                        {
                            for (int i = 0; i < this.Field.Type.FixedLength; ++i)
                            {
                                writer.AppendLine($"case {i}: thisItem.{this.Properties[i].FieldName} = value; break;");
                            }

                            writer.AppendLine($"default: {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.IndexOutOfRange)}(); break;");
                        }
                    }
                }
            }

            writer.AppendLine("System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();");
            writer.AppendLine();
            writer.AppendLine($"public System.Collections.Generic.IEnumerator<{typeName}> GetEnumerator()");
            using (writer.WithBlock())
            {
                writer.AppendLine("var thisItem = this.item;");
                for (int i = 0; i < this.Properties.Count; ++i)
                {
                    writer.AppendLine($"yield return thisItem.{this.Properties[i].FieldName};");
                }
            }

            foreach (var collectionType in new[] { $"ReadOnlySpan<{typeName}>", $"IReadOnlyList<{typeName}>" })
            {
                writer.AppendSummaryComment(
                    $"Deep copies the first {this.Properties.Count} items from the source into this struct vector.");

                writer.AppendLine($"public void CopyFrom({collectionType} source)");
                using (writer.WithBlock())
                {
                    if (context.FullyQualifiedCloneMethodName is not null)
                    {
                        writer.AppendLine("var thisItem = this.item;");

                        // Load in reverse so that the JIT can just do a bounds check on the very first item.
                        // This also requries the parameter being a local variable instead of a param.
                        writer.AppendLine("var s = source;");
                        for (int i = this.Properties.Count - 1; i >= 0; --i)
                        {
                            writer.AppendLine($"thisItem.{this.Properties[i].FieldName} = {context.FullyQualifiedCloneMethodName}(s[{i}]);");
                        }
                    }
                }
            }
        }
    }
}
