namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table RPCCall {
    name:string (required, key);
    request:Object (required);      // must be a table (not a struct)
    response:Object (required);     // must be a table (not a struct)
    attributes:[KeyValue];
    documentation:[string];
}
*/
    [FlatBufferTable]
    public class RpcCall
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1, Required = true)]
        public virtual FlatBufferObject Request { get; set; } = new FlatBufferObject();

        // Must be a table.
        [FlatBufferItem(2, Required = true)]
        public virtual FlatBufferObject Response { get; set; } = new FlatBufferObject();

        [FlatBufferItem(3)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(4)]
        public virtual IList<string>? Documentation { get; set; }
    }
}
