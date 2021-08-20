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
    using Xunit;

    
    public class RpcParsingTests
    {
        [Fact]
        public void BasicEnumTest_Byte()
        {
            string schema = @"
namespace Foo.Bar;

table Request { str:string; }
table Response { str:string; }
rpc_service Service {
    HelloUnary(Request):Response (streaming:unary);
    HelloServer(Request):Response (streaming:""server"");
    HelloClient(Request):Response (streaming:client);
    HelloBidi(Request):Response (streaming:""bidi"");
    HelloUnaryImplicit(Request):Response;
}
";
            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(schema);
            var rpcDef = member.Children["Foo"].Children["Bar"].Children["Service"] as RpcDefinition;
            Assert.NotNull(rpcDef);

            Assert.Equal(5, rpcDef.Methods.Count);
            Assert.Equal(("Request", "Response", RpcStreamingType.Unary), rpcDef.Methods["HelloUnary"]);
            Assert.Equal(("Request", "Response", RpcStreamingType.Unary), rpcDef.Methods["HelloUnaryImplicit"]);
            Assert.Equal(("Request", "Response", RpcStreamingType.Server), rpcDef.Methods["HelloServer"]);
            Assert.Equal(("Request", "Response", RpcStreamingType.Client), rpcDef.Methods["HelloClient"]); 
            Assert.Equal(("Request", "Response", RpcStreamingType.Bidirectional), rpcDef.Methods["HelloBidi"]);
        }
    }
}
