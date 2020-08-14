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

namespace BenchmarkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Benchmark.FBBench;
    using BenchmarkDotNet.Attributes;
    using FlatSharp;
    using FlatSharp.Attributes;

    [MediumRunJob]
    public class Program
    {
        const int GuidCount = 100;

        private static readonly byte[] buffer = new byte[10 * 1024 * 1024];
        private static SimpleVector<string> nonSharedVector;
        private static SimpleVector<SharedString> sharedVector;

        private static SpanWriter sharedWriter;

        public static void Main(string[] args)
        {
            FBSharedStringBench bench = new FBSharedStringBench();
            bench.CacheSize = 53;
            bench.VectorLength = 1000;
            bench.Setup();

            while (true)
            {
                bench.Parse_GuassianSharedStringVector_WithSharedStringWithCache_NonThreadSafe();
            }
        }

        [Params(true, false)]
        public bool RandomDistribution { get; set; }

        [Params(1, 10, 50, 100, 500)]
        public int LruLookupSize { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            sharedWriter = new SpanWriter();

            Random rng = new Random();
            var guids = Enumerable.Range(0, GuidCount).Select(i => Guid.NewGuid().ToString()).ToList();

            nonSharedVector = new SimpleVector<string>
            {
                Items = Enumerable.Range(0, 1000).Select(i => guids[rng.Next(0, guids.Count)]).ToList()
            };

            if (!this.RandomDistribution)
            {
                nonSharedVector.Items = Enumerable.Range(0, 1000).Select(i => guids[NextGaussianInt(rng, 0, guids.Count - 1)]).ToList();
            }

            sharedVector = new SimpleVector<SharedString> { Items = nonSharedVector.Items.Select(SharedString.Create).ToList() };

            Console.WriteLine($"{nameof(SharedStringLRU)} = {this.SharedStringLRU()}");
            Console.WriteLine($"{nameof(NonSharedString)} = {this.NonSharedString()}");
        }

        [Benchmark]
        public int SharedStringLRU()
        {
            return FlatBufferSerializer.Default.Serialize(sharedVector, buffer, sharedWriter);
        }

        [Benchmark]
        public int NonSharedString()
        {
            return FlatBufferSerializer.Default.Serialize(nonSharedVector, buffer, SpanWriter.Instance);
        }

        [FlatBufferTable]
        public class SimpleVector<T>
        {
            [FlatBufferItem(0)]
            public virtual IList<T> Items { get; set; }
        }

        [FlatBufferTable]
        public class SortedVector<T>
        {
            [FlatBufferItem(0, SortedVector = true)]
            public virtual IList<SortedVectorItem<T>> Item { get; set; }
        }

        [FlatBufferTable]
        public class UnsortedVector<T>
        {
            [FlatBufferItem(0, SortedVector = false)]
            public virtual IList<SortedVectorItem<T>> Item { get; set; }
        }

        [FlatBufferTable]
        public class SortedVectorItem<T>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual T Item { get; set; }
        }

        public static int NextGaussianInt(Random r, int min, int max)
        {
            double random = NextGaussian(r, (max - min) / 2.0, (max - min) / 6.0); // from -2 to 2.
            return Math.Max(min, Math.Min(max, (int)random));
        }

        public static double NextGaussian(Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}
