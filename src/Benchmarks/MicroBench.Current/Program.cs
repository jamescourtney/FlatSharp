/*
 * Copyright 2022 James Courtney
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

namespace Microbench
{
    using System;
    using System.Collections.Generic;

    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Summary> summaries = new List<Summary>();

            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(1)
                .WithWarmupCount(2)
                .WithIterationCount(6)
                .WithRuntime(CoreRuntime.Core70);
                //.WithEnvironmentVariable(new EnvironmentVariable("DOTNET_TieredPGO", "1"));

            var config = DefaultConfig.Instance
                 .AddColumn(new[] { StatisticColumn.P25, StatisticColumn.P95 })
                 .AddDiagnoser(MemoryDiagnoser.Default)
                 .AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(
                     maxDepth: 8,
                     printSource: false,
                     exportGithubMarkdown: true,
                     exportHtml: true,
                     exportCombinedDisassemblyReport: true)))
                 //.AddHardwareCounters(HardwareCounter.BranchInstructions, HardwareCounter.BranchMispredictions)
                 .AddJob(job.DontEnforcePowerPlan());

            //summaries.Add(BenchmarkRunner.Run(typeof(SerializeBenchmarks), config));
            summaries.Add(BenchmarkRunner.Run(typeof(ParseBenchmarks), config));
            //summaries.Add(BenchmarkRunner.Run(typeof(SortedVectorBenchmarks), config));
            //summaries.Add(BenchmarkRunner.Run(typeof(VTableBenchmarks), config));

            foreach (var item in summaries)
            {
                MarkdownExporter.Console.ExportToLog(item, new ConsoleLogger());
                MarkdownExporter.GitHub.ExportToFiles(item, new ConsoleLogger());
            }
        }
    }
}
