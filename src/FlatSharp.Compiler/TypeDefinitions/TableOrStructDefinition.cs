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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    internal class TableOrStructDefinition : BaseTableOrStructDefinition
    {
        internal const string SerializerPropertyName = "Serializer";

        public TableOrStructDefinition(
            string name,
            bool isTable,
            BaseSchemaMember parent) : base(name, parent)
        {
            this.IsTable = isTable;
        }

        public bool IsTable { get; }

        public override FlatBufferSchemaType SchemaType =>
            this.IsTable ? FlatBufferSchemaType.Table : FlatBufferSchemaType.Struct;

        public bool? NonVirtual { get; set; }

        public bool? ForceWrite { get; set; }

        public bool? WriteThrough { get; set; }

        public DefaultConstructorKind? DefaultConstructorKind { get; set; }

        public string? FileIdentifier;

        public FlatBufferDeserializationOption? RequestedSerializer { get; set; }

        public List<FieldDefinition> Fields { get; } = new();

        public List<StructVectorDefinition> StructVectors { get; } = new();

        protected override bool SupportsChildren => false;

        public override void AddField(FieldDefinition field)
        {
            if (field.IsUnsafeStructVector)
            {
                ErrorContext.Current.RegisterError($"Field '{field.Name}' declares the '{MetadataKeys.UnsafeValueStructVector}' attribute. Unsafe struct vectors are only supported on value structs.");
            }

            this.Fields.Add(field);
        }

        public override void AddStructVector(FieldDefinition definition, int count)
        {
            List<string> propNames = new();
            for (int i = 0; i < count; ++i)
            {
                string propName = $"__flatsharp__{definition.Name}_{i}";

                this.AddField(definition with
                {
                    Name = propName,
                    SetterKind = SetterKind.Protected,
                    GetterModifier = AccessModifier.Protected,
                    CustomGetter = $"{definition.Name}[{i}]"
                });

                propNames.Add(propName);
            }

            this.StructVectors.Add(new StructVectorDefinition(definition.Name, definition.FbsFieldType, definition.SetterKind, propNames));
        }

        public override void ApplyMetadata(Dictionary<string, string?> metadata)
        {
            this.NonVirtual = metadata.ParseNullableBooleanMetadata(MetadataKeys.NonVirtualProperty, MetadataKeys.NonVirtualPropertyLegacy);
            this.ForceWrite = metadata.ParseNullableBooleanMetadata(MetadataKeys.ForceWrite);
            this.WriteThrough = metadata.ParseNullableBooleanMetadata(MetadataKeys.WriteThrough);

            this.DefaultConstructorKind = metadata.ParseMetadata<DefaultConstructorKind?>(
                new[] { MetadataKeys.DefaultConstructorKind },
                ParseDefaultConstructorKind,
                Compiler.DefaultConstructorKind.Public,
                Compiler.DefaultConstructorKind.Public);

            this.RequestedSerializer = metadata.ParseMetadata<FlatBufferDeserializationOption?>(
                new[] { MetadataKeys.SerializerKind, MetadataKeys.PrecompiledSerializerLegacy },
                ParseSerializerKind,
                FlatBufferDeserializationOption.Default,
                null);

            if (!this.IsTable && this.RequestedSerializer is not null)
            {
                ErrorContext.Current.RegisterError("Structs may not have serializers.");
            }

            if (!this.IsTable && this.ForceWrite is not null)
            {
                ErrorContext.Current.RegisterError($"Structs may not use the '{MetadataKeys.ForceWrite}' attribute.");
            }

            if (metadata.ContainsKey(MetadataKeys.ObsoleteDefaultConstructorLegacy))
            {
                ErrorContext.Current.RegisterError($"The '{MetadataKeys.ObsoleteDefaultConstructorLegacy}' metadata attribute has been deprecated. Please use the '{MetadataKeys.DefaultConstructorKind}' attribute instead.");
            }

            if (metadata.TryGetValue(MetadataKeys.FileIdentifier, out var fileId))
            {
                if (!this.IsTable)
                {
                    ErrorContext.Current.RegisterError("Structs may not have file identifiers.");
                }

                this.FileIdentifier = fileId;
            }
        }

        private static bool ParseSerializerKind(string value, out FlatBufferDeserializationOption? result)
        {
            var success = Enum.TryParse<FlatBufferDeserializationOption>(value, true, out var tempResult);
            result = tempResult;
            return success;
        }

        private static bool ParseDefaultConstructorKind(string value, out DefaultConstructorKind? result)
        {
            var success = Enum.TryParse<DefaultConstructorKind>(value, true, out var tempResult);
            result = tempResult;
            return success;
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            this.AssignIndexes();

            var attributeParts = new List<string>();
            string attributeName = this.IsTable ? nameof(FlatBufferTableAttribute) : nameof(FlatBufferStructAttribute);

            if (this.IsTable && !string.IsNullOrEmpty(this.FileIdentifier))
            {
                context.TypeModelContainer.OffsetModel.ValidateFileIdentifier(ref this.FileIdentifier);
                attributeParts.Add($"{nameof(FlatBufferTableAttribute.FileIdentifier)} = \"{this.FileIdentifier}\"");
            }

            bool hasSerializer = context.CompilePass >= CodeWritingPass.SerializerGeneration && this.RequestedSerializer is not null;

            writer.AppendLine($"[{attributeName}({string.Join(", ", attributeParts)})]");
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
                            new PropertyWriter(this, field).WriteDefaultConstructorLine(writer, context);
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
                }
                writer.AppendLine("#pragma warning restore CS8618"); // NULL FORGIVING

                // Copy constructor.
                writer.AppendLine($"public {this.Name}({this.Name} source)");
                using (writer.WithBlock())
                {
                    foreach (var field in this.Fields)
                    {
                        new PropertyWriter(this, field).WriteCopyConstructorLine(writer, "source", context);
                    }

                    this.EmitStructVectorInitializations(writer);
                    writer.AppendLine("this.OnInitialized(null);");
                }

                writer.AppendLine($"partial void OnInitialized({nameof(FlatBufferDeserializationContext)}? context);");
                writer.AppendLine();

                writer.AppendLine($"protected void {TableTypeModel.OnDeserializedMethodName}({nameof(FlatBufferDeserializationContext)}? context) => this.OnInitialized(context);");
                writer.AppendLine();

                foreach (var field in this.Fields)
                {
                    new PropertyWriter(this, field).WriteField(writer, this, context);
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

        public bool TryResolveTypeName(
            string fbsFieldType, 
            CompileContext? context, 
            out ITypeModel? resolvedTypeModel, 
            [NotNullWhen(true)] out string? typeName)
        {
            resolvedTypeModel = null;

            if (context is not null && context.TypeModelContainer.TryResolveFbsAlias(fbsFieldType, out resolvedTypeModel))
            {
                typeName = resolvedTypeModel.ClrType.FullName ?? throw new InvalidOperationException("Full name was null");
                return true;
            }
            else if (this.TryResolveName(fbsFieldType, out var node))
            {
                typeName = node.GlobalName;
                return true;
            }

            typeName = null;
            return false;
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
