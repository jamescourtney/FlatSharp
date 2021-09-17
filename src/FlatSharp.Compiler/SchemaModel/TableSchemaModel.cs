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
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    public class TableSchemaModel : BaseReferenceTypeSchemaModel
    {
        private TableSchemaModel(Schema schema, FlatBufferObject table) : base(schema, table)
        {
            FlatSharpInternal.Assert(table.IsStruct == false, "Not expecting struct");

            this.AttributeValidator.DeserializationOptionValidator = _ => AttributeValidationResult.Valid;
            this.AttributeValidator.DefaultConstructorValidator = _ => AttributeValidationResult.Valid;
            this.AttributeValidator.ForceWriteValidator = _ => AttributeValidationResult.Valid;
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

        public override bool OptionalFieldsSupported => true;

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Table;

        protected override void OnValidate()
        {
            // TODO   
        }

        protected override void EmitExtraData(CodeWriter writer, CompileContext context)
        {
            if (this.Attributes.DeserializationOption is not null && context.CompilePass >= CodeWritingPass.SerializerAndRpcGeneration)
            {
                // generate the serializer.
                string serializer = this.GenerateSerializerForType(
                    context,
                    this.Attributes.DeserializationOption.Value);

                writer.AppendLine($"public static ISerializer<{this.FullName}> Serializer {{ get; }} = new {RoslynSerializerGenerator.GeneratedSerializerClassName}().AsISerializer();");

                writer.AppendLine();

                writer.AppendLine($"ISerializer {nameof(IFlatBufferSerializable)}.{nameof(IFlatBufferSerializable.Serializer)} => Serializer;");
                writer.AppendLine($"ISerializer<{this.FullName}> {nameof(IFlatBufferSerializable)}<{this.FullName}>.{nameof(IFlatBufferSerializable.Serializer)} => Serializer;");

                writer.AppendLine();
                writer.AppendLine($"#region Serializer for {this.FullName}");
                writer.AppendLine(serializer);
                writer.AppendLine($"#endregion");
            }
        }

        protected override void EmitClassDefinition(CodeWriter writer, CompileContext context)
        {
            string fileId = string.Empty;
            if (this.Schema.RootTable?.Name == this.FullName && !string.IsNullOrEmpty(this.Schema.FileIdentifier))
            {
                fileId = $"{nameof(FlatBufferTableAttribute.FileIdentifier)} = \"{this.Schema.FileIdentifier}\"";
            }

            string attribute = $"[FlatBufferTable({fileId})]";

            writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name}");

            using (writer.IncreaseIndent())
            {
                writer.AppendLine(": object");
                if (this.Attributes.DeserializationOption is not null && context.CompilePass >= CodeWritingPass.SerializerAndRpcGeneration)
                {
                    writer.AppendLine($", {nameof(IFlatBufferSerializable)}<{this.FullName}>");
                }
            }
        }

        protected override void EmitDefaultConstructorFieldInitialization(PropertyFieldModel model, CodeWriter writer, CompileContext context)
        {
            writer.AppendLine($"this.{model.Field.Name} = {model.GetDefaultValue()};");
        }

        private string GenerateSerializerForType(
            CompileContext context,
            FlatBufferDeserializationOption deserializationOption)
        {
            Type? type = context.PreviousAssembly?.GetType(this.FullName);
            FlatSharpInternal.Assert(type is not null, $"Flatsharp failed to find expected type '{this.FullName}' in assembly.");

            var options = new FlatBufferSerializerOptions(deserializationOption) { ConvertProtectedInternalToProtected = false };
            var generator = new RoslynSerializerGenerator(options, context.TypeModelContainer);

            MethodInfo method = generator.GetType()
                                         .GetMethod(nameof(RoslynSerializerGenerator.GenerateCSharp), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                                         .MakeGenericMethod(type);

            try
            {
                string code = (string)method.Invoke(generator, new[] { "private" })!;
                return code;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                throw;
            }
        }
    }
}
