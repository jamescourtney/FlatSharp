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
    /// Indicates how FlatSharp should use MemoryMarshal.Cast when interacting with this struct. When Memory Marshalling is enabled, FlatSharp will
    /// opportunistically serialize and deserialize a value-type struct with a call to <see cref="System.Runtime.InteropServices.MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>.
    /// </summary>
    [Flags]
    public enum MemoryMarshalBehavior
    {
        /// <summary>
        /// Never. Always use field by field copies.
        /// </summary>
        Never = 0,

        /// <summary>
        /// Flatsharp will choose when to enable memory marshalling. This can change from release to release.
        /// </summary>
        Default = 1,

        /// <summary>
        /// Memory marshalling is enabled when serializing only.
        /// </summary>
        Serialize = 2,

        /// <summary>
        /// Memory marshalling is enabled when parsing only.
        /// </summary>
        Parse = 3,

        /// <summary>
        /// Memory marshalling is enabled for both serializing and parsing.
        /// </summary>
        Always = 4,
    }

    /// <summary>
    /// Marks a class as being a FlatBuffer struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class FlatBufferStructAttribute : Attribute
    {
        /// <summary>
        /// Enables reading and writing value structs with calls to MemoryMarshal.Cast.
        /// </summary>
        public MemoryMarshalBehavior MemoryMarshalBehavior { get; set; } = MemoryMarshalBehavior.Default;
    }
}
