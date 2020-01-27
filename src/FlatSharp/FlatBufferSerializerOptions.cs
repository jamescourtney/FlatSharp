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

using System;

namespace FlatSharp
{
    /// <summary>
    /// Defines FlatSharp serializer options.
    /// </summary>
    [Flags]
    public enum FlatBufferSerializerFlags
    {
        /// <summary>
        /// No options specified. FlatSharp parses in full lazy mode.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates if list vectors should have their data cached after reading. This option will cause more allocations
        /// on deserializing, but will improve performance in cases of duplicate accesses to the same indices.
        /// </summary>
        CacheListVectorData = 1,

        /// <summary>
        /// Indicates if the serializer should generate mutable objects. Mutable objects are "copy-on-write"
        /// and do not modify the underlying buffer.
        /// </summary>
        GenerateMutableObjects = 2,

        /// <summary>
        /// Indicates if deserialization should be greedy.
        /// </summary>
        GreedyDeserialize = 4,

        /// <summary>
        /// Default options.
        /// </summary>
        Default = GreedyDeserialize,
}

    /// <summary>
    /// Defines various confiration settings for serializing and deserializing buffers.
    /// </summary>
    public class FlatBufferSerializerOptions
    {
        /// <summary>
        /// Creates a new instance of FlatBufferSerializer options.
        /// </summary>
        /// <param name="cacheListVectorData">
        /// Indicates that IList vectors should have their reads progressively cached.
        /// </param>
        /// <param name="generateMutableObjects">
        /// Indicates that deserialzied objects should be mutable. The semantics of mutability 
        /// are "copy on write", which means that the modified data is stored in memory independently 
        /// of the input buffer.
        /// </param>
        /// <param name="greedyDeserialize">
        /// Indicates if deserialization should be done greedily. 
        /// This option has similar performance as the <paramref name="cacheListVectorData"/> option, 
        /// assuming a full traversal of the object graph. The main use case of this option is to
        /// immediately release the reference to the input buffer, such that the calling application
        /// can more effectively pool or recycle the buffer.
        /// </param>
        [Obsolete("This constructor has been replaced. Please use FlatBufferSerializationFlags instead.")]
        public FlatBufferSerializerOptions(
            bool cacheListVectorData,
            bool generateMutableObjects,
            bool greedyDeserialize)
        {
            FlatBufferSerializerFlags flags = FlatBufferSerializerFlags.None;
            if (cacheListVectorData)
            {
                flags |= FlatBufferSerializerFlags.CacheListVectorData;
            }

            if (generateMutableObjects)
            {
                flags |= FlatBufferSerializerFlags.GenerateMutableObjects;
            }

            if (greedyDeserialize)
            {
                flags |= FlatBufferSerializerFlags.GreedyDeserialize;
            }

            this.Flags = flags;
        }

        /// <summary>
        /// Initializes a new instance of FlatBufferSerializerOptions with the given set of flags.
        /// </summary>
        /// <param name="flags">The flags.</param>
        public FlatBufferSerializerOptions(FlatBufferSerializerFlags flags = FlatBufferSerializerFlags.Default)
        {
            this.Flags = flags;
        }

        public FlatBufferSerializerFlags Flags { get; }

        /// <summary>
        /// Indicates if list vectors should have their data cached after reading. This option will cause more allocations
        /// on deserializing, but will improve performance in cases of duplicate accesses to the same indices.
        /// </summary>
        public bool CacheListVectorData => this.Flags.HasFlag(FlatBufferSerializerFlags.CacheListVectorData);
        
        /// <summary>
        /// Indicates if the serializer should generate mutable objects. Mutable objects are "copy-on-write"
        /// and do not modify the underlying buffer.
        /// </summary>
        public bool GenerateMutableObjects => this.Flags.HasFlag(FlatBufferSerializerFlags.GenerateMutableObjects);

        /// <summary>
        /// Indicates if deserialization should be greedy.
        /// </summary>
        public bool GreedyDeserialize => this.Flags.HasFlag(FlatBufferSerializerFlags.GreedyDeserialize);

        /// <summary>
        /// Indicates if FlatSharp should intercept app domain load events to look for cross-referenced generated assemblies. Mostly useful for FlatSharp unit tests.
        /// </summary>
        public bool EnableAppDomainInterceptOnAssemblyLoad { get; set; }
    }
}
