/*
 * Copyright 2023 James Courtney
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

#if AOT

using Microsoft.Testing.Framework;
using Microsoft.Testing.Platform.Builder;
using System.Threading.Tasks;
using Microsoft.Testing.Extensions;
using Microsoft.Testing.Framework.Configurations;

namespace FlatSharpEndToEndTests;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Beginning AOT test pass...");

        var builder = TestApplication.CreateBuilderAsync(args).GetAwaiter().GetResult();
        // Registers TestFramework, with tree of test nodes 
        // that are generated into your project by source generator.

        builder.AddTestFramework(new SourceGeneratedTestNodesBuilder());
        builder.AddTrxReportProvider();

        var app = builder.BuildAsync().GetAwaiter().GetResult();
        return app.RunAsync().GetAwaiter().GetResult();
    }
}

#endif