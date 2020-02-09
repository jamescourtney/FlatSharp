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
            string schema = @"
namespace routeguide;

    rpc_service RouteGuide
    {
        GetFeature(Point):Feature;
        ListFeatures(Rectangle):Feature (streaming:server);
        RecordRoute(Point):RouteSummary (streaming:client);
        RouteChat(RouteNote):RouteNote (streaming:duplex);
    }

    table Point (PrecompiledSerializer:lazy)
    {
        latitude:int32;
        longitude:int32;
    }

    table Rectangle (PrecompiledSerializer:lazy)
    {
        lo:Point;
        hi:Point;
    }

    table Feature (PrecompiledSerializer:lazy)
    {
        // The name of the feature.
        name:string;

        // The point where the feature is detected.
        location:Point;
    }

    table RouteNote (PrecompiledSerializer:lazy)
    {
        location:Point;
        message:string;
    }

    table RouteSummary (PrecompiledSerializer:greedymutable)
    {
        point_count:int;
        feature_count:int;
        distance:int;
        elapsed_time:int;
    }";
            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(schema);
            Assembly compiled = FlatSharpCompiler.CompileAndLoadAssembly(
                schema, 
                additionalReferences: new[] { typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly });

            var rpcType = compiled.GetType("routeguide.RouteGuide");
            Assert.IsNotNull(rpcType);

            var bindServiceMethod = rpcType.GetMethod("BindService", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(bindServiceMethod);

            var serverBaseClass = rpcType.GetNestedType("RouteGuideServerBase");
            Assert.IsTrue(serverBaseClass.IsAbstract);
            var attribute = serverBaseClass.GetCustomAttribute<Grpc.Core.BindServiceMethodAttribute>();
            Assert.IsNotNull(attribute);
            Assert.AreEqual(bindServiceMethod.Name, attribute.BindMethodName);
            Assert.AreEqual(rpcType, attribute.BindType);

            var clientClass = rpcType.GetNestedType("RouteGuideClient");
            Assert.IsFalse(clientClass.IsAbstract);
            Assert.AreEqual(clientClass.BaseType.GetGenericTypeDefinition(), typeof(Grpc.Core.ClientBase<>));
        }

        [TestMethod]
        public void RpcUnknownType()
        {
            string schema = @"
namespace RpcUnknownType;

    rpc_service RouteGuide
    {
        GetFeature(NotExisting):Point;
    }

    table Point (PrecompiledSerializer:lazy)
    {
        latitude:int32;
        longitude:int32;
    }";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema));
        }

        [TestMethod]
        public void RpcUnknownStreamingType()
        {
            string schema = @"
namespace RpcUnknownStreamingType;

    rpc_service RouteGuide
    {
        GetFeature(Point):Point (streaming:banana);
    }

    table Point (PrecompiledSerializer:lazy)
    {
        latitude:int32;
        longitude:int32;
    }";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema));
        }

        [TestMethod]
        public void RpcReturnsAStruct()
        {
            string schema = @"
namespace RpcReturnsAStruct;

    rpc_service RouteGuide
    {
        GetFeature(Point):Point;
    }

    struct Point (PrecompiledSerializer:lazy)
    {
        latitude:int32;
        longitude:int32;
    }";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema));
        }

        [TestMethod]
        public void NoPrecompiledSerializer()
        {
            string schema = @"
namespace NoPrecompiledSerializer;

    rpc_service RouteGuide
    {
        GetFeature(Point):Point;
    }

    table Point
    {
        latitude:int32;
        longitude:int32;
    }";

            Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(schema));
        }
    }
}
