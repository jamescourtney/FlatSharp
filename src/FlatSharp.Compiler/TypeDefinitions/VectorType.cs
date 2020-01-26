namespace FlatSharp.Compiler
{
    internal enum VectorType
    {
        /// <summary>
        /// Not a vector.
        /// </summary>
        None,

        IList,
        IReadOnlyList,
        Array,
        Memory,
        ReadOnlyMemory,
    }
}
