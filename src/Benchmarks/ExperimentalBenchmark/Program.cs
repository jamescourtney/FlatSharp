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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BenchmarkCore
{
    public class Benchmark
    {
        private readonly byte[] data = new byte[10 * 1024 * 1024];

        private ArrayInputBuffer inputBuffer;

        [Params(
            FlatBufferDeserializationOption.Lazy,
            FlatBufferDeserializationOption.Progressive,
            FlatBufferDeserializationOption.Greedy
        )]
        public FlatBufferDeserializationOption Option { get; set; }

        [Params(1000)]
        public int Length { get; set; }

        [Params(1, 5)]
        public int TravCount { get; set; }

        private FlatBufferSerializer serializer;

        private int[] indexesToAccess;

        [GlobalSetup]
        public void Setup()
        {
            ValueTable t = new ValueTable { Points = new List<Vec3>() };
            for (int i = 0; i < this.Length; ++i)
            {
                t.Points.Add(new Vec3 { X = i, Y = i * 2, Z = i * 3 });
            }

            this.indexesToAccess = new int[this.Length / 10];
            Random r = new Random();
            for (int i = 0; i < this.indexesToAccess.Length; ++i)
            {
                this.indexesToAccess[i] = r.Next(this.indexesToAccess.Length);
            }
             
            this.serializer = new FlatBufferSerializer(this.Option);
            this.serializer.Serialize(t, this.data);
            this.inputBuffer = new ArrayInputBuffer(this.data);
        }

        [Benchmark]
        public int ParseAndTraverse_Full()
        {
            var t = this.serializer.Parse<ValueTable, ArrayInputBuffer>(this.inputBuffer);

            int sum = 0;

            var points = t.Points;
            if (points is null)
            {
                return 0;
            }

            for (int c = 0; c < this.TravCount; ++c)
            {
                int count = points.Count;
                for (int i = 0; i < count; ++i)
                {
                    var point = points[i];
                    sum += (point.X + point.Y + point.Z);
                }
            }

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_Sparse()
        {
            var t = this.serializer.Parse<ValueTable, ArrayInputBuffer>(this.inputBuffer);

            int sum = 0;

            var points = t.Points;
            if (points is null)
            {
                return 0;
            }

            for (int c = 0; c < this.TravCount; ++c)
            {
                foreach (var i in this.indexesToAccess)
                {
                    var point = points[i];
                    sum += (point.X + point.Y + point.Z);
                }
            }

            return sum;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(7)
                .WithWarmupCount(3)
                .WithIterationCount(3)
                .WithRuntime(CoreRuntime.Core50);

            var config = DefaultConfig.Instance
                 .AddDiagnoser(MemoryDiagnoser.Default)
                 .AddJob(job);

            BenchmarkRunner.Run(typeof(Benchmark), config);
        }
    }
}
