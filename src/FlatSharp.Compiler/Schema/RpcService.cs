namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table Service {
    name:string (required, key);
    calls:[RPCCall];
    attributes:[KeyValue];
    documentation:[string];
    /// File that this Service is declared in.
    declaration_file: string;
}
*/
    [FlatBufferTable]
    public class RpcService
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1)]
        public virtual IIndexedVector<string, RpcCall>? Calls { get; set; }

        [FlatBufferItem(2)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(3)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(4)]
        public virtual string? DeclaringFile { get; set; }
    }
}
