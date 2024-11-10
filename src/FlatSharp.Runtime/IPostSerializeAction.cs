/*
 * Copyright 2024 James Courtney
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
/// Describes an action that can be invoked after a serialize operation.
/// </summary>
public interface IPostSerializeAction
{
    void Invoke<TTarget>(TTarget target, SerializationContext context)
        where TTarget : IFlatBufferSerializationTarget<TTarget>
        #if NET9_0_OR_GREATER
        , allows ref struct
        #endif
        ;
}