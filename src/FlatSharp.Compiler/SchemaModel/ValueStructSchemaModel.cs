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

    public class ValueStructSchemaModel : BaseSchemaModel
    {
        private readonly FlatBufferObject @struct;

        private readonly List<(int offset, string name, string visibility, string type, string accessor)> fields;
        private readonly List<(string type, string name, List<string> items, Field field)> structVectors;

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

                string fieldType = field.Type.ResolveTypeOrElementTypeName(schema, this.Attributes);
                if (field.Type.BaseType == BaseType.Array)
                {
                    // struct vector
                    int size = field.Type.ElementType.GetScalarSize();

                    List<string> vectorFields = new();
                    for (int i = 0; i < field.Type.FixedLength; ++i)
                    {
                        string name = $"__flatsharp__{field.Name}_{i}";

                        vectorFields.Add(name);
                        this.fields.Add((field.Offset + (i * size), name, "private", fieldType, $"{field.Name}({i})"));
                    }

                    this.structVectors.Add((fieldType, field.Name, vectorFields, field));
                }
                else
                {
                    this.fields.Add((field.Offset, field.Name, "public", fieldType, field.Name));
                }
            }
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
                    writer.AppendLine($"[System.Runtime.InteropServices.FieldOffset({field.offset})]");
                    writer.AppendLine($"[FlatBufferMetadataAttribute(FlatBufferMetadataKind.Accessor, \"{field.accessor}\")]");
                    writer.AppendLine($"{field.visibility} {field.type} {field.name};");
                    writer.AppendLine();
                }
            }
        }

        public override bool SupportsMemoryMarshal(MemoryMarshalBehavior option) => true;
    }
}
