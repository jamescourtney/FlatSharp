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

namespace FlatSharp.Attributes
{
    using System;

    /// <summary>
    /// Marks an enum as being elible for FlatSharp serialization. 
    /// </summary>
    /// <remarks>
    /// Usage of enums comes with two main caveats: the enum may only be extended (ie, values cannot be recycled), and 
    /// the underlying type may not be changed.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public class FlatBufferEnumAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the FlatBufferEnumAttribute class.
        /// </summary>
        /// <param name="underlyingType">The declared underlying type of the enum. This must match the enum's actual underlying type.</param>
        public FlatBufferEnumAttribute(Type underlyingType)
        {
            this.DeclaredUnderlyingType = underlyingType;
        }

        /// <summary>
        /// The declared underlying type. This is a saftey check to prevent unintended binary breaks.
        /// </summary>
        public Type DeclaredUnderlyingType { get; }
    }
}
