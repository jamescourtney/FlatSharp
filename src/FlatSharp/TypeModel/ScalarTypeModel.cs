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

    /// <summary>
    /// Defines a scalar FlatSharp type model.
    /// </summary>
    public class ScalarTypeModel : RuntimeTypeModel
    {
        internal ScalarTypeModel(
            Type type,
            int size) : base(type)
        {
            this.Alignment = size;
            this.InlineSize = size;
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
        public override bool IsValidStructMember => true;

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
        public override bool IsValidVectorMember => true;

        /// <summary>
        /// Scalars can be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => true;

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            return defaultValue.GetType() == this.ClrType;
        }

        /// <summary>
        /// Indicates if we can natively read this scalar by referencing memory directly.
        /// </summary>
        /// <remarks>
        /// When size is 1 (ie, a byte), it can always be read. FlatBuffers stores data as little endian,
        /// so on compatible systems we have the ability to simply dereference an address and read the value
        /// without additional parsing.
        /// </remarks>
        public bool NativelyReadableFromMemory => this.InlineSize == 1 || BitConverter.IsLittleEndian;
    }
}
