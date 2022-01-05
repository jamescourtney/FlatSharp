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

        [Params(10000)]
        public int Length { get; set; }

        [Params(1)]
        public int TravCount { get; set; }

        private FlatBufferSerializer serializer;

        private Table tableToWrite;

        private byte[] tableToRead;

        private List<string> keys;

        private string EmptyGuid = Guid.Empty.ToString();

        [GlobalSetup]
        public void Setup()
        {
            this.serializer = new FlatBufferSerializer(this.Option);

            this.tableToWrite = new()
            {
                Items = new List<VecTable>()
            };

            this.keys = new();

            for (int i = 0; i < this.Length; ++i)
            {
                string key = Guid.NewGuid().ToString();

                this.keys.Add(key);
                this.tableToWrite.Items.Add(new VecTable
                {
                    Key = key,
                });
            }

            int bytesNeeded = this.serializer.GetMaxSize(this.tableToWrite);
            this.tableToRead = new byte[bytesNeeded];
            this.serializer.Serialize(this.tableToWrite, this.tableToRead);
        }

        [Benchmark]
        public void Serialize()
        {
            this.serializer.Serialize(this.tableToWrite, this.tableToRead);
        }

        [Benchmark]
        public int ParseAndTraverse()
        {
            var t = this.serializer.Parse<Table>(this.tableToRead);
            var items = t.Items;
            var keys = this.keys;

            int found = 0;
            foreach (var key in keys)
            {
                var findResult = items.BinarySearchByFlatBufferKey(key);
                if (findResult != null)
                {
                    found++;
                }
            }

            return found;
        }
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
