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
    using System.ComponentModel;

    /// <summary>
    /// Marks a class as being a FlatBuffer struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FlatBufferStructAttribute : Attribute
    {
        /// <summary>
        /// Specifies the write-through interface for derived classes to implement. Reserved for use by the FlatSharp compiler
        /// and may change in future versions.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? WriteThroughInterfaceName { get; set; }

        /// <summary>
        /// Specifies the maximum size of the object pool for items of this type. A value of 0 indicates that the pool is disabled, -1 allows the pool to grow 
        /// without bound.
        /// </summary>
        public int RecyclePoolSize { get; set; }
    }
}
