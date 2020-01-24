using System;
using System.Collections.Generic;

namespace FlatSharp.Compiler
{
    internal class SchemaDefinition
    {
        // Maps fully qualified type name to the definition.
        private readonly Dictionary<string, ITypeDefinition> types = new Dictionary<string, ITypeDefinition>(StringComparer.OrdinalIgnoreCase);

        public string NamespaceName { get; set; }

        public IReadOnlyDictionary<string, ITypeDefinition> Types => this.types;

        public void Write(CodeWriter writer)
        {
            writer.AppendLine("using System;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using FlatSharp;");
            writer.AppendLine("using FlatSharp.Attributes;");

            ErrorContext.Current.WithScope(this.NamespaceName, () =>
            {
                writer.AppendLine($"namespace {this.NamespaceName}");
                writer.AppendLine($"{{");
                using (writer.IncreaseIndent())
                {
                    foreach (var type in this.types.Values)
                    {
                        type.WriteType(writer, this);
                    }
                }
                writer.AppendLine($"}}");
            });
        }

        public bool TryGetTypeDefinition(string typeName, out ITypeDefinition typeDefinition)
        {
            return this.types.TryGetValue(this.GetFullyQualifiedTypeName(typeName), out typeDefinition);
        }

        public void AddType(ITypeDefinition typeDefinition)
        {
            string fullyQualifiedName = this.GetFullyQualifiedTypeName(typeDefinition.TypeName);
            if (this.types.ContainsKey(fullyQualifiedName))
            {
                ErrorContext.Current?.RegisterError($"Duplicate type name: '{fullyQualifiedName}'.");
            }

            this.types[fullyQualifiedName] = typeDefinition;
        }

        public static string ResolvePrimitiveType(string type)
        {
            if (!TryResolvePrimitiveType(type, out string clrtype))
            {
                ErrorContext.Current?.RegisterError("Unexpected primitive type: " + type);
            }

            return clrtype;
        }

        public static bool TryResolvePrimitiveType(string type, out string clrType)
        {
            switch (type)
            {
                case "bool":
                    clrType = "bool";
                    break;

                case "byte":
                case "int8":
                    clrType = "sbyte";
                    break;

                case "ubyte":
                case "uint8":
                    clrType = "byte";
                    break;

                case "int16":
                case "short":
                    clrType = "short";
                    break;

                case "uint16":
                case "ushort":
                    clrType = "ushort";
                    break;

                case "int32":
                case "int":
                    clrType = "int";
                    break;

                case "uint32":
                case "uint":
                    clrType = "uint";
                    break;

                case "int64":
                case "long":
                    clrType = "long";
                    break;

                case "uint64":
                case "ulong":
                    clrType = "ulong"; 
                    break;

                case "float32":
                case "float":
                    clrType = "float";
                    break;

                case "float64":
                case "double":
                    clrType = "double";
                    break;

                default:
                    clrType = null;
                    return false;
            }

            return true;
        }

        private string GetFullyQualifiedTypeName(string typeName)
        {
            if (typeName.IndexOf(".") >= 0)
            {
                // Already namespace'd.
                return typeName;
            }

            return this.NamespaceName + "." + typeName;
        }
    }
}
