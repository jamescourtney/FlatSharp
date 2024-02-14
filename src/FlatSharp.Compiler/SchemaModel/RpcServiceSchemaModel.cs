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

using FlatSharp.CodeGen;
using FlatSharp.Compiler.Schema;

namespace FlatSharp.Compiler.SchemaModel;

public class RpcServiceSchemaModel : BaseSchemaModel
{
    private const string GrpcCore = "Grpc.Core";
    private const string Channels = "System.Threading.Channels";
    private const string CancellationToken = "System.Threading.CancellationToken";

    private static readonly string CreateMarshallerFunction = $@"
        private static Grpc.Core.Marshaller<T> CreateMarshaller<T>() where T : class
        {{
            return Grpc.Core.Marshallers.Create<T>(
                (item, sc) =>
                {{
                    Serializer<T>.Value.Write(sc.GetBufferWriter(), item);
                    sc.Complete();
                }},
                dc => Serializer<T>.Value.Parse(new ArrayInputBuffer(dc.PayloadAsNewBuffer())));
        }}";

    private readonly RpcService service;
    private readonly List<RpcCallSchemaModel> calls;
    private string interfaceName;

    public RpcServiceSchemaModel(Schema.Schema schema, RpcService service) : base(schema, service.Name, new FlatSharpAttributes(service.Attributes))
    {
        this.service = service;
        this.interfaceName = $"I{this.Name}";
        this.DeclaringFile = service.DeclaringFile;
        this.calls = new();

        if (service.Calls is not null)
        {
            foreach (var call in service.Calls)
            {
                this.calls.Add(new(service, call));
            }
        }

        this.AttributeValidator.RpcInterfaceValidator = _ => AttributeValidationResult.Valid;
    }

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.RpcService;

    public override string DeclaringFile { get; }

    public override void TraverseTypeModel(CompileContext context, HashSet<Type> seenTypes)
    {
    }

