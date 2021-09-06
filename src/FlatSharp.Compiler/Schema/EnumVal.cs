namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table EnumVal {
    name:string (required);
    value:long (key);
    object:Object (deprecated);
    union_type:Type;
    documentation:[string];
}
*/

    [FlatBufferTable]
    public class EnumVal
    {
        [FlatBufferItem(0, Required = true)]
        public virtual string Key { get; set; } = string.Empty;

        [FlatBufferItem(1, Key = true)]
        public virtual long Value { get; set; }

        [FlatBufferItem(2, Deprecated =  true)]
        public bool Object { get; set; }

        [FlatBufferItem(3)]
        public virtual FlatBufferType? UnionType { get; set; }

        [FlatBufferItem(4)]
        public virtual IList<string>? Documentation { get; set; }
    }
}
