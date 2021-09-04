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
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;

    internal class PropertyWriter
    {
        private readonly FieldDefinition field;
        private readonly TableOrStructDefinition parent;

        public PropertyWriter(TableOrStructDefinition parent, FieldDefinition field)
        {
            this.field = field;
            this.parent = parent;
        }

        public string? GetClrFieldType(CompileContext? context)
        {
            if (this.field.ResolvedTypeName is not null)
            {
                return this.field.ResolvedTypeName;
            }

            if (this.parent.TryResolveTypeName(this.field.FbsFieldType, context, out _, out string? type))
            {
                return type;
            }

            return null;
        }

        public void WriteDefaultConstructorLine(CodeWriter writer, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.field.Name, (Action)(() =>
            {
                if (context.CompilePass <= CodeWritingPass.PropertyModeling ||
                    !this.TryGetTypeModel(context, out var model))
                {
                    return;
                }

                string? assignment = null;
                if (this.TryGetDefaultValueLiteral(model, out var defaultValueLiteral))
                {
                    assignment = defaultValueLiteral;
                }
                else if (model.ClassifyContextually(this.parent.SchemaType).IsRequiredReference())
                {
                    var cSharpTypeName = CSharpHelpers.GetCompilableTypeName((Type)model.ClrType);
                    assignment = $"new {cSharpTypeName}()";
                }
                else if (model.ClassifyContextually(this.parent.SchemaType).IsOptional())
                {
                    assignment = $"null!";
                }

                if (!string.IsNullOrEmpty(assignment))
                {
                    writer.AppendLine($"this.{this.field.Name} = {assignment};");
                }
            }));
        }

        public void WriteCopyConstructorLine(CodeWriter writer, string sourceName, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.field.Name, () =>
            {
                if (context.CompilePass <= CodeWritingPass.PropertyModeling)
                {
                    return;
                }

                writer.AppendLine($"this.{this.field.Name} = {context.FullyQualifiedCloneMethodName}({sourceName}.{this.field.Name});");
            });
        }

        public void WriteField(CodeWriter writer, TableOrStructDefinition parent, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.field.Name, () =>
            {
                if (context.CompilePass == CodeWritingPass.Initialization)
                {
                    this.EmitBasicDefinition(writer, context);
                    return;
                }

                if (!this.TryGetTypeModel(context, out var model))
                {
                    return;
                }

                string clrType = model.GetGlobalCompilableTypeName();

                //clrType = this.GetClrVectorTypeName(context, this.field.VectorType, clrType);

                writer.AppendLine(this.FormatAttribute(model));
                writer.AppendLine(this.FormatPropertyDeclaration(model, clrType));
                writer.AppendLine();
            });
        }

        private bool TryGetTypeModel(
            CompileContext context,
            [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type? propertyType = context.PreviousAssembly?.GetType(this.parent.FullName)?.GetProperty(this.field.Name, flags)?.PropertyType;

            FlatSharpInternal.Assert(propertyType is not null, $"Unable to find property '{this.field.Name}' from parent '{this.parent.Name}'.");
            
            if (!context.TypeModelContainer.TryCreateTypeModel(propertyType, out typeModel))
            {
                ErrorContext.Current.RegisterError($"Type model container failed to create type model for type '{propertyType}'.");
                return false;
            }

            return true;
        }

        private bool TryGetDefaultValueLiteral(ITypeModel? typeModel, [NotNullWhen(true)] out string? literal)
        {
            if (typeModel is not null && !string.IsNullOrEmpty(this.field.DefaultValue))
            {
                if (typeModel.TryFormatStringAsLiteral(this.field.DefaultValue, out literal))
                {
                    return true;
                }
                else
                {
                    ErrorContext.Current.RegisterError($"Unable to format default value '{this.field.DefaultValue}' as a literal.");
                }
            }

            literal = null;
            return false;
        }

        private void EmitBasicDefinition(CodeWriter writer, CompileContext context)
        {
            string clrType = this.GetClrVectorTypeName(
                context,
                this.field.VectorType,
                this.GetBasicClrTypeName(context));

            writer.AppendLine(this.FormatAttribute(null));
            writer.AppendLine(this.FormatPropertyDeclaration(null, clrType));
            writer.AppendLine();
        }

        private string FormatPropertyDeclaration(ITypeModel? thisTypeModel, string clrTypeName)
        {
            string @virtual = "virtual";

            if (this.field.NonVirtual ?? this.parent.NonVirtual == true)
            {
                @virtual = string.Empty;
            }

            AccessModifier? setterModifier = this.field.SetterKind switch
            {
                SetterKind.Public or SetterKind.PublicInit => AccessModifier.Public,
                SetterKind.Protected or SetterKind.ProtectedInit => AccessModifier.Protected,
                SetterKind.ProtectedInternal or SetterKind.ProtectedInternalInit => AccessModifier.ProtectedInternal,
                SetterKind.Private => AccessModifier.Private,
                SetterKind.None => null,
                _ => throw new InvalidOperationException($"Unexpected Setter Access Modifier '{this.field.SetterKind}'")
            };

            var modifiers = CSharpHelpers.GetPropertyAccessModifiers(this.field.GetterModifier, setterModifier);

            string setter = string.Empty;
            if (this.field.SetterKind != SetterKind.None)
            {
                if (this.field.SetterKind is SetterKind.PublicInit or SetterKind.ProtectedInit or SetterKind.ProtectedInternalInit)
                {
                    setter = $"{modifiers.setModifier.ToCSharpString()} init;";
                }
                else
                {
                    setter = $"{modifiers.setModifier.ToCSharpString()} set;";
                }
            }

            if (thisTypeModel?.ClassifyContextually(this.parent.SchemaType).IsOptionalReference() == true && !this.field.IsRequired)
            {
                clrTypeName += "?";
            }

            return $"{modifiers.propertyModifier.ToCSharpString()} {@virtual} {clrTypeName} {this.field.Name} {{ {modifiers.getModifer.ToCSharpString()} get; {setter} }}";
        }

        private string FormatAttribute(ITypeModel? thisTypeModel)
        {
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string defaultValue = string.Empty;
            string isDeprecated = string.Empty;
            string forceWrite = string.Empty;
            string customAccessor = string.Empty;
            string writeThrough = string.Empty;
            string required = string.Empty;

            if (this.field.IsKey)
            {
                isKey = $", {nameof(FlatBufferItemAttribute.Key)} = true";
            }

            if (this.field.SortedVector)
            {
                sortedVector = $", {nameof(FlatBufferItemAttribute.SortedVector)} = true";
            }

            if (this.field.Deprecated)
            {
                isDeprecated = $", {nameof(FlatBufferItemAttribute.Deprecated)} = true";
            }

            if (!string.IsNullOrEmpty(this.field.CustomGetter))
            {
                customAccessor = $"[{nameof(FlatBufferMetadataAttribute)}({nameof(FlatBufferMetadataKind)}.{nameof(FlatBufferMetadataKind.Accessor)}, \"{this.field.CustomGetter}\")]";
            }

            if (this.TryGetDefaultValueLiteral(thisTypeModel, out var literal))
            {
                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = {literal}";
            }

            if (this.field.IsRequired)
            {
                required = $", {nameof(FlatBufferItemAttribute.Required)} = true";
            }

            if (thisTypeModel is not null)
            {
                bool? fw = this.field.ForceWrite;

                if (fw is null)
                {
                    // Only apply force-write where it is legal when setting from the parent context.
                    fw = this.parent.ForceWrite == true &&
                         thisTypeModel.ClassifyContextually(this.parent.SchemaType).IsRequiredValue();
                }

                if (fw == true)
                {
                    forceWrite = $", {nameof(FlatBufferItemAttribute.ForceWrite)} = true";
                }
            }

            if (this.field.WriteThrough ?? this.parent.WriteThrough == true)
            {
                writeThrough = $", {nameof(FlatBufferItemAttribute.WriteThrough)} = true";
            }

            return $"[{nameof(FlatBufferItemAttribute)}({this.field.Index}{defaultValue}{isDeprecated}{sortedVector}{isKey}{forceWrite}{writeThrough}{required})]{customAccessor}";
        }

        /// <summary>
        /// Returns the basic CLR type. Non-vectorized.
        /// </summary>
        private string GetBasicClrTypeName(CompileContext context)
        {
            if (this.field.SharedString)
            {
                return typeof(SharedString).FullName ?? throw new InvalidOperationException("Full name was null");
            }

            string? clrType = this.GetClrFieldType(context);
            if (clrType is null)
            {
                ErrorContext.Current.RegisterError($"Unable to resolve FBS type: {this.field.FbsFieldType}.");
            }

            if (this.field.IsOptionalScalar)
            {
                clrType += "?";
            }

            return clrType ?? string.Empty;
        }

        private string GetClrVectorTypeName(CompileContext context, VectorType vectorType, string clrType)
        {
            switch (vectorType)
            {
                case VectorType.Array:
                    return $"{clrType}[]";

                case VectorType.IList:
                    return $"IList<{clrType}>";

                case VectorType.IReadOnlyList:
                    return $"IReadOnlyList<{clrType}>";

                case VectorType.Memory:
                    return $"Memory<{clrType}>?";

                case VectorType.ReadOnlyMemory:
                    return $"ReadOnlyMemory<{clrType}>?";

                case VectorType.IIndexedVector:
                    string keyType = this.GetIndexedVectorKeyType(context);
                    return $"IIndexedVector<{keyType}, {clrType}>";

                case VectorType.None:
                    return clrType;

                default:
                    throw new InvalidOperationException($"Unexpected value for '{MetadataKeys.VectorKind}': '{vectorType}'");
            }
        }

        private string GetIndexedVectorKeyType(CompileContext context)
        {
            const string Default = "string";

            if (!this.parent.TryResolveName(this.field.FbsFieldType, out var resolvedNode) ||
                resolvedNode is not TableOrStructDefinition def)
            {
                ErrorContext.Current.RegisterError($"Unable to resolve type {this.field.FbsFieldType}.");
                return Default;
            }

            var keyFields = def.Fields.Where(x => x.IsKey);
            int count = keyFields.Count();

            if (count == 0)
            {
                ErrorContext.Current.RegisterError($"Table '{this.field.FbsFieldType}' has no property with the 'key' metadata.");
                return Default;
            }
            else if (count > 1)
            {
                ErrorContext.Current.RegisterError($"Table '{this.field.FbsFieldType}' declares more than one property with the 'key' metadata.");
                return Default;
            }

            FieldDefinition keyField = keyFields.Single();

            if (!context.TypeModelContainer.TryResolveFbsAlias(
                    keyField.FbsFieldType,
                    out ITypeModel? model))
            {
                ErrorContext.Current.RegisterError($"Key type for table '{this.field.FbsFieldType}' was not a known built-in type. Keys may be scalars or strings.");
                return Default;
            }

            if (model.SchemaType == FlatBufferSchemaType.String && keyField.SharedString)
            {
                return typeof(SharedString).GetCompilableTypeName();
            }

            return model.GetCompilableTypeName();
        }
    }
}
