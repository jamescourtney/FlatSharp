/*
 * Copyright 2020 James Courtney
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
 
namespace FlatSharp
{
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;

    /// <summary>
    /// Describes a type built into flatbuffers.
    /// </summary>
    internal interface IBuiltInType
    {
        /// <summary>
        /// The CLR type.
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// The type model.
        /// </summary>
        RuntimeTypeModel TypeModel { get; }

        /// <summary>
        /// The SpanWriter method to invoke to write an instance of the type.
        /// </summary>
        MethodInfo SpanWriterWrite { get; }

        /// <summary>
        /// An inputbuffer method to invoke to read an instance of the type.
        /// </summary>
        MethodInfo InputBufferRead { get; }

        /// <summary>
        /// A set of FBS aliases for this type.
        /// </summary>
        HashSet<string> FbsAliases { get; }

        /// <summary>
        /// The common name for the type in C#.
        /// </summary>
        string CSharpTypeName { get; }

        /// <summary>
        /// The ISpanComparer type to use for this built-in type.
        /// </summary>
        Type SpanComparerType { get; }
    }

    /// <summary>
    /// Describes a built in scalar type. Scalars need to be able to parse and format.
    /// </summary>
    internal interface IBuiltInScalarType : IBuiltInType
    {
        /// <summary>
        /// Formats the given string literal.
        /// </summary>
        string FormatLiteral(string literal);

        /// <summary>
        /// Formats the given value as a string literal.
        /// </summary>
        string FormatObject(object value);
    }

    /// <summary>
    /// Defines a built in type, including how to format, type model, read method, and write method.
    /// </summary>
    internal class BuiltInType : IBuiltInType
    {
        internal static readonly IReadOnlyDictionary<Type, IBuiltInScalarType> BuiltInScalars = new Dictionary<Type, IBuiltInScalarType>
        {
            [typeof(bool)] = new ScalarBuiltInType<bool>(
                new ScalarTypeModel(typeof(bool), sizeof(bool)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteBool)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadBool)),
                typeof(BoolSpanComparer),
                new[] { "bool" },
                "bool",
                b => b.ToString().ToLowerInvariant()),

            [typeof(byte)] = new ScalarBuiltInType<byte>(
                new ScalarTypeModel(typeof(byte), sizeof(byte)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteByte)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadByte)),
                typeof(ByteSpanComparer),
                new[] { "ubyte", "uint8" },
                "byte"),

            [typeof(sbyte)] = new ScalarBuiltInType<sbyte>(
                new ScalarTypeModel(typeof(sbyte), sizeof(sbyte)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteSByte)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadSByte)),
                typeof(SByteSpanComparer),
                new[] { "byte", "int8" },
                "sbyte"),

            [typeof(ushort)] = new ScalarBuiltInType<ushort>(
                new ScalarTypeModel(typeof(ushort), sizeof(ushort)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteUShort)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadUShort)),
                typeof(UShortSpanComparer),
                new[] { "uint16", "ushort" },
                "ushort"),

            [typeof(short)] = new ScalarBuiltInType<short>(
                new ScalarTypeModel(typeof(short), sizeof(short)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteShort)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadShort)),
                typeof(ShortSpanComparer),
                new[] { "int16", "short" },
                "short"),

            [typeof(uint)] = new ScalarBuiltInType<uint>(
                new ScalarTypeModel(typeof(uint), sizeof(uint)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteUInt)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadUInt)),
                typeof(UIntSpanComparer),
                new[] { "uint", "uint32" },
                "uint"),

            [typeof(int)] = new ScalarBuiltInType<int>(
                new ScalarTypeModel(typeof(int), sizeof(int)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteInt)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadInt)),
                typeof(IntSpanComparer),
                new[] { "int", "int32" },
                "int"),

            [typeof(ulong)] = new ScalarBuiltInType<ulong>(
                new ScalarTypeModel(typeof(ulong), sizeof(ulong)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteULong)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadULong)),
                typeof(ULongSpanComparer),
                new[] { "ulong", "uint64" },
                "ulong"),

            [typeof(long)] = new ScalarBuiltInType<long>(
                new ScalarTypeModel(typeof(long), sizeof(long)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteLong)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadLong)),
                typeof(LongSpanComparer),
                new[] { "long", "int64" },
                "long"),

            [typeof(float)] = new ScalarBuiltInType<float>(
                new ScalarTypeModel(typeof(float), sizeof(float)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteFloat)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadFloat)),
                typeof(FloatSpanComparer),
                new[] { "float", "float32" },
                "float",
                f => f.ToString("G17")),

            [typeof(double)] = new ScalarBuiltInType<double>(
                new ScalarTypeModel(typeof(double), sizeof(double)),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteDouble)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadDouble)),
                typeof(DoubleSpanComparer),
                new[] { "double", "float64" },
                "double",
                d => d.ToString("G17")),
        };

        public static readonly IReadOnlyDictionary<Type, IBuiltInType> BuiltInTypes = new Dictionary<Type, IBuiltInType>(BuiltInScalars.ToDictionary(x => x.Key, x => (IBuiltInType)x.Value))
        {
            [typeof(string)] = new BuiltInType(
                new StringTypeModel(),
                ReflectedMethods.GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteString)),
                ReflectedMethods.GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadString)),
                typeof(StringSpanComparer),
                new[] { "string" },
                "string"),
        };

        private BuiltInType(
            RuntimeTypeModel runtimeModel,
            MethodInfo spanWriterWrite,
            MethodInfo inputBufferRead,
            Type spanComparerType,
            string[] fbsAliases,
            string cSharpTypeName)
        {
            this.ClrType = runtimeModel.ClrType;
            this.TypeModel = runtimeModel;
            this.SpanWriterWrite = spanWriterWrite;
            this.InputBufferRead = inputBufferRead;
            this.FbsAliases = new HashSet<string>(fbsAliases);
            this.CSharpTypeName = cSharpTypeName;
            this.SpanComparerType = spanComparerType;
        }

        public Type ClrType { get; }

        public RuntimeTypeModel TypeModel { get; }

        public MethodInfo SpanWriterWrite { get; }

        public MethodInfo InputBufferRead { get; }

        public HashSet<string> FbsAliases { get; }

        public string CSharpTypeName { get; }

        public Type SpanComparerType { get; }

        private static bool TryParseBool(string value, NumberStyles numberStyle, IFormatProvider formatProvider, out bool result)
        {
            if (value == "false")
            {
                result = false;
                return true;
            }
            else if (value == "true")
            {
                result = true;
                return true;
            }

            result = false;
            return false;
        }

        private class ScalarBuiltInType<T> : BuiltInType, IBuiltInScalarType
        {
            private readonly Func<T, string> format;

            public ScalarBuiltInType(
                RuntimeTypeModel runtimeModel, 
                MethodInfo spanWriterWrite, 
                MethodInfo inputBufferRead, 
                Type spanComparerType,
                string[] fbsAliases, 
                string cSharpTypeName,
                Func<T, string> format = null) 
                    : base(runtimeModel, spanWriterWrite, inputBufferRead, spanComparerType, fbsAliases, cSharpTypeName)
            {
                this.format = format;
            }

            public string FormatLiteral(string literal)
            {
                return $"({this.CSharpTypeName}){literal}";
            }

            public string FormatObject(object value)
            {
                T typedValue = (T)value;
                return this.FormatLiteral(this.format?.Invoke(typedValue) ?? typedValue.ToString());
            }
        }
    }
}
