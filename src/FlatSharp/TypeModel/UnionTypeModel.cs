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
    using System.Linq;

    /// <summary>
    /// Defines a vector type model.
    /// </summary>
    public class UnionTypeModel : RuntimeTypeModel
    {
        private RuntimeTypeModel[] memberTypeModels;

        internal UnionTypeModel(Type unionType) : base(unionType)
        {
            // Look for the actual FlatBufferUnion.
            while (unionType.BaseType != typeof(object))
            {
                unionType = unionType.BaseType;
            }

            this.memberTypeModels = unionType.GetGenericArguments().Select(RuntimeTypeModel.CreateFrom).ToArray();
        }

        /// <summary>
        /// Gets the schema type of this element.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Union;

        /// <summary>
        /// Gets the required alignment of this element.
        /// </summary>
        public override int Alignment => sizeof(uint);

        /// <summary>
        /// Gets the inline size of this element. 1 byte for discriminator and 4 for the uoffset.
        /// </summary>
        public override int InlineSize => sizeof(uint) + sizeof(byte);

        /// <summary>
        /// The discriminator byte is always aligned, since it's a byte. However, the offset may need to be aligned.
        /// </summary>
        public override int MaxInlineSize => this.InlineSize + SerializationHelpers.GetMaxPadding(sizeof(uint));

        /// <summary>
        /// Unions are not fixed because they contain tables.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Gets the type model for this union's members. Index 0 corresponds to discriminator 1.
        /// </summary>
        public RuntimeTypeModel[] UnionElementTypeModel => this.memberTypeModels;

        protected override void Initialize()
        {
            FlatBufferSchemaType[] validUnionMemberTypes = 
            {
                FlatBufferSchemaType.String,
                FlatBufferSchemaType.Table,
                FlatBufferSchemaType.Struct
            };

            bool containsString = false;
            HashSet<Type> uniqueTypes = new HashSet<Type>();

            foreach (var item in this.memberTypeModels)
            {
                if (!validUnionMemberTypes.Contains(item.SchemaType))
                {
                    throw new InvalidFlatBufferDefinitionException($"Unions may not store '{item.SchemaType}'.");
                }
                else if (!uniqueTypes.Add(item.ClrType))
                {
                    throw new InvalidFlatBufferDefinitionException($"Unions must consist of unique types. The type '{item.ClrType.Name}' was repeated.");
                }

                if (item.SchemaType == FlatBufferSchemaType.String)
                {
                    if (containsString)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Unions may only contain one string type. String and SharedString cannot cohabit the union.");
                    }

                    containsString = true;
                }
            }
        }
    }
}
