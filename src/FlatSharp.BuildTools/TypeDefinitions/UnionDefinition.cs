namespace FlatSharp.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a Union.
    /// </summary>
    internal class UnionDefinition : ITypeDefinition
    {
        public string Namespace { get; set; }

        public List<string> ComponentTypeNames { get; set; } = new List<string>();

        public string TypeName { get; set; }

        public string ClrTypeName
        {
            get
            {
                return $"FlatBufferUnion<{string.Join(", ", this.ComponentTypeNames)}>";
            }
        }

        public void WriteType(CodeWriter writer, SchemaDefinition schemaDefinition)
        {
        }
    }
}
