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

namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;

    internal class ValueStructDefinition : BaseTableOrStructDefinition
    {
        private readonly Dictionary<string, StructMemberModel> fieldNameMap;
        private readonly List<FieldDefinition> fieldDefs;
        private readonly List<(string name, bool @unsafe, List<string> fieldNames)> structVectors;
        private int inlineSize;

        public ValueStructDefinition(
            string name,
            BaseSchemaMember parent) : base(name, parent)
        {
            this.fieldNameMap = new();
            this.fieldDefs = new();
            this.structVectors = new();
        }

        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Struct;

        protected override bool SupportsChildren => false;

        public override void ApplyMetadata(Dictionary<string, string?> metadata)
        {
        }

        public override void AddField(FieldDefinition fieldDefinition)
        {
            this.fieldDefs.Add(fieldDefinition);
        }

        public override void AddStructVector(FieldDefinition definition, int count)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < count; ++i)
            {
                string name = $"__flatsharp_{definition.Name}_{i}";

                this.AddField(definition with
                {
                    GetterModifier = AccessModifier.Private,
                    SetterKind = SetterKind.Private,
                    CustomGetter = $"{definition.Name}({i})",
                    Name = name,
                });

                names.Add(name);
            }

            this.structVectors.Add((definition.Name, definition.IsUnsafeStructVector, names));
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass == CodeWritingPass.Initialization)
            {
                FlatSharpInternal.Assert(this.Parent is not null, "Parent is null");
                TableOrStructDefinition def = new TableOrStructDefinition(this.Name, isTable: false, this.Parent);

                foreach (var f in this.fieldDefs)
                {
                    def.AddField(f with
                    {
                        GetterModifier = AccessModifier.Public,
                        SetterKind = SetterKind.Public,
                    });
                }

                def.WriteCode(writer, context);
                return;
            }
            else if (context.CompilePass == CodeWritingPass.PropertyModeling)
            {
                FlatSharpInternal.Assert(context.PreviousAssembly is not null, "Previous assembly was null");

                Type? refType = context.PreviousAssembly.GetType(this.FullName);
                FlatSharpInternal.Assert(refType is not null, "Unable to find type");

                StructTypeModel structModel = (StructTypeModel)RuntimeTypeModel.CreateFrom(refType);
                this.inlineSize = structModel.PhysicalLayout[0].InlineSize;

                foreach (var field in this.fieldDefs)
                {
                    StructMemberModel? member = structModel.Members.SingleOrDefault(
                        m => m.PropertyInfo.Name == field.Name);

                    FlatSharpInternal.Assert(
                        member is not null,
                        $"Unable to find match for struct offset. Field = {field.Name}");

                    this.fieldNameMap[member.PropertyInfo.Name] = member;
                }
            }

            foreach (var field in this.fieldDefs)
            {
                if (field.GetterModifier != AccessModifier.Public)
                {
                    field.GetterModifier = AccessModifier.Private;
                }
                else
                {
                    field.GetterModifier = AccessModifier.Public;
                }
            }

            writer.AppendLine($"[FlatBufferStruct]");
            writer.AppendLine($"[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = {this.inlineSize})]");
            writer.AppendLine($"public partial struct {this.Name}");
            using (writer.WithBlock())
            {
                for (int i = 0; i < this.fieldDefs.Count; ++i)
                {
                    FieldDefinition fieldDef = this.fieldDefs[i];
                    StructMemberModel memberModel = this.fieldNameMap[fieldDef.Name];

                    string modifier = fieldDef.GetterModifier.ToCSharpString();
                    int offset = memberModel.Offset;
                    string typeName = memberModel.ItemTypeModel.GetGlobalCompilableTypeName();

                    string getter = fieldDef.CustomGetter ?? fieldDef.Name;

                    writer.AppendLine($"[System.Runtime.InteropServices.FieldOffset({offset})]");
                    writer.AppendLine($"[{nameof(FlatBufferMetadataAttribute)}({nameof(FlatBufferMetadataKind)}.{nameof(FlatBufferMetadataKind.Accessor)}, \"{getter}\")]");
                    writer.AppendLine($"{modifier} {typeName} {memberModel.PropertyInfo.Name};");
                }

                foreach (var structVectorDef in this.structVectors)
                {
                    (string name, bool isUnsafeVector, List<string> props) = structVectorDef;
                    context.NeedsUnsafe |= isUnsafeVector;

                    StructMemberModel memberModel = this.fieldNameMap[props[0]];
                    string itemType = memberModel.ItemTypeModel.GetGlobalCompilableTypeName();

                    string type = memberModel.ItemTypeModel.GetGlobalCompilableTypeName();

                    writer.AppendLine($"public int {name}_Length => {props.Count};");

                    writer.AppendLine($"public static ref {type} {name}_Item(ref {this.Name} item, int index)");
                    using (writer.WithBlock())
                    {
                        if (isUnsafeVector)
                        {
                            writer.AppendLine($"if (unchecked((uint)index) >= {props.Count})");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine($"throw new IndexOutOfRangeException();");
                            }

                            writer.AppendLine($"unsafe");
                            using (writer.WithBlock())
                            {
                                writer.AppendLine($"fixed ({this.Name}* pItem = &item)");
                                using (writer.WithBlock())
                                {
                                    // Get byte* pointer to struct.
                                    writer.AppendLine($"byte* pByte = (byte*)pItem;");

                                    // Advance to position of the first item in the fector.
                                    writer.AppendLine($"pByte += {memberModel.Offset};");

                                    // Advance to correct index.
                                    writer.AppendLine($"pByte += index * sizeof({itemType});");

                                    // return ref.
                                    writer.AppendLine($"return ref {typeof(Unsafe).GetGlobalCompilableTypeName()}.AsRef<{itemType}>(pByte);");
                                }
                            }
                        }
                        else
                        {
                            writer.AppendLine("switch (index)");
                            using (writer.WithBlock())
                            {
                                for (int i = 0; i < props.Count; ++i)
                                {
                                    writer.AppendLine($"case {i}: return ref item.{props[i]};");
                                }

                                writer.AppendLine("default: throw new IndexOutOfRangeException();");
                            }
                        }
                    }
                }
            }

            if (this.structVectors.Count > 0)
            {
                writer.AppendLine($"public static class {this.Name}__FlatSharpExtensions");
                using (writer.WithBlock())
                {
                    foreach (var structVectorDef in this.structVectors)
                    {
                        (string name, _, List<string> props) = structVectorDef;
                        StructMemberModel memberModel = this.fieldNameMap[props[0]];
                        string type = memberModel.ItemTypeModel.GetGlobalCompilableTypeName();

                        writer.AppendLine($"public static ref {type} {name}(this ref {this.Name} item, int index)");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine($"return ref {this.Name}.{name}_Item(ref item, index);");
                        }
                    }
                }
            }
        }
    }
}
