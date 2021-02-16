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

using FlatSharp.TypeModel;
using System;
using System.Linq;
using System.Reflection;

namespace FlatSharp.Compiler
{
    internal class FieldDefinition
    {
        public int Index { get; set; }

        public bool IsIndexSetManually { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public bool SortedVector { get; set; }

        public bool IsKey { get; set; }

        public bool? NonVirtual { get; set; }

        public string DefaultValue { get; set; }

        public bool IsOptionalScalar { get; set; }

        public VectorType VectorType { get; set; }

        public SetterKind SetterKind { get; set; } = SetterKind.Public;

        public bool SharedString { get; set; }

        public string GetClrTypeName(BaseSchemaMember baseMember)
        {
            string clrType;
            string sortKeyType = null;

            if (SchemaDefinition.TryResolve(this.FbsFieldType, out ITypeModel builtInType))
            {
                clrType = builtInType.ClrType.FullName;
            }
            else
            {
                if (baseMember.TryResolveName(this.FbsFieldType, out var typeDefinition))
                {
                    clrType = typeDefinition is UnionDefinition unionDef ? unionDef.ClrTypeName : typeDefinition.GlobalName;

                    if (typeDefinition is TableOrStructDefinition tableOrStruct && tableOrStruct.IsTable)
                    {
                        sortKeyType = tableOrStruct.Fields.FirstOrDefault(x => x.IsKey)?.GetClrTypeName(baseMember);
                    }
                }
                else
                {
                    clrType = this.FbsFieldType;
                }
            }

            if (this.IsOptionalScalar)
            {
                // nullable.
                clrType = $"System.Nullable<{clrType}>";
            }
            else if (this.SharedString)
            {
                clrType = $"{typeof(SharedString).FullName}";
            }

            return this.GetClrTypeName(clrType, sortKeyType);
        }

        public void WriteCopyConstructorLine(CodeWriter writer, string sourceName, BaseSchemaMember parent)
        {
            bool isBuiltIn = SchemaDefinition.TryResolve(this.FbsFieldType, out _);

            bool foundNodeType = parent.TryResolveName(this.FbsFieldType, out var nodeType);
            if (!isBuiltIn && !foundNodeType)
            {
                ErrorContext.Current.RegisterError($"Unable to resolve type '{this.FbsFieldType}' as a built in or defined type");
                return;
            }

            string selectStatement = this.GetLinqSelectStatement(isBuiltIn, nodeType);

            switch (this.VectorType)
            {
                case VectorType.IList:
                case VectorType.IReadOnlyList:
                    writer.AppendLine($"this.{this.Name} = {sourceName}.{this.Name}?{selectStatement}.ToList();");
                    break;

                case VectorType.Array:
                    writer.AppendLine($"this.{this.Name} = {sourceName}.{this.Name}?{selectStatement}.ToArray();");
                    break;

                case VectorType.Memory:
                case VectorType.ReadOnlyMemory:
                    writer.AppendLine($"this.{this.Name} = {sourceName}.{this.Name}.ToArray();");
                    break;

                case VectorType.IIndexedVector:
                    writer.AppendLine($"this.{this.Name} = {sourceName}.{this.Name}?.Clone(x => {nodeType.GetCopyExpression("x")});");
                    break;

                case VectorType.None:
                {
                    if (isBuiltIn)
                    {
                        writer.AppendLine($"this.{this.Name} = {sourceName}.{this.Name};");
                    }
                    else
                    {
                        string cloneStatement = nodeType.GetCopyExpression($"{sourceName}.{this.Name}");
                        writer.AppendLine($"this.{this.Name} = {cloneStatement};");
                    }
                }
                break;
            }
        }

        private string GetLinqSelectStatement(bool isBuiltIn, BaseSchemaMember nodeType) => 
            isBuiltIn ? string.Empty : $".Select(x => {nodeType.GetCopyExpression("x")})";

        public void WriteField(CodeWriter writer, TableOrStructDefinition parent, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.Name, (Action)(() =>
            {
                bool isVector = this.VectorType != VectorType.None;
                EnumDefinition enumDefinition = null;

                if (parent.TryResolveName(this.FbsFieldType, out var typeDefinition))
                {
                    enumDefinition = typeDefinition as EnumDefinition;
                }

                bool isBuiltInType = SchemaDefinition.TryResolve(this.FbsFieldType, out ITypeModel builtInType);

                string clrType = isBuiltInType
                    ? builtInType.ClrType.FullName
                    : typeDefinition?.GlobalName ?? this.FbsFieldType;

                string defaultValue = this.GetDefaultValue(builtInType, enumDefinition, clrType);

                this.VerifySetterKindIsValid();
                this.WriteField(writer, this.GetClrTypeName(parent), defaultValue, this.Name, parent, context);
            }));
        }

