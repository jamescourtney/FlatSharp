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
    /// Describes a member of a FlatBuffer table or struct.
    /// </summary>
    public abstract class ItemMemberModel
    {
        protected ItemMemberModel(
            ITypeModel propertyModel,
            PropertyInfo propertyInfo,
            FlatBufferItemAttribute attribute)
        {
            var getMethod = propertyInfo.GetMethod;
            var setMethod = propertyInfo.SetMethod;

            this.ItemTypeModel = propertyModel;
            this.PropertyInfo = propertyInfo;
            this.Index = attribute.Index;
            this.CustomGetter = attribute.CustomGetter;
            this.IsWriteThrough = attribute.WriteThrough;

            string declaringType = "";
            if (propertyInfo.DeclaringType is not null)
            {
                declaringType = CSharpHelpers.GetCompilableTypeName(propertyInfo.DeclaringType);
            }

            declaringType = $"{declaringType}.{propertyInfo.Name} (Index {attribute.Index})";

            if (getMethod == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Property {declaringType} on did not declare a getter.");
            }

            if (!getMethod.IsPublic && string.IsNullOrEmpty(this.CustomGetter))
            {
                throw new InvalidFlatBufferDefinitionException($"Property {declaringType} must declare a public getter.");
            }

            if (CanBeOverridden(getMethod))
            {
                this.IsVirtual = true;
                if (!ValidateVirtualPropertyMethod(getMethod, false))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property {declaringType} did not declare a public/protected virtual getter.");
                }

                if (!ValidateVirtualPropertyMethod(setMethod, true))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property {declaringType} declared a set method, but it was not public/protected and virtual.");
                }
            }
            else
            {
                if (!ValidateNonVirtualMethod(getMethod))
                {
                    throw new InvalidFlatBufferDefinitionException($"Non-virtual property {declaringType} must declare a public and non-abstract getter.");
                }

                if (!ValidateNonVirtualMethod(setMethod))
                {
                    throw new InvalidFlatBufferDefinitionException($"Non-virtual property {declaringType} must declare a public/protected and non-abstract setter.");
                }
            }
        }

        private static bool CanBeOverridden(MethodInfo method)
        {
            // Note: !IsFinal is different than IsVirtual.
            // The difference is that interface implementations cause the "virtual" flag to be set,
            // so a method implementing an interface that is not virtual is both final and virtual.

            // Truth table:
            //                                     | IsVirtual | IsFinal  |  Overridable?
            // ------------------------------------|-----------|----------|--------------
            // NonVirtual Interface implementation |   True    |   True   |   False
            //    Virtual Interface Implementation |   True    |   False  |   True
            //    NonVirtual Method (no interface) |   False   |   False  |   False
            //       Virtual Method (no interface) |   True    |   False  |   True
            // NB: No confirmed examples of Final = True, Virtual = False.
            // Relationship appears to be: Overriddable = Virtual && !Final
            // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isvirtual?view=net-5.0
            return method.IsVirtual && !method.IsFinal;
        }

        private static bool ValidateNonVirtualMethod(MethodInfo? method)
        {
            if (method is null
                || method.IsAbstract
                || CanBeOverridden(method))
            {
                return false;
            }

            return method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly;
        }

        private static bool ValidateVirtualPropertyMethod(MethodInfo? method, bool allowNull)
        {
            if (method is null)
            {
                return allowNull;
            }

            if (!CanBeOverridden(method))
            {
                return false;
            }

            return method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily;
        }

        /// <summary>
        /// The index of the table member.
        /// </summary>
        public ushort Index { get; }

        /// <summary>
        /// The property info of the table member.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// The type model of the item.
        /// </summary>
        public ITypeModel ItemTypeModel { get; }

        /// <summary>
        /// The property is virtual (ie, FlatSharp will override it when generating code).
        /// </summary>
        public bool IsVirtual { get; }

        /// <summary>
        /// Indicates if this member writes through to the underlying buffer.
        /// </summary>
        public bool IsWriteThrough { get; }

        /// <summary>
        /// A custom getter for this item.
        /// </summary>
        public string? CustomGetter { get; set; }

        /// <summary>
        /// Creates a method body to read the given property. This is contextual depending
        /// on whether this member is table/struct/etc.
        /// </summary>
        /// <param name="parseItemMethodName">A method that can parse the type from a buffer and offset.</param>
        /// <param name="bufferVariableName">The buffer variable name.</param>
        /// <param name="offsetVariableName">The offset at which the container (table/struct) starts.</param>
        /// <param name="vtableLocationVariableName">For tables, the offset of the vtable.</param>
        /// <param name="vtableMaxIndexVariableName">For tables, the max index of the vtable.</param>
        public abstract string CreateReadItemBody(
            string parseItemMethodName,
            string bufferVariableName,
            string offsetVariableName,
            string vtableLocationVariableName,
            string vtableMaxIndexVariableName);

        /// <summary>
        /// Creates a method body to write the given property back to the buffer. This is contextual depending
        /// on whether this member is table/struct/etc.
        /// </summary>
        /// <param name="writeValueMethodName">The name of the method that does the write operation.</param>
        /// <param name="bufferVariableName">The input buffer.</param>
        /// <param name="offsetVariableName">The offset of the container.</param>
        /// <param name="valueVariableName">The variable name containing the value.</param>
        public abstract string CreateWriteThroughBody(
            string writeValueMethodName,
            string bufferVariableName,
            string offsetVariableName,
            string valueVariableName);
    }
}
