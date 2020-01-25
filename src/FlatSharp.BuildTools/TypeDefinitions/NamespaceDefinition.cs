using System;

namespace FlatSharp.Compiler
{
    internal class NamespaceDefinition : BaseSchemaMember
    {
        public NamespaceDefinition(string name, BaseSchemaMember parent) : base(name, parent)
        {
        }

        protected override bool SupportsChildren => true;

        protected override void OnWriteCode(CodeWriter writer)
        {
            writer.AppendLine($"namespace {this.Name}");
            writer.AppendLine($"{{");
            using (writer.IncreaseIndent())
            {
                foreach (var type in this.Children.Values)
                {
                    type.WriteCode(writer);
                }
            }
            writer.AppendLine($"}}");
        }
    }
}
