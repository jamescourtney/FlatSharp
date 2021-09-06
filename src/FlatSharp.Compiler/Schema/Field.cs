namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table Field {
    name:string (required, key);
    type:Type (required);
    id:ushort;
    offset:ushort;  // Offset into the vtable for tables, or into the struct.
    default_integer:long = 0;
    default_real:double = 0.0;
    deprecated:bool = false;
    required:bool = false;
    key:bool = false;
    attributes:[KeyValue];
    documentation:[string];
    optional:bool = false;
}
*/
    [FlatBufferTable]
    public class Field
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual FlatBufferType Type { get; set; } = new();

        [FlatBufferItem(2)]
        public virtual ushort Id { get; set; }

        // Offset into the vtable for tables, or into the struct.
        [FlatBufferItem(3)]
        public virtual ushort Offset { get; set; }

        [FlatBufferItem(4)]
        public virtual long DefaultInteger { get; set; }
        
        [FlatBufferItem(5)]
        public virtual double DefaultDouble { get; set; }

        [FlatBufferItem(6, DefaultValue = false)]
        public bool Deprecated { get; set; } = false;

        [FlatBufferItem(7, DefaultValue = false)]
        public virtual bool Required { get; set; } = false;

        [FlatBufferItem(8, DefaultValue = false)]
        public virtual bool Key { get; set; } = false;

        [FlatBufferItem(9)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(10)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(11, DefaultValue = false)]
        public virtual bool Optional { get; set; } = false;
    }
}
