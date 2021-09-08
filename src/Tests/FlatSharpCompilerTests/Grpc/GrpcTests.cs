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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Sources;
    using FlatSharp;
    using FlatSharp.Compiler;
    using Xunit;

    
    public class GrpcTests
    {
        [Fact]
        public void RouteGuideTest()
        {
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace routeguide;

    rpc_service RouteGuide
    {{
        GetFeature(Point):Feature;
        ListFeatures(Rectangle):Feature (streaming:""server"");
        RecordRoute(Point):RouteSummary (streaming:""client"");
        RouteChat(RouteNote):RouteNote (streaming:""duplex"");
    }}

    table Point ({MetadataKeys.SerializerKind}:""lazy"")
    {{
        latitude:int32;
        longitude:int32;
    }}

    table Rectangle ({MetadataKeys.SerializerKind}:""lazy"")
    {{
        lo:Point;
        hi:Point;
    }}

    table Feature ({MetadataKeys.SerializerKind}:""lazy"")
    {{
        // The name of the feature.
        name:string;

        // The point where the feature is detected.
        location:Point;
    }}

    table RouteNote ({MetadataKeys.SerializerKind}:""lazy"")
    {{
        location:Point;
        message:string;
    }}

    table RouteSummary ({MetadataKeys.SerializerKind}:""greedymutable"")
    {{
        point_count:int;
        feature_count:int;
        distance:int;
        elapsed_time:int;
    }}";
            (Assembly compiled, string source) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(
                schema,
                new(),
                additionalReferences: new[] 
                { 
                    typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly, 
                    typeof(ChannelReader<>).Assembly, 
                });

            var rpcType = compiled.GetType("routeguide.RouteGuide");
            Assert.NotNull(rpcType);

            var serverBaseClass = rpcType.GetNestedType("RouteGuideServerBase");
            Assert.True(serverBaseClass.IsAbstract);
            var attribute = serverBaseClass.GetCustomAttribute<Grpc.Core.BindServiceMethodAttribute>();
            Assert.NotNull(attribute);

            var bindServiceMethod = rpcType.GetMethod("BindService", new [] {serverBaseClass});
            Assert.NotNull(bindServiceMethod);
            Assert.Equal(bindServiceMethod.Name, attribute.BindMethodName);
            Assert.Equal(rpcType, attribute.BindType);
            Assert.True(bindServiceMethod.IsPublic);
            Assert.True(bindServiceMethod.IsStatic);

            var bindServiceOverload =
                rpcType.GetMethod("BindService", new[] {typeof(Grpc.Core.ServiceBinderBase), serverBaseClass});
            Assert.NotNull(bindServiceOverload);
            Assert.Equal(bindServiceOverload.Name, attribute.BindMethodName);
            Assert.Equal(rpcType, attribute.BindType);
            Assert.True(bindServiceOverload.IsPublic);
            Assert.True(bindServiceOverload.IsStatic);
            Assert.Equal(typeof(void), bindServiceOverload.ReturnType);

            var clientClass = rpcType.GetNestedType("RouteGuideClient");
            Assert.False(clientClass.IsAbstract);
            Assert.Equal(typeof(Grpc.Core.ClientBase<>), clientClass.BaseType.GetGenericTypeDefinition());
        }

        [Fact]
        public void NoPrecompiledSerializer()
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace NoPrecompiledSerializer;

                rpc_service RouteGuide
                {{
                    GetFeature(Point):Point;
                }}

                table Point
                {{
                    latitude:int32;
                    longitude:int32;
                }}";

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                additionalReferences: new[]
                {
                    typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly,
                    typeof(ChannelReader<>).Assembly,
                }));

            Assert.Contains(
                "RPC call 'NoPrecompiledSerializer.RouteGuide.GetFeature' uses table 'NoPrecompiledSerializer.Point', which does not specify the 'fs_serializer' attribute.",
                ex.Message);
        }

#if NET5_0_OR_GREATER
        [Fact]
        public void RpcInterfaceWithDefaultName()
        {
            this.RpcInterfaceWithNameHelper($"{MetadataKeys.RpcInterface}", "IFoobarService");
        }

        private void RpcInterfaceWithNameHelper(string attribute, string expectedInterfaceName)
        {
            string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace Foobar;
                table Message ({MetadataKeys.SerializerKind}) {{ Value : string; }}
                rpc_service FoobarService ({attribute}) 
                {{ 
                    GetMessage1(Message) : Message;
                    GetMessage2(Message) : Message (streaming:""client"");
                    GetMessage3(Message) : Message (streaming:""server"");
                    GetMessage4(Message) : Message (streaming:""duplex"");
                }}
            ";

            Assembly compiled = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                additionalReferences: new[]
                {
                    typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly,
                    typeof(ChannelReader<>).Assembly,
                });

            var rpcType = compiled.GetType("Foobar.FoobarService+FoobarServiceClient");
            var interfaceType = compiled.GetType($"Foobar.{expectedInterfaceName}");

            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsInterface);

            Assert.NotNull(rpcType);
            Assert.Single(rpcType.GetInterfaces());
            Assert.Equal(interfaceType, rpcType.GetInterfaces()[0]);
        }
#endif
    }
}
