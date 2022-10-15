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

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FlatSharp;
using FlatSharp.Internal;
using System;
using System.Collections.Generic;

namespace BenchmarkCore
{
    public class Benchmark
    {
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(1)
                .WithWarmupCount(3)
                .WithIterationCount(5)
                .WithRuntime(CoreRuntime.Core60)
                .WithEnvironmentVariable(new EnvironmentVariable("DOTNET_TieredPGO", "1"));

            var config = DefaultConfig.Instance
                 .AddDiagnoser(MemoryDiagnoser.Default)
                 .AddJob(job);

            BenchmarkRunner.Run(typeof(Benchmark), config);
        }
    }
}
