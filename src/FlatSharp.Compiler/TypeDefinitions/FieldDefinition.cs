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
    using System.Reflection;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;

    internal record FieldDefinition
    {
        public FieldDefinition(
            TableOrStructDefinition parentDefinition,
            string name,
            string fieldType)
        {
            this.Parent = parentDefinition;
            this.Name = name;
            this.FbsFieldType = fieldType;
        }

        public int Index { get; set; }

        public bool IsIndexSetManually { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public bool SortedVector { get; set; }

        public string? CustomGetter { get; set; }

        public bool IsKey { get; set; }

        public bool? NonVirtual { get; set; }

        public bool? ForceWrite { get; set; }

        public bool? WriteThrough { get; set; }

        public bool IsRequired { get; set; }

        public string? DefaultValue { get; set; }

        public bool IsOptionalScalar { get; set; }

        public VectorType VectorType { get; set; }

        public SetterKind SetterKind { get; set; } = SetterKind.Public;

        public AccessModifier GetterModifier { get; set; } = AccessModifier.Public;

        public bool SharedString { get; set; }

        public TableOrStructDefinition Parent { get; }

        public void WriteDefaultConstructorLine(CodeWriter writer, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.Name, () =>
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
                else if (model.ClassifyContextually(this.Parent.SchemaType).IsRequiredReference())
                {
                    var cSharpTypeName = CSharpHelpers.GetCompilableTypeName(model.ClrType);
                    assignment = $"new {cSharpTypeName}()";
                }

                if (!string.IsNullOrEmpty(assignment))
                {
                    writer.AppendLine($"this.{this.Name} = {assignment};");
                }
            });
        }

        public void WriteCopyConstructorLine(CodeWriter writer, string sourceName, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.Name, () =>
            {
                if (context.CompilePass <= CodeWritingPass.PropertyModeling)
                {
                    return;
                }

                if (!this.Parent.TryResolveTypeModelWithError(this.FbsFieldType, context, out _))
                {
                    return;
                }

                writer.AppendLine($"this.{this.Name} = {context.FullyQualifiedCloneMethodName}({sourceName}.{this.Name});");
            });
        }

        public void WriteField(CodeWriter writer, TableOrStructDefinition parent, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.Name, () =>
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

                string clrType = this.GetBasicClrTypeName(context);
                string vectorKeyType = string.Empty;

                if (model.TryGetUnderlyingVectorType(out ITypeModel? vectorItemModel) &&
                    vectorItemModel.TryGetTableKeyMember(out TableMemberModel? memberModel))
                {
                    vectorKeyType = CSharpHelpers.GetCompilableTypeName(memberModel.ItemTypeModel.ClrType);
                }

                clrType = this.GetClrVectorTypeName(this.VectorType, clrType, vectorKeyType);

                writer.AppendLine(this.FormatAttribute(model));
                writer.AppendLine(this.FormatPropertyDeclaration(model, clrType));
                writer.AppendLine();
            });
        }

        private bool TryGetTypeModel(
            CompileContext context,
            [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            typeModel = null;
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type? propertyType = context.PreviousAssembly?.GetType(this.Parent.FullName)?.GetProperty(this.Name, flags)?.PropertyType;

            if (propertyType is null)
            {
                ErrorContext.Current.RegisterError($"Unable to find property '{this.Name}' from parent '{this.Parent.Name}'.");
                return false;
            }

            if (!context.TypeModelContainer.TryCreateTypeModel(propertyType, out typeModel))
            {
                ErrorContext.Current.RegisterError($"Type model container failed to create type model for type '{propertyType}'.");
                return false;
            }

            return true;
        }

        private bool TryGetDefaultValueLiteral(ITypeModel? typeModel, [NotNullWhen(true)] out string? literal)
        {
            if (typeModel is not null && !string.IsNullOrEmpty(this.DefaultValue))
            {
                if (typeModel.TryFormatStringAsLiteral(this.DefaultValue, out literal))
                {
                    return true;
                }
                else
                {
                    ErrorContext.Current.RegisterError($"Unable to format default value '{this.DefaultValue}' as a literal.");
                }
            }

            literal = null;
            return false;
        }

        private void EmitBasicDefinition(CodeWriter writer, CompileContext context)
        {
            // Indexed Vectors require two generic parameters:
            // TKeyType, TableType
            // We have a chicken-egg problem here, because we need the compiled version
            // to discover the key type, but we need the key type to build the compiled version.
            // In the first pass, where we don't have any type model information, we can break
            // this circular loop by changing indexed vectors to arrays. Following that,
            // we can introspect on the key type and build the proper indexed vector definition
            // in pass #2.
            VectorType type = this.VectorType;
            if (type == VectorType.IIndexedVector)
            {
                type = VectorType.Array;
            }

            string clrType = this.GetClrVectorTypeName(
                type,
                this.GetBasicClrTypeName(context),
                string.Empty);

            writer.AppendLine(this.FormatAttribute(null));
            writer.AppendLine(this.FormatPropertyDeclaration(null, clrType));
            writer.AppendLine();
        }

        private string FormatPropertyDeclaration(ITypeModel? thisTypeModel, string clrTypeName)
        {
            string @virtual = "virtual";

            if (this.NonVirtual ?? this.Parent.NonVirtual == true)
            {
                @virtual = string.Empty;
            }

            AccessModifier? setterModifier = this.SetterKind switch
            {
                SetterKind.Public or SetterKind.PublicInit => AccessModifier.Public,
                SetterKind.Protected or SetterKind.ProtectedInit => AccessModifier.Protected,
                SetterKind.ProtectedInternal or SetterKind.ProtectedInternalInit => AccessModifier.ProtectedInternal,
                SetterKind.None => null,
                _ => throw new InvalidOperationException($"Unexpected Setter Access Modifier '{this.SetterKind}'")
            };

            var modifiers = CSharpHelpers.GetPropertyAccessModifiers(this.GetterModifier, setterModifier);

            string setter = string.Empty;
            if (this.SetterKind != SetterKind.None)
            {
                if (this.SetterKind is SetterKind.PublicInit or SetterKind.ProtectedInit or SetterKind.ProtectedInternalInit)
                {
                    setter = $"{modifiers.setModifier.ToCSharpString()} init;";
                }
                else
                {
                    setter = $"{modifiers.setModifier.ToCSharpString()} set;";
                }
            }

            if (thisTypeModel?.ClassifyContextually(this.Parent.SchemaType).IsOptionalReference() == true && !this.IsRequired)
            {
                clrTypeName += "?";
            }

            return $"{modifiers.propertyModifier.ToCSharpString()} {@virtual} {clrTypeName} {this.Name} {{ {modifiers.getModifer.ToCSharpString()} get; {setter} }}";
        }

        private string FormatAttribute(ITypeModel? thisTypeModel)
        {
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string defaultValue = string.Empty;
            string isDeprecated = string.Empty;
            string forceWrite = string.Empty;
            string customGetter = string.Empty;
            string writeThrough = string.Empty;
            string required = string.Empty;

            if (this.IsKey)
            {
                isKey = $", {nameof(FlatBufferItemAttribute.Key)} = true";
            }

            if (this.SortedVector)
            {
                sortedVector = $", {nameof(FlatBufferItemAttribute.SortedVector)} = true";
            }

            if (this.Deprecated)
            {
                isDeprecated = $", {nameof(FlatBufferItemAttribute.Deprecated)} = true";
            }

            if (!string.IsNullOrEmpty(this.CustomGetter))
            {
                customGetter = $", {nameof(FlatBufferItemAttribute.CustomGetter)} = \"{this.CustomGetter}\"";
            }

            if (this.TryGetDefaultValueLiteral(thisTypeModel, out var literal))
            {
                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = {literal}";
            }

            if (this.IsRequired)
            {
                required = $", {nameof(FlatBufferItemAttribute.Required)} = true";
            }

            if (thisTypeModel is not null)
            {
                bool? fw = this.ForceWrite;

                if (fw is null)
                {
                    // Only apply force-write where it is legal when setting from the parent context.
                    fw = this.Parent.ForceWrite == true &&
                         thisTypeModel.ClassifyContextually(this.Parent.SchemaType).IsRequiredValue();
                }

                if (fw == true)
                {
                    forceWrite = $", {nameof(FlatBufferItemAttribute.ForceWrite)} = true";
                }
            }

            if (this.WriteThrough ?? this.Parent.WriteThrough == true)
            {
                writeThrough = $", {nameof(FlatBufferItemAttribute.WriteThrough)} = true";
            }

            return $"[{nameof(FlatBufferItemAttribute)}({this.Index}{defaultValue}{isDeprecated}{sortedVector}{isKey}{forceWrite}{customGetter}{writeThrough}{required})]";
        }

        /// <summary>
        /// Returns the basic CLR type. Non-vectorized.
        /// </summary>
        private string GetBasicClrTypeName(CompileContext context)
        {
            if (this.SharedString)
            {
                return typeof(SharedString).FullName ?? throw new InvalidOperationException("Full name was null");
            }

            string clrType = this.Parent.ResolveTypeName(this.FbsFieldType, context, out _);

            if (this.IsOptionalScalar)
            {
                clrType += "?";
            }

            return clrType;
        }

        private string GetClrVectorTypeName(VectorType vectorType, string clrType, string sortKeyType)
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
                    if (string.IsNullOrWhiteSpace(sortKeyType))
                    {
                        ErrorContext.Current.RegisterError($"Unable to determine key type for table {clrType}. Please make sure a property has the 'Key' metadata.");
                    }

                    return $"IIndexedVector<{sortKeyType}, {clrType}>";

                case VectorType.None:
                    return clrType;

                default:
                    throw new InvalidOperationException($"Unexpected value for '{MetadataKeys.VectorKind}': '{vectorType}'");
            }
        }
    }
}
