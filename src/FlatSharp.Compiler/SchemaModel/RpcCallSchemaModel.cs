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
            this.call = call;

            this.ValidateAttributes(this.attributes);
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

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsDeserializationOption(FlatBufferDeserializationOption option) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsForceWrite(bool forceWriteOption) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsMemoryMarshal(MemoryMarshalBehavior option) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsNonVirtual(bool nonVirtualValue) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsRpcInterface(bool supportsRpcInterface) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsSetterKind(SetterKind setterKind) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsSharedString(bool sharedStringOption) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsSortedVector(bool sortedVectorOption) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsUnsafeStructVector(bool unsafeStructVector) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsVectorType(VectorType vectorType) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsWriteThrough(bool writeThroughOption) => SupportTestResult.NeverValid;

        SupportTestResult IFlatSharpAttributeSupportTester.SupportsStreamingType(RpcStreamingType streamingType) => SupportTestResult.Valid;
    }
}
