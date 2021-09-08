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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FlatSharp;
    using FlatSharp.Compiler.Schema;
    using System.Diagnostics.CodeAnalysis;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;

    public class RpcServiceSchemaModel : BaseSchemaModel
    {
        private const string GrpcCore = "Grpc.Core";
        private const string Channels = "System.Threading.Channels";

        private static readonly string CreateMarshallerFunction = $@"
        private static Grpc.Core.Marshaller<T> CreateMarshaller<T>() where T : class
        {{
            return Grpc.Core.Marshallers.Create<T>(
                (item, sc) =>
                {{
                    var serializer = Serializer<T>.Value;
                    var bufferWriter = sc.GetBufferWriter();
                    var span = bufferWriter.GetSpan(serializer.GetMaxSize(item));
                    int bytesWritten = serializer.Write(SpanWriter.Instance, span, item);
                    bufferWriter.Advance(bytesWritten);
                    sc.Complete();
                }},
                dc => Serializer<T>.Value.Parse(new ArrayInputBuffer(dc.PayloadAsNewBuffer())));
        }}";

        private readonly RpcService service;
        private readonly List<RpcCallSchemaModel> calls;
        private string interfaceName;

        public RpcServiceSchemaModel(Schema schema, RpcService service) : base(schema, service.Name, new FlatSharpAttributes(service.Attributes))
        {
            this.service = service;
            this.interfaceName = $"I{service.Name}";
            this.calls = new();

            if (service.Calls is not null)
            {
                foreach (var call in service.Calls)
                {
                    this.calls.Add(new(service, call));
                }
            }
        }

        public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.RpcService;

        public override string DeclaringFile => this.service.DeclaringFile;

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass < CodeWritingPass.RpcGeneration)
            {
                return;
            }

            if (this.Attributes.RpcInterface == true)
            {
                this.DefineInterface(writer);
            }
        }

        private void DefineInterface(CodeWriter writer)
        {
            writer.AppendLine($"public interface {this.interfaceName}");
            using (writer.WithBlock())
            {
                foreach (var call in this.calls)
                {
                    switch (call.StreamingType)
                    {
                        case RpcStreamingType.Unary:
                            writer.AppendLine($"Task<{call.ResponseType}> {call.Name}({call.RequestType} request, CancellationToken token);");
                            break;

                        case RpcStreamingType.Client:
                            writer.AppendLine($"Task<{call.ResponseType}> {call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, CancellationToken token);");
                            break;

                        case RpcStreamingType.Server:
                            writer.AppendLine($"Task {call.Name}({call.RequestType} request, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token);");
                            break;

                        case RpcStreamingType.Bidirectional:
                            writer.AppendLine($"Task {call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token);");
                            break;
                    }
                }
            }
        }

        public override bool SupportsRpcInterface(bool supportsRpcInterface) => true;
    }
}
