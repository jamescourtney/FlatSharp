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
    using FlatSharp.TypeModel;

    internal class ValueStructDefinition : BaseTableOrStructDefinition
    {
        private readonly List<StructMemberModel> fields;

        public ValueStructDefinition(
            string name,
            BaseSchemaMember parent) : base(name, parent)
        {
            this.fields = new();
        }

        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Struct;

        protected override bool SupportsChildren => false;

        public override void ApplyMetadata(Dictionary<string, string?> metadata)
        {
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass == CodeWritingPass.Initialization)
            {
                FlatSharpInternal.Assert(this.Parent is not null, "Parent is null");
                TableOrStructDefinition def = new TableOrStructDefinition(this.Name, isTable: false, this.Parent);

                def.Fields.AddRange(this.Fields);
                def.WriteCode(writer, context);
                return;
            }
            else if (context.CompilePass == CodeWritingPass.PropertyModeling)
            {
                FlatSharpInternal.Assert(context.PreviousAssembly is not null, "Previous assembly was null");

                Type? refType = context.PreviousAssembly.GetType(this.FullName);
                FlatSharpInternal.Assert(refType is not null, "Unable to find type");

                StructTypeModel structModel = (StructTypeModel)RuntimeTypeModel.CreateFrom(refType);

                foreach (var field in this.Fields)
                {
                    StructMemberModel? member = structModel.Members.SingleOrDefault(
                        m => m.PropertyInfo.Name == field.Name);

                    FlatSharpInternal.Assert(
                        member is not null,
                        $"Unable to find match for struct offset. Field = {field.Name}");

                    this.fields.Add(member);
                }
            }

            writer.AppendLine($"[FlatBufferStruct]");
            writer.AppendLine($"[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]");
            writer.AppendLine($"public partial struct {this.Name}");
            using (writer.WithBlock())
            {
                for (int i = 0; i < this.Fields.Count; ++i)
                {
                    int offset = this.fields[i].Offset;
                    string typeName = this.fields[i].ItemTypeModel.GetCompilableTypeName();

                    writer.AppendLine($"[System.Runtime.InteropServices.FieldOffset({offset})] public global::{typeName} {this.fields[i].PropertyInfo.Name};");
                }
            }

        }
    }
}
