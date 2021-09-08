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
    using FlatSharp;
    using FlatSharp.Attributes;

    public class MutableFlatSharpAttributes : IFlatSharpAttributes
    {
        public MutableFlatSharpAttributes() : this(null)
        {
        }

        public MutableFlatSharpAttributes(IFlatSharpAttributes? other)
        {
            if (other is not null)
            {
                this.DefaultCtorKind = other.DefaultCtorKind;
                this.DeserializationOption = other.DeserializationOption;
                this.ForceWrite = other.ForceWrite;
                this.MemoryMarshalBehavior = other.MemoryMarshalBehavior;
                this.NonVirtual = other.NonVirtual;
                this.RpcInterface = other.RpcInterface;
                this.SetterKind = other.SetterKind;
                this.SharedString = other.SharedString;
                this.SortedVector = other.SortedVector;
                this.UnsafeStructVector = other.UnsafeStructVector;
                this.ValueStruct = other.ValueStruct;
                this.VectorKind = other.VectorKind;
                this.WriteThrough = other.WriteThrough;
                this.StreamingType = other.StreamingType;
            }
        }

        public DefaultConstructorKind? DefaultCtorKind { get; set; }

        public FlatBufferDeserializationOption? DeserializationOption { get; set; }

        public bool? ForceWrite { get; set; }

        public MemoryMarshalBehavior? MemoryMarshalBehavior { get; set; }

        public bool? NonVirtual { get; set; }

        public bool? RpcInterface { get; set; }

        public SetterKind? SetterKind { get; set; }

        public bool? SharedString { get; set; }

        public bool? SortedVector { get; set; }

        public bool? UnsafeStructVector { get; set; }

        public bool? ValueStruct { get; set; }

        public VectorType? VectorKind { get; set; }

        public bool? WriteThrough { get; set; }

        public RpcStreamingType? StreamingType { get; set; }
    }
}
