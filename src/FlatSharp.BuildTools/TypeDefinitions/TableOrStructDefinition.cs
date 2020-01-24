namespace FlatSharp.Compiler
{
    using System.Collections.Generic;

    internal class TableOrStructDefinition : ITypeDefinition
    {
        public string Namespace { get; set; }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();

        public string TypeName { get; set; }

        public bool IsTable { get; set; }

        public void AssignIndexes(SchemaDefinition definition)
        {
            ErrorContext.Current.WithScope(this.TypeName, () =>
            {
                int nextIndex = 0;
                for (int i = 0; i < this.Fields.Count; ++i)
                {
                    this.Fields[i].Index = nextIndex;

                    nextIndex++;

                    if (definition.TryGetTypeDefinition(this.Fields[i].FbsFieldType, out var typeDef) && typeDef is UnionDefinition)
                    {
                        // Unions are double-wide.
                        nextIndex++;
                    }
                }
            });
        }

        public void WriteType(CodeWriter writer, SchemaDefinition schemaDefinition)
        {
            ErrorContext.Current.WithScope(this.TypeName, () =>
            {
                string attribute = this.IsTable ? "[FlatBufferTable]" : "[FlatBufferStruct]";

                writer.AppendLine(attribute);
                writer.AppendLine($"public class {this.TypeName} : object");
                writer.AppendLine($"{{");

                using (writer.IncreaseIndent())
                {
                    foreach (var field in this.Fields)
                    {
                        if (!this.IsTable && field.Deprecated)
                        {
                            ErrorContext.Current?.RegisterError($"FlatBuffer structs may not have deprecated fields.");
                        }

                        field.WriteField(writer, schemaDefinition);
                    }
                }

                writer.AppendLine($"}}");
            });
        }
    }
}
