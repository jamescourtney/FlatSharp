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
    using FlatSharp;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GrpcTests
    {
        [TestMethod]
        public void RouteGuideTest()
        {
            string schema = $@"
namespace routeguide;

    rpc_service RouteGuide
    {{
        GetFeature(Point):Feature;
        ListFeatures(Rectangle):Feature (streaming:server);
        RecordRoute(Point):RouteSummary (streaming:client);
        RouteChat(RouteNote):RouteNote (streaming:duplex);
    }}

    table Point ({MetdataKeys.SerializerKind}:lazy)
    {{
        latitude:int32;
        longitude:int32;
    }}

    table Rectangle ({MetdataKeys.PrecompiledSerializerLegacy}:lazy)
    {{
        lo:Point;
        hi:Point;
    }}

    table Feature ({MetdataKeys.SerializerKind}:lazy)
    {{
        // The name of the feature.
        name:string;

        // The point where the feature is detected.
        location:Point;
    }}

    table RouteNote ({MetdataKeys.SerializerKind}:lazy)
    {{
        location:Point;
        message:string;
    }}

    table RouteSummary ({MetdataKeys.SerializerKind}:greedymutable)
    {{
        point_count:int;
        feature_count:int;
        distance:int;
        elapsed_time:int;
    }}";
            Assembly compiled = FlatSharpCompiler.CompileAndLoadAssembly(
                schema,
                new(),
                additionalReferences: new[] { typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly });

            var rpcType = compiled.GetType("routeguide.RouteGuide");
            Assert.IsNotNull(rpcType);

            var serverBaseClass = rpcType.GetNestedType("RouteGuideServerBase");
            Assert.IsTrue(serverBaseClass.IsAbstract);
            var attribute = serverBaseClass.GetCustomAttribute<Grpc.Core.BindServiceMethodAttribute>();
            Assert.IsNotNull(attribute);

            var bindServiceMethod = rpcType.GetMethod("BindService", new [] {serverBaseClass});
            Assert.IsNotNull(bindServiceMethod);
            Assert.AreEqual(bindServiceMethod.Name, attribute.BindMethodName);
            Assert.AreEqual(rpcType, attribute.BindType);
            Assert.IsTrue(bindServiceMethod.IsPublic);
            Assert.IsTrue(bindServiceMethod.IsStatic);

            var bindServiceOverload =
                rpcType.GetMethod("BindService", new[] {typeof(Grpc.Core.ServiceBinderBase), serverBaseClass});
            Assert.IsNotNull(bindServiceOverload);
            Assert.AreEqual(bindServiceOverload.Name, attribute.BindMethodName);
            Assert.AreEqual(rpcType, attribute.BindType);
            Assert.IsTrue(bindServiceOverload.IsPublic);
            Assert.IsTrue(bindServiceOverload.IsStatic);
            Assert.AreEqual(bindServiceOverload.ReturnType, typeof(void));

            var clientClass = rpcType.GetNestedType("RouteGuideClient");
            Assert.IsFalse(clientClass.IsAbstract);
            Assert.AreEqual(clientClass.BaseType.GetGenericTypeDefinition(), typeof(Grpc.Core.ClientBase<>));
        }

        [TestMethod]
        public void RpcUnknownType()
        {
            string schema = $@"
namespace RpcUnknownType;

    rpc_service RouteGuide
    {{
        GetFeature(NotExisting):Point;
    }}

    table Point ({MetdataKeys.SerializerKind}:lazy)
    {{
        latitude:int32;
        longitude:int32;
    }}";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema, new()));
        }

        [TestMethod]
        public void RpcUnknownStreamingType()
        {
            string schema = $@"
namespace RpcUnknownStreamingType;

    rpc_service RouteGuide
    {{
        GetFeature(Point):Point (streaming:banana);
    }}

    table Point ({MetdataKeys.SerializerKind}:lazy)
    {{
        latitude:int32;
        longitude:int32;
    }}";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema, new()));
        }

        [TestMethod]
        public void RpcReturnsAStruct()
        {
            string schema = $@"
namespace RpcReturnsAStruct;

    rpc_service RouteGuide
    {{
        GetFeature(Point):Point;
    }}

    struct Point ({MetdataKeys.SerializerKind}:lazy)
    {{
        latitude:int32;
        longitude:int32;
    }}";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema, new()));
        }

        [TestMethod]
        public void NoPrecompiledSerializer()
        {
            string schema = $@"
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

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema, new()));
        }
    }
}
