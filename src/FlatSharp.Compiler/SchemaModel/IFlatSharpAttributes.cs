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
    using FlatSharp.Attributes;

    public interface IFlatSharpAttributes
    {
        DefaultConstructorKind? DefaultCtorKind { get; }

        FlatBufferDeserializationOption? DeserializationOption { get; }

        bool? ForceWrite { get; }

        MemoryMarshalBehavior? MemoryMarshalBehavior { get; }

        bool? NonVirtual { get; }

        bool? RpcInterface { get; }

        SetterKind? SetterKind { get; }

        bool? SharedString { get; }

        bool? SortedVector { get; }

        bool? UnsafeStructVector { get; }

        bool? ValueStruct { get; }

        VectorType? VectorKind { get; }

        bool? WriteThrough { get; }

        RpcStreamingType? StreamingType { get; }
    }
}