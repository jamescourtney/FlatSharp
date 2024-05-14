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

using System.Linq;
using FlatSharp.CodeGen;
using FlatSharp.Compiler.Schema;
using FlatSharp.TypeModel;

namespace FlatSharp.Compiler.SchemaModel;

/// <summary>
/// A common base class for reference tables and structs.
/// </summary>
public abstract class BaseReferenceTypeSchemaModel : BaseSchemaModel
{
    protected readonly Dictionary<int, PropertyFieldModel> properties;
    protected readonly List<StructVectorPropertyFieldModel> structVectors;
    protected readonly FlatBufferObject flatBufferObject;

    protected BaseReferenceTypeSchemaModel(Schema.Schema schema, FlatBufferObject flatBufferObject) : base(schema, flatBufferObject.Name, new FlatSharpAttributes(flatBufferObject.Attributes))
    {
        this.properties = new Dictionary<int, PropertyFieldModel>();
        this.DeclaringFile = flatBufferObject.DeclarationFile;
        this.structVectors = new();
        this.flatBufferObject = flatBufferObject;

        int previousIndex = -1;
        foreach (var field in flatBufferObject.Fields.OrderBy(x => x.Id))
        {
            if (PropertyFieldModel.TryCreate(this, field, previousIndex, out PropertyFieldModel? model))
            {
                previousIndex = model.Index;
                this.properties[model.Index] = model;
            }
            else if (StructVectorPropertyFieldModel.TryCreate(this, field, previousIndex, out StructVectorPropertyFieldModel? svModel))
            {
                for (int i = 0; i < svModel.Properties.Count; ++i)
                {
                    previousIndex = svModel.Properties[i].Index;
                    this.properties[previousIndex] = svModel.Properties[i];
                }

                this.structVectors.Add(svModel);
            }
        }
    }

    public abstract bool OptionalFieldsSupported { get; }

    public sealed override string DeclaringFile { get; }

    public IEnumerable<string>? Documentation => this.flatBufferObject.Documentation;

    protected sealed override void OnWriteCode(CodeWriter writer, CompileContext context)
    {
        this.EmitClassDefinition(writer, context);

        using (writer.WithBlock())
        {
            this.EmitStaticConstructor(writer, context);
            this.EmitDefaultConstrutor(writer, context);
            this.EmitDeserializationConstructor(writer);
            this.EmitCopyConstructor(writer, context);
            this.EmitPoolableObject(writer, context);

            writer.AppendLine("static partial void OnStaticInitialize();");

            writer.AppendLine("partial void OnInitialized(FlatBufferDeserializationContext? context);");

            writer.AppendLine($"protected void {TableTypeModel.OnDeserializedMethodName}({nameof(FlatBufferDeserializationContext)} context)");
            using (writer.WithBlock())
            {
                writer.AppendLine("this.OnInitialized(context);");
            }

            var orderedProperties = this.properties.OrderBy(x => x.Key);
            foreach (var property in orderedProperties)
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

            // This matches C# records
            string fieldStrings = string.Join(", ", orderedProperties.Select(p => p.Value.FieldName).Select(n => $"{n} = {{this.{n}}}"));
            string fieldStringsWithSpace = this.properties.Count == 0 ? " " : $" {fieldStrings} ";
            writer.AppendLine($"public override string ToString() => $\"{this.Name} {{{{{fieldStringsWithSpace}}}}}\";");

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
        writer.BeginPreprocessorIf(CSharpHelpers.Net7PreprocessorVariable, "[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]")
              .Flush();

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

            if (context.Options.MutationTestingMode)
            {
                writer.AppendLine($"[{typeof(ExcludeFromCodeCoverageAttribute).GetCompilableTypeName()}]");
            }

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

    private void EmitPoolableObject(CodeWriter writer, CompileContext context)
    {
        if (context.Options.GeneratePoolableObjects)
        {
            writer.AppendLine("/// <inheritdoc />");
            writer.AppendLine("public virtual void ReturnToPool(bool unsafeForce = false)");
            using (writer.WithBlock())
            {
            }
        }
    }

    protected virtual void EmitStaticConstructor(CodeWriter writer, CompileContext context)
    {
        writer.AppendLine($"static {this.Name}()");
        using (writer.WithBlock())
        {
            var keyProperty = this.properties.Values.SingleOrDefault(p => p.Field.Key);
            if (keyProperty is not null && context.CompilePass == CodeWritingPass.LastPass)
            {
                writer.AppendLine($"global::FlatSharp.SortedVectorHelpers.RegisterKeyLookup<{this.Name}, {keyProperty.Field.Type.ResolveTypeOrElementTypeName(this.Schema, keyProperty.Attributes)}>(x => x.{keyProperty.FieldName}, {keyProperty.Index});");
            }

            writer.AppendLine("OnStaticInitialize();");
        }
    }
}
