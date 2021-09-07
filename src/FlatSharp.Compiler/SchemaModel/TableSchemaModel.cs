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

    public class TableSchemaModel : BaseSchemaModel
    {
        private readonly Dictionary<int, PropertyFieldModel> properties;
        private readonly FlatBufferObject table;

        private TableSchemaModel(Schema schema, FlatBufferObject table) : base(schema, new FlatSharpAttributes(table.Attributes))
        {
            FlatSharpInternal.Assert(table.IsStruct == false, "Not expecting struct");

            this.FullName = table.Name;
            this.DeclaringFile = table.DeclarationFile;
            this.properties = new Dictionary<int, PropertyFieldModel>();
            this.table = table;
            
            foreach (var field in table.Fields)
            {
                FlatSharpInternal.Assert(PropertyFieldModel.TryCreate(this, field.Value, out PropertyFieldModel? model), "Failed to create property model");
                this.properties[field.Value.Id] = model;
            }
        }

        public static bool TryCreate(Schema schema, FlatBufferObject table, [NotNullWhen(true)] out TableSchemaModel? model)
        {
            model = null;
            if (table.IsStruct)
            {
                return false;
            }

            model = new TableSchemaModel(schema, table);
            return true;
        }

        public override string FullName { get; }

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Table;

        public override string DeclaringFile { get; }

        protected override void OnValidate()
        {
            // TODO   
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            string fileId = string.Empty;
            if (this.Schema.RootTable?.Name == this.Name && !string.IsNullOrEmpty(this.Schema.FileIdentifier))
            {
                fileId = $"{nameof(FlatBufferTableAttribute.FileIdentifier)} = \"{fileId}\"";
            }

            string attribute = $"[FlatBufferTable({fileId})]";

            writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name}");
            using (writer.IncreaseIndent())
            {
                writer.AppendLine(": object");
            }

            using (writer.WithBlock())
            {
                this.EmitDefaultConstrutor(writer);
                this.EmitDeserializationConstructor(writer);
                this.EmitCopyConstructor(writer, context);

                writer.AppendLine("partial void OnInitialized(FlatBufferDeserializationContext? context);");

                writer.AppendLine($"protected void {TableTypeModel.OnDeserializedMethodName}({nameof(FlatBufferDeserializationContext)}? context) => this.OnInitialized(context);");
                writer.AppendLine();

                foreach (var property in this.properties)
                {
                    int index = property.Key;
                    PropertyFieldModel model = property.Value;
                    model.WriteCode(writer, index);
                }
            }
        }

        private void EmitCopyConstructor(CodeWriter writer, CompileContext context)
        {
            writer.AppendLine($"public {this.Name}({this.Name} source)");
            using (writer.WithBlock())
            {
                if (context.CompilePass <= CodeWritingPass.PropertyModeling)
                {
                    return;
                }

                foreach (var property in this.properties)
                {
                    string name = property.Value.Field.Name;
                    writer.AppendLine($"this.{name} = {context.FullyQualifiedCloneMethodName}(source.{name});");
                }
            }
        }

        private void EmitDefaultConstrutor(CodeWriter writer)
        {
            if (this.Attributes.DefaultCtorKind != DefaultConstructorKind.None)
            {
                if (this.Attributes.DefaultCtorKind == DefaultConstructorKind.PublicObsolete)
                {
                    writer.AppendLine("[Obsolete]");
                }

                writer.AppendLine("#pragma warning disable CS8618"); // nullable
                writer.AppendLine($"public {this.Name}()");
                using (writer.WithBlock())
                {
                    writer.AppendLine("this.OnInitialized(null);");
                }
                writer.AppendLine("#pragma warning restore CS8618"); // nullable
            }
        }

        private void EmitDeserializationConstructor(CodeWriter writer)
        {
            writer.AppendLine("#pragma warning disable CS8618"); // nullable
            writer.AppendLine($"protected {this.Name}(FlatBufferDeserializationContext context)");
            using (writer.WithBlock())
            {
                // Intentionally left empty.
            }

            writer.AppendLine("#pragma warning restore CS8618"); // nullable
        }

        public override bool SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => true;

        public override bool SupportsDeserializationOption(FlatBufferDeserializationOption option) => true;

        public override bool SupportsForceWrite(bool forceWriteOption) => true;

        public override bool SupportsNonVirtual(bool nonVirtualValue) => true;
    }
}
