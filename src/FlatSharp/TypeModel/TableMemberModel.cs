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
            ITypeModel propertyModel, 
            PropertyInfo propertyInfo, 
            ushort index, 
            bool hasDefaultValue,
            object defaultValue,
            bool isSortedVector,
            bool isKey) : base(propertyModel, propertyInfo, index)
        {
            this.HasDefaultValue = hasDefaultValue;
            this.DefaultValue = defaultValue;
            this.IsSortedVector = isSortedVector;
            this.IsKey = isKey;

            if (!propertyModel.IsValidTableMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} with type {propertyInfo.PropertyType.Name} cannot be part of a flatbuffer table.");
            }
            
            if (this.HasDefaultValue && !propertyModel.ValidateDefaultValue(this.DefaultValue))
            {
                throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} declared default value of type {propertyModel.ClrType.Name}, but the value was of type {defaultValue?.GetType()?.Name}. Please ensure that the property is allowed to have a default value and that the types match.");
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
        /// Indicates if the member vector should be sorted before serializing.
        /// </summary>
        public bool IsSortedVector { get; }

        /// <summary>
        /// Indicates that this property is the key for the table.
        /// </summary>
        public bool IsKey { get; }

        public string DefaultValueToken
        {
            get
            {
                string defaultValue = $"default({CSharpHelpers.GetCompilableTypeName(this.ItemTypeModel.ClrType)})";
                if (this.HasDefaultValue)
                {
                    if (!this.ItemTypeModel.TryFormatDefaultValueAsLiteral(this.DefaultValue, out defaultValue))
                    {
                        throw new InvalidFlatBufferDefinitionException($"Unable to format {this.DefaultValue} (type {this.DefaultValue.GetType().Name}) as a literal.");
                    }
                }

                return defaultValue;
            }
        }
    }
}
