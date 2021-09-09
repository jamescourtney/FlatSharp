/*
 * Copyright 2021 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [FlatBufferEnum(typeof(byte))]
    public enum BaseType : byte
    {
        None,
        UType,
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
                case BaseType.Float:
                case BaseType.Double:
                    return true;
            }

            return type.IsInteger();
        }

        public static bool IsInteger(this BaseType type)
        {
            switch (type)
            {
                case BaseType.Byte:
                case BaseType.UByte:
                case BaseType.Short:
                case BaseType.UShort:
                case BaseType.Int:
                case BaseType.UInt:
                case BaseType.Long:
                case BaseType.ULong:
                    return true;
            }

            return false;
        }

        public static int GetScalarSize(this BaseType type)
        {
            FlatSharpInternal.Assert(type.IsScalar(), "Type " + type + " was not a scalar");

            switch (type)
            {
                case BaseType.Bool:
                case BaseType.Byte:
                case BaseType.UByte:
                    return 1;

                case BaseType.Short:
                case BaseType.UShort:
                    return 2;

                case BaseType.Int:
                case BaseType.UInt:
                case BaseType.Float:
                    return 4;

                case BaseType.Long:
                case BaseType.ULong:
                case BaseType.Double:
                    return 8;

                default:
                    throw new InvalidOperationException("impossible");
            }
        }
    }
}
