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
    public class ItemMemberModel
    {
        internal ItemMemberModel(
            ITypeModel propertyModel,
            PropertyInfo propertyInfo,
            ushort index)
        {
            var getMethod = propertyInfo.GetMethod;
            var setMethod = propertyInfo.SetMethod;

            this.ItemTypeModel = propertyModel;
            this.PropertyInfo = propertyInfo;
            this.Index = index;

            if (getMethod == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Property {propertyInfo.Name} did not declare a getter.");
            }

            if (!getMethod.IsPublic)
            {
                throw new InvalidFlatBufferDefinitionException($"Property {propertyInfo.Name} must declare a public getter.");
            }

            if (getMethod.IsVirtual)
            {
                this.IsVirtual = true;
                if (!ValidateVirtualPropertyMethod(getMethod, false))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property {propertyInfo.Name} did not declare a public/protected virtual getter.");
                }

                if (!ValidateVirtualPropertyMethod(setMethod, true))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property {propertyInfo.Name} declared a set method, but it was not public/protected and virtual.");
                }
            }
            else
            {
                if (!ValidateNonVirtualMethod(getMethod))
                {
                    throw new InvalidFlatBufferDefinitionException($"Non-virtual property {propertyInfo.Name} must declare a public and non-abstract getter.");
                }

                if (!ValidateNonVirtualMethod(setMethod))
                {
                    throw new InvalidFlatBufferDefinitionException($"Non-virtual property {propertyInfo.Name} must declare a public/protected and non-abstract setter.");
                }
            }
        }

        private static bool ValidateNonVirtualMethod(MethodInfo method)
        {
            if (method == null ||
                method.IsAbstract ||
                method.IsVirtual)
            {
                return false;
            }

            return method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly;
        }

        private static bool ValidateVirtualPropertyMethod(MethodInfo method, bool allowNull)
        {
            if (method == null)
            {
                return allowNull;
            }

            if (!method.IsVirtual || method.IsFinal)
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
    }
}
