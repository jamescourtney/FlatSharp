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
    using System.Reflection;
    using FlatSharp.Attributes;

    /// <summary>
    /// Describes a member of a FlatBuffer table.
    /// </summary>
    public class TableMemberModel : ItemMemberModel
    {
        public TableMemberModel(
            ITypeModel propertyModel, 
            PropertyInfo propertyInfo, 
            FlatBufferItemAttribute attribute) : base(propertyModel, propertyInfo, attribute)
        {
            this.DefaultValue = attribute.DefaultValue;
            this.IsSortedVector = attribute.SortedVector;
            this.IsKey = attribute.Key;
            this.IsDeprecated = attribute.Deprecated;
            this.ForceWrite = attribute.ForceWrite;

            if (!propertyModel.IsValidTableMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} with type {propertyInfo.PropertyType.Name} cannot be part of a flatbuffer table.");
            }

            if (this.DefaultValue is not null && !propertyModel.ValidateDefaultValue(this.DefaultValue))
            {
                throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} declared default value of type {propertyModel.ClrType.Name}, but the value was of type {this.DefaultValue.GetType().GetCompilableTypeName()}. Please ensure that the property is allowed to have a default value and that the types match.");
            }

            if (this.IsWriteThrough)
            {
                throw new InvalidFlatBufferDefinitionException($"Table property {propertyInfo.Name} declared the WriteThrough attribute. WriteThrough is only supported on struct fields.");
            }
        }
        
        /// <summary>
        /// The default value of the table member.
        /// </summary>
        public object? DefaultValue { get; set; }

        /// <summary>
        /// Indicates if the member vector should be sorted before serializing.
        /// </summary>
        public bool IsSortedVector { get; set; }

        /// <summary>
        /// Indicates that this property is the key for the table.
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// Indicates that this property is deprecated and serializers need not be generated for it.
        /// </summary>
        public bool IsDeprecated { get; set; }

        /// <summary>
        /// Indicates that this field should always be written to a table, even
        /// if the default value matches.
        /// </summary>
        public bool ForceWrite { get; set; }

        /// <summary>
        /// Returns a C# literal that is equal to the default value.
        /// </summary>
        public string DefaultValueLiteral => this.ItemTypeModel.FormatDefaultValueAsLiteral(this.DefaultValue);

        public override string CreateReadItemBody(string parseItemMethodName, string bufferVariableName, string offsetVariableName, string vtableLocationVariableName, string vtableMaxIndexVariableName)
        {
            if (this.ItemTypeModel.PhysicalLayout.Length == 1)
            {
                return this.CreateSingleWidthReadItemBody(parseItemMethodName, bufferVariableName, offsetVariableName, vtableLocationVariableName, vtableMaxIndexVariableName);
            }
            else
            {
                return this.CreateWideReadItemBody(parseItemMethodName, bufferVariableName, offsetVariableName, vtableLocationVariableName, vtableMaxIndexVariableName);
            }
        }

        private string CreateSingleWidthReadItemBody(string parseItemMethodName, string bufferVariableName, string offsetVariableName, string vtableLocationVariableName, string vtableMaxIndexVariableName)
        {
            return $@"
                if ({this.Index} > {vtableMaxIndexVariableName})
                {{
                    return {this.DefaultValueLiteral};
                }}

                ushort relativeOffset = buffer.ReadUShort({vtableLocationVariableName} + {4 + (2 * this.Index)});
                if (relativeOffset == 0)
                {{
                    return {this.DefaultValueLiteral};
                }}

                int absoluteLocation = {offsetVariableName} + relativeOffset;
                return {parseItemMethodName}({bufferVariableName}, absoluteLocation);";
        }

        private string CreateWideReadItemBody(string parseItemMethodName, string bufferVariableName, string offsetVariableName, string vtableLocationVariableName, string vtableMaxIndexVariableName)
        {
            int items = this.ItemTypeModel.PhysicalLayout.Length;

            List<string> relativeOffsets = new();
            List<string> absoluteLocations = new();

            for (int i = 0; i < items; ++i)
            {
                int idx = this.Index + i;

                relativeOffsets.Add($@"
                ushort relativeOffset{i} = buffer.ReadUShort({vtableLocationVariableName} + {4 + (2 * idx)});
                if (relativeOffset{i} == 0)
                {{
                    return {this.DefaultValueLiteral};
                }}
                ");

                absoluteLocations.Add($"relativeOffset{i} + {offsetVariableName}");
            }

            return $@"
                if ({this.Index + items - 1} > {vtableMaxIndexVariableName})
                {{
                    return {this.DefaultValueLiteral};
                }}

                {string.Join("\r\n", relativeOffsets)}

                var absoluteLocations = ({string.Join(", ", absoluteLocations)});
                return {parseItemMethodName}({bufferVariableName}, ref absoluteLocations);";
        }

        public override string CreateWriteThroughBody(string writeValueMethodName, string bufferVariableName, string offsetVariableName, string valueVariableName)
        {
            throw new NotImplementedException();
        }
    }
}
