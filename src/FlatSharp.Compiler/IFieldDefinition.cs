namespace FlatSharp.Compiler
{
    using System.Collections.Generic;
    using System.Text;

    internal interface IFieldDefinition
    {
        string Name { get; }

        string FbsFieldType { get; }

        void WriteField(CodeWriter writer, BaseSchemaMember schemaDefinition);
    }
}
