namespace FlatSharp.Compiler
{
    /// <summary>
    /// Defines an enum.
    /// </summary>
    internal class RootNodeDefinition : BaseSchemaMember
    {
        public RootNodeDefinition() : base("", null)
        {
        }

        protected override bool SupportsChildren => true;

        protected override void OnWriteCode(CodeWriter writer)
        {
            writer.AppendLine("using System;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using FlatSharp;");
            writer.AppendLine("using FlatSharp.Attributes;");

            foreach (var child in this.Children.Values)
            {
                child.WriteCode(writer);
            }
        }
    }
}
