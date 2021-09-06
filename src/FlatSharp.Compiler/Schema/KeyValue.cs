namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;

    [FlatBufferTable]
    public class KeyValue
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Key { get; set; } = string.Empty;

        [FlatBufferItem(1)]
        public virtual string? Value { get; set; }
    }
}
