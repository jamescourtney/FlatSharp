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
        public void BasicEnumTest_Byte()
        {
            string schema = $@"

namespace Ns;

table Request (PrecompiledSerializer) {{ Data:string; }}
table Response (PrecompiledSerializer) {{ Data:string; }}

rpc_service MyService {{
    SayHello(Request):Response;
}}
";

            string cSharp = FlatSharpCompiler.CreateCSharp(schema, string.Empty);
        }
    }
}
