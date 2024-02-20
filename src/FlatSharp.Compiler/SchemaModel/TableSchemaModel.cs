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

using FlatSharp.Attributes;
using FlatSharp.CodeGen;
using FlatSharp.Compiler.Schema;
using FlatSharp.TypeModel;
using System.Linq;

namespace FlatSharp.Compiler.SchemaModel;

public class TableSchemaModel : BaseReferenceTypeSchemaModel
{
    private TableSchemaModel(Schema.Schema schema, FlatBufferObject table) : base(schema, table)
    {
        FlatSharpInternal.Assert(table.IsStruct == false, "Not expecting struct");

        this.AttributeValidator.DeserializationOptionValidator = _ => AttributeValidationResult.Valid;
        this.AttributeValidator.DefaultConstructorValidator = _ => AttributeValidationResult.Valid;
        this.AttributeValidator.ForceWriteValidator = _ => AttributeValidationResult.Valid;
    }

    public static bool TryCreate(Schema.Schema schema, FlatBufferObject table, [NotNullWhen(true)] out TableSchemaModel? model)
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
            ITypeModel model = context.TypeModelContainer.CreateTypeModel(context.PreviousAssembly!.GetType(this.FullName)!);
            (string ns, string name) = DefaultMethodNameResolver.ResolveGeneratedSerializerClassName(model);

            string optionTypeName = typeof(FlatBufferDeserializationOption).GetGlobalCompilableTypeName();

            writer.AppendLine($"public static ISerializer<{this.FullName}> Serializer {{ get; }} = new global::{ns}.{name}().AsISerializer({optionTypeName}.{this.Attributes.DeserializationOption.Value});");

            writer.AppendLine();

            writer.AppendLine($"ISerializer {nameof(IFlatBufferSerializable)}.{nameof(IFlatBufferSerializable.Serializer)} => (ISerializer)(({nameof(IFlatBufferSerializable)}<{this.FullName}>)this).Serializer;");
            writer.AppendLine($"ISerializer<{this.FullName}> {nameof(IFlatBufferSerializable)}<{this.FullName}>.{nameof(IFlatBufferSerializable.Serializer)} => Serializer;");

            writer.AppendLine();

            foreach (var option in new[] { FlatBufferDeserializationOption.Lazy, FlatBufferDeserializationOption.Greedy, FlatBufferDeserializationOption.GreedyMutable, FlatBufferDeserializationOption.Progressive })
            {
                string staticAbstractMethod = $"static ISerializer<{this.FullName}> {nameof(IFlatBufferSerializable)}<{this.FullName}>.{option}Serializer {{ get; }} = new global::{ns}.{name}().AsISerializer({optionTypeName}.{option});";
                writer.BeginPreprocessorIf(CSharpHelpers.Net7PreprocessorVariable, staticAbstractMethod).Flush();
            }
        }
    }

    protected override void EmitClassDefinition(CodeWriter writer, CompileContext context)
    {
        string fileId = string.Empty;
        if (this.Schema.RootTable?.Name == this.FullName && !string.IsNullOrEmpty(this.Schema.FileIdentifier))
        {
            fileId = $", {nameof(FlatBufferTableAttribute.FileIdentifier)} = \"{this.Schema.FileIdentifier}\"";
        }

        string emitSerializer = $"{nameof(FlatBufferTableAttribute.BuildSerializer)} = {(this.Attributes.DeserializationOption is not null ? "true" : "false")}";

        string attribute = $"[FlatBufferTable({emitSerializer}{fileId})]";

        writer.AppendSummaryComment(this.Documentation);
        writer.AppendLine(attribute);
        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        writer.AppendLine($"[System.Diagnostics.DebuggerTypeProxy(\"{this.FullName}\")]");
        writer.AppendLine($"public partial class {this.Name}");

        using (writer.IncreaseIndent())
        {
            writer.AppendLine(": object");

            if (context.Options.GeneratePoolableObjects)
            {
                writer.AppendLine(", IPoolableObject");
            }

            if (this.Attributes.DeserializationOption is not null && context.CompilePass >= CodeWritingPass.SerializerAndRpcGeneration)
            {
                writer.AppendLine($", {nameof(IFlatBufferSerializable)}<{this.FullName}>");
                writer.AppendLine($", {nameof(IFlatBufferSerializable)}");
            }

            PropertyFieldModel? keyField = this.properties.Values.SingleOrDefault(x => x.Field.Key);
            if (keyField is not null)
            {
                writer.AppendLine($", {nameof(ISortableTable<bool>)}<{keyField.GetTypeName()}>");
            }
        }
    }

    protected override void EmitDefaultConstructorFieldInitialization(PropertyFieldModel model, CodeWriter writer, CompileContext context)
    {
        string line = $"this.{model.Field.Name} = {model.GetDefaultValue()};";

        if (model.Field.Required)
        {
            writer.BeginPreprocessorIf(CSharpHelpers.Net7PreprocessorVariable, string.Empty)
                  .Else(line)
                  .Flush();
        }
        else
        {
            writer.AppendLine(line);
        }
    }
}
