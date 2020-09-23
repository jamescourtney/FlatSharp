/*
 * Copyright 2018 James Courtney
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
 
 namespace FlatSharp.TypeModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a scalar FlatSharp type model.
    /// </summary>
    public class ScalarTypeModel : RuntimeTypeModel, ITypeModel
    {
        protected readonly bool isNullable;

        internal ScalarTypeModel(
            Type type,
            int size) : base(type)
        {
            this.Alignment = size;
            this.InlineSize = size;
            this.isNullable = Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// The schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Scalar;

        /// <summary>
        /// The alignment of this scalar. Will be equal to the size.
        /// </summary>
        public override int Alignment { get; }

        /// <summary>
        /// The size of this scalar. Equal to the alignment.
        /// </summary>
        public override int InlineSize { get; }

        /// <summary>
        /// Scalars are fixed size.
        /// </summary>
        public override bool IsFixedSize => true;

        /// <summary>
        /// Scalars are built-into FlatSharp.
        /// </summary>
        public override bool IsBuiltInType => true;

        /// <summary>
        /// Scalars can be part of Structs.
        /// </summary>
        public override bool IsValidStructMember => !this.isNullable;

        /// <summary>
        /// Scalars can be part of Tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Scalars can't be part of Unions.
        /// </summary>
        public override bool IsValidUnionMember => false;

        /// <summary>
        /// Scalars can be part of Vectors.
        /// </summary>
        public override bool IsValidVectorMember => !this.isNullable;

        /// <summary>
        /// Scalars can be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => !this.isNullable;

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            if (this.isNullable)
            {
                return false;
            }

            return defaultValue.GetType() == this.ClrType;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                IsMethodInline = true,
                MethodBody = $"return {this.MaxInlineSize};"
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            string methodName = null;

            if (this.ClrType == typeof(bool) || this.ClrType == typeof(bool?))
            {
                methodName = nameof(InputBuffer.ReadBool);
            }
            else if (this.ClrType == typeof(byte) || this.ClrType == typeof(byte?))
            {
                methodName = nameof(InputBuffer.ReadByte);
            }
            else if (this.ClrType == typeof(sbyte) || this.ClrType == typeof(sbyte?))
            {
                methodName = nameof(InputBuffer.ReadSByte);
            }
            else if (this.ClrType == typeof(short) || this.ClrType == typeof(short?))
            {
                methodName = nameof(InputBuffer.ReadShort);
            }
            else if (this.ClrType == typeof(ushort) || this.ClrType == typeof(ushort?))
            {
                methodName = nameof(InputBuffer.ReadUShort);
            }
            else if (this.ClrType == typeof(int) || this.ClrType == typeof(int?))
            {
                methodName = nameof(InputBuffer.ReadInt);
            }
            else if (this.ClrType == typeof(uint) || this.ClrType == typeof(uint?))
            {
                methodName = nameof(InputBuffer.ReadUInt);
            }
            else if (this.ClrType == typeof(long) || this.ClrType == typeof(long?))
            {
                methodName = nameof(InputBuffer.ReadLong);
            }
            else if (this.ClrType == typeof(ulong) || this.ClrType == typeof(ulong?))
            {
                methodName = nameof(InputBuffer.ReadULong);
            }
            else if (this.ClrType == typeof(float) || this.ClrType == typeof(float?))
            {
                methodName = nameof(InputBuffer.ReadFloat);
            }
            else if (this.ClrType == typeof(double) || this.ClrType == typeof(double?))
            {
                methodName = nameof(InputBuffer.ReadDouble);
            }

            return new CodeGeneratedMethod
            {
                IsMethodInline = true,
                MethodBody = $"return {context.InputBufferVariableName}.{methodName}({context.OffsetVariableName});"
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            string methodName = null;

            if (this.ClrType == typeof(bool) || this.ClrType == typeof(bool?))
            {
                methodName = nameof(SpanWriter.WriteBool);
            }
            else if (this.ClrType == typeof(byte) || this.ClrType == typeof(byte?))
            {
                methodName = nameof(SpanWriter.WriteByte);
            }
            else if (this.ClrType == typeof(sbyte) || this.ClrType == typeof(sbyte?))
            {
                methodName = nameof(SpanWriter.WriteSByte);
            }
            else if (this.ClrType == typeof(short) || this.ClrType == typeof(short?))
            {
                methodName = nameof(SpanWriter.WriteShort);
            }
            else if (this.ClrType == typeof(ushort) || this.ClrType == typeof(ushort?))
            {
                methodName = nameof(SpanWriter.WriteUShort);
            }
            else if (this.ClrType == typeof(int) || this.ClrType == typeof(int?))
            {
                methodName = nameof(SpanWriter.WriteInt);
            }
            else if (this.ClrType == typeof(uint) || this.ClrType == typeof(uint?))
            {
                methodName = nameof(SpanWriter.WriteUInt);
            }
            else if (this.ClrType == typeof(long) || this.ClrType == typeof(long?))
            {
                methodName = nameof(SpanWriter.WriteLong);
            }
            else if (this.ClrType == typeof(ulong) || this.ClrType == typeof(ulong?))
            {
                methodName = nameof(SpanWriter.WriteULong);
            }
            else if (this.ClrType == typeof(float) || this.ClrType == typeof(float?))
            {
                methodName = nameof(SpanWriter.WriteFloat);
            }
            else if (this.ClrType == typeof(double) || this.ClrType == typeof(double?))
            {
                methodName = nameof(SpanWriter.WriteDouble);
            }

            string variableName = context.ValueVariableName;
            if (this.isNullable)
            {
                variableName += ".Value";
            }

            return new CodeGeneratedMethod 
            {
                MethodBody = $"{context.SpanWriterVariableName}.{methodName}({context.SpanVariableName}, {variableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});",
                IsMethodInline = true,
            };
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            if (this.isNullable)
            {
                return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
            }
            else
            {
                return string.Empty;
            }
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            if (this.isNullable)
            {
                return $"{itemVariableName} != null";
            }
            else
            {
                return "true";
            }
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            if (this.isNullable)
            {
                seenTypes.Add(Nullable.GetUnderlyingType(this.ClrType));
            }
        }
    }
}
