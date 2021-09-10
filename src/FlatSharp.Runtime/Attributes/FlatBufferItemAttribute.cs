﻿/*
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
    using System.ComponentModel;

    /// <summary>
    /// Defines a member of a FlatBuffer struct or table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FlatBufferItemAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new FlatBufferItemAttribute.
        /// </summary>
        /// <param name="index">The field index within the struct or table.</param>
        public FlatBufferItemAttribute(ushort index)
        {
            this.Index = index;
        }

        /// <summary>
        /// The index within the struct or table.
        /// </summary>
        public ushort Index { get; }

        /// <summary>
        /// For tables, indicates if this field is deprecated. Deprecated fields are not written or read.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Indicates that the given vector property is to be serialized sorted by the key property of the vector type.
        /// </summary>
        public bool SortedVector { get; set; }

        /// <summary>
        /// Indicates that this field is the key for the table. Keys are used when sorting inside a vector. Only valid on a table.
        /// </summary>
        public bool Key { get; set; }

        /// <summary>
        /// For table items, gets or sets the default value. The type of the object must match the
        /// type of the property.
        /// </summary>
        public object? DefaultValue { get; set; }

        /// <summary>
        /// When set, indicates that this item should always be written to tables,
        /// even if it matches the default value. An exception is raised if this
        /// is set in a context where Force-Writing is not allowed.
        /// </summary>
        public bool ForceWrite { get; set; }

        /// <summary>
        /// When set, indicates that this property is write-through to the underlying vector.
        /// Only supported on scalars, structs, and enums in addition to vectors of those types.
        /// </summary>
        public bool WriteThrough { get; set; }

        /// <summary>
        /// When set, indicates that the field is required. Only applies to non-scalar fields on tables.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Indicates if strings within this vector or item should be shared.
        /// </summary>
        public bool SharedString { get; set; }
    }
}
