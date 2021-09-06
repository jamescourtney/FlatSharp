namespace FlatSharp.Compiler.Schema
{
    using System;
    using FlatSharp.Attributes;

/*
/// New schema language features that are not supported by old code generators.
enum AdvancedFeatures : ulong (bit_flags) {
    AdvancedArrayFeatures,
    AdvancedUnionFeatures,
    OptionalScalars,
    DefaultVectorsAndStrings,
} 
*/
    [FlatBufferEnum(typeof(ulong))]
    [Flags]
    public enum AdvancedFeatures : ulong
    {
        None = 0,

        AdvancedArrayFeatures = 1,
        AdvancedUnionFeatures = 2,
        OptionalScalars = 4,
        DefaultVectorsAndStrings = 8,

        All = AdvancedArrayFeatures 
            | AdvancedUnionFeatures 
            | OptionalScalars 
            | DefaultVectorsAndStrings,
    }
}
