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

namespace FlatSharp
{
    using System;

    /// <summary>
    /// An object that can supply an <see cref="ISerializer"/> instance that can serialize and parse it.
    /// </summary>
    public interface IFlatBufferSerializable
    {
        /// <summary>
        /// Gets a <see cref="ISerializer"/> instance that can serialize this object.
        /// </summary>
        ISerializer Serializer { get; }
    }

    /// <summary>
    /// An object that can supply an <see cref="ISerializer{T}"/> instance that can serialize and parse it.
    /// </summary>
    public interface IFlatBufferSerializable<T> : IFlatBufferSerializable
        where T : class
    {
        /// <summary>
        /// Gets a <see cref="ISerializer{T}"/> instance that can serialize this object.
        /// </summary>
        new ISerializer<T> Serializer { get; }
    }
}
