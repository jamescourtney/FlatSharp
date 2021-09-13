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
    using FlatSharp.Attributes;
    using System;
    using System.Reflection;

    /// <summary>
    /// Describes a member of a FlatBuffer struct.
    /// </summary>
    public class StructMemberModel : ItemMemberModel
    {
        public StructMemberModel(
            ITypeModel propertyModel,
            PropertyInfo propertyInfo,
            FlatBufferItemAttribute attribute,
            int offset,
            int length) : base(propertyModel, propertyInfo, attribute)
        {
            this.Offset = offset;
            this.Length = length;

            if (!this.IsVirtual && this.IsWriteThrough)
            {
                throw new InvalidFlatBufferDefinitionException($"Struct member '{this.FriendlyName}' declared the WriteThrough attribute, but WriteThrough is only supported on virtual fields.");
            }

            if (propertyModel.SerializeMethodRequiresContext)
            {
                throw new InvalidFlatBufferDefinitionException($"The type model for struct member '{this.FriendlyName}' requires a serialization context, but Structs do not have one.");
            }

            if (attribute.Required)
            {
                throw new InvalidFlatBufferDefinitionException($"Struct member '{this.FriendlyName}' declared the Required attribute. Required is not valid inside structs.");
            }

            if (attribute.SharedString)
            {
                throw new InvalidFlatBufferDefinitionException($"Struct member '{this.FriendlyName}' declared the SharedString attribute. SharedString is not valid inside structs.");
            }
        }

        /// <summary>
        /// When the item is stored in a struct, this is defines the relative offset of this field within the struct.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// The length of the item when stored in a struct. Does not include padding.
        /// </summary>
        public int Length { get; }

        public override string CreateReadItemBody(
            ParserCodeGenContext context,
            string vtableLocationVariableName,
            string vtableMaxIndexVariableName)
        {
            context = context with
            {
                OffsetVariableName = $"{context.OffsetVariableName} + {this.Offset}",
            };

            return $"return {context.GetParseInvocation(this.ItemTypeModel.ClrType)};";
        }

        public override string CreateWriteThroughBody(
            SerializationCodeGenContext context,
            string vtableLocationVariableName,
            string vtableMaxIndexVariableName)
        {
            context = context with
            {
                OffsetVariableName = $"{context.OffsetVariableName} + {this.Offset}"
            };

            return context.GetSerializeInvocation(this.ItemTypeModel.ClrType) + ";";
        }
    }
}
