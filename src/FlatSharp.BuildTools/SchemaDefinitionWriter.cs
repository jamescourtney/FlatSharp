namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /*
internal static class SchemaDefinitionWriter
{
    public static string Write(SchemaDefinition sd, List<string> errorAggregator)
    {
        var builder = new StringBuilder();

        // usings that we'll need.
        builder.AppendLine("using System;");
        builder.AppendLine("using System.Collections.Generic;");
        builder.AppendLine("using FlatSharp;");
        builder.AppendLine();

        builder.AppendLine($"namespace {sd.NamespaceName}");
        builder.AppendLine("{");


        foreach (var enumDef in sd.Enums)
        {
            WriteEnum(enumDef, builder, errorAggregator, sd);
        }

        foreach (var typeDef in sd.CustomTypes)
        {
            WriteCustomType(typeDef, builder, errorAggregator, sd);
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    private static void WriteEnum(EnumDefinition ed, StringBuilder b, List<string> errorAggregator, SchemaDefinition schemaDef)
    {
        b.AppendLine($"\tpublic enum {ed.TypeName} : {MapSimpleType(ed.UnderlyingType)}");
        b.AppendLine("\t{");

        int value = 1;
        foreach (var item in ed.EnumNameValueMapping)
        {
            if (!string.IsNullOrEmpty(item.value))
            {
                if (!int.TryParse(item.value, out value))
                {
                    errorAggregator.Add($"Invalid value for enum member '{ed.Namespace}.{ed.TypeName}.{item.name}. Value must be an integer. Got value = {item.value}");
                    continue;
                }
            }

            b.AppendLine($"\t\t{item.name} = {value},");
            value++;
        }

        b.AppendLine("\t}");
    }

    private static void WriteCustomType(TableOrStructDefinition def, StringBuilder b, List<string> errorAggregator, SchemaDefinition schemaDef)
    {
        string attribute = def.IsTable ? "FlatBufferTable" : "FlatBufferStruct";

        b.AppendLine($"\t[{attribute}]");
        b.AppendLine($"\tpublic class {def.TypeName}");
        b.AppendLine("\t{");

        // Generate explicit ctor.
        b.AppendLine($"\t\tpublic {def.TypeName}() {{ }}");

        foreach (var fieldDef in def.Fields)
        {
            b.AppendLine();
            string clrType = MapFbsTypeToClrType(fieldDef, schemaDef);

            EnumDefinition enumDef = schemaDef.Enums.FirstOrDefault(d => d.TypeName == fieldDef.FieldType);

            string attributeDefaultValue = string.Empty;
            string defaultValueAssignment = string.Empty;
            if (!string.IsNullOrEmpty(fieldDef.DefaultValue))
            {
                if (enumDef == null)
                {
                    attributeDefaultValue = $", DefaultValue = {FormatValue(clrType, fieldDef.DefaultValue)}";
                    defaultValueAssignment = $" = {FormatValue(clrType, fieldDef.DefaultValue)};";
                }
                else
                {
                    var rawValue = enumDef.EnumNameValueMapping.FirstOrDefault(x => x.name == fieldDef.DefaultValue).value;
                    if (string.IsNullOrEmpty(rawValue))
                    {
                        throw new InvalidFbsFileException($"Unable to find raw value of enum: {enumDef.TypeName}.{fieldDef.DefaultValue}");
                    }

                    attributeDefaultValue = $", DefaultValue = {FormatValue(enumDef.UnderlyingType, fieldDef.DefaultValue)}";
                    defaultValueAssignment = $" = {FormatValue(enumDef.UnderlyingType, fieldDef.DefaultValue)};";
                }
            }

            if (enumDef == null)
            {
                b.AppendLine($"\t\t[FlatBufferItem({fieldDef.Index}, Deprecated = {fieldDef.Deprecated.ToString().ToLower()}{attributeDefaultValue})]");
                b.AppendLine($"\t\tpublic virtual {clrType} {fieldDef.Name} {{ get; set; }}{defaultValueAssignment}");
            }
            else
            {
                string underlyingType = MapSimpleType(enumDef.UnderlyingType);

                b.AppendLine($"\t\tpublic {clrType} {fieldDef.Name} {{ get => ({clrType})this.__Raw{fieldDef.Name}; set => this.__Raw{fieldDef.Name} = ({underlyingType})value; }}");

                b.AppendLine($"\t\t[FlatBufferItem({fieldDef.Index}, Deprecated = {fieldDef.Deprecated.ToString().ToLower()}{attributeDefaultValue})]");
                b.AppendLine($"\t\tprotected virtual {underlyingType} __Raw{fieldDef.Name} {{ get; set; }}{defaultValueAssignment}");
            }
        }

        b.AppendLine("\t}");
    }

    private static string MapFbsTypeToClrType(FieldDefinition fieldDef, SchemaDefinition schemaDef)
    {
        string type = fieldDef.FieldType;
        if (type.StartsWith("["))
        {
            type = type.Substring(1, type.Length - 2);

            switch (fieldDef.VectorType)
            {
                case VectorType.IList:
                    return $"IList<{type}>";
                case VectorType.IReadOnlyList:
                    return $"IReadOnlyList<{type}>";
                case VectorType.Array:
                    return $"{type}[]";
                case VectorType.Memory:
                    return $"Memory<{type}>";
                case VectorType.ReadOnlyMemory:
                    return $"ReadOnlyMemory<{type}>";
                default:
                    throw new InvalidFbsFileException("Unrecognized VectorType: " + fieldDef.VectorType);
            }
        }

        var unionDef = schemaDef.Unions.FirstOrDefault(x => x.TypeName == type);
        if (unionDef != null)
        {
            var componentTypes = unionDef.ComponentTypeNames.Select(x => MapSimpleType(x));
            return $"FlatBufferUnion<{string.Join(",", componentTypes)}>";
        }

        return MapSimpleType(type);
    }

    private static string MapSimpleType(string type) 
    {
        switch (type)
        {
            case "byte":
            case "int8":
                return "sbyte";

            case "ubyte":
            case "uint8":
                return "byte";

            case "int16":
                return "short";

            case "uint16":
                return "ushort";

            case "int32":
                return "int";

            case "uint32":
                return "uint";

            case "int64":
                return "long";

            case "uint64":
                return "ulong";

            case "float32":
                return "float";

            case "float64":
                return "double";

            default:
                return type;
        }
    }

    private static string FormatValue(string type, string value)
    {
        switch (type)
        {
            case "byte":
                return $"(byte){value}";

            case "sbyte":
                return $"(sbyte){value}";

            case "short":
                return $"(short){value}";

            case "ushort":
                return $"(ushort){value}";

            case "int":
                return value;

            case "uint":
                return $"{value}u";

            case "long":
                return $"{value}L";

            case "ulong":
                return $"{value}ul";

            case "float":
                return $"{value}f";

            case "double":
                return $"{value}d";

            default:
                throw new InvalidOperationException("Unexpected type: " + type);
        }
    }
}*/
}
