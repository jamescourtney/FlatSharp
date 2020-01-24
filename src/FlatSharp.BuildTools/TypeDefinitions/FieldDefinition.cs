using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace FlatSharp.Compiler
{
    internal class FieldDefinition : IFieldDefinition
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public string DefaultValue { get; set; }

        public VectorType VectorType { get; set; }

        public void WriteField(CodeWriter writer, SchemaDefinition schemaDefinition)
        {
            ErrorContext.Current.WithScope(this.Name, () =>
            {
                bool isVector = this.VectorType != VectorType.None;
                EnumDefinition enumDefinition = null;

                if (schemaDefinition.TryGetTypeDefinition(this.FbsFieldType, out var typeDefinition))
                {
                    enumDefinition = typeDefinition as EnumDefinition;

                    if (typeDefinition is UnionDefinition unionDef)
                    {
                        this.WriteField(writer, unionDef.ClrTypeName, null, this.Name);
                        return;
                    }
                }

                if (isVector)
                {
                    this.WriteVectorField(writer);
                    return;
                }

                string defaultValue = string.Empty;

                bool isPrimitive = SchemaDefinition.TryResolvePrimitiveType(this.FbsFieldType, out string clrType);
                if (!isPrimitive)
                {
                    clrType = this.FbsFieldType;
                }

                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    if (isPrimitive)
                    {
                        defaultValue = CodeWriter.GetPrimitiveTypeLiteral(clrType, this.DefaultValue);
                    }
                    else if (enumDefinition != null)
                    {
                        if (enumDefinition.NameValuePairs.ContainsKey(this.DefaultValue))
                        {
                            // Referenced by name.
                            defaultValue = $"{enumDefinition.TypeName}.{this.DefaultValue}";
                        }
                        else
                        {
                            defaultValue = CodeWriter.GetPrimitiveTypeLiteral(enumDefinition.ClrUnderlyingType, this.DefaultValue);
                        }
                    }
                    else
                    {
                        ErrorContext.Current?.RegisterError($"Only primitive types and enums may have default values. Field '{this.Name}' declares a default value but has type '{this.FbsFieldType}'.");
                    }
                }

                this.WriteField(writer, clrType, defaultValue, this.Name);
            });
        }

        private void WriteVectorField(CodeWriter writer)
        {
            string innerType = this.FbsFieldType;
            if (!SchemaDefinition.TryResolvePrimitiveType(innerType, out string clrType))
            {
                clrType = innerType;
            }
            
            switch (this.VectorType)
            {
                case VectorType.Array:
                    clrType = $"{clrType}[]";
                    break;

                case VectorType.IList:
                    clrType = $"IList<{clrType}>";
                    break;

                case VectorType.IReadOnlyList:
                    clrType = $"IReadOnlyList<{clrType}>";
                    break;

                case VectorType.Memory:
                    clrType = $"Memory<{clrType}>";
                    break;

                case VectorType.ReadOnlyMemory:
                    clrType = $"ReadOnlyMemory<{clrType}>";
                    break;

                default:
                    ErrorContext.Current?.RegisterError("Unexpected vector type: " + this.VectorType);
                    clrType = $"IList<{clrType}>";
                    break;
            }

            this.WriteField(writer, clrType, null, this.Name);
        }

        private void WriteField(CodeWriter writer, string clrTypeName, string defaultValue, string name, string accessModifier = "public")
        {
            string defaultValueAttribute = string.Empty;
            string defaultValueAssignment = string.Empty;

            if (!string.IsNullOrEmpty(defaultValue))
            {
                defaultValueAttribute = $", DefaultValue = {defaultValue}";
                defaultValueAssignment = $" = {defaultValue};";
            }

            writer.AppendLine($"[FlatBufferItem({this.Index}{this.GetDeprecatedString()}{defaultValueAttribute})]");
            writer.AppendLine($"{accessModifier} virtual {clrTypeName} {name} {{ get; set; }}{defaultValueAssignment}");
        }

        private string GetDeprecatedString()
        {
            if (this.Deprecated)
            {
                return ", Deprecated = true";
            }

            return string.Empty;
        }
    }
}
