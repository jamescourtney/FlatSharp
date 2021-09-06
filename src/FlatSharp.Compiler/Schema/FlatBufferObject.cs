namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table Object {  // Used for both tables and structs.
    name:string (required, key);
    fields:[Field] (required);  // Sorted.
    is_struct:bool = false;
    minalign:int;
    bytesize:int;  // For structs.
    attributes:[KeyValue];
    documentation:[string];
    /// File that this Object is declared in.
    declaration_file: string;
}
*/
    [FlatBufferTable]
    public class FlatBufferObject
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual IIndexedVector<string, Field> Fields { get; set; } = new IndexedVector<string, Field>();

        [FlatBufferItem(2)]
        public virtual bool IsStruct { get; set; }

        [FlatBufferItem(3)]
        public virtual int MinAlign { get; set; }

        // For structs, the size.
        [FlatBufferItem(4)]
        public virtual int ByteSize { get; set; }

        [FlatBufferItem(5)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(6)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(7)]
        public virtual string? DeclarationFile { get; set; }
    }
}
