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
    using FlatSharp.TypeModel;

    /// <summary>
    /// A common base class for reference tables and structs.
    /// </summary>
    public abstract class BaseReferenceTypeSchemaModel : BaseSchemaModel
    {
        private readonly Dictionary<int, PropertyFieldModel> properties;
        private readonly List<StructVectorPropertyFieldModel> structVectors;
        private readonly FlatBufferObject table;

        protected BaseReferenceTypeSchemaModel(Schema schema, FlatBufferObject table) : base(schema, table.Name, new FlatSharpAttributes(table.Attributes))
        {
            this.DeclaringFile = table.DeclarationFile;
            this.properties = new Dictionary<int, PropertyFieldModel>();
            this.structVectors = new();
            this.table = table;

            int previousIndex = -1;
            foreach (var field in table.Fields.OrderBy(x => x.Value.Id))
            {
                if (PropertyFieldModel.TryCreate(this, field.Value, previousIndex, out PropertyFieldModel? model))
                {
                    previousIndex = model.Index;
                    this.properties[model.Index] = model;
                }
                else if (StructVectorPropertyFieldModel.TryCreate(this, field.Value, previousIndex, out StructVectorPropertyFieldModel? svModel))
                {
                    for (int i = 0; i < svModel.Properties.Count; ++i)
                    {
                        previousIndex = svModel.Properties[i].Index;
                        this.properties[previousIndex] = svModel.Properties[i];
                    }

                    this.structVectors.Add(svModel);
                }
            }

            this.AttributeValidator.NonVirtualValidator = (b) => AttributeValidationResult.Valid;
        }

        public abstract bool OptionalFieldsSupported { get; }

        public sealed override string DeclaringFile { get; }

        protected sealed override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            this.EmitClassDefinition(writer, context);

            using (writer.WithBlock())
            {
                this.EmitDefaultConstrutor(writer, context);
                this.EmitDeserializationConstructor(writer);
                this.EmitCopyConstructor(writer, context);

                writer.AppendLine("partial void OnInitialized(FlatBufferDeserializationContext? context);");

                writer.AppendLine($"protected void {TableTypeModel.OnDeserializedMethodName}({nameof(FlatBufferDeserializationContext)}? context) => this.OnInitialized(context);");
                writer.AppendLine();

                foreach (var property in this.properties.OrderBy(x => x.Key))
                {
                    int index = property.Key;
                    PropertyFieldModel model = property.Value;

                    writer.AppendLine();
                    model.WriteCode(writer);
                }

                foreach (var sv in this.structVectors)
                {
                    sv.WriteCode(writer, context);
                }

                this.EmitExtraData(writer, context);
            }
        }

        protected abstract void EmitClassDefinition(CodeWriter writer, CompileContext context);

        protected abstract void EmitDefaultConstructorFieldInitialization(PropertyFieldModel model, CodeWriter writer, CompileContext context);

        protected virtual void EmitExtraData(CodeWriter writer, CompileContext context) 
        { 
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

                writer.AppendLine("this.OnInitialized(null);");
            }
        }

        private void EmitDefaultConstrutor(CodeWriter writer, CompileContext context)
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
                    foreach (var item in this.properties.OrderBy(x => x.Key))
                    {
                        this.EmitDefaultConstructorFieldInitialization(item.Value, writer, context);
                    }

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
    }
}
