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

namespace FlatSharp.Compiler.SchemaModel
{
    using FlatSharp.Attributes;

    public enum FlatBufferSchemaElementType
    {
        Unknown = 0,
        Table = 1,
        Struct = 2,
        ValueStruct = 3,
        Enum = 4,
        Union = 5,
        RpcService = 6,
        TableField = 7,
        StructField = 8,
        ValueStructField = 9,
    }

    public interface IFlatSharpAttributeSupportTester
    {
        FlatBufferSchemaElementType ElementType { get; }

        string FullName { get; }

        bool SupportsNonVirtual(bool nonVirtualValue);

        bool SupportsVectorType(VectorType vectorType);

        bool SupportsDeserializationOption(FlatBufferDeserializationOption option);

        bool SupportsSortedVector(bool sortedVectorOption);

        bool SupportsSharedString(bool sharedStringOption);

        bool SupportsDefaultCtorKindOption(DefaultConstructorKind kind);

        bool SupportsSetterKind(SetterKind setterKind);

        bool SupportsForceWrite(bool forceWriteOption);

        bool SupportsUnsafeStructVector(bool unsafeStructVector);

        bool SupportsMemoryMarshal(MemoryMarshalBehavior option);

        bool SupportsWriteThrough(bool writeThroughOption);

        bool SupportsRpcInterface(bool rpcInterface);
    }
}
