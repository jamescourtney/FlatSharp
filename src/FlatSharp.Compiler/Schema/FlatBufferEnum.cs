namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;
    using System.Linq;

    /*
    table Enum {
        name:string (required, key);
        values:[EnumVal] (required);  // In order of their values.
        is_union:bool = false;
        underlying_type:Type (required);
        attributes:[KeyValue];
        documentation:[string];
        /// File that this Enum is declared in.
        declaration_file: string;
    }
    */

    [FlatBufferTable]
    public class FlatBufferEnum
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual IIndexedVector<long, EnumVal> Values { get; set; } = new IndexedVector<long, EnumVal>();

        [FlatBufferItem(2)]
        public virtual bool IsUnion { get; set; }

        [FlatBufferItem(3, Required = true)]
        public virtual FlatBufferType UnderlyingType { get; set; } = new FlatBufferType();

        [FlatBufferItem(4)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(5)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(6)]
        public virtual string? DeclarationFile { get; set; }

        internal void WriteCode(CodeWriter writer, CompileContext context)
        {
            if (this.Attributes is not null)
            {
                if (this.Attributes.TryGetValue(MetadataKeys.BitFlags, out var kvp))
                {
                    writer.AppendLine("[Flags]");
                }
            }

            (string ns, string name) = Helpers.ParseName(this.Name);

            writer.AppendLine($"namespace {ns}");
            using (writer.WithBlock())
            {
                FlatSharpInternal.Assert(this.UnderlyingType.BaseType.TryGetBuiltInTypeName(out string? underlyingType), "Failed to get underlying type");

                writer.AppendLine($"[FlatBufferEnum(typeof({underlyingType}))]");
                writer.AppendLine($"public enum {name} : {underlyingType}");

                foreach (var item in this.Values.Select(x => x.Value).OrderBy(x => x.Value))
                {
                    writer.AppendLine($"{item.Key} = {item.Value},");
                }
            }
        }
    }
}
