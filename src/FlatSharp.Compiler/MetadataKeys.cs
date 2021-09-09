/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Names of attributes specified in FBS files.
    /// </summary>
    public static class MetadataKeys
    {
        /// <summary>
        /// Controls the type of serializer generated for the given table.
        /// Valid On:
        /// - Table
        /// </summary>
        public const string SerializerKind = "fs_serializer";

        /// <summary>
        /// Controls whether fields are virtual or non-virtual. They are virtual by default.
        /// Valid On:
        /// - Table (as a default for all fields in the table, overridden by the setting on a field)
        /// - Struct (as a default for all fields in the struct, overridden by the setting on a field)
        /// - Field 
        /// </summary>
        public const string NonVirtualProperty = "fs_nonVirtual";

        /// <summary>
        /// Controls whether a vector field should be sorted or not. The field must be a vector.
        /// Valid On:
        /// - Field 
        /// </summary>
        public const string SortedVector = "fs_sortedVector";

        /// <summary>
        /// Controls whether FlatSharp should model the string or string vector as a SharedString for string deduplication.
        /// Valid On:
        /// - String Field or Vector of String field
        /// </summary>
        public const string SharedString = "fs_sharedString";

        /// <summary>
        /// Controls how FlatSharp should generate a default constructor for the given type.
        /// </summary>
        public const string DefaultConstructorKind = "fs_defaultCtor";

        /// <summary>
        /// Controls the type of vector FlatSharp will generate. Valid values can be found in <see cref="VectorType"/>.
        /// Valid On:
        /// - Table vector field
        /// </summary>
        public const string VectorKind = "fs_vector";

        /// <summary>
        /// Controls the type of setter FlatSharp will generate for a given field. Valid values can be found in <see cref="SetterKind"/>.
        /// Valid On:
        /// - Table field
        /// - Struct field
        /// </summary>
        public const string Setter = "fs_setter";

        /// <summary>
        /// Indicates that a struct is to be generated as a value type.
        /// </summary>
        public const string ValueStruct = "fs_valueStruct";

        /// <summary>
        /// Indicates than a value struct vector's accessor should be generated with unsafe code.
        /// </summary>
        public const string UnsafeValueStructVector = "fs_unsafeStructVector";

        /// <summary>
        /// Allows a value struct to be serialized/deserialized with a call to MemoryMarshal.
        /// </summary>
        public const string MemoryMarshalBehavior = "fs_memoryMarshal";

        /// <summary>
        /// Indicates that a single table field should be force-written, or when declared on a table
        /// that all fields should be force written. Force Write refers to writing a value even if it
        /// matches the default.
        /// </summary>
        public const string ForceWrite = "fs_forceWrite";

        /// <summary>
        /// Indicates that a single field should be enabled for write-through to the underlying buffer.
        /// </summary>
        public const string WriteThrough = "fs_writeThrough";

        /// <summary>
        /// Indicates that a defined rpc should have an interface generated for it.
        /// </summary>
        public const string RpcInterface = "fs_rpcInterface";

        /// <summary>
        /// Marks a table field as deprecated. Deprecated fields do not have their values serialized or parsed.
        /// Valid On:
        /// - Table field
        /// </summary>
        public const string Deprecated = "deprecated";

        /// <summary>
        /// Marks a table field as being the key to a sorted vector.
        /// Valid On:
        /// - Table field, with the type of string or scalar.
        /// </summary>
        public const string Key = "key";

        /// <summary>
        /// Defines an explicit ID for a table field, so fields can be out-of-order in the FBS file.
        /// Valid On:
        /// - Table field
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// Added to enums to define them as a bit mask.
        /// </summary>
        public const string BitFlags = "bit_flags";

        /// <summary>
        /// Required fields on tables.
        /// </summary>
        public const string Required = "required";

        /// <summary>
        /// gRPC streaming kinds.
        /// </summary>
        public const string Streaming = "streaming";

        public static IEnumerable<string> UnsupportedStandardAttributes => new[]
        {
            "force_align", "flexbuffer", "hash", "original_order"
        };
    }
}
