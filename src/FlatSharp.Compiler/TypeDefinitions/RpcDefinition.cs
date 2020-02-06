/*
 * Copyright 2020 James Courtney
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
    using System;
    using System.Collections.Generic;

    public enum RpcStreamingType
    {
        Unary = 0,
        None = 0,

        Client = 1,
        Server = 2,
        Bidirectional = 3,
    }

    internal class RpcDefinition : BaseSchemaMember
    {
        private const string GrpcCore = "Grpc.Core";

        private static readonly string CreateMarshallerFunction = $@"
        private static Grpc.Core.Marshaller<T> CreateMarshaller<T>(ISerializer<T> serializer) where T : class
        {{
            return Grpc.Core.Marshallers.Create<T>(
                (item, sc) =>
                {{
                    var bufferWriter = sc.GetBufferWriter();
                    var span = bufferWriter.GetSpan(serializer.GetMaxSize(item));
                    int bytesWritten = serializer.Write(new SpanWriter(), span, item);
                    bufferWriter.Advance(bytesWritten);
                    sc.Complete();
                }},
                dc => serializer.Parse(new ArrayInputBuffer(dc.PayloadAsNewBuffer())));
        }}";

        private readonly Dictionary<string, (string requestType, string responseType, RpcStreamingType streamingType)> methods;

        public RpcDefinition(string serviceName, BaseSchemaMember parent) : base(serviceName, parent)
        {
            this.methods = new Dictionary<string, (string, string, RpcStreamingType)>();
        }

        public IReadOnlyDictionary<string, (string requestType, string responseType, RpcStreamingType streamingType)> Methods => this.methods;

        public void AddRpcMethod(string name, string requestType, string responseType, RpcStreamingType rpcType)
        {
            if (this.methods.ContainsKey(name))
            {
                ErrorContext.Current.RegisterError($"Duplicate RPC method added: '{name}'");
                return;
            }

            this.methods[name] = (requestType, responseType, rpcType);
        }

        protected override bool SupportsChildren => false;

        protected override void OnWriteCode(CodeWriter writer, CodeWritingPass pass, IReadOnlyDictionary<string, string> precompiledSerializer)
        {
            if (pass == CodeWritingPass.FirstPass)
            {
                return;
            }

            writer.AppendLine($"public static class {this.Name}");
            writer.AppendLine("{");
            using (writer.IncreaseIndent())
            {
                // #1: Define the static marshaller method:
                writer.AppendLine(CreateMarshallerFunction);
                writer.AppendLine(string.Empty);

                // #2: Define the marshallers for all of our types:
                var marshallers = this.GenerateMarshallers(writer);

                // #3: Define all of the methods in our RPC. These are the core
                // of the client/server classes.
                var methods = this.DefineMethods(writer, marshallers);
            }

            writer.AppendLine("}");
        }

        private Dictionary<string, string> GenerateMarshallers(CodeWriter writer)
        {
            Dictionary<string, string> marshallerTypeNameMappings = new Dictionary<string, string>();
            foreach (var rpc in this.methods)
            {
                string requestType = rpc.Value.requestType;
                string responseType = rpc.Value.responseType;

                if (!marshallerTypeNameMappings.ContainsKey(requestType))
                {
                    marshallerTypeNameMappings[requestType] = this.GenerateMarshaller(requestType, writer);
                }

                if (!marshallerTypeNameMappings.ContainsKey(responseType))
                {
                    marshallerTypeNameMappings[responseType] = this.GenerateMarshaller(responseType, writer);
                }
            }

            return marshallerTypeNameMappings;
        }

        private string GenerateMarshaller(string type, CodeWriter writer)
        {
            string name = $"__Marshaller_{Guid.NewGuid():n}";
            writer.AppendLine($"private static readonly {GrpcCore}.Marshaller<{type}> {name} = CreateMarshaller({type}.Serializer);");

            return name;
        }

        private Dictionary<string, string> DefineMethods(CodeWriter writer, Dictionary<string, string> marshallers)
        {
            var methodDefinitionMap = new Dictionary<string, string>();

            foreach (var kvp in this.methods)
            {
                string methodName = kvp.Key;
                var streamingType = kvp.Value.streamingType;
                var requestType = kvp.Value.requestType;
                var responseType = kvp.Value.responseType;

                string methodVariableName = $"__Method_{Guid.NewGuid():n}";

                methodDefinitionMap[methodName] = methodVariableName;

                writer.AppendLine($"private static readonly {GrpcCore}.Method<{requestType},{responseType}> {methodVariableName} = new {GrpcCore}.Method<{requestType},{responseType}>(");
                using (writer.IncreaseIndent())
                {
                    writer.AppendLine($"{GrpcCore}.MethodType.{GetGrpcMethodType(streamingType)},");
                    writer.AppendLine($"\"{this.FullName}\",");
                    writer.AppendLine($"\"{methodName}\",");
                    writer.AppendLine($"{marshallers[requestType]},");
                    writer.AppendLine($"{marshallers[responseType]});");
                }
            }

            return methodDefinitionMap;
        }

        private static string GetGrpcMethodType(RpcStreamingType streamingType)
        {
            switch (streamingType)
            {
                case RpcStreamingType.Client:
                    return "ClientStreaming";

                case RpcStreamingType.Server:
                    return "ServerStreaming";

                case RpcStreamingType.Bidirectional:
                    return "DuplexStreaming";

                case RpcStreamingType.Unary:
                    return "Unary";

                default:
                    throw new InvalidOperationException("Unexpected streaming type: " + streamingType);
            }
        }
    }
}
