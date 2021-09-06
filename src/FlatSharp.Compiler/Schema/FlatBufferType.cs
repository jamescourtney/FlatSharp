namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;

    /*
    table Type
    {
        base_type:BaseType;
        element:BaseType = None;  // Only if base_type == Vector
                                  // or base_type == Array.
        index:int = -1;  // If base_type == Object, index into "objects" below.
                         // If base_type == Union, UnionType, or integral derived
                         // from an enum, index into "enums" below.
        fixed_length:uint16 = 0;  // Only if base_type == Array.
    }
    */

    [FlatBufferTable]
    public class FlatBufferType
    {
        [FlatBufferItem(0)]
        public virtual BaseType BaseType { get; set; }

        /// <summary>
        /// Only set if BaseType == Array or Vector.
        /// </summary>
        [FlatBufferItem(1, DefaultValue = BaseType.None)]
        public virtual BaseType ElementType { get; set; } = BaseType.None;

        [FlatBufferItem(2, DefaultValue = -1)]
        public virtual int Index { get; set; } = -1;

        [FlatBufferItem(3)]
        public virtual ushort FixedLength { get; set; }
    }
}
