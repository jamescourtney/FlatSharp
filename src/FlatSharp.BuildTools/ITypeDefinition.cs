namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;

    internal interface ITypeDefinition
    {
        string TypeName { get; }

        string Namespace { get; }

        void WriteType(CodeWriter writer, SchemaDefinition schemaDefinition);
    }
}
