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

    using FlatSharp;
    using FlatSharp.Compiler.Schema;
    using FlatSharp.Attributes;

    public class RpcCallSchemaModel : IFlatSharpAttributeSupportTester
    {
        private readonly FlatSharpAttributes attributes;
        private readonly string fullName;
        private readonly RpcCall call;

        public RpcCallSchemaModel(RpcService parentService, RpcCall call)
        {
            this.attributes = new(call.Attributes);
            this.fullName = $"{parentService.Name}.{call.Name}";

            this.attributes.Validate(this);
            this.ValidateHasSerializer(call.Request);
            this.ValidateHasSerializer(call.Response);
        }

        public string Name => this.call.Name;

        public string RequestType => this.call.Request.Name;

        public string ResponseType => this.call.Response.Name;

        public RpcStreamingType StreamingType => this.attributes.StreamingType ?? RpcStreamingType.None;

        private void ValidateHasSerializer(FlatBufferObject obj)
        {
            FlatSharpInternal.Assert(!obj.IsStruct, "expecting only tables");
            FlatSharpAttributes attrs = new FlatSharpAttributes(obj.Attributes);

            if (attrs.DeserializationOption is null)
            {
                ErrorContext.Current.RegisterError($"RPC call '{this.fullName}' uses table '{obj.Name}', which does not specify the '{MetadataKeys.SerializerKind}' attribute.");
            }
        }

        FlatBufferSchemaElementType IFlatSharpAttributeSupportTester.ElementType => FlatBufferSchemaElementType.RpcCall;

        string IFlatSharpAttributeSupportTester.FullName => this.fullName;

        bool IFlatSharpAttributeSupportTester.SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => false;

        bool IFlatSharpAttributeSupportTester.SupportsDeserializationOption(FlatBufferDeserializationOption option) => false;

        bool IFlatSharpAttributeSupportTester.SupportsForceWrite(bool forceWriteOption) => false;

        bool IFlatSharpAttributeSupportTester.SupportsMemoryMarshal(MemoryMarshalBehavior option) => false;

        bool IFlatSharpAttributeSupportTester.SupportsNonVirtual(bool nonVirtualValue) => false;

        bool IFlatSharpAttributeSupportTester.SupportsRpcInterface(bool supportsRpcInterface) => false;

        bool IFlatSharpAttributeSupportTester.SupportsSetterKind(SetterKind setterKind) => false;

        bool IFlatSharpAttributeSupportTester.SupportsSharedString(bool sharedStringOption) => false;

        bool IFlatSharpAttributeSupportTester.SupportsSortedVector(bool sortedVectorOption) => false;

        bool IFlatSharpAttributeSupportTester.SupportsUnsafeStructVector(bool unsafeStructVector) => false;

        bool IFlatSharpAttributeSupportTester.SupportsVectorType(VectorType vectorType) => false;

        bool IFlatSharpAttributeSupportTester.SupportsWriteThrough(bool writeThroughOption) => false;

        bool IFlatSharpAttributeSupportTester.SupportsStreamingType(RpcStreamingType streamingType) => true;
    }
}
