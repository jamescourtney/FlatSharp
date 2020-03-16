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
    using System.Reflection;

    /// <summary>
    /// Describes a member of a FlatBuffer table.
    /// </summary>
    public class TableMemberModel : ItemMemberModel
    {
        internal TableMemberModel(
            RuntimeTypeModel propertyModel, 
            PropertyInfo propertyInfo, 
            ushort index, 
            bool hasDefaultValue,
            object defaultValue,
            bool isKey,
            bool isSortedVector) : base(propertyModel, propertyInfo, index)
        {
            this.HasDefaultValue = hasDefaultValue;
            this.DefaultValue = defaultValue;
            this.IsKey = isKey;
            this.IsSortedVector = isSortedVector;
            
            if (this.HasDefaultValue)
            {
                if (propertyModel is ScalarTypeModel)
                {
                    if (defaultValue?.GetType() != propertyModel.ClrType)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} declared default value of type {propertyModel.ClrType.Name}, but the value was of type {defaultValue?.GetType()?.Name}.");
                    }
                }
                else
                {
                    throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} declared default value, but this type is not allowed to have one.");
                }
            }
        }
        
        /// <summary>
        /// The default value of the table member.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Indicates if this member type has a default value at all. Only valid for tables.
        /// </summary>
        public bool HasDefaultValue { get; }

        /// <summary>
        /// Indicates if this member type is a key for the table.
        /// </summary>
        public bool IsKey { get; }

        /// <summary>
        /// Indicates if the member vector should be sorted before serializing.
        /// </summary>
        public bool IsSortedVector { get; }

        /// <summary>
        /// Indicates how "wide" this element is in the table's vtable. Unions consume 2 slots in the vtable.
        /// </summary>
        public int VTableSlotCount => this.ItemTypeModel.SchemaType == FlatBufferSchemaType.Union ? 2 : 1;
    }
}