    protected override void OnWriteCode(CodeWriter writer, CompileContext context)
    {
        if (context.CompilePass < CodeWritingPass.SerializerAndRpcGeneration)
        {
            return;
        }

        if (this.Attributes.RpcInterface == true)
        {
            this.DefineInterface(writer);
        }

        writer.AppendSummaryComment(this.service.Documentation);
        this.Attributes.EmitAsMetadata(writer);
        writer.AppendLine($"public static partial class {this.Name}");
        using (writer.WithBlock())
        {
            writer.AppendLine($@"
                public static class Serializer<T> where T : class
                {{
                    private static ISerializer<T> __value;
                    static Serializer()
                    {{
                        __value = null!;
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof({this.Name}).TypeHandle);
                    }}

                    public static ISerializer<T> Value
                    {{
                        get => __value;
                        set => __value = value ?? {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.ArgumentNull)}<ISerializer<T>>(nameof(value));
                    }}
                }}
                ");

            // #1: Define the static marshaller method:
            writer.AppendLine(CreateMarshallerFunction);
            writer.AppendLine(string.Empty);

            // #2: Define the marshallers for all of our types:
            var marshallers = this.GenerateMarshallers(writer);

            // #3: Define all of the methods in our RPC. These are the core
            // of the client/server classes.
            var methods = this.DefineMethods(writer, marshallers);

            // #4: Static constructor to initialize default serializers.
            writer.AppendLine($"static partial void OnStaticInitialization();");
            writer.AppendLine();

            writer.AppendLine($"static {this.Name}()");
            using (writer.WithBlock())
            {
                foreach (string type in marshallers.Keys)
                {
                    writer.AppendLine($"Serializer<{type}>.Value = {type}.Serializer;");
                }

                writer.AppendLine("OnStaticInitialization();");
            }


            this.DefineServerBaseClass(writer, methods);
            this.DefineClientClass(writer, methods);
        }
    }

    private Dictionary<string, string> GenerateMarshallers(CodeWriter writer)
    {
        Dictionary<string, string> marshallerTypeNameMappings = new Dictionary<string, string>();
        foreach (var rpc in this.calls)
        {
            string requestType = rpc.RequestType;
            string responseType = rpc.ResponseType;

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
        writer.AppendLine($"private static readonly {GrpcCore}.Marshaller<{type}> {name} = CreateMarshaller<{type}>();");

        return name;
    }

    private Dictionary<string, string> DefineMethods(CodeWriter writer, Dictionary<string, string> marshallers)
    {
        var methodDefinitionMap = new Dictionary<string, string>();

        foreach (var call in this.calls)
        {
            string methodName = call.Name;
            var streamingType = call.StreamingType;
            var requestType = call.RequestType;
            var responseType = call.ResponseType;

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

    private void DefineServerBaseClass(CodeWriter writer, Dictionary<string, string> methodNameMap)
    {
        string baseClassName = $"{this.Name}ServerBase";

        writer.AppendSummaryComment(this.service.Documentation);
        writer.AppendLine($"[{GrpcCore}.BindServiceMethod(typeof({this.Name}), \"BindService\")]");
        writer.AppendLine($"public abstract partial class {baseClassName}");
        using (writer.WithBlock())
        {
            foreach (var call in this.calls)
            {
                writer.AppendSummaryComment(call.Documentation);
                writer.AppendLine(this.GetServerMethodSignature(call));
            }
        }

        // Write bind service method
        writer.AppendLine($"public static {GrpcCore}.ServerServiceDefinition BindService({baseClassName}? serviceImpl)");
        using (writer.WithBlock())
        {
            writer.AppendLine("#pragma warning disable CS8604");
            writer.AppendLine($"return {GrpcCore}.ServerServiceDefinition.CreateBuilder()");
            using (writer.IncreaseIndent())
            {
                foreach (var method in this.calls)
                {
                    writer.AppendLine($".AddMethod({methodNameMap[method.Name]}, serviceImpl == null ? ({this.GetServerHandlerDelegateType(method)}?)null : serviceImpl.{method.Name})");
                }

                writer.AppendLine(".Build();");
            }
            writer.AppendLine("#pragma warning restore CS8604");
        }

        // Write bind service overload
        writer.AppendLine();
        writer.AppendLine($"public static void BindService({GrpcCore}.ServiceBinderBase serviceBinder, {baseClassName}? serviceImpl)");
        using (writer.WithBlock())
        {
            writer.AppendLine("#pragma warning disable CS8604");
            foreach (var method in this.calls)
            {
                string serverDelegate = GetServerHandlerDelegate(method);
                writer.AppendLine($"serviceBinder.AddMethod({methodNameMap[method.Name]}, serviceImpl == null ? ({this.GetServerHandlerDelegateType(method)}?)null : {serverDelegate});");
            }
            writer.AppendLine("#pragma warning restore CS8604");
        }
    }

    private string GetServerMethodSignature(RpcCallSchemaModel call)
    {
        const string TaskString = "System.Threading.Tasks.Task";
        switch (call.StreamingType)
        {
            case RpcStreamingType.Unary:
                return $"public abstract {TaskString}<{call.ResponseType}> {call.Name}({call.RequestType} request, {GrpcCore}.ServerCallContext callContext);";


            case RpcStreamingType.Client:
                return $"public abstract {TaskString}<{call.ResponseType}> {call.Name}({GrpcCore}.IAsyncStreamReader<{call.RequestType}> requestStream, {GrpcCore}.ServerCallContext callContext);";


            case RpcStreamingType.Server:
                return $"public abstract {TaskString} {call.Name}({call.RequestType} request, {GrpcCore}.IServerStreamWriter<{call.ResponseType}> responseStream, {GrpcCore}.ServerCallContext callContext);";


            case RpcStreamingType.Bidirectional:
                return $"public abstract {TaskString} {call.Name}({GrpcCore}.IAsyncStreamReader<{call.RequestType}> requestStream, {GrpcCore}.IServerStreamWriter<{call.ResponseType}> responseStream, {GrpcCore}.ServerCallContext callContext);";
        }

        throw new InvalidOperationException("Unrecognized streaming type: " + call.StreamingType);
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

    private void DefineInterface(CodeWriter writer)
    {
        writer.AppendSummaryComment(this.service.Documentation);
        writer.AppendLine($"public interface {this.interfaceName}");
        using (writer.WithBlock())
        {
            foreach (var call in this.calls)
            {
                switch (call.StreamingType)
                {
                    case RpcStreamingType.Unary:
                        writer.AppendSummaryComment(call.Documentation);
                        writer.AppendLine($"Task<{call.ResponseType}> {call.Name}({call.RequestType} request, CancellationToken token);");
                        break;

                    case RpcStreamingType.Client:
                        writer.AppendSummaryComment(call.Documentation);
                        writer.AppendLine($"Task<{call.ResponseType}> {call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, CancellationToken token);");
                        break;

                    case RpcStreamingType.Server:
                        writer.AppendSummaryComment(call.Documentation);
                        writer.AppendLine($"Task {call.Name}({call.RequestType} request, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token);");
                        break;

                    case RpcStreamingType.Bidirectional:
                        writer.AppendSummaryComment(call.Documentation);
                        writer.AppendLine($"Task {call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token);");
                        break;
                }
            }
        }
    }

    private void DefineClientClass(
        CodeWriter writer,
        Dictionary<string, string> methodMapping)
    {
        string clientClassName = $"{this.Name}Client";
        string interfaceDeclaration = string.Empty;
        if (this.Attributes.RpcInterface == true)
        {
            interfaceDeclaration = $", {this.interfaceName}";
        }

        writer.AppendSummaryComment(this.service.Documentation);
        writer.AppendLine($"public partial class {clientClassName} : {GrpcCore}.ClientBase<{clientClassName}>{interfaceDeclaration}");
        using (writer.WithBlock())
        {
            this.DefineClientConstructors(writer, clientClassName);

            foreach (var call in this.calls)
            {
                this.WriteClientMethod(writer, call, methodMapping);
            }

            if (this.Attributes.RpcInterface == true)
            {
                foreach (var item in this.calls)
                {
                    switch (item.StreamingType)
                    {
                        case RpcStreamingType.Unary:
                            GenerateUnaryInterfaceImpl(item);
                            break;

                        case RpcStreamingType.Client:
                            GenerateClientStreamingInterfaceImpl(item);
                            break;

                        case RpcStreamingType.Server:
                            GenerateServerStreamingImpl(item);
                            break;

                        case RpcStreamingType.Bidirectional:
                            GenerateBidirectionalStreamingImpl(item);
                            break;
                    }
                }
            }
        }

        void ReadFromRequestChannelIntoRequestStream(string cancellationTokenName)
        {
            writer.AppendLine("try");
            using (writer.WithBlock())
            {
                writer.AppendLine($"while (await requestChannel.WaitToReadAsync({cancellationTokenName}))");
                using (writer.WithBlock())
                {
                    writer.AppendLine("while (requestChannel.TryRead(out var item))");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine($"await call.RequestStream.WriteAsync(item);");
                    }
                }
            }
            writer.AppendLine("finally");
            using (writer.WithBlock())
            {
                writer.AppendLine("await call.RequestStream.CompleteAsync();");
            }
        }

        void ReadFromResponseStreamIntoResponseChannel(string cancellationTokenName)
        {
            writer.AppendLine("try");
            using (writer.WithBlock())
            {
                writer.AppendLine($"while (await call.ResponseStream.MoveNext({cancellationTokenName}))");
                using (writer.WithBlock())
                {
                    writer.AppendLine($"await responseChannel.WriteAsync(call.ResponseStream.Current, {cancellationTokenName});");
                }

                writer.AppendLine("responseChannel.Complete();");
            }
            writer.AppendLine("catch (Exception ex)");
            using (writer.WithBlock())
            {
                writer.AppendLine("responseChannel.TryComplete(ex);");
                writer.AppendLine("throw;");
            }
        }

        void GenerateUnaryInterfaceImpl(RpcCallSchemaModel call)
        {
            writer.AppendLine($"async Task<{call.ResponseType}> {this.interfaceName}.{call.Name}({call.RequestType} request, CancellationToken token)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"return await this.{call.Name}(request, cancellationToken: token).ResponseAsync;");
            }
        }

        void GenerateClientStreamingInterfaceImpl(RpcCallSchemaModel call)
        {
            writer.AppendLine($"async Task<{call.ResponseType}> {this.interfaceName}.{call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, CancellationToken token)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"var call = this.{call.Name}(cancellationToken: token);");
                ReadFromRequestChannelIntoRequestStream("token");
                writer.AppendLine($"return await call.ResponseAsync;");
            }
        }

        void GenerateServerStreamingImpl(RpcCallSchemaModel call)
        {
            writer.AppendLine($"async Task {this.interfaceName}.{call.Name}({call.RequestType} request, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"var call = this.{call.Name}(request, cancellationToken: token);");
                ReadFromResponseStreamIntoResponseChannel("token");
            }
        }

        void GenerateBidirectionalStreamingImpl(RpcCallSchemaModel call)
        {
            writer.AppendLine($"async Task {this.interfaceName}.{call.Name}({Channels}.ChannelReader<{call.RequestType}> requestChannel, {Channels}.ChannelWriter<{call.ResponseType}> responseChannel, CancellationToken token)");
            using (writer.WithBlock())
            {
                writer.AppendLine($"using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))");
                using (writer.WithBlock())
                {
                    writer.AppendLine("var tasks = new List<Task>();");
                    writer.AppendLine($"var call = this.{call.Name}(cancellationToken: cts.Token);");

                    // pulls from request channel and writes into the call.
                    writer.AppendLine("tasks.Add(Task.Run(async () => ");
                    using (writer.WithBlock())
                    {
                        ReadFromRequestChannelIntoRequestStream("cts.Token");
                    }
                    writer.AppendLine("));");

                    // reads from the response stream and pushes to the channel.
                    writer.AppendLine("tasks.Add(Task.Run(async () => ");
                    using (writer.WithBlock())
                    {
                        ReadFromResponseStreamIntoResponseChannel("cts.Token");
                    }
                    writer.AppendLine("));");

                    writer.AppendLine("try");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("while (tasks.Count > 0)");
                        using (writer.WithBlock())
                        {
                            writer.AppendLine("Task completedTask = await Task.WhenAny(tasks);");
                            writer.AppendLine("tasks.Remove(completedTask);");
                            writer.AppendLine("await completedTask;");
                        }
                    }
                    writer.AppendLine("finally");
                    using (writer.WithBlock())
                    {
                        writer.AppendLine("cts.Cancel();");
                    }
                }
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

    private void WriteClientMethod(CodeWriter writer, RpcCallSchemaModel call, Dictionary<string, string> methodMap)
    {
        switch (call.StreamingType)
        {
            case RpcStreamingType.Unary:
                this.WriteRequestParameterMethod(writer, "AsyncUnaryCall", call, methodMap);
                break;

            case RpcStreamingType.Client:
                this.WriteNoRequestParameterMethod(writer, "AsyncClientStreamingCall", call, methodMap);
                break;

            case RpcStreamingType.Server:
                this.WriteRequestParameterMethod(writer, "AsyncServerStreamingCall", call, methodMap);
                break;

            case RpcStreamingType.Bidirectional:
                this.WriteNoRequestParameterMethod(writer, "AsyncDuplexStreamingCall", call, methodMap);
                break;
        }
    }

    private void WriteRequestParameterMethod(
       CodeWriter writer,
       string returnType,
       RpcCallSchemaModel call,
       Dictionary<string, string> methodMap)
    {
        writer.AppendSummaryComment(call.Documentation);
        writer.AppendLine($"public virtual {GrpcCore}.{returnType}<{call.ResponseType}> {call.Name}({call.RequestType} request, {GrpcCore}.Metadata? headers = null, System.DateTime? deadline = null, {CancellationToken} cancellationToken = default({CancellationToken}))");
        using (writer.WithBlock())
        {
            writer.AppendLine($"return {call.Name}(request, new {GrpcCore}.CallOptions(headers, deadline, cancellationToken));");
        }

        writer.AppendSummaryComment(call.Documentation);
        writer.AppendLine($"public virtual {GrpcCore}.{returnType}<{call.ResponseType}> {call.Name}({call.RequestType} request, {GrpcCore}.CallOptions options)");
        using (writer.WithBlock())
        {
            writer.AppendLine($"return CallInvoker.{returnType}({methodMap[call.Name]}, null, options, request);");
        }
    }

    private void WriteNoRequestParameterMethod(
        CodeWriter writer,
        string key,
        RpcCallSchemaModel call,
        Dictionary<string, string> methodMap)
    {
        writer.AppendSummaryComment(call.Documentation);
        writer.AppendLine($"public virtual {GrpcCore}.{key}<{call.RequestType}, {call.ResponseType}> {call.Name}({GrpcCore}.Metadata? headers = null, System.DateTime? deadline = null, {CancellationToken} cancellationToken = default({CancellationToken}))");
        using (writer.WithBlock())
        {
            writer.AppendLine($"return {call.Name}(new {GrpcCore}.CallOptions(headers, deadline, cancellationToken));");
        }

        writer.AppendSummaryComment(call.Documentation);
        writer.AppendLine($"public virtual {GrpcCore}.{key}<{call.RequestType}, {call.ResponseType}> {call.Name}({GrpcCore}.CallOptions options)");
        using (writer.WithBlock())
        {
            writer.AppendLine($"return CallInvoker.{key}({methodMap[call.Name]}, null, options);");
        }
    }

    private string GetServerHandlerDelegate(RpcCallSchemaModel call)
    {
        return $"new {this.GetServerHandlerDelegateType(call)}(serviceImpl.{call.Name})";
    }

    private string GetServerHandlerDelegateType(RpcCallSchemaModel call)
    {
        string methodType = GetGrpcMethodType(call.StreamingType);
        return $"{GrpcCore}.{methodType}ServerMethod<{call.RequestType}, {call.ResponseType}>";
    }
}
