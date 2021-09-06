namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
/// File specific information.
/// Symbols declared within a file may be recovered by iterating over all
/// symbols and examining the `declaration_file` field.
table SchemaFile {
  /// Filename, relative to project root.
  filename:string (required, key);
  /// Names of included files, relative to project root.
  included_filenames:[string];
}

*/
    [FlatBufferTable]
    public class SchemaFile
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string FileName { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1)]
        public virtual IList<string>? IncludedFileNames { get; set; }
    }
}
