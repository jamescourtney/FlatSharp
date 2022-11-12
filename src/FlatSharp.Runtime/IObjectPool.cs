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
    /// Defines an object pool that FlatSharp may use to reduce allocations.
    /// </summary>
    public interface IObjectPool
    {
        /// <summary>
        /// Attempts to get an item from the pool.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="value">The value, as an output parameter.</param>
        /// <returns>True if the value was returned. False otherwise.</returns>
        bool TryGet<T>([NotNullWhen(true)] out T? value);

        /// <summary>
        /// Returns an item to the pool. FlatSharp users should never use this method directly.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to return.</param>
        void Return<T>(T item);
    }

    /// <summary>
    /// A FlatSharp poolable object.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Attempts to return this object to the pool.
        /// </summary>
        /// <param name="unsafeForce">Force this back to the pool, regardless of internal consistency rules.</param>
        void ReturnToPool(bool unsafeForce = false);
    }
}

namespace FlatSharp.Internal
{
    /// <summary>
    /// Debug information for poolable objects.
    /// </summary>
    public interface IPoolableObjectDebug
    {
        /// <summary>
        /// Indicates if this object is the root of the parse tree.
        /// </summary>
        bool IsRoot { get; set; }
    }
}