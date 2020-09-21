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
    /// Defines an optional (nullable) Enum FlatSharp type model, which derives from the scalar type model.
    /// </summary>
    public class NullableEnumTypeModel : ScalarTypeModel
    {
        internal NullableEnumTypeModel(Type type, int size) : base(type, size)
        {
        }

        public Type EnumType { get; private set; }

        public Type UnderlyingType { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            this.EnumType = Nullable.GetUnderlyingType(this.ClrType);
            var attribute = this.EnumType.GetCustomAttribute<FlatBufferEnumAttribute>();
            this.UnderlyingType = Enum.GetUnderlyingType(this.EnumType);

            if (attribute.DeclaredUnderlyingType != this.UnderlyingType)
            {
                throw new InvalidFlatBufferDefinitionException($"Enum '{this.EnumType.Name}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{this.UnderlyingType}'");
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
    }
}
