namespace FlatSharp.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a Union.
    /// </summary>
    internal class UnionDefinition : BaseSchemaMember
    {
        public UnionDefinition(string name, BaseSchemaMember parent) : base(name, parent)
        {
        }

        public List<string> ComponentTypeNames { get; set; } = new List<string>();

        public string ClrTypeName
        {
            get
            {
                return $"FlatBufferUnion<{string.Join(", ", this.ComponentTypeNames)}>";
            }
        }

        protected override bool SupportsChildren => false;

        protected override void OnWriteCode(CodeWriter writer)
        {
        }
    }
}
