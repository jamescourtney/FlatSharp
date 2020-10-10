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

namespace FlatSharp.Compiler
{
    internal class FieldDefinition
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public bool SortedVector { get; set; }

        public bool IsKey { get; set; }

        public bool? Virtual { get; set; }

        public string DefaultValue { get; set; }

        public bool IsOptionalScalar { get; set; }

        public VectorType VectorType { get; set; }

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
                    if (typeDefinition is UnionDefinition unionDef)
                    {
                        clrType = unionDef.ClrTypeName;
                    }
                    else
                    {
                        clrType = typeDefinition.GlobalName;
                    }

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
                clrType = $"global::{typeof(SharedString).FullName}";
            }

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

        private string GetLinqSelectStatement(bool isBuiltIn, BaseSchemaMember nodeType)
        {
            if (isBuiltIn)
            {
                return string.Empty;
            }
            else
            {
                string cloneStatement = nodeType.GetCopyExpression("x");
                return $".Select(x => {cloneStatement})";
            }
        }

        public void WriteField(CodeWriter writer, TableOrStructDefinition parent)
        {
            ErrorContext.Current.WithScope(this.Name, (Action)(() =>
            {
                bool isVector = this.VectorType != VectorType.None;
                EnumDefinition enumDefinition = null;

                if (parent.TryResolveName(this.FbsFieldType, out var typeDefinition))
                {
                    enumDefinition = typeDefinition as EnumDefinition;
                }

                string defaultValue = string.Empty;
                string clrType;
                bool isBuiltInType = SchemaDefinition.TryResolve(this.FbsFieldType, out ITypeModel builtInType);

                if (isBuiltInType)
                {
                    clrType = builtInType.ClrType.FullName;
                }
                else
                {
                    clrType = typeDefinition?.GlobalName ?? this.FbsFieldType;
                }

                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    if (isBuiltInType && builtInType.TryFormatStringAsLiteral(this.DefaultValue, out defaultValue))
                    {
                        // intentionally left blank.
                    }
                    else if (enumDefinition?.NameValuePairs.ContainsKey(this.DefaultValue) == true)
                    {
                        // Also ok.
                        defaultValue = $"{clrType}.{this.DefaultValue}";
                    }
                    else if (enumDefinition?.UnderlyingType.TryFormatStringAsLiteral(this.DefaultValue, out defaultValue) == true)
                    { 
                        defaultValue = $"({clrType})({defaultValue})";
                    }
                    else
                    {
                        ErrorContext.Current?.RegisterError($"Only primitive types and enums may have default values. Field '{this.Name}' declares a default value but has type '{this.FbsFieldType}'.");
                    }
                }

                this.WriteField(writer, this.GetClrTypeName(parent), defaultValue, this.Name, parent);
            }));
        }

        private void WriteField(
            CodeWriter writer,
            string clrTypeName,
            string defaultValue,
            string name,
            TableOrStructDefinition parent,
            string accessModifier = "public")
        {
            string defaultValueAttribute = string.Empty;
            string defaultValueAssignment = string.Empty;
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string isDeprecated = string.Empty;

            if (!string.IsNullOrEmpty(defaultValue))
            {
                defaultValueAttribute = $", DefaultValue = {defaultValue}";
                defaultValueAssignment = $" = {defaultValue};";
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

            bool isVirtual = true;
            if (this.Virtual != null)
            {
                isVirtual = this.Virtual.Value;
            }
            else if (parent.Virtual != null)
            {
                isVirtual = parent.Virtual.Value;
            }

            writer.AppendLine($"[FlatBufferItem({this.Index}{defaultValueAttribute}{isDeprecated}{sortedVector}{isKey})]");
            writer.AppendLine($"{accessModifier} {(isVirtual ? "virtual " : string.Empty)}{clrTypeName} {name} {{ get; set; }}{defaultValueAssignment}");
        }
    }
}
