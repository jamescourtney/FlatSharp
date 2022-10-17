/*
 * Copyright 2022 James Courtney
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

namespace FlatSharp;

/// <summary>
/// Traverses a vector.
/// </summary>
public interface IFlatBufferVectorVisitor<T>
{
    /// <summary>
    /// Visits the given item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>True to continue enumeration; false to terminate.</returns>
    bool Visit<TFlatBufferVector>(ref T item, ref TFlatBufferVector vector, ref int nextIndex)
        where TFlatBufferVector : IFlatBufferVector<T>;
}