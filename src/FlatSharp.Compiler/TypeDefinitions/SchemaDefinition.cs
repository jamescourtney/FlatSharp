using System;
using System.Collections.Generic;

namespace FlatSharp.Compiler
{
    internal class SchemaDefinition
    {
        public BaseSchemaMember Root { get; set; }

        public void Write(CodeWriter writer)
        {
            writer.AppendLine("using System;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using FlatSharp;");
            writer.AppendLine("using FlatSharp.Attributes;");

            this.Root.WriteCode(writer);
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
    }
}
