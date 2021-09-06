namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Diagnostics.CodeAnalysis;

    [FlatBufferEnum(typeof(byte))]
    public enum BaseType : byte
    {
        None,
        UnionDiscriminator,
        Bool,
        Byte,
        UByte,
        Short,
        UShort,
        Int,
        UInt,
        Long,
        ULong,
        Float,
        Double,
        String,
        Vector,
        Obj,     // Used for tables & structs.
        Union,
        Array,

        // Add any new type above this value.
        MaxBaseType
    }

    public static class BaseTypeExtensions
    {
        public static bool IsKnown(this BaseType type)
        {
            return type > BaseType.None && type < BaseType.MaxBaseType;
        }

        public static bool IsBuiltIn(this BaseType type)
        {
            return type.IsScalar() || type == BaseType.String;
        }

        public static bool TryGetBuiltInTypeName(this BaseType type, [NotNullWhen(true)] out string? typeName)
        {
            switch (type)
            {
                case BaseType.Bool:
                    typeName = "bool";
                    return true;

                case BaseType.Byte:
                    typeName = "sbyte";
                    return true;

                case BaseType.UByte:
                    typeName = "byte";
                    return true;

                case BaseType.Short:
                    typeName = "short";
                    return true;

                case BaseType.UShort:
                    typeName = "ushort";
                    return true;

                case BaseType.Int:
                    typeName = "int";
                    return true;

                case BaseType.UInt:
                    typeName = "uint";
                    return true;

                case BaseType.Long:
                    typeName = "long";
                    return true;

                case BaseType.ULong:
                    typeName = "ulong";
                    return true;

                case BaseType.Float:
                    typeName = "float";
                    return true;
                    
                case BaseType.Double:
                    typeName = "double";
                    return true;

                case BaseType.String:
                    typeName = "string";
                    return true;
            }

            typeName = null;
            return false;
        }

        public static bool IsScalar(this BaseType type)
        {
            switch (type)
            {
                case BaseType.Bool:
                case BaseType.Byte:
                case BaseType.UByte:
                case BaseType.Short:
                case BaseType.UShort:
                case BaseType.Int:
                case BaseType.UInt:
                case BaseType.Long:
                case BaseType.ULong:
                case BaseType.Float:
                case BaseType.Double:
                    return true;
            }

            return false;
        }
    }
}
