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
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Summary> summaries = new List<Summary>();

            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(7)
                .WithWarmupCount(5)
                .WithIterationCount(7)
                .WithRuntime(CoreRuntime.Core50);

            var config = DefaultConfig.Instance
                 .AddColumn(new[] { StatisticColumn.P25, StatisticColumn.P50, StatisticColumn.P67, StatisticColumn.P80, StatisticColumn.P90, StatisticColumn.P95 })
                 .AddJob(job);

            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.FBSerializeBench), config));
            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.FBDeserializeBench), config));
            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.OthersDeserializeBench), config));

#if !NO_SHARED_STRINGS
            summaries.Add(BenchmarkRunner.Run(typeof(FBBench.FBSharedStringBench), config));
#endif
#if CURRENT_VERSION_ONLY
            summaries.Add(BenchmarkRunner.Run(typeof(SerializationContextBenchmark), config));
#endif

            foreach (var item in summaries)
            {
                MarkdownExporter.Console.ExportToLog(item, new ConsoleLogger());
                MarkdownExporter.GitHub.ExportToFiles(item, new ConsoleLogger());
            }
        }
    }
}