        private void WriteField(
            CodeWriter writer,
            string clrTypeName,
            string defaultValue,
            string name,
            TableOrStructDefinition parent,
            CompileContext context)
        {
            string defaultValueAttribute = string.Empty;
            string defaultValueAssignment = string.Empty;
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string isDeprecated = string.Empty;

            ITypeModel typeModel = null;
            if (context.PreviousAssembly != null)
            {
                typeModel = this.GetTypeModel(parent, context);
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                defaultValueAttribute = $", DefaultValue = {defaultValue}";
                defaultValueAssignment = $" = {defaultValue};";
            }
            else if (typeModel?.SchemaType == FlatBufferSchemaType.Struct)
            {
                // structs are non-null but Flatsharp uses classes for structs, so we need to initialize them
                defaultValueAssignment = $" = new {clrTypeName}();";
            }

            if (this.SortedVector)
            {
                sortedVector = ", SortedVector = true";
            }

            if (this.IsKey)
            {
                isKey = ", Key = true";
            }

            if (this.Deprecated)
            {
                isDeprecated = ", Deprecated = true";
            }

            bool isNonVirtual = false;
            if (this.NonVirtual != null)
            {
                isNonVirtual = this.NonVirtual.Value;
            }
            else if (parent.NonVirtual != null)
            {
                isNonVirtual = parent.NonVirtual.Value;
            }

            string setter = this.SetterKind switch
            {
                SetterKind.Public => "set;", // setter has same access as getter.
                SetterKind.PublicInit => "init;",
                SetterKind.Protected => "protected set;",
                SetterKind.ProtectedInit => "protected init;",
                SetterKind.ProtectedInternal => "protected internal set;",
                SetterKind.ProtectedInternalInit => "protected internal init;",
                SetterKind.None => string.Empty,
                _ => string.Empty,
            };

            string nullableReferenceIndicator = string.Empty;
            if (context.Options.NullableAnnotations && context.CompilePass == CodeWritingPass.SecondPass)
            {
                if (typeModel.SchemaType == FlatBufferSchemaType.Table ||
                    typeModel.SchemaType == FlatBufferSchemaType.Vector ||
                    typeModel.SchemaType == FlatBufferSchemaType.Union ||
                    typeModel.SchemaType == FlatBufferSchemaType.String)
                {
                    if (!typeModel.ClrType.IsValueType)
                    {
                        nullableReferenceIndicator = "?";
                    }
                }
            }

            writer.AppendLine($"[FlatBufferItem({this.Index}{defaultValueAttribute}{isDeprecated}{sortedVector}{isKey})]");
            writer.AppendLine($"public {(isNonVirtual ? string.Empty : "virtual ")}{clrTypeName}{nullableReferenceIndicator} {name} {{ get; {setter} }}{defaultValueAssignment}");
        }

        private ITypeModel GetTypeModel(
            BaseSchemaMember parent,
            CompileContext context)
        {
            if (context.PreviousAssembly == null)
            {
                return null;
            }

            Type parentType = context.PreviousAssembly.GetType(parent.FullName);
            var thisProperty = parentType.GetProperty(this.Name);
            return context.TypeModelContainer.CreateTypeModel(thisProperty.PropertyType);
        }

        private string GetDefaultValue(ITypeModel builtInType, EnumDefinition enumDefinition, string clrType)
        {
            if (string.IsNullOrEmpty(this.DefaultValue))
            {
                return string.Empty;
            }

            string defaultValue = string.Empty;
            if (builtInType != null &&
                builtInType.TryFormatStringAsLiteral(this.DefaultValue, out defaultValue))
            {
                // intentionally left blank.
                return defaultValue;
            }

            if (enumDefinition?.NameValuePairs.ContainsKey(this.DefaultValue) == true)
            {
                // Also ok.
                return $"{clrType}.{this.DefaultValue}";
            }

            if (enumDefinition?.UnderlyingType.TryFormatStringAsLiteral(this.DefaultValue, out defaultValue) == true)
            {
                return $"({clrType})({defaultValue})";
            }

            ErrorContext.Current?.RegisterError(
                $"Only primitive types and enums may have default values. Field '{this.Name}' declares a default value but has type '{this.FbsFieldType}'.");

            return string.Empty;
        }

        private void VerifySetterKindIsValid()
        {
            if (this.SetterKind == SetterKind.None &&
                this.NonVirtual == true)
            {
                ErrorContext.Current?.RegisterError("NonVirtual:true cannot be combined with setter:None.");
            }
        }

        private string GetClrTypeName(string clrType, string sortKeyType)
        {
            switch (this.VectorType)
            {
                case VectorType.Array:
                    return $"{clrType}[]";

                case VectorType.IList:
                    return $"IList<{clrType}>";

                case VectorType.IReadOnlyList:
                    return $"IReadOnlyList<{clrType}>";

                case VectorType.Memory:
                    return $"Memory<{clrType}>";

                case VectorType.ReadOnlyMemory:
                    return $"ReadOnlyMemory<{clrType}>";

                case VectorType.IIndexedVector:
                    if (string.IsNullOrWhiteSpace(sortKeyType))
                    {
                        ErrorContext.Current.RegisterError($"Unable to determine key type for table {clrType}. Please make sure a property has the 'Key' metadata.");
                    }

                    return $"IIndexedVector<{sortKeyType}, {clrType}>";

                case VectorType.None:
                    return clrType;

                default:
                    throw new InvalidOperationException($"Unexpected value for vectortype: '{this.VectorType}'");
            }
        }
    }
}
