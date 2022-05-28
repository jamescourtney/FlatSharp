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
using System;
using System.Collections.Generic;

namespace BenchmarkCore
{
    public class Benchmark
    { 
        [Params(
            FlatBufferDeserializationOption.Lazy,
            FlatBufferDeserializationOption.GreedyMutable
        )]
        public FlatBufferDeserializationOption Option { get; set; }

        [Params(5000)]
        public int Length { get; set; }

        [Params(1)]
        public int TravCount { get; set; }

        private Wrapper tableToWrite;

        private byte[] tableToRead;

        private List<string> keys;

        private string EmptyGuid = Guid.Empty.ToString();

        [GlobalSetup]
        public void Setup()
        {
            this.tableToWrite = new()
            {
                Vector = new List<Table>(),
            };

            this.keys = new();
            Random r = new();

            for (int i = 0; i < this.Length; ++i)
            {
                string key = Guid.NewGuid().ToString();

                this.keys.Add(key);
                this.tableToWrite.Vector.Add(new()
                {
                    A = r.Next() % 2 == 0 ? "A" : null,
                    B = r.Next() % 2 == 0 ? "B" : null,
                    C = r.Next() % 2 == 0 ? "C" : null,
                    D = r.Next() % 2 == 0 ? "D" : null,
                });
            }

            int bytesNeeded = Wrapper.Serializer.GetMaxSize(this.tableToWrite);
            this.tableToRead = new byte[bytesNeeded];
            Wrapper.Serializer.Write(this.tableToRead, this.tableToWrite);
        }

        [Benchmark]
        public void Serialize()
        {
            Wrapper.Serializer.Write(this.tableToRead, this.tableToWrite);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //    Benchmark b = new();
            //    b.Length = 5000;
            //    b.Setup();

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
