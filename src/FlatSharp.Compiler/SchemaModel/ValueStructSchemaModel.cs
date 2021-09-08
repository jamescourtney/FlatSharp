﻿/*
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

    public class ValueStructSchemaModel : BaseSchemaModel
    {
        private readonly FlatBufferObject @struct;

        private readonly List<ValueStructFieldModel> fields;
        private readonly List<ValueStructVectorModel> structVectors;

        private ValueStructSchemaModel(Schema schema, FlatBufferObject @struct) : base(schema, @struct.Name, new FlatSharpAttributes(@struct.Attributes))
        {
            FlatSharpInternal.Assert(@struct.IsStruct, "Expecting struct");
            FlatSharpInternal.Assert(this.Attributes.ValueStruct == true, "Expecting value struct");

            this.@struct = @struct;
            this.fields = new();
            this.structVectors = new();

            foreach (var kvp in this.@struct.Fields.OrderBy(x => x.Value.Id))
            {
                Field field = kvp.Value;
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
                        this.fields.Add(new(field.Offset + (i * size), name, "private", fieldType, $"{field.Name}({i})", this, tempAttrs));
                    }

                    this.structVectors.Add(new(fieldType, field.Name, vectorFields, this, attrs));
                }
                else
                {
                    this.fields.Add(new(field.Offset, field.Name, "public", fieldType, field.Name, this, attrs));
                }
            }

            this.AttributeValidator.MemoryMarshalValidator = _ => AttributeValidationResult.Valid;
        }

        public static bool TryCreate(Schema schema, FlatBufferObject @struct, [NotNullWhen(true)] out ValueStructSchemaModel? model)
        {
            model = null;
            if (!@struct.IsStruct)
            {
                return false;
            }

            if (@struct.Attributes?.ContainsKey(MetadataKeys.ValueStruct) != true)
            {
                return false;
            }

            model = new ValueStructSchemaModel(schema, @struct);
            return true;
        }

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.ValueStruct;

        public override string DeclaringFile => this.@struct.DeclarationFile;

        protected override void OnValidate()
        {
            // TODO   
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            string memMarshalBehavior = string.Empty;
            if (this.Attributes.MemoryMarshalBehavior is not null)
            {
                memMarshalBehavior = $"{nameof(FlatBufferStructAttribute.MemoryMarshalBehavior)} = {nameof(MemoryMarshalBehavior)}.{this.Attributes.MemoryMarshalBehavior}";
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

            writer.AppendLine($"[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit{size})]");
            writer.AppendLine($"public partial struct {this.Name}");
            using (writer.WithBlock())
            {
                foreach (var field in this.fields)
                {
                    writer.AppendLine($"[System.Runtime.InteropServices.FieldOffset({field.Offset})]");
                    writer.AppendLine($"[FlatBufferMetadataAttribute(FlatBufferMetadataKind.Accessor, \"{field.Accessor}\")]");
                    writer.AppendLine($"{field.Visibility} {field.TypeName} {field.Name};");
                    writer.AppendLine();
                }

                foreach (var sv in this.structVectors)
                {
                    writer.AppendLine($"public int {sv.Name}_Length => {sv.Properties.Count};");
                    writer.AppendLine();
                    writer.AppendLine($"public static ref {sv.TypeName} {sv.Name}_Item(ref {this.Name} item, int index)");
                    using (writer.WithBlock())
                    {
                        if (sv.Attributes.UnsafeStructVector == true)
                        {
                            writer.AppendLine($"if (unchecked((uint)index) >= {sv.Properties.Count})");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine("throw new IndexOutOfRangeException();");
                            }

                            writer.AppendLine($"return ref System.Runtime.CompilerServices.Unsafe.Add(ref item.{sv.Properties[0]}, index);");
                        }
                        else
                        {
                            writer.AppendLine("switch (index)");
                            using (writer.WithBlock())
                            {
                                for (int i = 0; i < sv.Properties.Count; ++i)
                                {
                                    var item = sv.Properties[i];
                                    writer.AppendLine($"case {i}: return ref item.{item};");
                                }

                                writer.AppendLine("default: throw new IndexOutOfRangeException();");
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
            public ValueStructVectorModel(string type, string name, List<string> properties, ValueStructSchemaModel parent, IFlatSharpAttributes attributes)
            {
                this.Name = name;
                this.TypeName = type;
                this.Name = name;
                this.Properties = properties;
                this.Attributes = attributes;

                new FlatSharpAttributeValidator(FlatBufferSchemaElementType.ValueStructField, $"{parent.Name}.{name}")
                {
                    UnsafeStructVectorValidator = _ => AttributeValidationResult.Valid,
                }.Validate(attributes);
            }

            public IReadOnlyList<string> Properties { get; }

            public string Name { get; }

            public string TypeName { get; }

            public IFlatSharpAttributes Attributes { get; }
        }

        private class ValueStructFieldModel
        {
            public ValueStructFieldModel(int offset, string name, string visibility, string type, string accessor, ValueStructSchemaModel parent, IFlatSharpAttributes attributes)
            {
                this.Offset = offset;
                this.Name = name;
                this.Visibility = visibility;
                this.TypeName = type;
                this.Accessor = accessor;

                new FlatSharpAttributeValidator(FlatBufferSchemaElementType.ValueStructField, $"{parent.Name}.{name}").Validate(attributes);
            }

            public int Offset { get; }

            public string Name { get; }

            public string Visibility { get; }

            public string TypeName { get;  }

            public string Accessor { get; }
        }
    }
}
