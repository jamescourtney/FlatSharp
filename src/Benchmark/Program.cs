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
    using BenchmarkDotNet.Configs;
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

            summaries.Add(BenchmarkRunner.Run<FBBench.FBSerializeBench>());
            summaries.Add(BenchmarkRunner.Run<FBBench.FBDeserializeBench>());
            summaries.Add(BenchmarkRunner.Run<FBBench.OthersDeserializeBench>());

#if !NO_SHARED_STRINGS
            summaries.Add(BenchmarkRunner.Run<FBBench.FBSharedStringBench>());
#endif
#if CURRENT_VERSION_ONLY
            summaries.Add(BenchmarkRunner.Run<SerializationContextBenchmark>());
#endif

            foreach (var item in summaries)
            {
                MarkdownExporter.Console.ExportToLog(item, new ConsoleLogger());
                MarkdownExporter.GitHub.ExportToFiles(item, new ConsoleLogger());
            }
        }
    }
}
