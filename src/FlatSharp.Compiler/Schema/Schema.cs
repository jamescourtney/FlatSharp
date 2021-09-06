namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;

/*
table Schema {
    objects:[Object] (required);    // Sorted.
    enums:[Enum] (required);        // Sorted.
    file_ident:string;
    file_ext:string;
    root_table:Object;
    services:[Service];             // Sorted.
    advanced_features:AdvancedFeatures;
    /// All the files used in this compilation. Files are relative to where
    /// flatc was invoked.
    fbs_files:[SchemaFile];         // Sorted.
}
*/
    [FlatBufferTable]
    public class Schema
    {
        [FlatBufferItem(0, Required = true)]
        public virtual IIndexedVector<string, FlatBufferObject> Objects { get; set; } = new IndexedVector<string, FlatBufferObject>();

        [FlatBufferItem(1, Required = true)]
        public virtual IIndexedVector<string, FlatBufferEnum> Enums { get; set; } = new IndexedVector<string, FlatBufferEnum>();

        [FlatBufferItem(2)]
        public virtual string? FileIdentifier { get; set; }

        [FlatBufferItem(3)]
        public virtual string? FileExtension { get; set; }

        [FlatBufferItem(4)]
        public virtual FlatBufferObject? RootTable { get; set; }

        [FlatBufferItem(5)]
        public virtual IIndexedVector<string, RpcService>? Services { get; set; }

        [FlatBufferItem(6)]
        public virtual AdvancedFeatures AdvancedFeatures { get; set; }

        [FlatBufferItem(7)]
        public virtual IIndexedVector<string, SchemaFile>? FbsFiles { get; set; }

        internal void WriteCode(CodeWriter writer, CompileContext context)
        { 
            foreach (var kvp in this.Enums)
            {
                string name = kvp.Key;
                FlatBufferEnum @enum = kvp.Value;

                @enum.WriteCode(writer, context);
            }
        }
    }
}
