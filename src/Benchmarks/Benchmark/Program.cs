/*
 * Copyright 2018 James Courtney
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

namespace Benchmark
{
    using System;
    using System.Collections.Generic;
    using Benchmark.FBBench;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using BenchmarkDotNet.Toolchains;
    using BenchmarkDotNet.Toolchains.NativeAot;

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Summary> summaries = new List<Summary>();

#if AOT
            IToolchain toolChain = NativeAotToolchainBuilder.Create()
                 .UseNuGet()
                 .IlcInstructionSet("base,sse,sse2,sse3,ssse3,sse4.1,sse4.2,avx,avx2,aes,bmi,bmi2,fma,lzcnt,pclmul,popcnt")
                 .TargetFrameworkMoniker("net8.0")
                 .ToToolchain();
#endif


            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(7)
                .WithWarmupCount(3)
                .WithIterationCount(5)
#if AOT
                .WithToolchain(toolChain)
                .WithRuntime(NativeAotRuntime.Net80)
                .WithArguments(new[] { new MsBuildArgument("/p:BuildAot=true") })
#else
                .WithRuntime(CoreRuntime.Core80)
#endif
                ;

            // job = job.WithEnvironmentVariable(new EnvironmentVariable("DOTNET_TieredPGO", "0"));

            var config = DefaultConfig.Instance
                 .AddColumn(new[] { StatisticColumn.P25, StatisticColumn.P95 })
                 .AddDiagnoser(MemoryDiagnoser.Default)
                 .AddJob(job);

            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.FBSerializeBench), config));
            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.FBDeserializeBench), config));

#if RUN_COMPARISON_BENCHMARKS
            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.OthersDeserializeBench), config));
#endif

            foreach (var item in summaries)
            {
                MarkdownExporter.Console.ExportToLog(item, new ConsoleLogger());
                MarkdownExporter.GitHub.ExportToFiles(item, new ConsoleLogger());
            }
        }
    }
}
