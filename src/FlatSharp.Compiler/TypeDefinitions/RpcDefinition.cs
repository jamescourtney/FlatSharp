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
        Bidi = 3,
        Duplex = 3,
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
                    int bytesWritten = serializer.Write(SpanWriter.Instance, span, item);
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

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass == CodeWritingPass.FirstPass)
            {
                this.ValidateReferencedTables();
                return;
            }

            writer.AppendLine($"public static partial class {this.Name}");
            using (writer.WithBlock())
            {
                // #1: Define the static marshaller method:
                writer.AppendLine(CreateMarshallerFunction);
                writer.AppendLine(string.Empty);

                // #2: Define the marshallers for all of our types:
                var marshallers = this.GenerateMarshallers(writer);

                // #3: Define all of the methods in our RPC. These are the core
                // of the client/server classes.
                var methods = this.DefineMethods(writer, marshallers);

                this.DefineServerBaseClass(writer, methods);
                this.DefineClientClass(writer, methods);
            }
        }

        private void ValidateReferencedTables()
        {
            foreach (var method in this.methods)
            {
                ErrorContext.Current.WithScope(method.Key, () => 
                {
                    (string requestType, string responseType, _) = method.Value;

                    this.ValidateDependency(requestType);
                    this.ValidateDependency(responseType);
                });
            }
        }

        private void ValidateDependency(string typeName)
        {
            ErrorContext.Current.WithScope(typeName, () =>
            {
                if (!this.TryResolveName(typeName, out BaseSchemaMember node))
                {
                    ErrorContext.Current.RegisterError($"Unable to resolve '{typeName}'.");
                    return;
                }

                if (node is TableOrStructDefinition tableOrStruct)
                {
                    if (!tableOrStruct.IsTable)
                    {
                        ErrorContext.Current.RegisterError("RPC definitions can only operate on tables. Structs are not allowed.");
                        return;
                    }

                    if (tableOrStruct.RequestedSerializer == null)
                    {
                        ErrorContext.Current.RegisterError("Types declared in RPC definitions must have PrecompiledSerializers enabled.");
                    }
                }
                else
                {
                    ErrorContext.Current.RegisterError($"RPC definitions can only operate on tables. Unable to resolve '{typeName}' as a table.");
                }
            });
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

        private void DefineServerBaseClass(CodeWriter writer, Dictionary<string, string> methodNameMap)
        {
            string baseClassName = $"{this.Name}ServerBase";

            writer.AppendLine($"[{GrpcCore}.BindServiceMethod(typeof({this.Name}), \"BindService\")]");
            writer.AppendLine($"public abstract partial class {baseClassName}");
            using (writer.WithBlock())
            {
                foreach (var method in this.methods)
                {
                    writer.AppendLine(this.GetServerMethodSignature(method.Key, method.Value.requestType, method.Value.responseType, method.Value.streamingType));
                }
            }

            // Write bind service method
            writer.AppendLine($"public static {GrpcCore}.ServerServiceDefinition BindService({baseClassName} serviceImpl)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return {GrpcCore}.ServerServiceDefinition.CreateBuilder()");
                using (writer.IncreaseIndent())
                {
                    foreach (var method in this.methods)
                    {
                        writer.AppendLine($".AddMethod({methodNameMap[method.Key]}, serviceImpl.{method.Key})");
                    }

                    writer.AppendLine(".Build();");
                }
            }

            // Write bind service overload
            writer.AppendLine();
            writer.AppendLine($"public static void BindService({GrpcCore}.ServiceBinderBase serviceBinder, {baseClassName} serviceImpl)");
            using (writer.WithBlock())
            {
                foreach (var method in methods)
                {
                    writer.AppendLine($"serviceBinder.AddMethod({methodNameMap[method.Key]},");
                    using (writer.IncreaseIndent())
                    {
                        writer.AppendLine("serviceImpl == null");
                        using (writer.IncreaseIndent())
                        {
                            writer.AppendLine("? null");
                            string serverDelegate = GetServerHandlerDelegate(method.Key, method.Value.requestType,
                                method.Value.responseType, method.Value.streamingType);
                            writer.AppendLine($": {serverDelegate});");
                        }
                    }
                }
            }
        }

        private string GetServerMethodSignature(string name, string requestType, string responseType, RpcStreamingType streamingType)
        {
            const string TaskString = "System.Threading.Tasks.Task";
            switch (streamingType)
            {
                case RpcStreamingType.Unary:
                    return $"public abstract {TaskString}<{responseType}> {name}({requestType} request, {GrpcCore}.ServerCallContext callContext);";


                case RpcStreamingType.Client:
                    return $"public abstract {TaskString}<{responseType}> {name}({GrpcCore}.IAsyncStreamReader<{requestType}> requestStream, {GrpcCore}.ServerCallContext callContext);";


                case RpcStreamingType.Server:
                    return $"public abstract {TaskString} {name}({requestType} request, {GrpcCore}.IServerStreamWriter<{responseType}> responseStream, {GrpcCore}.ServerCallContext callContext);";


                case RpcStreamingType.Bidirectional:
                    return $"public abstract {TaskString} {name}({GrpcCore}.IAsyncStreamReader<{requestType}> requestStream, {GrpcCore}.IServerStreamWriter<{responseType}> responseStream, {GrpcCore}.ServerCallContext callContext);";
            }

            throw new InvalidOperationException("Unrecognized streaming type: " + streamingType);
        }

        private void DefineClientClass(CodeWriter writer, Dictionary<string, string> methodMapping)
        {
            string clientClassName = $"{this.Name}Client";

            writer.AppendLine($"public partial class {clientClassName} : {GrpcCore}.ClientBase<{clientClassName}>");
            using (writer.WithBlock())
            {
                this.DefineClientConstructors(writer, clientClassName);

                foreach (var item in this.methods)
                {
                    this.WriteClientMethod(writer, item.Key, item.Value.requestType, item.Value.responseType, item.Value.streamingType, methodMapping);
                }
            }
        }

        private void DefineClientConstructors(CodeWriter writer, string className)
        {
            writer.AppendLine($"public {className}({GrpcCore}.ChannelBase channel) : base(channel) {{ }}");
            writer.AppendLine($"public {className}({GrpcCore}.CallInvoker callInvoker) : base(callInvoker) {{ }}");
            writer.AppendLine($"protected {className}() : base() {{ }}");
            writer.AppendLine($"protected {className}(ClientBaseConfiguration configuration) : base(configuration) {{ }}");
            writer.AppendLine();

            writer.AppendLine($"protected override {className} NewInstance(ClientBaseConfiguration configuration)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return new {className}(configuration);");
            }
        }

        private void WriteClientMethod(CodeWriter writer, string methodName, string requestType, string responseType, RpcStreamingType streamingType, Dictionary<string, string> methodMap)
        {
            switch (streamingType)
            {
                case RpcStreamingType.Unary:
                    this.WriteRequestParameterMethod(writer, "AsyncUnaryCall", methodName, requestType, responseType, methodMap);
                    break;

                case RpcStreamingType.Client:
                    this.WriteNoRequestParameterMethod(writer, "AsyncClientStreamingCall", methodName, requestType, responseType, methodMap);
                    break;

                case RpcStreamingType.Server:
                    this.WriteRequestParameterMethod(writer, "AsyncServerStreamingCall", methodName, requestType, responseType, methodMap);
                    break;

                case RpcStreamingType.Bidirectional:
                    this.WriteNoRequestParameterMethod(writer, "AsyncDuplexStreamingCall", methodName, requestType, responseType, methodMap);
                    break;
            }
        }

        private const string CancellationToken = "System.Threading.CancellationToken";

        private void WriteRequestParameterMethod(
            CodeWriter writer,
            string returnType,
            string methodName,
            string requestType,
            string responseType,
            Dictionary<string, string> methodMap)
        {
            writer.AppendLine($"public virtual {GrpcCore}.{returnType}<{responseType}> {methodName}({requestType} request, {GrpcCore}.Metadata headers = null, System.DateTime? deadline = null, {CancellationToken} cancellationToken = default({CancellationToken}))");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return {methodName}(request, new {GrpcCore}.CallOptions(headers, deadline, cancellationToken));");
            }

            writer.AppendLine($"public virtual {GrpcCore}.{returnType}<{responseType}> {methodName}({requestType} request, {GrpcCore}.CallOptions options)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return CallInvoker.{returnType}({methodMap[methodName]}, null, options, request);");
            }
        }

        private void WriteNoRequestParameterMethod(
            CodeWriter writer,
            string key,
            string methodName,
            string requestType,
            string responseType,
            Dictionary<string, string> methodMap)
        {
            writer.AppendLine($"public virtual {GrpcCore}.{key}<{requestType}, {responseType}> {methodName}({GrpcCore}.Metadata headers = null, System.DateTime? deadline = null, {CancellationToken} cancellationToken = default({CancellationToken}))");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return {methodName}(new {GrpcCore}.CallOptions(headers, deadline, cancellationToken));");
            }

            writer.AppendLine($"public virtual {GrpcCore}.{key}<{requestType}, {responseType}> {methodName}({GrpcCore}.CallOptions options)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return CallInvoker.{key}({methodMap[methodName]}, null, options);");
            }
        }

        protected override string OnGetCopyExpression(string source)
        {
            throw new NotImplementedException();
        }

        private string GetServerHandlerDelegate(string name, string requestType, string responseType, RpcStreamingType streamingType)
        {
            string methodType = GetGrpcMethodType(streamingType);
            return $"new {GrpcCore}.{methodType}ServerMethod<{requestType}, {responseType}>(serviceImpl.{name})";
        }
    }
}
