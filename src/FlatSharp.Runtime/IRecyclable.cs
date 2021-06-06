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
    /// <summary>
    /// An object that can be recycled and returned to a pool.
    /// </summary>
    public interface IRecyclable
    {
        /// <summary>
        /// Formally releases this object and allows FlatSharp to recycle it for use in future deserialization operations. 
        /// This method should generally only be invoked from FlatSharp generated code, though special high-performance 
        /// use cases may exist where it is useful. Improper use of this method may lead to unexpected results and invalid state.
        /// This method is invoked automatically via <see cref="ISerializer{T}.Recycle(ref T?)"/>, which traverses the full object
        /// graph and releases everything. This method cannot be assumed to be thread safe.
        /// </summary>
        /// <remarks>
        /// A case where this method may be valuable is when enumeratoring vectors with <see cref="FlatBufferDeserializationOption.Lazy"/> deserialization.
        /// </remarks>
        void DangerousRecycle();
    }
}
