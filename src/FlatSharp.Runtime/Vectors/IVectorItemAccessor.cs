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

namespace FlatSharp.Internal;

/// <summary>
/// Small interface for reading and writing to/from vectors. Implementations should
/// prefer using a struct as that will enable devirtualization.
/// </summary>
public interface IVectorItemAccessor<TItem, TInputBuffer>
{
    int Count { get; }

    int ItemSize { get; }

    void ParseItem(int index, TInputBuffer buffer, short remainingDepth, TableFieldContext context, out TItem item);

    void WriteThrough(int index, TItem value, TInputBuffer inputBuffer, TableFieldContext context);

    int OffsetOf(int index);

    TReturn InvokeEnumerator<TEnumerator, TReturn>(TEnumerator enumerator)
        where TEnumerator : IFlatBufferVectorEnumerator<TItem, TReturn>;
}