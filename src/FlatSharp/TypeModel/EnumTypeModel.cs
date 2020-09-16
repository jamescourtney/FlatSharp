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
    /// Defines an Enum FlatSharp type model, which derives from the scalar type model.
    /// </summary>
    public class EnumTypeModel : ScalarTypeModel
    {
        internal EnumTypeModel(Type type, int size) : base(type, size)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            var attribute = this.ClrType.GetCustomAttribute<FlatBufferEnumAttribute>();
            if (attribute.DeclaredUnderlyingType != Enum.GetUnderlyingType(this.ClrType))
            {
                throw new InvalidFlatBufferDefinitionException($"Enum '{this.ClrType}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{Enum.GetUnderlyingType(this.ClrType)}'");
            }
        }

        /// <summary>
        /// Enums are not built in, even though scalars are.
        /// </summary>
        public override bool IsBuiltInType => false;

        /// <summary>
        /// Enums can't be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Validates that the default value is of the same type as this enum.
        /// </summary>
        public override bool ValidateDefaultValue(object defaultValue)
        {
            return defaultValue.GetType() == this.ClrType;
        }
    }
}
