namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal interface IFieldDefinitionEmitter
    {
        bool EmitDefinition(
            CodeWriter writer,
            FieldDefinition fieldDefinition,
            CompileContext context);

        void EmitCloneLine(
            CodeWriter writer,
            string variableName,
            FieldDefinition fieldDefinition,
            CompileContext compileContext);

        void EmitDefaultInitializationLine(
            CodeWriter writer,
            FieldDefinition fieldDefinition,
            CompileContext compileContext);
    }
}
