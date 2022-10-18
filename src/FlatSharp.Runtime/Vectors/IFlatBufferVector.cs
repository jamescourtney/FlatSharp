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
/// A deserialized flatbuffer vector.
/// </summary>
public interface IFlatBufferVector<T>
{
    public TReturn Enumerate<TEnumerator, TReturn>(TEnumerator visitor)
         where TEnumerator : IFlatBufferVectorEnumerator<T, TReturn>;
}

public interface IEnumerableFlatBufferVector<T>
{
    public int Count { get; }

    public T this[int index] { get; set; }
}
