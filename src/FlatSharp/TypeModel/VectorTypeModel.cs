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
    /// Defines a vector type model.
    /// </summary>
    public class VectorTypeModel : RuntimeTypeModel
    {
        private RuntimeTypeModel memberTypeModel;
        private bool isList;
        private bool isMemory;
        private bool isArray;
        private bool isReadOnly;

        internal VectorTypeModel(Type vectorType) : base(vectorType)
        {
        }

        /// <summary>
        /// Gets the schema type of this element.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Vector;

        /// <summary>
        /// Gets the required alignment of this element.
        /// </summary>
        public override int Alignment => sizeof(uint);

        /// <summary>
        /// Gets the inline size of this element.
        /// </summary>
        public override int InlineSize => sizeof(uint);

        /// <summary>
        /// Gets the type model for this vector's elements.
        /// </summary>
        public RuntimeTypeModel ItemTypeModel => this.memberTypeModel;

        /// <summary>
        /// Indicates whether this vector's type is <see cref="System.Memory{T}"/> or <see cref="System.ReadOnlyMemory{T}"/>.
        /// </summary>
        public bool IsMemoryVector => this.isMemory;

        /// <summary>
        /// Indicates whether this vector's type is <see cref="IList{T}"/> or <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        public bool IsList => this.isList;

        /// <summary>
        /// Indicates whether this vector's type is an array. Note that arrays are deserialized greedily.
        /// </summary>
        public bool IsArray => this.isArray;

        /// <summary>
        /// Indicates if this vector's type is read-only.
        /// </summary>
        public bool IsReadOnly => this.isReadOnly;

        protected override void Initialize()
        {
            bool isValidType = false;
            Type innerType = null;
            if (this.ClrType.IsGenericType)
            {
                var genericType = this.ClrType.GetGenericTypeDefinition();
                if (genericType == typeof(Memory<>) || genericType == typeof(ReadOnlyMemory<>))
                {
                    this.isMemory = true;
                    this.isReadOnly = genericType == typeof(ReadOnlyMemory<>);
                    isValidType = true;
                    innerType = this.ClrType.GetGenericArguments()[0];
                }
                else if (genericType == typeof(IList<>) || genericType == typeof(IReadOnlyList<>))
                {
                    this.isList = true;
                    this.isReadOnly = genericType == typeof(IReadOnlyList<>);
                    isValidType = true;
                    innerType = this.ClrType.GetGenericArguments()[0];
                }
            }
            else if (this.ClrType.IsArray)
            {
                isValidType = true;
                this.isReadOnly = false;
                this.isArray = true;
                innerType = this.ClrType.GetElementType();
            }

            if (!isValidType)
            {
                throw new InvalidFlatBufferDefinitionException($"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, ReadOnlyMemory, and Arrays are supported.");
            }

            this.memberTypeModel = RuntimeTypeModel.CreateFrom(innerType);
            if (this.memberTypeModel.SchemaType == FlatBufferSchemaType.Vector)
            {
                throw new InvalidFlatBufferDefinitionException("Vectors may not contain other vectors.");
            }

            if (this.memberTypeModel.SchemaType == FlatBufferSchemaType.Union)
            {
                throw new InvalidFlatBufferDefinitionException("Vectors of unions are not supported.");
            }

            if (this.isMemory)
            {
                if (this.memberTypeModel.SchemaType != FlatBufferSchemaType.Scalar)
                {
                    throw new InvalidFlatBufferDefinitionException("Vectors may only be Memory<T> or ReadOnlyMemory<T> when the type is a scalar.");
                }

                // We can play some tricks on little-endian machines that allow us to store other types inside our vectors.
                // 1 byte types (bool, byte, sbyte) are always allowed. However, anything larger is subject to endianness issues.
                if (this.memberTypeModel.InlineSize != 1 && !BitConverter.IsLittleEndian)
                {
                    throw new InvalidFlatBufferDefinitionException("Memory<T> vectors may only use 1-byte sized elements on big-endian architectures. Consider using IList<T> instead.");
                }
            }
        }
    }
}
