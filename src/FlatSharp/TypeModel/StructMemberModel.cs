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
    /// Describes a member of a FlatBuffer table or struct.
    /// </summary>
    public class StructMemberModel : ItemMemberModel
    {
        internal StructMemberModel(
            ITypeModel propertyModel,
            PropertyInfo propertyInfo,
            ushort index,
            int offset) : base(propertyModel, propertyInfo, index)
        {
            this.Offset = offset;
        }

        /// <summary>
        /// When the item is stored in a struct, this is defines the relative offset of this field within the struct.
        /// </summary>
        public int Offset { get; }
    }
}
