namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;

    internal class TableOrStructDefinition : BaseSchemaMember
    {
        public TableOrStructDefinition(
            string name, 
            BaseSchemaMember parent) : base(name, parent)
        {
        }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
        
        public bool IsTable { get; set; }

        protected override bool SupportsChildren => false;

        public void AssignIndexes()
        {
            ErrorContext.Current.WithScope(this.Name, () =>
            {
                int nextIndex = 0;
                for (int i = 0; i < this.Fields.Count; ++i)
                {
                    this.Fields[i].Index = nextIndex;

                    nextIndex++;

                    if (this.TryResolveName(this.Fields[i].FbsFieldType, out var typeDef) && typeDef is UnionDefinition)
                    {
                        // Unions are double-wide.
                        nextIndex++;
                    }
                }
            });
        }

        protected override void OnWriteCode(CodeWriter writer)
        {
            this.AssignIndexes();

            string attribute = this.IsTable ? "[FlatBufferTable]" : "[FlatBufferStruct]";

            writer.AppendLine(attribute);
            writer.AppendLine($"public class {this.Name} : object");
            writer.AppendLine($"{{");

            using (writer.IncreaseIndent())
            {
                foreach (var field in this.Fields)
                {
                    if (!this.IsTable && field.Deprecated)
                    {
                        ErrorContext.Current?.RegisterError($"FlatBuffer structs may not have deprecated fields.");
                    }

                    field.WriteField(writer, this);
                }
            }

            writer.AppendLine($"}}");
        }
    }
}
