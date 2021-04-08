/*
 * Copyright 2020 James Courtney
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
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    internal class TableOrStructDefinition : BaseSchemaMember
    {
        internal const string SerializerPropertyName = "Serializer";

        public TableOrStructDefinition(
            string name,
            BaseSchemaMember parent) : base(name, parent)
        {
        }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();

        public List<StructVectorDefinition> StructVectors { get; set; } = new List<StructVectorDefinition>();

        public bool IsTable { get; set; }

        public FlatBufferSchemaType SchemaType =>
            this.IsTable ? FlatBufferSchemaType.Table : FlatBufferSchemaType.Struct;

        public bool? NonVirtual { get; set; }

        public bool? ForceWrite { get; set; }

        public DefaultConstructorKind? DefaultConstructorKind { get; set; }

        public string? FileIdentifier { get; set; }

        public FlatBufferDeserializationOption? RequestedSerializer { get; set; }

        protected override bool SupportsChildren => false;

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            this.AssignIndexes();

            string attribute = "[FlatBufferStruct]";

            if (this.IsTable)
            {
                if (string.IsNullOrEmpty(this.FileIdentifier))
                {
                    attribute = "[FlatBufferTable]";
                }
                else
                {
                    attribute = $"[FlatBufferTable({nameof(FlatBufferTableAttribute.FileIdentifier)} = \"{this.FileIdentifier}\")]";
                }
            }

            bool hasSerializer = context.CompilePass >= CodeWritingPass.SerializerGeneration && this.RequestedSerializer is not null;

                writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name}");
            using (writer.IncreaseIndent())
            {
                writer.AppendLine(": object");
                if (hasSerializer)
                {
                    writer.AppendLine($", {nameof(IFlatBufferSerializable)}<{this.Name}>");
                }
            }
                
            writer.AppendLine($"{{");

            using (writer.IncreaseIndent())
            {
                // Default ctor.
                var defaultCtorKind = this.DefaultConstructorKind ?? Compiler.DefaultConstructorKind.Public;
                if (defaultCtorKind != Compiler.DefaultConstructorKind.None)
                {
                    if (defaultCtorKind == Compiler.DefaultConstructorKind.PublicObsolete)
                    {
                        writer.AppendLine("[Obsolete]");
                    }

                    writer.AppendLine($"public {this.Name}()");
                    using (writer.WithBlock())
                    {
                        foreach (var field in this.Fields)
                        {
                            field.WriteDefaultConstructorLine(writer, context);
                        }

                        this.EmitStructVectorInitializations(writer);
                        writer.AppendLine("this.OnInitialized(null);");
                    }
                }
                else if (!this.IsTable)
                {
                    ErrorContext.Current.RegisterError("Structs must have default constructors.");
                }

                writer.AppendLine("#pragma warning disable CS8618"); // NULL FORGIVING
                writer.AppendLine($"protected {this.Name}({nameof(FlatBufferDeserializationContext)} context)");
                using (writer.WithBlock())
                {
                    this.EmitStructVectorInitializations(writer);
                    writer.AppendLine("this.OnInitialized(context);");
                }
                writer.AppendLine("#pragma warning restore CS8618"); // NULL FORGIVING

                // Copy constructor.
                writer.AppendLine($"public {this.Name}({this.Name} source)");
                using (writer.WithBlock())
                {
                    foreach (var field in this.Fields)
                    {
                        field.WriteCopyConstructorLine(writer, "source", context);
                    }

                    this.EmitStructVectorInitializations(writer);
                    writer.AppendLine("this.OnInitialized(null);");
                }

                writer.AppendLine($"partial void OnInitialized({nameof(FlatBufferDeserializationContext)}? context);");

                foreach (var field in this.Fields)
                {
                    field.WriteField(writer, this, context);
                }

                foreach (var structVector in this.StructVectors)
                {
                    structVector.EmitStructVector(this, writer, context);
                }

                if (hasSerializer)
                {
                    // generate the serializer.
                    string serializer = this.GenerateSerializerForType(
                        context,
                        this.RequestedSerializer!.Value); // not null by assertion above.

                    writer.AppendLine($"public static ISerializer<{this.FullName}> {SerializerPropertyName} {{ get; }} = new {RoslynSerializerGenerator.GeneratedSerializerClassName}().AsISerializer();");

                    writer.AppendLine();

                    writer.AppendLine($"ISerializer {nameof(IFlatBufferSerializable)}.{nameof(IFlatBufferSerializable.Serializer)} => {SerializerPropertyName};");
                    writer.AppendLine($"ISerializer<{this.FullName}> {nameof(IFlatBufferSerializable)}<{this.FullName}>.{nameof(IFlatBufferSerializable.Serializer)} => {SerializerPropertyName};");

                    writer.AppendLine();
                    writer.AppendLine($"#region Serializer for {this.FullName}");
                    writer.AppendLine(serializer);
                    writer.AppendLine($"#endregion");
                }
            }

            writer.AppendLine($"}}");
        }

        private void EmitStructVectorInitializations(CodeWriter writer)
        {
            foreach (var sv in this.StructVectors)
            {
                sv.EmitStructVectorInitializer(writer);
            }
        }

        public string ResolveTypeName(string fbsFieldType, CompileContext context, out ITypeModel? resolvedTypeModel)
        {
            if (context.TypeModelContainer.TryResolveFbsAlias(fbsFieldType, out resolvedTypeModel))
            {
                fbsFieldType = resolvedTypeModel.ClrType.FullName ?? throw new InvalidOperationException("Full name was null");
            }
            else if (this.TryResolveName(fbsFieldType, out var node))
            {
                fbsFieldType = node.GlobalName;
            }

            return fbsFieldType;
        }

        private string GenerateSerializerForType(
            CompileContext context,
            FlatBufferDeserializationOption deserializationOption)
        {
            try
            {
                CSharpHelpers.ConvertProtectedInternalToProtected = false;

                Type? type = context.PreviousAssembly?.GetType(this.FullName);
                if (type is null)
                {
                    ErrorContext.Current.RegisterError($"Flatsharp failed to find expected type '{this.FullName}' in assembly.");
                    return string.Empty;
                }

                var options = new FlatBufferSerializerOptions(deserializationOption);
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
            finally
            {
                CSharpHelpers.ConvertProtectedInternalToProtected = true;
            }
        }

        private void AssignIndexes()
        {
            ErrorContext.Current.WithScope(
                this.Name,
                () =>
                {
                    if (this.Fields.Any(field => field.IsIndexSetManually))
                    {
                        if (!this.IsTable)
                        {
                            ErrorContext.Current.RegisterError("Structure fields may not have 'id' attribute set.");
                        }

                        if (!this.Fields.TrueForAll(field => field.IsIndexSetManually))
                        {
                            ErrorContext.Current.RegisterError("All or none fields should have 'id' attribute set.");
                        }

                        return;
                    }

                    int nextIndex = 0;
                    foreach (var field in this.Fields)
                    {
                        field.Index = nextIndex;

                        nextIndex++;

                        if (this.TryResolveName(field.FbsFieldType, out var typeDef) && typeDef is UnionDefinition)
                        {
                            // Unions are double-wide.
                            nextIndex++;
                        }
                    }
                });
        }
    }
}
