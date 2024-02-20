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

using FlatSharp.Compiler.Schema;
using System.Linq;

namespace FlatSharp.Compiler.SchemaModel;

public class ReferenceStructSchemaModel : BaseReferenceTypeSchemaModel
{
    private ReferenceStructSchemaModel(Schema.Schema schema, FlatBufferObject @struct) : base(schema, @struct)
    {
        FlatSharpInternal.Assert(@struct.IsStruct, "Expecting struct");

        this.AttributeValidator.DefaultConstructorValidator = kind => kind switch
        {
            DefaultConstructorKind.Public or DefaultConstructorKind.PublicObsolete => AttributeValidationResult.Valid,
            _ => AttributeValidationResult.ValueInvalid,
        };

        this.AttributeValidator.WriteThroughValidator = _ => AttributeValidationResult.Valid;
    }

    public static bool TryCreate(Schema.Schema schema, FlatBufferObject @struct, [NotNullWhen(true)] out ReferenceStructSchemaModel? model)
    {
        model = null;
        if (!@struct.IsStruct)
        {
            return false;
        }

        if (@struct.Attributes?.Any(x => x.Key == MetadataKeys.ValueStruct) == true)
        {
            return false;
        }

        model = new ReferenceStructSchemaModel(schema, @struct);
        return true;
    }

    public override bool OptionalFieldsSupported => false;

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.ReferenceStruct;

    protected override void OnValidate()
    {
        // TODO   
    }

    protected override void EmitDefaultConstructorFieldInitialization(PropertyFieldModel model, CodeWriter writer, CompileContext context)
    {
        if (model.Field.Type.BaseType == BaseType.Obj)
        {
            // another struct. We should new() this up.
            writer.AppendLine($"this.{model.Field.Name} = new {model.GetTypeName()}();");
        }
    }

    protected override void EmitClassDefinition(CodeWriter writer, CompileContext context)
    {
        string attribute = $"[FlatBufferStruct]";

        writer.AppendSummaryComment(this.Documentation);
        writer.AppendLine(attribute);
        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        writer.AppendLine($"[System.Diagnostics.DebuggerTypeProxy(\"{this.FullName}\")]");
        writer.AppendLine($"public partial class {this.Name}");
        writer.AppendLine($"    : object");

        if (context.Options.GeneratePoolableObjects)
        {
            writer.AppendLine($"    , IPoolableObject");
        }
    }
}
