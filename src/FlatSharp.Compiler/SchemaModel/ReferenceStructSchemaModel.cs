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

    public class ReferenceStructSchemaModel : BaseReferenceTypeSchemaModel
    {
        private ReferenceStructSchemaModel(Schema schema, FlatBufferObject @struct) : base(schema, @struct)
        {
            FlatSharpInternal.Assert(@struct.IsStruct, "Expecting struct");

            this.AttributeValidator.DefaultConstructorValidator = kind => kind switch
            {
                DefaultConstructorKind.Public or DefaultConstructorKind.PublicObsolete => AttributeValidationResult.Valid,
                _ => AttributeValidationResult.ValueInvalid,
            };

            this.AttributeValidator.WriteThroughValidator = _ => AttributeValidationResult.Valid;

        }

        public static bool TryCreate(Schema schema, FlatBufferObject @struct, [NotNullWhen(true)] out ReferenceStructSchemaModel? model)
        {
            model = null;
            if (!@struct.IsStruct)
            {
                return false;
            }

            if (@struct.Attributes?.ContainsKey(MetadataKeys.ValueStruct) == true)
            {
                return false;
            }

            model = new ReferenceStructSchemaModel(schema, @struct);
            return true;
        }

        public override bool OptionalFieldsSupported => false;

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Struct;

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

            writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name}");
        }
    }
}
